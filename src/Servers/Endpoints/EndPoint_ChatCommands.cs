using System;

namespace SDTM
{
	public class EndPoint_ChatCommands:WWWEndpointProvider
	{
		public EndPoint_ChatCommands ()
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

					groupInfoString += groupListItem;
				}
			}

			groupListTemplate = groupListTemplate.Replace ("{group_list_items}", groupInfoString);

			string userListTemplate = Servers.HTTP.WWW._templates["expermuserlist"];
			string userListItemTemplate = Servers.HTTP.WWW._templates["expermuserlistitem"];

			string userInfoString = "";

			foreach (PermissionUser user in API.Permissions.Users.Values) {

				string userListItem = userListItemTemplate;

				userListItem = userListItem.Replace ("{user_name}", user.DisplayName);

				userInfoString += userListItem;
			}

			userListTemplate = userListTemplate.Replace ("{user_list_items}", groupInfoString);

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

