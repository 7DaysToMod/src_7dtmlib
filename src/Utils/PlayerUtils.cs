using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDTM
{
	public class PlayerUtils
	{
		private static PlayerUtils instance;

		public static PlayerUtils Instance {
			get {
				if (instance == null) {
					instance = new PlayerUtils ();
				}
				return instance;
			}
		}

		private PlayerUtils ()
		{

		}

		public static List<EntityPlayer> GetOnlinePlayers(){
			List<EntityPlayer> players = new List<EntityPlayer> ();

			Dictionary<int, EntityPlayer>.Enumerator enumerator = GameManager.Instance.World.Players.dict.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				KeyValuePair<int, EntityPlayer> pair = enumerator.Current;
				EntityPlayer ep = pair.Value;

				players.Add (ep);
			}

			return players;
		}

		public static EntityPlayer GetEntityPlayer(string playerNameSteamIdOrEntityId){
			ClientInfo ci = ConsoleHelper.ParseParamIdOrName (playerNameSteamIdOrEntityId);

			return GetEntityPlayer (ci);
		}

		public static EntityPlayer GetEntityPlayer(ClientInfo ci){
			if(ci==null){
				return null;
			}

			EntityPlayer p = GameManager.Instance.World.Players.dict [ci.entityId];

			if(p == null){
				return null;
			}

			return p;
		}

		public static ClientInfo GetClientInfo(string playerNameSteamIdOrEntityId){
			return ConsoleHelper.ParseParamIdOrName (playerNameSteamIdOrEntityId);
		}

		public static ClientInfo GetClientInfo(EntityPlayer player){
			return ConsoleHelper.ParseParamEntityIdToClientInfo (player.entityId.ToString ());
		}

		public static string GetSteamID(string playerNameSteamIdOrEntityId){
			ClientInfo ci = ConsoleHelper.ParseParamIdOrName (playerNameSteamIdOrEntityId);

			if (ci == null) {
				return null;
			}

			return ci.playerId;
		}

		public static string GetDisplayName(string playerNameSteamIdOrEntityId){
			ClientInfo ci = ConsoleHelper.ParseParamIdOrName (playerNameSteamIdOrEntityId);

			if (ci == null) {
				return null;
			}

			return ci.playerName;
		}

		public static void SetPosition(ClientInfo ci, int x, int y, int z){
			Vector3 destPos = new Vector3 ();

			destPos.x = x;
			destPos.y = y;
			destPos.z = z;
			SetPosition (ci, destPos);
		}

		public static Vector3i GetBlockPosition(EntityPlayer p){
			Vector3 pos = p.GetPosition ();
			return new Vector3i (pos.x, pos.y, pos.z);
		}

		public static void SetPosition (ClientInfo ci, Vector3i destPos){
			Vector3 vec = new Vector3 (destPos.x, destPos.y, destPos.z);
			vec.x += 0.5f;
			vec.z += 0.5f;
			SetPosition (ci, vec);
		}

		public static void SetPosition (EntityPlayer p, Vector3i destPos){
			Vector3 vec = new Vector3 (destPos.x, destPos.y, destPos.z);
			vec.x += 0.5f;
			vec.z += 0.5f;
			SetPosition (p, vec);
		}

		public static void SetPosition(EntityPlayer p, Vector3 destPos){
			ClientInfo ci = PlayerUtils.GetClientInfo(p.entityId.ToString());
			SetPosition (ci, destPos);
		}

		public static void SetPosition(ClientInfo ci, Vector3 destPos){
			NetPackageTeleportPlayer pkg = new NetPackageTeleportPlayer (destPos);
			ci.SendPackage (pkg);
		}

		public static void Buff(EntityPlayer player, string buff){
			//chillOutAndDrinkBeer
			ClientInfo clientInfo = PlayerUtils.GetClientInfo(player);

			clientInfo.SendPackage (new NetPackageConsoleCmdClient ("buff "+buff, true));
		}

		public static void Debuff(EntityPlayer player, string buff){
			//chillOutAndDrinkBeer
			ClientInfo clientInfo = PlayerUtils.GetClientInfo(player);

			clientInfo.SendPackage (new NetPackageConsoleCmdClient ("debuff "+buff, true));
		}

		public static void AddQuest(EntityPlayer player, string questName){
			//chillOutAndDrinkBeer
			ClientInfo clientInfo = PlayerUtils.GetClientInfo(player);

			clientInfo.SendPackage (new NetPackageConsoleCmdClient ("givequest "+questName, true));
		}

		public static void SendChatMessageAs(ClientInfo ci, string chatString, string playerName){
			ci.SendPackage (new NetPackageGameMessage (EnumGameMessages.Chat, chatString, playerName, false, "", false));
		}

		public static void SendChatMessageToAll(ClientInfo fromClient, string message){
			GameManager.Instance.GameMessageServer (fromClient, EnumGameMessages.Chat, message, fromClient.playerName, false, "", false);
		}

		public static BlockValue GetBlockPlayerLookingAt(EntityPlayer player){
			for (int distance = 0; distance <= 100; distance++) {
				Vector3 lookPoint = player.GetLookRay ().GetPoint (distance);
				Vector3i lookVec = new Vector3i (lookPoint.x, lookPoint.y, lookPoint.z);
				BlockValue bv = BlockUtils.GetBlock (lookVec.x, lookVec.y, lookVec.z);

				if (bv.type != BlockValue.Air.type)
				{
					return bv;
				}
			}

			return BlockValue.Air;
		}

		public static Vector3i GetBlockPlayerLookingAtPos(EntityPlayer player){
			for (int distance = 0; distance <= 1000; distance++) {
				Vector3 lookPoint = player.GetLookRay ().GetPoint (distance);
				Vector3i lookVec = new Vector3i (lookPoint.x, lookPoint.y, lookPoint.z);
				BlockValue bv = BlockUtils.GetBlock (lookVec.x, lookVec.y, lookVec.z);
				if (bv.type != BlockValue.Air.type) {
					Log.Out (distance.ToString());
					return lookVec;
				}
			}

			return Vector3i.zero;
		}

		public static bool SetPlayerLevel(EntityPlayer player, int level){
			if (player == null) {
				return false;
			}

			if (level > Constants.Max_Level) {
				return false;
			}

			if (level < 1) {
				return false;
			}

			player.Level = level;
			return true;
		}

		public static PersistentPlayerData GetPPD(string playerName){
			PersistentPlayerList playerList = GameManager.Instance.GetPersistentPlayerList ();
			PersistentPlayerData ppd =  playerList.GetPlayerData (playerName);

			return ppd;
		}
	}
}

