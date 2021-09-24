using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.UX;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Field
{
	public interface IGlobalMetadata
	{
		bool bicycle { get; set; }
		bool surfing { get; set; }
		bool diving { get; set; }
		bool sliding { get; set; }
		bool fishing { get; set; }
		bool runtoggle { get; set; }
		/// <summary>
		/// </summary>
		/// Should not stack (encourage users to deplete excessive money); 
		/// reset count based on repel used.
		///ToDo: Missing Variables for RepelType, Swarm
		int repel { get; set; }
		bool flashUsed { get; set; }
		float bridge { get; set; }
		bool runningShoes { get; set; }
		bool snagMachine { get; set; }
		bool seenStorageCreator { get; set; }
		DateTime startTime { get; set; }
		/// <summary>
		/// Has player beaten the game already and viewed credits from start to end
		/// </summary>
		/// New Game Plus
		bool creditsPlayed { get; set; }
		int playerID { get; set; }
		int coins { get; set; }
		int sootsack { get; set; }
		int? mailbox { get; set; }
		//IPCItemStorage pcItemStorage	{ get; set; }
		int stepcount { get; set; }
		int happinessSteps { get; set; }
		int? pokerusTime { get; set; }
		PokemonUnity.Character.DayCare daycare { get; set; }
		bool daycareEgg { get; set; } //ToDo: int?...
		int daycareEggSteps { get; set; }
		bool[] pokedexUnlocked { get; set; } // Array storing which Dexes are unlocked
		List<int> pokedexViable { get; set; } // All Dexes of non-zero length and unlocked
		int pokedexDex { get; set; } // Dex currently looking at (-1 is National Dex)
		int[] pokedexIndex { get; set; } // Last species viewed per Dex
		int pokedexMode { get; set; } // Search mode
		int? healingSpot { get; set; }
		float[] escapePoint { get; set; }
		int pokecenterMapId { get; set; }
		float pokecenterX { get; set; }
		float pokecenterY { get; set; }
		int pokecenterDirection { get; set; }
		ITilePosition pokecenter			{ get; set; }
		List<int> visitedMaps { get; set; }
		List<int> mapTrail { get; set; }
		IAudioBGM nextBattleBGM { get; set; }
		IAudioME nextBattleME { get; set; }
		IAudioObject nextBattleBack { get; set; }
		ISafariState safariState { get; set; }
		//IBugContestState bugContestState			{ get; set; }
		ITrainer partner { get; set; }
		int? challenge { get; set; }
		int? lastbattle { get; set; }
		List<int> phoneNumbers { get; set; }
		int phoneTime { get; set; }
		bool safesave { get; set; }
		Dictionary<KeyValuePair<int, int>, int> eventvars { get; set; }


		//float bridge { get; }
		Pokemons[] roamPokemonCaught { get; }
	}
}