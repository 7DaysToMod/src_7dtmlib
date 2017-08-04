using System;
using System.Collections.Generic;

namespace SDTM
{
	public class ChatCmdHelp
	{
		public static void Help(List<string> _params, ClientInfo _cInfo){
			if (API.Permissions.isAllowed (_cInfo, "chatcommand.help")) {
				SDTM.PlayerUtils.SendChatMessageAs (_cInfo, "Help:", "Server");
				Dictionary<string, ChatCommand> commands = API.Instance._chatCommands;
				foreach(KeyValuePair<string, ChatCommand> command in commands){
					if (API.Permissions.isAllowed (_cInfo, command.Value.GetPermissionNode())) {
						SDTM.PlayerUtils.SendChatMessageAs (_cInfo, COLORS.AQUA+"!"+command.Key+COLORS.DEFAULT+" - "+command.Value.GetDescription(), COLORS.ColorModName("Help"));
					}
				}
			} else {
				SDTM.PlayerUtils.SendChatMessageAs (_cInfo, COLORS.ColorError(CommandUtils.MSG.DENIED), COLORS.ColorModName("Server"));
			}
		}
	}
}

