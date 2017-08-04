using System;

namespace SDTM
{
	public class ConsoleCmdSetTeam: ConsoleCmdAbstract
	{
		public override string GetDescription ()
		{
			return "Sets the team for a Player";
		}

		public override string GetHelp ()
		{
			return "Usage: /teamset <playername> <teamnumber>";
		}

		public override string[] GetCommands ()
		{
			return new string[]{ "teamset" };
		}

		public override void Execute (System.Collections.Generic.List<string> _params, CommandSenderInfo _senderInfo)
		{
			if (_params.Count != 2) {
				SdtdConsole.Instance.Output ("Invalid parameter Count");
				return;
			}

			int teamId = 0;
			if (int.TryParse (_params [1], out teamId)) {

				if (teamId <= Constants.cNumberOfTeams) {
					SDPlayer p = PlayerUtils.GetPlayer (_params [0]);
					if (p != null) {
						p.SetTeam (teamId);
						SdtdConsole.Instance.Output ("Player Team Set");
						return;
					} else {
						SdtdConsole.Instance.Output ("Player not Found: " + _params [0]);
					}
				} else {
					SdtdConsole.Instance.Output ("Invalid team Number: "+_params[1]+"(expected 0 -> "+(Constants.cNumberOfTeams-1).ToString()+")");
				}
			}

			SdtdConsole.Instance.Output ("Could not set player Team");
		}
	}
}

