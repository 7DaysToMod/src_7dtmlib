using System;

namespace SDTM
{
	public class ConsoleCmdChatCommands:ConsoleCmdAbstract
	{
		public override string GetDescription ()
		{
			return "Enables/Disables Chat Commands";
		}

		public override string GetHelp ()
		{
			return "Usage:\n" +
			"chatcommands_enabled <enabled>\n" +
			"Where:\n" +
			" - <enabled> is true or false\n";
			}

		public override string[] GetCommands ()
		{
			return new string[] { "chatcommands_enabled", "chatcommenabled"};
		}

		public override void Execute (System.Collections.Generic.List<string> _params, CommandSenderInfo _senderInfo)
		{
			if (_params.Count != 1) {
				SdtdConsole.Instance.Output ("[ChatCommandsEnabled] Invalid Parameter Length");
				return;
			}

			SDTM.API thisMod = SDTM.API.Instance;

			if (_params [0].ToLower () == "true") {
				API.ChatCommandsEnabled = true;
			} else {
				API.ChatCommandsEnabled = false;
			}

			API.Instance.SaveConfig ();

			SdtdConsole.Instance.Output ("[ChatCommandsEnabled] Settings changed to " + (API.ChatCommandsEnabled ? "Enabled" : "Disabled"));
		}
	}
}