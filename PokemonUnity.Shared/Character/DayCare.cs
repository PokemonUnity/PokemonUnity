using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PokemonEssentials.Interface.EventArg;
using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;

namespace PokemonUnity.Character
{
	/// <summary>
	/// </summary>
	//Daycare Data
	//(Slot 1) Occupied Flag 
	//(Slot 1) Steps Taken Since Depositing 
	//(Slot 1) Box EK6 1 
	//(Slot 2) Occupied Flag 
	//(Slot 2) Steps Taken Since Depositing2 
	//(Slot 2) Box EK6 2 
	//Flag (egg available) 
	//RNG Seed
    //ToDo: Also need to record the number of levels gained...
	public class DayCare
	{
		#region Variables
		/// <summary>
		/// </summary>
		// KeyValuePair<Pokemon,steps>[]
		public KeyValuePair<Pokemon, int>[] Slot	{ get; private set; }
		public bool HasEgg	{ get; }
		public int Egg	{ get; }
		public Pokemon this[int index]
		{ 
			get { return Slot[index].Key; }
			set { Slot[index] = new KeyValuePair<Pokemon, int>(value, 0); } //ToDo: Add if/else null?
		}
		#endregion

		#region Constructor
		public DayCare(int slots)
		{
			Slot = new KeyValuePair<Pokemon, int>[slots];
		}
		#endregion

		#region Methods
public static bool pbEggGenerated() {
  if (pbDayCareDeposited()!=2) return false;
  return Game.GameData.Global.daycareEgg;//==1;
}

public static int pbDayCareDeposited() {
  int ret=0;
  for (int i = 0; i < 2; i++) {
    if (Game.GameData.Global.daycare[i].IsNotNullOrNone()) ret+=1;//[0]
  }
  return ret;
}

public static void pbDayCareDeposit(int index) {
  for (int i = 0; i < 2; i++) {
    if (!Game.GameData.Global.daycare[i].IsNotNullOrNone()) {//[0]
      Game.GameData.Global.daycare[i]=Game.GameData.Player.Party[index];//[0]
      //Game.GameData.Global.daycare[i][1]=Game.GameData.Player.Party[index].Level;
      Game.GameData.Global.daycare[i].Heal();//[0]
      Game.GameData.Player.Party[index]=null;
      //Game.GameData.Player.Party.compact()!;
      Game.GameData.Player.Party.PackParty();
      Game.GameData.Global.daycareEgg=false;//0;
      Game.GameData.Global.daycareEggSteps=0;
      return;
    }
  }
  //raise Game._INTL("No room to deposit a Pokémon") ;
  throw new Exception(Game._INTL("No room to deposit a Pokémon"));
}

public static bool pbDayCareGetLevelGain(int index,int nameVariable,int levelVariable) {
  Pokemon pkmn=Game.GameData.Global.daycare[index];//[0];
  if (!pkmn.IsNotNullOrNone()) return false;
  Game.GameData.GameVariables[nameVariable]=pkmn.Name;
  Game.GameData.GameVariables[levelVariable]=pkmn.Level-Game.GameData.Global.daycare[index].Level;//[1];
  return true;
}

public static void pbDayCareGetDeposited(int index,int nameVariable,int costVariable) {
  for (int i = 0; i < 2; i++) {
    if ((index<0||i==index) && Game.GameData.Global.daycare[i].IsNotNullOrNone()) {//[0]
      int cost=Game.GameData.Global.daycare[i].Level-Game.GameData.Global.daycare[i].Level;//[0]|[1] ToDo: Levels raised since drop-off
      cost+=1;
      cost*=100;
      if (costVariable>=0) Game.GameData.GameVariables[costVariable]=cost;
      if (nameVariable>=0) Game.GameData.GameVariables[nameVariable]=Game.GameData.Global.daycare[i].Name;//[0]
      return;
    }
  }
  //raise Game._INTL("Can't find deposited Pokémon");
  throw new Exception(Game._INTL("Can't find deposited Pokémon"));
}

public static bool pbIsDitto (Pokemon pokemon) {
  //dexdata=pbOpenDexData();
  //pbDexDataOffset(dexdata,pokemon.Species,31);
  EggGroups compat10=Game.PokemonData[pokemon.Species].EggGroup[0]; //dexdata.fgetb();
  EggGroups compat11=Game.PokemonData[pokemon.Species].EggGroup[1]; //dexdata.fgetb();
  //dexdata.close();
  return compat10 == EggGroups.DITTO ||
         compat11 == EggGroups.DITTO;
}

public static bool pbDayCareCompatibleGender(Pokemon pokemon1,Pokemon pokemon2) {
  if ((pokemon1.IsFemale && pokemon2.IsMale) ||
     (pokemon1.IsMale && pokemon2.IsFemale)) {
    return true;
  }
  bool ditto1=pbIsDitto(pokemon1);
  bool ditto2=pbIsDitto(pokemon2);
  if (ditto1 && !ditto2) return true;
  if (ditto2 && !ditto1) return true;
  return false;
}

public static int pbDayCareGetCompat() {
  if (pbDayCareDeposited()==2) {
    Pokemon pokemon1=Game.GameData.Global.daycare[0];//[0];
    Pokemon pokemon2=Game.GameData.Global.daycare[1];//[0];
    if ((pokemon1.isShadow)) return 0; //? rescue false
    if ((pokemon2.isShadow)) return 0; //? rescue false
    //dexdata=pbOpenDexData();
    //pbDexDataOffset(dexdata,pokemon1.Species,31);
    EggGroups compat10=Game.PokemonData[pokemon1.Species].EggGroup[0]; //dexdata.fgetb();
    EggGroups compat11=Game.PokemonData[pokemon1.Species].EggGroup[1]; //dexdata.fgetb();
    //pbDexDataOffset(dexdata,pokemon2.Species,31);
    EggGroups compat20=Game.PokemonData[pokemon1.Species].EggGroup[0]; //dexdata.fgetb();
    EggGroups compat21=Game.PokemonData[pokemon1.Species].EggGroup[1]; //dexdata.fgetb();
    //dexdata.close();
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
        if (pbDayCareCompatibleGender(pokemon1,pokemon2)) {
          int ret=1;
          if (pokemon1.Species==pokemon2.Species) ret+=1;
          if (pokemon1.TrainerId!=pokemon2.TrainerId) ret+=1;
          return ret;
        }
      }
    }
  }
  return 0;
}

public static void pbDayCareGetCompatibility(int variable) {
  Game.GameData.GameVariables[variable]=pbDayCareGetCompat();
}

public static void pbDayCareWithdraw(int index) {
  if (!Game.GameData.Global.daycare[index].IsNotNullOrNone()) {//[0]
    //raise Game._INTL("There's no Pokémon here...");
    throw new Exception(Game._INTL("There's no Pokémon here..."));
  } else if (Game.GameData.Player.Party.Length>=6) {
    //raise Game._INTL("Can't store the Pokémon...");
    throw new Exception(Game._INTL("Can't store the Pokémon..."));
  } else {
    Game.GameData.Player.Party[Game.GameData.Player.Party.Length]=Game.GameData.Global.daycare[index];//[0];
    Game.GameData.Global.daycare[index]=null;//[0]
    //Game.GameData.Global.daycare[index][1]=0;
    Game.GameData.Global.daycareEgg=false;//0;
  }
}

public static void pbDayCareChoose(string text,int variable) {
  int count=pbDayCareDeposited();
  if (count==0) {
    //raise Game._INTL("There's no Pokémon here...");
    throw new Exception(Game._INTL("There's no Pokémon here..."));
  } else if (count==1) {
    Game.GameData.GameVariables[variable]=Game.GameData.Global.daycare[0].IsNotNullOrNone() ? 0 : 1;//[0]
  } else {
    List<string> choices=new List<string>();
    for (int i = 0; i < 2; i++) {
      Pokemon pokemon=Game.GameData.Global.daycare[i];//[0]
      if (pokemon.IsMale) {
        choices.Add(Game._INTL("{1} (♂, Lv{2})",pokemon.Name,pokemon.Level));
      } else if (pokemon.IsFemale) {
        choices.Add(Game._INTL("{1} (♀, Lv{2})",pokemon.Name,pokemon.Level));
      } else {
        choices.Add(Game._INTL("{1} (Lv{2})",pokemon.Name,pokemon.Level));
      }
    }
    choices.Add(Game._INTL("CANCEL"));
    int command=Game.pbMessage(text,choices.ToArray(),choices.Count);
    Game.GameData.GameVariables[variable]=(command==2) ? -1 : command;
  }
}

public static void pbDayCareGenerateEgg() {
  if (pbDayCareDeposited()!=2) {
    return;
  } else if (Game.GameData.Player.Party.Length>=6) {
    //raise Game._INTL("Can't store the egg");
    throw new Exception(Game._INTL("Can't store the egg"));
  }
  Pokemon pokemon0=Game.GameData.Global.daycare[0];//[0]
  Pokemon pokemon1=Game.GameData.Global.daycare[1];//[0]
  Pokemon mother=null;
  Pokemon father=null;
  Pokemons babyspecies=0;
  bool ditto0=pbIsDitto(pokemon0);
  bool ditto1=pbIsDitto(pokemon1);
  if ((pokemon0.IsFemale || ditto0)) {
    babyspecies=(ditto0) ? pokemon1.Species : pokemon0.Species;
    mother=pokemon0;
    father=pokemon1;
  } else {
    babyspecies=(ditto1) ? pokemon0.Species : pokemon1.Species;
    mother=pokemon1;
    father=pokemon0;
  }
  babyspecies= PokemonUnity.Monster.Evolution.pbGetBabySpecies(babyspecies,mother.Item,father.Item);
  if (babyspecies == Pokemons.MANAPHY) { //&& hasConst?(PBSpecies,:PHIONE)
    babyspecies=Pokemons.PHIONE;
  } else if ((babyspecies == Pokemons.NIDORAN_F) || //&& hasConst?(PBSpecies,:NIDORANmA)
        (babyspecies == Pokemons.NIDORAN_M)) { //&& hasConst?(PBSpecies,:NIDORANfE)
    babyspecies=new Pokemons[]{Pokemons.NIDORAN_M,
                 Pokemons.NIDORAN_F }[Core.Rand.Next(2)];
  } else if ((babyspecies == Pokemons.VOLBEAT) || //&& hasConst?(PBSpecies,:ILLUMISE)
        (babyspecies == Pokemons.ILLUMISE)) { //&& hasConst?(PBSpecies,:VOLBEAT)
    babyspecies=new Pokemons[] {Pokemons.VOLBEAT,
                 Pokemons.ILLUMISE }[Core.Rand.Next(2)];
  }
  //Generate egg
  //Combat.Pokemon egg=new PokeBattle_Pokemon(babyspecies,Core.EGGINITIALLEVEL,Game.GameData.Player);
  Pokemon egg=new Pokemon(babyspecies,Core.EGGINITIALLEVEL,isEgg: true);//,Game.GameData.Player
  //Randomise personal ID
  int pid=Core.Rand.Next(65536);
  pid|=(Core.Rand.Next(65536)<<16);
  //egg.PersonalId=pid;
  //Inheriting form
  if (babyspecies == Pokemons.BURMY ||
     babyspecies == Pokemons.SHELLOS ||
     babyspecies == Pokemons.BASCULIN) {
    //egg.form=mother.Form;
    egg.SetForm(mother.FormId);
  }
  //Inheriting Moves
  HashSet<Moves> moves=new HashSet<Moves>();
  HashSet<Moves> othermoves=new HashSet<Moves>();
  Pokemon movefather=father;Pokemon movemother=mother;
  if (pbIsDitto(movefather) && !mother.IsFemale) {
    movefather=mother; movemother=father;
  }
  //Initial Moves
  Moves[] initialmoves=egg.getMoveList(); //Level|Moves
  //foreach (Moves k in initialmoves) { //Key: Level | Value: Move
  foreach (KeyValuePair<Moves,int> k in Game.PokemonMovesData[egg.Species].LevelUp) { 
    if (k.Value<=Core.EGGINITIALLEVEL) {
      moves.Add(k.Key);
    } else {
      if (mother.hasMove(k.Key) && father.hasMove(k.Key)) othermoves.Add(k.Key);
      //if (mother.hasMove(k) && father.hasMove(k)) othermoves.Add(k);
    }
  }
  //Inheriting Natural Moves
  foreach (var move in othermoves) {
    moves.Add(move);
  }
  //Inheriting Machine Moves
  if (!Core.USENEWBATTLEMECHANICS) {
    //Loop through every item in the game?...
    //for (int i = 0; i < ItemData.Length; i++) {
    for (int i = 0; i < egg.getMoveList(LearnMethod.machine).Length; i++) {
      //if (!$ItemData[i]) continue;
      //Moves atk=$ItemData[i][Core.ITEMMACHINE];
      Moves atk=egg.getMoveList(LearnMethod.machine)[i];
      //if (!atk || atk==0) continue;
      if (egg.isCompatibleWithMove(atk)) {
        if (movefather.hasMove(atk)) moves.Add(atk);
      }
    }
  }
//  Inheriting Egg Moves
  if (movefather.IsMale) {
    //pbRgssOpen("Data/eggEmerald.dat","rb"){|f|
    //   f.pos=(babyspecies-1)*8;
    //   int offset=f.fgetdw;
    //   int length=f.fgetdw;
    //   if (length>0) {
    //     f.pos=offset;
         int i=0; do { //unless (i<length) break; //loop
           Moves atk=egg.getMoveList(LearnMethod.egg)[i]; //f.fgetw();
           if (movefather.hasMove(atk)) moves.Add(atk);
           i+=1;
         } while (i>=egg.getMoveList(LearnMethod.egg).Length);
    //   }
    //}
  }
  if (Core.USENEWBATTLEMECHANICS) {
    //pbRgssOpen("Data/eggEmerald.dat","rb"){|f|
    //   f.pos=(babyspecies-1)*8;
    //   offset=f.fgetdw;
    //   length=f.fgetdw;
    //   if (length>0) {
    //     f.pos=offset;
         int i=0; do { //unless (i<length) break; //loop
           Moves atk=egg.getMoveList(LearnMethod.egg)[i]; //f.fgetw;
           if (movemother.hasMove(atk)) moves.Add(atk);
           i+=1;
         } while (i>=egg.getMoveList(LearnMethod.egg).Length);
    //   }
    //}
  }
//  Volt Tackle
  bool lightball=false;
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
  if (lightball && babyspecies == Pokemons.PICHU
     ) { //&& hasConst?(PBMoves,:VOLTTACKLE)
    moves.Add(Moves.VOLT_TACKLE);
  }
  //moves|=[]; // remove duplicates
  //Assembling move list
  Move[] finalmoves=new Move[4];
  int listend=moves.Count-4;
  if (listend<0) listend=0;
  int j=0;
  for (int i = listend; i < listend+3; i++) {
    Moves moveid=(i>=moves.Count) ? 0 : moves.ElementAt(i);
    finalmoves[j]=new Move(moveid);
    j+=1;
  }
//  Inheriting Individual Values
  int[] ivs=new int[6];
  for (int i = 0; i < 6; i++) {
    ivs[i]=Core.Rand.Next(32);
  }
  List<int?> ivinherit=new List<int?>(2);
  for (int i = 0; i < 2; i++) {
    Pokemon parent=new Pokemon[] {mother,father}[i];
    if (parent.Item == Items.POWER_WEIGHT)  ivinherit[i]=(int)Stats.HP;
    if (parent.Item == Items.POWER_BRACER)  ivinherit[i]=(int)Stats.ATTACK;
    if (parent.Item == Items.POWER_BELT)    ivinherit[i]=(int)Stats.DEFENSE;
    if (parent.Item == Items.POWER_ANKLET)  ivinherit[i]=(int)Stats.SPEED;
    if (parent.Item == Items.POWER_LENS)    ivinherit[i]=(int)Stats.SPATK;
    if (parent.Item == Items.POWER_BAND)    ivinherit[i]=(int)Stats.SPDEF;
  }
  int num=0;int r=Core.Rand.Next(2);
  for (int i = 0; i < 2; i++) {
    if (ivinherit[r]!=null) {
      Pokemon parent=new Pokemon[] { mother, father }[r];
      ivs[ivinherit[r].Value]=parent.IV[ivinherit[r].Value];
      num+=1;
      break;
    }
    r=(r+1)%2;
  }
  Stats[] stats=new Stats[] { Stats.HP,Stats.ATTACK,Stats.DEFENSE,
         Stats.SPEED,Stats.SPATK,Stats.SPDEF };
  int limit=(Core.USENEWBATTLEMECHANICS && (mother.Item == Items.DESTINY_KNOT ||
         father.Item == Items.DESTINY_KNOT)) ? 5 : 3;
  do { //;loop
    List<int> freestats=new List<int>();
    foreach (Stats i in stats) {
      if (!ivinherit.Contains((int)i)) freestats.Add((int)i);
    }
    if (freestats.Count==0) break;
    r=freestats[Core.Rand.Next(freestats.Count)];
    Pokemon parent=new Pokemon[] { mother, father }[Core.Rand.Next(2)];
    ivs[r]=parent.IV[r];
    ivinherit.Add(r);
    num+=1;
    if (num>=limit) break;
  } while (true);
//  Inheriting nature
  List<Natures> newnatures=new List<Natures>();
  if (mother.Item == Items.EVERSTONE) newnatures.Add(mother.Nature);
  if (father.Item == Items.EVERSTONE) newnatures.Add(father.Nature);
  if (newnatures.Count>0) {
    egg.setNature(newnatures[Core.Rand.Next(newnatures.Count)]);
  }
//  Masuda method and Shiny Charm
  int shinyretries=0;
  //if (father.language!=mother.language) shinyretries+=5;
  if (//hasConst?(PBItems,:SHINYCHARM) &&
    Game.GameData.Bag.pbQuantity(Items.SHINY_CHARM)>0) shinyretries+=2;
  if (shinyretries>0) {
    for (int i = 0; i < shinyretries; i++) {
      if (egg.IsShiny) break;
      //egg.PersonalId=Core.Rand.Next(65536)|(Core.Rand.Next(65536)<<16);
      egg.shuffleShiny();
    }
  }
//  Inheriting ability from the mother
  if ((!ditto0 && !ditto1)) {
    if (mother.hasHiddenAbility()) {
      if (Core.Rand.Next(10)<6) egg.setAbility(mother.abilityIndex);
    } else {
      if (Core.Rand.Next(10)<8) {
        egg.setAbility(mother.abilityIndex);
      } else {
        egg.setAbility((mother.abilityIndex+1)%2);
      }
    }
  } else if (((!ditto0 && ditto1) || (!ditto1 && ditto0)) && Core.USENEWBATTLEMECHANICS) {
    Pokemon parent=(!ditto0) ? mother : father;
    if (parent.hasHiddenAbility()) {
      if (Core.Rand.Next(10)<6) egg.setAbility(parent.abilityIndex);
    }
  }
//  Inheriting Poké Ball from the mother
  if (mother.IsFemale &&
     mother.ballUsed != Items.MASTER_BALL && //!pbBallTypeToBall(mother.ballUsed)
     mother.ballUsed != Items.CHERISH_BALL){ //!pbBallTypeToBall(mother.ballUsed)
    egg.ballUsed=mother.ballUsed;
  }
  egg.IV[0]=(byte)ivs[0];
  egg.IV[1]=(byte)ivs[1];
  egg.IV[2]=(byte)ivs[2];
  egg.IV[3]=(byte)ivs[3];
  egg.IV[4]=(byte)ivs[4];
  egg.IV[5]=(byte)ivs[5];
  egg.moves[0]=finalmoves[0];
  egg.moves[1]=finalmoves[1];
  egg.moves[2]=finalmoves[2];
  egg.moves[3]=finalmoves[3];
  //egg.calcStats();
  //egg.obtainText=Game._INTL("Day-Care Couple"); //ToDo: Set obtain for daycare
  //egg.Name=Game._INTL("Egg");
  //dexdata=pbOpenDexData();
  //pbDexDataOffset(dexdata,babyspecies,21);
  //int eggsteps=dexdata.fgetw;
  //dexdata.close();
  //egg.eggsteps=eggsteps;
  if (Core.Rand.Next(65536)<Core.POKERUSCHANCE) {
    egg.GivePokerus();
  }
  Game.GameData.Player.Party[Game.GameData.Player.Party.Length]=egg;
}

//Events.onStepTaken+=delegate(object sender, EventArgs e) {
public void OnStepTakenEventHandler(object sender, OnStepTakenFieldMovementEventArgs e) {
   if (Game.GameData.Player == null) return;
   int deposited=DayCare.pbDayCareDeposited();
   if (deposited==2 && !Game.GameData.Global.daycareEgg) {//==0
     if (Game.GameData.Global.daycareEggSteps == null) Game.GameData.Global.daycareEggSteps=0;
     Game.GameData.Global.daycareEggSteps+=1;
     if (Game.GameData.Global.daycareEggSteps==256) {
       Game.GameData.Global.daycareEggSteps=0;
       int compatval=new int[] {0,20,50,70}[DayCare.pbDayCareGetCompat()];
       if (Game.GameData.Bag.pbQuantity(Items.OVAL_CHARM)>0) { //hasConst?(PBItems,:OVALCHARM) &&
         compatval=new int[] {0,40,80,88}[DayCare.pbDayCareGetCompat()];
       }
       int rnd=Core.Rand.Next(100);
       if (rnd<compatval) {
         //  Egg is generated
         Game.GameData.Global.daycareEgg=true; //+=1; try adding one instead of setting to one?
       }
     }
   }
   for (int i = 0; i < 2; i++) {
     Pokemon pkmn=Game.GameData.Global.daycare.Slot[i].Key;
     if (!pkmn.IsNotNullOrNone()) return;
     int maxexp=Monster.Data.Experience.GetMaxExperience(pkmn.GrowthRate);
     if (pkmn.Exp<maxexp) {
       int oldlevel=pkmn.Level;
       pkmn.Exp+=1;
       if (pkmn.Level!=oldlevel) {
         pkmn.calcStats();
         //Moves[] movelist=pkmn.getMoveList();
         var movelist=Game.PokemonMovesData[pkmn.Species].LevelUp;
         foreach (KeyValuePair<Moves,int> j in movelist) {
           if (j.Value==pkmn.Level) pkmn.pbLearnMove(j.Key);	// Learned a new move
         }       
       }     
      }   
    }
  }
		#endregion

		#region Explicit Operators
		/*public static bool operator ==(DayCare x, DayCare y)
		{
			return ((x.Gender == y.Gender) && (x.TrainerID == y.TrainerID) && (x.SecretID == y.SecretID)) & (x.Name == y.Name);
		}
		public static bool operator !=(DayCare x, DayCare y)
		{
			return ((x.Gender != y.Gender) || (x.TrainerID != y.TrainerID) || (x.SecretID != y.SecretID)) | (x.Name == y.Name);
		}
		public bool Equals(DayCare obj)
		{
			if (obj == null) return false;
			return this == obj; 
		}
		public bool Equals(Character.Player obj)
		{
			if (obj == null) return false;
			return this == obj; 
		}
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() == typeof(Player))
				return Equals((Player)obj);
			if (obj.GetType() == typeof(TrainerId))
				return Equals((TrainerId)obj);
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return ((ulong)(TrainerID + SecretID * 65536)).GetHashCode();
		}
		bool IEquatable<DayCare>.Equals(DayCare other)
		{
			return Equals(obj: (object)other);
		}
		bool IEqualityComparer<DayCare>.Equals(DayCare x, DayCare y)
		{
			return x == y;
		}
		int IEqualityComparer<DayCare>.GetHashCode(DayCare obj)
		{
			return obj.GetHashCode();
		}*/
		#endregion
	}
}