using PokemonUnity.Inventory;
using PokemonUnity.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using PokemonUnity.Overworld;
using PokemonUnity.Monster;
using PokemonUnity;
using PokemonUnity.Monster.Data;

namespace PokemonUnity.Inventory
{
    public static partial class Item { 
public const int ITEMID        = 0;
public const int ITEMNAME      = 1;
public const int ITEMPLURAL    = 2;
public const int ITEMPOCKET    = 3;
public const int ITEMPRICE     = 4;
public const int ITEMDESC      = 5;
public const int ITEMUSE       = 6;
public const int ITEMBATTLEUSE = 7;
public const int ITEMTYPE      = 8;
public const int ITEMMACHINE   = 9;

        /// <summary>
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        /// <seealso cref="HiddenMoves"/>
public static bool pbIsHiddenMove (Moves move) { //ToDo: Move to Game Class
  //if (Game.ItemData == null) return false;
  ////for (int i = 0; i < Game.ItemData.Count; i++) {
  //for (int i = 0; i < Game.MachineData.Count; i++) {
  //  //if (!pbIsHiddenMachine(i)) continue;
  //  //if(Game.ItemData[i].Pocket == ItemPockets.MACHINE)
  //  //atk=Game.ItemData[i][ITEMMACHINE];
  //  MachineData atk = Game.MachineData[i]; //HiddenMachine is not HiddenMove
  //  if (atk.Type != MachineData.MachineType.HiddenMachine && move==atk.Move) return true;
  //}
  Moves[] hidden = new Moves[] {
        Moves.SURF,
        Moves.CUT,
        Moves.STRENGTH,
        Moves.FLASH,
        Moves.FLY,
        Moves.WHIRLPOOL,
        Moves.WATERFALL,
        //Moves.RIDE,
        Moves.DIVE,
        Moves.ROCK_CLIMB,
        Moves.ROCK_SMASH,
        Moves.HEADBUTT,
        Moves.DEFOG }; 
  //return false;
  return hidden.Contains(move);
}

public static int pbGetPrice(Items item) {
  return Game.ItemData[item].Price; //[ITEMPRICE];
}

public static ItemPockets? pbGetPocket(Items item) {
  return Game.ItemData[item].Pocket; //[ITEMPOCKET];
}

// Important items can't be sold, given to hold, or tossed.
public static bool pbIsImportantItem (Items item) {
  return Game.ItemData.ContainsKey(item) && (pbIsKeyItem(item) ||
                             pbIsHiddenMachine(item) ||
                             (Core.INFINITETMS && pbIsTechnicalMachine(item)));
}

public static bool pbIsMachine (Items item) {
  return Game.ItemData[item].Category == ItemCategory.ALL_MACHINES || (pbIsTechnicalMachine(item) || pbIsHiddenMachine(item));
}

public static bool pbIsTechnicalMachine (Items item) {
  //return Game.ItemData.ContainsKey(item) && (Game.ItemData[item][ITEMUSE]==3);
  Items[] HMs = new Items[] { Items.TM01, Items.TM02, Items.TM03, Items.TM04, Items.TM05, Items.TM06, Items.TM07, Items.TM08, Items.TM09, Items.TM10, Items.TM11, Items.TM12, Items.TM13, Items.TM14, Items.TM15, Items.TM16, Items.TM17, Items.TM18, Items.TM19, Items.TM20, Items.TM21, Items.TM22, Items.TM23, Items.TM24, Items.TM25, Items.TM26, Items.TM27, Items.TM28, Items.TM29, Items.TM30, Items.TM31, Items.TM32, Items.TM33, Items.TM34, Items.TM35, Items.TM36, Items.TM37, Items.TM38, Items.TM39, Items.TM40, Items.TM41, Items.TM42, Items.TM43, Items.TM44, Items.TM45, Items.TM46, Items.TM47, Items.TM48, Items.TM49, Items.TM50, Items.TM51, Items.TM52, Items.TM53, Items.TM54, Items.TM55, Items.TM56, Items.TM57, Items.TM58, Items.TM59, Items.TM60, Items.TM61, Items.TM62, Items.TM63, Items.TM64, Items.TM65, Items.TM66, Items.TM67, Items.TM68, Items.TM69, Items.TM70, Items.TM71, Items.TM72, Items.TM73, Items.TM74, Items.TM75, Items.TM76, Items.TM77, Items.TM78, Items.TM79, Items.TM80, Items.TM81, Items.TM82, Items.TM83, Items.TM84, Items.TM85, Items.TM86, Items.TM87, Items.TM88, Items.TM89, Items.TM90, Items.TM91, Items.TM92, Items.TM93, Items.TM94, Items.TM95, Items.TM96, Items.TM97, Items.TM98, Items.TM99, Items.TM100 };
  return HMs.Contains(item);
}

public static bool pbIsHiddenMachine (Items item) {
  //return Game.ItemData.ContainsKey(item) && (Game.ItemData[item][ITEMUSE]==4);
  Items[] HMs = new Items[] { Items.HM01, Items.HM02, Items.HM03, Items.HM04, Items.HM05, Items.HM06, Items.HM07, Items.HM08 };
  return HMs.Contains(item);
}

public static bool pbIsMail (Items item) {
  return Game.ItemData.ContainsKey(item) && (ItemData.pbIsLetter(item)); //[ITEMTYPE]==1 || Game.ItemData[item][ITEMTYPE]==2
}

public static bool pbIsSnagBall (Items item) {
  return Game.ItemData.ContainsKey(item) && (ItemData.pbIsPokeBall(item) || Game.ItemData[item].Pocket == ItemPockets.POKEBALL);// || //[ITEMTYPE]==3
         //(Game.GameData.Global.snagMachine)); //Game.ItemData[item][ITEMTYPE]==4 && 4: SnagBall Item
}

public static bool pbIsPokeBall (Items item) {
  return Game.ItemData.ContainsKey(item) && (ItemData.pbIsPokeBall(item) || Game.ItemData[item].Pocket == ItemPockets.POKEBALL);//[ITEMTYPE]==4
}

public static bool pbIsBerry (Items item) {
  return Game.ItemData.ContainsKey(item) && ItemData.pbIsBerry(item); //[ITEMTYPE]==5 
}

public static bool pbIsKeyItem (Items item) {
  return Game.ItemData.ContainsKey(item) && (Game.ItemData[item].Pocket == ItemPockets.KEY);//[ITEMTYPE]==6
}

public static bool pbIsGem (Items item) {
  Items[] gems=new Items[] {Items.FIRE_GEM,Items.WATER_GEM,Items.ELECTRIC_GEM,Items.GRASS_GEM,Items.ICE_GEM,
        Items.FIGHTING_GEM,Items.POISON_GEM,Items.GROUND_GEM,Items.FLYING_GEM,Items.PSYCHIC_GEM,
        Items.BUG_GEM,Items.ROCK_GEM,Items.GHOST_GEM,Items.DRAGON_GEM,Items.DARK_GEM,
        Items.STEEL_GEM,Items.NORMAL_GEM,Items.FAIRY_GEM};
  //foreach (Items i in gems) {
  //  if (item == i) return true;
  //}
  //return false;
  return gems.Contains(item) || Game.ItemData[item].Category == ItemCategory.JEWELS;
}

public static bool pbIsEvolutionStone (Items item) {
  Items[] stones=new Items[] {Items.FIRE_STONE,Items.THUNDER_STONE,Items.WATER_STONE,Items.LEAF_STONE,Items.MOON_STONE,
          Items.SUN_STONE,Items.DUSK_STONE,Items.DAWN_STONE,Items.SHINY_STONE};
  //foreach (Items i in stones) {
  //  if (item == i) return true;
  //}
  //return false;
  return stones.Contains(item) || Game.ItemData[item].Category == ItemCategory.EVOLUTION;
}

public static bool pbIsMegaStone (Items item) {   // Does NOT include Red Orb/Blue Orb
  Items[] stones=new Items[] {Items.ABOMASITE,Items.ABSOLITE,Items.AERODACTYLITE,Items.AGGRONITE,Items.ALAKAZITE,
          Items.ALTARIANITE,Items.AMPHAROSITE,Items.AUDINITE,Items.BANETTITE,Items.BEEDRILLITE,
          Items.BLASTOISINITE,Items.BLAZIKENITE,Items.CAMERUPTITE,Items.CHARIZARDITE_X,Items.CHARIZARDITE_Y,
          Items.DIANCITE,Items.GALLADITE,Items.GARCHOMPITE,Items.GARDEVOIRITE,Items.GENGARITE,
          Items.GLALITITE,Items.GYARADOSITE,Items.HERACRONITE,Items.HOUNDOOMINITE,Items.KANGASKHANITE,
          Items.LATIASITE,Items.LATIOSITE,Items.LOPUNNITE,Items.LUCARIONITE,Items.MANECTITE,
          Items.MAWILITE,Items.MEDICHAMITE,Items.METAGROSSITE,Items.MEWTWONITE_X,Items.MEWTWONITE_Y,
          Items.PIDGEOTITE,Items.PINSIRITE,Items.SABLENITE,Items.SALAMENCITE,Items.SCEPTILITE,
          Items.SCIZORITE,Items.SHARPEDONITE,Items.SLOWBRONITE,Items.STEELIXITE,Items.SWAMPERTITE,
          Items.TYRANITARITE,Items.VENUSAURITE};
  //foreach (Items i in stones) {
  //  if (item == i) return true;
  //}
  //return false;
  return stones.Contains(item) || Game.ItemData[item].Category == ItemCategory.MEGA_STONES;
}

public static bool pbIsMulch (Items item) {
  Items[] mulches= new Items[] { Items.GROWTH_MULCH,Items.DAMP_MULCH,Items.STABLE_MULCH,Items.GOOEY_MULCH };
  //foreach (Items i in mulches) {
  //  if (item == i) return true;
  //}
  //return false;
  return mulches.Contains(item) || Game.ItemData[item].Category == ItemCategory.MULCH;
}

#region Move to Game class?
/*public static void pbChangeLevel(Pokemon pokemon,int newlevel,IScene scene) {
  if (newlevel<1) newlevel=1;
  if (newlevel>Core.MAXIMUMLEVEL) newlevel=Core.MAXIMUMLEVEL;
  if (pokemon.Level>newlevel) {
    int attackdiff=pokemon.ATK;
    int defensediff=pokemon.DEF;
    int speeddiff=pokemon.SPE;
    int spatkdiff=pokemon.SPA;
    int spdefdiff=pokemon.SPD;
    int totalhpdiff=pokemon.TotalHP;
    //pokemon.Level=newlevel;
    pokemon.SetLevel((byte)newlevel);
    //pokemon.Exp=Experience.GetStartExperience(pokemon.GrowthRate, newlevel);
    pokemon.calcStats();
    scene.pbRefresh();
    Game.pbMessage(Game._INTL("{1} was downgraded to Level {2}!",pokemon.Name,pokemon.Level));
    attackdiff=pokemon.ATK-attackdiff;
    defensediff=pokemon.DEF-defensediff;
    speeddiff=pokemon.SPE-speeddiff;
    spatkdiff=pokemon.SPA-spatkdiff;
    spdefdiff=pokemon.SPD-spdefdiff;
    totalhpdiff=pokemon.TotalHP-totalhpdiff;
    pbTopRightWindow(Game._INTL("Max. HP<r>{1}\r\nAttack<r>{2}\r\nDefense<r>{3}\r\nSp. Atk<r>{4}\r\nSp. Def<r>{5}\r\nSpeed<r>{6}",
       totalhpdiff,attackdiff,defensediff,spatkdiff,spdefdiff,speeddiff));
    pbTopRightWindow(Game._INTL("Max. HP<r>{1}\r\nAttack<r>{2}\r\nDefense<r>{3}\r\nSp. Atk<r>{4}\r\nSp. Def<r>{5}\r\nSpeed<r>{6}",
       pokemon.TotalHP,pokemon.ATK,pokemon.DEF,pokemon.SPA,pokemon.SPD,pokemon.SPE));
  } else if (pokemon.Level==newlevel) {
    Game.pbMessage(Game._INTL("{1}'s level remained unchanged.",pokemon.Name));
  } else {
    int attackdiff=pokemon.ATK;
    int defensediff=pokemon.DEF;
    int speeddiff=pokemon.SPE;
    int spatkdiff=pokemon.SPA;
    int spdefdiff=pokemon.SPD;
    int totalhpdiff=pokemon.TotalHP;
    int oldlevel=pokemon.Level;
    //pokemon.Level=newlevel;
    pokemon.SetLevel((byte)newlevel);
    //pokemon.Exp = Experience.GetStartExperience(pokemon.GrowthRate, newlevel);
    pokemon.ChangeHappiness(HappinessMethods.LEVELUP);
    pokemon.calcStats();
    scene.pbRefresh();
    Game.pbMessage(Game._INTL("{1} was elevated to Level {2}!",pokemon.Name,pokemon.Level));
    attackdiff=pokemon.ATK-attackdiff;
    defensediff=pokemon.DEF-defensediff;
    speeddiff=pokemon.SPE-speeddiff;
    spatkdiff=pokemon.SPA-spatkdiff;
    spdefdiff=pokemon.SPD-spdefdiff;
    totalhpdiff=pokemon.TotalHP-totalhpdiff;
    pbTopRightWindow(Game._INTL("Max. HP<r>+{1}\r\nAttack<r>+{2}\r\nDefense<r>+{3}\r\nSp. Atk<r>+{4}\r\nSp. Def<r>+{5}\r\nSpeed<r>+{6}",
       totalhpdiff,attackdiff,defensediff,spatkdiff,spdefdiff,speeddiff));
    pbTopRightWindow(Game._INTL("Max. HP<r>{1}\r\nAttack<r>{2}\r\nDefense<r>{3}\r\nSp. Atk<r>{4}\r\nSp. Def<r>{5}\r\nSpeed<r>{6}",
       pokemon.TotalHP,pokemon.ATK,pokemon.DEF,pokemon.SPA,pokemon.SPD,pokemon.SPE));
    //Moves[] movelist=pokemon.getMoveList();
    //foreach (Moves i in pokemon.getMoveList(LearnMethod.levelup)) { //movelist
    foreach (KeyValuePair<Moves,int> i in Game.PokemonMovesData[pokemon.Species].LevelUp) { 
      if (i.Value==pokemon.Level) {		// Learned a new move
        pbLearnMove(pokemon,i.Key,true);
      }
    }
    Pokemons newspecies=Evolution.pbCheckEvolution(pokemon)[0];
    if (newspecies>0) {
      Game.UI.pbFadeOutInWithMusic(99999, () => {
         IPokemonEvolutionScene evo=new PokemonEvolutionScene();
         evo.pbStartScreen(pokemon,newspecies);
         evo.pbEvolution();
         evo.pbEndScreen();
      });
    }
  }
}

public static int pbItemRestoreHP(Pokemon pokemon,int restorehp) {
  int newhp=pokemon.HP+restorehp;
  if (newhp>pokemon.TotalHP) newhp=pokemon.TotalHP;
  int hpgain=newhp-pokemon.HP;
  pokemon.HP=newhp;
  return hpgain;
}

public static bool pbHPItem(Pokemon pokemon,int restorehp,IScene scene) {
  if (pokemon.HP<=0 || pokemon.HP==pokemon.TotalHP || pokemon.isEgg) {
    scene.pbDisplay(Game._INTL("It won't have any effect."));
    return false;
  } else {
    int hpgain=pbItemRestoreHP(pokemon,restorehp);
    scene.pbRefresh();
    scene.pbDisplay(Game._INTL("{1}'s HP was restored by {2} points.",pokemon.Name,hpgain));
    return true;
  }
}

public static bool pbBattleHPItem(Pokemon pokemon,PokemonUnity.Combat.Pokemon battler,int restorehp,IScene scene) {
  if (pokemon.HP<=0 || pokemon.HP==pokemon.TotalHP || pokemon.isEgg) {
    scene.pbDisplay(Game._INTL("But it had no effect!"));
    return false;
  } else {
    int hpgain=pbItemRestoreHP(pokemon,restorehp);
    if (battler.IsNotNullOrNone()) battler.HP=pokemon.HP;
    scene.pbRefresh();
    scene.pbDisplay(Game._INTL("{1}'s HP was restored.",pokemon.Name,hpgain));
    return true;
  }
}

public static int pbJustRaiseEffortValues(Pokemon pokemon,Stats ev,int evgain) {
  int totalev=0;
  for (int i = 0; i < 6; i++) {
    totalev+=pokemon.EV[i];
  }
  if (totalev+evgain>Pokemon.EVLIMIT) {
    //  Bug Fix: must use "-=" instead of "="
    evgain-=totalev+evgain-Pokemon.EVLIMIT;
  }
  if (pokemon.EV[(int)ev]+evgain>Pokemon.EVSTATLIMIT) {
    //  Bug Fix: must use "-=" instead of "="
    evgain-=pokemon.EV[(int)ev]+evgain-Pokemon.EVSTATLIMIT;
  }
  if (evgain>0) {
    //pokemon.EV[ev]+=evgain;
    pokemon.EV[(int)ev]=(byte)(pokemon.EV[(int)ev]+evgain);
    pokemon.calcStats();
  }
  return evgain;
}

public static int pbRaiseEffortValues(Pokemon pokemon,Stats ev,int evgain=10,bool evlimit=true) {
  if (pokemon.EV[(int)ev]>=100 && evlimit) {
    return 0;
  }
  int totalev=0;
  for (int i = 0; i < 6; i++) {
    totalev+=pokemon.EV[i];
  }
  if (totalev+evgain>Pokemon.EVLIMIT) {
    evgain=Pokemon.EVLIMIT-totalev;
  }
  if (pokemon.EV[(int)ev]+evgain>Pokemon.EVSTATLIMIT) {
    evgain=Pokemon.EVSTATLIMIT-pokemon.EV[(int)ev];
  }
  if (evlimit && pokemon.EV[(int)ev]+evgain>100) {
    evgain=100-pokemon.EV[(int)ev];
  }
  if (evgain>0) {
    //pokemon.EV[ev]+=evgain;
    pokemon.EV[(int)ev]=(byte)(pokemon.EV[(int)ev]+evgain);
    pokemon.calcStats();
  }
  return evgain;
}

public static bool pbRaiseHappinessAndLowerEV(Pokemon pokemon,IScene scene,Stats ev,string[] messages) {
  bool h=(pokemon.Happiness<255);
  bool e=(pokemon.EV[(int)ev]>0);
  if (!h && !e) {
    scene.pbDisplay(Game._INTL("It won't have any effect."));
    return false;
  }
  if (h) {
    pokemon.ChangeHappiness(HappinessMethods.EVBERRY);
  }
  if (e) {
    pokemon.EV[(int)ev]-=10;
    if (pokemon.EV[(int)ev]<0) pokemon.EV[(int)ev]=0;
    pokemon.calcStats();
  }
  scene.pbRefresh();
  scene.pbDisplay(messages[2-(h ? 0 : 1)-(e ? 0 : 2)]);
  return true;
}

public static int pbRestorePP(Pokemon pokemon,int move,int pp) {
  if (pokemon.moves[move].MoveId==0) return 0;
  if (pokemon.moves[move].TotalPP==0) return 0;
  int newpp=pokemon.moves[move].PP+pp;
  if (newpp>pokemon.moves[move].TotalPP) {
    newpp=pokemon.moves[move].TotalPP;
  }
  int oldpp=pokemon.moves[move].PP;
  pokemon.moves[move].PP=(byte)newpp;
  return newpp-oldpp;
}

public static int pbBattleRestorePP(Pokemon pokemon,PokemonUnity.Combat.Pokemon battler,int move,int pp) {
  int ret=pbRestorePP(pokemon,move,pp);
  if (ret>0) {
    if (battler.IsNotNullOrNone()) battler.pbSetPP(battler.moves[move],pokemon.moves[move].PP);
  }
  return ret;
}

public static bool pbBikeCheck() {
  if (Game.GameData.Global.surfing ||
     (!Game.GameData.Global.bicycle && Terrain.onlyWalk(Game.GameData.pbGetTerrainTag()))) {
    Game.pbMessage(Game._INTL("Can't use that here."));
    return false;
  }
  if (Game.GameData.GamePlayer.pbHasDependentEvents()) {
    Game.pbMessage(Game._INTL("It can't be used when you have someone with you."));
    return false;
  }
  if (Game.GameData.Global.bicycle) {
    if (Game.pbGetMetadata(Game.GameData.GameMap.map_id,MetadataBicycleAlways)) {
      Game.pbMessage(Game._INTL("You can't dismount your Bike here."));
      return false;
    }
    return true;
  } else {
    bool? val=Game.pbGetMetadata(Game.GameData.GameMap.map_id,MetadataBicycle);
    if (val == null) val=Game.pbGetMetadata(Game.GameData.GameMap.map_id,MetadataOutdoor);
    if (val == null) {
      Game.pbMessage(Game._INTL("Can't use that here."));
      return false;
    }
    return true;
  }
}

public static Avatar.Character pbClosestHiddenItem() {
  List<Avatar.GameEvent> result = new List<Avatar.GameEvent>();
  float playerX=Game.GameData.GamePlayer.x;
  float playerY=Game.GameData.GamePlayer.y;
  foreach (Avatar.GameEvent @event in Game.GameData.GameMap.events.Values) {
    if (@event.name!="HiddenItem") continue;
    if (Math.Abs(playerX-@event.x)>=8) continue;
    if (Math.Abs(playerY-@event.y)>=6) continue;
    if (Game.GameData.GameSelfSwitches[(int)[Game.GameData.GameMap.map_id,@event.id,"A"]]) continue;
    result.Add(@event);
  }
  if (result.Count==0) return null;
  Avatar.Character ret=null;
  float retmin=0;
  foreach (Avatar.GameEvent @event in result) {
    float dist=Math.Abs(playerX-@event.x)+Math.Abs(playerY-@event.y);
    if (ret == null || retmin>dist) {
      ret=@event;
      retmin=dist;
    }
  }
  return ret;
}

public static void pbUseKeyItemInField(Items item) { //Move to game class
  if (!ItemHandlers.triggerUseInField(item)) {
    Game.pbMessage(Game._INTL("Can't use that here."));
  }
}

public static bool pbSpeciesCompatible (Pokemons species,Moves move) {
  //bool ret=false;
  //if (species<=0) return false;
  //data=load_data("Data/tm.dat");
  //if (!data[move]) return false;
  //return data[move].Any(item => item==species);
  return Game.PokemonMovesData[species].Machine.Contains(move);
}

public static int pbForgetMove(Pokemon pokemon,Moves moveToLearn) {
  int ret=-1;
  Game.UI.pbFadeOutIn(99999, () => {
     IPokemonSummaryScene scene=new PokemonSummaryScene();
     IPokemonSummary screen=new PokemonSummary(scene);
     ret=screen.pbStartForgetScreen(new Pokemon[] { pokemon },0,moveToLearn);
  });
  return ret;
}

public static bool pbLearnMove(Pokemon pokemon,Moves move,bool ignoreifknown=false,bool bymachine=false) {
  if (!pokemon.IsNotNullOrNone()) return false;
  string movename=move.ToString(TextScripts.Name);
  if (pokemon.isEgg && !Core.DEBUG) {
    Game.pbMessage(Game._INTL("{1} can't be taught to an Egg.",movename));
    return false;
  }
  if (pokemon is IShadowPokemon && pokemon.isShadow) {
    Game.pbMessage(Game._INTL("{1} can't be taught to this Pokémon.",movename));
    return false;
  }
  string pkmnname=pokemon.Name;
  for (int i = 0; i < 4; i++) {
    if (pokemon.moves[i].MoveId==move) {
      if (!ignoreifknown) Game.pbMessage(Game._INTL("{1} already knows {2}.",pkmnname,movename));
      return false;
    }
    if (pokemon.moves[i].MoveId==0) {
      pokemon.moves[i]=new Attack.Move(move);
      Game.pbMessage(Game._INTL("\\se[]{1} learned {2}!\\se[MoveLearnt]",pkmnname,movename));
      return true;
    }
  }
  do { //;loop
    Game.pbMessage(Game._INTL("{1} wants to learn the move {2}.",pkmnname,movename));
    Game.pbMessage(Game._INTL("However, {1} already knows four moves.",pkmnname));
    if (Game.pbConfirmMessage(Game._INTL("Should a move be deleted and replaced with {1}?",movename))) {
      Game.pbMessage(Game._INTL("Which move should be forgotten?"));
      int forgetmove=pbForgetMove(pokemon,move);
      if (forgetmove>=0) {
        string oldmovename=pokemon.moves[forgetmove].MoveId.ToString(TextScripts.Name);
        byte oldmovepp=pokemon.moves[forgetmove].PP;
        pokemon.moves[forgetmove]=new Attack.Move(move); // Replaces current/total PP
        if (bymachine) pokemon.moves[forgetmove].PP=Math.Min(oldmovepp,pokemon.moves[forgetmove].TotalPP);
        Game.pbMessage(Game._INTL("\\se[]1,\\wt[16] 2, and\\wt[16]...\\wt[16] ...\\wt[16] ... Ta-da!\\se[balldrop]"));
        Game.pbMessage(Game._INTL("\\se[]{1} forgot how to use {2}. And... {1} learned {3}!\\se[MoveLearnt]",pkmnname,oldmovename,movename));
        return true;
      } else if (Game.pbConfirmMessage(Game._INTL("Give up on learning the move {1}?",movename))) {
        Game.pbMessage(Game._INTL("{1} did not learn {2}.",pkmnname,movename));
        return false;
      }
    } else if (Game.pbConfirmMessage(Game._INTL("Give up on learning the move {1}?",movename))) {
      Game.pbMessage(Game._INTL("{1} did not learn {2}.",pkmnname,movename));
      return false;
    }
  } while (true);
}

public static bool pbCheckUseOnPokemon(Items item,Pokemon pokemon,IScreen screen) {
  return pokemon.IsNotNullOrNone() && !pokemon.isEgg;
}

public static bool pbConsumeItemInBattle(PokemonBag bag,Items item) {
  if (item!=0 && Game.ItemData[item].Flags.Consumable && //!=3 disappear after use
                 //Game.ItemData[item].Flags!=4 && //used on enemy and disappears after use (i.e. pokeball)
                 Game.ItemData[item].Flags.Useable_In_Battle) { //!=0 cannot be used in battle
    //  Delete the item just used from stock
    return Game.GameData.Bag.pbDeleteItem(item);
  }
  return false;
}

// Only called when in the party screen and having chosen an item to be used on
// the selected Pokémon
public static bool pbUseItemOnPokemon(Items item,Pokemon pokemon,IScene scene) {
  //if (Game.ItemData[item][ITEMUSE]==3 || Game.ItemData[item][ITEMUSE]==4) {		// TM or HM
  if (Item.pbIsMachine(item)) {
    Moves machine=Game.MachineData[(int)item].Move;
    if (machine==Moves.NONE) return false;
    string movename=machine.ToString(TextScripts.Name);
    if (pokemon.isShadow) { //? rescue false
      Game.pbMessage(Game._INTL("Shadow Pokémon can't be taught any moves."));
    } else if (!pokemon.isCompatibleWithMove(machine)) {
      Game.pbMessage(Game._INTL("{1} and {2} are not compatible.",pokemon.Name,movename));
      Game.pbMessage(Game._INTL("{1} can't be learned.",movename));
    } else {
      if (pbIsHiddenMachine(item)) {
        Game.pbMessage(Game._INTL("\\se[accesspc]Booted up an HM."));
        Game.pbMessage(Game._INTL(@"It contained {1}.\1",movename));
      } else {
        Game.pbMessage(Game._INTL("\\se[accesspc]Booted up a TM."));
        Game.pbMessage(Game._INTL(@"It contained {1}.\1",movename));
      }
      if (Game.pbConfirmMessage(Game._INTL("Teach {1} to {2}?",movename,pokemon.Name))) {
        if (pbLearnMove(pokemon,machine,false,true)) {
          if (pbIsTechnicalMachine(item) && !Core.INFINITETMS) Game.GameData.Bag.pbDeleteItem(item);
          return true;
        }
      }
    }
    return false;
  } else {
    bool ret=ItemHandlers.triggerUseOnPokemon(item,pokemon,scene);
    scene.pbClearAnnotations();
    scene.pbHardRefresh();
    if (ret && Game.ItemData[item].Flags.Consumable) {		//[ITEMUSE]==1 Usable on Pokémon, consumed
      Game.GameData.Bag.pbDeleteItem(item);
    }
    if (Game.GameData.Bag.pbQuantity(item)<=0) {
      Game.pbMessage(Game._INTL("You used your last {1}.",item.ToString(TextScripts.Name)));
    }
    return ret;
  }
  Game.pbMessage(Game._INTL("Can't use that on {1}.",pokemon.Name));
  return false;
}

public static int pbUseItem(PokemonBag bag,Items item,IScene bagscene=null) {
  bool found=false;
  //if (Game.ItemData[item][ITEMUSE]==3 || Game.ItemData[item][ITEMUSE]==4) {		// TM or HM
  if (Item.pbIsMachine(item)) {
    Moves machine=Game.MachineData[(int)item].Move;
    if (machine==Moves.NONE) return 0;
    if (Game.GameData.Trainer.pokemonCount==0) {
      Game.pbMessage(Game._INTL("There is no Pokémon."));
      return 0;
    }
    string movename=machine.ToString(TextScripts.Name);
    if (pbIsHiddenMachine(item)) {
      Game.pbMessage(Game._INTL("\\se[accesspc]Booted up an HM."));
      Game.pbMessage(Game._INTL(@"It contained {1}.\1",movename));
    } else {
      Game.pbMessage(Game._INTL("\\se[accesspc]Booted up a TM."));
      Game.pbMessage(Game._INTL(@"It contained {1}.\1",movename));
    }
    if (!Game.pbConfirmMessage(Game._INTL("Teach {1} to a Pokémon?",movename))) {
      return 0;
    } else if (Game.pbMoveTutorChoose(machine,null,true)) {
      if (pbIsTechnicalMachine(item) && !Core.INFINITETMS) bag.pbDeleteItem(item);
      return 1;
    } else {
      return 0;
    }
  } else if (Game.ItemData[item][ITEMUSE]==1 || Game.ItemData[item][ITEMUSE]==5) {		// Item is usable on a Pokémon
    if (Game.GameData.Trainer.pokemonCount==0) {
      Game.pbMessage(Game._INTL("There is no Pokémon."));
      return 0;
    }
    bool ret=false;
    List<string> annot=null;
    if (pbIsEvolutionStone(item)) {
      annot=new List<string>();
      foreach (var pkmn in Game.GameData.Trainer.party) {
        bool elig=(Evolution.pbCheckEvolution(pkmn,item)>0);
        annot.Add(elig ? Game._INTL("ABLE") : Game._INTL("NOT ABLE"));
      }
    }
    Game.UI.pbFadeOutIn(99999, () => {
       IPokemonScreen_Scene scene=new PokemonScreen_Scene();
       IPokemonScreen screen=new PokemonScreen(scene,Game.GameData.Trainer.party);
       screen.pbStartScene(Game._INTL("Use on which Pokémon?"),false,annot);
       do { //;loop
         scene.pbSetHelpText(Game._INTL("Use on which Pokémon?"));
         int chosen=screen.pbChoosePokemon();
         if (chosen>=0) {
           Pokemon pokemon=Game.GameData.Trainer.party[chosen];
           if (!pbCheckUseOnPokemon(item,pokemon,screen)) {
             Game.UI.pbPlayBuzzerSE();
           } else {
             ret=ItemHandlers.triggerUseOnPokemon(item,pokemon,screen);
             if (ret && Game.ItemData[item][ITEMUSE]==1) {		// Usable on Pokémon, consumed
               bag.pbDeleteItem(item);
             }
             if (bag.pbQuantity(item)<=0) {
               Game.pbMessage(Game._INTL("You used your last {1}.",item.ToString(TextScripts.Name)));
               break;
             }
           }
         } else {
           ret=false;
           break;
         }
       } while (true);
       screen.pbEndScene();
       if (bagscene!=null) bagscene.pbRefresh();
    });
    return ret ? 1 : 0;
  } else if (Game.ItemData[item][ITEMUSE]==2) {		// Item is usable from bag
    int intret=(int)ItemHandlers.triggerUseFromBag(item);
    switch (intret) {
    case 0:
      return 0;
      break;
    case 1: // Item used
      return 1;
      break;
    case 2: // Item used, end screen
      return 2;
      break;
    case 3: // Item used, consume item
      bag.pbDeleteItem(item);
      return 1;
      break;
    case 4: // Item used, end screen and consume item
      bag.pbDeleteItem(item);
      return 2;
      break;
    default:
      Game.pbMessage(Game._INTL("Can't use that here."));
      return 0;
    }
  } else {
    Game.pbMessage(Game._INTL("Can't use that here."));
    return 0;
  }
}

public static int pbChooseItem(int var=0,params Items[] args) {
  int ret=0;
  IPokemonBag_Scene scene=new PokemonBag_Scene();
  IPokemonBagScreen screen=new PokemonBagScreen(scene,Game.GameData.Bag);
  Game.UI.pbFadeOutIn(99999, () => { 
    ret=screen.pbChooseItemScreen();
  });
  if (var>0) Game.GameData.GameVariables[var]=ret;
  return ret;
}

// Shows a list of items to choose from, with the chosen item's ID being stored
// in the given Global Variable. Only items which the player has are listed.
public static Items pbChooseItemFromList(string message,int variable,params Items[] args) {
  List<string> commands=new List<string>();
  List<Items> itemid=new List<Items>();
  foreach (Items item in args) {
    //if (hasConst(PBItems,item)) {
      Items id=(Items)item;
      if (Game.GameData.Bag.pbQuantity(id)>0) {
        commands.Add(id.ToString(TextScripts.Name));
        itemid.Add(id);
      }
    //}
  }
  if (commands.Count==0) {
    Game.GameData.GameVariables[variable]=0;
    return 0;
  }
  commands.Add(Game._INTL("Cancel"));
  itemid.Add(0);
  int ret=Game.pbMessage(message,commands.ToArray(),-1);
  if (ret<0 || ret>=commands.Count-1) {
    Game.GameData.GameVariables[variable]=-1;
    return Items.NONE;
  } else {
    Game.GameData.GameVariables[variable]=itemid[ret];
    return itemid[ret];
  }
}

private static void pbTopRightWindow(string text) {
  //Window_AdvancedTextPokemon window=new Window_AdvancedTextPokemon(text);
  //window.z=99999;
  //window.width=198;
  //window.y=0;
  //window.x=Graphics.width-window.width;
  //Game.UI.pbPlayDecisionSE();
  //do { //;loop
  //  Graphics.update();
  //  Input.update();
  //  window.update();
  //  if (Input.trigger(Input.t)) {
  //    break;
  //  }
  //} while (true);
  //window.dispose();
}*/
#endregion
}

//public partial class ItemHandlerHash : HandlerHash {
//  public void initialize() {
//    super(:PBItems);
//  }
//}

        //Use `EventHandlerList` and replace dictionary/delegate with events? 
public static partial class ItemHandlers {
  //private static ItemHandlerHash UseFromBag=new ItemHandlerHash();
  //private static ItemHandlerHash UseInField=new ItemHandlerHash();
  //private static ItemHandlerHash UseOnPokemon=new ItemHandlerHash();
  //private static ItemHandlerHash BattleUseOnBattler=new ItemHandlerHash();
  //private static ItemHandlerHash BattleUseOnPokemon=new ItemHandlerHash();
  //private static ItemHandlerHash UseInBattle=new ItemHandlerHash();
  private static Dictionary<Items,Func<ItemUseResults>> UseFromBag=new Dictionary<Items, Func<ItemUseResults>>();
  private static Dictionary<Items,Action> UseInField=new Dictionary<Items, Action>();
  private static Dictionary<Items,UseOnPokemonDelegate> UseOnPokemon=new Dictionary<Items, UseOnPokemonDelegate>();
  private static Dictionary<Items,BattleUseOnBattlerDelegate> BattleUseOnBattler=new Dictionary<Items, BattleUseOnBattlerDelegate>();
  private static Dictionary<Items,BattleUseOnPokemonDelegate> BattleUseOnPokemon=new Dictionary<Items, BattleUseOnPokemonDelegate>();
  private static Dictionary<Items,UseInBattleDelegate> UseInBattle=new Dictionary<Items, UseInBattleDelegate>();
  public delegate bool UseOnPokemonDelegate(Items item, Monster.Pokemon pokemon, IHasDisplayMessage scene);
  public delegate bool BattleUseOnPokemonDelegate(Monster.Pokemon pokemon, Combat.Pokemon battler, IPokeBattle_Scene scene); //or Pokemon Party Scene
  public delegate bool BattleUseOnBattlerDelegate(Items item, Combat.Pokemon battler, IPokeBattle_Scene scene); //or Pokemon Party Scene
  public delegate void UseInBattleDelegate(Items item, PokemonUnity.Combat.Pokemon battler, PokemonUnity.Combat.Battle battle);

  public static void addUseFromBag(Items item,Func<ItemUseResults> proc) {
    if (!UseFromBag.ContainsKey(item))
      UseFromBag.Add(item,proc);
    else
      UseFromBag[item] = proc;
  }

  public static void addUseInField(Items item,Action proc) {
    if (!UseInField.ContainsKey(item))
      UseInField.Add(item,proc);
    else
      UseInField[item] = proc;
  }

  public static void addUseOnPokemon(Items item, UseOnPokemonDelegate proc) {
    if (!UseOnPokemon.ContainsKey(item))
      UseOnPokemon.Add(item,proc);
    else
      UseOnPokemon[item] = proc;
  }

  public static void addBattleUseOnBattler(Items item, BattleUseOnBattlerDelegate proc) {
    if (!BattleUseOnBattler.ContainsKey(item))
      BattleUseOnBattler.Add(item,proc);
    else
      BattleUseOnBattler[item] = proc;
  }

  public static void addBattleUseOnPokemon(Items item, BattleUseOnPokemonDelegate proc) {
    if (!BattleUseOnPokemon.ContainsKey(item))
      BattleUseOnPokemon.Add(item,proc);
    else
      BattleUseOnPokemon[item] = proc;
  }

        /// <summary>
        /// Shows "Use" option in Bag
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
  public static bool hasOutHandler(Items item) {                       
    return (!UseFromBag.ContainsKey(item) && UseFromBag[item]!=null) || (!UseOnPokemon.ContainsKey(item) && UseOnPokemon[item]!=null);
  }

        /// <summary>
        /// Shows "Register" option in Bag
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
  public static bool hasKeyItemHandler(Items item) {              
    return !UseInField.ContainsKey(item) && UseInField[item]!=null;
  }

  public static bool hasUseOnPokemon(Items item) {
    return !UseOnPokemon.ContainsKey(item) && UseOnPokemon[item]!=null;
  }

  public static bool hasBattleUseOnBattler(Items item) {
    return !BattleUseOnBattler.ContainsKey(item) && BattleUseOnBattler[item]!=null;
  }

  public static bool hasBattleUseOnPokemon(Items item) {
    return !BattleUseOnPokemon.ContainsKey(item) && BattleUseOnPokemon[item]!=null;
  }

  public static bool hasUseInBattle(Items item) {
    return !UseInBattle.ContainsKey(item) && UseInBattle[item]!=null;
    //return Game.ItemData[item].Flags.Useable_In_Battle;
  }

  /// <summary>
  /// </summary>
  /// <param name="item"></param>
  /// <returns>Return values: 0 = not used
  ///                         1 = used, item not consumed
  ///                         2 = close the Bag to use, item not consumed
  ///                         3 = used, item consumed
  ///                         4 = close the Bag to use, item consumed</returns>
  public static ItemUseResults triggerUseFromBag(Items item) {
//  Return value:
//  0 - Item not used
//  1 - Item used, don't end screen
//  2 - Item used, end screen
//  3 - Item used, consume item
//  4 - Item used, end screen, consume item
    if (!UseFromBag.ContainsKey(item) || UseFromBag[item] == null) { //Search List
      // Check the UseInField handler if present
      if (UseInField[item] != null) {
      //if (!Game.ItemData[item].Flags.Useable_Overworld) {
        UseInField[item].Invoke();
        //ItemHandlers.UseInField(item);
        return ItemUseResults.UsedNotConsumed; // item was used
      }
      return 0; // item was not used
    } else {
      return UseFromBag[item].Invoke();
      //return ItemHandlers.UseFromBag(item);
    }
  }

  public static bool triggerUseInField(Items item) {
    // No return value
    if (!UseInField.ContainsKey(item) || UseInField[item] == null) {
    //if (!Game.ItemData[item].Flags.Useable_Overworld) {
      return false;
    } else {
      UseInField[item].Invoke();
      //ItemHandlers.UseInField(item);
      return true;
    }
  }

  public static bool triggerUseOnPokemon(Items item,Pokemon pokemon,IHasDisplayMessage scene) {
    // Returns whether item was used
    if (!UseOnPokemon.ContainsKey(item) || UseOnPokemon[item] == null) { //Search List
      return false;
    } else {
      //return UseOnPokemon[item].Invoke(pokemon,scene);
      return (bool)UseOnPokemon[item].Invoke(item,pokemon,scene);
      //return ItemHandlers.UseOnPokemon(item,pokemon,scene);
    }
  }

  //ToDo: scene parameter is throwing errors in battle class, resolve in itemhandler method
  public static bool triggerBattleUseOnBattler(Items item,PokemonUnity.Combat.Pokemon battler,IHasDisplayMessage scene) { return false; }
  public static bool triggerBattleUseOnPokemon(Items item,Pokemon pokemon,PokemonUnity.Combat.Pokemon battler, IHasDisplayMessage scene) { return false; }

  public static bool triggerBattleUseOnBattler(Items item,PokemonUnity.Combat.Pokemon battler,IPokeBattle_Scene scene) {
    // Returns whether item was used
    if (!BattleUseOnBattler.ContainsKey(item) || BattleUseOnBattler[item] == null) {
    //if (!Game.ItemData[item].Flags.Useable_In_Battle) {
      return false;
    } else {
      return BattleUseOnBattler[item].Invoke(item,battler,scene);
      //return ItemHandlers.BattleUseOnBattler(item,battler,scene);
    }
  }

  public static bool triggerBattleUseOnPokemon(Items item,Pokemon pokemon,PokemonUnity.Combat.Pokemon battler,IPokeBattle_Scene scene) {
    // Returns whether item was used
    if (!BattleUseOnPokemon.ContainsKey(item) || BattleUseOnPokemon[item] == null) {
    //if (!Game.ItemData[item].Flags.Useable_In_Battle) {
      return false;
    } else {
      return BattleUseOnPokemon[item].Invoke(pokemon,battler,scene);
      //return ItemHandlers.BattleUseOnPokemon(item,pokemon,battler,scene);
    }
  }

  public static void triggerUseInBattle(Items item,PokemonUnity.Combat.Pokemon battler,PokemonUnity.Combat.Battle battle) {
    // Returns whether item was used
    if (!UseInBattle.ContainsKey(item) || UseInBattle[item] == null) {
    //if (!Game.ItemData[item].Flags.Useable_In_Battle) {
      return;
    } else {
      UseInBattle[item].Invoke(item,battler,battle);
      //ItemHandlers.UseInBattle(item,battler,battle);
    }
  }
}
}