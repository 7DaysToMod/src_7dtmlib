using System;
using System.Collections.Specialized;
using SDTM.Servers.HTTP;
namespace SDTM.Servers.EndPoints
{
	public class EndPoint_PasswordLogin:WWWEndpointProvider
	{
		new public string[] acl = new string[]{ "public" };

		public override WWWResponse ProcessRequest(WWWRequest request){
			//GameManager.Instance.adminTools.IsWhiteListEnabled
			WWWResponse response = null;
			if (!request.Form.ContainsKey("password") || request.Form["password"]=="") {
				string loginTemplate = WWW._templates ["loginform"];
				if (GameManager.Instance.adminTools.IsWhiteListEnabled ()) {
					string steamButton = WWW._templates["steambutton"];
					string hostAndPort = "http://"+request._request.Url.Host + ":" + request._request.Url.Port;

					string steamLoginUrl = EndPoint_SteamLogin.GetSteamLoginUrl (hostAndPort, hostAndPort+"/steam");

					steamButton = steamButton.Replace ("{login_url}", steamLoginUrl);

					loginTemplate = loginTemplate.Replace("{steam_button}", steamButton);
					loginTemplate = loginTemplate.Replace("{steam_button_title}", "Login with Steam");
				} else {
					loginTemplate = loginTemplate.Replace ("{steam_button}", "");
				}

				response = new WWWResponse (loginTemplate);
			} else {
				string pw = request.Form["password"];

				if (pw == GamePrefs.GetString (EnumGamePrefs.ServerPassword)) {
					request.User.SetType ("user");
					response = new WWWResponse ("/", 302);
				} else {
					if (pw == GamePrefs.GetString (EnumGamePrefs.ControlPanelPassword)) {
						request.User.SetType ("admin");
						response = new WWWResponse ("/", 302);
					} else {
						response = new WWWResponse ("/login/?err=InvalidPassword", 302);
					}
				}
			}

			return response;
		}
	}
}

