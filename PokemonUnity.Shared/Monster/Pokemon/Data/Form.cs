using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;

namespace PokemonUnity.Monster.Data
{
	public struct Form 
	{
		public Forms Id { get; private set; }
		public string Identifier { get; private set; }
		public Pokemons Species { get; private set; }
		public bool IsMega { get; private set; }
		public bool IsBattleOnly { get; private set; }
		public bool IsDefault { get; private set; }
		public byte FormOrder { get; private set; }
		public int Order { get; private set; }

		public Form(Forms id, Pokemons species, string identifier = null, bool isMega = false, bool isBattleOnly = false, bool isDefault = false, byte formOrder = 0, int order = 0)
		{
			Id = id;
			Identifier = identifier;
			Species = species;
			IsMega = isMega;
			IsBattleOnly = isBattleOnly;
			IsDefault = isDefault;
			FormOrder = formOrder;
			Order = order;
		}
	}
}