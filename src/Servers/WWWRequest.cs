using System;
using System.Net;
using System.Collections.Generic;

namespace SDTM
{
	public class WWWRequest
	{
		public HttpListenerRequest _request;
		private bool _auth;
		private string groupName = SDTM.API.Permissions.PublicGroupName;
		private string steamId = "";

		public Dictionary<string, string> Form = new Dictionary<string, string>();
		public CookieCollection Cookies = new CookieCollection();

		public Servers.WWWUser User = null;

		public bool isAuth {
			get {
				return _auth;
			}
		}

		public WWWRequest (HttpListenerRequest req)
		{
			_request = req;	
		}
	}
}

