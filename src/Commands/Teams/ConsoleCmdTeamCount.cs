using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDTM
{
	public class ConsoleCmdTeamCount:ConsoleCmdAbstract
	{

		private static string[] TEAMNAMES = { "NO TEAM", "TEAM 1", "TEAM 2", "TEAM 3", "TEAM 4", "TEAM 5", "TEAM 6", "TEAM 7", "TEAM 8", "TEAM 9", "TEAM 10", "TEAM 11", "TEAM 12", "TEAM 13", "TEAM 14", "TEAM 15",};
		//private static string[] TEAMSKINS = { };
		private static string[] TEAMCOLORS = { COLORS.HEX.WHITE, COLORS.HEX.BLUE, COLORS.HEX.RED, COLORS.HEX.GREEN, COLORS.HEX.YELLOW, COLORS.HEX.CYAN, COLORS.HEX.MAGENTA, COLORS.HEX.LIME, COLORS.HEX.OLIVE, COLORS.HEX.NAVY, COLORS.HEX.MAROON, COLORS.HEX.GRAY, COLORS.HEX.PURPLE, COLORS.HEX.TEAL, COLORS.HEX.SILVER, COLORS.HEX.BLACK};

		public override string GetDescription ()
		{
			return "Sets the number of Teams";
		}

		public override string GetHelp ()
		{
			return "Sets the number of Teams\nUsage:\n/teamcount <max_teams>";
		}

		public override string[] GetCommands ()
		{
			return new string[]{"teamcount"};
		}

		public override void Execute (List<string> _params, CommandSenderInfo _senderInfo)
		{
			DoExec (_params, _senderInfo);
		}

		private void DoExec(List<string> _params, CommandSenderInfo _senderInfo){
			int maxCount = 2;
			if(int.TryParse(_params[0], out maxCount)){
				if (maxCount <= 15) {
					ClearPlayerTeams ();
					SetupTeams (maxCount);
				} else {
					SdtdConsole.Instance.Output ("15 Teams Max");
				}

			}
		}

		private void ClearPlayerTeams(){
			List<SDPlayer> onlinePlayers = PlayerUtils.GetOnlinePlayers ();

			foreach (SDPlayer p in onlinePlayers) {
				p.SetTeam (0);
			}
		}

		private void SetupTeams(int teamCount){
			string skinName = Constants.cTeamSkinName [0];

			List<string> TeamNames = new List<string>();
			List<string> TeamSkins = new List<string> ();
			List<Color> TeamColors = new List<Color> ();

			for (int i = 0; i <= teamCount; i++) {
				TeamNames.Add (TEAMNAMES[i]);
				TeamSkins.Add (skinName);
				TeamColors.Add (COLORS.HexToColor(TEAMCOLORS[i]));
			}
				
			Constants.cTeamName = TeamNames.ToArray ();
			Constants.cTeamSkinName = TeamSkins.ToArray ();
			Constants.cTeamColors = TeamColors.ToArray ();
			Constants.cNumberOfTeams = teamCount + 1;

			SdtdConsole.Instance.Output ("Team Count Set");
		}
	}
}

