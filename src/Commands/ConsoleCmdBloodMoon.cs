using System;

namespace SDTM
{
	public class ConsoleCmdBloodMoon:ConsoleCmdAbstract
	{
		public override int DefaultPermissionLevel{
			get{ 
				return 1000;
			}
		}

		public override string GetDescription ()
		{
			return "Calculates the next Blood Moon";
		}

		public override string GetHelp ()
		{
			return "Usage:\n" +
				"bloodmoon";
		}

		public override string[] GetCommands ()
		{
			return new string[] { "bloodmoon", "bm"};
		}

		public override void Execute (System.Collections.Generic.List<string> _params, CommandSenderInfo _senderInfo)
		{
			ulong time = GameManager.Instance.World.GetWorldTime ();
			int currentDay = GameUtils.WorldTimeToDays (time);

			int remainder = currentDay % 7;

			if (remainder == 0) {
				SdtdConsole.Instance.Output ("PANIC!!! Today is Horde Day");
			} else {
				int daysTillHorde = 7 - remainder;
				int hordeDay = currentDay + daysTillHorde;
				SdtdConsole.Instance.Output ("Next Bloodmoon is in " + daysTillHorde.ToString () + " day(s) on day " + hordeDay.ToString ());
			}
		}
	}
}

