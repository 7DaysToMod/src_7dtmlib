using System;

namespace SDTM.Servers.EndPoints
{
	public class EndPoint_OnlinePlayers: WWWEndpointProvider
	{

		public EndPoint_OnlinePlayers(){
			this.acl = new string[]{"admin"};	
		}

		public override WWWResponse ProcessRequest (WWWRequest request)
		{
			
			string html = Servers.HTTP.WWW._templates["onlineplayers"];
			string itemHtml = Servers.HTTP.WWW._templates["onlineplayersitem"];

			string itemList = "";

			foreach (EntityPlayer player in PlayerUtils.GetOnlinePlayers()) {
				string sitem = itemHtml;
				string steamId = PlayerUtils.GetSteamID (player.entityId.ToString ());
				sitem = sitem.Replace("{steam_id}", steamId);
				sitem = sitem.Replace("{display_name}", PlayerUtils.GetDisplayName(player.entityId.ToString()));

				string playerGroupName = "guest";
				string playerAdminLevel = "1000";

				if (SDTM.API.Permissions.Users.ContainsKey (steamId)) {
					playerGroupName = SDTM.API.Permissions.Users [steamId].Group;
				}

				playerAdminLevel = GameManager.Instance.adminTools.GetAdminToolsClientInfo (steamId).PermissionLevel.ToString ();

				sitem = sitem.Replace("{experm_group}", playerGroupName);
				sitem = sitem.Replace("{admin_level}", playerAdminLevel);
				string itemTools = "";
				itemTools+="<a href=\"/settings/experm/user?user="+steamId+"\" title=\"Security\"><i class=\"fa fa-shield\" aria-hidden=\"true\"></i></a>";
				//itemTools+=" <a href=\"/settings/player/inventory?player="+steamId+"\" title=\"View Player Inventory\"><i class=\"fa fa-suitcase\" aria-hidden=\"true\"></i></a>";
				//itemTools+=" <a href=\"/settings/player/kick?player="+steamId+"\" title=\"Kick/Ban User\"><i class=\"fa fa-ban\" aria-hidden=\"true\"></i></a>";
				sitem = sitem.Replace("{tools}", itemTools);
				itemList+=sitem;
			}

			html = html.Replace ("{player_list}", itemList);

			WWWResponse response = new WWWResponse (html);

			return response;
		}
	}
}

