using System;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.EventArg;

namespace PokemonUnity
{
    public partial class Game { 
//Should this be nested under Overworld namespace, or Game class?
public partial class SafariState : PokemonEssentials.Interface.ISafariState {
  public int ballcount				                { get; protected set; }
  public int steps					                { get; set; }
  public Combat.BattleResults decision				{ get; set; }
  private int[] start; //map object
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

	bool ISafariState.inprogress { get { return inProgress; } }

  public void pbGoToStart() {
    //if (Scene is Scene_Map s) {
    //  UI.pbFadeOutIn(99999, () => {
    //     GameTemp.player_transferring = true;
    //     GameTemp.transition_processing = true;
    //     GameTemp.player_new_map_id = @start[0];
    //     GameTemp.player_new_x = @start[1];
    //     GameTemp.player_new_y = @start[2];
    //     GameTemp.player_new_direction = 2;
    //     s.transfer_player(); //Scene.transfer_player();
    //  });
    //}
  }

  public void pbStart(int ballcount) {
    @start=new int[0]; //{ GameMap.map_id, GamePlayer.x, GamePlayer.y, GamePlayer.direction };
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
    GameData.GameMap.need_refresh=true;
  }
	~SafariState()
	{
        Events.OnMapChange-=Events_OnMapChange; 
        Events.OnStepTakenTransferPossible-=Events_OnStepTakenTransferPossible;    
        Events.OnWildBattleOverride-=Events_OnWildBattleOverride;
	}

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
         if (!GameData.pbInSafari) {
           GameData.pbSafariState.pbEnd();
         }
	}
	private void Events_OnStepTakenTransferPossible(object sender, OnStepTakenTransferPossibleEventArgs e)
	{
        bool handled=e.Index;
        if (!handled) return; //(handled) continue;
        if (GameData.pbInSafari && GameData.pbSafariState.decision==0 && Core.SAFARISTEPS>0) {
            GameData.pbSafariState.steps-=1;
            if (GameData.pbSafariState.steps<=0) {
                pbMessage(_INTL("PA:  Ding-dong!\\1"));
                pbMessage(_INTL("PA:  Your safari game is over!"));
                GameData.pbSafariState.decision=Combat.BattleResults.WON; //1;
                GameData.pbSafariState.pbGoToStart();
                handled=true;
            }
        }
	}
	private void Events_OnWildBattleOverride(object sender, OnWildBattleOverrideEventArgs e)
	{
        Pokemons species=e.Species;
        int level=e.Level;
        Combat.BattleResults? handled=e.Result;
        if (handled==null) return; //(handled!=null) continue;
        if (GameData.pbInSafari) return; //(!GameData.pbInSafari) continue;
        handled=GameData.pbSafariBattle(species,level);
	}
}

public bool pbInSafari { get {
  //if (pbSafariState.InProgress) {
  //  //  Reception map is handled separately from safari map since the reception
  //  //  map can be outdoors, with its own grassy patches.
  //  int reception=pbSafariState.pbReceptionMap;
  //  if (GameMap.map_id==reception) return true;
  //  if (pbGetMetadata(GameMap.map_id,MetadataSafariMap)) {
  //    return true;
  //  }
  //}
  return false;
} }

public ISafariState pbSafariState { get {
  if (Global.safariState == null) {
    Global.safariState=new SafariState();
  }
  return Global.safariState;
} }

public Combat.BattleResults pbSafariBattle(Pokemons species,int level) {
  //Monster.Pokemon genwildpoke=pbGenerateWildPokemon(species,level);
  //IPokeBattle_Scene scene=pbNewBattleScene();
  //Combat.PokeBattle_SafariZone battle=new Combat.PokeBattle_SafariZone(scene,Trainer,new Monster.Pokemon[] { genwildpoke });
  //battle.ballCount=pbSafariState.ballcount;
  //battle.environment=pbGetEnvironment();
  Combat.BattleResults decision=Combat.BattleResults.ABORTED; //0
  //UI.pbBattleAnimation(UI.pbGetWildBattleBGM(species), () => { 
  //   UI.pbSceneStandby(() => {
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