using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.UX;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Field
{
	/// <summary>
	/// Extension on <see cref="ITempMetadata"/>
	/// </summary>
	public interface ITempMetadataRoaming {
		int nowRoaming				{ get; }
		int roamerIndex				{ get; }
	}

	/// <summary>
	/// Extension on <see cref="IGlobalMetadata"/>
	/// </summary>
	public interface IGlobalMetadataRoaming {
		int[] roamPosition			{ get; }
		int roamHistory				{ get; }
		int roamedAlready			{ get; }
		int roamEncounter			{ get; }
		bool?[] roamPokemon			{ get; }
		bool[] roamPokemonCaught	{ get; }

		/// <summary>
		/// Resets all roaming Pokemon that were defeated without having been caught.
		/// </summary>
		void pbResetAllRoamers();

		/// <summary>
		/// Gets the roaming areas for a particular Pokémon.
		/// </summary>
		int[] pbRoamingAreas(int index);

		/// <summary>
		/// Puts a roamer in a completely random map available to it.
		/// </summary>
		/// <param name="index"></param>
		void pbRandomRoam(int index);

		/// <summary>
		/// Roams all roamers, if their Switch is on.
		/// </summary>
		/// <param name="ignoretrail"></param>
		void pbRoamPokemon(bool ignoretrail = false);

		//Events.OnMapChange+=proc {|sender,e|

		//Events.OnWildBattleOverride+=proc { |sender,e|

		PokemonUnity.Combat.BattleResults pbRoamingPokemonBattle(Pokemons species, int level);

		//EncounterModifier.register(proc {|encounter|});

		//EncounterModifier.registerEncounterEnd(proc {});
		//EncounterModifier.registerEncounterEnd(Action);

		/// <summary>
		/// </summary>
		/// <param name="enctype">
		/// None		= 0,
		/// Any encounter method (except triggered ones and Bug Contest);
		/// Walking		= 1,
		/// Grass (except Bug Contest)/walking in caves only;
		/// Surfing		= 2,
		/// Surfing only;
		/// Fishing		= 3,
		/// Fishing only;
		/// AnyWater	= 4
		/// Water-based only
		/// </param>
		/// <returns></returns>
		bool pbRoamingMethodAllowed(Method enctype);
	}
}