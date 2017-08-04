using System;
using System.Collections.Generic;

namespace SDTM.Servers
{
	public class WWWUser
	{
		private string _type="public";
		private string _sessionid = Guid.NewGuid().ToString();

		public Dictionary<string, string> vars = new Dictionary<string, string> ();

		public string UserType{
			get{ 
				return _type;
			}
		}

		public string SessionId{
			get{ 
				return _sessionid;
			}
		}

		public WWWUser ()
		{
			
		}

		public void SetType(string type){
			_type = type;
		}
	}
}

