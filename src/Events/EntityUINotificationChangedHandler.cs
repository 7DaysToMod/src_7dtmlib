using System;

namespace SDTM
{
	public class EntityUINotificationChangedHandler:IEntityUINotificationChanged
	{
		public void EntityUINotificationAdded (EntityUINotification _notification){
			//Log.Out("UI NOTIFICATION ADDED");
		}

		public void EntityUINotificationRemoved (EntityUINotification _notification){
			//Log.Out("UI NOTIFICATION REMOVED");
		}
	}
}

