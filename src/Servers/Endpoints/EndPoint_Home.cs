using System;

namespace SDTM.Servers.EndPoints
{
	public class EndPoint_Home:WWWEndpointProvider
	{
		public EndPoint_Home(){
			this.acl = new string[]{ "user", "steam_user", "admin" };
		}

		public override WWWResponse ProcessRequest(WWWRequest request){
			//load the html and send the response
			WWWResponse response;

			if (request.User.UserType != "public") {
				response = new WWWResponse ("Welcome, " + request.User.UserType);
			} else {
				response = new WWWResponse ("Not Allowed");
			}

			return response;
		}
	}
}

