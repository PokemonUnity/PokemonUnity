using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.UX;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.Field;

namespace PokemonUnity
{
	/// <summary>
	/// Temporary data which is not saved and which is erased when a game restarts.
	/// </summary>
	public partial class PokemonTemp : PokemonEssentials.Interface.Field.ITempMetadata //PokemonEssentials.Interface.ITempMetadata,
	{
		public int menuLastChoice { get; set; }
		public int keyItemCalling { get; set; }
		public int hiddenMoveEventCalling { get; set; }
		public bool begunNewGame { get; set; }
		public int miniupdate { get; set; }
		public int waitingTrainer { get; set; }
		public int darknessSprite { get; set; }
		public IList<string> pokemonDexData { get; }
		public IDictionary<int, IPokemonMetadata> pokemonMetadata { get; set; }
		public IList<int> pokemonPhoneData { get; }
		public int lastbattle { get; set; }
		public int flydata { get; set; }

		public PokemonTemp() { initialize(); }
		public PokemonEssentials.Interface.Field.ITempMetadata initialize()
        {
			return this;
        }
	}
}