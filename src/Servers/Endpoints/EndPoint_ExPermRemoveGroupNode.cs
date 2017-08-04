using System;

namespace SDTM.Servers.EndPoints
{
	public class EndPoint_ExPermRemoveGroupNode:WWWEndpointProvider
	{
		public EndPoint_ExPermRemoveGroupNode ()
		{
			this.acl = new string[]{ "admin" };
		}

		public override WWWResponse ProcessRequest (WWWRequest request)
		{
			string nodeName = "";
			string groupName = "";
			if (request.Form.Count > 0) {
				nodeName = request.Form ["node"];
				groupName = request.Form ["group"];

				if (nodeName == null || nodeName == "" || groupName == null || groupName == "") {
					return new WWWResponse ("/settings/experm", 302);
				}

				if (API.Permissions.Groups.ContainsKey (groupName)) {
					PermissionGroup pGroup = API.Permissions.Groups [groupName];
					if (pGroup.Permissions.Exists (nodeName)) {
						pGroup.Permissions.Remove (nodeName);
						API.Permissions.Save ();
						return new WWWResponse ("/settings/experm/group?group=" + groupName, 302);
					} else {
						return new WWWResponse ("/settings/experm", 302);
					}
				}
			} else {
				if (request._request.QueryString ["node"] != null) {
					nodeName = request._request.QueryString ["node"];
				}

				if (request._request.QueryString ["group"] != null) {
					groupName = request._request.QueryString ["group"];
				}

				if (nodeName == null || nodeName == "" || groupName == null || groupName == "") {
					return new WWWResponse ("/settings/experm", 302);
				}
			}

			string html = Servers.HTTP.WWW._templates["expermgroupremovenode"];

			html = html.Replace ("{group_name}", groupName);
			html = html.Replace ("{node_name}", nodeName);

			return new WWWResponse (html);
		}
	}
}

