using System;
using System.Linq;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Inventory.Plants;
using PokemonUnity.Overworld;

namespace PokemonUnity.Monster
{
	public static class Evolution
	{
  public static string[] EVONAMES=new string[] {"Unknown",
     "Happiness","HappinessDay","HappinessNight","Level","Trade",
     "TradeItem","Item","AttackGreater","AtkDefEqual","DefenseGreater",
     "Silcoon","Cascoon","Ninjask","Shedinja","Beauty",
     "ItemMale","ItemFemale","DayHoldItem","NightHoldItem","HasMove",
     "HasInParty","LevelMale","LevelFemale","Location","TradeSpecies",
     "LevelDay","LevelNight","LevelDarkInParty","LevelRain","HappinessMoveType",
     "Custom1","Custom2","Custom3","Custom4","Custom5"
  };

//  0 = no parameter
//  1 = Positive integer
//  2 = Item internal name
//  3 = Move internal name
//  4 = Species internal name
//  5 = Type internal name
  public static int[] EVOPARAM=new int[] {0,    // Unknown (do not use)
     0,0,0,1,0,   // Happiness, HappinessDay, HappinessNight, Level, Trade
     2,2,1,1,1,   // TradeItem, Item, AttackGreater, AtkDefEqual, DefenseGreater
     1,1,1,1,1,   // Silcoon, Cascoon, Ninjask, Shedinja, Beauty
     2,2,2,2,3,   // ItemMale, ItemFemale, DayHoldItem, NightHoldItem, HasMove
     4,1,1,1,4,   // HasInParty, LevelMale, LevelFemale, Location, TradeSpecies
     1,1,1,1,5,   // LevelDay, LevelNight, LevelDarkInParty, LevelRain, HappinessMoveType
     1,1,1,1,1    // Custom 1-5
  };

        #region Evolution helper functions
//public static void pbGetEvolvedFormData(Pokemons species) {
//  List<> ret=new List<>();
//  int _EVOTYPEMASK=0x3F;
//  int _EVODATAMASK=0xC0;
//  int _EVONEXTFORM=0x00;
//  {  //pbRgssOpen("Data/evolutions.dat","rb"){|f|
//     //f.pos=(species-1)*8;
//     //offset=f.fgetdw();
//     int length=f.fgetdw();
//     if (length>0) {
//       //f.pos=offset;
//       int i=0; do { //unless (i<length) break; //loop
//         //Data.PokemonEvolution evo=Game.PokemonEvolutionsData[species][i]; //f.fgetb();
//         EvolutionMethod evonib=evo.EvolveMethod; //evo&_EVOTYPEMASK;
//         int level=(int)evo.EvolveValue; //f.fgetw();
//         Pokemons poke=evo.Species; //f.fgetw();
//         if ((evo&_EVODATAMASK)==_EVONEXTFORM) {
//           ret.Add([evonib,level,poke]);
//         }
//         i+=5;
//       } while (i >= length);
//     }
//  }
//  return ret;
//}

//Loops through each pokemon in db with evolution, 
//every 5 pokemons, log in debug output pokemon evolution
//public static void pbEvoDebug() {
//  int _EVOTYPEMASK=0x3F;
//  int _EVODATAMASK=0xC0;
//  {  //pbRgssOpen("Data/evolutions.dat","rb"){|f|
//     for (int species = 1; species < PBSpecies.maxValue; species++) {
//       //f.pos=(species-1)*8;
//       //offset=f.fgetdw;
//       int length=f.fgetdw;
//       GameDebug.Log(species.ToString(TextScripts.Name));
//       if (length>0) {
//         //f.pos=offset;
//         int i=0; do { //unless (i<length) break; //loop
//           //Data.PokemonEvolution evo=Game.PokemonEvolutionsData[species][i]; //f.fgetb;
//           EvolutionMethod evonib=evo.EvolveMethod; //evo&_EVOTYPEMASK;
//           int level=(int)evo.EvolveValue; //f.fgetw;
//           Pokemons poke=evo.Species; //f.fgetw;
//           GameDebug.Log(string.Format("type=%02X, data=%02X, name=%s, level=%d",
//              evonib,evo&_EVODATAMASK,poke.ToString(TextScripts.Name),level));
//           if (poke==0) {
//             //p f.eof?;
//             break;
//           }
//           i+=5;
//         } while (i >= length);
//       }
//     }
//  }
//}

public static Pokemons pbGetPreviousForm(Pokemons species) {
  //int _EVOTYPEMASK=0x3F;
  //int _EVODATAMASK=0xC0;
  //int _EVOPREVFORM=0x40;
  { //pbRgssOpen("Data/evolutions.dat","rb"){|f|
     //f.pos=(species-1)*8;
     //offset=f.fgetdw();
     int length=Game.PokemonData[species].EvoChainId; //f.fgetdw();
     if (length>0) {
       //f.pos=offset;
       int i=0; do { //unless (i<length) break; //loop
         Data.PokemonEvolution evo=Game.PokemonEvolutionsData[species][i]; //f.fgetb;
         EvolutionMethod evonib=evo.EvolveMethod; //evo&_EVOTYPEMASK;
         int level=(int)evo.EvolveValue; //f.fgetw;
         Pokemons poke=evo.Species; //f.fgetw;
         //if ((evo&_EVODATAMASK)==_EVOPREVFORM) {
         if (Game.PokemonData[poke].EvolveFrom != Pokemons.NONE) {
           return poke;
         }
         i++;//+=5;
       } while (i >= length);
     }
  }
  return species;
}

public static int pbGetMinimumLevel(Pokemons species) {
  int ret=-1;
  //int _EVOTYPEMASK=0x3F;
  //int _EVODATAMASK=0xC0;
  //int _EVOPREVFORM=0x40;
  { //pbRgssOpen("Data/evolutions.dat","rb"){|f|
    //f.pos=(species-1)*8;
    //offset=f.fgetdw();
    int length=Game.PokemonEvolutionsData[species].Length; //f.fgetdw();
    if (length>0) {
      //f.pos=offset;
      int i=0; do { //unless (i<length) break; //loop
        Data.PokemonEvolution evo=Game.PokemonEvolutionsData[species][i]; //f.fgetb();
        EvolutionMethod evonib=evo.EvolveMethod; //evo&_EVOTYPEMASK;
        int level=(int)evo.EvolveValue; //f.fgetw();
        //Pokemons poke=evo.Species; //f.fgetw();
        if (//poke<=Game.PokemonData.Count && //PBSpecies.maxValue
           //(evo&_EVODATAMASK)==_EVOPREVFORM && // evolved from
           new EvolutionMethod[] {EvolutionMethod.Level,EvolutionMethod.LevelMale,
           EvolutionMethod.LevelFemale,EvolutionMethod.AttackGreater,
           EvolutionMethod.AtkDefEqual,EvolutionMethod.DefenseGreater,
           EvolutionMethod.Silcoon,EvolutionMethod.Cascoon,
           EvolutionMethod.Ninjask,EvolutionMethod.Shedinja,
           EvolutionMethod.LevelDay,EvolutionMethod.LevelNight,
           EvolutionMethod.LevelRain}.Contains(evonib)) { //EvolutionMethod.LevelDarkInParty,
          ret=(ret==-1) ? level : (int)System.Math.Min(ret,level);
          break;
        }
        i++;//+=5;
      } while (i >= length);
    }
  }
  return (ret==-1) ? 1 : ret;
}

public static Pokemons pbGetBabySpecies(Pokemons species,Items item1=Items.NONE,Items item2=Items.NONE) {
  Pokemons ret=species;
  //int _EVOTYPEMASK=0x3F;
  //int _EVODATAMASK=0xC0;
  //int _EVOPREVFORM=0x40;
  { //pbRgssOpen("Data/evolutions.dat","rb"){ |f|
     //f.pos=(species-1)*8;
     //offset=f.fgetdw();
     int length=Game.PokemonEvolutionsData[species].Length; //f.fgetdw();
     if (length>0) {
       //f.pos=offset();
       int i=0; do { //unless (i<length) break; //loop
         Data.PokemonEvolution evo=Game.PokemonEvolutionsData[species][i]; //f.fgetb();
         EvolutionMethod evonib=evo.EvolveMethod; //evo&_EVOTYPEMASK;
         int level=(int)evo.EvolveValue; //f.fgetw();
         Pokemons poke=evo.Species; //f.fgetw();
         //if (poke<=PBSpecies.maxValue && (evo&_EVODATAMASK)==_EVOPREVFORM) {		// evolved from
         if (//poke<=Game.PokemonData.Keys.Count && 
             Game.PokemonData[poke].IsBaby) {		// evolved from
           if (item1>=0 && item2>=0) {
             //dexdata=pbOpenDexData();
             //pbDexDataOffset(dexdata,poke,54);
             Items incense=Game.PokemonData[poke].Incense; //dexdata.fgetw();
             //dexdata.close();
             if (item1==incense || item2==incense) ret=poke;
           } else {
             ret=poke;
           }
           break;
         }
         i++;//+=5;
       } while (i >= length);
     }
  }
  if (ret!=species) {
    ret=pbGetBabySpecies(ret);
  }
  return ret;
}
        #endregion

        #region Evolution methods
public static Pokemons pbMiniCheckEvolution(Pokemon pokemon,EvolutionMethod evonib,int level,Pokemons poke) {
  switch (evonib) {
  case EvolutionMethod.Happiness:
    if (pokemon.Happiness>=220) return poke;
    break;
  case EvolutionMethod.HappinessDay:
    if (pokemon.Happiness>=220 && Game.IsDay) return poke;
    break;
  case EvolutionMethod.HappinessNight:
    if (pokemon.Happiness>=220 && Game.IsNight) return poke;
    break;
  //case EvolutionMethod.HappinessMoveType:
  //  if (pokemon.Happiness>=220) {
  //    for (int i = 0; i < 4; i++) {
  //      if (pokemon.moves[i].MoveId>0 && pokemon.moves[i].Type==level) return poke;
  //    }
  //  }
  //  break;
  case EvolutionMethod.Level:
    if (pokemon.Level>=level) return poke;
    break;
  case EvolutionMethod.LevelDay:
    if (pokemon.Level>=level && Game.IsDay) return poke;
    break;
  case EvolutionMethod.LevelNight:
    if (pokemon.Level>=level && Game.IsNight) return poke;
    break;
  case EvolutionMethod.LevelMale:
    if (pokemon.Level>=level && pokemon.IsMale) return poke;
    break;
  case EvolutionMethod.LevelFemale:
    if (pokemon.Level>=level && pokemon.IsFemale) return poke;
    break;
  case EvolutionMethod.AttackGreater: // Hitmonlee
    pokemon.setItem(Items.NONE); //Null item to prevent from maniulating stats
    if (pokemon.Level>=level && pokemon.ATK>pokemon.DEF) return poke;
    break;
  case EvolutionMethod.AtkDefEqual: // Hitmontop
    pokemon.setItem(Items.NONE); //Null item to prevent from maniulating stats
    if (pokemon.Level>=level && pokemon.ATK==pokemon.DEF) return poke;
    break;
  case EvolutionMethod.DefenseGreater: // Hitmonchan
    pokemon.setItem(Items.NONE); //Null item to prevent from maniulating stats
    if (pokemon.Level>=level && pokemon.ATK<pokemon.DEF) return poke;
    break;
  case EvolutionMethod.Silcoon:
    if (pokemon.Level>=level && (((pokemon.PersonalId>>16)&0xFFFF)%10)<5) return poke;
    break;
  case EvolutionMethod.Cascoon:
    if (pokemon.Level>=level && (((pokemon.PersonalId>>16)&0xFFFF)%10)>=5) return poke;
    break;
  case EvolutionMethod.Ninjask:
    if (pokemon.Level>=level) return poke;
    break;
  case EvolutionMethod.Shedinja:
    return Pokemons.NONE; //-1;
    break;
  case EvolutionMethod.HoldItemDay: //.DayHoldItem:
    if (pokemon.Item==(Items)level && Game.IsDay) return poke;
    break;
  case EvolutionMethod.HoldItemNight: //.NightHoldItem:
    if (pokemon.Item==(Items)level && Game.IsNight) return poke;
    break;
  case EvolutionMethod.Move: //.HasMove:
    for (int i = 0; i < 4; i++) {
      if (pokemon.moves[i].MoveId==(Moves)level) return poke;
    }
    break;
  case EvolutionMethod.Party: //.HasInParty:
    foreach (var i in Game.GameData.Trainer.party) {
      if (!i.isEgg && i.Species==(Pokemons)level) return poke;
    }
    break;
  //case EvolutionMethod.LevelDarkInParty:
  //  if (pokemon.Level>=level) {
  //    foreach (Pokemon i in Game.GameData.Trainer.Party) {
  //      if (!i.isEgg && i.hasType(Types.DARK)) return poke;
  //    }
  //  }
  //  break;
  case EvolutionMethod.Location:
    if (Game.GameData.GameMap.map_id==level) return poke;
    break;
  case EvolutionMethod.LevelRain:
    if (pokemon.Level>=level) {
      if (Game.GameData.GameScreen != null && (Game.GameData.GameScreen.weather_type == FieldWeathers.Rain ||
                          Game.GameData.GameScreen.weather_type == FieldWeathers.HeavyRain ||
                          Game.GameData.GameScreen.weather_type == FieldWeathers.Thunderstorm)) {
        return poke;
      }
    }
    break;
  case EvolutionMethod.Beauty: // Feebas
    if (pokemon.Beauty>=level) return poke;
    break;
  case EvolutionMethod.Trade: case EvolutionMethod.TradeItem: case EvolutionMethod.TradeSpecies:
    return Pokemons.NONE; //-1;
    break;
  //case EvolutionMethod.Custom1:
  //  //  Add code for custom evolution type 1
  //  break;
  //case EvolutionMethod.Custom2:
  //  //  Add code for custom evolution type 2
  //  break;
  //case EvolutionMethod.Custom3:
  //  //  Add code for custom evolution type 3
  //  break;
  //case EvolutionMethod.Custom4:
  //  //  Add code for custom evolution type 4
  //  break;
  //case EvolutionMethod.Custom5:
  //  //  Add code for custom evolution type 5
  //  break;
  }
  return Pokemons.NONE; //-1;
}

public static Pokemons pbMiniCheckEvolutionItem(Pokemon pokemon,EvolutionMethod evonib,Items level,Pokemons poke,Items item) {
//  Checks for when an item is used on the Pokémon (e.g. an evolution stone)
  switch (evonib) {
  case EvolutionMethod.Item:
    if (level==item) return poke;
    break;
  case EvolutionMethod.ItemMale:
    if (level==item && pokemon.IsMale) return poke;
    break;
  case EvolutionMethod.ItemFemale:
    if (level==item && pokemon.IsFemale) return poke;
    break;
  }
  return Pokemons.NONE; //-1;
}

/// <summary>
/// Checks whether a Pokemon can evolve now. If a block is given, calls the block
/// with the following parameters:<para></para>
/// Pokemon to check; evolution type; level or other parameter; ID of the new Pokemon species
/// </summary>
/// <param name="pokemon"></param>
public static Pokemons pbCheckEvolutionEx(Pokemon pokemon) {
  if (pokemon.Species<=0 || pokemon.isEgg) return 0;
  if (pokemon.Species == Pokemons.PICHU && pokemon.form==1) return 0;
  if (pokemon.Item == Items.EVERSTONE &&
               pokemon.Species != Pokemons.KADABRA) return 0;
  Pokemons ret=0;
  //foreach (var form in pbGetEvolvedFormData(pokemon.Species)) {
  foreach (Data.PokemonEvolution form in Game.PokemonEvolutionsData[pokemon.Species]) {
    //ret=yield pokemon,form[0],form[1],form[2]; //EvolveMethod evonib,int level,poke
    ret=form.Species;
    if (ret>0) break;
  }
  return ret;
}

/// <summary>
/// Checks whether a Pokemon can evolve now. If an item is used on the Pokémon,
/// checks whether the Pokemon can evolve with the given item.
/// </summary>
/// <param name="pokemon"></param>
/// <param name="item"></param>
public static Pokemons[] pbCheckEvolution(Pokemon pokemon,Items item=0) {
  if (pokemon.Species<=0 || pokemon.isEgg) return new Pokemons[0];
  if (pokemon.Species == Pokemons.PICHU && pokemon.form==1) return new Pokemons[0];
  if (pokemon.Item == Items.EVERSTONE &&
               pokemon.Species != Pokemons.KADABRA) return new Pokemons[0];
  if (item==0) {
    //return pbCheckEvolutionEx(pokemon) { //|pokemon,evonib,level,poke|
    //   next pbMiniCheckEvolution(pokemon,evonib,level,poke);
    //}
    return Game.PokemonEvolutionsData[pokemon.Species].Where(a =>
       pbMiniCheckEvolution(pokemon,a.EvolveMethod,(int)a.EvolveValue,a.Species) != Pokemons.NONE
    ).Select(b => b.Species).ToArray();
  } else {
    //return pbCheckEvolutionEx(pokemon) { //|pokemon,evonib,level,poke|
    //   next pbMiniCheckEvolutionItem(pokemon,evonib,level,poke,item);
    //}
    return Game.PokemonEvolutionsData[pokemon.Species].Where(a =>
       pbMiniCheckEvolutionItem(pokemon,a.EvolveMethod,(Items)a.EvolveValue,a.Species, item) != Pokemons.NONE
    ).Select(b => b.Species).ToArray();
  }
}
        #endregion
	}
}