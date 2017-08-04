using System;

namespace SDTM.Servers.EndPoints
{
	public class EndPoint_ExPermAddGroupNode:WWWEndpointProvider
	{
		public EndPoint_ExPermAddGroupNode ()
		{
			this.acl = new string[]{ "admin" };
		}

		public override WWWResponse ProcessRequest (WWWRequest request)
		{
			string nodeName = "";
			string groupName = "";
			bool allowed = true;
			if (request.Form.Count > 0) {
				nodeName = request.Form ["node"];
				groupName = request.Form ["group"];

				if (!request.Form.ContainsKey ("allowed")) {
					allowed = false;
				}

				if (nodeName == null || nodeName == "" || groupName == null || groupName == "") {
					return new WWWResponse ("/settings/experm", 302);
				}

				if (API.Permissions.Groups.ContainsKey (groupName)) {
					PermissionGroup pGroup = API.Permissions.Groups [groupName];
					if (pGroup.Permissions.Exists (nodeName)) {
						pGroup.Permissions.Remove (nodeName);
					}
					pGroup.Permissions.Set (nodeName, allowed);
					API.Permissions.Save ();
					return new WWWResponse ("/settings/experm/group?group="+groupName, 302);
				}
			}

			return new WWWResponse ("/settings/experm", 302);
		}
	}
}

