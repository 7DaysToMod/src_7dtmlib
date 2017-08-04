using System;

namespace SDTM.Servers.EndPoints
{
	public class Endpoint_ExpermSettings:WWWEndpointProvider
	{
		public Endpoint_ExpermSettings ()
		{
			this.acl = new string[] { "admin" };
		}

		public override WWWResponse ProcessRequest (WWWRequest request)
		{
			if (request.Form.Count > 0) {
				bool wasChanged = false;
				if (request.Form.ContainsKey ("tickrate")) {
					int passedRate = 0;

					if(int.TryParse(request.Form["tickrate"], out passedRate)){
						if (API.Instance.tickRate != passedRate) {
							API.Instance.tickRate = passedRate;
							wasChanged = true;
						}
					}
				}

				if (request.Form.ContainsKey ("overridegamepref")) {
					if (API.OverrideGamePrefs == false) {
						API.OverrideGamePrefs = true;
						wasChanged = true;
					}

					//see if anything else has been changed
					foreach(string fieldName in request.Form.Keys){
						if (fieldName == "tickrate" || fieldName == "overridegamepref") {
							continue;
						}

						switch (fieldName) {
						case "world_type":
							API.Instance.SetConfig ("worldtype", request.Form [fieldName], false, true);
							wasChanged = true;
							break;
						case "world_name":
							
							API.Instance.SetConfig ("worldname", request.Form [fieldName], false, true);
							wasChanged = true;
							break;
						}
					}
				} else {
					if (API.OverrideGamePrefs == true) {
						API.OverrideGamePrefs = false;
						wasChanged = true;
					}
				}

				if (wasChanged) {
					API.Instance.SaveConfig ();
				}
			}

			string groupListTemplate = Servers.HTTP.WWW._templates["expermgrouplist"];
			string groupListItemTemplate = Servers.HTTP.WWW._templates["expermgrouplistitem"];

			string groupInfoString = "";

			foreach (PermissionGroup group in API.Permissions.Groups.Values) {
				if (group.Name != "_super") {
					string groupListItem = groupListItemTemplate;

					groupListItem = groupListItem.Replace ("{group_name}", group.Name);

					if (group.Name != "public") {
						groupListItem = groupListItem.Replace ("{admin_level}", group.AdminLevel.ToString());
						string itemTools = "";
						itemTools+="<a href=\"/settings/experm/group?group="+group.Name+"\" title=\"Security\"><i class=\"fa fa-shield\" aria-hidden=\"true\"></i></a>";

						groupListItem = groupListItem.Replace ("{tools}", itemTools);
					} else {
						groupListItem = groupListItem.Replace ("{admin_level}", "Web Only");
						groupListItem = groupListItem.Replace ("{tools}", "");
					}

					groupInfoString += groupListItem;
				}
			}

			groupListTemplate = groupListTemplate.Replace ("{group_list_items}", groupInfoString);

			string userListTemplate = Servers.HTTP.WWW._templates["expermuserlist"];
			string userListItemTemplate = Servers.HTTP.WWW._templates["expermuserlistitem"];

			string userInfoString = "";

			foreach (PermissionUser user in API.Permissions.Users.Values) {
				
				string userListItem = userListItemTemplate;
				userListItem = userListItem.Replace ("{steam_id}", user.SteamId);
				userListItem = userListItem.Replace ("{display_name}", user.DisplayName);
				userListItem = userListItem.Replace ("{group_name}", user.Group);
				userListItem = userListItem.Replace ("{admin_level}", GameManager.Instance.adminTools.GetAdminToolsClientInfo (user.SteamId).PermissionLevel.ToString ());
				string itemTools = "";
				itemTools+="<a href=\"/settings/experm/user?user="+user.SteamId+"\" title=\"Security\"><i class=\"fa fa-shield\" aria-hidden=\"true\"></i></a>";
				//itemTools+=" <a href=\"/settings/player/inventory?player="+user.SteamId+"\" title=\"View Player Inventory\"><i class=\"fa fa-suitcase\" aria-hidden=\"true\"></i></a>";
				//itemTools+=" <a href=\"/settings/player/kick?player="+user.SteamId+"\" title=\"Kick/Ban User\"><i class=\"fa fa-ban\" aria-hidden=\"true\"></i></a>";

				userListItem = userListItem.Replace ("{tools}", itemTools);

				userInfoString += userListItem;
			}

			userListTemplate = userListTemplate.Replace ("{user_list_items}", userInfoString);

			string nodeListTemplate = Servers.HTTP.WWW._templates["expermnodelist"];
			string nodeListItemTemplate = Servers.HTTP.WWW._templates["expermnodelistitem"];

			string nodeInfoString = "";

			foreach (string node in API.Permissions.PermissionNodes) {
				string nodeListItem = nodeListItemTemplate;

				nodeListItem = nodeListItem.Replace ("{node_name}", node);

				nodeInfoString += nodeListItem;
			}

			nodeListTemplate = nodeListTemplate.Replace ("{node_list_items}", nodeInfoString);

			string html = Servers.HTTP.WWW._templates["expermsettingsmain"];
			html = html.Replace ("{experm_groups_list}", groupListTemplate);
			html = html.Replace ("{experm_users_list}", userListTemplate);
			html = html.Replace ("{experm_nodes_list}", nodeListTemplate);


			WWWResponse response = new WWWResponse (html);

			return response;
		}
	}
}

