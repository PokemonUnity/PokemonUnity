using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;

namespace PokemonUnity
{
// ===============================================================================
// Battles
// ===============================================================================
public partial class GameTemp {
  public int background_bitmap				{ get; protected set; }
}

public partial class PokemonTemp {
  public EncounterTypes? encounterType	    { get; set; } 
  public int evolutionLevels			    { get; set; }
}

public partial class PokemonTemp {
  public int batterywarning				    { get; protected set; }
  public IAudioObject cueBGM				{ get; set; }
  public float? cueFrames				    { get; set; }
}

/// <summary>
/// This module stores encounter-modifying events that can happen during the game.
/// A procedure can subscribe to an event by adding itself to the event.  It will
/// then be called whenever the event occurs.
/// </summary>
public static partial class EncounterModifier {
  public static List<Func<Pokemon,Pokemon>> @procs=new List<Func<Pokemon,Pokemon>>();
  public static List<Action> @procsEnd=new List<Action>();

  public static void register(Func<Pokemon,Pokemon> p) {
    @procs.Add(p);
  }

  public static void registerEncounterEnd(Action p) {
    @procsEnd.Add(p);
  }

  public static Monster.Pokemon trigger(Monster.Pokemon encounter) {
    foreach (var prc in @procs) {
      //encounter=prc.call(encounter);
      encounter=prc.Invoke(encounter);
    }
    return encounter;
  }

  public static void triggerEncounterEnd() {
    foreach (var prc in @procsEnd) {
      //prc.call();
      prc.Invoke();
    }
  }
}

public partial class Game
{
public System.Collections.IEnumerator pbSceneStandby(Action action = null) {
  if (Game.GameData.Scene != null && Game.GameData.Scene is Scene_Map) {
    ((Scene_Map)Game.GameData.Scene).disposeSpritesets();
  }
  GC.Collect();
  Game.UI.frame_reset(); //Graphics.frame_reset();
  yield return null;
  action.Invoke();
  if (Game.GameData.Scene != null && Game.GameData.Scene is Scene_Map) {
    ((Scene_Map)Game.GameData.Scene).createSpritesets();
  }
}


public void pbPrepareBattle(Combat.Battle battle) {
  switch (Game.GameData.GameScreen.weather_type) {
  case FieldWeathers.Rain: case FieldWeathers.HeavyRain: case FieldWeathers.Thunderstorm:
    battle.weather=Combat.Weather.RAINDANCE;
    battle.weatherduration=-1;
    break;
  case FieldWeathers.Snow: case FieldWeathers.Blizzard:
    battle.weather=Combat.Weather.HAIL;
    battle.weatherduration=-1;
    break;
  case FieldWeathers.Sandstorm:
    battle.weather=Combat.Weather.SANDSTORM;
    battle.weatherduration=-1;
    break;
  case FieldWeathers.Sunny:
    battle.weather=Combat.Weather.SUNNYDAY;
    battle.weatherduration=-1;
    break;
  }
  battle.shiftStyle=(Game.GameData.PokemonSystem.battlestyle==0);
  battle.battlescene=(Game.GameData.PokemonSystem.battlescene==0);
  battle.environment=pbGetEnvironment();
}

public Environments pbGetEnvironment() {
  if (Game.GameData.GameMap == null) return Environments.None;
  if (Game.GameData.Global != null && Game.GameData.Global.diving) {
    return Environments.Underwater;
  } else if (Game.GameData.PokemonEncounters != null && Game.GameData.PokemonEncounters.isCave()) {
    return Environments.Cave;
  } else if (pbGetMetadata(Game.GameData.GameMap.map_id,MapMetadatas.MetadataOutdoor) == null) {
    return Environments.None;
  } else {
    switch (Game.GameData.GamePlayer.terrain_tag) {
    case Terrains.Grass:
           return Environments.Grass;       // Normal grass
      break;
    case Terrains.Sand:
            return Environments.Sand;
      break;
    case Terrains.Rock:
            return Environments.Rock;
      break;
    case Terrains.DeepWater:
       return Environments.MovingWater;
      break;
    case Terrains.StillWater:
      return Environments.StillWater;
      break;
    case Terrains.Water:
           return Environments.MovingWater;
      break;
    case Terrains.TallGrass:
       return Environments.TallGrass;   // Tall grass
      break;
    case Terrains.SootGrass:
       return Environments.Grass;       // Sooty tall grass
      break;
    case Terrains.Puddle:
          return Environments.StillWater;
      break;
    }
  }
  return Environments.None;
}

public Pokemon pbGenerateWildPokemon(Pokemons species,int level,bool isroamer=false) {
  Pokemon genwildpoke=new Monster.Pokemon(species,level: (byte)level);//,Game.GameData.Trainer
  //Items items=genwildpoke.wildHoldItems;
  Items[] items=Game.PokemonItemsData[species] //ToDo: Return Items[3];
                .OrderByDescending(x => x.Rarirty)
                .Select(x => x.ItemId).ToArray();
  Monster.Pokemon firstpoke=Game.GameData.Trainer.firstParty;
  int[] chances= new int[]{ 50,5,1 };
  if (firstpoke != null && !firstpoke.isEgg &&
                       firstpoke.Ability == Abilities.COMPOUND_EYES) chances= new int[]{ 60,20,5 };
  int itemrnd=Core.Rand.Next(100);
  if (itemrnd<chances[0] || (items[0]==items[1] && items[1]==items[2])) {
    genwildpoke.setItem(items[0]);
  } else if (itemrnd<(chances[0]+chances[1])) {
    genwildpoke.setItem(items[1]);
  } else if (itemrnd<(chances[0]+chances[1]+chances[2])) {
    genwildpoke.setItem(items[2]);
  }
  if (Game.GameData.Bag.pbQuantity(Items.SHINY_CHARM)>0) { //hasConst(PBItems,:SHINYCHARM) && 
    for (int i = 0; i < 2; i++) {	// 3 times as likely
      if (genwildpoke.IsShiny) break;
      //genwildpoke.PersonalId=Core.Rand.Next(65536)|(Core.Rand.Next(65536)<<16);
      genwildpoke.shuffleShiny();
    }
  }
  if (Core.Rand.Next(65536)<Core.POKERUSCHANCE) {
    genwildpoke.GivePokerus();
  }
  if (firstpoke != null && !firstpoke.isEgg) {
    if (firstpoke.Ability == Abilities.CUTE_CHARM &&
       !genwildpoke.IsSingleGendered) {
      if (firstpoke.IsMale) {
        if (Core.Rand.Next(3)<2) genwildpoke.makeFemale(); else genwildpoke.makeMale();
      } else if (firstpoke.IsFemale) {
        if (Core.Rand.Next(3)<2) genwildpoke.makeMale(); else genwildpoke.makeFemale();
      }
    } else if (firstpoke.Ability == Abilities.SYNCHRONIZE) {
      if (!isroamer && Core.Rand.Next(10)<5) genwildpoke.setNature(firstpoke.Nature);
    }
  }
  //Events.onWildPokemonCreate.trigger(null,genwildpoke);
  //Events.OnWildPokemonCreateEventArgs eventArgs = new Events.OnWildPokemonCreateEventArgs()
  //{
  //  Pokemon = genwildpoke
  //};
  //Events.OnWildPokemonCreate?.Invoke(null,eventArgs);
  return genwildpoke;
}

public bool pbWildBattle(Pokemons species,int level,int? variable=null,bool canescape=true,bool canlose=false) {
  if ((Input.press(Input.CTRL) && Core.DEBUG) || Game.GameData.Trainer.pokemonCount==0) {
    if (Game.GameData.Trainer.pokemonCount>0) {
      Game.pbMessage(Game._INTL("SKIPPING BATTLE..."));
    }
    pbSet(variable,1);
    Game.GameData.Global.nextBattleBGM=null;
    Game.GameData.Global.nextBattleME=null;
    Game.GameData.Global.nextBattleBack=null;
    return true;
  }
  //if (species is String || species is Symbol) {
  //  species=getID(PBSpecies,species);
  //}
  bool?[] handled= new bool?[]{ null };
  //Events.onWildBattleOverride.trigger(null,species,level,handled);
  //Events.OnWildBattleOverride?.Invoke(null,species,level,handled);
  if (handled[0]!=null) {
    return handled[0].Value;
  }
  List<int> currentlevels=new List<int>();
  foreach (var i in Game.GameData.Trainer.party) {
    currentlevels.Add(i.Level);
  }
  Monster.Pokemon genwildpoke=pbGenerateWildPokemon(species,level);
  //Events.onStartBattle.trigger(null,genwildpoke);
  //Events.OnStartBattle.Invoke(null,genwildpoke);
  IPokeBattle_Scene scene=Game.UI.pbNewBattleScene();
  Combat.Battle battle=new Combat.Battle(scene,Game.GameData.Trainer.party,new Monster.Pokemon[] { genwildpoke },new Combat.Trainer[] { Game.GameData.Trainer },null);
  battle.internalbattle=true;
  battle.cantescape=!canescape;
  pbPrepareBattle(battle);
  Combat.BattleResults decision=0;
  Game.UI.pbBattleAnimation(Game.UI.pbGetWildBattleBGM(species), () => { 
     pbSceneStandby(() => {
        decision=battle.pbStartBattle(canlose);
     });
     foreach (var i in Game.GameData.Trainer.party) { i.makeUnmega(); } //rescue null
     if (Game.GameData.Global.partner != null) {
       Game.GameData.pbHealAll();
       foreach (Monster.Pokemon i in Game.GameData.Global.partner.party) { //partner[3]
         i.Heal();
         i.makeUnmega(); //rescue null
       }
     }
     if (decision==Combat.BattleResults.LOST || decision==Combat.BattleResults.DRAW) {		// If loss or draw
       if (canlose) {
         foreach (var i in Game.GameData.Trainer.party) { i.Heal(); }
         for (int i = 0; i < 10; i++) {
           Game.UI.update(); //Graphics.update();
         }
//     } else {
//         Game.GameData.GameSystem.bgm_unpause()
//         Game.GameData.GameSystem.bgs_unpause()
//         Game.pbStartOver()
       }
     }
     //Events.onEndBattle.trigger(null,decision,canlose);
  });
  Input.update();
  pbSet(variable,decision);
  //Events.onWildBattleEnd.trigger(null,species,level,decision);
  //Events.OnWildBattleEnd.Invoke(null,species,level,decision);
  return (decision!=Combat.BattleResults.LOST);
}

public bool pbDoubleWildBattle(Pokemons species1,int level1,Pokemons species2,int level2,int? variable=null,bool canescape=true,bool canlose=false) {
  if ((Input.press(Input.CTRL) && Core.DEBUG) || Game.GameData.Trainer.pokemonCount==0) {
    if (Game.GameData.Trainer.pokemonCount>0) {
      Game.pbMessage(Game._INTL("SKIPPING BATTLE..."));
    }
    pbSet(variable,1);
    Game.GameData.Global.nextBattleBGM=null;
    Game.GameData.Global.nextBattleME=null;
    Game.GameData.Global.nextBattleBack=null;
    return true;
  }
  //if (species1 is String || species1 is Symbol) {
  //  species1=getID(PBSpecies,species1);
  //}
  //if (species2 is String || species2 is Symbol) {
  //  species2=getID(PBSpecies,species2);
  //}
  List<int> currentlevels=new List<int>();
  foreach (var i in Game.GameData.Trainer.party) {
    currentlevels.Add(i.Level);
  }
  Pokemon genwildpoke=pbGenerateWildPokemon(species1,level1);
  Pokemon genwildpoke2=pbGenerateWildPokemon(species2,level2);
  //Events.onStartBattle.trigger(null,genwildpoke);
  //Events.OnStartBattle.Invoke(null,genwildpoke);
  IPokeBattle_Scene scene=Game.UI.pbNewBattleScene();
  Combat.Battle battle;
  if (Game.GameData.Global.partner != null) {
    Combat.Trainer othertrainer=new Combat.Trainer(
       Game.GameData.Global.partner.trainerTypeName,Game.GameData.Global.partner.trainertype);//[1]|[0]
    othertrainer.id=Game.GameData.Global.partner.id;//[2]
    othertrainer.party=Game.GameData.Global.partner.party;//[3]
    List<Monster.Pokemon> combinedParty=new List<Monster.Pokemon>();
    for (int i = 0; i < Game.GameData.Trainer.party.Length; i++) {
      combinedParty[i]=Game.GameData.Trainer.party[i];
    }
    for (int i = 0; i < othertrainer.party.Length; i++) {
      combinedParty[6+i]=othertrainer.party[i];
    }
    battle=new Combat.Battle(scene,combinedParty.ToArray(),new Pokemon[] { genwildpoke, genwildpoke2 },
       new Combat.Trainer[] { Game.GameData.Trainer,othertrainer },null);
    battle.fullparty1=true;
  } else {
    battle=new Combat.Battle(scene,Game.GameData.Trainer.party,new Pokemon[] { genwildpoke, genwildpoke2 },
       new Combat.Trainer[] { Game.GameData.Trainer },null);
    battle.fullparty1=false;
  }
  battle.internalbattle=true;
  battle.doublebattle=battle.pbDoubleBattleAllowed();
  battle.cantescape=!canescape;
  pbPrepareBattle(battle);
  Combat.BattleResults decision=0;
  Game.UI.pbBattleAnimation(Game.UI.pbGetWildBattleBGM(species1), action: () => { 
     pbSceneStandby(() => {
        decision=battle.pbStartBattle(canlose);
     });
     foreach (var i in Game.GameData.Trainer.party) { i.makeUnmega(); } //rescue null
     if (Game.GameData.Global.partner != null) {
       Game.GameData.pbHealAll();
       foreach (Monster.Pokemon i in Game.GameData.Global.partner.party) {//[3]
         i.Heal();
         i.makeUnmega(); //rescue null
       }
     }
     if (decision==Combat.BattleResults.LOST || decision==Combat.BattleResults.DRAW) {
       if (canlose) {
         foreach (var i in Game.GameData.Trainer.party) { i.Heal(); }
         for (int i = 0; i < 10; i++) {
           Game.UI.update(); //Graphics.update();
         }
//       else {
//         Game.GameData.GameSystem.bgm_unpause()
//         Game.GameData.GameSystem.bgs_unpause()
//         Game.pbStartOver()
       }
     }
     //Events.onEndBattle.trigger(null,decision,canlose);
  });
  Input.update();
  pbSet(variable,decision);
  return (decision!=Combat.BattleResults.LOST && decision!=Combat.BattleResults.DRAW);
}

public void pbCheckAllFainted() {
  if (pbAllFainted()) {
    Game.pbMessage(Game._INTL(@"{1} has no usable Pokémon!\1",Game.GameData.Trainer.name));
    Game.pbMessage(Game._INTL("{1} blacked out!",Game.GameData.Trainer.name));
    Game.UI.pbBGMFade(1.0f);
    Game.UI.pbBGSFade(1.0f);
    Game.UI.pbFadeOutIn(99999, () => {
       Game.pbStartOver();
    });
  }
}

public void pbEvolutionCheck(int[] currentlevels) {
//  Check conditions for evolution
  for (int i = 0; i < currentlevels.Length; i++) {
    Monster.Pokemon pokemon=Game.GameData.Trainer.party[i];
    if (pokemon.IsNotNullOrNone() && (currentlevels[i] == null || pokemon.Level!=currentlevels[i])) {
      Pokemons newspecies=Evolution.pbCheckEvolution(pokemon)[0];
      if (newspecies>0) {
        //  Start evolution scene
        IPokemonEvolutionScene evo=new PokemonEvolutionScene();
        evo.pbStartScreen(pokemon,newspecies);
        evo.pbEvolution();
        evo.pbEndScreen();
      }
    }
  }
}

public Items[] pbDynamicItemList(params Items[] args) {
  List<Items> ret=new List<Items>();
  for (int i = 0; i < args.Length; i++) {
    //if (hasConst(PBItems,args[i])) {
    //  ret.Add(getConst(PBItems,args[i].to_sym));
      ret.Add(args[i]);
    //}
  }
  return ret.ToArray();
}

/// <summary>
/// Runs the Pickup event after a battle if a Pokemon has the ability Pickup.
/// </summary>
/// <param name="pokemon"></param>
public void pbPickup(Pokemon pokemon) {
  if (pokemon.Ability != Abilities.PICKUP || pokemon.isEgg) return;
  if (pokemon.Item!=0) return;
  if (Core.Rand.Next(10)!=0) return;
  Items[] pickupList=new Items[] {
     Items.POTION,
     Items.ANTIDOTE,
     Items.SUPER_POTION,
     Items.GREAT_BALL,
     Items.REPEL,
     Items.ESCAPE_ROPE,
     Items.FULL_HEAL,
     Items.HYPER_POTION,
     Items.ULTRA_BALL,
     Items.REVIVE,
     Items.RARE_CANDY,
     Items.SUN_STONE,
     Items.MOON_STONE,
     Items.HEART_SCALE,
     Items.FULL_RESTORE,
     Items.MAX_REVIVE,
     Items.PP_UP,
     Items.MAX_ELIXIR
  };
  Items[] pickupListRare=new Items[] {
     Items.HYPER_POTION,
     Items.NUGGET,
     Items.KINGS_ROCK,
     Items.FULL_RESTORE,
     Items.ETHER,
     Items.IRON_BALL,
     Items.DESTINY_KNOT,
     Items.ELIXIR,
     Items.DESTINY_KNOT,
     Items.LEFTOVERS,
     Items.DESTINY_KNOT
  };
  if (pickupList.Length!=18) return;
  if (pickupListRare.Length!=11) return;
  int[] randlist= new int[]{ 30,10,10,10,10,10,10,4,4,1,1 };
  List<Items> items=new List<Items>();
  int plevel=Math.Min(100,pokemon.Level);
  int itemstart=(plevel-1)/10;
  if (itemstart<0) itemstart=0;
  for (int i = 0; i < 9; i++) {
    items.Add(pickupList[itemstart+i]);
  }
  items.Add(pickupListRare[itemstart]);
  items.Add(pickupListRare[itemstart+1]);
  int rnd=Core.Rand.Next(100);
  int cumnumber=0;
  for (int i = 0; i < 11; i++) {
    cumnumber+=randlist[i];
    if (rnd<cumnumber) {
      pokemon.setItem(items[i]);
      break;
    }
  }
}

public bool pbEncounter(EncounterTypes enctype) {
  if (Game.GameData.Global.partner != null) {
    Pokemon encounter1=Game.GameData.PokemonEncounters.pbEncounteredPokemon(enctype);
    if (!encounter1.IsNotNullOrNone()) return false;
    Pokemon encounter2=Game.GameData.PokemonEncounters.pbEncounteredPokemon(enctype);
    if (!encounter2.IsNotNullOrNone()) return false;
    Game.GameData.PokemonTemp.encounterType=enctype;
    pbDoubleWildBattle(encounter1.Species,encounter1.Level,encounter2.Species,encounter2.Level); //[0]|[1]
    Game.GameData.PokemonTemp.encounterType=null;
    return true;
  } else {
    Pokemon encounter=Game.GameData.PokemonEncounters.pbEncounteredPokemon(enctype);
    if (!encounter.IsNotNullOrNone()) return false;
    Game.GameData.PokemonTemp.encounterType=enctype;
    pbWildBattle(encounter.Species,encounter.Level); //[0]|[1]
    Game.GameData.PokemonTemp.encounterType=null;
    return true;
  }
}

//Events.onStartBattle+=delegate(object sender, EventArgs e) {
//  Game.GameData.PokemonTemp.evolutionLevels=new int[6];
//  for (int i = 0; i < Game.GameData.Trainer.party.Length; i++) {
//    Game.GameData.PokemonTemp.evolutionLevels[i]=Game.GameData.Trainer.party[i].Level;
//  }
//}
//
//Events.onEndBattle+=delegate(object sender, EventArgs e) {
//  int decision=e[0];
//  bool canlose=e[1];
//  if (Core.USENEWBATTLEMECHANICS || (decision!=2 && decision!=5)) {		// not a loss or a draw
//    if (Game.GameData.PokemonTemp.evolutionLevels) {
//      pbEvolutionCheck(Game.GameData.PokemonTemp.evolutionLevels);
//      Game.GameData.PokemonTemp.evolutionLevels=null;
//    }
//  }
//  if (decision==1) {
//    foreach (Pokemon pkmn in Game.GameData.Trainer.party) {
//      Game.pbPickup(pkmn);
//      if (pkmn.Ability == Abilities.HONEYGATHER && !pkmn.isEgg && !pkmn.hasItem) {
//        //if (hasConst(PBItems,:HONEY)) {
//          int chance = 5 + Math.Floor((pkmn.Level-1)/10)*5;
//          if (Core.Rand.Next(100)<chance) pkmn.setItem(Items.HONEY);
//        //}
//      }
//    }
//  }
//  if ((decision==2 || decision==5) && !canlose) {
//    Game.GameData.GameSystem.bgm_unpause();
//    Game.GameData.GameSystem.bgs_unpause();
//    Game.pbStartOver();
//  }
//}

#region Scene_Map and Spriteset_Map
public partial class Scene_Map {
  public void createSingleSpriteset(int map) {
    temp=Game.GameData.Scene.spriteset.getAnimations();
    @spritesets[map]=new Spriteset_Map(Game.GameData.MapFactory.maps[map]);
    Game.GameData.Scene.spriteset.restoreAnimations(temp);
    Game.GameData.MapFactory.setSceneStarted(this);
    updateSpritesets();
  }
}

public partial class Spriteset_Map {
  private int usersprites;
  public int getAnimations() {
    return @usersprites;
  }

  public void restoreAnimations(int anims) {
    @usersprites=anims;
  }
}

//Events.onSpritesetCreate+=delegate(object sender, EventArgs e) {
//  spriteset=e[0]; // Spriteset being created
//  viewport=e[1]; // Viewport used for tilemap and characters
//  map=spriteset.map; // Map associated with the spriteset (not necessarily the current map).
//  foreach (var i in map.events.keys) {
//    if (map.events[i].name[/^OutdoorLight\((\w+)\)$/]) {
//      filename=$~[1].ToString();
//      spriteset.addUserSprite(new LightEffect_DayNight(map.events[i],viewport,map,filename));
//    } else if (map.events[i].name=="OutdoorLight") {
//      spriteset.addUserSprite(new LightEffect_DayNight(map.events[i],viewport,map));
//    } else if (map.events[i].name[/^Light\((\w+)\)$/]) {
//      filename=$~[1].ToString();
//      spriteset.addUserSprite(new LightEffect_Basic(map.events[i],viewport,map,filename));
//    } else if (map.events[i].name=="Light") {
//      spriteset.addUserSprite(new LightEffect_Basic(map.events[i],viewport,map));
//    }
//  }
//  spriteset.addUserSprite(new Particle_Engine(viewport,map));
//}

//public static void pbOnSpritesetCreate(spriteset,viewport) {
//  Events.onSpritesetCreate.trigger(null,spriteset,viewport);
//}
#endregion

#region Field movement
public bool pbLedge(float xOffset,float yOffset) {
  if (Terrain.isLedge(Game.pbFacingTerrainTag())) {
    if (Game.pbJumpToward(2,true)) {
      Game.GameData.Scene.spriteset.addUserAnimation(DUST_ANIMATION_ID,Game.GameData.GamePlayer.x,Game.GameData.GamePlayer.y,true);
      Game.GameData.GamePlayer.increase_steps();
      Game.GameData.GamePlayer.check_event_trigger_here(new int[] { 1, 2 });
    }
    return true;
  }
  return false;
}

public static void pbSlideOnIce(Avatar.Player @event=null) {
  if (@event == null) @event=Game.GameData.GamePlayer;
  if (@event == null) return;
  if (!Terrain.isIce(Game.GameData.pbGetTerrainTag(@event))) return;
  Game.GameData.Global.sliding=true;
  int direction=@event.direction;
  bool oldwalkanime=@event.walk_anime;
  @event.straighten();
  @event.pattern=1;
  @event.walk_anime=false;
  do { //;loop
    if (!@event.passable(@event.x,@event.y,direction)) break;
    if (!Terrain.isIce(Game.GameData.pbGetTerrainTag(@event))) break;
    @event.move_forward();
    while (@event.moving) {
      Game.UI.update();//Graphics.update();
      Input.update();
      Game.UI.pbUpdateSceneMap();
    }
  } while (true);
  @event.center(@event.x,@event.y);
  @event.straighten();
  @event.walk_anime=oldwalkanime;
  Game.GameData.Global.sliding=false;
}

// Poison event on each step taken
//Events.onStepTakenTransferPossible+=delegate(object sender, EventArgs e) {
//  handled=e[0];
//  if (handled[0]) continue;
//  if (Game.GameData.Global.stepcount % 4 == 0 && POISONINFIELD) {
//    flashed=false;
//    foreach (Pokemon i in Game.GameData.Trainer.party) {
//      if (i.status==Statuses.POISON && i.HP>0 && !i.isEgg? &&
//         !i.Ability == Abilities.IMMUNITY) {
//        if (!flashed) {
//          Game.GameData.GameScreen.start_flash(new Color(255,0,0,128), 4);
//          flashed=true;
//        }
//        if (i.HP==1 && !POISONFAINTINFIELD) {
//          i.status=0;
//          Game.pbMessage(Game._INTL("{1} survived the poisoning.\\nThe poison faded away!\\1",i.name));
//          continue;
//        }
//        i.HP-=1;
//        if (i.HP==1 && !POISONFAINTINFIELD) {
//          i.status=0;
//          Game.pbMessage(Game._INTL("{1} survived the poisoning.\\nThe poison faded away!\\1",i.name));
//        }
//        if (i.HP==0) {
//          i.changeHappiness("faint");
//          i.status=0;
//          Game.pbMessage(Game._INTL("{1} fainted...\\1",i.name));
//        }
//        if (pbAllFainted) handled[0]=true;
//        pbCheckAllFainted();
//      }
//    }
//  }
//}
//
//Events.onStepTaken+=proc{
//  if (!Game.GameData.Global.happinessSteps) Game.GameData.Global.happinessSteps=0;
//  Game.GameData.Global.happinessSteps+=1;
//  if (Game.GameData.Global.happinessSteps==128) {
//    foreach (var pkmn in Game.GameData.Trainer.party) {
//      if (pkmn.HP>0 && !pkmn.isEgg?) {
//        if (Core.Rand.Next(2)==0) pkmn.changeHappiness("walking");
//      }
//    }
//    Game.GameData.Global.happinessSteps=0;
//  }
//}
//
//Events.onStepTakenFieldMovement+=delegate(object sender, EventArgs e) {
//  @event=e[0]; // Get the event affected by field movement
//  thistile=Game.GameData.MapFactory.getRealTilePos(@event.map.map_id,@event.x,@event.y);
//  map=Game.GameData.MapFactory.getMap(thistile[0]);
//  sootlevel=-1;
//  foreach (var i in [2, 1, 0]) {
//    tile_id = map.data[thistile[1],thistile[2],i];
//    if (tile_id == null) continue;
//    if (map.terrain_tags[tile_id] &&
//       map.terrain_tags[tile_id]==Terrains.SootGrass) {
//      sootlevel=i;
//      break;
//    }
//  }
//  if (sootlevel>=0 && hasConst(PBItems,:SOOTSACK)) {
//    if (!Game.GameData.Global.sootsack) Game.GameData.Global.sootsack=0;
//    //map.data[thistile[1],thistile[2],sootlevel]=0
//    if (@event==Game.GameData.GamePlayer && Game.GameData.Bag.pbQuantity(:SOOTSACK)>0) {
//      Game.GameData.Global.sootsack+=1;
//    }
//    //Game.GameData.Scene.createSingleSpriteset(map.map_id)
//  }
//}
//
//Events.onStepTakenFieldMovement+=delegate(object sender, EventArgs e) {
//  @event=e[0]; // Get the event affected by field movement
//  if (Game.GameData.Scene is Scene_Map) {
//    currentTag=pbGetTerrainTag(@event);
//    if (Terrain.isJustGrass(pbGetTerrainTag(@event,true))) {		// Won't show if under bridge
//      Game.GameData.Scene.spriteset.addUserAnimation(GRASS_ANIMATION_ID,@event.x,@event.y,true);
//    } else if (@event==Game.GameData.GamePlayer && currentTag==Terrains.WaterfallCrest) {
//      //Descend waterfall, but only if this event is the player
//      Game.pbDescendWaterfall(@event);
//    } else if (@event==Game.GameData.GamePlayer && Terrain.isIce(currentTag) && !Game.GameData.Global.sliding) {
//      Game.pbSlideOnIce(@event);
//    }
//  }
//}

public void pbBattleOnStepTaken() {
  if (Game.GameData.Trainer.party.Length>0) {
    EncounterTypes? encounterType=Game.GameData.PokemonEncounters.pbEncounterType();
    if (encounterType>=0) {
      if (Game.GameData.PokemonEncounters.isEncounterPossibleHere()) {
        Pokemon encounter=Game.GameData.PokemonEncounters.pbGenerateEncounter(encounterType.Value);
        encounter=EncounterModifier.trigger(encounter);
        if (Game.GameData.PokemonEncounters.pbCanEncounter(encounter)) {
          if (Game.GameData.Global.partner != null) {
            Pokemon encounter2=Game.GameData.PokemonEncounters.pbEncounteredPokemon(encounterType.Value);
            pbDoubleWildBattle(encounter.Species,encounter.Level,encounter2.Species,encounter2.Level);
          } else {
            pbWildBattle(encounter.Species,encounter.Level);
          }
        }
        EncounterModifier.triggerEncounterEnd();
      }
    }
  }
}

public static void pbOnStepTaken(bool eventTriggered) {
  if (Game.GameData.GamePlayer.move_route_forcing || Game.GameData.pbMapInterpreterRunning || Game.GameData.Trainer == null) {
    //  if forced movement or if no trainer was created yet
    //Events.onStepTakenFieldMovement.trigger(null,Game.GameData.GamePlayer);
    return;
  }
  //if (Game.GameData.Global.stepcount == null) Game.GameData.Global.stepcount=0;
  Game.GameData.Global.stepcount+=1;
  Game.GameData.Global.stepcount&=0x7FFFFFFF;
  //Events.onStepTaken.trigger(null);
  //  Events.onStepTakenFieldMovement.trigger(null,Game.GameData.GamePlayer)
  bool?[] handled= new bool?[]{ null };
  //Events.OnStepTakenTransferPossible.trigger(null,handled);
  if (handled[0]==true) return;
  if (!eventTriggered) {
    Game.GameData.pbBattleOnStepTaken();
  }
}

// This method causes a lot of lag when the game is encrypted
public string pbGetPlayerCharset(string[] meta,int charset,Combat.Trainer trainer=null) {
  if (trainer == null) trainer=Game.GameData.Trainer;
  int outfit=trainer != null ? trainer.outfit.Value : 0;
  string ret=meta[charset];
  if (ret == null || ret=="") ret=meta[1];
//  if FileTest.image_exist("Graphics/Characters/"+ret+"_"+outfit.ToString())
  if (Game.UI.pbResolveBitmap("Graphics/Characters/"+ret+"_"+outfit.ToString())) {
    ret=ret+"_"+outfit.ToString();
  }
  return ret;
}

public static void pbUpdateVehicle() {
  string[] meta=(string[])Game.pbGetMetadata(0, GlobalMetadatas.MetadataPlayerA+Game.GameData.Global.playerID);
  if (meta != null) {
    if (Game.GameData.Global.diving) {
      Game.GameData.GamePlayer.character_name=Game.GameData.pbGetPlayerCharset(meta,5); // Diving graphic
    } else if (Game.GameData.Global.surfing) {
      Game.GameData.GamePlayer.character_name=Game.GameData.pbGetPlayerCharset(meta,3); // Surfing graphic
    } else if (Game.GameData.Global.bicycle) {
      Game.GameData.GamePlayer.character_name=Game.GameData.pbGetPlayerCharset(meta,2); // Bicycle graphic
    } else {
      Game.GameData.GamePlayer.character_name=Game.GameData.pbGetPlayerCharset(meta,1); // Regular graphic
    }
  }
}

public static void pbCancelVehicles(int? destination=null) {
  Game.GameData.Global.surfing=false;
  Game.GameData.Global.diving=false;
  if (destination == null || !Game.GameData.pbCanUseBike(destination.Value)) {
    Game.GameData.Global.bicycle=false;
  }
  Game.pbUpdateVehicle();
}

public bool pbCanUseBike (int mapid) {
  if ((bool)pbGetMetadata(mapid,MapMetadatas.MetadataBicycleAlways)) return true;
  bool? val=(bool?)pbGetMetadata(mapid,MapMetadatas.MetadataBicycle);
  if (val==null) val=(bool?)pbGetMetadata(mapid,MapMetadatas.MetadataOutdoor);
  return val != null ? true : false;
}

public static void pbMountBike() {
  if (Game.GameData.Global.bicycle) return;
  Game.GameData.Global.bicycle=true;
  Game.pbUpdateVehicle();
  IAudioObject bikebgm=(IAudioObject)pbGetMetadata(0,GlobalMetadatas.MetadataBicycleBGM);
  if (bikebgm != null) {
    Game.UI.pbCueBGM(bikebgm,0.5f);
  }
}

public static void pbDismountBike() {
  if (!Game.GameData.Global.bicycle) return;
  Game.GameData.Global.bicycle=false;
  Game.pbUpdateVehicle();
  Game.GameData.GameMap.autoplayAsCue();
}

public static void pbSetPokemonCenter() {
  Game.GameData.Global.pokecenterMapId=Game.GameData.GameMap.map_id;
  Game.GameData.Global.pokecenterX=Game.GameData.GamePlayer.x;
  Game.GameData.Global.pokecenterY=Game.GameData.GamePlayer.y;
  Game.GameData.Global.pokecenterDirection=Game.GameData.GamePlayer.direction;
}
#endregion

#region Fishing
public void pbFishingBegin() {
  Game.GameData.Global.fishing=true;
  if (!pbCommonEvent(Core.FISHINGBEGINCOMMONEVENT)) {
    int patternb = 2*Game.GameData.GamePlayer.direction - 1;
    //TrainerTypes playertrainer=Game.GameData.pbGetPlayerTrainerType();
    string[] meta=(string[])Game.pbGetMetadata(0,GlobalMetadatas.MetadataPlayerA+Game.GameData.Global.playerID);
    int num=(Game.GameData.Global.surfing) ? 7 : 6;
    if (meta != null && meta[num]!=null && meta[num]!="") {
      string charset=pbGetPlayerCharset(meta,num);
      int pattern = 0; do { //4.times |pattern|
        Game.GameData.GamePlayer.setDefaultCharName(charset,patternb-pattern);
        int i = 0; do { //;2.times 
          Graphics.update();
          Input.update();
          pbUpdateSceneMap(); i++;
        } while (i < 2); pattern++;
      } while (pattern < 4);
    }
  }
}

public void pbFishingEnd() {
  if (!pbCommonEvent(Core.FISHINGENDCOMMONEVENT)) {
    int patternb = 2*(Game.GameData.GamePlayer.direction - 2);
    //TrainerTypes playertrainer=Game.GameData.pbGetPlayerTrainerType();
    string[] meta=(string[])Game.pbGetMetadata(0,GlobalMetadatas.MetadataPlayerA+Game.GameData.Global.playerID);
    int num=(Game.GameData.Global.surfing) ? 7 : 6;
    if (meta != null && meta[num]!=null && meta[num]!="") {
      string charset=pbGetPlayerCharset(meta,num);
      int pattern = 0; do { //4.times |pattern|
        Game.GameData.GamePlayer.setDefaultCharName(charset,patternb+pattern);
        int i = 0; do { //;2.times 
          Game.UI.update(); //Graphics.update();
          Input.update();
          Game.UI.pbUpdateSceneMap(); i++;
        } while (i < 2); pattern++;
      } while (pattern < 4);
    }
  }
  Game.GameData.Global.fishing=false;
}

public bool pbFishing(bool hasencounter,int rodtype=1) {
  bool speedup=(Game.GameData.Trainer.firstParty.IsNotNullOrNone() && !Game.GameData.Trainer.firstParty.isEgg &&
     (Game.GameData.Trainer.firstParty.Ability == Abilities.STICKY_HOLD ||
     Game.GameData.Trainer.firstParty.Ability == Abilities.SUCTION_CUPS));
  float bitechance=20+(25*rodtype);   // 45, 70, 95
  if (speedup) bitechance*=1.5f;
  int hookchance=100;
  int oldpattern=Game.GameData.GamePlayer.fullPattern;
  pbFishingBegin();
  IWindow msgwindow=Game.UI.pbCreateMessageWindow();
  do { //;loop
    int time=2+Core.Rand.Next(10);
    if (speedup) time=Math.Min(time,2+Core.Rand.Next(10));
    string message="";
    int i = 0; do { //;time.times 
      message+=". "; i++;
    } while (i < time);
    if (pbWaitMessage(msgwindow,time)) {
      pbFishingEnd();
      Game.GameData.GamePlayer.setDefaultCharName(null,oldpattern);
      Game.UI.pbMessageDisplay(msgwindow,Game._INTL("Not even a nibble..."));
      Game.UI.pbDisposeMessageWindow(msgwindow);
      return false;
    }
    if (Core.Rand.Next(100)<bitechance && hasencounter) {
      int frames=Core.Rand.Next(21)+20;
      if (!pbWaitForInput(msgwindow,message+Game._INTL("\r\nOh! A bite!"),frames)) {
        pbFishingEnd();
        Game.GameData.GamePlayer.setDefaultCharName(null,oldpattern);
        Game.UI.pbMessageDisplay(msgwindow,Game._INTL("The Pokémon got away..."));
        Game.UI.pbDisposeMessageWindow(msgwindow);
        return false;
      }
      if (Core.Rand.Next(100)<hookchance || Core.FISHINGAUTOHOOK) {
        Game.UI.pbMessageDisplay(msgwindow,Game._INTL("Landed a Pokémon!"));
        Game.UI.pbDisposeMessageWindow(msgwindow);
        pbFishingEnd();
        Game.GameData.GamePlayer.setDefaultCharName(null,oldpattern);
        return true;
      }
//      bitechance+=15
//      hookchance+=15
    } else {
      pbFishingEnd();
      Game.GameData.GamePlayer.setDefaultCharName(null,oldpattern);
      Game.UI.pbMessageDisplay(msgwindow,Game._INTL("Not even a nibble..."));
      Game.UI.pbDisposeMessageWindow(msgwindow);
      return false;
    }
  } while (true);
  Game.UI.pbDisposeMessageWindow(msgwindow);
  return false;
}

public bool pbWaitForInput(IWindow msgwindow,string message,int frames) {
  if (Core.FISHINGAUTOHOOK) return true;
  Game.UI.pbMessageDisplay(msgwindow,message,false);
  int i = 0; do { //;frames.times 
    Game.UI.update(); //Graphics.update();
    Input.update();
    Game.UI.pbUpdateSceneMap();
    if (Input.trigger(Input.t) || Input.trigger(Input.t)) {
      return true;
    } i++;
  } while (i < frames);
  return false;
}

public bool pbWaitMessage(IWindow msgwindow,int time) {
  string message="";
  int i = 0; do { //(time+1).times |i|
    if (i>0) message+=". ";
    Game.UI.pbMessageDisplay(msgwindow,message,false);
    int j = 0; do { //20.times ;
      Game.UI.update(); //Graphics.update();
      Input.update();
      Game.UI.pbUpdateSceneMap();
      if (Input.trigger(Input.t) || Input.trigger(Input.t)) {
        return true;
      } j++;
    } while (j < 20); i++;
  } while (i < time+1);
  return false;
}
#endregion

#region Moving between maps
//Events.onMapChange+=delegate(object sender, EventArgs e) {
//  oldid=e[0]; // previous map ID, 0 if no map ID
//  healing=pbGetMetadata(Game.GameData.GameMap.map_id,MetadataHealingSpot);
//  if (healing) Game.GameData.Global.healingSpot=healing;
//  if (Game.GameData.PokemonMap) Game.GameData.PokemonMap.clear;
//  if (Game.GameData.PokemonEncounters) Game.GameData.PokemonEncounters.setup(Game.GameData.GameMap.map_id);
//  Game.GameData.Global.visitedMaps[Game.GameData.GameMap.map_id]=true;
//  if (oldid!=0 && oldid!=Game.GameData.GameMap.map_id) {
//    mapinfos=$RPGVX ? load_data("Data/MapInfos.rvdata") : load_data("Data/MapInfos.rxdata");
//    weather=pbGetMetadata(Game.GameData.GameMap.map_id,MetadataWeather);
//    if (Game.GameData.GameMap.name!=mapinfos[oldid].name) {
//      if (weather && Core.Rand.Next(100)<weather[1]) Game.GameData.GameScreen.weather(weather[0],8,20);
//    } else {
//      oldweather=pbGetMetadata(oldid,MetadataWeather);
//      if (weather && !oldweather && Core.Rand.Next(100)<weather[1]) Game.GameData.GameScreen.weather(weather[0],8,20);
//    }
//  }
//}
//
//Events.onMapChanging+=delegate(object sender, EventArgs e) {
//  newmapID=e[0];
//  newmap=e[1];
////  Undo the weather (Game.GameData.GameMap still refers to the old map)
//  mapinfos=$RPGVX ? load_data("Data/MapInfos.rvdata") : load_data("Data/MapInfos.rxdata");
//  if (newmapID>0) {
//    oldweather=pbGetMetadata(Game.GameData.GameMap.map_id,MetadataWeather);
//    if (Game.GameData.GameMap.name!=mapinfos[newmapID].name) {
//      if (oldweather) Game.GameData.GameScreen.weather(0,0,0);
//    } else {
//      newweather=pbGetMetadata(newmapID,MetadataWeather);
//      if (oldweather && !newweather) Game.GameData.GameScreen.weather(0,0,0);
//    }
//  }
//}
//
//Events.onMapSceneChange+=delegate(object sender, EventArgs e) {
//  scene=e[0];
//  mapChanged=e[1];
//  if (!scene || !scene.spriteset) return;
//  if (Game.GameData.GameMap) {
//    lastmapdetails=Game.GameData.Global.mapTrail[0] ?;
//       pbGetMetadata(Game.GameData.Global.mapTrail[0],MetadataMapPosition) : [-1,0,0];
//    if (!lastmapdetails) lastmapdetails= new []{ -1,0,0 };
//    newmapdetails=Game.GameData.GameMap.map_id ?;
//       pbGetMetadata(Game.GameData.GameMap.map_id,MetadataMapPosition) : [-1,0,0];
//    if (!newmapdetails) newmapdetails= new []{ -1,0,0 };
//    if (!Game.GameData.Global.mapTrail) Game.GameData.Global.mapTrail=[];
//    if (Game.GameData.Global.mapTrail[0]!=Game.GameData.GameMap.map_id) {
//      if (Game.GameData.Global.mapTrail[2]) Game.GameData.Global.mapTrail[3]=Game.GameData.Global.mapTrail[2];
//      if (Game.GameData.Global.mapTrail[1]) Game.GameData.Global.mapTrail[2]=Game.GameData.Global.mapTrail[1];
//      if (Game.GameData.Global.mapTrail[0]) Game.GameData.Global.mapTrail[1]=Game.GameData.Global.mapTrail[0];
//    }
//    Game.GameData.Global.mapTrail[0]=Game.GameData.GameMap.map_id;   // Update map trail
//  }
//  darkmap=pbGetMetadata(Game.GameData.GameMap.map_id,MetadataDarkMap);
//  if (darkmap) {
//    if (Game.GameData.Global.flashUsed) {
//      Game.GameData.PokemonTemp.darknessSprite=new DarknessSprite();
//      scene.spriteset.addUserSprite(Game.GameData.PokemonTemp.darknessSprite);
//      darkness=Game.GameData.PokemonTemp.darknessSprite;
//      darkness.radius=176;
//    } else {
//      Game.GameData.PokemonTemp.darknessSprite=new DarknessSprite();
//      scene.spriteset.addUserSprite(Game.GameData.PokemonTemp.darknessSprite);
//    }
//  } else if (!darkmap) {
//    Game.GameData.Global.flashUsed=false;
//    if (Game.GameData.PokemonTemp.darknessSprite) {
//      Game.GameData.PokemonTemp.darknessSprite.dispose();
//      Game.GameData.PokemonTemp.darknessSprite=null;
//    }
//  }
//  if (mapChanged) {
//    if (pbGetMetadata(Game.GameData.GameMap.map_id,MetadataShowArea)) {
//      nosignpost=false;
//      if (Game.GameData.Global.mapTrail[1]) {
//        for (int i = 0; i < NOSIGNPOSTS.Length; i++) {/2
//          if (NOSIGNPOSTS[2*i]==Game.GameData.Global.mapTrail[1] && NOSIGNPOSTS[2*i+1]==Game.GameData.GameMap.map_id) /nosignpost=true;
//          if (NOSIGNPOSTS[2*i+1]==Game.GameData.Global.mapTrail[1] && NOSIGNPOSTS[2*i]==Game.GameData.GameMap.map_id) /nosignpost=true;
//          if (nosignpost) break;
//        }
//        mapinfos=$RPGVX ? load_data("Data/MapInfos.rvdata") : load_data("Data/MapInfos.rxdata");
//        oldmapname=mapinfos[Game.GameData.Global.mapTrail[1]].name;
//        if (Game.GameData.GameMap.name==oldmapname) nosignpost=true;
//      }
//      if (!nosignpost) scene.spriteset.addUserSprite(new LocationWindow(Game.GameData.GameMap.name));
//    }
//  }
//  if (pbGetMetadata(Game.GameData.GameMap.map_id,MetadataBicycleAlways)) {
//    Game.pbMountBike;
//  } else {
//    if (!pbCanUseBike(Game.GameData.GameMap.map_id)) {
//      Game.pbDismountBike;
//    }
//  }
//}

public static void pbStartOver(bool gameover=false) {
  if (Game.GameData.pbInBugContest) {
    Game.GameData.pbBugContestStartOver();
    return;
  }
  Game.GameData.pbHealAll();
  if (Game.GameData.Global.pokecenterMapId != null && Game.GameData.Global.pokecenterMapId>=0) {
    if (gameover) {
      Game.pbMessage(Game._INTL("\\w[]\\wm\\c[8]\\l[3]After the unfortunate defeat, {1} scurried to a Pokémon Center.",Game.GameData.Trainer.name));
    } else {
      Game.pbMessage(Game._INTL("\\w[]\\wm\\c[8]\\l[3]{1} scurried to a Pokémon Center, protecting the exhausted and fainted Pokémon from further harm.",Game.GameData.Trainer.name));
    }
    Game.pbCancelVehicles();
    Game.GameData.pbRemoveDependencies();
    //Game.GameData.GameSwitches[Core.STARTING_OVER_SWITCH]=true;
    Game.GameData.Features.StartingOver();
    Game.GameData.GameTemp.player_new_map_id=Game.GameData.Global.pokecenterMapId;
    Game.GameData.GameTemp.player_new_x=Game.GameData.Global.pokecenterX;
    Game.GameData.GameTemp.player_new_y=Game.GameData.Global.pokecenterY;
    Game.GameData.GameTemp.player_new_direction=Game.GameData.Global.pokecenterDirection;
    if (Game.GameData.Scene is Scene_Map) ((Scene_Map)Game.GameData.Scene).transfer_player();
    Game.GameData.GameMap.refresh();
  } else {
    int[] homedata=(int[])Game.pbGetMetadata(0,GlobalMetadatas.MetadataHome);
    //Overworld.TilePosition homedata=(int[])Game.pbGetMetadata(0,GlobalMetadatas.MetadataHome);
    if ((homedata != null && !pbRxdataExists(string.Format("Data/Map%03d",homedata[0])) )) {
      if (Core.DEBUG) {
        Game.pbMessage(string.Format("Can't find the map 'Map{0}' in the Data folder. The game will resume at the player's position.",homedata[0]));
      }
      Game.GameData.pbHealAll();
      return;
    }
    if (gameover) {
      Game.pbMessage(Game._INTL("\\w[]\\wm\\c[8]\\l[3]After the unfortunate defeat, {1} scurried home.",Game.GameData.Trainer.name));
    } else {
      Game.pbMessage(Game._INTL("\\w[]\\wm\\c[8]\\l[3]{1} scurried home, protecting the exhausted and fainted Pokémon from further harm.",Game.GameData.Trainer.name));
    }
    if (homedata != null) {
      Game.pbCancelVehicles();
      Game.GameData.pbRemoveDependencies();
      //Game.GameData.GameSwitches[STARTING_OVER_SWITCH]=true;
      Game.GameData.Features.StartingOver();
      Game.GameData.GameTemp.player_new_map_id=homedata[0];
      Game.GameData.GameTemp.player_new_x=homedata[1];
      Game.GameData.GameTemp.player_new_y=homedata[2];
      Game.GameData.GameTemp.player_new_direction=homedata[3];
      if (Game.GameData.Scene is Scene_Map) ((Scene_Map)Game.GameData.Scene).transfer_player();
      Game.GameData.GameMap.refresh();
    } else {
      Game.GameData.pbHealAll();
    }
  }
  Game.GameData.pbEraseEscapePoint();
}

public void pbCaveEntranceEx(bool exiting) {
  //sprite=new BitmapSprite(Graphics.width,Graphics.height);
  //Bitmap sprite=new BitmapSprite(Graphics.width,Graphics.height);
  ////sprite.z=100000;
  //int totalBands=15;
  //int totalFrames=15;
  //float bandheight=((Graphics.height/2)-10f)/totalBands;
  //float bandwidth=((Graphics.width/2)-12f)/totalBands;
  //List<double> grays=new List<double>();
  //int tbm1=totalBands-1;
  //for (int i = 0; i < totalBands; i++) {
  //  grays.Add(exiting ? 0 : 255);
  //}
  //int j = 0; do { //totalFrames.times |j|
  //  float x=0;
  //  float y=0;
  //  float rectwidth=Graphics.width;
  //  float rectheight=Graphics.height;
  //  for (int k = 0; k < j; k++) {
  //    double t=(255.0f)/totalFrames;
  //    if (exiting) {
  //      t=1.0-t;
  //      t*=1.0f+((k)/totalFrames);
  //    } else {
  //      t*=1.0+0.3f*Math.Pow(((totalFrames-k)/totalFrames),0.7f);
  //    }
  //    //grays[k]-=t;
  //    grays[k]=grays[k]-t;
  //    if (grays[k]<0) grays[k]=0;
  //  }
  //  for (int i = 0; i < totalBands; i++) {
  //    double currentGray=grays[i];
  //    sprite.bitmap.fill_rect(new Rect(x,y,rectwidth,rectheight),
  //       new Color(currentGray,currentGray,currentGray));
  //    x+=bandwidth;
  //    y+=bandheight;
  //    rectwidth-=bandwidth*2;
  //    rectheight-=bandheight*2;
  //  }
  //  Graphics.update();
  //  Input.update(); j++;
  //} while (j < totalFrames);
  //if (exiting) {
  //  pbToneChangeAll(new Tone(255,255,255),0);
  //} else {
  //  pbToneChangeAll(new Tone(-255,-255,-255),0);
  //}
  //for (j = 0; j < 15; j++) {
  //  if (exiting) {
  //    sprite.color=new Color(255,255,255,j*255/15);
  //  } else {
  //    sprite.color=new Color(0,0,0,j*255/15) ;
  //  }
  //  Graphics.update();
  //  Input.update();
  //}
  //pbToneChangeAll(new Tone(0,0,0),8);
  //for (j = 0; j < 5; j++) {
  //  Graphics.update();
  //  Input.update();
  //}
  //sprite.dispose();
}

public void pbCaveEntrance() {
  pbSetEscapePoint();
  pbCaveEntranceEx(false);
}

public void pbCaveExit() {
  pbEraseEscapePoint();
  pbCaveEntranceEx(true);
}

public void pbSetEscapePoint() {
  if (Game.GameData.Global.escapePoint == null) Game.GameData.Global.escapePoint=new float[0];
  float xco=Game.GameData.GamePlayer.x;
  float yco=Game.GameData.GamePlayer.y;
  float dir = 0;
  switch (Game.GameData.GamePlayer.direction) {
  case 2:   // Down
    yco-=1; dir=8;
    break;
  case 4:   // Left
    xco+=1; dir=6;
    break;
  case 6:   // Right
    xco-=1; dir=4;
    break;
  case 8:   // Up
    yco+=1; dir=2;
    break;
  }
  Game.GameData.Global.escapePoint=new float[] { Game.GameData.GameMap.map_id, xco, yco, dir };
}

public void pbEraseEscapePoint() {
  Game.GameData.Global.escapePoint=new float[0];
}
#endregion

#region Partner trainer
public void pbRegisterPartner(TrainerTypes trainerid,string trainername,int partyid=0) {
  Game.pbCancelVehicles();
  Combat.TrainerObject trainer=Combat.Trainer.pbLoadTrainer(trainerid,trainername,partyid);
  //Events.OnTrainerPartyLoad.trigger(null,trainer);
  Combat.Trainer trainerobject=new Combat.Trainer(Game._INTL(trainer.Trainer.name),trainerid);
  trainerobject.setForeignID(Game.GameData.Trainer);
  foreach (Monster.Pokemon i in trainer.party) {
    //i.TrainerId=trainerobject.id;
    //i.OT=trainerobject.name;
    i.SetCatchInfos(trainer:trainerobject);
    i.calcStats();
  }
  //Game.GameData.Global.partner=new Combat.Trainer(trainerid,trainerobject.name,trainerobject.id,trainer.party);
  Combat.Trainer t=new Combat.Trainer(trainertype:trainerid,name:trainerobject.name); t.party = trainer.party;
  Game.GameData.Global.partner=t;//new Combat.Trainer(trainerid,trainerobject.name,trainerobject.id,trainer.party);
}

public void pbDeregisterPartner() {
  Game.GameData.Global.partner=null;
}
#endregion

#region Constant checks
// ===============================================================================
//Events.onMapUpdate+=delegate(object sender, EventArgs e) {   // Pokérus check
//  last=Game.GameData.Global.pokerusTime;
//  now=pbGetTimeNow();
//  if (!last || last.year!=now.year || last.month!=now.month || last.day!=now.day) {
//    if (Game.GameData.Trainer && Game.GameData.Trainer.party) {
//      foreach (var i in Game.GameData.Trainer.pokemonParty) {
//        i.lowerPokerusCount;
//      }
//      Game.GameData.Global.pokerusTime=now;
//    }
//  }
//}

/// <summary>
/// Returns whether the Poké Center should explain Pokérus to the player, if a
/// healed Pokémon has it.
/// </summary>
/// <returns></returns>
public static bool pbPokerus() {
  //if (Game.GameData.GameSwitches[Core.SEEN_POKERUS_SWITCH]) return false;
  //if (Core.SEEN_POKERUS_SWITCH) return false;
  if (Game.GameData.Features.SeenPokerusSwitch) return false;
  foreach (Pokemon i in Game.GameData.Trainer.party) {
    //if (i.PokerusStage==1) return true;
    if (i.PokerusStage==true) return true;
  }
  return false;
}


/// <summary>
/// Connects to User's OS and checks if laptop battery 
/// life alert should be displayed on screen
/// </summary>
/// <returns></returns>
//public bool pbBatteryLow() {
//  //int[] power="\0"*12;
//  int[] power=new int[12];
//  try {
//    sps=new Win32API('kernel32.dll','GetSystemPowerStatus','p','l');
//  } catch (Exception) {
//    return false;
//  }
//  if (sps.call(power)==1) {
//    status=power;//.unpack("CCCCVV")
//    //  Battery Flag
//    if (status[1]!=255 && (status[1]&6)!=0) {		// Low or Critical
//      return true;
//    }
//    //  Battery Life Percent
//    if (status[2]<3) {		// Less than 3 percent
//      return true;
//    }
//    //  Battery Life Time
//    if (status[4]<300) {		// Less than 5 minutes
//      return true;
//    }
//  }
//  return false;
//}

//Events.onMapUpdate+=delegate(object sender, EventArgs e) {
//  DateTime time=pbGetTimeNow();
//  if (time.Sec==0 && Game.GameData.Trainer != null && Game.GameData.Global != null && Game.GameData.GamePlayer != null && Game.GameData.GameMap != null &&
//     !Game.GameData.PokemonTemp.batterywarning && !Game.GameData.GamePlayer.move_route_forcing &&
//     !pbMapInterpreterRunning? && !Game.GameData.GameTemp.message_window_showing &&
//     pbBatteryLow?) {
//    Game.GameData.PokemonTemp.batterywarning=true;
//    Game.pbMessage(Game._INTL("The game has detected that the battery is low. You should save soon to avoid losing your progress."));
//  }
//  if (Game.GameData.PokemonTemp.cueFrames) {
//    Game.GameData.PokemonTemp.cueFrames-=1;
//    if (Game.GameData.PokemonTemp.cueFrames<=0) {
//      Game.GameData.PokemonTemp.cueFrames=null;
//      if (Game.GameData.GameSystem.getPlayingBGM==null) {
//        pbBGMPlay(Game.GameData.PokemonTemp.cueBGM);
//      }
//    }
//  }
//}
#endregion



#region Voice recorder
public IAudioObject pbRecord(string text,float maxtime=30.0f) {
  if (text == null) text="";
  IWindow textwindow=new Window_UnformattedTextPokemon().WithSize(text,
     0,0,Graphics.width,Graphics.height-96);
  //textwindow.z=99999;
  if (text=="") {
    textwindow.visible=false;
  }
  IAudioObject wave=null;
  IWindow msgwindow=Game.UI.pbCreateMessageWindow();
  float oldvolume=Game.UI.Audio_bgm_get_volume();
  Game.UI.Audio_bgm_set_volume(0);
  int delay=2;
  int i = 0; do { //delay.times |i|
    Game.UI.pbMessageDisplay(msgwindow,
      string.Format("Recording in {0} second(s)...\nPress ESC to cancel.",delay-i),false);
    int n = 0; do { //;Graphics.frame_rate.times 
      Game.UI.update(); //Graphics.update();
      Input.update();
      textwindow.update();
      msgwindow.update();
      if (Input.trigger(Input.t)) {
        Game.UI.Audio_bgm_set_volume(oldvolume);
        Game.UI.pbDisposeMessageWindow(msgwindow);
        textwindow.dispose();
        return null;
      } n++;
    } while(n < Game.UI.frame_rate); i++; //Graphics.frame_rate
  } while(i < delay);
  Game.pbMessageDisplay(msgwindow,
     Game._INTL("NOW RECORDING\nPress ESC to stop recording."),false);
  if (Game.UI.beginRecordUI()) {
    int frames=(int)(maxtime*Game.UI.frame_rate); //Graphics.frame_rate
    i = 0; do { //;frames.times 
      Game.UI.update(); //Graphics.update();
      Input.update();
      textwindow.update();
      msgwindow.update();
      if (Input.trigger(Input.t)) {
        break;
      }
    } while(i < frames);
    string tmpFile="\\record.wav";//ENV["TEMP"]+
    Game.UI.endRecord(tmpFile);
    wave=Game.UI.getWaveDataUI(tmpFile,true);
    if (wave != null) {
      Game.pbMessageDisplay(msgwindow,Game._INTL("PLAYING BACK..."),false);
      textwindow.update();
      msgwindow.update();
      Game.UI.update(); //Graphics.update();
      Input.update();
      wave.play();
      i = 0; do { //(Graphics.frame_rate*wave.time).to_i.times 
        Game.UI.update(); //Graphics.update();
        Input.update();
        textwindow.update();
        msgwindow.update();
      } while(i < (Game.UI.frame_rate*wave.time)); //Graphics.frame_rate
    }
  }
  Game.UI.Audio_bgm_set_volume(oldvolume);
  Game.UI.pbDisposeMessageWindow(msgwindow);
  textwindow.dispose();
  return wave;
}

//public static void pbRxdataExists(string file) {
//  if ($RPGVX) {
//    return pbRgssExists(file+".rvdata");
//  } else {
//    return pbRgssExists(file+".rxdata") ;
//  }
//}
#endregion

#region Gaining items
public static bool pbItemBall(Items item,int quantity=1) {
  //if (item is String || item is Symbol) {
  //  item=getID(PBItems,item);
  //}
  if (item == null || item<=0 || quantity<1) return false;
  string itemname=(quantity>1) ? Game.ItemData[item].Plural : item.ToString(TextScripts.Name);
  //int pocket=pbGetPocket(item);
  ItemPockets pocket=Game.ItemData[item].Pocket??ItemPockets.MISC;
  if (Game.GameData.Bag.pbStoreItem(item,quantity)) {		// If item can be picked up
    if (Game.ItemData[item].Category==ItemCategory.ALL_MACHINES) { //[ITEMUSE]==3 || Game.ItemData[item][ITEMUSE]==4) { If item is TM=>3 or HM=>4
      Game.pbMessage(Game._INTL("\\se[ItemGet]{1} found \\c[1]{2}\\c[0]!\\nIt contained \\c[1]{3}\\c[0].\\wtnp[30]",
         Game.GameData.Trainer.name,itemname,Game.ItemData[item].Id.ToString(TextScripts.Name)));//ToDo:[ITEMMACHINE] param for Machine-to-Move Id
    } else if (item == Items.LEFTOVERS) {
      Game.pbMessage(Game._INTL("\\se[ItemGet]{1} found some \\c[1]{2}\\c[0]!\\wtnp[30]",Game.GameData.Trainer.name,itemname));
    } else if (quantity>1) {
      Game.pbMessage(Game._INTL("\\se[ItemGet]{1} found {2} \\c[1]{3}\\c[0]!\\wtnp[30]",Game.GameData.Trainer.name,quantity,itemname));
    } else {
      Game.pbMessage(Game._INTL("\\se[ItemGet]{1} found one \\c[1]{2}\\c[0]!\\wtnp[30]",Game.GameData.Trainer.name,itemname));
    }
    Game.pbMessage(Game._INTL("{1} put the \\c[1]{2}\\c[0]\r\nin the <icon=bagPocket#{pocket}>\\c[1]{3}\\c[0] Pocket.",
       Game.GameData.Trainer.name,itemname,pocket.ToString())); //PokemonBag.pocketNames()[pocket]
    return true;
  } else {   // Can't add the item
    if (Game.ItemData[item].Category==ItemCategory.ALL_MACHINES) { //[ITEMUSE]==3 || Game.ItemData[item][ITEMUSE]==4) {
      Game.pbMessage(Game._INTL("{1} found \\c[1]{2}\\c[0]!\\wtnp[20]",Game.GameData.Trainer.name,itemname));
    } else if (item == Items.LEFTOVERS) {
      Game.pbMessage(Game._INTL("{1} found some \\c[1]{2}\\c[0]!\\wtnp[20]",Game.GameData.Trainer.name,itemname));
    } else if (quantity>1) {
      Game.pbMessage(Game._INTL("{1} found {2} \\c[1]{3}\\c[0]!\\wtnp[20]",Game.GameData.Trainer.name,quantity,itemname));
    } else {
      Game.pbMessage(Game._INTL("{1} found one \\c[1]{2}\\c[0]!\\wtnp[20]",Game.GameData.Trainer.name,itemname));
    }
    Game.pbMessage(Game._INTL("Too bad... The Bag is full..."));
    return false;
  }
}

public static bool pbReceiveItem(Items item,int quantity=1) {
  //if (item is String || item is Symbol) {
  //  item=getID(PBItems,item);
  //}
  if (item == null || item<=0 || quantity<1) return false;
  string itemname=(quantity>1) ? Game.ItemData[item].Plural : item.ToString(TextScripts.Name);
  //int pocket=pbGetPocket(item);
  ItemPockets pocket=Game.ItemData[item].Pocket??ItemPockets.MISC;
  if (Game.ItemData[item].Category==ItemCategory.ALL_MACHINES) { //[ITEMUSE]==3 || Game.ItemData[item][ITEMUSE]==4) {
    Game.pbMessage(Game._INTL("\\se[ItemGet]Obtained \\c[1]{1}\\c[0]!\\nIt contained \\c[1]{2}\\c[0].\\wtnp[30]",
       itemname,Game.ItemData[item].Id.ToString(TextScripts.Name)));//ToDo:[ITEMMACHINE] param for Machine-to-Move Id
  } else if (item == Items.LEFTOVERS) {
    Game.pbMessage(Game._INTL("\\se[ItemGet]Obtained some \\c[1]{1}\\c[0]!\\wtnp[30]",itemname));
  } else if (quantity>1) {
    Game.pbMessage(Game._INTL("\\se[ItemGet]Obtained \\c[1]{1}\\c[0]!\\wtnp[30]",itemname));
  } else {
    Game.pbMessage(Game._INTL("\\se[ItemGet]Obtained \\c[1]{1}\\c[0]!\\wtnp[30]",itemname));
  }
  if (Game.GameData.Bag.pbStoreItem(item,quantity)) {		// If item can be added
    Game.pbMessage(Game._INTL("{1} put the \\c[1]{2}\\c[0]\r\nin the <icon=bagPocket#{pocket}>\\c[1]{3}\\c[0] Pocket.",
       Game.GameData.Trainer.name,itemname,pocket.ToString())); //PokemonBag.pocketNames()[pocket]
    return true;
  }
  return false;   // Can't add the item
}

public void pbUseKeyItem() {
  if (Game.GameData.Bag.registeredItem==0) {
    Game.pbMessage(Game._INTL("A Key Item in the Bag can be registered to this key for instant use."));
  } else {
    Game.GameData.pbUseKeyItemInField(Game.GameData.Bag.registeredItem);
  }
}
#endregion

#region Bridges
public void pbBridgeOn(float height=2) {
  Game.GameData.Global.bridge=height;
}

public void pbBridgeOff() {
  Game.GameData.Global.bridge=0;
}
#endregion

#region Event locations, terrain tags
public bool pbEventFacesPlayer (Avatar.Character @event,Avatar.Player player,float distance) {
  if (distance<=0) return false;
//  Event can't reach player if no coordinates coincide
  if (@event.x!=player.x && @event.y!=player.y) return false;
  float deltaX = (@event.direction == 6 ? 1 : @event.direction == 4 ? -1 : 0);
  float deltaY = (@event.direction == 2 ? 1 : @event.direction == 8 ? -1 : 0);
//  Check for existence of player
  float curx=@event.x;
  float cury=@event.y;
  bool found=false;
  for (int i = 0; i < distance; i++) {
    curx+=deltaX;
    cury+=deltaY;
    if (player.x==curx && player.y==cury) {
      found=true;
      break;
    }
  }
  return found;
}

public static bool pbEventCanReachPlayer (Avatar.Character @event,Avatar.Player player,float distance) {
  if (distance<=0) return false;
//  Event can't reach player if no coordinates coincide
  if (@event.x!=player.x && @event.y!=player.y) return false;
  float deltaX = (@event.direction == 6 ? 1 : @event.direction == 4 ? -1 : 0);
  float deltaY =  (@event.direction == 2 ? 1 : @event.direction == 8 ? -1 : 0);
//  Check for existence of player
  float curx=@event.x;
  float cury=@event.y;
  bool found=false;
  float realdist=0;
  for (int i = 0; i < distance; i++) {
    curx+=deltaX;
    cury+=deltaY;
    if (player.x==curx && player.y==cury) {
      found=true;
      break;
    }
    realdist+=1;
  }
  if (!found) return false;
//  Check passibility
  curx=@event.x;
  cury=@event.y;
  for (int i = 0; i < realdist; i++) {
    if (!@event.passable(curx,cury,@event.direction)) {
      return false;
    }
    curx+=deltaX;
    cury+=deltaY;
  }
  return true;
}

public static TilePosition pbFacingTileRegular(float? direction=null,Avatar.Character @event=null) {
  if (@event == null) @event=Game.GameData.GamePlayer;
  if (@event == null) return new TilePosition();
  float x=@event.x;
  float y=@event.y;
  if (direction == null) direction=@event.direction;
  switch (direction) {
  case 1:
    y+=1; x-=1;
    break;
  case 2:
    y+=1;
    break;
  case 3:
    y+=1; x+=1;
    break;
  case 4:
    x-=1;
    break;
  case 6:
    x+=1;
    break;
  case 7:
    y-=1; x-=1;
    break;
  case 8:
    y-=1;
    break;
  case 9:
    y-=1; x+=1;
    break;
  }
  return Game.GameData.GameMap != null ? Game.GameData.GameMap.map_id : new TilePosition(0, x, y);
}

public static TilePosition? pbFacingTile(float? direction=null,Avatar.Character @event=null) {
  if (Game.GameData.MapFactory != null) {
    return Game.GameData.MapFactory.getFacingTile(direction,@event);
  } else {
    return pbFacingTileRegular(direction,@event);
  }
}

public bool pbFacingEachOther(Avatar.Character event1,Avatar.Character event2) {
  if (event1 == null || event2 == null) return false; TilePosition? tile1, tile2;
  if (Game.GameData.MapFactory != null) {             
    tile1=Game.GameData.MapFactory.getFacingTile(null,event1);
    tile2=Game.GameData.MapFactory.getFacingTile(null,event2);
    if (tile1 == null || tile2 == null) return false;
    if (tile1.Value.MapId==event2.map.map_id &&
       tile1.Value.X==event2.x && tile1.Value.Y==event2.y &&
       tile2.Value.MapId==event1.map.map_id &&
       tile2.Value.X==event1.x && tile2.Value.Y==event1.y) {
      return true;
    } else {
      return false;
    }
  } else {
    tile1=Game.pbFacingTile(null,event1);
    tile2=Game.pbFacingTile(null,event2);
    if (tile1 == null || tile2 == null) return false;
    if (tile1.Value.X==event2.x && tile1.Value.Y==event2.y &&
       tile2.Value.X==event1.x && tile2.Value.Y==event1.y) {
      return true;
    } else {
      return false;
    }
  }
}

public Terrains pbGetTerrainTag(Avatar.Character @event=null,bool countBridge=false) {
  if (@event == null) @event=Game.GameData.GamePlayer;
  if (@event == null) return 0;
  if (Game.GameData.MapFactory != null) {
    return Game.GameData.MapFactory.getTerrainTag(@event.map.map_id,@event.x,@event.y,countBridge).Value;
  } else {
    return Game.GameData.GameMap.terrain_tag(@event.x,@event.y,countBridge);
  }
}

public static Terrains pbFacingTerrainTag(Avatar.Character @event=null,float? dir=null) {
  if (Game.GameData.MapFactory) {
    return Game.GameData.MapFactory.getFacingTerrainTag(dir,@event);
  } else {
    if (@event == null) @event=Game.GameData.GamePlayer;
    if (@event == null) return 0;
    TilePosition? facing=pbFacingTile(dir,@event);
   return Game.GameData.GameMap.terrain_tag(facing[1],facing[2]);
  }
}
#endregion

#region Event movement
public void pbTurnTowardEvent(Avatar.Character @event,Avatar.Character otherEvent) {
  float sx=0;
  float sy=0;
  if (Game.GameData.MapFactory) {
    Point relativePos=Game.GameData.MapFactory.getThisAndOtherEventRelativePos(otherEvent,@event);
    sx = relativePos[0];
    sy = relativePos[1];
  } else {
    sx = @event.x - otherEvent.x;
    sy = @event.y - otherEvent.y;
  }
  if (sx == 0 && sy == 0) {
    return;
  }
  if (Math.Abs(sx) > Math.Abs(sy)) {
    //sx > 0 ? @event.turn_left : @event.turn_right;
    if(sx > 0) @event.turn_left(); else @event.turn_right();
  } else {
    //sy > 0 ? @event.turn_up : @event.turn_down;
    if(sy > 0) @event.turn_up(); else @event.turn_down();
  }
}

public static void pbMoveTowardPlayer(Avatar.Character @event) {
  int maxsize=Math.Max(Game.GameData.GameMap.width,Game.GameData.GameMap.height);
  if (!pbEventCanReachPlayer(@event,Game.GameData.GamePlayer,maxsize)) return;
  do { //;loop
    float x=@event.x;
    float y=@event.y;
    @event.move_toward_player();
    if (@event.x==x && @event.y==y) break;
    while (@event.moving) {
      Game.UI.update(); //Graphics.update();
      Input.update();
      Game.UI.pbUpdateSceneMap();
    }
  } while (true);
  if (Game.GameData.PokemonMap) Game.GameData.PokemonMap.addMovedEvent(@event.id);
}

public static bool pbJumpToward(float dist=1,bool playSound=false,bool cancelSurf=false) {
  float x=Game.GameData.GamePlayer.x;
  float y=Game.GameData.GamePlayer.y;
  switch (Game.GameData.GamePlayer.direction) {
  case 2: // down
    Game.GameData.GamePlayer.jump(0,dist);
    break;
  case 4: // left
    Game.GameData.GamePlayer.jump(-dist,0);
    break;
  case 6: // right
    Game.GameData.GamePlayer.jump(dist,0);
    break;
  case 8: // up
    Game.GameData.GamePlayer.jump(0,-dist);
    break;
  }
  if (Game.GameData.GamePlayer.x!=x || Game.GameData.GamePlayer.y!=y) {
    if (playSound) Game.UI.pbSEPlay("jump");
    if (cancelSurf) Game.pbCancelVehicles();
    while (Game.GameData.GamePlayer.jumping) {
      Game.UI.update(); //Graphics.update();
      Input.update();
      Game.UI.pbUpdateSceneMap();
    }
    return true;
  }
  return false;
}

public void pbWait(int numframes) {
  do { //;numframes.times 
    Game.UI.update(); //Graphics.update();
    Input.update();
    Game.UI.pbUpdateSceneMap();
  } while (true);
}

public void pbMoveRoute(Avatar.Character @event,int[] commands,bool waitComplete=false) {
  MoveRoute route=new RPG.MoveRoute();
  route.repeat=false;
  route.skippable=true;
  route.list.clear();
  route.list.Add(new RPG.MoveCommand(MoveRoutes.ThroughOn));
  int i=0; while (i<commands.Length) { 
    switch (commands[i]) {
    case MoveRoutes.Wait: case MoveRoutes.SwitchOn: case MoveRoutes.SwitchOff: case
       MoveRoutes.ChangeSpeed: case MoveRoutes.ChangeFreq: case MoveRoutes.Opacity: case
       MoveRoutes.Blending: case MoveRoutes.PlaySE: case MoveRoutes.Script:
      route.list.Add(new RPG.MoveCommand(commands[i],new int[] { commands[i + 1] }));
      i+=1;
      break;
    case MoveRoutes.ScriptAsync:
      route.list.Add(new RPG.MoveCommand(MoveRoutes.Script,new int[] { commands[i + 1] }));
      route.list.Add(new RPG.MoveCommand(MoveRoutes.Wait,new int[] { 0 }));
      i+=1;
      break;
    case MoveRoutes.Jump:
      route.list.Add(new RPG.MoveCommand(commands[i],new int[] { commands[i + 1], commands[i + 2] }));
      i+=2;
      break;
    case MoveRoutes.Graphic:
      route.list.Add(new RPG.MoveCommand(commands[i],
         new int[] { commands[i + 1], commands[i + 2], commands[i + 3], commands[i + 4] }));
      i+=4;
      break;
    default:
      route.list.Add(new RPG.MoveCommand(commands[i]));
      break;
    }
    i+=1;
  }
  route.list.Add(new RPG.MoveCommand(MoveRoutes.ThroughOff));
  route.list.Add(new RPG.MoveCommand(0));
  if (@event != null) {
    @event.force_move_route(route);
  }
  return route;
}
#endregion

#region Screen effects
public void pbToneChangeAll(Tone tone,float duration) {
  Game.GameData.GameScreen.start_tone_change(tone,duration * 2);
  foreach (var picture in Game.GameData.GameScreen.pictures) {
    if (picture) picture.start_tone_change(tone,duration * 2);
  }
}

public void pbShake(float power,float speed,int frames) {
  Game.GameData.GameScreen.start_shake(power,speed,frames * 2);
}

public void pbFlash(PokemonUnity.Color color,int frames) {
  Game.GameData.GameScreen.start_flash(color,frames * 2);
}

public void pbScrollMap(int direction, int distance, float speed) {
  if (Game.GameData.GameMap == null) return;
  if (speed==0) {
    switch (direction) {
    case 2:
      Game.GameData.GameMap.scroll_down(distance * 128);
      break;
    case 4:
      Game.GameData.GameMap.scroll_left(distance * 128);
      break;
    case 6:
      Game.GameData.GameMap.scroll_right(distance * 128);
      break;
    case 8:
      Game.GameData.GameMap.scroll_up(distance * 128);
      break;
    }
  } else {
    Game.GameData.GameMap.start_scroll(direction, distance, speed);
    float oldx=Game.GameData.GameMap.display_x;
    float oldy=Game.GameData.GameMap.display_y;
    do { //;loop
      Game.UI.update(); //Graphics.update();
      Input.update();
      if (!Game.GameData.GameMap.scrolling()) {
        break;
      }
      Game.UI.pbUpdateSceneMap();
      if (Game.GameData.GameMap.display_x==oldx && Game.GameData.GameMap.display_y==oldy) {
        break;
      }
      oldx=Game.GameData.GameMap.display_x;
      oldy=Game.GameData.GameMap.display_y;
    } while (true);
  }
}
#endregion
}

namespace Avatar { 
// ===============================================================================
// Events
// ===============================================================================
public partial class GameEvent {
  public bool cooledDown (int seconds) {
    if (!(expired(seconds) && tsOff("A"))) {
      this.need_refresh=true;
      return false;
    } else {
      return true;
    }
  }

  public bool cooledDownDays (int days) {
    if (!(expiredDays(days) && tsOff("A"))) {
      this.need_refresh=true;
      return false;
    } else {
      return true;
    }
  }
}
}

public partial class InterpreterFieldMixin {
        private int @event_id;
        private Avatar.IEntity @event;
//  Used in boulder events. Allows an event to be pushed. To be used in
//  a script event command.
  public void pbPushThisEvent() {
    @event=Game.GameData.get_character(0);
    float oldx=@event.x;
    float oldy=@event.y;
//  Apply strict version of passable, which makes impassable
//  tiles that are passable only from certain directions
    if (!@event.passableStrict(@event.x,@event.y,Game.GameData.GamePlayer.direction)) {
      return;
    }
    switch (Game.GameData.GamePlayer.direction) {
    case 2: // down
      @event.move_down();
      break;
    case 4: // left
      @event.move_left();
      break;
    case 6: // right
      @event.move_right();
      break;
    case 8: // up
      @event.move_up();
      break;
    }
    if (Game.GameData.PokemonMap) Game.GameData.PokemonMap.addMovedEvent(@event_id);
    if (oldx!=@event.x || oldy!=@event.y) {
      Game.GameData.GamePlayer._lock();
      do {
        Game.UI.update(); //Graphics.update();
        Input.update();
        Game.UI.pbUpdateSceneMap();
      } while (@event.moving);
      Game.GameData.GamePlayer.unlock();
    }
  }

  public bool pbPushThisBoulder() {
    if (Game.GameData.PokemonMap.strengthUsed) {
      pbPushThisEvent();
    }
    return true;
  }

  public bool pbHeadbutt() {
    Game.GameData.pbHeadbutt(Game.GameData.get_character(0));
    return true;
  }

  public bool pbTrainerIntro(TrainerTypes symbol) {
    //if (Core.DEBUG) {
    //  if (!Game.pbTrainerTypeCheck(symbol)) return false;
    //}
    TrainerTypes trtype=symbol; //PBTrainers.const_get(symbol);
    Game.GameData.pbGlobalLock();
    Game.UI.pbPlayTrainerIntroME(trtype);
    return true;
  }

  public void pbTrainerEnd() {
    Game.pbGlobalUnlock();
    Avatar.GameEvent e=get_character(0);
    if (e != null) e.erase_route();
  }

  public object[] pbParams { get {
    return @parameters != null ? @parameters : @params;
  } }

  public Pokemon pbGetPokemon(int id) {
    return Game.GameData.Trainer.party[pbGet(id)];
  }

  public void pbSetEventTime(params int[] arg) {
    if (Game.GameData.Global.eventvars == null) Game.GameData.Global.eventvars=new Dictionary<KeyValuePair<int, int>, int>();
    DateTime time=Game.pbGetTimeNow();
    //time=time.to_i;
    Game.GameData.pbSetSelfSwitch(@event_id,"A",true);
    Game.GameData.Global.eventvars[new KeyValuePair<int, int>(@map_id,@event_id)]=time;
    foreach (var otherevt in arg) {
      Game.GameData.pbSetSelfSwitch(otherevt,"A",true);
      Game.GameData.Global.eventvars[new KeyValuePair<int, int>(@map_id,otherevt)]=time;
    }
  }

  public object getVariable(params int[] arg) {
    if (arg.Length==0) {
      if (Game.GameData.Global.eventvars == null) return null;
      return Game.GameData.Global.eventvars[new KeyValuePair<int, int>(@map_id,@event_id)];
    } else {
      return Game.GameData.GameVariables[arg[0]];
    }
  }

  public void setVariable(params int[] arg) {
    if (arg.Length==1) {
      if (Game.GameData.Global.eventvars == null) Game.GameData.Global.eventvars=new Dictionary<KeyValuePair<int, int>, int>();
      Game.GameData.Global.eventvars[new KeyValuePair<int, int>(@map_id,@event_id)]=arg[0];
    } else {
      Game.GameData.GameVariables[arg[0]]=arg[1];
      Game.GameData.GameMap.need_refresh=true;
    }
  }

  public bool tsOff (string c) {
    return get_character(0).tsOff(c);
  }

  public bool tsOn (string c) {
    return get_character(0).tsOn(c);
  }

  //alias isTempSwitchOn? tsOn?;
  //alias isTempSwitchOff? tsOff?;

  public void setTempSwitchOn(string c) {
    get_character(0).setTempSwitchOn(c);
  }

  public void setTempSwitchOff(string c) {
    get_character(0).setTempSwitchOff(c);
  }

// Must use this approach to share the methods because the methods already
// defined in a class override those defined in an included module
  /*CustomEventCommands=<<_END_;

  public void command_352() {
    scene=new PokemonSaveScene();
    screen=new PokemonSave(scene);
    screen.pbSaveScreen;
    return true;
  }

  public void command_125() {
    value = operate_value(pbParams[0], pbParams[1], pbParams[2]);
    Game.GameData.Trainer.money+=value;
    return true;
  }

  public void command_132() {
    Game.GameData.Global.nextBattleBGM=(pbParams[0]) ? pbParams[0].clone : null;
    return true;
  }

  public void command_133() {
    Game.GameData.Global.nextBattleME=(pbParams[0]) ? pbParams[0].clone : null;
    return true;
  }

  public void command_353() {
    pbBGMFade(1.0);
    pbBGSFade(1.0);
    pbFadeOutIn(99999){ Game.pbStartOver(true) }
  }

  public void command_314() {
    if (pbParams[0] == 0 && Game.GameData.Trainer && Game.GameData.Trainer.party) {
      pbHealAll();
    }
    return true;
  }
_END_;*/
}

//public partial class Interpreter : InterpreterFieldMixin {
//  //include InterpreterFieldMixin;
//  //eval(InterpreterFieldMixin.CustomEventCommands);
//}
//
//public partial class Game_Interpreter : InterpreterFieldMixin {
//  //include InterpreterFieldMixin;
//  //eval(InterpreterFieldMixin.CustomEventCommands);
//}
}