

using System;

namespace SDTM
{
	public class ConsoleCmdIntegratedWeb:ConsoleCmdAbstract
	{
		public override string GetDescription ()
		{
			return "Sets configuration for the Integrated Web Server";
		}

		public override string GetHelp ()
		{
			return "Usage:\n" +
				"integratedweb <enabled> <port>\n"+
				"Where:\n"+
				" - <enabled> is true or false\n"+
				" - <port> is the port to run the web server on\n";
		}

		public override string[] GetCommands ()
		{
			return new string[] { "integratedweb", "intweb"};
		}

		public override void Execute (System.Collections.Generic.List<string> _params, CommandSenderInfo _senderInfo)
		{
			if (_params.Count != 2) {
				SdtdConsole.Instance.Output ("[IntegratedWeb] Invalid Parameter Length");
				return;
			}

			SDTM.API thisMod = SDTM.API.Instance;

			if (_params [0].ToLower () == "true") {
				API.WebConsoleEnabled = true;
				API.WebConsolePort = _params[1];
			} else {
				API.WebConsoleEnabled = false;
			}
			API.Instance.SaveConfig ();
			SdtdConsole.Instance.Output ("[IntegratedWeb] Settings changed to " + (API.WebConsoleEnabled ? "Enabled on port " + API.WebConsolePort : "Disabled"));
		}
	}
}



