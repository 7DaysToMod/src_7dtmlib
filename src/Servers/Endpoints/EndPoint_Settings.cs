using System;

namespace SDTM.Servers.EndPoints
{
	public class EndPoint_Settings:WWWEndpointProvider
	{
		public EndPoint_Settings ()
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

				if (request.Form.ContainsKey ("webconsole_enabled")) {
					API.WebConsoleEnabled = true;
					API.WebConsolePort = request.Form ["webconsole_port"];
					wasChanged = true;
				} else {
					API.WebConsoleEnabled = false;
					wasChanged = true;
				}

				if (request.Form.ContainsKey ("chatcommands_enabled")) {
					API.ChatCommandsEnabled = true;
					wasChanged = true;
				} else {
					API.ChatCommandsEnabled = false;
					wasChanged = true;
				}

				if (wasChanged) {
					API.Instance.SaveConfig ();
				}
			}

			string html = Servers.HTTP.WWW._templates["settingsform"];
			html = html.Replace ("{tickrate}", API.Instance.tickRate.ToString ());
			html = html.Replace ("{chatcommands_enabled}", API.ChatCommandsEnabled.ToString().ToLower());
			html = html.Replace ("{overridegameprefs}", API.OverrideGamePrefs.ToString().ToLower());
			html = html.Replace ("{world_type}", GamePrefs.GetString (EnumGamePrefs.GameWorld).ToLower());
			html = html.Replace ("{world_name}", GamePrefs.GetString(EnumGamePrefs.GameName));
			html = html.Replace ("{webconsole_enabled}", API.WebConsoleEnabled.ToString().ToLower());
			html = html.Replace ("{webconsole_port}", API.WebConsolePort);
			WWWResponse response = new WWWResponse (html);

			return response;
		}
	}
}

