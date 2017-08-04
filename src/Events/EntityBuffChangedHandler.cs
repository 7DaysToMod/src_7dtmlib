using System;

namespace SDTM
{
	public class EntityBuffChangedHandler:IEntityBuffsChanged
	{
		private EntityPlayer _player;

		public EntityBuffChangedHandler(EntityPlayer player){
			_player = player;
		}

		public void EntityBuffAdded(Buff _buff){
			API.Events.NotifyPlayerBuffAddedHandlers (_player, _buff);
		}

		public void EntityBuffRemoved(Buff _buff){
			API.Events.NotifyPlayerBuffRemovedHandlers(_player, _buff);
		}
	}
}

