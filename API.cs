using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

namespace SDTM
{
	public delegate bool OnProductPurchasedDelegate(ClientInfo _cInfo, string productName, int count, int amount); //support so paydaylib can be optional
	public class API : ModApiAbstract
	{
		public static string ModPath = "";
		public static string configDataPath = Directory.GetParent(Application.dataPath).FullName+Path.DirectorySeparatorChar+"Data"+Path.DirectorySeparatorChar+"7DTM"+Path.DirectorySeparatorChar;
		public static string DataPath = Directory.GetParent(Application.dataPath).FullName + Path.DirectorySeparatorChar + "Data"+Path.DirectorySeparatorChar;
		public static string XMLConfigPath = Directory.GetParent(Application.dataPath).FullName + Path.DirectorySeparatorChar + Constants.cDirConfig;
		public static string PrefabPath = Directory.GetParent(Application.dataPath).FullName + Path.DirectorySeparatorChar + Constants.cDirPrefabs;
		public static string ModsPath = Directory.GetParent(Application.dataPath).FullName + Path.DirectorySeparatorChar + "Mods"+Path.DirectorySeparatorChar;
		public static string saveDataPath = GameUtils.GetSaveGameDir()+ Path.DirectorySeparatorChar+"7DTM"+Path.DirectorySeparatorChar;

		public static bool ChatCommandsEnabled = true;

		public static bool WebConsoleEnabled = false;
		public static string WebConsolePort = (GamePrefs.GetInt(EnumGamePrefs.ControlPanelPort)).ToString();


		public static string ServerDescription = GamePrefs.GetString (EnumGamePrefs.ServerDescription);
		public static bool ServerIsPublic = GamePrefs.GetBool (EnumGamePrefs.ServerIsPublic);
		public static string ServerName = GamePrefs.GetString (EnumGamePrefs.ServerName);
		public static string ServerWebsiteURL = GamePrefs.GetString (EnumGamePrefs.ServerWebsiteURL);

		public bool ShowFriendPlayerOnMap = GamePrefs.GetBool (EnumGamePrefs.ShowFriendPlayerOnMap);
		public static int ZombiesRun = GamePrefs.GetInt (EnumGamePrefs.ZombiesRun);

		public static bool OverrideGamePrefs = false;

		public static string WorldType = GamePrefs.GetString(EnumGamePrefs.GameWorld);
		public static string WorldName = GamePrefs.GetString(EnumGamePrefs.GameName);
//
//		public static int AirdropFrequency = GamePrefs.GetInt (EnumGamePrefs.AirDropFrequency);
//		public static bool AirdropMarker = GamePrefs.GetBool (EnumGamePrefs.AirDropMarker);
//		public static int BlockDurabilityModifier = GamePrefs.GetInt (EnumGamePrefs.BlockDurabilityModifier);
//		public static bool BuildCreate = GamePrefs.GetBool (EnumGamePrefs.BuildCreate);
//		public static int DayLightLength = GamePrefs.GetInt (EnumGamePrefs.DayLightLength);
//		public static int DayNightLength = GamePrefs.GetInt (EnumGamePrefs.DayNightLength);
//		public static int DropOnDeath = GamePrefs.GetInt (EnumGamePrefs.DropOnDeath);
//		public static int DropOnQuit = GamePrefs.GetInt (EnumGamePrefs.DropOnQuit);
//		public static int EnemyDifficulty = GamePrefs.GetInt (EnumGamePrefs.EnemyDifficulty);
//		//public static bool EnemySenseMemory = GamePrefs.GetInt (EnumGamePrefs.EnemySpawnMode);
//		public static int GameDifficulty = GamePrefs.GetInt (EnumGamePrefs.GameDifficulty);
//
//		public static int LandClaimDeadZone = GamePrefs.GetInt (EnumGamePrefs.LandClaimDeadZone);
//		public static int LandClaimDecayMode = GamePrefs.GetInt (EnumGamePrefs.LandClaimDecayMode);
//		public static int LandClaimExpiryTime = GamePrefs.GetInt (EnumGamePrefs.LandClaimExpiryTime);
//		public static int LandClaimOfflineDurabilityModifier = GamePrefs.GetInt (EnumGamePrefs.LandClaimOfflineDurabilityModifier);
//		public static int LandClaimOnlineDurabilityModifier = GamePrefs.GetInt (EnumGamePrefs.LandClaimOnlineDurabilityModifier);
//		public static int LandClaimSize = GamePrefs.GetInt (EnumGamePrefs.LandClaimSize);
//		public static int LootAbundance = GamePrefs.GetInt (EnumGamePrefs.LootAbundance);
//		public static int LootRespawnDays = GamePrefs.GetInt (EnumGamePrefs.LootRespawnDays);
//
//		public static int MaxSpawnedAnimals = GamePrefs.GetInt (EnumGamePrefs.MaxSpawnedAnimals);
//		public static int MaxSpawnedZombies = GamePrefs.GetInt (EnumGamePrefs.MaxSpawnedZombies);

		private ChunkCallbackHandler chunkCbHandler;

		private Servers.HTTP.WWW webServer = null;

		public bool started = false;

		public bool doTick = false;
		public int tickCount = 1;
		public int tickRate = 40;


		public ulong lastTime = 0;
		public int day = 0;
		public int hour = 0;
		public int minute = 0;

		public bool firstTP=false;

		private static Permissions _permissions;
		private static Events _events;

		public Dictionary<string, ChatCommand> _chatCommands = new Dictionary<string, ChatCommand>();

		public static Events Events {
			get {
				if (_events == null) {
					_events = new Events();
				}

				return _events;
			}
		}

		public static Permissions Permissions {
			get{
				if (_permissions == null) {
					_permissions = new Permissions ();
				}

				return _permissions;
			}
		}

		public static API Instance{
			get{ 
				Mod thisMod = ModManager.GetMod ("7DTMLib");
				ModPath = thisMod.Path;
				return thisMod.ApiInstance as SDTM.API;
			}
		}

		public API ()
		{
			
			//Event Manager
			_events = new Events ();
			//setup default permissions
			_permissions = new Permissions();
			_permissions.Load ();

			Init ();


		}

		public void Init(){
			
			//Load Config
			if (!LoadConfig ()) {
				return;
			}

			SDTM.API.Permissions.RegisterPermission ("permissions.user.list");
			SDTM.API.Permissions.RegisterPermission ("permissions.user.setgroup");
			SDTM.API.Permissions.RegisterPermission ("permissions.user.remove");
			SDTM.API.Permissions.RegisterPermission ("permissions.user.setperm");
			SDTM.API.Permissions.RegisterPermission ("permissions.user.removeperm");
			SDTM.API.Permissions.RegisterPermission ("permissions.user.test");

			SDTM.API.Permissions.RegisterPermission ("permissions.group.list");
			SDTM.API.Permissions.RegisterPermission ("permissions.group.info");
			SDTM.API.Permissions.RegisterPermission ("permissions.group.create");
			SDTM.API.Permissions.RegisterPermission ("permissions.group.modify");
			SDTM.API.Permissions.RegisterPermission ("permissions.group.remove");
			SDTM.API.Permissions.RegisterPermission ("permissions.group.default");
			SDTM.API.Permissions.RegisterPermission ("permissions.group.setperm");
			SDTM.API.Permissions.RegisterPermission ("permissions.group.removeperm");

			if (SDTM.API.Permissions.Groups.Count == 0) {
				SDTM.API.Permissions.AddGroup ("public");
				SDTM.API.Permissions.AddGroup ("guest");
				SDTM.API.Permissions.AddGroup ("regular");
				SDTM.API.Permissions.AddGroup ("moderator");
				SDTM.API.Permissions.AddGroup ("admin");

				SDTM.API.Permissions.PublicGroupName = "public";
				SDTM.API.Permissions.DefaultGroupName = "guest";
				SDTM.API.Permissions.DefaultRegularGroupName = "regular";

				SDTM.API.Permissions.SetGroupPermission ("guest", "chatcommand.help", true); //chatcommand.help
				SDTM.API.Permissions.SetGroupPermission ("regular", "chatcommand.*", true);
				SDTM.API.Permissions.SetGroupPermission ("moderator", "chatcommand.*", true);
				SDTM.API.Permissions.SetGroupPermission ("moderator", "permissions.user.*", true);
				SDTM.API.Permissions.SetGroupPermission ("admin", "*", true);
			}

			//create the super admin group
			SDTM.API.Permissions.AddGroup ("_super");
			SDTM.API.Permissions.SetGroupPermission ("_super", "*", true);

			if (ChatCommandsEnabled) {
				RegisterChatCommand ("help", "Returns a list of chat commands", "!help - show this help text\n!help [command] - show help for [command]", new OnChatCommandDelegate (ChatCmdHelp.Help));
			}

			started = true;
		}

		public bool LoadConfig(){
			XMLConfig conf = new XMLConfig("sdtmlib");
			if (!conf.Load ()) {
				Log.Out ("Could not Load SDTMLib config");
				return false;
			}

			int tryTickRate = 0;
			string sTickRate = conf.Get("tickrate");

			if (sTickRate != "") {
				int.TryParse (sTickRate, out tryTickRate);
				if (tryTickRate > 0) {
					tickRate = tryTickRate;
				} else {
					SaveConfig ();
				}
			} else {
				SaveConfig ();
			}

			ChatCommandsEnabled = conf.Get ("chatcommandsenabled").ToLower () == "true" ? true : false;
			API.OverrideGamePrefs = conf.Get ("overridegameprefs").ToLower () == "true" ? true : false;
			API.WebConsoleEnabled = conf.Get ("webconsole_enabled").ToLower () == "true" ? true : false;
			API.WebConsolePort = conf.Get ("webconsole_port");

			if (WebConsolePort == "" || WebConsolePort==null) {
				WebConsolePort = GamePrefs.GetInt (EnumGamePrefs.ControlPanelPort).ToString ();
			}

			if (API.OverrideGamePrefs) {
				string confWorldType = conf.Get ("WorldType");
				string confWorldName = conf.Get ("WorldName");

				if (confWorldType != "") {
					SetConfig ("worldtype", confWorldType, false, true);
				}

				if (confWorldName != "") {
					SetConfig ("worldname", confWorldName, false, true);
				}
			}


			return true;
		}

		public bool SaveConfig(){
			XMLConfig conf = new XMLConfig ("sdtmlib");
			conf.Set("tickrate", tickRate.ToString());
			//conf.Set("experm_enabled", );
			conf.Set("overridegameprefs", OverrideGamePrefs.ToString());
			conf.Set("webconsole_port", WebConsolePort.ToString());
			conf.Set("webconsole_enabled", WebConsoleEnabled.ToString());
			//conf.Set ("AirdropFreq", WorldType);
			conf.Set ("WorldType", WorldType);
			conf.Set ("WorldName", WorldName);
			conf.Set ("chatcommandsenabled", ChatCommandsEnabled?"true":"false");
			return conf.Save();
		}

		public bool SetConfig(string confName, string confValue, bool bSave=true, bool setGamePref=false){
			bool wasSet = false;
			switch (confName.ToLower ()) {
			case "tickrate":
				int confTickRate = -1;
				if(int.TryParse(confValue, out confTickRate)){
					tickRate = confTickRate;
					wasSet = true;
				}
				break;
			case "worldtype":
				switch (confValue.ToLower ()) {
				case "navezgane":
					WorldType = "Navezgane";
					if (setGamePref) {
						GamePrefs.Set (EnumGamePrefs.GameWorld, WorldType);
					}
					wasSet = true;
					break;
				case "random":
				case "random gen":
					WorldType = "Random Gen";
					if (setGamePref) {
						GamePrefs.Set (EnumGamePrefs.GameWorld, WorldType);
					}
					wasSet = true;
					break;
				default:
					Log.Warning ("[7dtmLib] Invalid World Config Type Value '" + confValue + "'; ignoring.");
					break;
				}
				break;
			case "worldname":
				WorldName = confValue;
				if (setGamePref) {
					GamePrefs.Set (EnumGamePrefs.GameName, WorldName);
				}
				wasSet = true;
				break;
			case "chatcommandsenabled":
				ChatCommandsEnabled = confValue.ToLower () == "true" ? true : false;
				wasSet = true;
				break;
			}

			if (wasSet) {
				if (bSave) {
					SaveConfig ();
				}
				return true;
			}

			return false;
		}

		public void RegisterChatCommand(string cmd, string description, string helptext, OnChatCommandDelegate handler, string permissionNode=""){
			//if (ChatCommandsEnabled) {
				if (_chatCommands.ContainsKey (cmd)) {
					Log.Warning ("[ChatCommands] Handler Exists for command: " + cmd);
					return;
				}

				ChatCommand chCommand = new ChatCommand (cmd, description, helptext, handler, permissionNode);

				_chatCommands.Add (cmd, chCommand);
				SDTM.API.Permissions.RegisterPermission ("chatcommand." + cmd);
			//}
		}

		public override void CalcChunkColorsDone (Chunk _chunk)
		{
			DictionarySave<Vector3i, TileEntity> tileEntitiesInChunk = _chunk.GetTileEntities ();

			foreach (TileEntity tileEntity in tileEntitiesInChunk.Values) {
				RegisterTEHandlers (tileEntity);
			}

			API.Events.NotifyChunkColorsDoneHandlers (_chunk);
		}

		private void RegisterTEHandlers(TileEntity tileEntity){
			if (tileEntity is TileEntityLootContainer) {
				if (API.Events.hasLootContainerChangedHandlers) {
				
					tileEntity.listeners.Add (new TELootContainerChangedHandler (tileEntity as TileEntityLootContainer));
				}
				return;
			}

			if (tileEntity is TileEntityPowerSource) {
				tileEntity.listeners.Add(new TEPowerSourceChangedHandler(tileEntity));
			}

			if (tileEntity is TileEntityPoweredTrigger) {
				tileEntity.listeners.Add (new TEPoweredTriggerChangedHandler (tileEntity));
			}

			if (tileEntity is TileEntityPoweredBlock) {
				tileEntity.listeners.Add (new TEPoweredBlockChangedHandler (tileEntity));
			}
		}

		public override bool ChatMessage (ClientInfo _cInfo, EnumGameMessages _type, string _msg, string _mainName, bool _localizeMain, string _secondaryName, bool _localizeSecondary)
		{
			
			if (API.Events.hasChatHandlers || _chatCommands.Count>0) {
				if (_type == EnumGameMessages.Chat) {
					if (_msg.Substring (0, 1) == "!") {
						string[] cmdParts = SplitArguments (_msg);
						string cmd = cmdParts [0].Replace ("!", "");
						string paramString = "";
						List<string> cmdParams = new List<string> ();

						if (cmdParts.Length > 1) {
							for (int i = 1; i < cmdParts.Length; i++) {
								cmdParams.Add (cmdParts [i]);
								paramString += cmdParts [i];
								if (i <= cmdParts.Length - 1) {
									paramString += " ";
								}
							}
						}

						if (_chatCommands.ContainsKey (cmd)) {
							_chatCommands [cmd].Execute (cmdParams, _cInfo);
						}
						API.Events.NotifyChatCommandHandlers (_cInfo, cmd, paramString);
						return false;
					}
				}
			}

			if (API.Events.hasPlayerKilledHandlers) {
				if (_type == EnumGameMessages.EntityWasKilled) {
					
					EntityPlayer killedPlayer = PlayerUtils.GetEntityPlayer (_mainName);
					EntityPlayer killerPlayer = PlayerUtils.GetEntityPlayer (_secondaryName);

					if (killerPlayer == null) {
						OnPlayerDied (killedPlayer);
					} else {
						OnPlayerKilledPlayer (killedPlayer, killerPlayer);
					}
				}
			}

			return true;
		}

		public static string[] SplitArguments(string commandLine)
		{
			var parmChars = commandLine.ToCharArray();
			var inSingleQuote = false;
			var inDoubleQuote = false;
			for (var index = 0; index < parmChars.Length; index++)
			{
				if (parmChars[index] == '"' && !inSingleQuote)
				{
					inDoubleQuote = !inDoubleQuote;
					parmChars[index] = '\n';
				}
				if (parmChars[index] == '\'' && !inDoubleQuote)
				{
					inSingleQuote = !inSingleQuote;
					parmChars[index] = '\n';
				}
				if (!inSingleQuote && !inDoubleQuote && parmChars[index] == ' ')
					parmChars[index] = '\n';
			}
			return (new string(parmChars)).Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		}

		public override void GameAwake ()
		{
			API.Events.NotifyGameStartedHandlers ();
		}

		public override void GameShutdown ()
		{
			if (this.webServer != null) {
				this.webServer.Stop();
			}
			API.Events.NotifyGameStoppedHandlers ();
		}

		public override void GameStartDone ()
		{
			//register the additional event handlers as required
			if (API.Events.hasPersistentPlayerDataHandlers) {
				GameManager.Instance.GetPersistentPlayerList ().AddPlayerEventHandler (new PersistentPlayerData.PlayerEventHandler (OnPersistentPlayerEvent));
			}

			API.Events.NotifyGameStartDoneHandlers ();

			World w = GameManager.Instance.World;

			//if (API.Events.hasBlockChangeHandlers) {
				w.ChunkCache.OnBlockChangedDelegates += new ChunkCluster.OnBlockChangedDelegate (OnBlockChanged);

			//}

			//if (API.Events.hasBlockChangeHandlers) {
				w.ChunkCache.OnBlockDamagedDelegates += new ChunkCluster.OnBlockDamagedDelegate (OnBlockDamaged);
			//}

			//if (API.Events.hasEntityLoadedHandlers) {
				w.EntityLoadedDelegates+= new World.OnEntityLoadedDelegate(OnEntityLoaded);
			//}

			//if (API.Events.hasEntityUnloadedHandlers) {
				w.EntityUnloadedDelegates += new World.OnEntityUnloadedDelegate (OnEntityUnloaded);
			//}

			//if (API.Events.hasGameStatsChangedHandlers) {
				GameStats.OnChangedDelegates += new GameStats.OnChangedDelegate(OnGameStatsChanged);
			//}

			//if (API.Events.hasGamePrefsChangedHandlers) {
				GamePrefs.AddChangeListener (new GamePrefChangedHandler ());
			//}

			if (API.Events.hasLibTickHandlers) {
				doTick = true;
			}

			if (API.Events.hasChunkAddedHandlers) {
				chunkCbHandler = new ChunkCallbackHandler ();
			}

			//start the web server if applicable
			if (WebConsoleEnabled) {
				if (WebConsoleEnabled || Servers.HTTP.EndPointsList._endPoints.Count > 0) {
					WWWEndpointProvider homePageProvider = new Servers.EndPoints.EndPoint_Home ();

					Servers.HTTP.EndPointsList.RegisterEndpoint ("/", homePageProvider);
					Servers.HTTP.EndPointsList.RegisterEndpoint ("/login/", new Servers.EndPoints.EndPoint_PasswordLogin ());
					Servers.HTTP.EndPointsList.RegisterEndpoint ("/steam/", new Servers.EndPoints.EndPoint_SteamLogin ());
					Servers.HTTP.EndPointsList.RegisterEndpoint ("/settings/", new Servers.EndPoints.EndPoint_Settings ());
					Servers.HTTP.EndPointsList.RegisterEndpoint ("/settings/experm/", new Servers.EndPoints.Endpoint_ExpermSettings ());
					Servers.HTTP.EndPointsList.RegisterEndpoint ("/settings/experm/group/", new Servers.EndPoints.EndPoint_ExpermGroup ());
					Servers.HTTP.EndPointsList.RegisterEndpoint ("/settings/experm/group/removenode/", new Servers.EndPoints.EndPoint_ExPermRemoveGroupNode ());
					Servers.HTTP.EndPointsList.RegisterEndpoint ("/settings/experm/group/addnode/", new Servers.EndPoints.EndPoint_ExPermAddGroupNode ());
					Servers.HTTP.EndPointsList.RegisterEndpoint ("/settings/experm/user/", new Servers.EndPoints.EndPoint_ExpermUser ());
					Servers.HTTP.EndPointsList.RegisterEndpoint ("/settings/experm/user/removenode/", new Servers.EndPoints.EndPoint_ExPermRemoveUserNode ());
					Servers.HTTP.EndPointsList.RegisterEndpoint ("/settings/experm/user/addnode/", new Servers.EndPoints.EndPoint_ExPermAddUserNode ());
					Servers.HTTP.EndPointsList.RegisterEndpoint ("/settings/onlineplayers/", new Servers.EndPoints.EndPoint_OnlinePlayers ());

					this.webServer = new Servers.HTTP.WWW ("*", WebConsolePort);
					this.webServer.Run ();
				}
			}
		}

		public override void GameUpdate ()
		{
			API.Events.NotifyGameUpdateHandlers ();

			if (doTick == true) {
				if (tickCount % tickRate == 0) {
					tickCount = 1;

					Tick ();
				} else {
					tickCount++;
				}
			}
		}

		public override void PlayerDisconnected (ClientInfo _cInfo, bool _bShutdown)
		{
			API.Events.NotifyPlayerDisconnectedHandlers (_cInfo);
		}

		public override void PlayerLogin (ClientInfo _cInfo, string _compatibilityVersion)
		{
			API.Events.NotifyPlayerLoginHandlers (_cInfo, _compatibilityVersion);

		}

		public override void PlayerSpawning (ClientInfo _cInfo, int _chunkViewDim, PlayerProfile _playerProfile)
		{
			if (API.Events.hasPlayerSpawningHandlers) {
				API.Events.NotifyPlayerSpawningHandlers (_cInfo);
			}
		}

		public override void SavePlayerData (ClientInfo _cInfo, PlayerDataFile _playerDataFile)
		{
			API.Events.NotifyPlayerDataSavedHandlers (_cInfo, _playerDataFile);
		}

		public override void PlayerSpawnedInWorld (ClientInfo _cInfo, RespawnType _respawnReason, Vector3i _pos)
		{
			Log.Out (_respawnReason.ToString());
			if (_respawnReason == RespawnType.JoinMultiplayer || _respawnReason == RespawnType.EnterMultiplayer) {
				EntityPlayer eplayer = PlayerUtils.GetEntityPlayer (_cInfo);
				Log.Out ("PLAYER JOINED");
				if (API.Events.hasPlayerBuffAddedHandlers || API.Events.hasPlayerBuffRemovedHandlers) {
					Log.Out ("ADDING BUFF LISTENERS");
					EntityBuffChangedHandler bch = new EntityBuffChangedHandler (eplayer);
					eplayer.Stats.AddBuffChangedDelegate (bch);
				}

				if (API.Events.hasPlayerWellnessChangedHandlers) {
					EntityWellnessChangedHandler ech = new EntityWellnessChangedHandler (eplayer);
					eplayer.Stats.AddWellnessChangedDelegate (ech);
				}
					
				//eplayer.inventory.AddChangeListener (new InventoryChangedHandler (eplayer));
			}

			API.Events.NotifyPlayerSpawnedHandlers(_cInfo, _respawnReason, _pos);
		}

		/*
		 * Custom Event Handlers 
		 */

		private void OnGameStatsChanged(EnumGameStats _stat, object _newValue){
			API.Events.NotifyGameStatsChangedHandlers (_stat, _newValue);
		}

		public void OnGameTimeChange(int _day, int _hour, int _minute){
			API.Events.NotifyGameMinuteHandlers (_day, _hour, _minute);
		}

		public void OnPersistentPlayerEvent(PersistentPlayerData ppData, PersistentPlayerData otherPlayer, EnumPersistentPlayerDataReason reason){
			API.Events.NotifyPersistentPlayerDataEventHandlers (ppData, otherPlayer, reason);
		}

		private void OnPlayerDied(EntityPlayer killedPlayer){
			API.Events.NotifyPlayerDiedHandlers (killedPlayer);
		}

		private void OnPlayerKilledPlayer(EntityPlayer killedPlayer, EntityPlayer killerPlayer){
			API.Events.NotifyPlayerKilledByPlayerHandlers (killedPlayer, killerPlayer);
		}

		private void OnEntityLoaded(Entity _entity) {
			
			API.Events.NotifyEntityLoadedHandlers (_entity);
			if (_entity is EntityNPC) {
				EntityNPC npc = _entity as EntityNPC;

				if (npc.TileEntityTrader != null) {
					npc.TileEntityTrader.listeners.Add (new TENPCChangedHandler (npc.TileEntityTrader));
				}
			}
		}

		private void OnEntityUnloaded(Entity _entity, EnumRemoveEntityReason _reason) {
			API.Events.NotifyEntityUnloadedHandlers (_entity, _reason);
		}

		private void OnBlockChanged (Vector3i _blockPos, BlockValue _blockValueOld, BlockValue _blockValueNew){
			API.Events.NotifyBlockChangedHandlers (_blockPos, _blockValueOld, _blockValueNew);
			Chunk chunk = (Chunk)GameManager.Instance.World.GetChunkFromWorldPos (_blockPos);
			TileEntity te = GameManager.Instance.World.GetTileEntity (chunk.ClrIdx, _blockPos);
			if (te != null) {
				RegisterTEHandlers (te);
			}

		}

		private void OnBlockDamaged (Vector3i _blockPos, BlockValue _blockValue, int _damage, int _attackerEntityId){
			API.Events.NotifyBlockDamagedHandlers (_blockPos, _blockValue, _damage, _attackerEntityId);
		}

		private void Tick(){
			World w = GameManager.Instance.World;
			ulong time = w.GetWorldTime ();

			if (lastTime != time) {
				lastTime = time;
				int thisDay = GameUtils.WorldTimeToDays (time);
				int thisHour = GameUtils.WorldTimeToHours (time);
				int thisMinute = GameUtils.WorldTimeToMinutes (time);
				if (thisDay != day || thisHour != hour || thisMinute != minute) {
					day = thisDay;
					hour = thisHour;
					minute = thisMinute;
					OnGameTimeChange(day, hour, minute);
				}
			}
		}
	}
}

