using System;

namespace SDTM
{
	public static class WorldUtils
	{
		public static void SetTime(int day, int hour, int minute){
			ulong num = GameUtils.DayTimeToWorldTime (day, hour, minute);
			GameManager.Instance.World.worldTime = num;
			GameManager.Instance.World.aiDirector.BloodMoonComponent.TimeSeek ();
		}
	}
}

