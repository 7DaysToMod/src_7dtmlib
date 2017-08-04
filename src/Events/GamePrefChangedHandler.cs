using System;

namespace SDTM
{
	public class GamePrefChangedHandler: IGamePrefsChangedListener
	{
		public void OnGamePrefChanged (EnumGamePrefs _enum){
			API.Events.NotifyGamePrefsChangedHandlers(_enum);
		}
	}
}

