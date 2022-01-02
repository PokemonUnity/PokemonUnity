using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Saving;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Utility;

namespace PokemonEssentials.Interface
{
	public interface IMoveData
	{
		//int function { get; set; }
		PokemonUnity.Attack.Data.Effects Effect { get; }
		int basedamage { get; set; }
		Types type { get; set; }
		int accuracy { get; set; }
		int totalpp { get; set; }
		int addlEffect { get; set; }
		PokemonUnity.Attack.Data.Targets target { get; set; }
		int priority { get; set; }
		PokemonUnity.Attack.Data.Flag flags { get; set; }
		PokemonUnity.Attack.Category category { get; set; }

		//IMoveData initializeOld(Moves moveid);
		IMoveData initialize(Moves moveid);
	}

	public interface IMove
	{
		PokemonUnity.Attack.Data.Effects Effect { get; }
		/// <summary>
		/// This move's ID
		/// </summary>
		Moves id { get; }
		/// <summary>
		/// The amount of PP remaining for this move
		/// </summary>
		int PP { get; set; }
		/// <summary>
		/// The number of PP Ups used for this move
		/// </summary>
		int PPups { get; set; }

		/// <summary>
		/// Gets this move's type.
		/// </summary>
		Types Type { get; }

		/// <summary>
		/// Gets the maximum PP for this move.
		/// </summary>
		int TotalPP { get; }

		/// <summary>
		/// Initializes this object to the specified move ID.
		/// </summary>
		IMove initialize(Moves moveid = Moves.NONE);
	}
}