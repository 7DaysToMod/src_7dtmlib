using System;
using System.Collections.Generic;
using System.Xml;

namespace SDTM
{
	public class ItemUtils
	{
		private static ItemUtils instance;

		public static ItemUtils Instance {
			get {
				if (instance == null) {
					instance = new ItemUtils ();
				}
				return instance;
			}
		}

		private ItemUtils ()
		{
			Refresh ();
		}

		private SortedDictionary<string, ItemValue> items = new SortedDictionary<string, ItemValue> ();

		public List<string> ItemNames {
			get { return new List<string> (items.Keys); }
		}

		public static string GetItemName(int itemId){
			if (itemId == 0) {
				return "";
			}
			//Log.Out ("GETTING ITEM NAME FOR: " + itemId.ToString ());
			ItemClass _itemClass = ItemClass.list [itemId];

			return _itemClass.GetItemName ();
		}

		public string GetItemName(ItemValue item){
			return GetItemName (item.type);
		}

		public ItemValue GetItemValue (string itemName)
		{
			if (items.ContainsKey (itemName)) {
				return items [itemName].Clone ();
			} else {
				itemName = itemName.ToLower ();
				foreach (KeyValuePair<string, ItemValue> kvp in items) {
					if (kvp.Key.ToLower ().Equals (itemName)) {
						return kvp.Value.Clone ();
					}
				}
				return null;
			}
		}

		public void Refresh ()
		{
			NGuiInvGridCreativeMenu cm = new NGuiInvGridCreativeMenu ();
			foreach (ItemStack invF in cm.GetAllItems()) {
				ItemClass ib = ItemClass.list [invF.itemValue.type];

				string name = ib.GetItemName ();
				if (name != null && name.Length > 0) {
					if (!items.ContainsKey (name)) {
						items.Add (name, invF.itemValue);
					} else {
						//Log.Out ("Item \"" + name + "\" already in list!");
					}
				}
			}

			foreach (ItemStack invF in cm.GetAllBlocks()) {
				ItemClass ib = ItemClass.list [invF.itemValue.type];
				string name = ib.GetItemName ();
				if (name != null && name.Length > 0) {
					if (!items.ContainsKey (name)) {
						items.Add (name, invF.itemValue);
					} else {
						//Log.Out ("Item \"" + name + "\" already in list!");
					}
				}
			}

			//Log.Out (items.Count + " items loaded");
		}

		public static ItemStack GetItemStack(EntityItem _item){
			return _item.itemStack;
		}

	}
}

