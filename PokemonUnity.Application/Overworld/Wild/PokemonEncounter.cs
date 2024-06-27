using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Utility;
using PokemonUnity.Interface;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonUnity.Overworld
{
	//public enum EncounterTypes {
	//	Land         = 0 ,
	//	Cave         = 1 ,
	//	Water        = 2 ,
	//	RockSmash    = 3 ,
	//	OldRod       = 4 ,
	//	GoodRod      = 5 ,
	//	SuperRod     = 6 ,
	//	HeadbuttLow  = 7 ,
	//	HeadbuttHigh = 8 ,
	//	LandMorning  = 9 ,
	//	LandDay      = 10,
	//	LandNight    = 11,
	//	BugContest   = 12
	//}
	public partial class PokemonEncounter : PokemonEssentials.Interface.Field.IEncounters
	{
		/// <summary>
		/// Chances of pokemon encounter equal up to 100%
		/// </summary>
		/// Technically all land encounters share the same odds
		//public int[][] EnctypeChances { get { return new int[][] {
		//	new int[] { 20,20,10,10,10,10,5,5,4,4,1,1 }, //Land
		//	new int[] { 20,20,10,10,10,10,5,5,4,4,1,1 }, //Cave
		//	new int[] { 60,30,5,4,1 }, //Surf
		//	new int[] { 60,30,5,4,1 }, //RockSmash
		//	new int[] { 70,30 }, //OldRod
		//	new int[] { 60,20,20 }, //GoodRod
		//	new int[] { 40,40,15,4,1 }, //SuperRod
		//	new int[] { 30,25,20,10,5,5,4,1 }, //HeadbuttLow
		//	new int[] { 30,25,20,10,5,5,4,1 }, //HeadbuttHigh
		//	new int[] { 20,20,10,10,10,10,5,5,4,4,1,1 }, //LandMorning
		//	new int[] { 20,20,10,10,10,10,5,5,4,4,1,1 }, //LandDay
		//	new int[] { 20,20,10,10,10,10,5,5,4,4,1,1 }, //LandNight
		//	new int[] { 20,20,10,10,10,10,5,5,4,4,1,1 } //BugContest
		//}; } }
		public IDictionary<EncounterOptions, int[]> EnctypeChances
		{
			get
			{
				return new Dictionary<EncounterOptions, int[]>()
				{
					{ EncounterOptions.Land, Walk },
					{ EncounterOptions.Cave, DarkSpots },
					{ EncounterOptions.Water, Surf },
					{ EncounterOptions.RockSmash, Hidden },
					//{ EncounterOptions.OldRod, OldRod },
					//{ EncounterOptions.GoodRod, GoodRod },
					//{ EncounterOptions.SuperRod, SuperRod },
					{ EncounterOptions.HeadbuttLow, Hidden },
					{ EncounterOptions.HeadbuttHigh, Hidden },
					{ EncounterOptions.LandMorning, Walk },
					{ EncounterOptions.LandDay, Walk },
					{ EncounterOptions.LandNight, Walk },
					//{ EncounterOptions.BugContest, BugContest }
				};
			}
		}
		/// <summary>
		/// Density is rate encounter;
		/// How often player is to encounter a pokemon
		/// </summary>
		//public int[] EnctypeDensities { get { return new int[] {25,10,10,0,0,0,0,0,0,25,25,25,25}; } }
		public IDictionary<EncounterOptions, int> EnctypeDensities { get; }
		//public int[] EnctypeCompileDens { get { return new int[] {1,2,3,0,0,0,0,0,0,1,1,1,1}; } }
		public IDictionary<EncounterOptions,IList<IEncounterPokemon>> EnctypeEncounters { get { return enctypes; } }
		private IDictionary<EncounterOptions,IList<IEncounterPokemon>> enctypes;
		//private NestedDictionary<EncounterOptions, NestedDictionary<Pokemons,int[]>> enctypes;
		private int[] density;

		public PokemonEncounter() {
			//@enctypes=new EncounterTypes[0]; //[EncounterType][Pokemons,Min,Max]
			//@enctypes=new NestedDictionary<EncounterOptions, NestedDictionary<Pokemons, int[]>>(); //[EncounterType][Pokemons][Min,Max]
			@enctypes=new Dictionary<EncounterOptions,IList<IEncounterPokemon>>(); //[EncounterType][Pokemons,Min,Max]
			@density=null;
		}

		public int stepcount { get; private set; }
		//public int stepcount{ get {
		//  return @stepcount;
		//} }

		public void clearStepCount() {
			@stepcount=0;
		}

		public bool hasEncounter (EncounterOptions enc) {
			if (@density==null || enc<0) return false;
			//return @enctypes[enc] != null ? true : false;
			return @enctypes.Keys.Contains(enc);
		}

		public bool IsCave { get {
			if (@density==null) return false;
				//return @enctypes[EncounterTypes.Cave] != null ? true : false;
				return @enctypes.Keys.Contains(EncounterOptions.Cave);
		} }

		//public bool IsFlower { get {
		//	if (@density==null) return false;
		//	return (@enctypes.Keys.Contains(Method.RED_FLOWERS) ||
		//			@enctypes.Keys.Contains(Method.ORANGE_FLOWERS) ||
		//			@enctypes.Keys.Contains(Method.YELLOW_FLOWERS) ||
		//			@enctypes.Keys.Contains(Method.BLUE_FLOWERS) ||
		//			@enctypes.Keys.Contains(Method.WHITE_FLOWERS) ||
		//			@enctypes.Keys.Contains(Method.PINK_FLOWERS) ||
		//			@enctypes.Keys.Contains(Method.PURPLE_FLOWERS));
		//} }

		public bool IsGrass { get {
			if (@density==null) return false;
			return (@enctypes.Keys.Contains(EncounterOptions.Land) ||
					@enctypes.Keys.Contains(EncounterOptions.LandMorning) ||
					@enctypes.Keys.Contains(EncounterOptions.LandDay) ||
					@enctypes.Keys.Contains(EncounterOptions.LandNight) ||
					@enctypes.Keys.Contains(EncounterOptions.BugContest));
		} }

		public bool IsRegularGrass { get {
			if (@density==null) return false;
			return (@enctypes.Keys.Contains(EncounterOptions.Land) ||
					@enctypes.Keys.Contains(EncounterOptions.LandMorning) ||
					@enctypes.Keys.Contains(EncounterOptions.LandDay) ||
					@enctypes.Keys.Contains(EncounterOptions.LandNight));
		} }

		public bool IsWater { get {
			if (@density==null) return false;
			//return @enctypes[EncounterTypes.Water] != null ? true : false;
			return @enctypes.Keys.Contains(EncounterOptions.Water);
		} }

		public EncounterOptions? EncounterType() {
			if (Game.GameData != null && Game.GameData.Global.surfing) {
				return EncounterOptions.Water;
			} else if (IsCave) {
				return EncounterOptions.Cave;
			//} else if (IsFlower) {
			//	return EncounterOptions.Land;
			//	return Method.RED_FLOWERS;
			} else if (IsGrass) {
				//time=GetTimeNow;
				EncounterOptions enctype=EncounterOptions.Land;
				//if (this.hasEncounter(EncounterTypes.LandNight) && DayNight.isNight(time)) enctype=EncounterTypes.LandNight;
				if (this.hasEncounter(EncounterOptions.LandNight) && Game.IsNight) enctype=EncounterOptions.LandNight;
				//if (this.hasEncounter(EncounterTypes.LandDay) && DayNight.isDay(time)) enctype=EncounterTypes.LandDay;
				if (this.hasEncounter(EncounterOptions.LandDay) && Game.IsDay) enctype=EncounterOptions.LandDay;
				//if (this.hasEncounter(EncounterTypes.LandMorning) && DayNight.isMorning(time)) enctype=EncounterTypes.LandMorning;
				if (this.hasEncounter(EncounterOptions.LandMorning) && Game.IsMorning) enctype=EncounterOptions.LandMorning;
				if (Game.GameData is IGameBugContest gbc && gbc.InBugContest && this.hasEncounter(EncounterOptions.BugContest)) {
					enctype=EncounterOptions.BugContest;
				}
				return enctype;
			}
			return null; //-1
		}

		public bool IsEncounterPossibleHere { get {
			if (Game.GameData != null && Game.GameData.Global.surfing) {
				return true;
			} else if (Game.GameData is IGameField gf && Terrain.isIce(gf.GetTerrainTag(Game.GameData.GamePlayer))) {
				return false;
			} else if (IsCave) {
				return true;
			//} else if (IsFlower) {
			//	return Terrain.isFlower(Game.GameData.GameMap.terrain_tag(Game.GameData.GamePlayer.x,Game.GameData.GamePlayer.y));
			} else if (IsGrass) {
				return Terrain.isGrass(Game.GameData.GameMap.terrain_tag(Game.GameData.GamePlayer.x,Game.GameData.GamePlayer.y));
			}
			return false;
		} }

		public void setup(int mapID) {
			@density=null;
			@stepcount=0;
			//@enctypes=new EncounterTypes[0];
			//@enctypes=new NestedDictionary<EncounterOptions, NestedDictionary<Pokemons, int[]>>();
			@enctypes=new Dictionary<EncounterOptions, IList<IEncounterPokemon>>();
			try {
				IDictionary<int, IEncounters> data = new Dictionary<int, IEncounters>();//load_data("Data/encounters.dat");
				Kernal.load_data(data, "Data/encounters.dat");
				if (data[mapID] != null) { //data.is_a(Hash) &&
					@density=data[mapID].EnctypeDensities.Values.ToArray();	//[0] | ToDo: refactor this and remove private variable?
					@enctypes=data[mapID].EnctypeEncounters;	//[1]
				} else {
					@density=null;
					//@enctypes=new EncounterTypes[0];
					//@enctypes=new NestedDictionary<EncounterOptions, NestedDictionary<Pokemons, int[]>>();
					@enctypes=new Dictionary<EncounterOptions, IList<IEncounterPokemon>>();
				}
			} catch (Exception) {
				@density=null;
				//@enctypes=new EncounterTypes[0];
				//@enctypes=new NestedDictionary<EncounterOptions, NestedDictionary<Pokemons, int[]>>();
				@enctypes=new Dictionary<EncounterOptions, IList<IEncounterPokemon>>();
			}
		}

		public bool MapHasEncounter (int mapID, EncounterOptions enctype) {
			IDictionary<int, IEncounters> data = new Dictionary<int, IEncounters>();//load_data("Data/encounters.dat");
			Kernal.load_data(data, "Data/encounters.dat");
			//Kernal.EncounterData[mapID]
			if (data[mapID] != null) { //data.is_a(Hash) &&
				enctypes=data[mapID].EnctypeEncounters;	//[1]
				density=data[mapID].EnctypeDensities.Values.ToArray();	//[0] | ToDo: refactor this and remove private variable?
			} else {
				return false;
			}
			if (density==null || enctype<0) return false;
			return enctypes[enctype] != null ? true : false;
			//return enctypes.Contains(enctype);
		}

		public IPokemon MapEncounter(int mapID, EncounterOptions enctype) {
			if (enctype<0 || (int)enctype>EnctypeChances.Count) {
				//raise new ArgumentError(Game._INTL("Encounter type out of range"));
				//throw new Exception(Game._INTL("Encounter type out of range"));
				Core.Logger.LogError(Game._INTL("Encounter type out of range"));
				return null;
			}
			IDictionary<int, IEncounters> data = new Dictionary<int, IEncounters>();//load_data("Data/encounters.dat");
			Kernal.load_data(data, "Data/encounters.dat");
			if (data[mapID] != null) { //data.is_a(Hash) &&
				enctypes=data[mapID].EnctypeEncounters;	//[1]
			} else {
				return null;
			}
			//if (enctypes[(int)enctype]==null) return null;
			if (!enctypes.Keys.Contains(enctype)) return null;
			int[] chances=EnctypeChances[enctype];
			int chancetotal=0;
			//chances.each {|a| chancetotal+=a}
			foreach(int a in chances) chancetotal+=a;
			int rnd=Core.Rand.Next(chancetotal);
			int chosenpkmn=0;
			int chance=0;
			for (int i = 0; i < chances.Length; i++) {
				chance+=chances[i];
				if (rnd<chance) {
					chosenpkmn=i;
					break;
				}
			}
			//int[] encounter=enctypes[enctype][chosenpkmn];  //[EncounterType][Pokemons][Min,Max]
			IEncounterPokemon encounter=enctypes[enctype][chosenpkmn];
			int level=encounter.MinLevel+Core.Rand.Next(1+encounter.MaxLevel-encounter.MinLevel);
			return new Pokemon(encounter.Pokemon,level:(byte)level);
		}

		public IPokemon EncounteredPokemon(EncounterOptions enctype, int tries=1) {
			if (enctype<0 || (int)enctype>EnctypeChances.Count) {
				//raise new ArgumentError(Game._INTL("Encounter type out of range"));
				//throw new Exception(Game._INTL("Encounter type out of range"));
				Core.Logger.LogError(Game._INTL("Encounter type out of range"));
				return null;
			}
			if (@enctypes[enctype]==null) return null;
			//Pokemons[] encounters=@enctypes[enctype].Value.Keys.ToArray();
			//NestedDictionary<Pokemons,int[]> encounters=@enctypes[enctype].Value;
			IList<IEncounterPokemon> encounters=@enctypes[enctype];
			int[] chances=EnctypeChances[enctype];
			IPokemon firstpoke=Game.GameData.Trainer.firstParty;
			if (firstpoke.IsNotNullOrNone() && !firstpoke.isEgg && Core.Rand.Next(2)==0) {
				if (firstpoke.Ability == Abilities.STATIC) {
					//Dictionary<Pokemons,int[]> newencs=new Dictionary<Pokemons, int[]>();List<int> newchances=new List<int>();
					IList<IEncounterPokemon> newencs=new List<IEncounterPokemon>();IList<int> newchances=new List<int>();
					//dexdata=OpenDexData();
					for (int x = 0; x < encounters.Count; x++) {
						//DexDataOffset(dexdata,encounters[x][0],8);
						//Types t1=dexdata.fgetb();
						//Types t1=Kernal.PokemonData[encounters.Keys.ElementAt(x)].Type[0];
						Types t1=Kernal.PokemonData[encounters[x].Pokemon].Type[0];
						//Types t2=dexdata.fgetb();
						//Types t2=Kernal.PokemonData[encounters.Keys.ElementAt(x)].Type[1];
						Types t2=Kernal.PokemonData[encounters[x].Pokemon].Type[1];
						if (t1 == Types.ELECTRIC || t2 == Types.ELECTRIC) {
							newencs.Add(encounters[x]); //[Pokemon,Min,Max]
							//newencs.Add(encounters.ElementAt(x).Key, encounters.ElementAt(x).Value.Value); //[Pokemon][Min,Max]
							newchances.Add(chances[x]);
						}
					}
					//dexdata.close();
					if (newencs.Count>0) {
						//encounters=(NestedDictionary<Pokemons, int[]>)newencs;
						encounters=newencs.ToArray();
						chances=newchances.ToArray();
					}
				}
				if (firstpoke.Ability == Abilities.MAGNET_PULL) {
					IList<IEncounterPokemon> newencs=new List<IEncounterPokemon>();List<int> newchances=new List<int>();
					//List<Pokemons> newencs=new List<Pokemons>(); List<int> newchances=new List<int>();
					//dexdata=OpenDexData();
					for (int y = 0; y < encounters.Count; y++) {
						//DexDataOffset(dexdata,encounters[x][0],8);
						//Types t1=dexdata.fgetb();
						//Types t1=Kernal.PokemonData[encounters.Keys.ElementAt(y)].Type[0];
						Types t1=Kernal.PokemonData[encounters[y].Pokemon].Type[0];
						//Types t2=dexdata.fgetb();
						//Types t2=Kernal.PokemonData[encounters.Keys.ElementAt(y)].Type[1];
						Types t2=Kernal.PokemonData[encounters[y].Pokemon].Type[1];
						if (t1 == Types.STEEL || t2 == Types.STEEL) {
							//newencs.Add(encounters.ElementAt(y).Key, encounters.ElementAt(y).Value.Value); //[Pokemon][Min,Max]
							newencs.Add(encounters[y]); //[Pokemon,Min,Max]
							newchances.Add(chances[y]);
						}
					}
					//dexdata.close();
					if (newencs.Count>0) {
						encounters=newencs;	//(NestedDictionary<Pokemons, int[]>)
						chances=newchances.ToArray();
					}
				}
			}
			int chancetotal=0;
			//chances.each {|a| chancetotal+=a}
			foreach(int a in chances) chancetotal+=a;
			int rnd=0; int i = 0;
			do { //tries.times ;
				int r=Core.Rand.Next(chancetotal);
				if (rnd<r) rnd=r;
			} while(i < tries);
			int chosenpkmn=0;
			int chance=0;
			for (i = 0; i < chances.Length; i++) {
				chance+=chances[i];
				if (rnd<chance) {
					chosenpkmn=i;
					break;
				}
			}
			//EncounterData encounter=encounters[chosenpkmn]; //[Pokemon,Min,Max]
			IEncounterPokemon encounter=encounters[chosenpkmn]; //[Pokemon,Min,Max]
			if (encounter == null) return null;
			//int level=encounter[0]+Core.Rand.Next(1+encounter[1]-encounter[0]);
			int level=encounter.MinLevel+Core.Rand.Next(1+encounter.MaxLevel-encounter.MinLevel);
			if (Game.GameData.Trainer.firstParty != null && !Game.GameData.Trainer.firstParty.isEgg &&
				(Game.GameData.Trainer.firstParty.Ability == Abilities.HUSTLE ||
				Game.GameData.Trainer.firstParty.Ability == Abilities.VITAL_SPIRIT ||
				Game.GameData.Trainer.firstParty.Ability == Abilities.PRESSURE) &&
				Core.Rand.Next(2)==0) {
				//int level2=encounter[0]+Core.Rand.Next(1+encounter[1]-encounter[0]);
				int level2=encounter.MinLevel+Core.Rand.Next(1+encounter.MaxLevel-encounter.MinLevel);
				level=Math.Max(level,level2);
			}
			if (Game.GameData.PokemonMap.blackFluteUsed && Core.USENEWBATTLEMECHANICS) {
				level=Math.Min(level+1+Core.Rand.Next(3),Core.MAXIMUMLEVEL);
			} else if (Game.GameData.PokemonMap.whiteFluteUsed && Core.USENEWBATTLEMECHANICS) {
				level=Math.Max(level-1-Core.Rand.Next(3),1);
			}
			//return [encounter[chosenpkmn].Pokemon,level];
			//return new Pokemon(encounters.ElementAt(chosenpkmn).Key,level:(byte)level);
			return new Pokemon(encounter.Pokemon,level:(byte)level);
		}

		public bool CanEncounter (IPokemon encounter) {
			if (Game.GameData.GameSystem.encounter_disabled) return false;
			if (!encounter.IsNotNullOrNone() || Game.GameData.Trainer == null) return false;
			if (Core.DEBUG && Input.press(PokemonUnity.Input.CTRL)) return false;
			if (Game.GameData is IGamePokeRadar gr && !gr.PokeRadarOnShakingGrass()) {
				if (Game.GameData.Global.repel>0 && Game.GameData.Trainer.ablePokemonCount>0 &&
					encounter.Level<=Game.GameData.Trainer.ablePokemonParty.ElementAt(0).Level) return false;
			}
			return true;
		}

		public IPokemon GenerateEncounter(EncounterOptions enctype) {
			if (enctype<0 || (int)enctype>EnctypeChances.Count) {
				//raise new ArgumentError(Game._INTL("Encounter type out of range"));
				//throw new Exception(Game._INTL("Encounter type out of range"));
				return new Pokemon(Pokemons.NONE);
			}
			if (@density==null) return null;
			if (@density[(int)enctype]==0 || @density[(int)enctype] == null) return null;
			if (@enctypes[enctype]==null) return null;
			@stepcount+=1;
			if (@stepcount<=3) return null;	// Check three steps after battle ends
			float encount=@density[(int)enctype]*16;
			if (Game.GameData.Global.bicycle) {
				encount=(encount*0.8f);
			}
			if (Game.GameData.PokemonMap.blackFluteUsed && !Core.USENEWBATTLEMECHANICS) {
				encount=(encount/2);
			} else if (Game.GameData.PokemonMap.whiteFluteUsed && !Core.USENEWBATTLEMECHANICS) {
				encount=(encount*1.5f);
			}
			IPokemon firstpoke=Game.GameData.Trainer.firstParty;
			if (firstpoke.IsNotNullOrNone() && !firstpoke.isEgg) {
				if (firstpoke.Item == Items.CLEANSE_TAG) {
					encount=(encount*2/3);
				} else if (firstpoke.Item == Items.PURE_INCENSE) {
					encount=(encount*2/3);
				} else {  // Ignore ability effects if an item effect applies
					if (firstpoke.Ability == Abilities.STENCH) {
						encount=(encount/2);
					} else if (firstpoke.Ability == Abilities.WHITE_SMOKE) {
						encount=(encount/2);
					} else if (firstpoke.Ability == Abilities.QUICK_FEET) {
						encount=(encount/2);
					} else if (firstpoke.Ability == Abilities.SNOW_CLOAK &&
						(Game.GameData.GameScreen.weather_type==FieldWeathers.Snow ||		//Game.Weather
						Game.GameData.GameScreen.weather_type==FieldWeathers.Blizzard)) {	//Game.Weather
						encount=(encount/2);
					} else if (firstpoke.Ability == Abilities.SAND_VEIL &&
						Game.GameData.GameScreen.weather_type==FieldWeathers.Sandstorm) {	//Game.Weather
						encount=(encount/2);
					} else if (firstpoke.Ability == Abilities.SWARM) {
						encount=(encount*1.5f);
					} else if (firstpoke.Ability == Abilities.ILLUMINATE) {
						encount=(encount*2);
					} else if (firstpoke.Ability == Abilities.ARENA_TRAP) {
						encount=(encount*2);
					} else if (firstpoke.Ability == Abilities.NO_GUARD) {
						encount=(encount*2);
					}
				}
			}
			if (Core.Rand.Next(180*16)>=encount) return null;
			IPokemon encpoke=EncounteredPokemon(enctype);
			if (encpoke.IsNotNullOrNone() && firstpoke.IsNotNullOrNone() && !firstpoke.isEgg) {
				if (firstpoke.Ability == Abilities.INTIMIDATE ||
					firstpoke.Ability == Abilities.KEEN_EYE) {
					if (encpoke.Level<=firstpoke.Level-5 && Core.Rand.Next(2)==0) {
						encpoke=null;
					}
				}
			}
			return encpoke;
		}

		#region Encounter Modifiers
		//###############################################################################
		// This section was created solely for you to put various bits of code that
		// modify various wild Pokémon and trainers immediately prior to battling them.
		// Be sure that any code you use here ONLY applies to the Pokémon/trainers you
		// want it to apply to!
		//###############################################################################


		public event Action<object, IOnWildPokemonCreateEventArgs> OnWildPokemonCreate;
		//Events.onWildPokemonCreate+=proc {|sender,e|
		protected void Events_onWildPokemonCreate(object sender, EventArg.OnWildPokemonCreateEventArgs e) {
		   // Make all wild Pokémon shiny while a certain Switch is ON (see Settings).
		   //Monster.Pokemon pokemon = e[0];
			IPokemon pokemon = e.Pokemon;
			if (Core.SHINY_WILD_POKEMON_SWITCH) {
				pokemon.makeShiny();
			}
		//}

		// Used in the random dungeon map.  Makes the levels of all wild Pokémon in that
		// map depend on the levels of Pokémon in the player's party.
		// This is a simple method, and can/should be modified to account for evolutions
		// and other such details.  Of course, you don't HAVE to use this code.
		//Events.onWildPokemonCreate+=proc {|sender,e|
		//private void onWildPokemonCreate(object sender, EventArgs e) {
			//IPokemon pokemon = e[0];
			if (Game.GameData.GameMap is IGameMapOrgBattle gmo && gmo.map_id==51) {
				int newlevel=(int)Math.Round((Game.GameData as PokemonEssentials.Interface.IGameUtility).BalancedLevel(Game.GameData.Trainer.party) - 4 + Core.Rand.Next(5));   // For variety
				if (newlevel < 1) newlevel=1;
				if (newlevel > Core.MAXIMUMLEVEL) newlevel = Core.MAXIMUMLEVEL;
				//pokemon.Level=newlevel;
				((Pokemon)pokemon).SetLevel((byte)newlevel);
				pokemon.calcStats();
				pokemon.resetMoves();
			}
		}

		// This is the basis of a trainer modifier.  It works both for trainers loaded
		// when you battle them, and for partner trainers when they are registered.
		// Note that you can only modify a partner trainer's Pokémon, and not the trainer
		// themselves nor their items this way, as those are generated from scratch
		// before each battle.
		public event Action<object, IOnTrainerPartyLoadEventArgs> OnTrainerPartyLoad;
		//Events.onTrainerPartyLoad+=proc {|sender,e|
		private protected void Events_onTrainerPartyLoad(object sender, EventArg.OnTrainerPartyLoadEventArgs e) {
			if (e.Trainer!=null) {				//[0] Game.GameData.Trainer data should exist to be loaded, but may not exist somehow
				ITrainer trainer=e.Trainer;		//[0][0] A PokeBattle_Trainer object of the loaded trainer
				IList<Items> items=e.Items;		//[0][1] An array of the trainer's items they can use
				IList<IPokemon> party=e.Party;	//[0][2] An array of the trainer's Pokémon
				//YOUR CODE HERE
			}
		}
		#endregion
	}
}