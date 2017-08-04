using System;
using System.Collections.Generic;
using System.Net;

namespace SDTM
{
	public class WWWResponse
	{
		private Dictionary<string, string> _headers = new Dictionary<string, string>();
		private int _statusCode=200;
		private string _content="";

		public string Title = "7 Days to Die";
		public CookieCollection Cookies = new CookieCollection();

		public string Content{
			get{ 
				return _content;
			}
		}

		public int StatusCode{
			get{ 
				return _statusCode;
			}
		}

		public Dictionary<string, string> Headers{
			get{ 
				return _headers;
			}
		}

		public WWWResponse (string content, int statusCode=200, Dictionary<string, string> headers=null)
		{
			this._content = content;
			this._statusCode = statusCode;
			if (headers == null) {
				this._headers = new Dictionary<string, string> ();
			} else {
				this._headers = headers;
			}
		}
	}
}

