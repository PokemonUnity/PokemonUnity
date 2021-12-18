using System;
using System.Collections.Generic;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonUnity
{
	public partial class Game //: IGameMetadata
	{
		/// <summary>
		/// Opens the Pokémon screen
		/// </summary>
		public void pbPokemonScreen() {
			if (Trainer == null) return;
			IPartyDisplayScene sscene = Scenes.Party; //new PokemonScreen_Scene();
			IPartyDisplayScreen sscreen = Screens.Party.initialize(sscene, Trainer.party); //new PokemonScreen(sscene,GameData.Trainer.party);
			pbFadeOutIn(99999, block: () => { sscreen.pbPokemonScreen(); });
		}

		public void pbSaveScreen() {
			bool ret=false;
			ISaveScene scene = Scenes.Save; //new PokemonSaveScene();
			ISaveScreen screen = Screens.Save.initialize(scene); //new PokemonSave(scene);
			ret=screen.pbSaveScreen();
			return ret;
		}

		public void pbConvertItemToItem(int? variable, object[] array) {
			Items item=pbGet(variable);
			pbSet(variable,0);
			for (int i = 0; i < (array.Length/2); i++) {
				if (array[2*i] is Items) { //isConst(item,PBItems,array[2*i])
					pbSet(variable,array[2*i+1] as Items); //getID(PBItems,array[2*i+1])
					return;
				}
			}
		}
		
		public void pbConvertItemToPokemon(int? variable,object[] array) {
			Pokemons item=pbGet(variable);
			pbSet(variable,0);
			for (int i = 0; i < (array.Length/2); i++) {
				if (array[2*i] is Pokemons) { //isConst(item,PBItems,array[2*i])
					pbSet(variable,array[2*i+1]); //getID(PBSpecies,array[2*i+1])
					return;
				}
			}
		}

		public bool pbRecordTrainer() {
			IAudioObject wave = pbRecord(null,10);
			if (wave != null) {
				Global.trainerRecording=wave;
				return true;
			}
			return false;
		}
	}

	public partial class GlobalMetadata : PokemonEssentials.Interface.Field.IGlobalMetadata
	{
		public IAudioObject trainerRecording { get; set; }
		public bool bicycle					{ get; set; }
		public bool surfing					{ get; set; }
		public bool diving					{ get; set; }
		public bool sliding					{ get; set; }
		public bool fishing					{ get; set; }
		public bool runtoggle				{ get; set; }
		/// <summary>
		/// </summary>
		/// Should not stack (encourage users to deplete excessive money); 
		/// reset count based on repel used.
		///ToDo: Missing Variables for RepelType, Swarm
		public int repel				    { get; set; }
		public bool flashUsed				{ get; set; }
		public float bridge					{ get; set; }
		public bool runningShoes			{ get; set; }
		public bool snagMachine				{ get; set; }
		public bool seenStorageCreator		{ get; set; }
		public DateTime startTime			{ get; set; }
		/// <summary>
		/// Has player beaten the game already and viewed credits from start to end
		/// </summary>
		/// New Game Plus
		public bool creditsPlayed			                { get; set; }
		public int playerID									{ get; set; }
		public int coins				                    { get; set; }
		public int sootsack									{ get; set; }
		public IList<IMail> mailbox							{ get; set; }
		public IPCItemStorage pcItemStorage					{ get; set; }
		public int stepcount				                { get; set; }
		public int happinessSteps				            { get; set; }
		public int? pokerusTime								{ get; set; }
		public IDayCare daycare								{ get; set; }
		public bool daycareEgg								{ get; set; } //ToDo: int?...
		public int daycareEggSteps							{ get; set; }
		public bool[] pokedexUnlocked			            { get; set; } // Array storing which Dexes are unlocked
		public IList<int> pokedexViable						{ get; set; } // All Dexes of non-zero length and unlocked
		public int pokedexDex				                { get; set; } // Dex currently looking at (-1 is National Dex)
		public int[] pokedexIndex				            { get; set; } // Last species viewed per Dex
		public int pokedexMode								{ get; set; } // Search mode
		public int? healingSpot								{ get; set; }
		public float[] escapePoint							{ get; set; }
		public int pokecenterMapId							{ get; set; }
		public float pokecenterX				            { get; set; }
		public float pokecenterY				            { get; set; }
		public int pokecenterDirection						{ get; set; }
		public ITilePosition pokecenter						{ get; set; }
		public IList<int> visitedMaps				        { get; set; }
		public IList<int> mapTrail				            { get; set; }
		public IAudioBGM nextBattleBGM						{ get; set; }
		public IAudioME nextBattleME						{ get; set; }
		public IAudioObject nextBattleBack			        { get; set; }
		public ISafariState safariState						{ get; set; }
		public IBugContestState bugContestState				{ get; set; }
		public ITrainer partner								{ get; set; }
		public int? challenge				                { get; set; }
		public IBattleRecordData lastbattle					{ get; set; }
		public IList<IPhoneContact> phoneNumbers			{ get; set; }
		public int phoneTime				                { get; set; }
		public bool safesave				                { get; set; }
		public IDictionary<KeyValuePair<int,int>,int> eventvars				{ get; set; }

		//public float bridge { get {
		//  if (@bridge == null) @bridge=0;
		//  return @bridge;
		//} }

		public Pokemons[] roamPokemonCaught { get {
			if (_roamPokemonCaught == null) {
				_roamPokemonCaught= new List<Pokemons>();
			}
			return _roamPokemonCaught.ToArray();
		} }
		private IList<Pokemons> _roamPokemonCaught;

		public GlobalMetadata() {
			@bicycle              = false;
			@surfing              = false;
			@diving               = false;
			@sliding              = false;
			@fishing              = false;
			@runtoggle            = false;
			@repel                = 0;
			@flashUsed            = false;
			@bridge               = 0;
			@runningShoes         = false;
			@snagMachine          = false;
			@seenStorageCreator   = false;
			@startTime            = Game.GetTimeNow;
			@creditsPlayed        = false;
			@playerID             = -1;
			@coins                = 0;
			@sootsack             = 0;
			@mailbox              = null;
			@pcItemStorage        = null;
			@stepcount            = 0;
			@happinessSteps       = 0;
			@pokerusTime          = null;
			@daycare              = new Character.DayCare(2); //{ { null, 0 }, { null, 0 } };
			@daycareEgg           = false;//0;
			@daycareEggSteps      = 0;
			int numRegions        = 0;
			//pbRgssOpen("Data/regionals.dat","rb"){|f| numRegions = f.fgetw }
			@pokedexUnlocked      = new bool[numRegions];
			@pokedexViable        = new List<int>();
			@pokedexDex           = (numRegions==0) ? -1 : 0;
			@pokedexIndex         = new int[numRegions];
			@pokedexMode          = 0;
			for (int i = 0; i < numRegions+1; i++) {	// National Dex isn't a region, but is included
				@pokedexIndex[i]    = 0;
				@pokedexUnlocked[i] = (i==0);
			}
			@healingSpot          = null;
			@escapePoint          = new float[0];
			@pokecenterMapId      = -1;
			@pokecenterX          = -1;
			@pokecenterY          = -1;
			@pokecenterDirection  = -1;
			@visitedMaps          = new List<int>();
			@mapTrail             = new List<int>();
			@nextBattleBGM        = null;
			@nextBattleME         = null;
			@nextBattleBack       = null;
			@safariState          = null;
			@bugContestState      = null;
			@partner              = null;
			@challenge            = null;
			@lastbattle           = null;
			@phoneNumbers         = new List<int>();
			@phoneTime            = 0;
			@eventvars            = new Dictionary<KeyValuePair<int, int>, int>();
			@safesave             = false;
		}
	}
}