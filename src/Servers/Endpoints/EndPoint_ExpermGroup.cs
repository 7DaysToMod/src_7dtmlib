using System;
using System.Collections.Generic;

namespace SDTM.Servers.EndPoints
{
	public class EndPoint_ExpermGroup:WWWEndpointProvider
	{
		public EndPoint_ExpermGroup ()
		{
			this.acl = new string[]{"admin"};
		}

		public override WWWResponse ProcessRequest (WWWRequest request)
		{
			string groupName = "";
			int adminLevel = 1000;

			PermissionGroup pGroup = null;

			if (request.Form.Count > 0) {
				groupName = request.Form ["group_name"];
				string adminFormStr = request.Form ["admin_level"];
				int.TryParse (adminFormStr, out adminLevel);
				if (API.Permissions.Groups.ContainsKey (groupName)) {
					pGroup = API.Permissions.Groups [groupName];
					pGroup.AdminLevel = adminLevel;
					API.Permissions.Save ();
				}
			} else {
				if (request._request.QueryString ["group"] != null) {
					groupName = request._request.QueryString ["group"];
				}
			}

			if (API.Permissions.Groups.ContainsKey (groupName)) {
				pGroup = API.Permissions.Groups [groupName];
				adminLevel = pGroup.AdminLevel;
			}

			string html = Servers.HTTP.WWW._templates["expermgroupeditor"];
			html = html.Replace ("{group_name}", groupName);
			html = html.Replace ("{admin_level}", adminLevel.ToString());

			string groupPermList = "";
			if (pGroup != null) {
				string permissionEntryTemplate = Servers.HTTP.WWW._templates ["expermpermissionentry"];
				Dictionary<string, bool> permissions = pGroup.Permissions.GetAll ();

				foreach (KeyValuePair<string, bool> perm in permissions) {
					string permItem = permissionEntryTemplate.Replace ("{node_name}", perm.Key);
					permItem = permItem.Replace ("{allowed}", perm.Value.ToString ());
					string toolList = "<a href=\"/settings/experm/group/removenode?group="+groupName+"&node="+perm.Key+"\"><i class=\"fa fa-times\" aria-hidden=\"true\"></i></a>";
					permItem = permItem.Replace ("{tools}", toolList);
					groupPermList += permItem;
				}
			}
			html = html.Replace ("{group_permissions}", groupPermList);


			WWWResponse response = new WWWResponse (html);

			return response;
		}
	}
}

