using System;

namespace SDTM.Servers.EndPoints
{
	public class EndPoint_ExPermRemoveUserNode:WWWEndpointProvider
	{
		public EndPoint_ExPermRemoveUserNode ()
		{
			this.acl = new string[]{ "admin" };
		}

		public override WWWResponse ProcessRequest (WWWRequest request)
		{
			string nodeName = "";
			string steamId = "";
			string displayName = "";

			if (request.Form.Count > 0) {
				steamId = request.Form ["steam_id"];
			} else {
				if (request._request.QueryString ["steam_id"] != null) {
					steamId = request._request.QueryString ["steam_id"];
				}
			}

			if (API.Permissions.Users.ContainsKey (steamId)) {
				displayName = API.Permissions.Users [steamId].DisplayName;
			} else {
				
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
				nodeName = request.Form ["node"];
				steamId = request.Form ["steam_id"];

				if (nodeName == null || nodeName == "" || steamId == null || steamId == "") {
					return new WWWResponse ("/settings/experm", 302);
				}

				if (API.Permissions.Users.ContainsKey (steamId)) {
					PermissionUser pUser = API.Permissions.Users [steamId];
					if (pUser.Permissions.Exists (nodeName)) {
						pUser.Permissions.Remove (nodeName);
						API.Permissions.Save ();
						return new WWWResponse ("/settings/experm/user?user=" + steamId, 302);
					} else {
						return new WWWResponse ("/settings/experm", 302);
					}
				}
			} else {
				if (request._request.QueryString ["node"] != null) {
					nodeName = request._request.QueryString ["node"];
				}

				if (request._request.QueryString ["steam_id"] != null) {
					steamId = request._request.QueryString ["steam_id"];
				}

				if (nodeName == null || nodeName == "" || steamId == null || steamId == "") {
					return new WWWResponse ("/settings/experm", 302);
				}
			}

			string html = Servers.HTTP.WWW._templates["expermuserremovenode"];

			html = html.Replace ("{steam_id}", steamId);
			html = html.Replace ("{display_name}", displayName);
			html = html.Replace ("{node_name}", nodeName);

			return new WWWResponse (html);
		}
	}
}

