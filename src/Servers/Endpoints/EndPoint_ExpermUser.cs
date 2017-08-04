using System;
using System.Collections.Generic;

namespace SDTM.Servers.EndPoints
{
	public class EndPoint_ExpermUser:WWWEndpointProvider
	{
		public EndPoint_ExpermUser ()
		{
			this.acl = new string[]{"admin"};
		}

		public override WWWResponse ProcessRequest (WWWRequest request)
		{
			string steamId = "";
			string groupName = "";
			string displayName = "";

			if (request.Form.Count > 0) {
				steamId = request.Form ["steam_id"];
			} else {
				if (request._request.QueryString ["user"] != null) {
					steamId = request._request.QueryString ["user"];
				}
			}

			if (API.Permissions.Users.ContainsKey (steamId)) {
				groupName = API.Permissions.Users [steamId].Group;
				displayName = API.Permissions.Users [steamId].DisplayName;
			} else {
				groupName = "guest";
				//check the online players
				foreach (EntityPlayer player in PlayerUtils.GetOnlinePlayers()) {
					if(steamId == PlayerUtils.GetSteamID(player.entityId.ToString())){
						displayName = PlayerUtils.GetDisplayName(player.entityId.ToString());
					}
				}

				if (displayName == "") {
					//persistent data?

				}
			}

			if (request.Form.Count > 0) {
				groupName = request.Form ["experm_group"];
				API.Permissions.SetUserGroup (steamId, groupName);
				API.Permissions.Save ();
			}

			string html = Servers.HTTP.WWW._templates["expermusereditor"];

			string groupOptions = "";

			foreach(string expermGroupName in API.Permissions.Groups.Keys){
				groupOptions += "<option value=\"" + expermGroupName + "\"";
				if (expermGroupName == groupName) {
					groupOptions += " SELECTED";
				}
				groupOptions+=">" + expermGroupName + "</option>";
			}

			string userPermList = "";
			PermissionUser pUser = null;
			if (API.Permissions.Users.ContainsKey (steamId)) {
				pUser = API.Permissions.Users [steamId];
			}
			if (pUser != null) {
				string permissionEntryTemplate = Servers.HTTP.WWW._templates ["expermpermissionentry"];
				Dictionary<string, bool> permissions = pUser.Permissions.GetAll ();

				foreach (KeyValuePair<string, bool> perm in permissions) {
					string permItem = permissionEntryTemplate.Replace ("{node_name}", perm.Key);
					permItem = permItem.Replace ("{allowed}", perm.Value.ToString ());
					string toolList = "<a href=\"/settings/experm/user/removenode?steam_id="+steamId+"&node="+perm.Key+"\"><i class=\"fa fa-times\" aria-hidden=\"true\"></i></a>";
					permItem = permItem.Replace ("{tools}", toolList);
					userPermList += permItem;
				}
			}

			html = html.Replace ("{steam_id}", steamId);
			html = html.Replace ("{display_name}", displayName);
			html = html.Replace ("{experm_groups_options}", groupOptions);
			html = html.Replace ("{user_permissions}", userPermList);


			WWWResponse response = new WWWResponse (html);

			return response;
		}
	}
}

