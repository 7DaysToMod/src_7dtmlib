using System;
using System.Net;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.IO;

namespace SDTM.Servers.HTTP
{
	public static class EndPointsList {
		public static Dictionary<string, WWWEndpointProvider> _endPoints = new Dictionary<string, WWWEndpointProvider>();

		//public static List<Dictionary<string, string>> mainMenu = new List<Dictionary<string, string>>();

		public static Dictionary<string, string> _menu = new Dictionary<string, string>();
		public static Dictionary<string, string> _adminMenu = new Dictionary<string, string>();

		public static void RegisterEndpoint(string path, WWWEndpointProvider provider){
			_endPoints.Add (path, provider);

		}

		public static void RegisterMenu(string title, string path){
			_menu.Add (title, path);
		}

		public static void RegisterSettingsMenu(string title, string path){
			_adminMenu.Add (title, path);
		}
	}


	public class WWW
	{
		//public static Dictionary<string, WWWEndpointProvider> _endPoints = new Dictionary<string, WWWEndpointProvider>();

		//public static Dictionary<string, string> _menu = Dictionary<string, string>();

		private readonly HttpListener _listener = new HttpListener();

		private Dictionary<string, WWWUser> _sessions = new Dictionary<string, WWWUser>();
		public static Dictionary<string, string> _templates = new Dictionary<string, string> ();

		public WWW(string ip, string port){
			//string homeep = "http://*:" + port;
			//_listener.Prefixes.Add(homeep);
			foreach(KeyValuePair<string, WWWEndpointProvider> rep in Servers.HTTP.EndPointsList._endPoints){
				string ep = "http://*:" + port + rep.Key;
				_listener.Prefixes.Add(ep);
			}


			string basePath = API.ModPath;
			if (basePath == "") {
				foreach (Mod m in ModManager.GetLoadedMods()) {
					if (m.ModInfo.Name == "7DTMLib") {
						basePath = m.Path;
					}
				}
			}

			if (basePath!="") {
				_templates.Add ("main", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_main.html"));
				_templates.Add ("menu", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_menu.html"));
				_templates.Add ("menuitem", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_menuitem.html"));
				_templates.Add ("settingsmain", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_settingsmain.html"));
				_templates.Add ("settingsmenu", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_settingsmenu.html"));
				_templates.Add ("settingsmenuitem", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_settingsmenuitem.html"));
				_templates.Add ("settingsform", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_settingsform.html"));
				_templates.Add ("expermsettingsmain", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_expermsettings.html"));

				_templates.Add ("expermgrouplist", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_expermgrouplist.html"));
				_templates.Add ("expermgrouplistitem", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_expermgrouplistitem.html"));

				_templates.Add ("expermuserlist", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_expermuserlist.html"));
				_templates.Add ("expermuserlistitem", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_expermuserlistitem.html"));

				_templates.Add ("expermnodelist", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_expermnodelist.html"));
				_templates.Add ("expermnodelistitem", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_expermnodelistitem.html"));

				_templates.Add ("expermgroupeditor", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_expermgroup.html"));
				_templates.Add ("expermgroupremovenode", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_expermgroupremovenode.html"));
				_templates.Add ("expermuserremovenode", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_expermuserremovenode.html"));
				_templates.Add ("expermusereditor", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_expermuser.html"));
				_templates.Add ("expermpermissionentry", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_expermpermissionentry.html"));

				_templates.Add("onlineplayers", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_onlineplayerlist.html"));
				_templates.Add("onlineplayersitem", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_onlineplayerlistitem.html"));

				_templates.Add ("loginform", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_loginform.html"));
				_templates.Add ("steambutton", File.ReadAllText (basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "tpl_steambutton.html"));
			}

			_listener.Start();
			Log.Out ("Web Server Listening on Port: " + port);
		}

		public WWW(Dictionary<string, WWWEndpointProvider> endPoints){
			Servers.HTTP.EndPointsList._endPoints = endPoints;
		}

		public void Run()
		{
			
			ThreadPool.QueueUserWorkItem((o) =>
				{
					
					try
					{
						while (_listener.IsListening)
						{
							ThreadPool.QueueUserWorkItem((c) =>
								{
									//bool redirected = false;
									var ctx = c as HttpListenerContext;

									try
									{
										
										if(ctx.Request.Url.LocalPath=="/favicon.ico"){
											ctx.Response.Close();
											return;
										}

										string localPath = ctx.Request.Url.LocalPath;

										string fileName = Path.GetFileName(localPath);

										if(fileName!="" && fileName.IndexOf(".")>0){
											localPath = localPath.Replace(Path.GetFileName(localPath), "");
										}else{
											if(localPath.Substring(localPath.Length-1,1)!="/"){
												localPath+="/";
											}
										}

										WWWRequest request = new WWWRequest(ctx.Request);
										WWWUser requestUser = null;

										string sessionId = "";

										if(ctx.Request.Cookies["sessionid"]!=null){
											sessionId = ctx.Request.Cookies["sessionid"].Value;
										}

										if(sessionId!="" && _sessions.ContainsKey(sessionId)){
											requestUser = _sessions[sessionId];
										}else{
											requestUser = new WWWUser();
										}

										if(localPath == "/logout/"){
											_sessions.Remove(sessionId);
											ctx.Response.Redirect("/login/");
											ctx.Response.OutputStream.Close();
											return;
										}

										ctx.Response.Cookies.Add(new Cookie("sessionid", requestUser.SessionId+"; path=/"));

										if(!Servers.HTTP.EndPointsList._endPoints.ContainsKey(localPath)){
											if(Path.GetExtension(localPath)!=""){
												localPath = Path.GetDirectoryName(localPath);
											}
										}

										if(Servers.HTTP.EndPointsList._endPoints.ContainsKey(localPath)){
											WWWEndpointProvider endpointProvider = (WWWEndpointProvider) Servers.HTTP.EndPointsList._endPoints[localPath];

											bool allowed = false;

											if(requestUser.UserType=="admin"){
												allowed = true;
											}else{
												if(endpointProvider.acl!=null){
													foreach(string acl in endpointProvider.acl){
														if(acl==requestUser.UserType  || acl=="public"){
															allowed = true;
															break;
														}
													}
												}
											}

											if(!allowed){
												ctx.Response.Redirect("/login/");
												ctx.Response.OutputStream.Close();
												return;
											}else{
												
												string postData = GetRequestPostData(ctx.Request);

												Dictionary<string, string> FormValues = new Dictionary<string, string>();

												if(postData!=string.Empty && postData.Length>0){
													FormValues = GetFormParameters(postData);
												}

												request.Form = FormValues;

												request.Cookies = ctx.Request.Cookies;


												request.User = requestUser;

												WWWResponse userResponse = endpointProvider.ProcessRequest(request);

												//update session user
												_sessions[request.User.SessionId] = request.User;

												foreach(Cookie ck in userResponse.Cookies){
													if(ck.Name!="sessionid"){
														ctx.Response.Cookies.Add(ck);
													}
												}

												if(userResponse.StatusCode==302){
													ctx.Response.Redirect(userResponse.Content);
													//redirected=true;
												}else{
													if(userResponse.StatusCode==-1){//file download
														
														using (FileStream fs = File.OpenRead(userResponse.Content))
														{
															string filename = Path.GetFileName(userResponse.Content);
															//response is HttpListenerContext.Response...
															ctx.Response.ContentLength64 = fs.Length;
															ctx.Response.SendChunked = false;
															ctx.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Octet;
															ctx.Response.AddHeader("Content-disposition", "attachment; filename=" + filename);

															byte[] buffer = new byte[64 * 1024];
															int read;
															using (BinaryWriter bw = new BinaryWriter(ctx.Response.OutputStream))
															{
																while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
																{
																	bw.Write(buffer, 0, read);
																	bw.Flush(); //seems to have no effect
																}

																bw.Close();
															}

															ctx.Response.StatusCode = (int)HttpStatusCode.OK;
															ctx.Response.StatusDescription = "OK";
															ctx.Response.OutputStream.Close();
															return;
														}
													}else{
														if(userResponse.StatusCode==-2){ // XML Output, no template
															byte[] buf = Encoding.UTF8.GetBytes(userResponse.Content);
															ctx.Response.StatusCode = 200;

															ctx.Response.ContentType = "text/xml";
															ctx.Response.ContentLength64 = buf.Length;
															ctx.Response.OutputStream.Write(buf, 0, buf.Length);
														}else{
															if(userResponse.StatusCode==-4){ //JSON output, no template
																byte[] buf = Encoding.UTF8.GetBytes(userResponse.Content);
																ctx.Response.StatusCode = 200;

																ctx.Response.ContentType = "application/json";
																ctx.Response.ContentLength64 = buf.Length;
																ctx.Response.OutputStream.Write(buf, 0, buf.Length);
															}else{
																if(userResponse.StatusCode==-5){
																	using (FileStream fs = File.OpenRead(userResponse.Content))
																	{
																		string filename = Path.GetFileName(userResponse.Content);
																		//response is HttpListenerContext.Response...
																		ctx.Response.ContentLength64 = fs.Length;
																		ctx.Response.SendChunked = false;
																		ctx.Response.ContentType = "image/png";
																		ctx.Response.AddHeader("Content-disposition", "attachment; filename=" + filename);

																		byte[] buffer = new byte[64 * 1024];
																		int read;
																		using (BinaryWriter bw = new BinaryWriter(ctx.Response.OutputStream))
																		{
																			while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
																			{
																				bw.Write(buffer, 0, read);
																				bw.Flush(); //seems to have no effect
																			}

																			bw.Close();
																		}

																		ctx.Response.StatusCode = (int)HttpStatusCode.OK;
																		ctx.Response.StatusDescription = "OK";
																		ctx.Response.OutputStream.Close();
																		return;
																	}
																}else{

																	string menuString = BuildMenu(request.User,EndPointsList._menu);

																	string rstr = "TEMPLATE NOT FOUND: "+ctx.Request.Url.LocalPath;
																	string[] parts = ctx.Request.Url.LocalPath.Split(new string[]{"/"}, StringSplitOptions.RemoveEmptyEntries);

																	if(parts.Length==0 || parts[0]!="settings"){
																		rstr = _templates["main"];
																	}else{
																		rstr = _templates["settingsmain"];
																		string settingsMenuStr = BuildSettingsMenu(request.User, EndPointsList._adminMenu);
																		rstr = rstr.Replace("{settings_menu}", settingsMenuStr);
																	}
																		
																	rstr = rstr.Replace("{content}", userResponse.Content).Replace("{title}", userResponse.Title).Replace("{menu}", menuString);

																	byte[] buf = Encoding.UTF8.GetBytes(rstr);
																	ctx.Response.StatusCode = userResponse.StatusCode;

																	ctx.Response.ContentType = "text/html";
																	ctx.Response.ContentLength64 = buf.Length;
																	ctx.Response.OutputStream.Write(buf, 0, buf.Length);
																}
															}
														}
													}
												}
											}
										}else{
											string rstr = "Page not Found";
											byte[] buf = Encoding.UTF8.GetBytes(rstr);
											ctx.Response.StatusCode = 404;
											ctx.Response.ContentLength64 = buf.Length;
											ctx.Response.OutputStream.Write(buf, 0, buf.Length);
										}
									}
									catch (Exception err){
										Log.Out(err.Message);
									} // suppress any exceptions
									finally
									{
										// always close the stream
										ctx.Response.OutputStream.Close();
									}
								}, _listener.GetContext());
						}
					}
					catch { } // suppress any exceptions
				});
		}

		public void Stop()
		{
			_listener.Stop();
			_listener.Close();
		}

		public string BuildMenu(WWWUser user,  Dictionary<string, string> items){
			string itemsString = "";

			if (user.UserType != "public") {
				itemsString+=_templates["menuitem"].Replace("{path}", "/").Replace("{title}", "Home");
			}
			foreach(KeyValuePair<string, string> menuItem in items){
				if (menuItem.Value!="" && Servers.HTTP.EndPointsList._endPoints [menuItem.Value]!=null) {
					WWWEndpointProvider endpointProvider = Servers.HTTP.EndPointsList._endPoints [menuItem.Value];
					bool allowed = false;

					if(user.UserType=="admin"){
						allowed = true;
					}else{
						if(endpointProvider.acl!=null){
							foreach(string acl in endpointProvider.acl){
								if(acl==user.UserType || acl=="public"){
									allowed = true;
									break;
								}
							}
						}
					}

					if (allowed) {
						itemsString += _templates["menuitem"].Replace("{path}", menuItem.Value).Replace("{title}", menuItem.Key);	
					}
				}
			}

			if (user.UserType != "public") {
				if (user.UserType == "admin") {
					itemsString+=_templates["menuitem"].Replace("{path}", "/settings/").Replace("{title}", "Settings");
				}
				itemsString+=_templates["menuitem"].Replace("{path}", "/logout/").Replace("{title}", "Logout");
			}

			return _templates["menu"].Replace("{items}", itemsString);
		}

		public string BuildSettingsMenu(WWWUser user,  Dictionary<string, string> items){
			string itemsString = "";


			if(user.UserType=="admin"){
				itemsString += _templates["menuitem"].Replace("{path}", "/settings/").Replace("{title}", "7dtmlib");
				itemsString += _templates["menuitem"].Replace("{path}", "/settings/experm").Replace("{title}", "ExPerm");
				itemsString += _templates["menuitem"].Replace("{path}", "/settings/onlineplayers").Replace("{title}", "Online Players");
			}

			foreach(KeyValuePair<string, string> menuItem in items){
				if (menuItem.Value!="" && Servers.HTTP.EndPointsList._endPoints [menuItem.Value]!=null) {
					WWWEndpointProvider endpointProvider = Servers.HTTP.EndPointsList._endPoints [menuItem.Value];
					bool allowed = false;

					if(user.UserType=="admin"){
						allowed = true;
					}else{
						if(endpointProvider.acl!=null){
							foreach(string acl in endpointProvider.acl){
								if(acl==user.UserType || acl=="public"){
									allowed = true;
									break;
								}
							}
						}
					}

					if (allowed) {
						itemsString += _templates["menuitem"].Replace("{path}", menuItem.Value).Replace("{title}", menuItem.Key);
					}
				}
			}


			return _templates["settingsmenu"].Replace("{items}", itemsString);
		}

		public static string GetRequestPostData(HttpListenerRequest request)
		{
			if (!request.HasEntityBody)
			{
				return string.Empty;
			}
			using (System.IO.Stream body = request.InputStream) // here we have data
			{
				using (System.IO.StreamReader reader = new System.IO.StreamReader(body, request.ContentEncoding))
				{
					return reader.ReadToEnd();
				}
			}
		}

		//static readonly Regex HttpQueryDelimiterRegex = new Regex(@"\?", RegexOptions.Compiled);
		static readonly Regex HttpQueryParameterDelimiterRegex = new Regex(@"&", RegexOptions.Compiled);
		static readonly Regex HttpQueryParameterRegex = new Regex(@"^(?<ParameterName>\S+)=(?<ParameterValue>\S*)$", RegexOptions.Compiled);

		static Dictionary<string, string> GetQueryParameters(string bodyString)
		{
			var parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			var components = bodyString;//HttpQueryDelimiterRegex.Split(pathAndQuery, 2);

			if (components.Length > 1)
			{
				var queryParameters = HttpQueryParameterDelimiterRegex.Split(bodyString);

				foreach(var queryParameter in queryParameters)
				{
					var match = HttpQueryParameterRegex.Match(queryParameter);

					if (!match.Success) continue;

					string parameterName = HtmlDecode(match.Groups["ParameterName"].Value) ?? string.Empty;
					string parameterValue = HtmlDecode(match.Groups["ParameterValue"].Value) ?? string.Empty;

					parameters[parameterName] = parameterValue;
				}
			}

			return parameters;
		}

		static Dictionary<string, string> GetFormParameters(string bodyString)
		{
			var parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			var components = bodyString;//HttpQueryDelimiterRegex.Split(pathAndQuery, 2);

			if (components.Length > 1)
			{
				var queryParameters = HttpQueryParameterDelimiterRegex.Split(bodyString);

				foreach(var queryParameter in queryParameters)
				{
					var match = HttpQueryParameterRegex.Match(queryParameter);

					if (!match.Success) continue;

					string parameterName = Uri.UnescapeDataString (match.Groups ["ParameterName"].Value).Replace ("+", " ") ?? string.Empty;
					string parameterValue = Uri.UnescapeDataString(match.Groups["ParameterValue"].Value).Replace ("+", " ") ?? string.Empty;

					parameters[parameterName] = parameterValue;
				}
			}

			return parameters;
		}

		public static string HtmlDecode(string source)
		{
			string[] parts = source.Split(new string[] {"&#x"}, StringSplitOptions.None);
			for (int i = 1; i < parts.Length; i++)
			{
				int n = parts[i].IndexOf(';');
				string number = parts[i].Substring(0,n);
				try
				{
					int unicode = Convert.ToInt32(number,16);
					parts[i] = ((char)unicode) + parts[i].Substring(n+1);
				} catch {}
			}
			return String.Join("",parts);
		}

		public static Dictionary<string, string> GetCookies(HttpListenerRequest request){
			Dictionary<string, string> cookies = new Dictionary<string, string> ();
			//string newcookies = "";
			if(request.Headers["cookie"]!=null && request.Headers["cookie"]!=""){
				string[] lst = request.Headers ["cookie"].Split (new string[]{";"}, StringSplitOptions.RemoveEmptyEntries);
				foreach (string cookieItem in lst) {
					string[] itemVal = cookieItem.Split(new string[]{"="}, StringSplitOptions.RemoveEmptyEntries);
					string cookieName = itemVal [0];

					if (cookieName.Substring (0, 1) == " ") {
						cookieName = cookieName.Substring (1, cookieName.Length-1);
					}

					if (cookies.ContainsKey (cookieName)) {
						cookies.Remove (cookieName);
					}
					cookies.Add (cookieName, itemVal[1]);
				}
			}

			return cookies;
		}
	}
}