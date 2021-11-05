using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Saving;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Utility;
using PokemonUnity.Monster;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface
{
	namespace EventArg
	{
		#region Multiple Forms EventArgs
		public class MultipleFormsEventArgs : EventArgs
		{
			public readonly int EventId = typeof(UseFromBagEventArgs).GetHashCode();

			public int Id { get; }
			public Items Item { get; set; }
			public ItemUseResults Response { get; set; }
		}
		#endregion
	}

	namespace PokeBattle
	{
	    /// <summary>
	    /// Extensions of <seealso cref="IPokemon"/>
	    /// </summary>
	    public interface IPokemonMultipleForms
	    {
            int formTime				{ get; }   // Time when Furfrou's/Hoopa's form was set
            int form                  { get; set; }


            //void formNoCall(int value);
            int formNoCall { set; }

            // Used by the Pokédex only
            void forceForm(int value);

            //alias __mf_baseStats baseStats;
            //alias __mf_ability ability;
            //alias __mf_getAbilityList getAbilityList;
            //alias __mf_type1 type1;
            //alias __mf_type2 type2;
            //alias __mf_height height;
            //alias __mf_weight weight;
            //alias __mf_getMoveList getMoveList;
            //alias __mf_isCompatibleWithMove? isCompatibleWithMove?;
            //alias __mf_wildHoldItems wildHoldItems;
            //alias __mf_baseExp baseExp;
            //alias __mf_evYield evYield;
            //alias __mf_kind kind;
            //alias __mf_dexEntry dexEntry;
            //alias __mf_initialize initialize;

            int baseStats { get; }

            [System.Obsolete("DEPRECATED - do not use")]
            int ability { get; }
        
            int getAbilityList { get; }
        
            int type1 { get; }
        
            int type2 { get; }
        
            int height { get; }
        
            int weight { get; }
        
            int getMoveList { get; }

            bool isCompatibleWithMove(Moves move);

            int wildHoldItems { get; }

            int baseExp { get; }

            int evYield { get; }

            int kind { get; }

            int dexEntry { get; }

            //IPokemon initialize(*args);
      }

        public interface IPokeBattle_RealBattlePeer {
	        void pbOnEnteringBattle(IBattle battle, IPokemon pokemon);
        }

        //public interface IMultipleForms {
        //  @@formSpecies=new HandlerHash(:PBSpecies);

        //  public static void copy(sym,*syms) {
        //    @@formSpecies.copy(sym,*syms);
        //  }

        //  public static void register(sym,hash) {
        //    @@formSpecies.add(sym,hash);
        //  }

        //  public static void registerIf(cond,hash) {
        //    @@formSpecies.addIf(cond,hash);
        //  }

        //  public static void hasFunction(pokemon,func) {
        //    spec=(pokemon is Numeric) ? pokemon : pokemon.Species;
        //    sp=@@formSpecies[spec];
        //    return sp && sp[func];
        //  }

        //  public static void getFunction(pokemon,func) {
        //    spec=(pokemon is Numeric) ? pokemon : pokemon.Species;
        //    sp=@@formSpecies[spec];
        //    return (sp && sp[func]) ? sp[func] : null;
        //  }

        //  public static void call(func,pokemon,*args) {
        //    sp=@@formSpecies[pokemon.Species];
        //    if (!sp || !sp[func]) return null;
        //    return sp[func].call(pokemon,*args);
        //  }
        //}

        public interface IGamePkemonMultipleForms
		{
            void drawSpot(IBitmap bitmap,int[][] spotpattern, int x, int y, sbyte red, sbyte green, sbyte blue);

            int[][] pbSpindaSpots(IPokemon pokemon, IBitmap bitmap);
		}
	}
}