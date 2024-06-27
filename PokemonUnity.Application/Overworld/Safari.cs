using System;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonUnity.EventArg;

namespace PokemonUnity
{
	public partial class SafariState : PokemonEssentials.Interface.Field.ISafariState {
		public int ballcount								{ get; set; }
		public int steps									{ get; set; }
		public Combat.BattleResults decision				{ get; set; }
		private MetadataPosition? start; //map object
		private bool inProgress;

		public SafariState() {
			initialize();

			Events.OnMapChange+=Events_OnMapChange;
			Events.OnStepTakenTransferPossible+=Events_OnStepTakenTransferPossible;
			Events.OnWildBattleOverride+=Events_OnWildBattleOverride;
		}

		public int ReceptionMap { get {
			//return @inProgress ? @start[0] : 0; //returns mapId
			return @inProgress ? @start?.MapId??0 : 0;
		} }

		public bool InProgress { get {
			return inProgress;
		} }

		//bool ISafariState.inprogress { get { return inProgress; } }

		public void GoToStart() {
			if (Game.GameData.Scene is ISceneMap s && Game.GameData is IGameSpriteWindow g) {
				g.FadeOutIn(99999, block: () => {
					Game.GameData.GameTemp.player_transferring = true;
					Game.GameData.GameTemp.transition_processing = true;
					Game.GameData.GameTemp.player_new_map_id = @start?.MapId??0; //@start[0];
					Game.GameData.GameTemp.player_new_x = @start?.X??0; //@start[1];
					Game.GameData.GameTemp.player_new_y = @start?.Y??0; //@start[2];
					Game.GameData.GameTemp.player_new_direction = 2;
					s.transfer_player(); //Scene.transfer_player();
				});
			}
		}

		public void Start(int ballcount_) {
			@start=null;
			if (Game.GameData.GameMap is IGameMapOrgBattle gmo)
				@start=new MetadataPosition() { MapId = gmo.map_id, X = Game.GameData.GamePlayer.x, Y = Game.GameData.GamePlayer.y, Direction = Game.GameData.GamePlayer.direction };
			@ballcount=ballcount_;
			@inProgress=true;
			@steps=Core.SAFARISTEPS;
		}

		public void End() {
			@start=null;
			@ballcount=0;
			@inProgress=false;
			@steps=0;
			@decision=0;
			Game.GameData.GameMap.need_refresh=true;
		}
		~SafariState()
		{
			Events.OnMapChange -= Events_OnMapChange;
			Events.OnStepTakenTransferPossible -= Events_OnStepTakenTransferPossible;
			Events.OnWildBattleOverride -= Events_OnWildBattleOverride;
		}

		public ISafariState initialize()
		{
			@start=null;
			@ballcount=0;
			@inProgress=false;
			@steps=0;
			@decision=0;

			return this;
		}
		protected virtual void Events_OnMapChange(object sender, IOnMapChangeEventArgs e)
		{
			if (Game.GameData is IGameSafari s && !s.InSafari) {
				s.SafariState.End();
			}
		}
		protected virtual void Events_OnStepTakenTransferPossible(object sender, IOnStepTakenTransferPossibleEventArgs e)
		{
			bool handled=e.Index;
			if (!handled) return; //(handled) continue;
			if (Game.GameData is IGameSafari s && s.InSafari && s.SafariState.decision==0 && Core.SAFARISTEPS>0) {
				s.SafariState.steps-=1;
				if (s.SafariState.steps<=0) {
					if (Game.GameData is IGameMessage m0) m0.Message(Game._INTL("PA:  Ding-dong!\\1"));
					if (Game.GameData is IGameMessage m1) m1.Message(Game._INTL("PA:  Your safari game is over!"));
					s.SafariState.decision=Combat.BattleResults.WON; //1;
					s.SafariState.GoToStart();
					handled=true;
				}
			}
		}
		protected virtual void Events_OnWildBattleOverride(object sender, IOnWildBattleOverrideEventArgs e)
		{
			Pokemons species=e.Species;
			int level=e.Level;
			Combat.BattleResults? handled=e.Result;
			if (handled==null) return; //(handled!=null) continue;
			if (Game.GameData is IGameSafari s && s.InSafari) return; //(!GameData.InSafari) continue;
			handled=Game.GameData is IGameSafari s0 ? s0.SafariBattle(species,level) : (Combat.BattleResults?)null;
		}
	}

	public partial class Game : IGameSafari {
		//event EventHandler IGameSafari.OnMapChange { add { OnMapChange += value; } remove { OnMapChange -= value; } }
		//event Action<object, IOnStepTakenTransferPossibleEventArgs> IGameSafari.OnStepTakenTransferPossible { add { OnStepTakenTransferPossible += value; } remove { OnStepTakenTransferPossible += value; } }
		//event Action<object, IOnWildBattleOverrideEventArgs> IGameSafari.OnWildBattleOverride { add { OnWildBattleOverride += value; } remove { OnWildBattleOverride += value; } }

		public bool InSafari { get {
			if (SafariState.InProgress && GameMap is IGameMapOrgBattle gmo) {
				//  Reception map is handled separately from safari map since the reception
				//  map can be outdoors, with its own grassy patches.
				int reception=SafariState.ReceptionMap;
				if (gmo.map_id==reception) return true;
				//if (GetMetadata(GameMap.map_id,MetadataSafariMap)) {
				if (GetMetadata(gmo.map_id).Map.SafariMap) {
					return true;
				}
			}
			return false;
		} }

		public ISafariState SafariState { get {
			if (Global.safariState == null) {
				Global.safariState=new SafariState();
			}
			return Global.safariState;
		} }

		public Combat.BattleResults SafariBattle(Pokemons species,int level) {
			IPokemon genwildpoke=(this as IGameField).GenerateWildPokemon(species,level);
			IPokeBattle_Scene scene=(this as IGameField).NewBattleScene();
			ISafariZone_Scene battle=new Combat.PokeBattle_SafariZone(scene,Trainer,null,new IPokemon[] { genwildpoke });
			battle.ballCount=SafariState.ballcount;
			if (this is IGameField f0) battle.environment=f0.GetEnvironment();
			Combat.BattleResults decision=Combat.BattleResults.ABORTED; //0
			if (this is IGameField f1) f1.BattleAnimation(GetWildBattleBGM(species), block: () => {
				f1.SceneStandby(block: () => {
					decision=battle.StartBattle();
				});
			});
			SafariState.ballcount=battle.ballCount;
			Input.update();
			if (SafariState.ballcount<=0) {
				if (decision!=Combat.BattleResults.LOST && decision!=Combat.BattleResults.DRAW) { //!=2 && !=5
					if (Game.GameData is IGameMessage m) m.Message(_INTL("Announcer:  You're out of Safari Balls! Game over!"));
				}
				SafariState.decision=Combat.BattleResults.WON; //1
				SafariState.GoToStart();
			}
			IOnWildBattleEndEventArgs e = new OnWildBattleEndEventArgs
			{
				Level = level,
				Species = species,
				Result = decision
			};
			//Events.onWildBattleEnd.trigger(null,species,level,decision);
			Events.OnWildBattleEndTrigger(null,species,level,decision);
			//Events.OnWildBattleEnd?.Invoke(this, e);
			return decision;
		}
	}
}