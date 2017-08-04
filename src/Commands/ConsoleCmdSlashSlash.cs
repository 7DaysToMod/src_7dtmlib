using System;

namespace SDTM
{
	public class ConsoleCmdSlashSlash: ConsoleCmdAbstract
	{
		public override string[] GetCommands ()
		{
			return new string[]{"/"};
		}

		public override string GetDescription ()
		{
			return "Bleh";
		}

		public override string GetHelp ()
		{
			return "HELP!!!!";
		}

		public override void Execute (System.Collections.Generic.List<string> _params, CommandSenderInfo _senderInfo)
		{
			SdtdConsole.Instance.Output ("YO YO");
		}
	}
}

