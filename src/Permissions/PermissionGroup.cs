using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SDTM
{
	public class PermissionGroup
	{
		public string Name = "";
		public int AdminLevel = 1000;

		public PermissionList Permissions = new PermissionList();

		public PermissionGroup (XmlNode groupNode)
		{
			this.Read (groupNode);
		}

		public PermissionGroup (string _GroupName)
		{
			Name = _GroupName;
		}

		public bool SetPermission(string permission, bool allowed){
			return this.Permissions.Set (permission, allowed);
		}

		public bool RemovePermission(string permission){
			return this.Permissions.Remove (permission);
		}

		public bool Read(XmlNode groupNode){
			if (groupNode.Attributes.Count > 0) {
				Name = groupNode.Attributes.GetNamedItem ("name").Value;
			}

			XmlNode adminLevelAttr = groupNode.Attributes.GetNamedItem ("admin_level");

			if (adminLevelAttr != null) {
				string adminLevelStr = adminLevelAttr.Value;
				Log.Out ("******" + adminLevelStr + "********");
				int.TryParse (adminLevelStr, out AdminLevel);
			}

			if (Name == "") {
				Log.Out ("[ExPerm] Group Name cannot be empty");
				return false;
			}

			foreach (XmlNode childNode in groupNode.ChildNodes) {
				if (childNode.Name == "permissions") {
					this.Permissions.Read (childNode);
				}
			}

			return true;
		}

		public void Write(StreamWriter sw){
			sw.WriteLine ("<group name=\"" + Name + "\" admin_level=\""+AdminLevel.ToString()+"\">");
			this.Permissions.Write (sw);
			sw.WriteLine ("</group>");
		}
	}
}

