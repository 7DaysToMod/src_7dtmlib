using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SDTM
{
	public class PermissionList
	{
		private Dictionary<string, bool> _permissions = new Dictionary<string, bool>();

		public PermissionList ()
		{
			
		}

		public bool Set(string permission, bool isAllowed){
			if (_permissions.ContainsKey (permission)) {
				bool allowed = _permissions [permission];
				if (!allowed) {
					_permissions [permission] = isAllowed;
				}
				return true;
			} else {
				_permissions.Add (permission, isAllowed);
				return true;
			}
		}

		public bool Get(string permission){
			if (_permissions.ContainsKey (permission)) {
				return _permissions[permission];
			}

			bool allowed = false;

			string[] nodePath = permission.Split (new string[]{ "." }, StringSplitOptions.RemoveEmptyEntries);

			if (_permissions.ContainsKey ("*")) {
				allowed = _permissions ["*"];
			}

			string currentPath = nodePath[0];
			if (_permissions.ContainsKey (currentPath + ".*")) {
				allowed = _permissions [currentPath + ".*"];
			}

			for (int i = 1; i < nodePath.Length; i++) {
				currentPath += "." + nodePath [i];
				if (_permissions.ContainsKey (currentPath + ".*")) {
					allowed = _permissions [currentPath + ".*"];
				}
			}

			return allowed;
		}

		public bool Exists(string permission){
			if (_permissions.ContainsKey (permission)) {
				return true;
			}

			string[] nodePath = permission.Split (new string[]{ "." }, StringSplitOptions.RemoveEmptyEntries);

			string currentPath = nodePath[0];
			if (_permissions.ContainsKey (currentPath + ".*")) {
				return true;
			}

			bool found = false;

			for (int i = 1; i < nodePath.Length; i++) {
				currentPath += "." + nodePath [i];
				if (_permissions.ContainsKey (currentPath + ".*")) {
					found = true;
					break;
				}
			}

			return found;
		}

		public Dictionary<string, bool> GetAll(){
			return _permissions;
		}

		public bool Remove(string permission){
			if (_permissions.ContainsKey (permission)) {
				return _permissions.Remove (permission);
			}

			return false;
		}

		public bool RemoveAll(){
			_permissions.Clear ();
			return true;
		}

		public void Read(XmlNode permissionsNode){
			foreach(XmlNode permNode in permissionsNode.ChildNodes){
				if (permNode.Name.ToLower () == "node") {
					string permName = permNode.Attributes.GetNamedItem ("name").Value;
					string permValue = permNode.InnerText;
					Log.Out ("Loaded Permission Node: " + permName);
					bool allowed = false;
					if (permValue.ToLower () == "true") {
						allowed = true;
					}

					this.Set (permName, allowed);
				}
			}
		}

		public void Write(StreamWriter sw){
			sw.WriteLine ("<permissions>");
			foreach (KeyValuePair<string, bool> kvp in _permissions) {
				sw.WriteLine("<node name=\""+kvp.Key+"\">"+(kvp.Value==true?"true":"false")+"</node>");
			}
			sw.WriteLine ("</permissions>");
		}
	}
}

