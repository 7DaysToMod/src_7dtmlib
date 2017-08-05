using System;
using System.Collections.Generic;

namespace SDTM
{
	//Server Event Delegates
	public delegate void OnGameStartedDelegate();
	public delegate void OnGameStoppedDelegate();

	public delegate void OnBeforeGameStartDoneDelegate();
	public delegate void OnGameStartDoneDelegate();
	public delegate void OnAfterGameStartDoneDelegate();
	public delegate void OnGameUpdateDelegate();
	public delegate void OnGameStatsChangedDelegate(string _statName, object _newValue);
	public delegate void OnGamePrefsChangedDelegate(EnumGamePrefs _enum);

	//Chunk Events
	public delegate void OnChunkColorsDoneDelegate(Chunk chunk);
	public delegate void OnChunkAddedDelegate(Chunk _chunk);
	public delegate void OnBeforeChunkRemovedDelegate(Chunk _chunk);
	public delegate void OnBeforeChunkSavedDelegate(Chunk _chunk);

	//Game Time Event Delegates
	public delegate void OnLibTickDelegate();
	public delegate void OnGameMinuteDelegate(int _day, int _hour, int _minute);
	public delegate void OnGameHourDelegate(int _day, int _hour);

	//Player Event Delegates
	public delegate void OnPlayerLoginDelegate(ClientInfo _cInfo, string _compatibilityVersion);
	public delegate void OnPlayerSpawningDelegate(EntityPlayer _player);
	public delegate void OnPlayerSpawnedDelegate(ClientInfo _cInfo, RespawnType _respawnReason, Vector3i _pos);
	public delegate void OnPlayerDisconnectedDelegate(EntityPlayer _player);
	public delegate void OnPlayerDataSavedDelegate(ClientInfo _cInfo, PlayerDataFile _playerDataFile);


	public delegate void OnPlayerDroppedItemDelegate(EntityPlayer _player, EntityItem _item);
	public delegate void OnPlayerCollectedItemDelegate(EntityPlayer _player, ItemStack _itemStack);
	public delegate void OnPlayerInventoryChanged(EntityPlayer _player, Inventory _inventory);
	public delegate void OnPlayerBuffAddedDelegate(EntityPlayer _player, Buff _buff);
	public delegate void OnPlayerBuffRemovedDelegate(EntityPlayer _player, Buff _buff);
	public delegate void OnPlayerWellnessChangedDelegate(EntityPlayer _player, float _amount);
	public delegate void OnPlayerDiedDelegate(EntityPlayer player);
	public delegate void OnPlayerKilledByPlayerDelegate(EntityPlayer _player, EntityPlayer _killer);

	public delegate void OnPersistentPlayerDataEventDelegate(PersistentPlayerData _player, PersistentPlayerData _otherPlayer, EnumPersistentPlayerDataReason _reason);

	public delegate void OnPlayerACLInviteSentDelegate(PersistentPlayerData _player, PersistentPlayerData _invitedPlayer);
	public delegate void OnPlayerACLInviteAcceptedDelegate(PersistentPlayerData _player, PersistentPlayerData _invitedPlayer);
	public delegate void OnPlayerACLInviteDeclinedDelegate(PersistentPlayerData _player, PersistentPlayerData _invitedPlayer);
	public delegate void OnPlayerACLRemovedDelegate(PersistentPlayerData _player, PersistentPlayerData _invitedPlayer);

	public delegate void OnPlayerPlacedBedDelegate(EntityPlayer player, Vector3i bedPos);
	public delegate void OnPlayerPlacedStorageDelegate(EntityPlayer player, Vector3i storagePos);

	//Chat Command Delegates
	public delegate void OnChatCommandDelegate(List<string> _params, ClientInfo _cInfo);
	public delegate void OnChatCommandEventDelegate(ClientInfo _player, string _command, string _param);

	//Entity Event Delegates
	public delegate void OnEntityLoadedDelegate(Entity _entity);
	public delegate void OnEntityUnloadedDelegate(Entity _entity, string _reason);
	public delegate void OnNPCLoaded(EntityNPC _entity);
	public delegate void OnNPCUnloaded(EntityNPC _entity, string _reason);
	public delegate void OnNPCBanditLoaded(EntityBandit _entity);
	public delegate void OnNPCBanditUnloaded(EntityBandit _entity, string _reason);
	public delegate void OnNPCSurvivorLoaded(EntitySurvivor _entity);
	public delegate void OnNPCSurvivorUnloaded(EntitySurvivor _entity, string _reason);
	public delegate void OnTraderPrimaryInventoryChanged(EntityNPC _entity, List<ItemStack> previousInventory);
	public delegate void OnTraderSecretStashChanged(EntityNPC _entity, List<ItemStack[]> previousStash);

	public delegate void OnSupplyCrateSpawnedDelgate(Entity supplyCrate);
	public delegate void OnZombieSpawnedDelegate(Entity zombie);

	//Block Event Delegates
	public delegate void OnBlockChangedDelegate(Vector3i _pos, BlockValue _oldBlock, BlockValue _newBlock);
	public delegate void OnBlockDamagedDelegate(Vector3i _blockPos, BlockValue _blockValue, int _damage, int _attackerEntityId);

	//Workstation Event Delegates

//	public delegate void OnWorkstationToolChangedDelegate(TileEntityWorkstation ws, int slot, ItemStack oldStack, ItemStack newStack);
//	public delegate void OnWorkstationInputChangedDelegate(TileEntityWorkstation ws,int slot, ItemStack oldStack, ItemStack newStack);
//	public delegate void OnWorkstationOutputChangedDelegate(TileEntityWorkstation ws,int slot, ItemStack oldStack, ItemStack newStack);
//	public delegate void OnWorkstationQueueChangedDelegate(TileEntityWorkstation ws);
//
//	public delegate void OnWorkStationFuelChangedDelegate(TileEntityWorkstation ws,int slot, ItemStack oldStack, ItemStack newStack);
//	public delegate void OnWorkstationIsActiveChangedDelegate(TileEntityWorkstation ws, bool oldState, bool newState);
//	public delegate void OnWorkstationTotalBurnTimeChangedDelegate(TileEntityWorkstation ws, float oldTime, float newTime);
//	public delegate void OnWorkstationBurnTimeChangedDelegate(TileEntityWorkstation ws, float oldTime, float newTime);
//
	//Loot Container Events
	public delegate void OnLootContainerTouchedChangedDelegate(TileEntityLootContainer lc, ulong oldValue, ulong newValue);
	public delegate void OnLootContainerOpenedTimeChangedDelegate(TileEntityLootContainer lc, float oldValue, float newValue);
	public delegate void OnLootContainerItemChangedDelegate(TileEntityLootContainer lc, int slot, ItemStack oldStack, ItemStack newStack);


	//Power Source Delegates
	public delegate void OnPoweredIsPoweredChangeDelegate(TileEntityPowered te, bool oldValue, bool newValue);
	public delegate void OnPoweredPowerUsedChangeDelegate(TileEntityPowered te, int oldValue, int newValue);
	public delegate void OnPoweredRequiredPowerChangeDelegate(TileEntityPowered te, int oldValue, int newValue);

	public delegate void OnPoweredTriggerIsTriggeredChangeDelegate(TileEntityPoweredTrigger te, bool oldValue, bool newValue);
	public delegate void OnPoweredTriggerProperty1ChangeDelegate(TileEntityPoweredTrigger te, byte oldValue, byte newValue);
	public delegate void OnPoweredTriggerProperty2ChangeDelegate(TileEntityPoweredTrigger te, byte oldValue, byte newValue);
	public delegate void OnPoweredTriggerTargetSelfChangeDelegate(TileEntityPoweredTrigger te, bool oldValue, bool newValue);
	public delegate void OnPoweredTriggerTargetAlliesChangeDelegate(TileEntityPoweredTrigger te, bool oldValue, bool newValue);
	public delegate void OnPoweredTriggerTargetStrangersChangeDelegate(TileEntityPoweredTrigger te, bool oldValue, bool newValue);
	public delegate void OnPoweredTriggerTargetZombiesChangeDelegate(TileEntityPoweredTrigger te, bool oldValue, bool newValue);

	public delegate void OnPowerSourceTurnedOnDelegate(TileEntityPowerSource te);
	public delegate void OnPowerSourceTurnedOffDelegate(TileEntityPowerSource te);
	public delegate void OnPowerSourceMaxOutputChanged(TileEntityPowerSource te, int oldValue, int newValue);
	public delegate void OnPowerSourceCurrentFuelChanged(TileEntityPowerSource te, int oldValue, int newValue);

	public class Events
	{
		/* -------------------------------------------------------------
		 * Events
		 * -------------------------------------------------------------
		 */

		//Server Event Handlers
		public event OnGameStartedDelegate OnGameStarted;
		public event OnGameStoppedDelegate OnGameStopped;
		public event OnBeforeGameStartDoneDelegate OnBeforeGameStartDone;
		public event OnGameStartDoneDelegate OnGameStartDone;
		public event OnAfterGameStartDoneDelegate OnAfterGameStartDone;
		public event OnGameUpdateDelegate OnGameUpdate;
		public event OnGameStatsChangedDelegate OnGameStatsChanged;
		public event OnGamePrefsChangedDelegate OnGamePrefsChanged;

		//Chunk Events
		public event OnChunkColorsDoneDelegate OnChunkColorsDone;
		public event OnChunkAddedDelegate OnChunkAdded;
		public event OnBeforeChunkRemovedDelegate OnBeforeChunkRemoved;
		public event OnBeforeChunkSavedDelegate OnBeforeChunkSaved;

		//Game Time Events Handlers
		public event OnLibTickDelegate OnLibTick;
		public event OnGameMinuteDelegate OnGameMinute;
		public event OnGameHourDelegate OnGameHour;

		//Player Event handlers
		public event OnPlayerLoginDelegate OnPlayerLogin;
		public event OnPlayerSpawningDelegate OnPlayerSpawning;
		public event OnPlayerSpawnedDelegate OnPlayerSpawned;
		public event OnPlayerDisconnectedDelegate OnPlayerDisconnected;
		public event OnPlayerDataSavedDelegate OnPlayerDataSaved;
		public event OnPlayerDroppedItemDelegate OnPlayerDroppedItem;
		public event OnPlayerCollectedItemDelegate OnPlayerCollectedItem;
		public event OnPlayerInventoryChanged OnPlayerInventoryChanged;
		public event OnPlayerBuffAddedDelegate OnPlayerBuffAdded;
		public event OnPlayerBuffRemovedDelegate OnPlayerBuffRemoved;
		public event OnPlayerWellnessChangedDelegate OnPlayerWellnessChanged;
		public event OnPlayerDiedDelegate OnPlayerDied;
		public event OnPlayerKilledByPlayerDelegate OnPlayerKilledByPlayer;

		public event OnPersistentPlayerDataEventDelegate OnPersistentPlayerDataEvent;

		public event OnPlayerACLInviteSentDelegate OnPlayerACLInviteSent;
		public event OnPlayerACLInviteAcceptedDelegate OnPlayerACLInviteAccepted;
		public event OnPlayerACLInviteDeclinedDelegate OnPlayerACLInviteDeclined;
		public event OnPlayerACLRemovedDelegate OnPlayerACLRemoved;

		//Chat Command Delegates
		public event OnChatCommandEventDelegate OnChatCommand;

		//Entity Event Handlers
		public event OnEntityLoadedDelegate OnEntityLoaded;
		public event OnEntityUnloadedDelegate OnEntityUnloaded;
		public event OnNPCLoaded OnNPCLoaded;
		public event OnNPCUnloaded OnNPCUnloaded;
		public event OnNPCBanditLoaded OnNPCBanditLoaded;
		public event OnNPCBanditUnloaded OnNPCBanditUnloaded;
		public event OnNPCSurvivorLoaded OnNPCSurvivorLoaded;
		public event OnNPCSurvivorUnloaded OnNPCSurvivorUnloaded;

		public event OnTraderPrimaryInventoryChanged OnTraderPrimaryInventoryChanged;
		public event OnTraderSecretStashChanged OnTraderSecretStashChanged;

		//Block Events
		public event OnBlockChangedDelegate OnBlockChanged;
		public event OnBlockDamagedDelegate OnBlockDamaged;

		//Workstation Events
//		public event OnWorkstationToolChangedDelegate OnWorkstationToolChanged;
//		public event OnWorkStationFuelChangedDelegate OnWorkstationFuelChanged;
//		public event OnWorkstationInputChangedDelegate OnWorkstationInputChanged;
//		public event OnWorkstationOutputChangedDelegate OnWorkstationOutputChanged;
//		public event OnWorkstationQueueChangedDelegate OnWorkstationQueueChanged;
//		public event OnWorkstationIsActiveChangedDelegate OnWorkstationIsActiveChanged;
//		public event OnWorkstationTotalBurnTimeChangedDelegate OnWorkstationTotalBurnTimeChanged;
//		public event OnWorkstationBurnTimeChangedDelegate OnWorkstationBurnTimeChanged;
//
		//LootContainer Events
		//public event OnLootContainerTouchedChangedDelegate OnLootContainerTouchChanged;
		//public event OnLootContainerOpenedTimeChangedDelegate OnLootContainerOpenedTimeChanged;
		public event OnLootContainerItemChangedDelegate OnLootContainerItemChanged;

		//PowerSource Events
		public event OnPoweredIsPoweredChangeDelegate OnPoweredIsPoweredChanged;
		public event OnPoweredPowerUsedChangeDelegate OnPoweredPowerUsedChanged;
		public event OnPoweredRequiredPowerChangeDelegate OnPoweredRequiredPowerChanged;

		public event OnPoweredTriggerIsTriggeredChangeDelegate OnPoweredTriggerIsTriggeredChange;
		public event OnPoweredTriggerProperty1ChangeDelegate OnPoweredTriggerProperty1Change;
		public event OnPoweredTriggerProperty2ChangeDelegate OnPoweredTriggerProperty2Change;
		public event OnPoweredTriggerTargetSelfChangeDelegate OnPoweredTriggerTargetSelfChange;
		public event OnPoweredTriggerTargetAlliesChangeDelegate OnPoweredTriggerTargetAlliesChange;
		public event OnPoweredTriggerTargetStrangersChangeDelegate OnPoweredTriggerTargetStrangersChange;
		public event OnPoweredTriggerTargetZombiesChangeDelegate OnPoweredTriggerTargetZombiesChange;



		public event OnPowerSourceTurnedOnDelegate OnPowerSourceTurnedOn;
		public event OnPowerSourceTurnedOffDelegate OnPowerSourceTurnedOff;

		public event OnPowerSourceCurrentFuelChanged OnPowerSourceFuelChanged;
		public event OnPowerSourceMaxOutputChanged OnPowerSourceMaxOutputChanged;

		/* -------------------------------------------------------------
		 * End Events
		 * -------------------------------------------------------------
		 */
		public Events ()
		{
			
		}

		public bool hasGameStartedHandlers{
			get{
				return OnGameStarted != null;
			}
		}

		public void NotifyGameStartedHandlers(){
			if (OnGameStarted != null) {
				OnGameStarted ();
			}
		}

		public bool hasGameStopedHandlers{
			get{
				return OnGameStopped != null;
			}
		}

		public void NotifyGameStoppedHandlers(){
			if (OnGameStopped != null) {
				OnGameStopped ();
			}
		}

		public bool hasGameStartDoneHandlers {
			get{
				if (OnGameStartDone != null || OnBeforeGameStartDone != null || OnAfterGameStartDone != null) {
					return true;
				}

				return false;
			}
		}

		public void NotifyGameStartDoneHandlers(){
			if (OnBeforeGameStartDone != null) {
				OnBeforeGameStartDone ();
			}

			if (OnGameStartDone != null) {
				OnGameStartDone ();
			}

			if (OnAfterGameStartDone != null) {
				OnAfterGameStartDone ();
			}
		}

		public bool hasGameUpdateHandlers{
			get {
				return OnGameUpdate != null;
			}
		}

		public void NotifyGameUpdateHandlers(){
			if (OnGameUpdate != null) {
				OnGameUpdate ();
			}
		}

		public bool hasGameStatsChangedHandlers {
			get {
				return OnGameStatsChanged != null;
			}
		}

		public void NotifyGameStatsChangedHandlers(EnumGameStats _stat, object _newValue){
			if (OnGameStatsChanged != null) {
				OnGameStatsChanged (_stat.ToString (), _newValue);
			}
		}

		public bool hasGamePrefsChangedHandlers{
			get{ 
				return OnGamePrefsChanged != null;
			}
		}

		public void NotifyGamePrefsChangedHandlers(EnumGamePrefs _pref){
			if (OnGamePrefsChanged != null) {
				OnGamePrefsChanged (_pref);
			}
		}

		public bool hasNotifyChunkColorsDoneHandlers{
			get{ 
				return OnChunkColorsDone != null;
			}
		}

		public void NotifyChunkColorsDoneHandlers(Chunk chunk){
			if (OnChunkColorsDone != null) {
				OnChunkColorsDone (chunk);
			}
		}

		public bool hasLibTickHandlers{
			get{ 
				if (OnLibTick != null || OnGameHour != null || OnGameMinute != null) {
					return true;
				} else {
					return false;
				}
			}
		}

		public void NotifyLibTickHandlers(){
			if (OnLibTick != null) {
				OnLibTick ();
			}
		}

		public bool hasGameMinuteHandlers{
			get { 
				return OnGameMinute != null;
			}
		}

		public void NotifyGameMinuteHandlers(int _day, int _hour, int _minute){
			if (OnGameMinute != null) {
				OnGameMinute (_day, _hour, _minute);
			}

			if (_minute == 0) {
				NotifyGameHourHandlers (_day, _hour);
			}
		}

		public bool hasGameHourHandlers{
			get{ 
				return OnGameHour != null;
			}
		}

		public void NotifyGameHourHandlers(int _day, int _hour){
			if(OnGameHour != null){
				OnGameHour(_day, _hour);
			}
		}

		public bool hasPlayerLoginHandlers {
			get { 
				return OnPlayerLogin != null;
			}
		}

		public void NotifyPlayerLoginHandlers(ClientInfo _cInfo, string _compat){
			if (OnPlayerLogin != null) {
				OnPlayerLogin (_cInfo, _compat);
			}
		}

		public bool hasPlayerSpawningHandlers{
			get {
				return OnPlayerSpawning != null;
			}
		}

		public void NotifyPlayerSpawningHandlers(ClientInfo _cInfo){
			EntityPlayer player = PlayerUtils.GetEntityPlayer (_cInfo);

			if (OnPlayerSpawning != null) {
				OnPlayerSpawning (player);
			}
		}

		public bool hasPlayerDisconnectedHandlers{
			get{ 
				return OnPlayerDisconnected != null;
			}
		}

		public void NotifyPlayerDisconnectedHandlers(ClientInfo _cInfo){
			//EntityPlayer player = PlayerUtils.GetEntityPlayer (_cInfo);

			//if (player != null) {
			//	if (OnPlayerDisconnected != null) {
			//		OnPlayerDisconnected (player);
			//	}
			//}
		}

		public bool hasPlayerDataSavedHandlers{
			get { 
				return OnPlayerDataSaved != null;
			}
		}

		public void NotifyPlayerDataSavedHandlers(ClientInfo _cInfo, PlayerDataFile _playerDataFile){
			if(OnPlayerDataSaved!=null){
				OnPlayerDataSaved (_cInfo, _playerDataFile);
			}
		}

		public void NotifyPlayerSpawnedHandlers(ClientInfo _cInfo, RespawnType _respawnReason, Vector3i _pos){
			if (OnPlayerSpawned != null) {
				OnPlayerSpawned (_cInfo, _respawnReason, _pos);
			}
		}

		public bool hasPlayerDroppedItemHandlers{
			get{ 
				return OnPlayerDroppedItem != null;
			}
		}

		public void NotifyPlayerDroppedItemHandlers(EntityPlayer _player, EntityItem _item){
			if (OnPlayerDroppedItem != null) {
				OnPlayerDroppedItem (_player, _item);
			}
		}

		public bool hasPlayerCollectedItemHandlers{
			get{ 
				return OnPlayerCollectedItem != null;
			}
		}

		public void NotifyPlayerCollectedItemHandlers(EntityPlayer _player, EntityItem _item){
			if (OnPlayerCollectedItem != null) {
				ItemStack itemStack = ItemUtils.GetItemStack (_item);

				OnPlayerCollectedItem (_player, itemStack);
			}
		}

		public bool hasPlayerInventoryChangedHandlers{
			get{ 
				return OnPlayerInventoryChanged != null;
			}
		}

		public void NotifyPlayerInventoryChangedHandlers(EntityPlayer _player){
			if (OnPlayerInventoryChanged != null) {
				Inventory inventory = _player.inventory;
				OnPlayerInventoryChanged (_player, inventory);
			}
		}

		public bool hasPlayerKilledHandlers{
			get { 
				if(OnPlayerKilledByPlayer!=null || OnPlayerDied!=null){
					return true;
				}

				return false;
			}
		}

		public void NotifyPlayerDiedHandlers(EntityPlayer player){
			if (OnPlayerDied != null) {
				OnPlayerDied (player);
			}
		}

		public void NotifyPlayerKilledByPlayerHandlers(EntityPlayer player, EntityPlayer killer){
			if(OnPlayerKilledByPlayer != null){
				OnPlayerKilledByPlayer (player, killer);
			}
		}

		public bool hasPlayerBuffAddedHandlers{
			get{ 
				return OnPlayerBuffAdded != null;
			}
		}

		public void NotifyPlayerBuffAddedHandlers(EntityPlayer _player, Buff _buff){
			if (OnPlayerBuffAdded != null) {
				OnPlayerBuffAdded (_player, _buff);
			}
		}

		public bool hasPlayerBuffRemovedHandlers{
			get{ 
				return OnPlayerBuffRemoved != null;
			}
		}

		public void NotifyPlayerBuffRemovedHandlers(EntityPlayer _player, Buff _buff){
			if (OnPlayerBuffRemoved != null) {
				OnPlayerBuffRemoved (_player, _buff);
			}
		}

		public bool hasPlayerWellnessChangedHandlers{
			get{ 
				return OnPlayerWellnessChanged != null;
			}
		}

		public void NotifyPlayerWellnessChangedHandlers(EntityPlayer _player, float amount){
			if (OnPlayerWellnessChanged != null) {
				OnPlayerWellnessChanged (_player, amount);
			}
		}

		public bool hasPersistentPlayerDataHandlers{
			get { 
				if (OnPlayerACLInviteAccepted != null || OnPlayerACLInviteDeclined != null || OnPlayerACLInviteSent != null || OnPlayerACLRemoved != null) {
					return true;
				}

				return false;
			}
		}

		public void NotifyPersistentPlayerDataEventHandlers(PersistentPlayerData player, PersistentPlayerData otherPlayer, EnumPersistentPlayerDataReason reason){
			if (OnPersistentPlayerDataEvent != null) {
				OnPersistentPlayerDataEvent (player, otherPlayer, reason);
			}
			switch (reason) {
			case EnumPersistentPlayerDataReason.ACL_Invite:
				NotifyPlayerACLInviteSentHandlers (player, otherPlayer);
				break;
			case EnumPersistentPlayerDataReason.ACL_AcceptedInvite:
				NotifyPlayerACLInviteAcceptedHandlers (player, otherPlayer);
				break;
			case EnumPersistentPlayerDataReason.ACL_DeclinedInvite:
				NotifyPlayerACLInviteDeclinedHandlers (player, otherPlayer);
				break;
			case EnumPersistentPlayerDataReason.ACL_Removed:
				NotifyPlayerACLRemovedHandlers (player, otherPlayer);
				break;
			}
		}

		public void NotifyPlayerACLInviteSentHandlers(PersistentPlayerData player, PersistentPlayerData otherPlayer){
			if (OnPlayerACLInviteSent != null) {
				OnPlayerACLInviteSent (player, otherPlayer);
			}
		}

		public void NotifyPlayerACLInviteAcceptedHandlers(PersistentPlayerData player, PersistentPlayerData otherPlayer){
			if (OnPlayerACLInviteAccepted != null) {
				OnPlayerACLInviteAccepted (player, otherPlayer);
			}
		}

		public void NotifyPlayerACLInviteDeclinedHandlers(PersistentPlayerData player, PersistentPlayerData otherPlayer){
			if (OnPlayerACLInviteDeclined != null) {
				OnPlayerACLInviteDeclined (player, otherPlayer);
			}
		}

		public void NotifyPlayerACLRemovedHandlers(PersistentPlayerData player, PersistentPlayerData otherPlayer){
			if (OnPlayerACLRemoved != null) {
				OnPlayerACLRemoved (player, otherPlayer);
			}
		}

		/*
		 * Chat Commands
		 */

		public bool hasChatHandlers {
			get { 
				return OnChatCommand != null;
			}
		}
		public void NotifyChatCommandHandlers(ClientInfo _player, string _command, string _param){
			if (OnChatCommand != null) {
				OnChatCommand (_player, _command, _param);
			}
		}

		public bool hasEntityLoadedHandlers{
			get{ 
				return OnEntityLoaded != null || OnNPCLoaded != null || OnNPCSurvivorLoaded != null || OnNPCBanditLoaded != null || OnPlayerDroppedItem != null;
			}
		}

		public void NotifyEntityLoadedHandlers(Entity _entity){
			if (OnEntityLoaded != null) {
				OnEntityLoaded (_entity);
			}

			if (_entity is EntityNPC) {
				if (OnNPCLoaded != null) {
					OnNPCLoaded (_entity as EntityNPC);
				}

				if (_entity is EntitySurvivor) {
					if (OnNPCSurvivorLoaded != null) {
						OnNPCSurvivorLoaded (_entity as EntitySurvivor);
					}
				} else {
					if (_entity is EntityBandit) {
						if (OnNPCBanditLoaded != null) {
							OnNPCBanditLoaded (_entity as EntityBandit);
						}
					}
				}
			}

			if(OnPlayerDroppedItem != null){
				if (_entity.GetType ().ToString() == "EntityItem") {
					EntityItem ei = (EntityItem)_entity;

					if (ei.belongsPlayerId > 0) {
						NotifyPlayerDroppedItemHandlers (PlayerUtils.GetEntityPlayer (ei.belongsPlayerId.ToString ()), ei);
					}
				}
			}
		}

		public bool hasEntityUnloadedHandlers{
			get { 
				return OnEntityUnloaded != null || OnNPCUnloaded != null || OnNPCSurvivorUnloaded != null || OnNPCBanditUnloaded!=null || OnPlayerCollectedItem!=null;
			}
		}

		public void NotifyEntityUnloadedHandlers(Entity _entity, EnumRemoveEntityReason _reason){
			if (OnEntityUnloaded != null) {
				OnEntityUnloaded (_entity, _reason.ToString ());
			}

			if (_entity is EntityNPC) {
				if (OnNPCUnloaded != null) {
					OnNPCUnloaded (_entity as EntityNPC, _reason.ToString());
				}

				if (_entity is EntitySurvivor) {
					if (OnNPCSurvivorUnloaded != null) {
						OnNPCSurvivorUnloaded (_entity as EntitySurvivor, _reason.ToString());
					}
				} else {
					if (_entity is EntityBandit) {
						if (OnNPCBanditUnloaded != null) {
							OnNPCBanditUnloaded (_entity as EntityBandit, _reason.ToString());
						}
					}
				}
			}

			if (OnPlayerCollectedItem != null) {
				if (_entity.GetType ().ToString() == "EntityItem") {
					EntityItem ei = (EntityItem)_entity;
					if (ei.belongsPlayerId > 0) {
						NotifyPlayerCollectedItemHandlers (PlayerUtils.GetEntityPlayer (ei.belongsPlayerId.ToString()), ei);
					}
				}
			}
		}

		public void NotifyTraderPrimaryInventoryChangedHandlers(EntityNPC trader, List<ItemStack> inv){
			if (OnTraderPrimaryInventoryChanged != null) {
				OnTraderPrimaryInventoryChanged (trader, inv);
			}
		}

		public void NotifyTraderSecretStashChangedHandlers(EntityNPC trader, List<ItemStack[]> inv){
			if (OnTraderSecretStashChanged != null) {
				OnTraderSecretStashChanged (trader, inv);
			}
		}

		public bool hasChunkAddedHandlers{
			get{ 
				return OnChunkAdded != null || OnBeforeChunkRemoved!=null || OnBeforeChunkSaved!=null;
			}
		}

		public void NotifyChunkAddedHandlers(Chunk _Chunk){
			if(OnChunkAdded!=null){
				OnChunkAdded (_Chunk);		
			}	
		}

		public bool hasBeforeChunkRemovedHandlers{
			get{ 
				return OnBeforeChunkRemoved != null;
			}
		}

		public void NotifyBeforeChunkRemovedHandlers(Chunk _Chunk){
			if(OnBeforeChunkRemoved!=null){
				OnBeforeChunkRemoved (_Chunk);		
			}	
		}

		public bool hasBeforeChunkSavedHandlers{
			get{ 
				return OnBeforeChunkSaved != null;
			}
		}

		public void NotifyBeforeChunkSavedHandlers(Chunk _Chunk){
			if(OnBeforeChunkSaved!=null){
				OnBeforeChunkSaved (_Chunk);		
			}	
		}

		public bool hasBlockChangeHandlers{
			get { 
				return OnBlockChanged != null;
			}
		}

		public void NotifyBlockChangedHandlers(Vector3i _blockPos, BlockValue _blockValueOld, BlockValue _blockValueNew){
			if (OnBlockChanged != null) {
				OnBlockChanged (_blockPos, _blockValueOld, _blockValueNew);
			}
		}

		public bool hasBlockDamagedHandlers{
			get { 
				return OnBlockDamaged != null;
			}
		}

		public void NotifyBlockDamagedHandlers(Vector3i _blockPos, BlockValue _blockValue, int _damage, int _attackerEntityId){
			if(OnBlockDamaged != null){
				OnBlockDamaged (_blockPos, _blockValue, _damage, _attackerEntityId);
			}
		}

		/*
		 * Workstation Notifiers
		 * */
//		public void NotifyWorkstationToolChangedHandlers(TileEntityWorkstation te, int slotIdx, ItemStack oldItem, ItemStack newItem){
//			if (OnWorkstationToolChanged != null) {
//				OnWorkstationToolChanged (te, slotIdx, oldItem, newItem);
//			}
//		}
//
//		public void NotifyWorkstationFuelChangedHandlers(TileEntityWorkstation te,int slotIdx, ItemStack oldItem, ItemStack newItem){
//			if (OnWorkstationFuelChanged != null) {
//				OnWorkstationFuelChanged (te, slotIdx, oldItem, newItem);
//			}
//		}
//
//		public void NotifyWorkstationInputChangedHandlers(TileEntityWorkstation te,int slotIdx, ItemStack oldItem, ItemStack newItem){
//			if (OnWorkstationInputChanged != null) {
//				OnWorkstationInputChanged (te, slotIdx, oldItem, newItem);
//			}
//		}
//
//		public void NotifyWorkstationOutputChangedHandlers(TileEntityWorkstation te, int slotIdx, ItemStack oldItem, ItemStack newItem){
//			if (OnWorkstationOutputChanged != null) {
//				OnWorkstationOutputChanged (te, slotIdx, oldItem, newItem);
//			}
//		}
//
//		public void NotifyWorkstationQueueChangedHandlers(TileEntityWorkstation te){
//			if (OnWorkstationQueueChanged != null) {
//				OnWorkstationQueueChanged (te);
//			}
//		}
//
//		public void NotifyWorkstationIsActiveChangedHandlers(TileEntityWorkstation te, bool oldState, bool newState){
//			if (OnWorkstationIsActiveChanged != null) {
//				OnWorkstationIsActiveChanged (te, oldState, newState);
//			}
//		}
//
//		public void NotifyWorkstationTotalBurnTimeChangedHandlers(TileEntityWorkstation te, float oldTime, float newTime){
//			if (OnWorkstationTotalBurnTimeChanged != null) {
//				OnWorkstationTotalBurnTimeChanged (te, oldTime, newTime);
//			}
//		}
//
//		public void NotifyWorkstationBurnTimeChangedHandlers(TileEntityWorkstation te, float oldTime, float newTime){
//			if (OnWorkstationBurnTimeChanged != null) {
//				OnWorkstationBurnTimeChanged(te, oldTime, newTime);
//			}
//		}

		/*
		 * Loot Container Notifiers 
		 */


//
//		public void NotifyLootContainerTouchedChangedHandlers(TileEntityLootContainer lc, ulong oldValue, ulong newValue){
//			if (OnLootContainerTouchChanged != null) {
//				OnLootContainerTouchChanged (lc, oldValue, newValue);
//			}
//		}
//
//		public void NotifyLootContainerOpenedTimeChangedHandlers(TileEntityLootContainer lc, float oldValue, float newValue){
//			if (OnLootContainerOpenedTimeChanged != null) {
//				OnLootContainerOpenedTimeChanged (lc, oldValue, newValue);
//			}
//		}

		public bool hasLootContainerChangedHandlers{
			get { 
				return OnLootContainerItemChanged != null;
			}
		}

		public void NotifyLootContainerItemChangedHandlers(TileEntityLootContainer lc, int slotIdx, ItemStack oldStack, ItemStack newStack){
			//Log.Out ("LOOT ITEM CHANGED" + slotIdx.ToString());
			if (OnLootContainerItemChanged != null) {
				OnLootContainerItemChanged (lc, slotIdx, oldStack, newStack);
			}
		}

		/*
		 * Power Source Notifiers
		 */

		public void NotifyPoweredIsPoweredChangeHandlers(TileEntityPowered te, bool oldValue, bool newValue){
			if (OnPoweredIsPoweredChanged != null) {
				OnPoweredIsPoweredChanged (te, oldValue, newValue);
			}
		}

		public void NotifyPoweredPowerUsedChangeHandlers(TileEntityPowered te, int oldValue, int newValue){
			if (OnPoweredPowerUsedChanged != null) {
				OnPoweredPowerUsedChanged (te, oldValue, newValue);
			}
		}

		public void NotifyPoweredRequiredPowerChangeHandlers(TileEntityPowered te, int oldValue, int newValue){
			if (OnPoweredRequiredPowerChanged != null) {
				OnPoweredRequiredPowerChanged (te, oldValue, newValue);
			}
		}

		public void NotifyPoweredTriggerIsTriggeredHandlers(TileEntityPoweredTrigger te, bool oldValue, bool newValue){
			if (OnPoweredTriggerIsTriggeredChange != null) {
				OnPoweredTriggerIsTriggeredChange (te, oldValue, newValue);
			}
		}

		public void NotifyPoweredTriggerProperty1ChangeHandlers(TileEntityPoweredTrigger te, byte oldValue, byte newValue){
			if (OnPoweredTriggerProperty1Change != null) {
				OnPoweredTriggerProperty1Change(te, oldValue, newValue);
			}
		}

		public void NotifyPoweredTriggerProperty2ChangeHandlers(TileEntityPoweredTrigger te, byte oldValue, byte newValue){
			if (OnPoweredTriggerProperty2Change != null) {
				OnPoweredTriggerProperty2Change(te, oldValue, newValue);
			}
		}

		public void NotifyPoweredTriggerTargetSelfChangeHandlers(TileEntityPoweredTrigger te, bool oldValue, bool newValue){
			if (OnPoweredTriggerTargetSelfChange != null) {
				OnPoweredTriggerTargetSelfChange (te, oldValue, newValue);
			}
		}

		public void NotifyPoweredTriggerTargetAlliesChangeHandlers(TileEntityPoweredTrigger te, bool oldValue, bool newValue){
			if (OnPoweredTriggerTargetAlliesChange != null) {
				OnPoweredTriggerTargetAlliesChange (te, oldValue, newValue);
			}
		}

		public void NotifyPoweredTriggerTargetStrangersChangeHandlers(TileEntityPoweredTrigger te, bool oldValue, bool newValue){
			if (OnPoweredTriggerTargetStrangersChange != null) {
				OnPoweredTriggerTargetStrangersChange (te, oldValue, newValue);
			}
		}

		public void NotifyPoweredTriggerTargetZombieChangeHandlers(TileEntityPoweredTrigger te, bool oldValue, bool newValue){
			if (OnPoweredTriggerTargetZombiesChange != null) {
				OnPoweredTriggerTargetZombiesChange (te, oldValue, newValue);
			}
		}









		public void NotifyPowerSourceTurnedOnHandlers(TileEntityPowerSource te){
			if (OnPowerSourceTurnedOn != null) {
				OnPowerSourceTurnedOn (te);
			}
		}

		public void NotifyPowerSourceTurnedOffHandlers(TileEntityPowerSource te){
			if (OnPowerSourceTurnedOff != null) {
				OnPowerSourceTurnedOff (te);
			}
		}

		public void NotifyPowerSourceCurrentFuelChangedHandlers(TileEntityPowerSource te, int oldValue, int newValue){
			if (OnPowerSourceFuelChanged != null) {
				OnPowerSourceFuelChanged (te, oldValue, newValue);
			}
		}

		public void NotifyPowerSourceMaxPowerChangedHandlers(TileEntityPowerSource te, int oldValue, int newValue){
			if (OnPowerSourceMaxOutputChanged != null) {
				OnPowerSourceMaxOutputChanged (te, oldValue, newValue);
			}
		}
	}
}

