using System.Collections.Generic;
using System.Linq;

namespace PokemonUnity.Character
{
	/// <summary>
	/// This class handles all actions regarding Badge loading and displaying.
	/// </summary
	public struct BadgeData
	{
		/// <summary>
		/// Id of Trainer Badge
		/// </summary>
		public GymBadges Id { get; private set; } 
		//public string Name { get; private set; }
		/// <summary>
		/// Pokemons with level under this int, become less disobedient
		/// </summary>
		public byte LevelCap { get; private set; }
		//public Moves[] HMs { get; private set; } 
		//public HiddenMoves[] HMs { get; private set; } 
		//public string TexturePath { get; private set; } //= @"GUI\Badges";
		public Regions Region { get; private set; } //= "Johto";

		public BadgeData(GymBadges id, Regions region, byte levelCap = 0)
		{
			Id = id;
			Region = region;
			LevelCap = levelCap;
		}
	}

	public struct HiddenMoveData
	{
		public HiddenMoves Id { get; private set; }
		/// <summary>
		/// Number of badges needed in order to use this HM outside of battle
		/// <see cref="Core.HIDDENMOVESCOUNTBADGES"/>
		/// </summary>
		public byte Badges { get; private set; }
		/// <summary>
		/// Gym badge needed in order to use this HM outside of battle
		/// <see cref="Core.HIDDENMOVESCOUNTBADGES"/>
		/// </summary>
		public GymBadges Badge { get; private set; }
		/// <summary>
		/// Move associated with this particular HM
		/// </summary>
		public Moves Move { get; private set; }

		public HiddenMoveData(HiddenMoves id, Moves move, GymBadges badge, byte badges)
		{
			Id = id;
			Move = move;
			Badge = badge;
			Badges = badges;
		}
	}

	//ToDo: public static Moves[] HMs
	//ToDo: Move HMs to Machine class... Match TMs to MoveId?
	public enum HiddenMoves
	{
		Surf,
		Cut,
		Strength,
		Flash,
		Fly,
		Whirlpool,
		Waterfall,
		Ride,
		Dive,
		RockClimb,
		RockSmash,
		Headbutt
	}
}