using System;

namespace SDTM
{
	public class EntityWellnessChangedHandler:IEntityWellnessChanged
	{
		private EntityPlayer _player;

		public EntityWellnessChangedHandler(EntityPlayer p){
			_player =p;
		}

		public void EntityWellnessChanged(EntityAlive _ent, float amount){
			API.Events.NotifyPlayerWellnessChangedHandlers (_player, amount);
		}
	}
}

