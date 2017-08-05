using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Threading;

namespace SDTM.Data
{
	public class Region
	{
		private string owner;
		private string name;

		private Prefab selectionPrefab;
		private int rotations = 0;
		private int YOffset = 0;

		private Vector3i start;
		private Vector3i end;

		public string Owner {
			get { return owner; }
			set { owner = value; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public Vector3i Start{
			get{ return start; }
			set{ start = value; }
		}

		public Vector3i End{
			get{ return end; }
			set{ end = value; }
		}

		public Region (Vector3i _start, Vector3i _end)
		{
			this.name = "";
			this.start = _start;
			this.end = _end;
		}

		public Region (string _Name)
		{
			this.name = _Name;
		}

		public Dictionary<String, int> GetBlockList(){
			return new Dictionary<String, int> ();
		}

		public Prefab GetPrefab(){
			return this.selectionPrefab;
		}

		public bool Clear(){
			this.selectionPrefab.Clear ();
			this.selectionPrefab = null;
			return true;
		}


		public bool ExportAsPrefab(string prefabName, int yOffset){
			if (start == Vector3i.zero || end == Vector3i.zero) {
				return false;
			}

			Prefab prefabFromSelection = new Prefab ();
			prefabFromSelection.CopyFromWorld (GameManager.Instance.World, start, end);
			prefabFromSelection.bCopyAirBlocks = true;
			prefabFromSelection.yOffset = yOffset;
			prefabFromSelection.filename = prefabName;
			prefabFromSelection.Save (prefabName);

			return true;
		}

		public bool ExportAsPrefab(string prefabName){
			return ExportAsPrefab (prefabName, 0);
		}

		public bool ImportFromPrefab(string prefabName){
			selectionPrefab = new Prefab ();
			selectionPrefab.Load (prefabName);

			return true;
		}

		public int Copy(){
			if (start == Vector3i.zero || end == Vector3i.zero) {
				SdtdConsole.Instance.Output ("[WorldTool Selections] Invalid Start or End in Selection for Copy");
				return 0;
			}
			//fix the coordinates
			int x1, x2,y1,y2,z1,z2;
			if (start.x > end.x) {
				x1 = end.x;
				x2 = start.x;
			} else {
				x1 = start.x;
				x2 = end.x;
			}

			if (start.y > end.y) {
				y1 = end.y;
				y2 = start.y;
			} else {
				y1 = start.y;
				y2 = end.y;
			}

			if (start.z > end.z) {
				z1 = end.z;
				z2 = start.z;
			} else {
				z1 = start.z;
				z2 = end.z;
			}

			start = new Vector3i (x1, y1, z1);
			end = new Vector3i (x2, y2, z2);

			selectionPrefab = new Prefab ();

			selectionPrefab.CopyFromWorldWithEntities (GameManager.Instance.World, start, end, null);
			Vector3i dimensions = selectionPrefab.size;
			Log.Out ("COPIED: " + dimensions.ToString ());
			List<EntityCreationData> ecd = selectionPrefab.GetEntities();
			foreach(EntityCreationData ecdItem in ecd){
				Log.Out (ecdItem.entityName);
			}
			int totalBlocksCopied = dimensions.x * dimensions.y * dimensions.z;
			Log.Out (totalBlocksCopied.ToString ());
			return totalBlocksCopied;
		}

//		public bool Paste(EntityPlayer player){
//			return Paste (player, 0);
//		}

//		public bool Paste(EntityPlayer player, int rotations){
//			Vector3i playerPos = new Vector3i(player.position.x, player.position.y, player.position.z);
//			return Paste (playerPos);
//		}

		public Region Paste(EntityPlayer player){
			return Paste (player, 0);
		}

		public Region Paste(EntityPlayer player, int rotations=0){
			Vector3i pos = new Vector3i(player.position.x, player.position.y, player.position.z);
			return Paste (pos);
		}

		public Region Paste(EntityPlayer player, int rotations=0, int yOff=0){
			this.YOffset = yOff;
			this.rotations = rotations;
			Vector3i pos = new Vector3i(player.position.x, player.position.y, player.position.z);
			return Paste (pos);
		}

		public Region Paste(Vector3i pos){
			int width = selectionPrefab.size.x;
			int height = selectionPrefab.size.y;
			int length = selectionPrefab.size.z;
			Vector3i cloneEnd = new Vector3i (pos.x + width, pos.y + height, pos.z + length);
			FixSelections ();

			Region backup = new Region (pos,cloneEnd);
			backup.Copy ();
			PasteSelection (pos);
			return backup;

		}

		public bool PasteSelection(Vector3i pos){

			int width = selectionPrefab.size.x;
			int height = selectionPrefab.size.y;
			int length = selectionPrefab.size.z;

			this.start = pos;
			this.end = new Vector3i (pos.x + width, pos.y + height, pos.z + length);
			FixSelections ();

			Dictionary<long, Chunk> usedChunks = new Dictionary<long, Chunk>();


			for (int xOffset = -1; xOffset < width+2; xOffset++) {
				for (int zOffset = -1; zOffset < length+2; zOffset++) {
					for (int yOffset = -1; yOffset < height+2; yOffset++) {
						if (GameManager.Instance.World.IsChunkAreaLoaded (pos.x + xOffset, pos.y + yOffset, pos.z + zOffset)) {
							Chunk usedChunk = GameManager.Instance.World.GetChunkFromWorldPos (pos.x + xOffset, pos.y + yOffset, pos.z + zOffset) as Chunk;
							if (!usedChunks.ContainsKey(usedChunk.Key)) {
								usedChunks [usedChunk.Key] = usedChunk;
							}
						} else {
							//SdtdConsole.Instance.Output ("[WorldTool Selections] Prefab will use chunks that are not Loaded.  Aborting Paste.");
							return false;
						}
					}
				}
			}

			Prefab prefabClone = selectionPrefab.Clone ();
			System.Random rnd = new System.Random ();
			for (int blockX = 0; blockX < width; blockX++) {
				for (int blockY = 0; blockY < height; blockY++) {
					for (int blockZ = 0; blockZ < length; blockZ++) {
						BlockValue currentBV = prefabClone.GetBlock (blockX, blockY, blockZ);
						if (currentBV.type != 0) {
							BlockValue lootBV = LootContainer.lootPlaceholderMap.Replace (currentBV, rnd);
							if (lootBV.type != currentBV.type) {
								prefabClone.SetBlock (blockX, blockY, blockZ, lootBV);
							}
						}
					}
				}
			}


			bool currentPhysicsState = GameManager.bPhysicsActive;
			GameManager.bPhysicsActive = false;
			if (selectionPrefab.yOffset != 0) {
				pos.y += selectionPrefab.yOffset;
			}

			if (this.YOffset != 0) {
				pos.y += this.YOffset;
			}

			if (this.rotations > 0) {
				prefabClone.RotateY (false, this.rotations);
			}

			prefabClone.CopyIntoLocal(GameManager.Instance.World.ChunkClusters[0], new Vector3i(pos.x, pos.y, pos.z), true, true);
			GameManager.bPhysicsActive = currentPhysicsState;

			//Thread.Sleep(50);

			Log.Out("PASTING PREFAB");
			Log.Out (pos.ToString ());

			StabilityInitializer stabInit = new StabilityInitializer (GameManager.Instance.World);

			for(int yOffset = -1; yOffset<height+2; yOffset++){
				for (int xOffset = -1; xOffset < width+2; xOffset++) {
					for (int zOffset = -1; zOffset < length+2; zOffset++) {
						BlockValue block = GameManager.Instance.World.GetBlock (pos.x + xOffset, yOffset, pos.z + zOffset);
						if (block.type != 0) {
							stabInit.BlockPlacedAt (pos.x + xOffset, pos.y + yOffset, pos.z + zOffset, block);
						}
					}
				}
			}

			Dictionary<String, List<Chunk>> playerChunksToReload = new Dictionary<String, List<Chunk>>();
			foreach (Chunk currentChunk in usedChunks.Values) {
				//currentChunk.RepairDensities ();
				foreach (string playerSteamId in GameManager.Instance.persistentPlayers.Players.Keys) {
					ClientInfo connectedPlayer = ConsoleHelper.ParseParamIdOrName (playerSteamId);
					//TODO: for efficieny, we should probably add a check to see if the player is in range
					if (connectedPlayer != null) {
						List<Chunk> playerChunks;

						if (!playerChunksToReload.ContainsKey (playerSteamId)) {
							playerChunks = new List<Chunk> ();	
						} else {
							playerChunks = playerChunksToReload [playerSteamId];
						}

						playerChunks.Add (currentChunk);
						playerChunksToReload.Remove (playerSteamId);
						playerChunksToReload.Add (playerSteamId, playerChunks);
					}
				}
			}

			//send chunk remove messages
			foreach (string playerSteamId in playerChunksToReload.Keys) {
				ClientInfo connectedPlayer = ConsoleHelper.ParseParamIdOrName (playerSteamId);

				List<Chunk> playerChunks = playerChunksToReload [playerSteamId];
				if (playerChunks != null && playerChunks.Count > 0) {
					foreach (Chunk currentChunk in playerChunks) {
						connectedPlayer.SendPackage (new NetPackageChunkRemove (currentChunk.Key));
					}
				}
			}

			//send chunk add messages
			foreach (string playerSteamId in playerChunksToReload.Keys) {
				ClientInfo connectedPlayer = ConsoleHelper.ParseParamIdOrName (playerSteamId);

				List<Chunk> playerChunks = playerChunksToReload [playerSteamId];
				if (playerChunks != null && playerChunks.Count > 0) {
					foreach (Chunk currentChunk in playerChunks) {
						connectedPlayer.SendPackage (new NetPackageChunk (currentChunk));
					}
				}
			}

			return true;
		}

		public bool RotateOnY(int rotations){
			/*if (start == Vector3i.zero || end == Vector3i.zero) {
				SdtdConsole.Instance.Output ("[WorldTool Selections] Invalid Start or End in Selection for RotateOnY");
				return false;
			}*/
			if (selectionPrefab == null) {
				Copy ();
			}

			bool rotateLeft = false;//rotations < 0;

			int numRotations = System.Math.Abs(rotations);
            
			selectionPrefab.RotateY (rotateLeft, numRotations);
			

			return true;
		}

		public int Replace(BlockValue findBlock, BlockValue replaceBlock){
			if (start == Vector3i.zero || end == Vector3i.zero) {
				SdtdConsole.Instance.Output ("[WorldTool Replace] Invalid Start or End in Selection for Replace");
				return 0;
			}

			Prefab tempPrefab = new Prefab ();
			tempPrefab.CopyFromWorld (GameManager.Instance.World, start, end);

			return 0;
		}

		public Region Fill (string blockNameOrId){
			return Fill (blockNameOrId, 0);
		}

		public Region Fill(string blockNameOrId, int rotations){
			Region backup = new Region (start,end);
			backup.Copy ();
			FillSelection (blockNameOrId, rotations);
			return backup;
		}

		public bool FillSelection(string blockNameOrId){
			return FillSelection (blockNameOrId, 0);
		}

		public bool FillSelection(string blockNameOrId, int rotation){
			BlockValue bv = BlockUtils.GetBlockValueByNameOrId (blockNameOrId);

			FixSelections ();

			int width = (end.x - start.x) + 1;
			int height = (end.y - start.y) + 1;
			int depth = (end.z - start.z) + 1;

			Prefab tempPrefab = new Prefab (new Vector3i(width, height, depth));
			Log.Out ("SETTING BLOCK FOR FILL:"+bv.ToItemValue ().type.ToString ());
			Log.Out (tempPrefab.size.ToString ());
			for (var x = 0; x < tempPrefab.size.x; x++) {
				for (var y = 0; y < tempPrefab.size.y; y++) {
					for (var z = 0; z < tempPrefab.size.z; z++) {
						tempPrefab.SetBlock (x, y, z, bv);
					}
				}
			}

			this.selectionPrefab = tempPrefab;
			return PasteSelection (new Vector3i(start.x, start.y, start.z));
		}

		public Dictionary<string, int> GetStatistics(){
			Prefab tempPrefab = new Prefab ();
			Dictionary<string, int> stats = new Dictionary<string, int> ();
			tempPrefab.CopyFromWorld (GameManager.Instance.World, start, end);

			Prefab.BlockStatistics blockStats = tempPrefab.GetBlockStatistics ();

			stats ["doors"] = blockStats.cntDoors;
			stats ["windows"] = blockStats.cntWindows;
			stats ["blockmodels"] = blockStats.cntBlockModels;
			stats ["blockenities"] = blockStats.cntBlockEntities;
			stats ["solidblocks"] = blockStats.cntSolid;
			stats ["width"] = tempPrefab.size.x;
			stats ["length"] = tempPrefab.size.z;
			stats ["height"] = tempPrefab.size.y;

			tempPrefab.Clear ();
			tempPrefab = null;
			return stats;
		}

		public Dictionary<long, Chunk> GetUsedChunks(Vector3i playerPos){

			int width = selectionPrefab.size.x;
			int height = selectionPrefab.size.y;
			int length = selectionPrefab.size.z;

			Dictionary<long, Chunk> usedChunks = new Dictionary<long, Chunk>();

			for (int xOffset = -1; xOffset < width+2; xOffset++) {
				for (int zOffset = -1; zOffset < length+2; zOffset++) {
					for (int yOffset = -1; yOffset < height+2; yOffset++) {
						Chunk usedChunk = GameManager.Instance.World.GetChunkFromWorldPos (playerPos.x + xOffset, playerPos.y + yOffset, playerPos.z + zOffset) as Chunk;
						if (!usedChunks.ContainsKey(usedChunk.Key)) {
							usedChunks [usedChunk.Key] = usedChunk;
						}
					}
				}
			}

			return usedChunks;
		}

		public bool FixStability(Vector3i playerPos){
			if (start == Vector3i.zero || end == Vector3i.zero) {
				return false;
			}
			if (selectionPrefab == null) {
				if (Copy () < 1) {
					Log.Out ("Could not Copy Selection");
					return false;
				}
			}

			int startX, startY, startZ;

			if (start.x < end.x) {
				startX = start.x;
			} else {
				startX = end.x;
			}

			if (start.y < end.y) {
				startY = start.y;
			} else {
				startY = end.y;
			}

			if (start.z < end.z) {
				startZ = start.z;
			} else {
				startZ = end.z;
			}

			int width = selectionPrefab.size.x;
			int height = selectionPrefab.size.y;
			int length = selectionPrefab.size.z;

			StabilityInitializer stabInit = new StabilityInitializer (GameManager.Instance.World);

			for(int yOffset = -1; yOffset<height+2; yOffset++){
				for (int xOffset = -1; xOffset < width+2; xOffset++) {
					for (int zOffset = -1; zOffset < length+2; zOffset++) {
						BlockValue block = GameManager.Instance.World.GetBlock (startX + xOffset, startY+yOffset, startZ + zOffset);
						if (block.type != 0) {
							stabInit.BlockPlacedAt (startX + xOffset, startY + yOffset, startZ + zOffset, block);
						}
					}
				}
			}

			return true;
		}

		public bool FixEntities(int blockID=-1){

			if (selectionPrefab == null) {
				if (Copy () < 1) {
					Log.Out ("Could not Copy Selection");
					return false;
				}
			}

			int width = selectionPrefab.size.x;
			int height = selectionPrefab.size.y;
			int length = selectionPrefab.size.z;

			int startX, startY, startZ;

			if (start.x < end.x) {
				startX = start.x;
			} else {
				startX = end.x;
			}

			if (start.y < end.y) {
				startY = start.y;
			} else {
				startY = end.y;
			}

			if (start.z < end.z) {
				startZ = start.z;
			} else {
				startZ = end.z;
			}

			Dictionary<int, BlockValue> blockList = new Dictionary<int, BlockValue> ();

			for (int xOffset = -1; xOffset < width+2; xOffset++) {
				for (int zOffset = -1; zOffset < length+2; zOffset++) {
					for (int yOffset = -1; yOffset < height+2; yOffset++) {

						BlockValue bv = GameManager.Instance.World.GetBlock (startX + xOffset, startY + yOffset, startZ + zOffset);
						if (bv.type != BlockValue.Air.type) {
							if (!blockList.ContainsKey (bv.type)) {
								blockList [bv.type] = bv;
							}

							if (bv.type==blockID || bv.type == 1455 || bv.type == 588 || bv.type == 579) {
								BlockUtils.SetBlock (startX + xOffset, startY + yOffset, startZ + zOffset, "air");
							}
						}
					}
				}
			}

			foreach (int blockType in blockList.Keys) {
				SdtdConsole.Instance.Output (blockType.ToString () + " - " + Block.list [blockType].GetBlockName ());
			}

			return true;
		}

		private void FixSelections(){
			int startX, endX, startY, endY, startZ, endZ;

			if (start.x < end.x) {
				startX = start.x;
				endX = end.x;
			} else {
				startX = end.x;
				endX = start.x;
			}

			if (start.y < end.y) {
				startY = start.y;
				endY = end.y;
			} else {
				startY = end.y;
				endY = start.y;
			}

			if (start.z < end.z) {
				startZ = start.z;
				endZ = end.z;
			} else {
				startZ = end.z;
				endZ = start.z;
			}

			start.x = startX;
			start.y = startY;
			start.z = startZ;

			end.x = endX;
			end.y = endY;
			end.z = endZ;
		}

		public bool Write(StreamWriter sw){
			sw.WriteLine ("<region name=\""+name+"\" start=\""+start.x.ToString()+","+start.y.ToString()+","+start.z.ToString()+"\" end=\""+end.x.ToString()+","+end.y.ToString()+","+end.z.ToString()+"\">");

			sw.WriteLine ("</region>");

			return true;
		}

		public bool Read(XmlNode regionNode){
			string nameStr = regionNode.Attributes.GetNamedItem ("name").Value;

			string startPosStr = regionNode.Attributes.GetNamedItem ("start").Value;
			string endPosStr = regionNode.Attributes.GetNamedItem ("end").Value;
			string[] splitter = { "," };
			string[] startParts = startPosStr.Split (splitter, StringSplitOptions.RemoveEmptyEntries);
			string[] endParts = endPosStr.Split (splitter, StringSplitOptions.RemoveEmptyEntries);

			int sX, sY, sZ, eX, eY, eZ;
			if (int.TryParse (startParts [0], out sX)) {
				if (int.TryParse (startParts [1], out sY)) {
					if (int.TryParse (startParts [2], out sZ)) {
						if (int.TryParse (endParts [0], out eX)) {
							if (int.TryParse (endParts [1], out eY)) {
								if (int.TryParse (endParts [2], out eZ)) {
									start = new Vector3i (sX, sY, sZ);
									end = new Vector3i (eX, eY, eZ);

									return true;
								}
							}
						}
					}
				}
			}

			return false;
		}
	}
}

