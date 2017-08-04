using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SDTM
{
	public class Permissions
	{
		public Dictionary<string, PermissionGroup> Groups = new Dictionary<string, PermissionGroup>();
		public Dictionary<string, PermissionUser> Users = new Dictionary<string, PermissionUser>();

		public string PublicGroupName = "";
		public string DefaultGroupName = "";
		public string DefaultRegularGroupName = "";

		private List<string> _permissionNodes = new List<string>();

		public List<string> _superGroup = new List<string> ();

		public string[] PermissionNodes{
			get{
				_permissionNodes.Sort ();
				return _permissionNodes.ToArray ();
			}	
		}

		public Permissions ()
		{
			
		}

		public bool Load(){
			string configPath = SDTM.API.configDataPath;

			if (FileUtils.ensureDirectoryExists (configPath)) {
				configPath = configPath + "/experm.xml";

				if (!File.Exists (configPath)) {
					return false;
				}

				XmlDocument xmlDoc = new XmlDocument();
				try{
					xmlDoc.Load(configPath);
				}
				catch (XmlException e){
					Log.Error(string.Format("[ExPerm] Could not Load Config: {0}", e.Message));
					return false;
				}

				XmlNode rootNode = xmlDoc.DocumentElement;
				XmlNode defaultGroupAttr = rootNode.Attributes.GetNamedItem ("defaultgroup");
				if (defaultGroupAttr != null) {
					this.DefaultGroupName = defaultGroupAttr.Value;
				}
				foreach (XmlNode configNode in rootNode.ChildNodes) {
					switch (configNode.Name.ToLower ()) {
					case "groups":
						foreach (XmlNode groupNode in configNode.ChildNodes) {
							PermissionGroup g = new PermissionGroup (groupNode);
							Log.Out ("Added Group: " + g.Name);
							this.Groups.Add (g.Name, g);
						}
						break;
					case "users":
						foreach (XmlNode userNode in configNode.ChildNodes) {
							PermissionUser u = new PermissionUser (userNode);
							this.Users.Add (u.SteamId, u);
						}
						break;
					}
				}

				return true;
			}

			return false;
		}

		public bool Save(){
			string configPath = SDTM.API.configDataPath;
			if (FileUtils.ensureDirectoryExists (configPath)) {
				configPath = configPath + "/experm.xml";
				StreamWriter sw = new StreamWriter (configPath);

				sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				sw.WriteLine("<Config defaultgroup=\""+this.DefaultGroupName+"\">");
				sw.WriteLine ("<groups>");
				foreach (KeyValuePair<string, PermissionGroup> kvp in Groups) {
					if (kvp.Value.Name != "_super") {
						kvp.Value.Write (sw);
					}
				}
				sw.WriteLine ("</groups>");
				sw.WriteLine ("<users>");
				foreach (KeyValuePair<string, PermissionUser> kvp in Users) {
					kvp.Value.Write (sw);
				}
				sw.WriteLine ("</users>");
				sw.WriteLine("</Config>");
				sw.Flush();
				sw.Close();

				return true;
			}

			return false;
		}

		public bool isAllowed(ClientInfo _cInfo, string _permissionNode){
			if (_cInfo == null) {
				return true;
			}
			string sid = PlayerUtils.GetSteamID (_cInfo.entityId.ToString());
			return isAllowed(sid, _permissionNode);
		}	

		public bool isAllowed(string _steamIdOrUserName, string _permissionNode){
			bool allowed = false;

			string steamId = PlayerUtils.GetSteamID (_steamIdOrUserName);

			if (_superGroup.Contains (steamId)) {
				return true;
			}

			if (Users.ContainsKey (steamId)) {
				PermissionUser user = Users [steamId];
				string groupName = user.Group;
				if (groupName != null && groupName != "") {
					if (Groups.ContainsKey (groupName)) {
						allowed = Groups [groupName].Permissions.Get (_permissionNode);
					}
				}

				if (Users [steamId].Permissions.Exists (_permissionNode)) {
					allowed = Users [steamId].Permissions.Get (_permissionNode);
				}
			} else {//default group
				if (Groups.ContainsKey (this.DefaultGroupName)) {
					allowed = Groups [this.DefaultGroupName].Permissions.Get (_permissionNode);
				}
			}

			return allowed;
		}

		public bool RegisterPermission(string permission){
			if (!_permissionNodes.Contains (permission)) {
				_permissionNodes.Add (permission);
			}

			return true;
		}

		public bool AddGroup(string groupName){
			if (!Groups.ContainsKey (groupName)) {
				PermissionGroup grp = new PermissionGroup (groupName);
				Groups.Add (groupName, grp);
			}

			return this.Save();
		}

		public bool SetGroupPermission(string groupName, string permission, bool allowed){
			if(!this.Groups.ContainsKey(groupName)){
				return false;
			}

			PermissionGroup grp = this.Groups[groupName];
			if (grp.SetPermission (permission, allowed)) {
				return this.Save ();
			}

			return false;
		}

		public bool ClearGroupPermission(string groupName, string permission){
			if(!this.Groups.ContainsKey(groupName)){
				return false;
			}
			if (this.Groups.Remove (groupName)) {
				return this.Save ();
			}

			return false;
		}

		public bool toggleSuper(string steamIdOrUserName){
			string steamId = PlayerUtils.GetSteamID (steamIdOrUserName);
			if (_superGroup.Contains (steamId)) {
				_superGroup.Remove (steamId);
				return false;
			} else {
				_superGroup.Add (steamId);
				return true;
			}
		}

		public bool SetUserGroup(string steamIdOrUserName, string groupName){

			if (!this.Groups.ContainsKey (groupName)) {
				return false;
			}

			string steamId = PlayerUtils.GetSteamID (steamIdOrUserName);
			PermissionUser user;

			if (!this.Users.ContainsKey (steamId)) {
				user = new PermissionUser (steamId);
				user.SteamId = steamId;
				user.Group = groupName;
				this.Users.Add (steamId, user);
				GameManager.Instance.adminTools.AddAdmin (steamId, this.Groups [groupName].AdminLevel);
			} else {
				user = this.Users [steamId];
				user.Group = groupName;
				GameManager.Instance.adminTools.AddAdmin (steamId, this.Groups [groupName].AdminLevel);
				this.Users [steamId] = user;
			}

			return false;
		}

		public bool SetUserPermission(string steamIdOruserName, string permNode, bool allowed){
			string steamId = PlayerUtils.GetSteamID (steamIdOruserName);
			if (SDTM.API.Permissions.Users.ContainsKey (steamId)) {

				PermissionUser setPermUser = SDTM.API.Permissions.Users [steamId];

				if (setPermUser.Permissions.Set (permNode, allowed)) {
					SDTM.API.Permissions.Save ();
					return true;
				} else {
					return false;
				}
			} else {
				PermissionUser newUser = new PermissionUser (steamId);
				newUser.Permissions.Set (permNode, allowed);
				SDTM.API.Permissions.Users.Add (steamId, newUser);
				SDTM.API.Permissions.Save ();
				return true;
			}
		}

		public bool ClearUserPermission(string steamIdOrUserName, string permNode){
			string steamId = PlayerUtils.GetSteamID (steamIdOrUserName);
			if (SDTM.API.Permissions.Users.ContainsKey (steamId)) {

				PermissionUser setPermUser = SDTM.API.Permissions.Users [steamId];

				if (setPermUser.Permissions.Remove (permNode)) {
					SDTM.API.Permissions.Save ();
					return true;
				}
			}

			return false;
		}
	}
}

