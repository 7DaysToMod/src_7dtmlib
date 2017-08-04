using System;

namespace SDTM
{
	public class ConsoleCmdExpermSuper:ConsoleCmdAbstract
	{
		public override string GetDescription ()
		{
			return "Adds or removes the user from the Super Admin Group.";
		}

		public override string GetHelp ()
		{
			return "Usage:\n" +
				"/expermadmin";
		}

		public override string[] GetCommands ()
		{
			return new string[] { "expermadmin", "expermad" };
		}

		public override void Execute (System.Collections.Generic.List<string> _params, CommandSenderInfo _senderInfo)
		{
			if (_params.Count > 2) {
				SdtdConsole.Instance.Output ("[ExPermEnabled] Invalid Parameter Length");
				return;
			}


			string steamId = _senderInfo.RemoteClientInfo.playerId;

			if (_params.Count == 2) {
				steamId = PlayerUtils.GetSteamID (_params [1]);
			}

			if (SDTM.API.Permissions.toggleSuper (steamId)) {
				SdtdConsole.Instance.Output ("[ExPermAdmin] Added to Group _super");
			} else {
				SdtdConsole.Instance.Output ("[ExPermAdmin] Removed from Group _super");
			}
		}
	}
}

