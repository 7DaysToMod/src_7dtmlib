using System;
using System.Collections.Generic;

namespace SDTM
{
	public class ChatCommand
	{
		private string _name;
		private string _description;
		private string _helptext;
		private string _permission;
		private OnChatCommandDelegate _handler;

		public ChatCommand (string name, string description, string helptext, OnChatCommandDelegate handler, string permissionNode="")
		{
			_name = name;
			_description = description;
			_helptext = helptext;
			_handler = handler;
			if (permissionNode != "") {
				_permission = permissionNode;
			} else {
				_permission = "chatcommand." + name;
			}
		}

		public string GetName(){
			return _name;
		}

		public string GetDescription(){
			return _description;
		}

		public string GetHelp(){
			return _helptext;
		}

		public string GetPermissionNode(){
			return _permission;
		}

		public void Execute(List<string> _params, ClientInfo _cInfo){
			_handler.Invoke(_params, _cInfo);
		}
	}
}

