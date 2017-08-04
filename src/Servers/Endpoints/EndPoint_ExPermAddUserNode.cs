using System;

namespace SDTM.Servers.EndPoints
{
	public class EndPoint_ExPermAddUserNode:WWWEndpointProvider
	{
		public EndPoint_ExPermAddUserNode ()
		{
			this.acl = new string[]{ "admin" };
		}

		public override WWWResponse ProcessRequest (WWWRequest request)
		{
			string nodeName = "";
			string steamId = "";
			bool allowed = true;
			if (request.Form.Count > 0) {
				nodeName = request.Form ["node"];
				steamId = request.Form ["steam_id"];
				if (!request.Form.ContainsKey ("allowed")) {
					allowed = false;
				}

				if (nodeName == null || nodeName == "" || steamId == null || steamId == "") {
					return new WWWResponse ("/settings/experm", 302);
				}

				if (API.Permissions.Users.ContainsKey (steamId)) {
					PermissionUser pUser = API.Permissions.Users [steamId];
					if (pUser.Permissions.Exists (nodeName)) {
						pUser.Permissions.Remove (nodeName);
					}
					pUser.Permissions.Set (nodeName, allowed);
					API.Permissions.Save ();
					return new WWWResponse ("/settings/experm/user?user=" + steamId, 302);
				}
			}

			return new WWWResponse ("/settings/experm", 302);
		}
	}
}

