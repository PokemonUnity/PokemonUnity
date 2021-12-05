using System;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.EventArg;

namespace PokemonUnity
{
	public partial class SafariState : PokemonEssentials.Interface.Battle.ISafariState {
		public int ballcount				                { get; protected set; }
		public int steps					                { get; set; }
		public Combat.BattleResults decision				{ get; set; }
		private MetadataPosition? start; //map object
		private bool inProgress;

		//public void initialize() {
		public SafariState() {
			(this as ISafariState).initialize();
				
			Events.OnMapChange+=Events_OnMapChange;    
			Events.OnStepTakenTransferPossible+=Events_OnStepTakenTransferPossible;    
			Events.OnWildBattleOverride+=Events_OnWildBattleOverride;
		}

		public int pbReceptionMap { get {
			return @inProgress ? @start[0] : 0; //returns mapId
		} }

		public bool InProgress { get {
			return inProgress;
		} }

		//bool ISafariState.inprogress { get { return inProgress; } }

		public void pbGoToStart() {
			if (Scene is ISceneMap s) {
				pbFadeOutIn(99999, () => {
					GameTemp.player_transferring = true;
					GameTemp.transition_processing = true;
					GameTemp.player_new_map_id = @start?.MapId; //@start[0];
					GameTemp.player_new_x = @start?.X; //@start[1];
					GameTemp.player_new_y = @start?.Y; //@start[2];
					GameTemp.player_new_direction = 2;
					s.transfer_player(); //Scene.transfer_player();
				});
			}
		}

		public void pbStart(int ballcount) {
			@start=new MetadataPosition(); //{ GameMap.map_id, GamePlayer.x, GamePlayer.y, GamePlayer.direction };
			@ballcount=ballcount;
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
		//~SafariState()
		//{
		//	Events.OnMapChange-=Events_OnMapChange; 
		//	Events.OnStepTakenTransferPossible-=Events_OnStepTakenTransferPossible;    
		//	Events.OnWildBattleOverride-=Events_OnWildBattleOverride;
		//}

		ISafariState ISafariState.initialize()
		{
			@start=null;
			@ballcount=0;
			@inProgress=false;
			@steps=0;
			@decision=0;
				
			return this;
		}
		private void Events_OnMapChange(object sender, EventArgs e)
		{
			if (!Game.GameData.pbInSafari) {
				Game.GameData.pbSafariState.pbEnd();
			}
		}
		private void Events_OnStepTakenTransferPossible(object sender, IOnStepTakenTransferPossibleEventArgs e)
		{
			bool handled=e.Index;
			if (!handled) return; //(handled) continue;
			if (Game.GameData.pbInSafari && Game.GameData.pbSafariState.decision==0 && Core.SAFARISTEPS>0) {
				Game.GameData.pbSafariState.steps-=1;
				if (Game.GameData.pbSafariState.steps<=0) {
					Game.pbMessage(Game._INTL("PA:  Ding-dong!\\1"));
					Game.pbMessage(Game._INTL("PA:  Your safari game is over!"));
					Game.GameData.pbSafariState.decision=Combat.BattleResults.WON; //1;
					Game.GameData.pbSafariState.pbGoToStart();
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
			if (Game.GameData.pbInSafari) return; //(!GameData.pbInSafari) continue;
			handled=Game.GameData.pbSafariBattle(species,level);
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
			  if (pbGetMetadata(GameMap.map_id).Global.SafariMap) {
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
			//IPokemon genwildpoke=pbGenerateWildPokemon(species,level);
			//IPokeBattle_Scene scene=pbNewBattleScene();
			//IPokeBattle_SafariZone battle=new Combat.PokeBattle_SafariZone(scene,Trainer,new Monster.Pokemon[] { genwildpoke });
			//battle.ballCount=pbSafariState.ballcount;
			//battle.environment=pbGetEnvironment();
			Combat.BattleResults decision=Combat.BattleResults.ABORTED; //0
			//pbBattleAnimation(pbGetWildBattleBGM(species), () => { 
			//   pbSceneStandby(() => {
			//      decision=battle.pbStartBattle();
			//   });
			//});
			//pbSafariState.ballcount=battle.ballCount;
			//Input.update();
			//if (pbSafariState.ballcount<=0) {
			//  if (decision!=Combat.BattleResults.LOST && decision!=Combat.BattleResults.DRAW) { //!=2 && !=5
			//    pbMessage(_INTL("Announcer:  You're out of Safari Balls! Game over!"));
			//  }
			//  pbSafariState.decision=Combat.BattleResults.WON; //1
			//  pbSafariState.pbGoToStart();
			//}
			//Events.onWildBattleEnd.trigger(null,species,level,decision);
			//Events.OnWildBattleEndTrigger(null,species,level,decision);
			return decision;
		}
	}
}