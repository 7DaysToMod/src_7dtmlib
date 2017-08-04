using System;
using System.Net;

namespace SDTM
{
	public abstract class WWWEndpointProvider
	{
		public string[] acl = new string[]{"public"};
		public abstract WWWResponse ProcessRequest(WWWRequest request);
	}
}

