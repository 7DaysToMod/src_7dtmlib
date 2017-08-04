using System;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Security;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.Specialized;

namespace SDTM.Servers.EndPoints
{
	public class EndPoint_SteamLogin:WWWEndpointProvider
	{
		
		private const string STEAM_LOGIN = "https://steamcommunity.com/openid/login";
		private static Regex steamIdUrlMatcher = new Regex (@"^http:\/\/steamcommunity\.com\/openid\/id\/([0-9]{17,18})");

		public EndPoint_SteamLogin () {
			ServicePointManager.ServerCertificateValidationCallback = (srvPoint, certificate, chain, errors) => {
				if (errors == SslPolicyErrors.None)
					return true;

				Log.Out ("Steam certificate error: {0}", errors);

				return true;
			};

		}

		public override WWWResponse ProcessRequest (WWWRequest request)
		{
			if (request._request.QueryString ["openid.ns"] != null) {
				ulong steamId = Validate (request._request);
				if (steamId > 0) {
					if(GameManager.Instance.adminTools.IsWhitelisted(steamId.ToString())){
						request.User.SetType("steam_user");
						return new WWWResponse ("/",302);						
					}else{
						return new WWWResponse ("You are not whitelisted");
					}

				} else {
					return new WWWResponse ("Auth Failed");
				}

			}	

			return new WWWResponse ("/login/", 302);
		}


		public static string GetSteamLoginUrl (string _returnHost, string _returnUrl) {
			Dictionary<string, string> queryParams = new Dictionary<string, string> ();

			queryParams.Add ("openid.ns", "http://specs.openid.net/auth/2.0");
			queryParams.Add ("openid.mode", "checkid_setup");
			queryParams.Add ("openid.return_to", _returnUrl);
			queryParams.Add ("openid.realm", _returnHost);
			queryParams.Add ("openid.identity", "http://specs.openid.net/auth/2.0/identifier_select");
			queryParams.Add ("openid.claimed_id", "http://specs.openid.net/auth/2.0/identifier_select");

			return STEAM_LOGIN + '?' + buildUrlParams (queryParams);
		}

		public string Validate (string authToken)
		{
			return "";
		}

		public ulong Validate (HttpListenerRequest _req) {
			if (getValue (_req, "openid.mode") == "cancel") {
				Log.Warning ("Steam OpenID login canceled");
				return 0;
			}
			string steamIdString = getValue (_req, "openid.claimed_id");
			ulong steamId = 0;
			Match steamIdMatch = steamIdUrlMatcher.Match (steamIdString);
			if (steamIdMatch.Success) {
				steamId = ulong.Parse (steamIdMatch.Groups [1].Value);
			} else {
				Log.Warning ("Steam OpenID login result did not give a valid SteamID");
				return 0;
			}

			Dictionary<string, string> queryParams = new Dictionary<string, string> ();

			queryParams.Add ("openid.ns", "http://specs.openid.net/auth/2.0");

			queryParams.Add ("openid.assoc_handle", getValue (_req, "openid.assoc_handle"));
			queryParams.Add ("openid.signed", getValue (_req, "openid.signed"));
			queryParams.Add ("openid.sig", getValue (_req, "openid.sig"));
			queryParams.Add ("openid.identity", "http://specs.openid.net/auth/2.0/identifier_select");
			queryParams.Add ("openid.claimed_id", "http://specs.openid.net/auth/2.0/identifier_select");

			string[] signeds = getValue (_req, "openid.signed").Split (',');
			foreach (string s in signeds) {
				queryParams ["openid." + s] = getValue (_req, "openid." + s);
			}

			queryParams.Add ("openid.mode", "check_authentication");

			byte[] postData = Encoding.ASCII.GetBytes (buildUrlParams (queryParams));
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create (STEAM_LOGIN);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = postData.Length;
			request.Headers.Add (HttpRequestHeader.AcceptLanguage, "en");
			using (Stream st = request.GetRequestStream ()) {
				st.Write (postData, 0, postData.Length);
			}

			HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
			string responseString = null;
			using (Stream st = response.GetResponseStream ()) {
				using (StreamReader str = new StreamReader (st)) {
					responseString = str.ReadToEnd ();
				}
			}

			if (responseString.ToLower ().Contains ("is_valid:true")) {
				return steamId;
			} else {
				Log.Warning ("Steam OpenID login failed: {0}", responseString);
				return 0;
			}
		}

		private static string buildUrlParams (Dictionary<string, string> _queryParams) {
			string[] paramsArr = new string[_queryParams.Count];
			int i = 0;
			foreach (KeyValuePair<string, string> kvp in _queryParams) {
				paramsArr [i++] = kvp.Key + "=" + Uri.EscapeDataString (kvp.Value);
			}
			return string.Join ("&", paramsArr);
		}

		private static string getValue (HttpListenerRequest _req, string _name) {
			NameValueCollection nvc = _req.QueryString;
			if (nvc [_name] == null) {
				throw new MissingMemberException ("OpenID parameter \"" + _name + "\" missing");
			}
			return nvc [_name];
		}
	}
}

