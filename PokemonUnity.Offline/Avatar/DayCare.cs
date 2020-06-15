using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;

namespace PokemonUnity.Offline.Avatar
{
	/// <summary>
	/// </summary>
	public class DayCares
	{
public bool pbEggGenerated() {
  if (pbDayCareDeposited()!=2) return false;
  return Game.GameData.Player.DayCare.Egg==1;
}

public int pbDayCareDeposited() {
  int ret = 0;
  for (int i = 0; i < 2; i++) {
    if (Game.GameData.Player.DayCare[i].IsNotNullOrNone()) ret+=1;
  }
  return ret;
}

public void pbDayCareDeposit(int index) {
  for (int i = 0; i < 2; i++) {
    if (!Game.GameData.Player.DayCare[i].IsNotNullOrNone()) {
      Game.GameData.Player.DayCare[i]=Game.GameData.Player.Party[index];
      //Game.GameData.Player.DayCare[i].Level=Game.GameData.Player.Trainer.Party[index].Level;
      Game.GameData.Player.DayCare[i].Heal();
      Game.GameData.Player.Party[index]=null;
      Game.GameData.Player.Party.PackParty();
      //Game.GameData.Player.DayCare.Egg=0;
      //Game.GameData.Player.DayCare.EggSteps=0;
      return;
    }
  }
  GameDebug.LogError(_INTL("No room to deposit a Pokémon"));
  //throw new Exception(_INTL("No room to deposit a Pokémon"));
}

public bool pbDayCareGetLevelGain(int index) {//, int nameVariable, int levelVariable
  Pokemon pkmn=Game.GameData.Player.DayCare[index];
  if (!pkmn.IsNotNullOrNone()) return false;
  //$game_variables[nameVariable]=pkmn.Name;
  //$game_variables[levelVariable]=pkmn.Level-Game.GameData.Player.DayCare[index].Level;
  return true;
}

public void pbDayCareGetDeposited(int index) {//, int nameVariable, int costVariable
  for (int i = 0; i < 2; i++) {
    if ((index<0||i==index) && Game.GameData.Player.DayCare[i].IsNotNullOrNone()) {
      int cost=Game.GameData.Player.DayCare[i].Level-Game.GameData.Player.DayCare[i].Level;
      cost+=1;
      cost*=100;
      //if (costVariable>=0) $game_variables[costVariable]=cost;
      //if (nameVariable>=0) $game_variables[nameVariable]=Game.GameData.Player.DayCare[i].Name;
      return;
    }
  }
  GameDebug.LogError(_INTL("Can't find deposited Pokémon"));
  //throw new Exception(_INTL("Can't find deposited Pokémon"));
}

public bool pbIsDitto(Pokemon pokemon) {
  //dexdata = pbOpenDexData;
  //pbDexDataOffset(dexdata, pokemon.Species,31);
  //compat10=dexdata.fgetb;
  //compat11=dexdata.fgetb;
  //dexdata.close;
  EggGroups compat10=Game.PokemonData[pokemon.Species].EggGroup[0];
  EggGroups compat11=Game.PokemonData[pokemon.Species].EggGroup[1];
  return compat10 == EggGroups.DITTO ||
         compat11 == EggGroups.DITTO;
}

public bool pbDayCareCompatibleGender(Pokemon pokemon1, Pokemon pokemon2) {
  if ((pokemon1.IsFemale && pokemon2.IsMale) || 
     (pokemon1.IsMale && pokemon2.IsFemale)) { 
    return true;
  }
  bool ditto1 = pbIsDitto (pokemon1);
  bool ditto2 = pbIsDitto (pokemon2);
  if (ditto1 && !ditto2) return true;
  if (ditto2 && !ditto1) return true;
  return false;
}

public int pbDayCareGetCompat() {
  if (pbDayCareDeposited()==2) {
    Pokemon pokemon1=Game.GameData.Player.DayCare[0];
    Pokemon pokemon2 =Game.GameData.Player.DayCare[1];
    if (pokemon1.isShadow) return 0; //rescue false
    if (pokemon2.isShadow) return 0; //rescue false
    //dexdata=pbOpenDexData;
    //pbDexDataOffset(dexdata, pokemon1.Species,31);
    EggGroups compat10=Game.PokemonData[pokemon1.Species].EggGroup[0];
    EggGroups compat11=Game.PokemonData[pokemon1.Species].EggGroup[1];
    //pbDexDataOffset(dexdata, pokemon2.Species,31);
    EggGroups compat20=Game.PokemonData[pokemon2.Species].EggGroup[0];
    EggGroups compat21=Game.PokemonData[pokemon2.Species].EggGroup[1];
    //dexdata.close;
    if (compat10 != EggGroups.UNDISCOVERED &&
       compat11 != EggGroups.UNDISCOVERED &&
       compat20 != EggGroups.UNDISCOVERED &&
       compat21 != EggGroups.UNDISCOVERED) {
      if (compat10==compat20 || compat11==compat20 ||
         compat10==compat21 || compat11==compat21 ||
         compat10 == EggGroups.DITTO ||
         compat11 == EggGroups.DITTO ||
         compat20 == EggGroups.DITTO ||
         compat21 == EggGroups.DITTO) {
        if (pbDayCareCompatibleGender(pokemon1, pokemon2)) {
          int ret=1;
          if (pokemon1.Species==pokemon2.Species) ret+=1;
          if (pokemon1.OT.TrainerID!=pokemon2.OT.TrainerID) ret+=1;
          return ret;
        }
      }
    }
  }
  return 0;
}

//public void pbDayCareGetCompatibility(variable) {
//  $game_variables[variable]=pbDayCareGetCompat;
//}

public void pbDayCareWithdraw(int index) {
  if (!Game.GameData.Player.DayCare[index].IsNotNullOrNone()) {
    GameDebug.LogError(_INTL("There's no Pokémon here..."));
    //throw new Exception(_INTL("There's no Pokémon here..."));
  } else if (Game.GameData.Player.Party.Length>=6) {
    GameDebug.LogError(_INTL("Can't store the Pokémon..."));
    //throw new Exception(_INTL("Can't store the Pokémon..."));
  }
  else {
    Game.GameData.Player.Party[Game.GameData.Player.Party.Length]=Game.GameData.Player.DayCare[index];
    Game.GameData.Player.DayCare[index]=null;
    //Game.GameData.Player.DayCare[index].Level=0;
    //Game.GameData.Player.DayCare.Egg=0;
  }
}

public void pbDayCareChoose(string text,int variable) {
  int count=pbDayCareDeposited();
  if (count==0) {
    GameDebug.LogError(_INTL("There's no Pokémon here..."));
    //throw new Exception(_INTL("There's no Pokémon here..."));
  } else if (count==1) {
    //$game_variables[variable]=Game.GameData.Player.DayCare[0]? 0 : 1;
  }
  else {
    List<string> choices=new List<string>();
    for (int i = 0; i < 2; i++) {
      Pokemon pokemon=Game.GameData.Player.DayCare[i];
      if (pokemon.Gender == true) {//isMale
        choices.Add(string.Format("{0} (♂, Lv{1})", pokemon.Name, pokemon.Level));
      } else if (pokemon.Gender == false) {//isFemale
        choices.Add(string.Format("{0} (♀, Lv{1})", pokemon.Name, pokemon.Level));
      }
      else {
        choices.Add(string.Format("{0} (Lv{1})",pokemon.Name,pokemon.Level));
      }
    }
    choices.Add(_INTL("CANCEL"));
    //int command=Game.pbMessage(text, choices, choices.Count);
    //$game_variables[variable]=(command==2) ? -1 : command;
  }
}

public void pbDayCareGenerateEgg() {
  if (pbDayCareDeposited()!=2) {
    return;
  } else if (Game.GameData.Player.Party.Length>=6) {
    GameDebug.LogError(_INTL("Can't store the egg"));
    //throw new Exception(_INTL("Can't store the egg"));
  }
  Pokemon pokemon0 =Game.GameData.Player.DayCare[0];
  Pokemon pokemon1 =Game.GameData.Player.DayCare[1];
  Pokemon mother = null;
  Pokemon father=null;
  Pokemons babyspecies = 0;
  bool ditto0=pbIsDitto(pokemon0);
  bool ditto1=pbIsDitto(pokemon1);
  if (pokemon0.Gender == false || ditto0) {//isFemale?
    babyspecies=(ditto0)? pokemon1.Species : pokemon0.Species;
    mother = pokemon0;
    father= pokemon1;
  }
  else {
    babyspecies= (ditto1) ? pokemon0.Species : pokemon1.Species;
    mother= pokemon1;
    father= pokemon0;
  }
  //babyspecies = pbGetBabySpecies(babyspecies, mother.Item, father.Item);
  if (babyspecies == Pokemons.MANAPHY) { //&& hasConst(PBSpecies,:PHIONE)
    babyspecies=Pokemons.PHIONE;
  } else if (babyspecies == Pokemons.NIDORAN_F || //&& hasConst(PBSpecies,:NIDORANmA)
        babyspecies == Pokemons.NIDORAN_M) { //&& hasConst(PBSpecies,:NIDORANfE)
    babyspecies=new Pokemons[] { Pokemons.NIDORAN_M,
                 Pokemons.NIDORAN_F }[Core.Rand.Next(2)];
} else if (babyspecies == Pokemons.VOLBEAT || //&& hasConst(PBSpecies,:ILLUMISE)
        babyspecies == Pokemons.ILLUMISE) { //&& hasConst(PBSpecies,:VOLBEAT)
    babyspecies=new Pokemons[] { Pokemons.VOLBEAT,
                 Pokemons.ILLUMISE }[Core.Rand.Next(2)];
}
// Generate egg
  //Pokemon egg = new PokeBattle_Pokemon(babyspecies, EGGINITIALLEVEL,Game.GameData.Player.Trainer);
  Pokemon egg = new Pokemon(babyspecies, Core.EGGINITIALLEVEL, true);
  // Randomise personal ID
  /*int pid=Core.Rand.Next(65536);
  pid|=(Core.Rand.Next(65536)<<16);
  egg.personalID=pid;
  // Inheriting form
  if (babyspecies == Pokemons.BURMY ||
     babyspecies == Pokemons.SHELLOS ||
     babyspecies == Pokemons.BASCULIN) {
    egg.form=mother.form;
  }
  // Inheriting Moves
  List<Moves> moves=new List<Moves>();
  List<Moves> othermoves=new List<Moves>();
  Pokemon movefather=father; Pokemon movemother=mother;
  if (pbIsDitto(movefather) && !mother.isFemale?) {
     movefather = mother; movemother=father;
     }
  // Initial Moves
  Moves[] initialmoves=egg.getMoveList;
  foreach (var k in initialmoves) {
    if (k[0]<=EGGINITIALLEVEL) {
      moves.Add(k[1]);
    }
    else {
      if (mother.hasMove(k[1]) && father.hasMove(k[1])) othermoves.Add(k[1]);
      }
  }
  // Inheriting Natural Moves
  foreach (var move in othermoves) {
    moves.Add(move);
  }
  // Inheriting Machine Moves
  if (!Core.USENEWBATTLEMECHANICS) {
    for (int i = 0; i < $ItemData.Length; i++) {
      if (!$ItemData[i]) continue;
      atk =$ItemData[i][ITEMMACHINE];
       if (!atk || atk==0) continue;
      if (egg.isCompatibleWithMove(atk)) {
         if (movefather.hasMove(atk)) moves.Add(atk);
        }
    }
  }
  // Inheriting Egg Moves
  if (movefather.isMale?) {
    pbRgssOpen("Data/eggEmerald.dat","rb"){| f |
       f.pos = (babyspecies - 1) * 8;
       offset = f.fgetdw;
       length = f.fgetdw;
       if (length > 0) {
         f.pos = offset;
         i = 0; do { //loop
           atk = f.fgetw;
           if (movefather.hasMove (atk)) moves.Add(atk);
           i += 1;
         } unless (i < length) //break
       }
    }
}
  if (Core.USENEWBATTLEMECHANICS) {
    pbRgssOpen("Data/eggEmerald.dat","rb"){| f |
       f.pos = (babyspecies - 1) * 8;
       offset = f.fgetdw;
       length = f.fgetdw;
       if (length > 0) {
         f.pos = offset;
         i = 0; do { //loop
           atk = f.fgetw;
           if (movemother.hasMove (atk)) moves.Add(atk);
           i += 1;
         } unless (i < length); //break
       }
    }
}
// Volt Tackle
  bool lightball = false;
  if ((father.Species == Pokemons.PIKACHU || 
      father.Species == Pokemons.RAICHU) && 
      father.Item == Items.LIGHT_BALL) {
    lightball=true;
  }
  if ((mother.Species == Pokemons.PIKACHU || 
      mother.Species == Pokemons.RAICHU) && 
      mother.Item == Items.LIGHT_BALL) {
    lightball=true;
  }
  if (lightball && babyspecies == Pokemons.PICHU) { //&& hasConst(PBMoves,:VOLTTACKLE)
    moves.Add(Moves.VOLTTACKLE);
  }
  moves|=[]; // remove duplicates
  */Moves[] moves=new Moves[4];
  // Assembling move list
  Move[] finalmoves=new Move[4];
  int listend=moves.Length-4;
  if (listend<0) listend=0;
  int j=0;
  for (int i = listend; i < listend+3; i++) {
    Moves moveid=(i>=moves.Length) ? 0 : moves[i];
    finalmoves[j] = new Move(moveid);
      j+=1;
  }
// Inheriting Individual Values
  int[] ivs =new int[6];
  for (int i = 0; i < 6; i++) {
    ivs[i]=Core.Rand.Next(32);
  }
  //Stats?[] ivinherit =new Stats?[2];
  List<Stats?> ivinherit =new List<Stats?>();
  for (int i = 0; i < 2; i++) {
    Pokemon parent=new Pokemon[] { mother, father }[i];
    if (parent.Item == Items.POWER_WEIGHT)   ivinherit[i]=Stats.HP;
    if (parent.Item == Items.POWER_BRACER)   ivinherit[i]=Stats.ATTACK;
    if (parent.Item == Items.POWER_BELT)     ivinherit[i]=Stats.DEFENSE;
    if (parent.Item == Items.POWER_ANKLET)   ivinherit[i]=Stats.SPEED;
    if (parent.Item == Items.POWER_LENS)     ivinherit[i]=Stats.SPATK;
    if (parent.Item == Items.POWER_BAND)     ivinherit[i]=Stats.SPDEF;
  }
  int num = 0; int r=Core.Rand.Next(2);
  for (int i = 0; i < 2; i++) {
    if (ivinherit[r]!=null) {
      Pokemon parent =new Pokemon[] { mother, father }[r];
      ivs[(int)ivinherit[r].Value]=parent.IV[(int)ivinherit[r].Value];
      num+=1;
      break;
    }
    r = (r + 1) % 2;
  }
  Stats[] stats =new Stats[] {Stats.HP, Stats.ATTACK, Stats.DEFENSE,
         Stats.SPEED, Stats.SPATK, Stats.SPDEF };
  int limit=(Core.USENEWBATTLEMECHANICS && (mother.Item == Items.DESTINY_KNOT ||
         father.Item == Items.DESTINY_KNOT)) ? 5 : 3;
  do { //loop;
    List<int> freestats=new List<int>();
    foreach (Stats i in stats) {
      if (!ivinherit.Contains(i)) freestats.Add((int)i);
     }
    if (freestats.Count==0) break;
    r=freestats[Core.Rand.Next(freestats.Count)];
    Pokemon parent =new Pokemon[] { mother, father }[Core.Rand.Next(2)];
    ivs[r]=parent.IV[r];
    ivinherit.Add((Stats)r);
    num+=1;
    if (num>=limit) break;
  } while (true);
  // Inheriting nature
  List<Natures> newnatures=new List<Natures>();
  if (mother.Item == Items.EVERSTONE) newnatures.Add(mother.Nature);
  if (father.Item == Items.EVERSTONE) newnatures.Add(father.Nature);
  if (newnatures.Count>0) {
    //egg.setNature(newnatures[Core.Rand.Next(newnatures.Count)]);
  }
  // Masuda method and Shiny Charm
  int shinyretries=0;
  //if (father.Language!=mother.Language) shinyretries+=5;
  if (Game.GameData.Player.Bag.GetItemAmount(Items.SHINY_CHARM)>0) shinyretries+=2; //&& hasConst(PBItems,:SHINYCHARM)
  if (shinyretries>0) {
    for (int i = 0; i < shinyretries; i++) {
      //if (egg.isShiny) break;
      //egg.PersonalID=Core.Rand.Next(65536)|(Core.Rand.Next(65536)<<16);
    }
  }
  // Inheriting ability from the mother
  if (!ditto0 && !ditto1) {
    if (mother.hasHiddenAbility()) {
      //if (Core.Rand.Next(10)<6) egg.setAbility(mother.AbilityIndex);
    }
    else {
      if (Core.Rand.Next(10)<8) {
        //egg.setAbility(mother.AbilityIndex);
      }
      else {
        //egg.setAbility((mother.AbilityIndex+1)%2);
      }
    }
  } else if (((!ditto0 && ditto1) || (!ditto1 && ditto0)) && Core.USENEWBATTLEMECHANICS) { 
   Pokemon parent = (!ditto0) ? mother : father;
    if (parent.hasHiddenAbility()) {
      //if (Core.Rand.Next(10)<6) egg.setAbility(parent.AbilityIndex);
    }
  }
  // Inheriting Poké Ball from the mother
  if (mother.Gender == false && //isFemale?
     mother.ballUsed != Items.MASTER_BALL &&
     mother.ballUsed != Items.CHERISH_BALL) {
    //egg.ballUsed=mother.ballUsed;
  }
  egg.IV[0]= ivs[0];
  egg.IV[1]= ivs[1];
  egg.IV[2]= ivs[2];
  egg.IV[3]= ivs[3];
  egg.IV[4]= ivs[4];
  egg.IV[5]= ivs[5];
  egg.moves[0]= finalmoves[0];
  egg.moves[1]= finalmoves[1];
  egg.moves[2]= finalmoves[2];
  egg.moves[3]= finalmoves[3];
  //egg.calcStats;
  //egg.obtainText= _INTL("Day-Care Couple");
  //egg.Name= _INTL("Egg");
  //dexdata= pbOpenDexData;
  //pbDexDataOffset(dexdata, babyspecies,21);
  //eggsteps=dexdata.fgetw;
  //dexdata.close;
  //egg.eggsteps= eggsteps;
  if (Core.Rand.Next(65536)<Core.POKERUSCHANCE) {
    egg.GivePokerus();
  }
  Game.GameData.Player.Party[Game.GameData.Player.Party.Length]= egg;
}

/*Events.onStepTaken+=proc {//| sender,e |
    if (!Game.GameData.Player.Trainer) continue;
    int deposited = pbDayCareDeposited;
   if (deposited == 2 && Game.GameData.Player.DayCare.Egg == 0) {
     if (!Game.GameData.Player.DayCare.EggSteps) Game.GameData.Player.DayCare.EggSteps = 0;
     Game.GameData.Player.DayCare.EggSteps += 1;
     if (Game.GameData.Player.DayCare.EggSteps == 256) {
       Game.GameData.Player.DayCare.EggSteps = 0;
       int compatval =new int[] { 0, 20, 50, 70 }[pbDayCareGetCompat];
       if (Game.GameData.Player.Bag.GetItemAmount(Items.OVAL_CHARM) > 0) { //&& hasConst (PBItems,: OVALCHARM)
         compatval =new int[] { 0, 40, 80, 88 }[pbDayCareGetCompat];
       }
       int rnd = Core.Rand.Next(100);
       if (rnd < compatval) {
         // Egg is generated
         Game.GameData.Player.DayCare.Egg = 1;
       }
     }
   }
   for (int i = 0; i < 2; i++) {
     Pokemon pkmn =Game.GameData.Player.DayCare[i];
     if (!pkmn) continue;
     int maxexp = Experience.GetMaxExperience(pkmn.growthrate);
     if (pkmn.exp < maxexp) {
       int oldlevel = pkmn.Level;
       //pkmn.exp += 1;
       pkmn.AddExperience(1);
       //if (pkmn.Level != oldlevel) {
       //  //pkmn.calcStats;
       //  movelist = pkmn.getMoveList;
       //  foreach (var i in movelist) {
       //    if (i[0] == pkmn.Level) pkmn.pbLearnMove(i[1]);	// Learned a new move
       //  }
       //}
     }
   }
}*/
public string _INTL(string msg, params object[] arg)
{
    return string.Format(msg, arg);
}
	}
}