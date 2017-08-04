using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SDTM
{
	public class PermissionUser
	{
		public string SteamId = "";
		public string Group = "";
		public string DisplayName = "";

		public PermissionList Permissions = new PermissionList();

		public PermissionUser (XmlNode userNode)
		{
			this.Read (userNode);
		}

		public PermissionUser (string SteamIdOrUserName)
		{
			ClientInfo ci = PlayerUtils.GetClientInfo (SteamIdOrUserName);
			if (ci == null) {
				return;
			}

			SteamId = ci.playerId;
			DisplayName = ci.playerName;
		}

		public bool Read(XmlNode userNode){
			if (userNode.Attributes.Count > 0) {
				SteamId = userNode.Attributes.GetNamedItem ("steamid").Value;
				Group = userNode.Attributes.GetNamedItem ("group").Value;
				DisplayName = userNode.Attributes.GetNamedItem ("display_name").Value;
			}

			if (SteamId == "") {
				Log.Out ("[ExPerm] User Steam ID cannot be empty");
				return false;
			}

			foreach (XmlNode childNode in userNode.ChildNodes) {
				if (childNode.Name == "permissions") {
					this.Permissions.Read (childNode);
				}
			}

			return true;
		}

		public void Write(StreamWriter sw){
			sw.WriteLine ("<user steamid=\"" + SteamId + "\" display_name=\""+DisplayName+"\" group=\""+Group+"\">");
			this.Permissions.Write (sw);
			sw.WriteLine ("</user>");
		}
	}
}

