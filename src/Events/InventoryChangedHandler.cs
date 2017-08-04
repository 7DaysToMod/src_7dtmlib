using System;

namespace SDTM
{
	public class InventoryChangedHandler:IInventoryChangedListener
	{
		private EntityPlayer player;

		public InventoryChangedHandler(EntityPlayer p){
			player = p;
		}

		public void OnInventoryChanged (Inventory _inventory){
			
			Log.Out ("AN INVENTORY HAS CHANGED: "+player.entityId);
		}
	}
}

