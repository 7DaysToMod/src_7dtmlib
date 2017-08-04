using System;

namespace SDTM
{
	public class ConsoleCmdSetConf:ConsoleCmdAbstract
	{
		public override string GetDescription ()
		{
			return "Sets a Game Configuration.  These will override Game Preferences set in serverconfig.xml.";
		}

		public override string GetHelp ()
		{
			return "Usage:\n" +
				"/setconf conf_name value\n"+
				"Conf Names:\n"+
				" - worldtype: \"navezgane\" or \"random\" (overrides GamePrefs.GameWorld)\n"+
				" - worldname: Your game seed name (overrides GamePrefs.GameName)\n";
		}

		public override string[] GetCommands ()
		{
			return new string[] { "setconf"};
		}

		public override void Execute (System.Collections.Generic.List<string> _params, CommandSenderInfo _senderInfo)
		{
			if (_params.Count != 2) {
				SdtdConsole.Instance.Output ("[SetConf] Invalid Parameter Length");
				return;
			}

			SDTM.API thisMod = SDTM.API.Instance;
			API.OverrideGamePrefs = true;
			API.Instance.SaveConfig ();
			if (thisMod.SetConfig (_params [0], _params [1], true)) {
				SdtdConsole.Instance.Output ("[SetConf] Config Saved");
			} else {
				SdtdConsole.Instance.Output ("[SetConf] Could not set config value");
			}
		}
	}
}

