using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDTM
{
	public class BlockUtils
	{
		private static BlockUtils instance;
		//private static Dictionary<string, string> blockMap = new Dictionary<string, string>();
		//private static bool mapLoaded = false;

		public static BlockUtils Instance {
			get {
				if (instance == null) {
					instance = new BlockUtils ();
				}
				return instance;
			}
		}

		private BlockUtils ()
		{
			
		}

		public static BlockValue GetBlockValueByNameOrId(string idOrName){
			BlockValue bv = new BlockValue();

			int blockId;

			if(int.TryParse(idOrName, out blockId)){//checkById
				foreach(Block b in Block.list){
					if(b.blockID==blockId){
						bv = Block.GetBlockValue (b.GetBlockName ());
						break;
					}
				}

			}else{ //checkByName
				bv = Block.GetBlockValue (idOrName);
			}
			Log.Out (bv.ToString ());
			return bv;
		}

		public static BlockValue GetBlock(int _x, int _y, int _z){
			
			Vector3i vec = new Vector3i (_x, _y, _z);
			return GetBlock (vec);
		}

		public static BlockValue GetBlock(Vector3i pos){
			World w = GameManager.Instance.World;
			if (w == null) {
				return BlockValue.Air;
			}

			BlockValue? bvn = GameManager.Instance.World.GetBlock (pos);
			BlockValue bv;
			if (bvn == null) {
				return BlockValue.Air;
			} else {
				bv = (BlockValue) bvn;
				return bv;
			}
		}

		public static bool SetBlock(int _x, int _y, int _z, string blockNameOrId, int rotation=0){

			Vector3i pos = new Vector3i (_x, _y, _z);

			return SetBlock (pos, blockNameOrId, rotation);
		}

		public static bool SetBlock(Vector3i pos, string blockNameOrId, int rotation=0){
			List<BlockChangeInfo> changes = new List<BlockChangeInfo> ();
			BlockValue bv;
			if(blockNameOrId.ToLower().Equals("air") || blockNameOrId.ToLower().Equals("0")){
				bv = new BlockValue (0);
			}else{
				ItemValue iv = ItemUtils.Instance.GetItemValue (blockNameOrId);
				if (iv == null) {
					Log.Out ("Could not Get Block ItemValue");
					return false;
				}

				bv = new BlockValue ((uint) iv.type);
			}

			bv.rotation = Convert.ToByte(rotation);

			BlockChangeInfo bci = new BlockChangeInfo (pos, bv, true, false);
			changes.Add (bci);
			GameManager.Instance.SetBlocksRPC (changes);

			return true;
		}


		public static string GetBlockName(BlockValue blockValue){
			ItemValue itemValue = blockValue.ToItemValue();

			if (itemValue == null) {
				return "air";
			}

			if (itemValue.type == 0) {
				return "air";
			}

			ItemClass itemClass = ItemClass.list [itemValue.type];
			if (itemClass == null) {
				return "air";
			}

			return itemClass.GetItemName ();
		}

//		public void ExportBlockTextures(){
//			
//			foreach (UISpriteData data in atlas.spriteList) {
//				string name = data.name;
//				Texture2D tex = new Texture2D (data.width, data.height, TextureFormat.ARGB32, false);
//				tex.SetPixels (atlasTex.GetPixels (data.x, atlasTex.height - data.height - data.y, data.width, data.height));
//				byte[] pixData = tex.EncodeToPNG ();
//				File.WriteAllBytes (exportPath + "/" + name + ".png", pixData);
//
//				UnityEngine.Object.Destroy (tex);
//			}
//		}

	}
}

