using System;

namespace SDTM
{
	public class InventoryGridChangeHandler: IInvGridChangedListener
	{
		private EntityPlayer player;
		public InventoryGridChangeHandler (EntityPlayer p)
		{
			player = p;
		}

		public void OnInvGridItemAdded (NGuiInvGrid _invGrid, int _idx, ItemStack _itemStack){
			Log.Out("BACKPACK ITEM ADDED:"+player.entityId);
		}

		public void OnInvGridItemRemoved (NGuiInvGrid _invGrid, int _idx, ItemStack _itemStack){
			Log.Out("BACKPACK ITEM REMOVED:"+player.entityId);
		}
	}
}

