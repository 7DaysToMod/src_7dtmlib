using System;
using System.Collections.Generic;

namespace SDTM
{
	public class TENPCChangedHandler:ITileEntityChangedListener
	{
		private List<ItemStack> lastPrimaryInventory = new List<ItemStack>();
		private List<ItemStack[]> lastTierGroupItems = new List<ItemStack[]>();

		List<ItemStack> PrimaryInventory = new List<ItemStack>();
		List<ItemStack[]> TierGroupItems = new List<ItemStack[]>();

		public TENPCChangedHandler(TileEntityTrader _te){
			SetHistory (_te);
		}

		public void OnTileEntityChanged (TileEntity _te, int _dataObject){
			if (_te == null) {
				return;
			}

			TileEntityTrader npc = _te as TileEntityTrader;

			bool primaryChanged = false;
			bool stashChanged = false;

			//check for inventory changes
			if (npc.TraderData.PrimaryInventory.Count != this.PrimaryInventory.Count) {
				//certainly have a change
				primaryChanged = true;
			} else {
				//check to make sure everything is still the same
				for (int i = 0; i < npc.TraderData.PrimaryInventory.Count; i++) {
					ItemStack oldItem = this.PrimaryInventory [i];
					ItemStack currentItem = npc.TraderData.PrimaryInventory [i];
					if (oldItem.itemValue.type != currentItem.itemValue.type || oldItem.count != currentItem.count) {
						primaryChanged = true;
						break;
					}
				}
			}

			if (npc.TraderData.TierItemGroups.Count != this.TierGroupItems.Count) {
				stashChanged = true;
			} else {
				for (int grp = 0; grp < npc.TraderData.TierItemGroups.Count; grp++) {
					ItemStack[] oldItemGroup = this.TierGroupItems [grp];
					ItemStack[] itemGroup = npc.TraderData.TierItemGroups [grp];
					for (int i = 0; i < itemGroup.Length; i++) {
						if (oldItemGroup [i].itemValue.type != itemGroup [i].itemValue.type || oldItemGroup [i].count != itemGroup [i].count) {
							stashChanged = true;
							break;
						}
					}
				}
			}

			Entity e = GameManager.Instance.World.GetEntity (_te.entityId);

			if (primaryChanged) {
				API.Events.NotifyTraderPrimaryInventoryChangedHandlers (e as EntityNPC, this.lastPrimaryInventory);
			}

			if (stashChanged) {
				API.Events.NotifyTraderSecretStashChangedHandlers (e as EntityNPC, this.lastTierGroupItems);
			}

			SetHistory (npc);
			Log.Out (_te.GetTileEntityType ().ToString ()+" - "+primaryChanged);
		}

		public void SetHistory(TileEntityTrader _te){
			this.lastPrimaryInventory = this.PrimaryInventory;
			this.lastTierGroupItems = this.TierGroupItems;

			this.PrimaryInventory.Clear();
			this.TierGroupItems.Clear();

			foreach (ItemStack itemStack in _te.TraderData.PrimaryInventory) {
				this.PrimaryInventory.Add (itemStack);
			}

			foreach (ItemStack[] items in _te.TraderData.TierItemGroups) {
				this.TierGroupItems.Add (items);
			}
		}
	}
}

