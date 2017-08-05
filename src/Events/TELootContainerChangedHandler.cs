using System;
using System.Collections.Generic;

namespace SDTM
{
	public class TELootContainerChangedHandler:ITileEntityChangedListener
	{
		private ulong lastTouched;
		//private float lastOpened;

		KeyValuePair<ItemValue, int>[] items;

		public TELootContainerChangedHandler (TileEntityLootContainer lc)
		{
			SetHistory (lc);
		}

		private void SetHistory(TileEntityLootContainer lc){
			if (lc != null) {
				lastTouched = lc.worldTimeTouched;
				//lastOpened = lc.GetOpenTime();

				if (lc.items != null) {
					items = new KeyValuePair<ItemValue, int>[lc.items.Length];
					ItemStack[] itemList = lc.GetItems ();
					if (itemList != null) {
						for (int i = 0; i < itemList.Length; i++) {
							ItemStack item = (ItemStack)itemList[i];
							if (item != null) {
								if (item.itemValue != null) {
									ItemValue iv = item.itemValue.Clone ();
									int itemCount = item.count;
									KeyValuePair<ItemValue, int> kvp = new KeyValuePair<ItemValue, int> (iv, itemCount);
									items [i] = kvp;
								}
							}
						}
					}
				}
			}
		}

		public void OnTileEntityChanged (TileEntity _te, int _dataObject){
			if (_te == null) {
				return;
			}
			if (_te is TileEntityLootContainer) {
				TileEntityLootContainer lc = _te as TileEntityLootContainer;

				bool isChanged = false;

				/*if (lc.worldTimeTouched != lastTouched) {
					SDTM.API.Events.NotifyLootContainerTouchedChangedHandlers (lc, lastTouched, lc.worldTimeTouched);
					isChanged = true;
				}*/

//				if (lastOpened != lc.GetOpenTime ()) {
//					SDTM.API.Events.NotifyLootContainerOpenedTimeChangedHandlers (lc, lastOpened, lc.GetOpenTime ());
//					isChanged = true;
//				}

				ItemStack[] itemList = lc.GetItems ();
				for (int i = 0; i < itemList.Length; i++) {
					ItemStack item = (ItemStack)itemList.GetValue (i);
					ItemValue iv = item.itemValue.Clone ();

					if (iv.type != items [i].Key.type || item.count!=items[i].Value) {
						ItemStack oldStack = new ItemStack (items [i].Key, items [i].Value);
						SDTM.API.Events.NotifyLootContainerItemChangedHandlers (lc, i, oldStack, item);
						isChanged = true;
					}
				}

				if (isChanged) {
					SetHistory (lc);
				}
			}
		}
	}
}

