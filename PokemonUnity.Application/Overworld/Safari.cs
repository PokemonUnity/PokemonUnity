using System;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;

namespace PokemonUnity
{
	public partial class SafariState : PokemonEssentials.Interface.Battle.ISafariState {
		public int ballcount				                { get; set; }
		public int steps					                { get; set; }
		public Combat.BattleResults decision				{ get; set; }
		private MetadataPosition? start; //map object
		private bool inProgress;

		public SafariState() {
			initialize();
				
			Events.OnMapChange+=Events_OnMapChange;    
			Events.OnStepTakenTransferPossible+=Events_OnStepTakenTransferPossible;    
			Events.OnWildBattleOverride+=Events_OnWildBattleOverride;
		}

		public int pbReceptionMap { get {
			//return @inProgress ? @start[0] : 0; //returns mapId
			return @inProgress ? @start?.MapId??0 : 0;
		} }

		public bool InProgress { get {
			return inProgress;
		} }

		//bool ISafariState.inprogress { get { return inProgress; } }

		public void pbGoToStart() {
			if (Game.GameData.Scene is ISceneMap s && Game.GameData is IGameSpriteWindow g) {
				g.pbFadeOutIn(99999, block: () => {
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

		public void pbStart(int ballcount_) {
			@start=new MetadataPosition() { MapId = Game.GameData.GameMap.map_id, X = Game.GameData.GamePlayer.x, Y = Game.GameData.GamePlayer.y, Direction = Game.GameData.GamePlayer.direction };
			@ballcount=ballcount_;
			@inProgress=true;
			@steps=Core.SAFARISTEPS;
		}

		public void pbEnd() {
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
		private void Events_OnMapChange(object sender, IOnMapChangeEventArgs e)
		{
			if (Game.GameData is IGameSafari s && !s.pbInSafari) {
				s.pbSafariState.pbEnd();
			}
		}
		private void Events_OnStepTakenTransferPossible(object sender, IOnStepTakenTransferPossibleEventArgs e)
		{
			bool handled=e.Index;
			if (!handled) return; //(handled) continue;
			if (Game.GameData is IGameSafari s && s.pbInSafari && s.pbSafariState.decision==0 && Core.SAFARISTEPS>0) {
				s.pbSafariState.steps-=1;
				if (s.pbSafariState.steps<=0) {
					if (Game.GameData is IGameMessage m0) m0.pbMessage(Game._INTL("PA:  Ding-dong!\\1"));
					if (Game.GameData is IGameMessage m1) m1.pbMessage(Game._INTL("PA:  Your safari game is over!"));
					s.pbSafariState.decision=Combat.BattleResults.WON; //1;
					s.pbSafariState.pbGoToStart();
					handled=true;
				}
			}
		}
		private void Events_OnWildBattleOverride(object sender, IOnWildBattleOverrideEventArgs e)
		{
			Pokemons species=e.Species;
			int level=e.Level;
			Combat.BattleResults? handled=e.Result;
			if (handled==null) return; //(handled!=null) continue;
			if (Game.GameData is IGameSafari s && s.pbInSafari) return; //(!GameData.pbInSafari) continue;
			handled=Game.GameData is IGameSafari s0 ? s0.pbSafariBattle(species,level) : (Combat.BattleResults?)null;
		}
	}

	public partial class Game : IGameSafari { 
		public bool pbInSafari { get {
			if (pbSafariState.InProgress) {
				//  Reception map is handled separately from safari map since the reception
				//  map can be outdoors, with its own grassy patches.
				int reception=pbSafariState.pbReceptionMap;
				if (GameMap.map_id==reception) return true;
				//if (pbGetMetadata(GameMap.map_id,MetadataSafariMap)) {
				if (pbGetMetadata(GameMap.map_id).Map.SafariMap) {
					return true;
				}
			}
			return false;
		} }

		public ISafariState pbSafariState { get {
			if (Global.safariState == null) {
				Global.safariState=new SafariState();
			}
			return Global.safariState;
		} }

		public Combat.BattleResults pbSafariBattle(Pokemons species,int level) {
			IPokemon genwildpoke=(this as IGameField).pbGenerateWildPokemon(species,level);
			IPokeBattle_Scene scene=(this as IGameField).pbNewBattleScene();
			ISafariZone battle=new Combat.PokeBattle_SafariZone(scene,Trainer,null,new IPokemon[] { genwildpoke });
			battle.ballCount=pbSafariState.ballcount;
			if (this is IGameField f0) battle.environment=f0.pbGetEnvironment();
			Combat.BattleResults decision=Combat.BattleResults.ABORTED; //0
			if (this is IGameField f1) f1.pbBattleAnimation(pbGetWildBattleBGM(species), block: () => { 
				f1.pbSceneStandby(block: () => {
					decision=battle.pbStartBattle();
				});
			});
			pbSafariState.ballcount=battle.ballCount;
			Input.update();
			if (pbSafariState.ballcount<=0) {
				if (decision!=Combat.BattleResults.LOST && decision!=Combat.BattleResults.DRAW) { //!=2 && !=5
					if (Game.GameData is IGameMessage m) m.pbMessage(_INTL("Announcer:  You're out of Safari Balls! Game over!"));
				}
				pbSafariState.decision=Combat.BattleResults.WON; //1
				pbSafariState.pbGoToStart();
			}
			//Events.onWildBattleEnd.trigger(null,species,level,decision);
			//Events.OnWildBattleEndTrigger(null,species,level,decision);
			return decision;
		}
	}
}