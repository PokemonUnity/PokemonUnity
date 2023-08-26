using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Attack;

namespace PokemonUnity.Attack.Data
{
	public struct MetaData
	{
		public Moves Move { get; private set; }
		public MetaCategory Category { get; private set; }
		public MoveMetaAilments Ailment { get; private set; }
		public int? MinHits { get; private set; }
		public int? MaxHits { get; private set; }
		public int? MinTurns { get; private set; }
		public int? MaxTurns { get; private set; }
		public int Drain { get; private set; }
		public int Healing { get; private set; }
		public int CritRate { get; private set; }
		public int AilmentChance { get; private set; }
		public int FlinchChance { get; private set; }
		public int StatChance { get; private set; }
		public MetaData(Moves move = Moves.NONE, MetaCategory category = MetaCategory.DAMAGE, MoveMetaAilments ailment = MoveMetaAilments.NONE, int? minHits = null, int? maxHits = null, int? minTurns = null, int? maxTurns = null, int drain = 0, int healing = 0, int critRate = 0, int ailmentChance = 0, int flinchChance = 0, int statChance = 0)
		{
			Move = move;
			Category = category;
			Ailment = ailment;
			MinHits = minHits;
			MaxHits = maxHits;
			MinTurns = minTurns;
			MaxTurns = maxTurns;
			Drain = drain;
			Healing = healing;
			CritRate = critRate;
			AilmentChance = ailmentChance;
			FlinchChance = flinchChance;
			StatChance = statChance;
		}
	}

	public enum Targets
	{
		/// <summary>
		/// One specific move.  How this move is chosen depends upon on the move being used.
		/// </summary>
		SPECIFIC_MOVE = 1,
		/// <summary>
		/// One other Pokémon on the field, selected by the trainer.  Stolen moves reuse the same target.
		/// </summary>
		SELECTED_POKEMON_ME_FIRST = 2,
		/// <summary>
		/// The user's ally (if any).
		/// </summary>
		ALLY = 3,
		/// <summary>
		/// The user's side of the field.  Affects the user and its ally (if any).
		/// </summary>
		USERS_FIELD = 4,
		/// <summary>
		/// Either the user or its ally, selected by the trainer.
		/// </summary>
		USER_OR_ALLY = 5,
		/// <summary>
		/// The opposing side of the field.  Affects opposing Pokémon.
		/// </summary>
		OPPONENTS_FIELD = 6,
		/// <summary>
		/// The user.
		/// </summary>
		USER = 7,
		/// <summary>
		/// One opposing Pokémon, selected at random.
		/// </summary>
		RANDOM_OPPONENT = 8,
		/// <summary>
		/// Every other Pokémon on the field.
		/// </summary>
		ALL_OTHER_POKEMON = 9,
		/// <summary>
		/// One other Pokémon on the field, selected by the trainer.
		/// </summary>
		SELECTED_POKEMON = 10,
		/// <summary>
		/// All opposing Pokémon.
		/// </summary>
		ALL_OPPONENTS = 11,
		/// <summary>
		/// The entire field.  Affects all Pokémon.
		/// </summary>
		ENTIRE_FIELD = 12,
		/// <summary>
		/// The user and its allies.
		/// </summary>
		USER_AND_ALLIES = 13,
		/// <summary>
		/// Every Pokémon on the field.
		/// </summary>
		ALL_POKEMON = 14,
	}
}