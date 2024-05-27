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
	[Serializable]
	public struct MetaData //ToDo: Rename to MoveMetaData?
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
}