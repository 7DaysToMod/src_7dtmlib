using System;
using System.Collections.Generic;

namespace SDTM
{
	public class ConsoleCmdExPerm:ConsoleCmdAbstract
	{
		public override string GetDescription ()
		{
			return "Extended Permission Management";
		}

		public override string GetHelp ()
		{
			return "Common Commands:\n" +
				"/experm, /experm help - Get additional information about ExPerm Commands\n" +
				"/experm nodes - List all Permission Nodes registered with the ExPerm system\n" +
				"/experm user - List all Users registered with the ExPerm system\n" +
				"/experm user info <SteamIdOrUserName> - Show ExPerm information for the user <SteamIdOrUserName>\n" +
				"/experm user group <SteamIdOrUserName> <group name> - Set the group for <SteamIdOrUserName> to <group name>\n" +
				"/experm user setperm <SteamIdOrUserName> <permname> <allowed> - Set the permission <permname> for <SteamIdOrUserName> to the true/false value of <allowed>\n" +
				"/experm group - List all groups registered with the ExPerm system\n" +
				"/experm group info <group name> - Shows ExPerm information for the group <group name>\n" +
				"/experm group setperm <group name> <permname> <allowed> - Set the permission <permname> for <group name> to the true/false value of <allowed>\n";
		}

		public override string[] GetCommands ()
		{
			return new string[]{ "experm" };
		}

		public override void Execute (List<string> _params, CommandSenderInfo _senderInfo)
		{
			string subCommand;

			if (_params.Count == 0) {
				subCommand = "help";
			} else {
				subCommand = _params [0];
			}

			SDTM.API thisMod = SDTM.API.Instance;

			switch (subCommand.ToLower ()) {
			case "help":
				ProcessHelpCommand (_params, _senderInfo);
				break;
			case "nodes":
				string[] nodesList = API.Permissions.PermissionNodes;
				SdtdConsole.Instance.Output ("ExPerm Permission Nodes");
				foreach (string nodeName in nodesList) {
					SdtdConsole.Instance.Output (nodeName);
				}
				break;
			case "user":
				ProcessUserCommand (_params, _senderInfo);
				break;
			case "group":
				ProcessGroupCommand (_params, _senderInfo);
				break;
			default:
				SdtdConsole.Instance.Output ("[ExPerm] Command not found: " + _params [0]);
				break;
			}
		}

		public void ProcessHelpCommand(List<string> _params, CommandSenderInfo _senderInfo){
			SdtdConsole.Instance.Output ("ExPerm Help");
			SdtdConsole.Instance.Output ("Utility Commands");
			SdtdConsole.Instance.Output ("/experm help - Shows this Help.");
			SdtdConsole.Instance.Output ("/experm nodes\n - Shows a list of all permission nodes registered with ExPerm.");
			SdtdConsole.Instance.Output ("Group Commands");
			SdtdConsole.Instance.Output ("/experm group, /experm group list\n - Shows all Groups Registered with ExPerm.");
			SdtdConsole.Instance.Output ("/experm group <GroupName>\n - Shows all ExPerm information for the group <GroupName>.");
			SdtdConsole.Instance.Output ("/experm group create <GroupName>\n - Create a new ExPerm group called <GroupName>.");
			SdtdConsole.Instance.Output ("/experm group rename <GroupName> <NewGroupName>\n - Rename an existing ExPerm Group.");
			SdtdConsole.Instance.Output ("/experm group remove <GroupName>\n - Removes an existing ExPerm Group.");
			SdtdConsole.Instance.Output ("/experm group default <GroupName>\n - Makes the group called <GroupName> the default Group for new players.");
			SdtdConsole.Instance.Output ("/experm group setperm <GroupName> <PermissionNode> <allowed>\n - Sets the Group Permission value of <PermissionNode> to the true/false value supplied in <allowed>");
			SdtdConsole.Instance.Output ("/experm group removeperm <GroupName> <PermissionNode>\n - Removes the Group Permission entry for <PermissionNode>");
			SdtdConsole.Instance.Output ("User Commands");
			SdtdConsole.Instance.Output ("/experm user, /experm user list\n - Shows all Users Registered with ExPerm.");
			SdtdConsole.Instance.Output ("/experm user <SteamIdOrUserName>, /experm user <SteamIdOrUserName> list\n - Shows ExPerm information Registered for <SteamIdOrUserName>.");
			SdtdConsole.Instance.Output ("/experm user group <SteamIdOrUserName> <GroupName>\n - Sets the users group to <GroupName>.");
			SdtdConsole.Instance.Output ("/experm user remove <SteamIdOrUserName>\n - Removes any ExPerm information for the supplied user.");
			SdtdConsole.Instance.Output ("/experm user setperm <SteamIdOrUserName> <PermissionNode> <allowed>\n - Sets the  User Permission value of <PermissionNode> to the true/false value supplied in <allowed>.");
			SdtdConsole.Instance.Output ("/experm user test <SteamIdOrUserName> <PermissionNode>\n - Returns the Calculated Permission Value for the supplied PermissionNode");
		}	

		public void ProcessUserCommand(List<string> _params, CommandSenderInfo _senderInfo){
			string userCommand = "";
			if (_params.Count == 1) {
				userCommand = "list";
			} else {
				userCommand = _params [1].ToLower ();
			}

			SDTM.API thisMod = SDTM.API.Instance;
			string steamIdOrUsername="";
			string steamId = "";

			ClientInfo p;

			switch (userCommand) {
			case "list":
				if (SDTM.API.Permissions.isAllowed (_senderInfo.RemoteClientInfo, "permissions.user.list")==false) {
					SdtdConsole.Instance.Output ("[ExPerm] Permission Denied");
					return;
				}

				if (SDTM.API.Permissions.Users.Count > 0) {
					foreach (KeyValuePair<string, PermissionUser> kvp in SDTM.API.Permissions.Users) {
						PermissionUser user = kvp.Value;
						SdtdConsole.Instance.Output (user.SteamId+"\t"+user.DisplayName+"\t"+user.Group);
					}
				} else {
					SdtdConsole.Instance.Output ("[ExPerm] No ExPerm Users found.");
				}
				break;
			case "group":
				if (SDTM.API.Permissions.isAllowed (_senderInfo.RemoteClientInfo, "permissions.user.setgroup") == false) {
					SdtdConsole.Instance.Output ("[ExPerm] Permission Denied");
					return;
				}

				if (_params.Count != 4) {
					SdtdConsole.Instance.Output ("[ExPerm] Invalid Parameter Count");
					return;
				}

				steamIdOrUsername = _params [2];
				string groupName = _params [3];

				//p = PlayerUtils.GetPlayer (steamIdOrUsername);
				p = PlayerUtils.GetClientInfo(steamIdOrUsername);
				if (p == null) {
					if (SDTM.API.Permissions.Users.ContainsKey (steamIdOrUsername)) {
						steamId = steamIdOrUsername;
					} else {
						//TODO: see if we can load the player by the username

						SdtdConsole.Instance.Output ("[ExPerm] Player not found: " + steamIdOrUsername);
						return;
					}
				} else {
					steamId = p.playerId;
				}

				if (steamId == null || steamId == "") {
					if (p == null) {
						SdtdConsole.Instance.Output ("[ExPerm] You cannot create an ExPerm User for a player that is not online.");
					} else {
						SdtdConsole.Instance.Output ("[ExPerm] Player not Found: " + steamIdOrUsername);
					}
					return;
				}

				PermissionUser userPerm; 
				if (SDTM.API.Permissions.Users.ContainsKey (steamId)) {
					userPerm = SDTM.API.Permissions.Users [steamId];
				}else{
					userPerm = new PermissionUser (steamId);
					userPerm.DisplayName = p.playerName;
				}

				if (SDTM.API.Permissions.Groups.ContainsKey (groupName) == false) {
					SdtdConsole.Instance.Output ("[ExPerm] Group not Found: " + groupName);
					return;
				}

				userPerm.Group = groupName;
				SDTM.API.Permissions.Users [steamId] = userPerm;
				SDTM.API.Permissions.Save ();
				SdtdConsole.Instance.Output ("[ExPerm] Player's Group Updated.");
				break;
			case "remove":
				if (SDTM.API.Permissions.isAllowed (_senderInfo.RemoteClientInfo, "permissions.user.remove") == false) {
					SdtdConsole.Instance.Output ("[ExPerm] Permission Denied");
					return;
				}

				if (_params.Count != 3) {
					SdtdConsole.Instance.Output ("[ExPerm] Invalid Parameter Count");
					return;
				}

				steamIdOrUsername = _params [2];

				p = PlayerUtils.GetClientInfo (steamIdOrUsername);//PlayerUtils.GetPlayer (steamIdOrUsername);

				if (p == null) {
					if (SDTM.API.Permissions.Users.ContainsKey (steamIdOrUsername)) {
						steamId = steamIdOrUsername;
					} else {
						//TODO: see if we can load the player by the username

						SdtdConsole.Instance.Output ("[ExPerm] Player not found: " + steamIdOrUsername);
						return;
					}
				} else {
					steamId = p.playerId;
				}

				if (steamId == null || steamId == "") {
					if (p == null) {
						SdtdConsole.Instance.Output ("[ExPerm] You cannot remove an ExPerm User for a player that is not online.");
					} else {
						SdtdConsole.Instance.Output ("[ExPerm] Player not Found: " + steamIdOrUsername);
					}
					return;
				}

				if (SDTM.API.Permissions.Users.ContainsKey (steamId)) {
					if (SDTM.API.Permissions.Users.Remove (steamId)) {
						SDTM.API.Permissions.Save ();
						SdtdConsole.Instance.Output ("[ExPerm] ExPerm Player Data removed");
					} else {
						SdtdConsole.Instance.Output ("[ExPerm] Could not remove ExPerm Player Data.");
					}
				}

				break;
			case "setperm":
				//experm user setperm chromecide perm.name true
				if (SDTM.API.Permissions.isAllowed (_senderInfo.RemoteClientInfo, "permissions.user.setperm") == false) {
					SdtdConsole.Instance.Output ("[ExPerm] Permission Denied");
					return;
				}

				if (_params.Count != 5) {
					SdtdConsole.Instance.Output ("[ExPerm] Invalid Parameter Count");
					return;
				}

				steamIdOrUsername = _params [2];

				p = PlayerUtils.GetClientInfo (steamIdOrUsername);

				if (p == null) {
					if (SDTM.API.Permissions.Users.ContainsKey (steamIdOrUsername)) {
						steamId = steamIdOrUsername;
					} else {
						//TODO: see if we can load the player by the username

						SdtdConsole.Instance.Output ("[ExPerm] Player not found: " + steamIdOrUsername);
						return;
					}
				} else {
					steamId = p.playerId;

				}

				SdtdConsole.Instance.Output ("STEAM ID:" + steamId);

				if (steamId == null || steamId == "") {
					if (p == null) {
						SdtdConsole.Instance.Output ("[ExPerm] You cannot remove an ExPerm User for a player that is not online.");
					} else {
						SdtdConsole.Instance.Output ("[ExPerm] Player not Found: " + steamIdOrUsername);
					}
					return;
				}

				string permNode = _params [3];
				bool bIsAllowed = _params [4].ToLower () == "true" ? true : false;

				if (SDTM.API.Permissions.Users.ContainsKey (steamId)) {
					
					PermissionUser setPermUser = SDTM.API.Permissions.Users [steamId];

					if (setPermUser.Permissions.Set (permNode, bIsAllowed)) {
						SDTM.API.Permissions.Save ();
						SdtdConsole.Instance.Output ("[ExPerm] Permission set Successfully.");
					} else {
						SdtdConsole.Instance.Output ("[ExPerm] Could not set User Permission.");
					}
				} else {
					PermissionUser newUser = new PermissionUser (steamId);
					newUser.Permissions.Set (permNode, bIsAllowed);
					SDTM.API.Permissions.Users.Add (steamId, newUser);
					SDTM.API.Permissions.Save ();
					SdtdConsole.Instance.Output ("[ExPerm] Permission set Successfully.");
				}

				break;
			case "removeperm":
				if (SDTM.API.Permissions.isAllowed (_senderInfo.RemoteClientInfo, "permissions.user.removeperm") == false) {
					SdtdConsole.Instance.Output ("[ExPerm] Permission Denied");
					return;
				}

				if (_params.Count != 4) {
					SdtdConsole.Instance.Output ("[ExPerm] Invalid Parameter Count");
					return;
				}

				steamIdOrUsername = _params [2];

				p = PlayerUtils.GetClientInfo (steamIdOrUsername);

				if (p == null) {
					if (SDTM.API.Permissions.Users.ContainsKey (steamIdOrUsername)) {
						steamId = steamIdOrUsername;
					} else {
						//TODO: see if we can load the player by the username

						SdtdConsole.Instance.Output ("[ExPerm] Player not found: " + steamIdOrUsername);
						return;
					}
				} else {
					steamId = p.playerId;

				}

				SdtdConsole.Instance.Output ("STEAM ID:" + steamId);

				if (steamId == null || steamId == "") {
					if (p == null) {
						SdtdConsole.Instance.Output ("[ExPerm] You cannot remove an ExPerm User for a player that is not online.");
					} else {
						SdtdConsole.Instance.Output ("[ExPerm] Player not Found: " + steamIdOrUsername);
					}
					return;
				}

				string sPermNode = _params [3];

				if (SDTM.API.Permissions.Users.ContainsKey (steamId)) {
					PermissionUser setPermUser = SDTM.API.Permissions.Users [steamId];
					if (setPermUser.Permissions.Remove(sPermNode)) {
						SDTM.API.Permissions.Save ();
						SdtdConsole.Instance.Output ("[ExPerm] Permission removed Successfully.");
					} else {
						SdtdConsole.Instance.Output ("[ExPerm] Could not remove User Permission.");
					}
				} else {
					SdtdConsole.Instance.Output ("[ExPerm] Player not found: " + steamIdOrUsername);	
				}
				break;
			case "clearperms":
				if (SDTM.API.Permissions.isAllowed (_senderInfo.RemoteClientInfo, "permissions.user.removeperm") == false) {
					SdtdConsole.Instance.Output ("[ExPerm] Permission Denied");
					return;
				}

				if (_params.Count != 3) {
					SdtdConsole.Instance.Output ("[ExPerm] Invalid Parameter Count");
					return;
				}

				steamIdOrUsername = _params [2];

				p = PlayerUtils.GetClientInfo (steamIdOrUsername);

				if (p == null) {
					if (SDTM.API.Permissions.Users.ContainsKey (steamIdOrUsername)) {
						steamId = steamIdOrUsername;
					} else {
						//TODO: see if we can load the player by the username

						SdtdConsole.Instance.Output ("[ExPerm] Player not found: " + steamIdOrUsername);
						return;
					}
				} else {
					steamId = p.playerId;

				}

				if (steamId == null || steamId == "") {
					if (p == null) {
						SdtdConsole.Instance.Output ("[ExPerm] You cannot Clear Permissions for a player that is not online.");
					} else {
						SdtdConsole.Instance.Output ("[ExPerm] Player not Found: " + steamIdOrUsername);
					}
					return;
				}

				if (SDTM.API.Permissions.Users.ContainsKey (steamId)) {
					PermissionUser setPermUser = SDTM.API.Permissions.Users [steamId];

					if (setPermUser.Permissions.RemoveAll()) {
						SDTM.API.Permissions.Save ();
						SdtdConsole.Instance.Output ("[ExPerm] Permission Cleared Successfully.");
					} else {
						SdtdConsole.Instance.Output ("[ExPerm] Could not clear User Permission.");
					}
				} else {
					SdtdConsole.Instance.Output ("[ExPerm] Player not found: " + steamIdOrUsername);	
				}
				break;
			case "test":
				if (SDTM.API.Permissions.isAllowed (_senderInfo.RemoteClientInfo, "permissions.user.test")==false) {
					SdtdConsole.Instance.Output ("[ExPerm] Permission Denied");
					return;
				}

				bool allowed = API.Permissions.isAllowed (_params [2], _params [3]);
				SdtdConsole.Instance.Output("Allowed: "+(allowed?"Yes":"No"));
				break;
			case "info":
				steamIdOrUsername = _params [2];

				p = PlayerUtils.GetClientInfo (steamIdOrUsername);

				if (p == null) {
					if (SDTM.API.Permissions.Users.ContainsKey (steamIdOrUsername)) {
						steamId = steamIdOrUsername;
					} else {
						//TODO: see if we can load the player by the username

						SdtdConsole.Instance.Output ("[ExPerm] Player not found: " + steamIdOrUsername);
						return;
					}
				} else {
					steamId = p.playerId;
					SdtdConsole.Instance.Output ("STEAM ID:" + steamId);
				}

				if (steamId == null || steamId == "") {
					SdtdConsole.Instance.Output ("[ExPerm] Player not Found: " + steamIdOrUsername);
					return;
				}

				if (SDTM.API.Permissions.Users.ContainsKey (steamId)) {
					PermissionUser userInfo = SDTM.API.Permissions.Users [steamId];
					if (userInfo == null) {
						SdtdConsole.Instance.Output ("[ExPerm] No user information for: " + steamId);
						return;
					}

					SdtdConsole.Instance.Output ("[ExPerm] ExPerm User Information");
					SdtdConsole.Instance.Output ("SteamID: " + userInfo.SteamId);
					SdtdConsole.Instance.Output ("Display: " + userInfo.DisplayName);
					SdtdConsole.Instance.Output ("Group: " + (userInfo.Group == ""?"No Group":userInfo.Group));
					SdtdConsole.Instance.Output ("Permissions");
					Dictionary<string, bool> perms = userInfo.Permissions.GetAll ();
					foreach (KeyValuePair<string, bool> kvp in perms) {
						SdtdConsole.Instance.Output(kvp.Key+" - "+(kvp.Value==true?"Allowed":"Denied"));
					}
				} else {
					SdtdConsole.Instance.Output ("[ExPerm] Player not Found: " + steamIdOrUsername);
				}

				break;
			}
		}

		public void ProcessGroupCommand(List<string> _params, CommandSenderInfo _senderInfo){
			string userCommand = "";
			if (_params.Count == 1) {
				userCommand = "list";
			} else {
				userCommand = _params [1].ToLower ();
			}

			SDTM.API thisMod = SDTM.API.Instance;

			string groupName;

			switch (userCommand) {
			case "list":
				if (SDTM.API.Permissions.isAllowed (_senderInfo.RemoteClientInfo, "permissions.group.list")==false) {
					SdtdConsole.Instance.Output ("[ExPerm] Permission Denied");
					return;
				}

				if (SDTM.API.Permissions.Groups.Count > 0) {
					SdtdConsole.Instance.Output ("[ExPerm] Groups List");
					foreach (KeyValuePair<string, PermissionGroup> kvp in SDTM.API.Permissions.Groups) {
						PermissionGroup grp = kvp.Value;
						string sOut = grp.Name;
						if (grp.Name == SDTM.API.Permissions.DefaultGroupName) {
							sOut += "(Default Group)";
						}
						SdtdConsole.Instance.Output (sOut);
					}
				} else {
					SdtdConsole.Instance.Output ("[ExPerm] No ExPerm Groups found.");
				}

				break;
			case "info":
				if (SDTM.API.Permissions.isAllowed (_senderInfo.RemoteClientInfo, "permissions.group.info") == false) {
					SdtdConsole.Instance.Output ("[ExPerm] Permission Denied");
					return;
				}

				if (_params.Count != 3) {
					SdtdConsole.Instance.Output ("[ExPerm] Invalid Parameter Count.");
					return;
				}

				groupName = _params [2].ToLower ();

				if (!SDTM.API.Permissions.Groups.ContainsKey (groupName)) {
					SdtdConsole.Instance.Output ("[ExPerm] Group not Found.");
					return;
				}

				PermissionGroup pGrp = SDTM.API.Permissions.Groups [groupName];
				SdtdConsole.Instance.Output ("[ExPerm] Group Information");
				SdtdConsole.Instance.Output ("Name: " + pGrp.Name);
				SdtdConsole.Instance.Output ("Default: " + (pGrp.Name == SDTM.API.Permissions.DefaultGroupName ? "Yes" : "No"));
				SdtdConsole.Instance.Output ("Permissions: ");
				Dictionary<string, bool> perms = pGrp.Permissions.GetAll ();
				foreach (KeyValuePair<string, bool> kvp in perms) {
					SdtdConsole.Instance.Output(kvp.Key+" - "+(kvp.Value==true?"Allowed":"Denied"));
				}
				break;
			case "create":
				if (SDTM.API.Permissions.isAllowed (_senderInfo.RemoteClientInfo, "permissions.group.create") == false) {
					SdtdConsole.Instance.Output ("[ExPerm] Permission Denied");
					return;
				}

				if (_params.Count != 3) {
					SdtdConsole.Instance.Output ("[ExPerm] Invalid Parameter Count.");
					return;
				}

				groupName = _params [2].ToLower ();

				if (SDTM.API.Permissions.Groups.ContainsKey (groupName)) {
					SdtdConsole.Instance.Output ("[ExPerm] Group Already Exists");
					return;
				}

				if (SDTM.API.Permissions.AddGroup (groupName)) {
					SDTM.API.Permissions.Save ();
					SdtdConsole.Instance.Output ("[ExPerm] Group created Successfully.");
				} else {
					SdtdConsole.Instance.Output ("[ExPerm] Could not create Group.");
				}

				break;
			case "rename":
				if (SDTM.API.Permissions.isAllowed (_senderInfo.RemoteClientInfo, "permissions.group.modify") == false) {
					SdtdConsole.Instance.Output ("[ExPerm] Permission Denied");
					return;
				}

				if (_params.Count != 4) {
					SdtdConsole.Instance.Output ("[ExPerm] Invalid Parameter Count.");
					return;
				}


				groupName = _params [2].ToLower ();

				if (!SDTM.API.Permissions.Groups.ContainsKey (groupName)) {
					SdtdConsole.Instance.Output ("[ExPerm] Group not Found.");
					return;
				}

				string newGroupName = _params [3].ToLower ();

				if (SDTM.API.Permissions.Groups.ContainsKey (newGroupName)) {
					SdtdConsole.Instance.Output ("[ExPerm] Group already Exists: " + newGroupName);
					return;
				}

				PermissionGroup grp1 = SDTM.API.Permissions.Groups [groupName];

				if (grp1 == null) {
					SdtdConsole.Instance.Output ("[ExPerm] Group Object not Found: " + groupName);
					return;
				}

				grp1.Name = newGroupName;
				SDTM.API.Permissions.Groups.Remove (groupName);
				SDTM.API.Permissions.Groups.Add (newGroupName, grp1);
				foreach (KeyValuePair<string, PermissionUser> grp1User in SDTM.API.Permissions.Users) {
					PermissionUser grpRenUser = grp1User.Value;
					if (grpRenUser.Group == groupName) {
						grpRenUser.Group = newGroupName;
						SDTM.API.Permissions.Users [grp1User.Key] = grpRenUser;
					}
				}

				if (groupName == SDTM.API.Permissions.DefaultGroupName) {
					SDTM.API.Permissions.DefaultGroupName = newGroupName;
				}
				SDTM.API.Permissions.Save ();
				SdtdConsole.Instance.Output ("[ExPerm] Group renamed successfully.");
				break;
			case "remove":
				if (SDTM.API.Permissions.isAllowed (_senderInfo.RemoteClientInfo, "permissions.group.remove") == false) {
					SdtdConsole.Instance.Output ("[ExPerm] Permission Denied");
					return;
				}

				if (_params.Count != 3) {
					SdtdConsole.Instance.Output ("[ExPerm] Invalid Parameter Count.");
					return;
				}


				groupName = _params [2].ToLower ();

				if (!SDTM.API.Permissions.Groups.ContainsKey (groupName)) {
					SdtdConsole.Instance.Output ("[ExPerm] Group not Found.");
					return;
				}

				if (groupName == SDTM.API.Permissions.DefaultGroupName) {
					SdtdConsole.Instance.Output ("[ExPerm] You can not remove the default group.");
					return;
				}

				if (SDTM.API.Permissions.Groups.Remove (groupName)) {
					SdtdConsole.Instance.Output ("[ExPerm] Group removed.");
				} else {
					SdtdConsole.Instance.Output ("[ExPerm] Could not remove Group.");
				}
				break;
			case "default":
				if (SDTM.API.Permissions.isAllowed (_senderInfo.RemoteClientInfo, "permissions.group.default") == false) {
					SdtdConsole.Instance.Output ("[ExPerm] Permission Denied");
					return;
				}

				if (_params.Count != 3) {
					SdtdConsole.Instance.Output ("[ExPerm] Invalid Parameter Count.");
					return;
				}

				groupName = _params [2].ToLower ();

				if (!SDTM.API.Permissions.Groups.ContainsKey (groupName)) {
					SdtdConsole.Instance.Output ("[ExPerm] Group not Found.");
					return;
				}

				SDTM.API.Permissions.DefaultGroupName = groupName;
				SDTM.API.Permissions.Save ();

				SdtdConsole.Instance.Output ("[ExPerm] Default Group Updated");
				break;
			case "setperm":
				if (SDTM.API.Permissions.isAllowed (_senderInfo.RemoteClientInfo, "permissions.group.setperm") == false) {
					SdtdConsole.Instance.Output ("[ExPerm] Permission Denied");
					return;
				}

				if (_params.Count != 5) {
					SdtdConsole.Instance.Output ("[ExPerm] Invalid Paramater Count.");
					return;
				}

				groupName = _params [2].ToLower ();
				string permName = _params [3];
				bool bIsAllowed = _params [4].ToLower () == "true" ? true : false;
				if (!SDTM.API.Permissions.Groups.ContainsKey (groupName)) {
					SdtdConsole.Instance.Output ("[ExPerm] Group could not be Found.");
					return;
				}

				if (SDTM.API.Permissions.Groups [groupName].SetPermission (permName, bIsAllowed)) {
					SDTM.API.Permissions.Save ();
					SdtdConsole.Instance.Output ("[ExPerm] Group permission set Successfully.");
				} else {
					SdtdConsole.Instance.Output ("[ExPerm] Could not set Group Permission.");
				}

				break;
			case "removeperm":
				if (SDTM.API.Permissions.isAllowed (_senderInfo.RemoteClientInfo, "permissions.group.removeperm")==false) {
					SdtdConsole.Instance.Output ("[ExPerm] Permission Denied");
					return;
				}

				if (_params.Count != 4) {
					SdtdConsole.Instance.Output ("[ExPerm] Invalid Paramater Count.");
					return;
				}

				groupName = _params [2].ToLower ();
				string sPermName = _params [3];

				if (!SDTM.API.Permissions.Groups.ContainsKey (groupName)) {
					SdtdConsole.Instance.Output ("[ExPerm] Group could not be Found.");
					return;
				}

				if (SDTM.API.Permissions.Groups [groupName].RemovePermission (sPermName)) {
					SDTM.API.Permissions.Save ();
					SdtdConsole.Instance.Output ("[ExPerm] Group permission removed Successfully.");
				} else {
					SdtdConsole.Instance.Output ("[ExPerm] Could not remove Group Permission.");
				}
				break;
			case "clearperms":
				if (SDTM.API.Permissions.isAllowed (_senderInfo.RemoteClientInfo, "permissions.user.removeperm") == false) {
					SdtdConsole.Instance.Output ("[ExPerm] Permission Denied");
					return;
				}

				if (_params.Count != 3) {
					SdtdConsole.Instance.Output ("[ExPerm] Invalid Parameter Count");
					return;
				}

				groupName = _params [2].ToLower ();

				if (SDTM.API.Permissions.Groups.ContainsKey (groupName)) {
					PermissionGroup setPermGroup = SDTM.API.Permissions.Groups [groupName];

					if (setPermGroup.Permissions.RemoveAll()) {
						SDTM.API.Permissions.Save ();
						SdtdConsole.Instance.Output ("[ExPerm] Permissions Cleared Successfully.");
					} else {
						SdtdConsole.Instance.Output ("[ExPerm] Could not clear Group Permissions.");
					}
				} else {
					SdtdConsole.Instance.Output ("[ExPerm] Group not found: " + groupName);	
				}
				break;
			}
		}
	}
}