using System;

namespace SDTM
{
	public class ConsoleCmdExpermEnabled:ConsoleCmdAbstract
	{
		public override string GetDescription ()
		{
			return "Enable/Disable ExPerm";
		}

		public override string GetHelp ()
		{
			return "Usage:\n" +
			"/expermenabled true|false";
		}

		public override string[] GetCommands ()
		{
			return new string[] { "experm_enabled", "expermen" };
		}

		public override void Execute (System.Collections.Generic.List<string> _params, CommandSenderInfo _senderInfo)
		{
			if (_params.Count != 1) {
				SdtdConsole.Instance.Output ("[ExPermEnabled] Invalid Parameter Length");
				return;
			}

			SDTM.API thisMod = SDTM.API.Instance;
			if (_params [0].ToLower () == "true") {
				SDTM.API.ExPermEnabled = true;
				thisMod.SaveConfig ();
				return;
			}

			if (_params [0].ToLower () == "false") {
				SDTM.API.ExPermEnabled = false;
				thisMod.SaveConfig ();
				return;
			}

			SdtdConsole.Instance.Output ("[ExPermEnabled] Invalid Parameter Value.  Expected 'true' or 'false'.");
		}
	}
}

