using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Attack;
using PokemonUnity.Saving;
using PokemonUnity.Overworld;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using PokemonUnity.Inventory.Plants;
using PokemonUnity.Attack.Data;
using PokemonUnity.Character;
//using System.Data.SqlClient;
//using System.Security.Cryptography;

namespace PokemonUnity
{
	/// <summary>
	/// Variables that are stored when game is saved, and other temp values used for gameplay.
	/// This class should be called once, when the game boots-up.
	/// During boot-up, game will check directory for save files and load data.
	/// Game class will overwrite all the other class default values when player triggers a load state.
	/// </summary>
	//ToDo: Missing (Trainer)Player.Rival variable (KeyValuePair <Type,Name>)
	//ToDo: Add Write_DB_To_File Function
	public partial class Game
	{
		#region Variables
		public static string FilePokemonXML = "";
		public static Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonEvolution[]> PokemonEvolutionsData { get; private set; }
		public static Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonWildItems[]> PokemonItemsData { get; private set; }
		public static Dictionary<Pokemons, PokemonUnity.Monster.Data.Form[]> PokemonFormsData { get; private set; }
		public static Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonMoveTree> PokemonMovesData { get; private set; }
		public static Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonData> PokemonData { get; private set; }
		public static Dictionary<Natures,PokemonUnity.Monster.Nature> NatureData { get; private set; }
		public static Dictionary<Types,PokemonUnity.Monster.Data.Type> TypeData { get; private set; }
		public static Dictionary<Moves,Attack.Data.MoveData> MoveData { get; private set; }
		public static Dictionary<Moves,Attack.Data.MetaData> MoveMetaData { get; private set; }
		public static Dictionary<Attack.Data.Effects,Combat.Move> MoveEffectData { get; private set; }
		public static Dictionary<Items,ItemData> ItemData { get; private set; }
		public static Dictionary<Items,Berry> BerryData { get; private set; }
		public static Dictionary<Regions,Locations[]> RegionData { get; private set; }
		public static Dictionary<Regions,Pokedex> PokedexData { get; private set; }
		/// <summary>
		/// Key: <seealso cref="Locations"/> | Value: <seealso cref="Area.Id"/>
		/// </summary>
		public static Dictionary<Locations,int[]> LocationData { get; private set; }
		/// <summary>
		/// List of <see cref="Player.Area"/> that triggers <seealso cref="Overworld.EncounterData"/>
		/// <para></para>
		/// Key: <seealso cref="Overworld.Area.Id"/> | Value: <seealso cref="Player.Area"/>
		/// </summary>
		public static Dictionary<int,Area> AreaData { get; private set; }
		/// <summary>
		/// Key: <seealso cref="Method"/> | Value: <seealso cref="EncounterData.Id"/>
		/// </summary>
		public static Dictionary<Method,int[]> MethodData { get; private set; }
		/// <summary>
		/// Key: <seealso cref="EncounterData.Id"/> | Value: <seealso cref="Overworld.EncounterData"/>
		/// </summary>
		public static Dictionary<int,EncounterData> EncounterData { get; private set; }
		//public static Dictionary<Method,EncounterData> EncounterMethodData { get; private set; }
		public static Dictionary<TrainerTypes,TrainerMetaData> TrainerMetaData { get; private set; }
		public static Dictionary<GymBadges,Character.BadgeData> BadgeData { get; private set; }
		public static Dictionary<HiddenMoves,HiddenMoveData> HiddenMoveData { get; private set; }
		public static Dictionary<int,MachineData> MachineData { get; private set; }
		//public static Dictionary<Items,KeyValuePair<Moves,int[]>[]> MachineMetaData { get; private set; }
		//public static Dictionary<int,Machine> MapData { get; private set; }
		//public static Dictionary<int,int> TextureData1 { get; private set; }
		//public static Dictionary<int,int> TextureData2 { get; private set; }
		//public static Dictionary<Location,Script[]> ScriptData { get; private set; }
		//public static Dictionary<Location,Script[]> LevelScriptData { get; private set; }
		//public static Dictionary<Location,Text[]> TextData { get; private set; }
		//public static Dictionary<Location,EventData> EventData { get; private set; }
		//public static Dictionary<Location,Event[]> EventData { get; private set; }
		//public static Dictionary<Location,EventFlags[]> MapFlagData { get; private set; }
		#endregion

		#region Initialize Data From Database
		public static bool InitPokemons(bool sql = true)
		{
			PokemonData = new Dictionary<Pokemons, Monster.Data.PokemonData>();
			if (sql) //using (con)
				return GetPokemonsFromSQL(con);
			else return GetPokemonsFromXML();
		}
		public static bool InitPokemonMoves(bool sql = true)
		{
			PokemonMovesData = new Dictionary<Pokemons, Monster.Data.PokemonMoveTree>();
			if (sql) //using (con)
				return GetPokemonMovesFromSQL(con);
			else return false;
		}
		public static bool InitPokemonEvolutions(bool sql = true)
		{
			PokemonEvolutionsData = new Dictionary<Pokemons, Monster.Data.PokemonEvolution[]>();
			if (sql) //using (con)
				return GetPokemonEvolutionsFromSQL(con);
			else return false;
		}
		public static bool InitPokemonItems(bool sql = true)
		{
			PokemonItemsData = new Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonWildItems[]>();
			if (sql) //using (con)
				return GetPokemonItemsFromSQL(con);
			else return false;
		}
		public static bool InitPokemonForms(bool sql = true)
		{
			PokemonFormsData = new Dictionary<Pokemons, Monster.Data.Form[]>();
			if (sql) //using (con)
				return GetPokemonFormsFromSQL(con);
			else return false;
		}
		public static bool InitNatures(bool sql = true)
		{
			NatureData = new Dictionary<Natures, Nature>();
			if (sql) //using (con)
				return GetNaturesFromSQL(con);
			else return false;
		}
		public static bool InitTypes(bool sql = true)
		{
			TypeData = new Dictionary<Types, PokemonUnity.Monster.Data.Type>();
			if (sql) //using (con)
				return GetTypesFromSQL(con);
			else return false;
		}
		public static bool InitMoves(bool sql = true)
		{
			MoveData = new Dictionary<Moves, Attack.Data.MoveData>();
			MoveMetaData = new Dictionary<Moves, Attack.Data.MetaData>();
			//MoveEffectData = new Dictionary<Attack.Data.Effects, Combat.Move>(); //Below
			if (sql) //using (con)
				return GetMovesFromSQL(con);
			else return false;
		}
		public static bool InitMoveEffects(bool sql = false)
		{
			MoveEffectData = new Dictionary<Attack.Data.Effects, Combat.Move>();
			if (sql) //using (con)
				return false;
			else return GetMoveEffectFromXML();
		}
		public static bool InitItems(bool sql = true)
		{
			ItemData = new Dictionary<Items, ItemData>();
			if (sql) //using (con)
				return GetItemsFromSQL(con);
			else return false;
		}
		public static bool InitBerries(bool sql = true)
		{
			BerryData = new Dictionary<Items, Berry>();
			if (sql) //using (con)
				return GetBerriesFromSQL(con);
			else return false;
		}
		public static bool InitRegions(bool sql = true)
		{
			RegionData = new Dictionary<Regions, Locations[]>();
			//PokedexData = new Dictionary<Regions, Pokedex>();
			if (sql) //using (con)
				return  GetRegionsFromSQL(con);
			else return false;
		}
		public static bool InitLocations(bool sql = true)
		{
			LocationData = new Dictionary<Locations, int[]>();
			AreaData = new Dictionary<int, Area>();
			if (sql) //using (con)
				return  GetLocationsFromSQL(con);
			else return false;
		}
		public static bool InitEncounters(bool sql = true)
		{
			MethodData = new Dictionary<Method, int[]>();
			EncounterData = new Dictionary<int, EncounterData>();
			if (sql) //using (con)
				return GetEncountersFromSQL(con);
			else return false;
		}
		public static bool InitMachines(bool sql = true)
		{
			//MethodData = new Dictionary<Method, int[]>();
			//EncounterData = new Dictionary<int, Encounter>();
			if (sql) //using (con)
				return GetMachinesFromSQL(con);
			else return false;
		}
		public static bool InitTrainers(bool sql = false)
		{
			TrainerMetaData = new Dictionary<TrainerTypes, TrainerData>();
			//TrainerData = new Dictionary<int, Encounter>();
			if (sql) //using (con)
				return false; //GetTrainersFromSQL(con);
			else return GetTrainersFromXML();
		}
		public static bool InitGymBadges(bool sql = false)
		{
			BadgeData = new Dictionary<GymBadges, Character.BadgeData>();
			HiddenMoveData = new Dictionary<HiddenMoves, HiddenMoveData>();
			if (sql) //using (con)
				return false; //GetGymBadgesFromSQL(con);
			else return GetGymBadgesFromXML();
		}
		public static bool InitPokedex(bool sql = true)
		{
			//Init in Region Method
			PokedexData = new Dictionary<Regions, Pokedex>();
			if (sql) //using (con)
				return GetPokedexFromSQL(con);
			else return false;
		}
		//public static bool InitLoadFile(bool sql = true)
		//{
		//	TrainerMetaData = new Dictionary<TrainerTypes, TrainerMetaData>();
		//	if (sql) //using (con)
		//		return GetTrainersFromSQL(con);
		//	else return false;
		//}
		#endregion

		#region From XML
		static bool GetPokemonsFromXML()
		{
			var xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(LockFileStream(FilePokemonXML));

			var pokemons = xmlDocument.SelectNodes("/Pokemons");
			if (pokemons == null || pokemons.Count <= 0)
			{
				//throw new Exception("Xml must include "Pokemons" as root node.");
				GameDebug.LogError("Xml must include \"Pokemons\" as root node.");
				return false;
			}

			//var dublicateNames = new List<string>();

			var pokemonNodes = xmlDocument.SelectSingleNode("/Pokemons").ChildNodes;// /text
			if (pokemonNodes != null)
			{
				//foreach value in xml
				foreach (XmlNode node in pokemonNodes)
				{
					//XmlAttribute sngl = node.GetAttributeValueOrNull("SingleGender");
					PokemonData.Add((Pokemons)int.Parse(node.GetAttributeValueOrNull("Id")),
						new PokemonUnity.Monster.Data.PokemonData(
							Id: (Pokemons)int.Parse(node.GetAttributeValueOrNull("Id"))
							,regionalDex: new int[] { int.Parse(node.GetAttributeValueOrNull("ReDex")) }
							,type1: (Types)int.Parse(node.GetAttributeValueOrNull("Type1"))
							,type2: (Types)int.Parse(node.GetAttributeValueOrNull("Type2"))
							,ability1: (Abilities)int.Parse(node.GetAttributeValueOrNull("Ability1"))
							,ability2: (Abilities)int.Parse(node.GetAttributeValueOrNull("Ability2"))
							,hiddenAbility: (Abilities)int.Parse(node.GetAttributeValueOrNull("Ability3"))
							,genderRatio: (PokemonUnity.Monster.GenderRatio)int.Parse(node.GetAttributeValueOrNull("Gender"))
							//,maleRatio: float.Parse(node.GetAttributeValueOrNull("Gender"))
							,catchRate: int.Parse(node.GetAttributeValueOrNull("Catch"))
							,eggGroup1: (PokemonUnity.Monster.EggGroups)int.Parse(node.GetAttributeValueOrNull("Egg1"))
							,eggGroup2: (PokemonUnity.Monster.EggGroups)int.Parse(node.GetAttributeValueOrNull("Egg2"))
							,hatchTime: int.Parse(node.GetAttributeValueOrNull("Hatch"))
							,height: float.Parse(node.GetAttributeValueOrNull("Height"))
							,weight: float.Parse(node.GetAttributeValueOrNull("Weight"))
							,baseExpYield: int.Parse(node.GetAttributeValueOrNull("Exp"))
							,levelingRate: (PokemonUnity.Monster.LevelingRate)int.Parse(node.GetAttributeValueOrNull("Growth"))
							,evHP:  int.Parse(node.GetAttributeValueOrNull("eHP") )
							,evATK: int.Parse(node.GetAttributeValueOrNull("eATK"))
							,evDEF: int.Parse(node.GetAttributeValueOrNull("eDEF"))
							,evSPA: int.Parse(node.GetAttributeValueOrNull("eSPA"))
							,evSPD: int.Parse(node.GetAttributeValueOrNull("eSPD"))
							,evSPE: int.Parse(node.GetAttributeValueOrNull("eSPE"))
							,pokedexColor: (Color)int.Parse(node.GetAttributeValueOrNull("Color"))
							,baseFriendship: int.Parse(node.GetAttributeValueOrNull("Friend"))
							,baseStatsHP:  int.Parse(node.GetAttributeValueOrNull("HP" ))
							,baseStatsATK: int.Parse(node.GetAttributeValueOrNull("ATK"))
							,baseStatsDEF: int.Parse(node.GetAttributeValueOrNull("DEF"))
							,baseStatsSPA: int.Parse(node.GetAttributeValueOrNull("SPA"))
							,baseStatsSPD: int.Parse(node.GetAttributeValueOrNull("SPD"))
							,baseStatsSPE: int.Parse(node.GetAttributeValueOrNull("SPE"))
							,rarity: (Rarity)int.Parse(node.GetAttributeValueOrNull("Rarity"))
							//,luminance: Data.luminance
							//,movesetmoves: Data.movesetmoves
							//,movesetLevels: Data.movesetLevels
							//,movesetMoves: Data.movesetMoves
							//,tmList: Data.tmList
							//,evolution: Data.evolution
							//,evolutionID: Data.EvolutionTO
							//,evolutionLevel: Data.evolutionLevel
							//,evolutionMethod: Data.evolutionMethod
							//,evolutionFROM: Data.EvolutionFROM
							//,evolutionTO: Data.EvolutionTO
							,evoChainId: int.Parse(node.GetAttributeValueOrNull("EvoChainId"))
							,generationId: (byte)int.Parse(node.GetAttributeValueOrNull("GenerationId"))
							,isDefault:		node.GetAttributeValueOrNull("IsBaseForm") == "1"
							,isBaby:			node.GetAttributeValueOrNull("IsBaby") == "1"
							,formSwitchable:	node.GetAttributeValueOrNull("SwitchableForm") == "1"
							,hasGenderDiff:	node.GetAttributeValueOrNull("GenderDiff") == "1"
							,habitatId: (PokemonUnity.Monster.Habitat)int.Parse(node.GetAttributeValueOrNull("HabitatId"))
							,shapeId: (PokemonUnity.Monster.Shape)int.Parse(node.GetAttributeValueOrNull("ShapeId"))
							//,baseForm: Data.baseForm
							//,heldItem: Data.heldItem
						)
					);
				}
			}

			//dictionaries cant store duplicate values...
			//if (dublicateNames.Count > 0)
			//{
			//	//throw new Exception("A dictionary can not contain same key twice. There are some duplicated names: " + dublicateNames.JoinAsString(", "));//string.Join(", ",dublicateNames.ToArray())
			//	GameDebug.LogError("A dictionary can not contain same key twice. There are some duplicated names: " + dublicateNames.JoinAsString(", "));//string.Join(", ",dublicateNames.ToArray())
			//}

			return true; //dictionary;
		}
		static bool GetMoveEffectFromXML()
		{
			try
			{
				foreach (Attack.Data.Effects effect in Enum.GetValues(typeof(Attack.Data.Effects)))
				{
					switch (effect)
					{
						case Effects.NONE:
							break;
						case Effects.x001:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_000());
							break;
						case Effects.x002:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_003());
							break;
						case Effects.x003:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_005());
							break;
						case Effects.x004:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0DD());
							break;
						case Effects.x005:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_00A());
							break;
						case Effects.x006:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_00C());
							break;
						case Effects.x007:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_007());
							break;
						case Effects.x008:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0E0());
							break;
						case Effects.x009:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0DE());
							break;
						case Effects.x00A:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0AE());
							break;
						case Effects.x00B:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_01C());
							break;
						case Effects.x00C:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_01D());
							break;
						case Effects.x00E:
							break;
						case Effects.x011:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_022());
							break;
						case Effects.x012:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0A5());
							break;
						case Effects.x013:
							break;
						case Effects.x014:
							//ToDo: Tail whip has just one effect
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_043());
							break;
						case Effects.x015:
							//ToDo: low-sweep and electroweb
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_044());
							break;
						case Effects.x018:
							//ToDo: Only has one effect
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_047());
							break;
						case Effects.x019:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_048());
							break;
						case Effects.x01A:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_051());
							break;
						case Effects.x01B:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0D4());
							break;
						case Effects.x01C:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0D2());
							break;
						case Effects.x01D:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0EB());
							break;
						case Effects.x01E:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0C0());
							break;
						case Effects.x01F:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_05E());
							break;
						case Effects.x020:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_00F());
							break;
						case Effects.x021:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0D5());
							break;
						case Effects.x022:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_006());
							break;
						case Effects.x023:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_109());
							break;
						case Effects.x024:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0A3());
							break;
						case Effects.x025:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_017());
							break;
						case Effects.x026:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0D9());
							break;
						case Effects.x027:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_070());
							break;
						case Effects.x028:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0C3());
							break;
						case Effects.x029:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_06C());
							break;
						case Effects.x02A:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_06B());
							break;
						case Effects.x02B:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0CF());
							break;
						case Effects.x02C:
							//ToDo: has an effect...
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_000());
							break;
						case Effects.x02D:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0BD());
							break;
						case Effects.x02E:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_10B());
							break;
						case Effects.x02F:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_056());
							break;
						case Effects.x030:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_023());
							break;
						case Effects.x031:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0FA());
							break;
						case Effects.x032:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_12A());
							break;
						case Effects.x033:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_02E());
							break;
						case Effects.x034:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_02F());
							break;
						case Effects.x035:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_030());
							break;
						case Effects.x036:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_032());
							break;
						case Effects.x037:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_033());
							break;
						case Effects.x03A:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_069());
							break;
						case Effects.x03B:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_04B());
							break;
						case Effects.x03C:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_04C());
							break;
						case Effects.x03D:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_04D());
							break;
						case Effects.x03E:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_13D());
							break;
						case Effects.x03F:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_04F());
							break;
						case Effects.x042:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0A2());
							break;
						case Effects.x043:
							break;
						case Effects.x044:
							break;
						case Effects.x045:
							break;
						case Effects.x046:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_043());
							break;
						case Effects.x047:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_044());
							break;
						case Effects.x048:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_045());
							break;
						case Effects.x049:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_046());
							break;
						case Effects.x04A:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_047());
							break;
						case Effects.x04B:
							break;
						case Effects.x04C:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0C7());
							break;
						case Effects.x04D:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_013());
							break;
						case Effects.x04E:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0BE());
							break;
						case Effects.x04F:
							break;
						case Effects.x050:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_10C());
							break;
						case Effects.x051:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0C2());
							break;
						case Effects.x052:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_093());
							break;
						case Effects.x053:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_05C());
							break;
						case Effects.x054:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0B6());
							break;
						case Effects.x055:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0DC());
							break;
						case Effects.x056:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_001());
							break;
						case Effects.x057:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0B9());
							break;
						case Effects.x058:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_06D());
							break;
						case Effects.x059:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_06F());
							break;
						case Effects.x05A:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_071());
							break;
						case Effects.x05B:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0BC());
							break;
						case Effects.x05C:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_05A());
							break;
						case Effects.x05D:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_011());
							break;
						case Effects.x05E:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_05F());
							break;
						case Effects.x05F:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0A6());
							break;
						case Effects.x060:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_05D());
							break;
						case Effects.x062:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0B4());
							break;
						case Effects.x063:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0E7());
							break;
						case Effects.x064:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_098());
							break;
						case Effects.x065:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_10E());
							break;
						case Effects.x066:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0E9());
							break;
						case Effects.x067:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_019());
							break;
						case Effects.x068:
							break;
						case Effects.x069:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0BF());
							break;
						case Effects.x06A:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0F1());
							break;
						case Effects.x06B:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0EF());
							break;
						case Effects.x06C:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_10F());
							break;
						case Effects.x06D:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_034());
							break;
						case Effects.x06E:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_10D());
							break;
						case Effects.x070:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0AA());
							break;
						case Effects.x071:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_103());
							break;
						case Effects.x072:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0A7());
							break;
						case Effects.x073:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0E5());
							break;
						case Effects.x074:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_101());
							break;
						case Effects.x075:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0E8());
							break;
						case Effects.x076:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0D3());
							break;
						case Effects.x077:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_041());
							break;
						case Effects.x078:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_091());
							break;
						case Effects.x079:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_016());
							break;
						case Effects.x07A:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_089());
							break;
						case Effects.x07B:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_094());
							break;
						case Effects.x07C:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_08A());
							break;
						case Effects.x07D:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_01A());
							break;
						case Effects.x07E:
							break;
						case Effects.x07F:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_095());
							break;
						case Effects.x080:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0ED());
							break;
						case Effects.x081:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_088());
							break;
						case Effects.x082:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_110());
							break;
						case Effects.x083:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_06A());
							break;
						case Effects.x085:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0D8());
							break;
						case Effects.x088:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_090());
							break;
						case Effects.x089:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_100());
							break;
						case Effects.x08A:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0FF());
							break;
						case Effects.x08B:
							//ToDo: Has other effects beyond this class
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_01D());
							break;
						case Effects.x08C:
							break;
						case Effects.x08D:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_02D());
							break;
						case Effects.x08F:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_03A());
							break;
						case Effects.x090:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_055());
							break;
						case Effects.x091:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_072());
							break;
						case Effects.x092:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0C8());
							break;
						case Effects.x093:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_078());
							break;
						case Effects.x094:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_076());
							break;
						case Effects.x095:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_111());
							break;
						case Effects.x096:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_077());
							break;
						case Effects.x097:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_010());
							break;
						case Effects.x098:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0C4());
							break;
						case Effects.x099:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_008());
							break;
						case Effects.x09A:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0EA());
							break;
						case Effects.x09B:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0C1());
							break;
						case Effects.x09C:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0C9());
							break;
						case Effects.x09D:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_01E());
							break;
						case Effects.x09F:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_012());
							break;
						case Effects.x0A0:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0D1());
							break;
						case Effects.x0A1:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_112());
							break;
						case Effects.x0A2:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_113());
							break;
						case Effects.x0A3:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_114());
							break;
						case Effects.x0A5:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_102());
							break;
						case Effects.x0A6:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0B7());
							break;
						case Effects.x0A7:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_040());
							break;
						case Effects.x0A8:
							break;
						case Effects.x0A9:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0E2());
							break;
						case Effects.x0AA:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_07E());
							break;
						case Effects.x0AB:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_115());
							break;
						case Effects.x0AC:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_07C());
							break;
						case Effects.x0AD:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_117());
							break;
						case Effects.x0AE:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0B3());
							break;
						case Effects.x0AF:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_021());
							break;
						case Effects.x0B0:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0BA());
							break;
						case Effects.x0B1:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_09C());
							break;
						case Effects.x0B2:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0F2());
							break;
						case Effects.x0B3:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_065());
							break;
						case Effects.x0B4:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0D7());
							break;
						case Effects.x0B5:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0B5());
							break;
						case Effects.x0B6:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0DB());
							break;
						case Effects.x0B7:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_03B());
							break;
						case Effects.x0B8:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0B1());
							break;
						case Effects.x0B9:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0F6());
							break;
						case Effects.x0BA:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_081());
							break;
						case Effects.x0BB:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_10A());
							break;
						case Effects.x0BC:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_004());
							break;
						case Effects.x0BD:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0F0());
							break;
						case Effects.x0BE:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_06E());
							break;
						case Effects.x0BF:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_08B());
							break;
						case Effects.x0C0:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_067());
							break;
						case Effects.x0C1:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0B8());
							break;
						case Effects.x0C2:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_018());
							break;
						case Effects.x0C3:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0E6());
							break;
						case Effects.x0C4:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0B2());
							break;
						case Effects.x0C5:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_09A());
							break;
						case Effects.x0C6:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0A4());
							break;
						case Effects.x0C7:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0FB());
							break;
						case Effects.x0C8:
							break;
						case Effects.x0C9:
							break;
						case Effects.x0CA:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_09D());
							break;
						case Effects.x0CB:
							break;
						case Effects.x0CC:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_087());
							break;
						case Effects.x0CD:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_03F());
							break;
						case Effects.x0CE:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_04A());
							break;
						case Effects.x0CF:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_02A());
							break;
						case Effects.x0D0:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_11B());
							break;
						case Effects.x0D1:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_024());
							break;
						case Effects.x0D2:
							break;
						case Effects.x0D3:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_09E());
							break;
						case Effects.x0D4:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_02C());
							break;
						case Effects.x0D5:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_026());
							break;
						case Effects.x0D6:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_060());
							break;
						case Effects.x0D7:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0D6());
							break;
						case Effects.x0D8:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_118());
							break;
						case Effects.x0D9:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0A8());
							break;
						case Effects.x0DA:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_07D());
							break;
						case Effects.x0DB:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_03E());
							break;
						case Effects.x0DC:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_08D());
							break;
						case Effects.x0DD:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0E3());
							break;
						case Effects.x0DE:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_080());
							break;
						case Effects.x0DF:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_096());
							break;
						case Effects.x0E0:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0AD());
							break;
						case Effects.x0E1:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0F4());
							break;
						case Effects.x0E2:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_05B());
							break;
						case Effects.x0E3:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_037());
							break;
						case Effects.x0E4:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_073());
							break;
						case Effects.x0E5:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0EE());
							break;
						case Effects.x0E6:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_03C());
							break;
						case Effects.x0E7:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_084());
							break;
						case Effects.x0E8:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_082());
							break;
						case Effects.x0E9:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0F8());
							break;
						case Effects.x0EA:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0F7());
							break;
						case Effects.x0EB:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_01B());
							break;
						case Effects.x0EC:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_097());
							break;
						case Effects.x0ED:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0BB());
							break;
						case Effects.x0EE:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_08C());
							break;
						case Effects.x0EF:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_057());
							break;
						case Effects.x0F0:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_068());
							break;
						case Effects.x0F1:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0A1());
							break;
						case Effects.x0F2:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0B0());
							break;
						case Effects.x0F3:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0AF());
							break;
						case Effects.x0F4:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_052());
							break;
						case Effects.x0F5:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_053());
							break;
						case Effects.x0F6:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_08F());
							break;
						case Effects.x0F7:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_125());
							break;
						case Effects.x0F8:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_064());
							break;
						case Effects.x0F9:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_116());
							break;
						case Effects.x0FA:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_104());
							break;
						case Effects.x0FB:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_054());
							break;
						case Effects.x0FC:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0DA());
							break;
						case Effects.x0FD:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_119());
							break;
						case Effects.x0FE:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0FE());
							break;
						case Effects.x0FF:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_002());
							break;
						case Effects.x100:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0CB());
							break;
						case Effects.x101:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0CA());
							break;
						case Effects.x102:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_075());
							break;
						case Effects.x103:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_049());
							break;
						case Effects.x104:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_11F());
							break;
						case Effects.x105:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_00D());
							break;
						case Effects.x106:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0D0());
							break;
						case Effects.x107:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0FD());
							break;
						case Effects.x108:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0CC());
							break;
						case Effects.x10A:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_04E());
							break;
						case Effects.x10B:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_105());
							break;
						case Effects.x10C:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_014());
							break;
						case Effects.x10D:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_09F());
							break;
						case Effects.x10E:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0FC());
							break;
						case Effects.x10F:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0E4());
							break;
						case Effects.x110:
							//ToDo: Seed Flare additional effect
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_04F());
							break;
						case Effects.x111:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0CD());
							//ToDo: Phantom and shadow force are the same...
							//MoveEffectData.Add(effect, new Combat.PokeBattle_Move_14D());
							break;
						case Effects.x112:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_00B());
							break;
						case Effects.x113:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_00E());
							break;
						case Effects.x114:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_009());
							break;
						case Effects.x115:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_020());
							break;
						case Effects.x116:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_029());
							break;
						case Effects.x117:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0AC());
							break;
						case Effects.x118:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_059());
							break;
						case Effects.x119:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_058());
							break;
						case Effects.x11A:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_124());
							break;
						case Effects.x11B:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_122());
							break;
						case Effects.x11C:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_07B());
							break;
						case Effects.x11D:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_031());
							break;
						case Effects.x11E:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_11A());
							break;
						case Effects.x11F:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0F9());
							break;
						case Effects.x120:
							//ToDo: Smack down has different effect
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_11C());
							break;
						case Effects.x121:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0A0());
							break;
						case Effects.x122:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_074());
							break;
						case Effects.x123:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_02B());
							break;
						case Effects.x124:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_09B());
							break;
						case Effects.x125:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_123());
							break;
						case Effects.x126:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_099());
							break;
						case Effects.x127:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_061());
							break;
						case Effects.x128:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_01F());
							break;
						case Effects.x129:
							//ToDo: Acid-spray additional effects
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_04F());
							break;
						case Effects.x12A:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_121());
							break;
						case Effects.x12B:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_063());
							break;
						case Effects.x12C:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_066());
							break;
						case Effects.x12D:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_11D());
							break;
						case Effects.x12E:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_083());
							break;
						case Effects.x12F:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_092());
							break;
						case Effects.x130:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0A9());
							break;
						case Effects.x131:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_050());
							break;
						case Effects.x132:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_08E());
							break;
						case Effects.x133:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0AB());
							break;
						case Effects.x134:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_120());
							break;
						case Effects.x135:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_035());
							break;
						case Effects.x136:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0DF());
							break;
						case Effects.x137:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_07F());
							break;
						case Effects.x138:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0CE());
							break;
						case Effects.x139:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_036());
							break;
						case Effects.x13A:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0EC());
							break;
						case Effects.x13B:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0F5());
							break;
						case Effects.x13C:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_11E());
							break;
						case Effects.x13D:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_028());
							break;
						case Effects.x13E:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_086());
							break;
						case Effects.x13F:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_062());
							break;
						case Effects.x140:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_085());
							break;
						case Effects.x141:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0E1());
							break;
						case Effects.x142:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_039());
							break;
						case Effects.x143:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_025());
							break;
						case Effects.x144:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0F3());
							break;
						case Effects.x145:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_108());
							break;
						case Effects.x146:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_107());
							break;
						case Effects.x147:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_106());
							break;
						case Effects.x148:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_027());
							break;
						case Effects.x149:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_038());
							break;
						case Effects.x14A:
							break;
						case Effects.x14B:
							//ToDo: Glaciate
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_044());
							break;
						case Effects.x14C:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0C5());
							break;
						case Effects.x14D:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0C6());
							break;
						case Effects.x14E:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_015());
							break;
						case Effects.x14F:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_03D());
							break;
						case Effects.x150:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_07A());
							break;
						case Effects.x151:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_079());
							break;
						case Effects.x152:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_144());
							break;
						case Effects.x153:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_158());
							break;
						case Effects.x154:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_13E());
							break;
						case Effects.x155:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_153());
							break;
						case Effects.x156:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_150());
							break;
						case Effects.x157:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_142());
							break;
						case Effects.x158:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_13A());
							break;
						case Effects.x159:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_146());
							break;
						case Effects.x15A:
							//ToDo: Parabolic charge has different effect
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0DD());
							break;
						case Effects.x15B:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_151());
							break;
						case Effects.x15C:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_141());
							break;
						case Effects.x15D:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_14F());
							break;
						case Effects.x15E:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_14A());
							break;
						case Effects.x15F:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_13F());
							break;
						case Effects.x160:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_155());
							break;
						case Effects.x161:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_156());
							break;
						case Effects.x162:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_145());
							break;
						case Effects.x163:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_152());
							break;
						case Effects.x164:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_14B());
							break;
						case Effects.x165:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_139());
							break;
						case Effects.x166:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_13C());
							break;
						case Effects.x167:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_136());
							break;
						case Effects.x168:
							//ToDo: I think the function both do the samething
							//MoveEffectData.Add(effect, new Combat.PokeBattle_Move_13B());
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_147());
							break;
						case Effects.x169:
							//ToDo: Water Shuriken has different effect
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0C0());
							break;
						case Effects.x16A:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_14C());
							break;
						case Effects.x16B:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_138());
							break;
						case Effects.x16C:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_140());
							break;
						case Effects.x16D:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_042());
							break;
						case Effects.x16E:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_14E());
							break;
						case Effects.x16F:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_137());
							break;
						case Effects.x170:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_157());
							break;
						case Effects.x171:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_154());
							break;
						case Effects.x172:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_134());
							break;
						case Effects.x173:
							break;
						case Effects.x174:
							break;
						case Effects.x175:
							//ToDo: Thousand arrows....
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_11C());
							break;
						case Effects.x176:
							//ToDo: Thousand wave has different effects
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0EF());
							break;
						case Effects.x177:
							break;
						case Effects.x178:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_143());
							break;
						case Effects.x179:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_149());
							break;
						case Effects.x17A:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_148());
							break;
						case Effects.x17B:
							break;
						case Effects.x17C:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_135());
							break;
						case Effects.x17D:
							//Disarming voice, inflices extra effects
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_0A5());
							break;
						case Effects.x17E:
							break;
						case Effects.x17F:
							break;
						case Effects.x180:
							break;
						case Effects.x181:
							break;
						case Effects.x182:
							break;
						case Effects.x183:
							break;
						case Effects.x184:
							break;
						case Effects.x185:
							break;
						case Effects.x186:
							break;
						case Effects.x187:
							break;
						case Effects.x188:
							break;
						case Effects.x189:
							break;
						case Effects.x18A:
							break;
						case Effects.x18B:
							break;
						case Effects.x18C:
							break;
						case Effects.x18D:
							break;
						case Effects.x18E:
							break;
						case Effects.x18F:
							break;
						case Effects.x190:
							break;
						case Effects.x191:
							break;
						case Effects.x192:
							break;
						case Effects.x193:
							break;
						case Effects.x194:
							break;
						case Effects.x195:
							break;
						case Effects.x196:
							break;
						case Effects.x197:
							break;
						case Effects.x198:
							break;
						case Effects.x199:
							break;
						case Effects.x19A:
							break;
						case Effects.x19B:
							break;
						case Effects.x19C:
							break;
						case Effects.x19D:
							break;
						case Effects.x19E:
							break;
						case Effects.x19F:
							break;
						case Effects.x1A0:
							break;
						case Effects.x1A1:
							break;
						case Effects.x1A2:
							break;
						case Effects.x1A3:
							break;
						case Effects.x1A4:
							break;
						case Effects.x711:
							//126	No effect.	Shadow Blast, Shadow Blitz, Shadow Break, Shadow Rave, Shadow Rush, Shadow Wave
							//Effect x711 is only used by Shadow Rush... the others just deal regular damage, with no additional effects 
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_126());
							break;
						case Effects.x712:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_130());
							break;
						case Effects.x713:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_12E());
							break;
						case Effects.x714:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_12C());
							break;
						case Effects.x715:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_132());
							break;
						case Effects.x716:
							MoveEffectData.Add(effect, new Combat.PokeBattle_Move_131());
							break;
						default:
							break;
					}
				}
				return true;
			} catch (Exception) {
				return false;
			}
		}
		static bool GetTrainersFromXML()
		{
			try
			{
				foreach (TrainerTypes type in Enum.GetValues(typeof(TrainerTypes)))
				{
					byte BaseMoney = 30;
					byte SkillLevel = 1;
					bool? gender = null;
					bool doubel = false;
					#region Gender & IsDouble
					switch (type)
					{
						case TrainerTypes.POKEMONTRAINER_Red:
						case TrainerTypes.POKEMONTRAINER_Leaf:
						case TrainerTypes.POKEMONTRAINER_Brendan:
						case TrainerTypes.RIVAL1:
						case TrainerTypes.RIVAL2:
						case TrainerTypes.BIKER:
						case TrainerTypes.BIRDKEEPER:
						case TrainerTypes.BUGCATCHER:
						case TrainerTypes.BURGLAR:
						case TrainerTypes.CHANELLER:
						case TrainerTypes.CUEBALL:
						case TrainerTypes.ENGINEER:
						case TrainerTypes.FISHERMAN:
						case TrainerTypes.GAMBLER:
						case TrainerTypes.GENTLEMAN:
						case TrainerTypes.HIKER:
						case TrainerTypes.JUGGLER:
						case TrainerTypes.PAINTER:
						case TrainerTypes.POKEMANIAC:
						case TrainerTypes.POKEMONBREEDER:
						case TrainerTypes.PROFESSOR:
						case TrainerTypes.ROCKER:
						case TrainerTypes.RUINMANIAC:
						case TrainerTypes.SAILOR:
						case TrainerTypes.SCIENTIST:
						case TrainerTypes.SUPERNERD:
						case TrainerTypes.TAMER:
						case TrainerTypes.BLACKBELT:
						case TrainerTypes.CAMPER:
						case TrainerTypes.PICNICKER:
						case TrainerTypes.COOLTRAINER_M:
						case TrainerTypes.YOUNGSTER:
						case TrainerTypes.POKEMONRANGER_M:
						case TrainerTypes.PSYCHIC_M:
						case TrainerTypes.SWIMMER_M:
						case TrainerTypes.SWIMMER2_M:
						case TrainerTypes.TUBER_M:
						case TrainerTypes.TUBER2_M:
						case TrainerTypes.CRUSHKIN:
						case TrainerTypes.TEAMROCKET_M:
						case TrainerTypes.ROCKETBOSS:
						case TrainerTypes.LEADER_Brock:
						case TrainerTypes.LEADER_Surge:
						case TrainerTypes.LEADER_Koga:
						case TrainerTypes.LEADER_Blaine:
						case TrainerTypes.LEADER_Giovanni:
						case TrainerTypes.ELITEFOUR_Bruno:
						case TrainerTypes.ELITEFOUR_Lance:
						case TrainerTypes.CHAMPION:
							gender = true;
							break;
						case TrainerTypes.POKEMONTRAINER_May:
						case TrainerTypes.AROMALADY:
						case TrainerTypes.BEAUTY:
						case TrainerTypes.LADY:
						case TrainerTypes.CRUSHGIRL:
						case TrainerTypes.COOLTRAINER_F:
						case TrainerTypes.LASS:
						case TrainerTypes.POKEMONRANGER_F:
						case TrainerTypes.PSYCHIC_F:
						case TrainerTypes.SWIMMER_F:
						case TrainerTypes.SWIMMER2_F:
						case TrainerTypes.TUBER_F:
						case TrainerTypes.TUBER2_F:
						case TrainerTypes.TEAMROCKET_F:
						case TrainerTypes.LEADER_Misty:
						case TrainerTypes.LEADER_Erika:
						case TrainerTypes.LEADER_Sabrina:
						case TrainerTypes.ELITEFOUR_Lorelei:
						case TrainerTypes.ELITEFOUR_Agatha:
							gender = false;
							break;
						case TrainerTypes.COOLCOUPLE:
						case TrainerTypes.SISANDBRO:
						case TrainerTypes.TWINS:
						case TrainerTypes.YOUNGCOUPLE:
							doubel = true;
							break;
						case TrainerTypes.WildPokemon:
						case TrainerTypes.PLAYER:
						default:
							break;
					}
					switch (type)
					{
						case TrainerTypes.BURGLAR:
						case TrainerTypes.GAMBLER:
							SkillLevel = 32;
							break;
						case TrainerTypes.LADY:
							SkillLevel = 72;
							break;
						case TrainerTypes.TUBER_M:
						case TrainerTypes.TUBER2_M:
						case TrainerTypes.SWIMMER_M:
						case TrainerTypes.SWIMMER2_M:
							SkillLevel = 32;
							break;
						case TrainerTypes.TUBER_F:
						case TrainerTypes.TUBER2_F:
						case TrainerTypes.SWIMMER_F:
						case TrainerTypes.SWIMMER2_F:
							break;
						case TrainerTypes.COOLCOUPLE:
						case TrainerTypes.SISANDBRO:
						case TrainerTypes.YOUNGCOUPLE:
							break;
						default:
							break;
					}
					#endregion
					#region Base Money
					switch (type)
					{
						case TrainerTypes.TUBER_M:
						case TrainerTypes.TUBER2_M:
						case TrainerTypes.TUBER_F:
						case TrainerTypes.TUBER2_F:
							BaseMoney = 4;
							break;
						case TrainerTypes.RIVAL1:
						case TrainerTypes.BUGCATCHER:
						case TrainerTypes.PAINTER:
						case TrainerTypes.CAMPER:
						case TrainerTypes.LASS:
						case TrainerTypes.PICNICKER:
						case TrainerTypes.SISANDBRO:
						case TrainerTypes.SWIMMER_M:
						case TrainerTypes.SWIMMER_F:
						case TrainerTypes.SWIMMER2_M:
						case TrainerTypes.SWIMMER2_F:
						case TrainerTypes.YOUNGSTER:
							BaseMoney = 16;
							break;
						case TrainerTypes.CUEBALL:
						case TrainerTypes.CRUSHGIRL:
						case TrainerTypes.ROCKER:
						case TrainerTypes.TWINS:
							BaseMoney = 24;
							break;
						case TrainerTypes.AROMALADY:
						case TrainerTypes.BIKER:
						case TrainerTypes.BIRDKEEPER:
						case TrainerTypes.BLACKBELT:
						case TrainerTypes.CHANELLER:
						case TrainerTypes.FISHERMAN:
						case TrainerTypes.HIKER:
						case TrainerTypes.JUGGLER:
						case TrainerTypes.PSYCHIC_M:
						case TrainerTypes.PSYCHIC_F:
						case TrainerTypes.SAILOR:
						case TrainerTypes.TEAMROCKET_M:
						case TrainerTypes.TEAMROCKET_F:
							BaseMoney = 32;
							break;
						case TrainerTypes.RIVAL2:
							BaseMoney = 36;
							break;
						case TrainerTypes.CRUSHKIN:
						case TrainerTypes.TAMER:
							BaseMoney = 48;
							break;
						case TrainerTypes.ENGINEER:
						case TrainerTypes.POKEMONBREEDER:
						case TrainerTypes.RUINMANIAC:
						case TrainerTypes.SCIENTIST:
						case TrainerTypes.SUPERNERD:
							BaseMoney = 48;
							break;
						case TrainerTypes.BEAUTY:
							BaseMoney = 56;
							break;
						case TrainerTypes.PLAYER:
						case TrainerTypes.POKEMONTRAINER_Red:
						case TrainerTypes.POKEMONTRAINER_Brendan:
						case TrainerTypes.POKEMONTRAINER_Leaf:
						case TrainerTypes.POKEMONTRAINER_May:
						case TrainerTypes.COOLTRAINER_M:
						case TrainerTypes.COOLTRAINER_F:
						case TrainerTypes.POKEMONRANGER_M:
						case TrainerTypes.POKEMONRANGER_F:
						case TrainerTypes.YOUNGCOUPLE:
							BaseMoney = 60;
							break;
						case TrainerTypes.POKEMANIAC:
							BaseMoney = 64;
							break;
						case TrainerTypes.COOLCOUPLE:
						case TrainerTypes.GAMBLER:
						case TrainerTypes.GENTLEMAN:
							BaseMoney = 72;
							break;
						case TrainerTypes.BURGLAR:
							BaseMoney = 88;
							break;
						case TrainerTypes.CHAMPION:
						case TrainerTypes.LEADER_Brock:
						case TrainerTypes.LEADER_Surge:
						case TrainerTypes.LEADER_Koga:
						case TrainerTypes.LEADER_Blaine:
						case TrainerTypes.LEADER_Giovanni:
						case TrainerTypes.LEADER_Misty:
						case TrainerTypes.LEADER_Erika:
						case TrainerTypes.LEADER_Sabrina:
						case TrainerTypes.ELITEFOUR_Lorelei:
						case TrainerTypes.ELITEFOUR_Agatha:
						case TrainerTypes.ELITEFOUR_Bruno:
						case TrainerTypes.ELITEFOUR_Lance:
						case TrainerTypes.ROCKETBOSS:
						case TrainerTypes.PROFESSOR:
							BaseMoney = 100;
							break;
						case TrainerTypes.LADY:
							BaseMoney = 160;
							break;
						default:
							BaseMoney = 30;
							break;
					}
					#endregion
					#region Skill Level
					switch (type)
					{
						case TrainerTypes.BURGLAR:
						case TrainerTypes.GAMBLER:
							SkillLevel = 32;
							break;
						case TrainerTypes.LADY:
							SkillLevel = 72;
							break;
						case TrainerTypes.SWIMMER_M:
						case TrainerTypes.SWIMMER_F:
						case TrainerTypes.SWIMMER2_M:
						case TrainerTypes.SWIMMER2_F:
							SkillLevel = 32;
							break;
						case TrainerTypes.TUBER_M:
						case TrainerTypes.TUBER_F:
						case TrainerTypes.TUBER2_M:
						case TrainerTypes.TUBER2_F:
							SkillLevel = 16;
							break;
						case TrainerTypes.COOLCOUPLE:
						case TrainerTypes.SISANDBRO:
							SkillLevel = 48;
							break;
						case TrainerTypes.YOUNGCOUPLE:
							SkillLevel = 32;
							break;
						default:
							SkillLevel = BaseMoney;
							break;
					}
					#endregion
					TrainerMetaData.Add(type, new TrainerMetaData(
							id: type
							, @double: doubel
							, baseMoney: BaseMoney
							, gender: gender
							, skillLevel: SkillLevel
							//, skillCodes: SkillLevel
						)
					);
				}
				return true;
			} catch (Exception) {
				return false;
			}
		}
		static bool GetGymBadgesFromXML()
		{
			try
			{
				var xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(LockFileStream(FilePokemonXML));

				var pokemons = xmlDocument.SelectNodes("/Pokemons");
				if (pokemons == null || pokemons.Count <= 0)
				{
					//throw new Exception("Xml must include "Pokemons" as root node.");
					GameDebug.LogError("Xml must include \"Pokemons\" as root node.");
					return false;
				}

				//var dublicateNames = new List<string>();

				var pokemonNodes = xmlDocument.SelectSingleNode("/Pokemons").ChildNodes;// /text
				if (pokemonNodes != null)
				{
					//foreach value in xml
					foreach (XmlNode node in pokemonNodes)
					{
						List<HiddenMoves> HMs = new List<HiddenMoves>();
						List<Moves> moves = new List<Moves>();

						GymBadges id = (GymBadges)int.Parse(node.GetAttributeValueOrNull("Id"));
						Regions region = (Regions)int.Parse(node.GetAttributeValueOrNull("region")); //Regions.NOT_IN_OVERWORLD;
						int level = int.Parse(node.GetAttributeValueOrNull("level"));

						string[] hms = node.GetAttributeValueOrNull("hm")?.Split(',');

						if (hms != null)
							foreach (string hm in hms)
							{
								switch (hm.ToLower())
								{
									case "surf":
									{
										HMs.Add(HiddenMoves.Surf);
										moves.Add(Moves.SURF);
										if(!HiddenMoveData.ContainsKey(HiddenMoves.Surf))
											HiddenMoveData.Add(HiddenMoves.Surf,
												new Character.HiddenMoveData(
													id: HiddenMoves.Surf,
													move: Moves.SURF,
													badge: (GymBadges)Core.BADGEFORSURF,
													badges: (byte)Core.BADGEFORSURF
												)
											);
										break;
									}
									case "cut":
									{
										HMs.Add(HiddenMoves.Cut);
										moves.Add(Moves.CUT);
										if(!HiddenMoveData.ContainsKey(HiddenMoves.Cut))
											HiddenMoveData.Add(HiddenMoves.Surf,
												new Character.HiddenMoveData(
													id: HiddenMoves.Cut,
													move: Moves.CUT,
													badge: (GymBadges)Core.BADGEFORCUT,
													badges: (byte)Core.BADGEFORCUT
												)
											);
										break;
									}
									case "strength":
									{
										HMs.Add(HiddenMoves.Strength);
										moves.Add(Moves.STRENGTH);
										if(!HiddenMoveData.ContainsKey(HiddenMoves.Strength))
											HiddenMoveData.Add(HiddenMoves.Strength,
												new Character.HiddenMoveData(
													id: HiddenMoves.Strength,
													move: Moves.STRENGTH,
													badge: (GymBadges)Core.BADGEFORSTRENGTH,
													badges: (byte)Core.BADGEFORSTRENGTH
												)
											);
										break;
									}
									case "flash":
									{
										HMs.Add(HiddenMoves.Flash);
										moves.Add(Moves.FLASH);
										if(!HiddenMoveData.ContainsKey(HiddenMoves.Flash))
											HiddenMoveData.Add(HiddenMoves.Flash,
												new Character.HiddenMoveData(
													id: HiddenMoves.Flash,
													move: Moves.FLASH,
													badge: (GymBadges)Core.BADGEFORFLASH,
													badges: (byte)Core.BADGEFORFLASH
												)
											);
										break;
									}
									case "fly":
									{
										HMs.Add(HiddenMoves.Fly);
										moves.Add(Moves.FLY);
										if(!HiddenMoveData.ContainsKey(HiddenMoves.Fly))
											HiddenMoveData.Add(HiddenMoves.Fly,
												new Character.HiddenMoveData(
													id: HiddenMoves.Fly,
													move: Moves.FLY,
													badge: (GymBadges)Core.BADGEFORFLY,
													badges: (byte)Core.BADGEFORFLY
												)
											);
										break;
									}
									case "whirlpool":
									{
										HMs.Add(HiddenMoves.Whirlpool);
										moves.Add(Moves.WHIRLPOOL);
										if(!HiddenMoveData.ContainsKey(HiddenMoves.Whirlpool))
											HiddenMoveData.Add(HiddenMoves.Whirlpool,
												new Character.HiddenMoveData(
													id: HiddenMoves.Whirlpool,
													move: Moves.WHIRLPOOL,
													badge: 0,
													badges: 0
												)
											);
										break;
									}
									case "waterfall":
									{
										HMs.Add(HiddenMoves.Waterfall);
										moves.Add(Moves.WATERFALL);
										if(!HiddenMoveData.ContainsKey(HiddenMoves.Waterfall))
											HiddenMoveData.Add(HiddenMoves.Waterfall,
												new Character.HiddenMoveData(
													id: HiddenMoves.Waterfall,
													move: Moves.WATERFALL,
													badge: (GymBadges)Core.BADGEFORWATERFALL,
													badges: (byte)Core.BADGEFORWATERFALL
												)
											);
										break;
									}
									case "ride":
									{
										HMs.Add(HiddenMoves.Ride);
										if(!HiddenMoveData.ContainsKey(HiddenMoves.Ride))
											HiddenMoveData.Add(HiddenMoves.Ride,
												new Character.HiddenMoveData(
													id: HiddenMoves.Ride,
													move: Moves.NONE,
													badge: 0,
													badges: 0
												)
											);
										break;
									}
									case "dive":
									{
										HMs.Add(HiddenMoves.Dive);
										moves.Add(Moves.DIVE);
										if(!HiddenMoveData.ContainsKey(HiddenMoves.Dive))
											HiddenMoveData.Add(HiddenMoves.Dive,
												new Character.HiddenMoveData(
													id: HiddenMoves.Dive,
													move: Moves.DIVE,
													badge: (GymBadges)Core.BADGEFORDIVE,
													badges: (byte)Core.BADGEFORDIVE
												)
											);
										break;
									}
									case "rockclimb":
									{
										HMs.Add(HiddenMoves.RockClimb);
										moves.Add(Moves.ROCK_CLIMB);
										if(!HiddenMoveData.ContainsKey(HiddenMoves.RockClimb))
											HiddenMoveData.Add(HiddenMoves.RockClimb,
												new Character.HiddenMoveData(
													id: HiddenMoves.RockClimb,
													move: Moves.ROCK_CLIMB,
													badge: 0,
													badges: 0
												)
											);
										break;
									}
									case "rocksmash":
									{
										HMs.Add(HiddenMoves.RockSmash);
										moves.Add(Moves.ROCK_SMASH);
										if(!HiddenMoveData.ContainsKey(HiddenMoves.RockSmash))
											HiddenMoveData.Add(HiddenMoves.RockSmash,
												new Character.HiddenMoveData(
													id: HiddenMoves.RockSmash,
													move: Moves.ROCK_SMASH,
													badge: (GymBadges)Core.BADGEFORROCKSMASH,
													badges: (byte)Core.BADGEFORROCKSMASH
												)
											);
										break;
									}
									case "headbutt":
									{
										HMs.Add(HiddenMoves.Headbutt);
										moves.Add(Moves.HEADBUTT);
										if(!HiddenMoveData.ContainsKey(HiddenMoves.Headbutt))
											HiddenMoveData.Add(HiddenMoves.Headbutt,
												new Character.HiddenMoveData(
													id: HiddenMoves.Headbutt,
													move: Moves.HEADBUTT,
													badge: 0,
													badges: 0
												)
											);
										break;
									}
									case "defog":
									{
										HMs.Add(HiddenMoves.Defog);
										moves.Add(Moves.DEFOG);
										if(!HiddenMoveData.ContainsKey(HiddenMoves.Defog))
											HiddenMoveData.Add(HiddenMoves.Defog,
												new Character.HiddenMoveData(
													id: HiddenMoves.Defog,
													move: Moves.DEFOG,
													badge: 0,
													badges: 0
												)
											);
										break;
									}
								}
							}
						BadgeData.Add(id, new Character.BadgeData(id, region, (byte)level));
					}
				}
				return true;
			} catch (Exception) {
				return false;
			}
		}
		#endregion
		#region From SQL
		static bool GetPokemonsFromSQL(SQLiteConnection con)
		{
			OnLoadEventArgs onLoad = new OnLoadEventArgs { Check = 0 };
			try
			{
				//for(int n = 1; n <= Enum.GetValues(typeof(Pokemons)).Length; n++)
				//{
					//Step 3: Running a Command
					SQLiteCommand stmt = con.CreateCommand();

					#region DataReader
					stmt.CommandText = "select COUNT(*) from pokemon_views;select * from pokemon_views --order by id ASC";
					//	@"select pokemon.id, pokemon.species_id, pokemon.identifier, pokemon.height, pokemon.weight, pokemon.base_experience, --pokemon.""order""
					//pokemon_abilities_view.ability1, pokemon_abilities_view.ability2, pokemon_abilities_view.ability3,
					//pokemon_egg_groups_view.egg_group1, pokemon_egg_groups_view.egg_group2,
					//pokemon_stats_view.bhp, pokemon_stats_view.batk, pokemon_stats_view.bdef, pokemon_stats_view.bspa, pokemon_stats_view.bspd, pokemon_stats_view.bspe, pokemon_stats_view.ehp, pokemon_stats_view.eatk, pokemon_stats_view.edef, pokemon_stats_view.espa, pokemon_stats_view.espd, pokemon_stats_view.espe,
					//pokemon_types_view.type1, pokemon_types_view.type2,
					//pokemon_color_names.name as color,
					//pokemon_species.generation_id, pokemon_species.evolves_from_species_id, pokemon_species.evolution_chain_id, pokemon_species.color_id, pokemon_species.shape_id, pokemon_species.habitat_id, pokemon_species.gender_rate, pokemon_species.capture_rate, pokemon_species.base_happiness, pokemon_species.is_baby, pokemon_species.hatch_counter, pokemon_species.has_gender_differences, pokemon_species.growth_rate_id, pokemon_species.forms_switchable, pokemon_species.""order"",
					//evolution_chains.baby_trigger_item_id as incense,
					//pokemon_species_names.name,pokemon_species_names.genus,
					//pokemon_species_flavor_text.flavor_text
					//from pokemon
					//left join pokemon_abilities_view on pokemon.id = pokemon_abilities_view.pokemon_id
					//left join pokemon_egg_groups_view on pokemon_egg_groups_view.pokemon_id = pokemon.id
					//left join pokemon_stats_view on pokemon_stats_view.pokemon_id = pokemon.id
					//left join pokemon_types_view on pokemon_types_view.pokemon_id = pokemon.id
					//left join pokemon_species on pokemon_species.id = pokemon.species_id
					//left join evolution_chains on evolution_chains.id = pokemon_species.evolution_chain_id
					//left join pokemon_colors on pokemon_colors.id = pokemon_species.color_id
					//left join pokemon_color_names on pokemon_color_names.pokemon_color_id = pokemon_colors.id AND pokemon_color_names.local_language_id = 9
					//left join pokemon_species_names on pokemon_species_names.pokemon_species_id = pokemon.id AND pokemon_species_names.local_language_id = 9
					//left join pokemon_species_flavor_text on pokemon_species_flavor_text.species_id = pokemon.id AND pokemon_species_flavor_text.version_id = 26 AND pokemon_species_flavor_text.language_id = 9
					//order by pokemon.id ASC;";
					SQLiteDataReader reader = stmt.ExecuteReader();

					//Step 4: Read the results
					using(reader)
					{
						if (OnLoad != null) { onLoad.TotalPieces = (int)reader[0]; OnLoad.Invoke(Game.GameData, onLoad); } reader.NextResult();
						PokemonData.Add(Pokemons.NONE, new Monster.Data.PokemonData(Id:Pokemons.NONE, hatchTime: 1000));
						while(reader.Read()) //if(reader.Read())
						{
							if (OnLoad != null) { onLoad.Piece = reader.StepCount; OnLoad.Invoke(Game.GameData, onLoad); }
							PokemonData.Add((Pokemons)int.Parse((string)reader["id"].ToString()),
								new PokemonUnity.Monster.Data.PokemonData(
									Id: (Pokemons)int.Parse((string)reader["id"].ToString())
									,baseForm: (Forms)int.Parse((string)reader["species_id"].ToString())
									//,regionalDex: new int[] { int.Parse((string)reader["ReDex"].ToString()) }
									,type1: (Types)int.Parse(string.IsNullOrEmpty((string)reader["type1"].ToString()) ? "0" : (string)reader["type1"].ToString())
									,type2: (Types)int.Parse(string.IsNullOrEmpty((string)reader["type2"].ToString()) ? "0" : (string)reader["type2"].ToString())
									,ability1: (Abilities)int.Parse(string.IsNullOrEmpty((string)reader["ability1"].ToString()) ? "0" : (string)reader["ability1"].ToString())
									,ability2: (Abilities)int.Parse(string.IsNullOrEmpty((string)reader["ability2"].ToString()) ? "0" : (string)reader["ability2"].ToString())
									,hiddenAbility: (Abilities)int.Parse(string.IsNullOrEmpty((string)reader["ability3"].ToString()) ? "0" : (string)reader["ability3"].ToString())
									,genderRatio: (PokemonUnity.Monster.GenderRatio)int.Parse(string.IsNullOrEmpty((string)reader["gender_rate"].ToString()) ? "0" : (string)reader["gender_rate"].ToString())
									//,maleRatio: float.Parse((string)reader["Gender"].ToString())
									,catchRate: int.Parse(string.IsNullOrEmpty((string)reader["capture_rate"].ToString()) ? "0" : (string)reader["capture_rate"].ToString())
									,eggGroup1: (PokemonUnity.Monster.EggGroups)int.Parse(string.IsNullOrEmpty((string)reader["egg_group1"].ToString()) ? "0" : (string)reader["egg_group1"].ToString())
									,eggGroup2: (PokemonUnity.Monster.EggGroups)int.Parse(string.IsNullOrEmpty((string)reader["egg_group2"].ToString()) ? "0" : (string)reader["egg_group2"].ToString())
									,hatchTime: int.Parse(string.IsNullOrEmpty((string)reader["hatch_counter"].ToString()) ? "0" : (string)reader["hatch_counter"].ToString())
									,height: float.Parse((string)reader["height"].ToString())
									,weight: float.Parse((string)reader["weight"].ToString())
									,baseExpYield: int.Parse(string.IsNullOrEmpty((string)reader["base_experience"].ToString()) ? "0" : (string)reader["base_experience"].ToString())
									,levelingRate: (PokemonUnity.Monster.LevelingRate)int.Parse(string.IsNullOrEmpty((string)reader["growth_rate_id"].ToString()) ? "0" : (string)reader["growth_rate_id"].ToString())
									,evHP:  int.Parse((string)reader["ehp"].ToString())
									,evATK: int.Parse((string)reader["eatk"].ToString())
									,evDEF: int.Parse((string)reader["edef"].ToString())
									,evSPA: int.Parse((string)reader["espa"].ToString())
									,evSPD: int.Parse((string)reader["espd"].ToString())
									,evSPE: int.Parse((string)reader["espe"].ToString())
									,pokedexColor: (Color)int.Parse(string.IsNullOrEmpty((string)reader["color_id"].ToString()) ? "0" : (string)reader["color_id"].ToString())
									,baseFriendship: int.Parse(string.IsNullOrEmpty((string)reader["base_happiness"].ToString()) ? "0" : (string)reader["base_happiness"].ToString())
									,baseStatsHP:  int.Parse((string)reader["bhp"].ToString())
									,baseStatsATK: int.Parse((string)reader["batk"].ToString())
									,baseStatsDEF: int.Parse((string)reader["bdef"].ToString())
									,baseStatsSPA: int.Parse((string)reader["bspa"].ToString())
									,baseStatsSPD: int.Parse((string)reader["bspd"].ToString())
									,baseStatsSPE: int.Parse((string)reader["bspe"].ToString())
									//,rarity: (Rarity)int.Parse((string)reader["Rarity"].ToString())
									//,luminance: Data.luminance
									//,movesetmoves: Data.movesetmoves
									//,movesetLevels: Data.movesetLevels
									//,movesetMoves: Data.movesetMoves
									//,tmList: Data.tmList
									//,evolution: Data.evolution
									//,evolutionID: Data.EvolutionTO
									//,evolutionLevel: Data.evolutionLevel
									//,evolutionMethod: Data.evolutionMethod
									//,evolutionFROM: Data.EvolutionFROM
									//,evolutionTO: Data.EvolutionTO
									,incense: (Items)int.Parse(string.IsNullOrEmpty((string)reader["incense"].ToString()) ? "0" : (string)reader["incense"].ToString())
									,evoChainId: int.Parse(string.IsNullOrEmpty((string)reader["evolution_chain_id"].ToString()) ? "0" : (string)reader["evolution_chain_id"].ToString())
									,generationId: (byte)int.Parse(string.IsNullOrEmpty((string)reader["generation_id"].ToString()) ? "0" : (string)reader["generation_id"].ToString())
									//,isDefault: (string)reader["IsBaseForm"].ToString() == "1"
									,isBaby: (string)reader["is_baby"].ToString() == "1"
									,formSwitchable: (string)reader["forms_switchable"].ToString() == "1"
									,hasGenderDiff: (string)reader["has_gender_differences"].ToString() == "1"
									,habitatId: (PokemonUnity.Monster.Habitat)int.Parse(string.IsNullOrEmpty((string)reader["habitat_id"].ToString()) ? "0" : (string)reader["habitat_id"].ToString())
									,shapeId: (PokemonUnity.Monster.Shape)int.Parse(string.IsNullOrEmpty((string)reader["shape_id"].ToString()) ? "0" : (string)reader["shape_id"].ToString())
									//,heldItem: Data.heldItem
								)
							);
							//foreach(var gen in new int[] { int.Parse((string)reader["ReDex"].ToString()) }) { PokedexData.Add(); }
					}
					//}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
				}
				return true;
			} catch (SQLiteException e) {
				//GameDebug.LogError("SQL Exception Message:" + e.Message);
				//GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				//GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				GameDebug.LogError($"Failed to Load ID #{onLoad.Piece} from DB\nSQL Exception Message:" + e.Message);
				return false;
			}
		}
		static bool GetPokemonMovesFromSQL(SQLiteConnection con)
		{
			try
			{
				//for(int n = 1; n <= Enum.GetValues(typeof(Pokemons)).Length; n++)
				//{
					//Step 3: Running a Command
					SQLiteCommand stmt = con.CreateCommand();

					#region DataReader
					stmt.CommandText = "select distinct move_id, pokemon_id, [level], pokemon_move_method_id from pokemon_moves --where version_group_id=18 order by pokemon_id ASC";
					//	@"select pokemon_moves.pokemon_id, pokemon_moves.move_id, pokemon_moves.level, pokemon_moves.pokemon_move_method_id, pokemon_moves."order", pokemon_moves.version_group_id,
					//pokemon_move_methods.identifier
					//from pokemon_moves
					//left join pokemon_move_methods on pokemon_move_methods.id=pokemon_moves.pokemon_move_method_id
					//where version_group_id=18
					//--order by pokemon_id ASC";
					SQLiteDataReader reader = stmt.ExecuteReader();

					//Step 4: Read the results
					using(reader)
					{
						Dictionary<Pokemons, List<Monster.Data.PokemonMoveset>> p = new Dictionary<Pokemons, List<Monster.Data.PokemonMoveset>>();
						while(reader.Read()) //if(reader.Read())
						{
							if (!p.ContainsKey((Pokemons)int.Parse((string)reader["pokemon_id"].ToString())))
								p.Add((Pokemons)int.Parse((string)reader["pokemon_id"].ToString()),
									new List<Monster.Data.PokemonMoveset>());
							p[(Pokemons)int.Parse((string)reader["pokemon_id"].ToString())].Add(
								new PokemonUnity.Monster.Data.PokemonMoveset(
									moveId: (Moves)int.Parse((string)reader["move_id"].ToString())
									//pokemonId: (Pokemons)int.Parse((string)reader["pokemon_id"].ToString())
									//,generation: int.Parse((string)reader["version_group_id"].ToString())
									,method: (LearnMethod)int.Parse((string)reader["pokemon_move_method_id"].ToString())
									,level: int.Parse((string)reader["level"].ToString())
									//,order: int.Parse((string)reader["order"].ToString())
								)
							);
						}
					//}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
					PokemonMovesData.Add(Pokemons.NONE, new Monster.Data.PokemonMoveTree(new Monster.Data.PokemonMoveset[] { }));
					foreach (var pkmn in p)
					{
						PokemonMovesData.Add(pkmn.Key, new Monster.Data.PokemonMoveTree(pkmn.Value.ToArray()));
					}
				}
				return true;
			} catch (SQLiteException e) {
				GameDebug.LogError("SQL Exception Message:" + e.Message);
				GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetPokemonFormsFromSQL(SQLiteConnection con)
		{
			try
			{
				Dictionary<Pokemons, List<Monster.Data.Form>> p = new Dictionary<Pokemons, List<Monster.Data.Form>>();
				foreach (Pokemons x in PokemonData.Keys)//for(int n = 1; n <= PokemonData.Keys.Length; n++)
				{
					p.Add(x, new List<Monster.Data.Form>());
				}
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = @"select pokemon_forms.id, pokemon_forms.form_identifier, pokemon_forms.is_default, pokemon_forms.is_battle_only, pokemon_forms.is_mega, pokemon_forms.form_order, pokemon_forms.""order"",
				pokemon.id as pokemon_id, pokemon.species_id as species_id--, pokemon.identifier, pokemon.height, pokemon.weight, pokemon.base_experience, --pokemon.""order""
				--pokemon_stats_view.bhp, pokemon_stats_view.batk, pokemon_stats_view.bdef, pokemon_stats_view.bspa, pokemon_stats_view.bspd, pokemon_stats_view.bspe, pokemon_stats_view.ehp, pokemon_stats_view.eatk, pokemon_stats_view.edef, pokemon_stats_view.espa, pokemon_stats_view.espd, pokemon_stats_view.espe,
				--pokemon_species.generation_id, pokemon_species.evolves_from_species_id, pokemon_species.evolution_chain_id, pokemon_species.color_id, pokemon_species.shape_id, pokemon_species.habitat_id, pokemon_species.gender_rate, pokemon_species.capture_rate, pokemon_species.base_happiness, pokemon_species.is_baby, pokemon_species.hatch_counter, pokemon_species.has_gender_differences, pokemon_species.growth_rate_id, pokemon_species.forms_switchable, pokemon_species.""order""
				from pokemon
				left join pokemon_forms on pokemon_forms.pokemon_id = pokemon.id
				left join pokemon_stats_view on pokemon_stats_view.pokemon_id = pokemon.id
				left join pokemon_species on pokemon_species.id = pokemon.species_id
				--where --pokemon_species.id != pokemon.id AND
				--pokemon_forms.identifier != pokemon.identifier OR
				--pokemon_forms.id != pokemon.id
				--pokemon_forms.id != pokemon_species.id
				order by pokemon.species_id, pokemon_forms.form_order ASC;";
				//	@"select pokemon_forms.id, pokemon_forms.identifier,
				//pokemon.id, pokemon.species_id, pokemon.identifier, pokemon.height, pokemon.weight, pokemon.base_experience, --pokemon."order"
				//pokemon_stats_view.bhp, pokemon_stats_view.batk, pokemon_stats_view.bdef, pokemon_stats_view.bspa, pokemon_stats_view.bspd, pokemon_stats_view.bspe, pokemon_stats_view.ehp, pokemon_stats_view.eatk, pokemon_stats_view.edef, pokemon_stats_view.espa, pokemon_stats_view.espd, pokemon_stats_view.espe,
				//pokemon_species.generation_id, pokemon_species.evolves_from_species_id, pokemon_species.evolution_chain_id, pokemon_species.color_id, pokemon_species.shape_id, pokemon_species.habitat_id, pokemon_species.gender_rate, pokemon_species.capture_rate, pokemon_species.base_happiness, pokemon_species.is_baby, pokemon_species.hatch_counter, pokemon_species.has_gender_differences, pokemon_species.growth_rate_id, pokemon_species.forms_switchable, pokemon_species."order"
				//from pokemon
				//left join pokemon_forms on pokemon_forms.pokemon_id = pokemon.id
				//left join pokemon_stats_view on pokemon_stats_view.pokemon_id = pokemon.id
				//left join pokemon_species on pokemon_species.id = pokemon.species_id
				//left join pokemon_species_names on pokemon_species_names.pokemon_species_id = pokemon.species_id AND pokemon_species_names.local_language_id=9
				//where --pokemon_species.id != pokemon.id AND
				//pokemon_forms.identifier != pokemon.identifier OR
				//--pokemon_forms.id != pokemon.id
				//pokemon_forms.id != pokemon_species.id
				//order by pokemon.species_id, pokemon_forms.id ASC;";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					while(reader.Read()) //if(reader.Read())
					{
						//if (!p.ContainsKey((Pokemons)int.Parse((string)reader["pokemon_id"].ToString())))
						//	p.Add((Pokemons)int.Parse((string)reader["pokemon_id"].ToString()),
						//		new List<Monster.Data.Form>());
						p[(Pokemons)int.Parse((string)reader["species_id"].ToString())].Add(
							new PokemonUnity.Monster.Data.Form(
								id: (Forms)int.Parse((string)reader["id"].ToString())
								,pkmn: (Pokemons)int.Parse((string)reader["pokemon_id"].ToString())
								,species: (Pokemons)int.Parse((string)reader["species_id"].ToString())
								,identifier: (string)reader["form_identifier"].ToString()
								,isMega: (string)reader["is_mega"].ToString() == "1"
								,isDefault: (string)reader["is_default"].ToString() == "1"
								,isBattleOnly: (string)reader["is_battle_only"].ToString() == "1"
								,formOrder: (byte)int.Parse((string)reader["form_order"].ToString())
								,order: int.Parse((string)reader["order"].ToString())
							)
						);
						if (int.Parse((string)reader["pokemon_id"].ToString()) != int.Parse((string)reader["species_id"].ToString()))
							p[(Pokemons)int.Parse((string)reader["pokemon_id"].ToString())].Add(
								new PokemonUnity.Monster.Data.Form(
									id: (Forms)int.Parse((string)reader["id"].ToString())
									,pkmn: (Pokemons)int.Parse((string)reader["pokemon_id"].ToString())
									,species: (Pokemons)int.Parse((string)reader["species_id"].ToString())
									,identifier: (string)reader["form_identifier"].ToString()
									,isMega: (string)reader["is_mega"].ToString() == "1"
									,isDefault: (string)reader["is_default"].ToString() == "1"
									,isBattleOnly: (string)reader["is_battle_only"].ToString() == "1"
									,formOrder: (byte)int.Parse((string)reader["form_order"].ToString())
									,order: int.Parse((string)reader["order"].ToString())
								)
							);
					}
				}
				//Step 5: Closing up
				reader.Close();
				reader.Dispose();
				#endregion
				foreach (var pkmn in p)
				{
					PokemonFormsData.Add(pkmn.Key, pkmn.Value
						//.OrderBy(x => x.FormOrder)
						.ToArray());
					//if (PokemonFormsData[pkmn.Value[0].Pokemon].Length == 0)
					if (pkmn.Key == Pokemons.NONE)// && pkmn.Value[0].Pokemon != pkmn.Value[0].Base)
						PokemonFormsData[Pokemons.NONE] = new Monster.Data.Form[] { new Monster.Data.Form(Forms.NONE, Pokemons.NONE, Pokemons.NONE) };
					else if (pkmn.Key != Pokemons.NONE && pkmn.Value[0].Pokemon != pkmn.Value[0].Base)
						PokemonFormsData[pkmn.Key] = PokemonFormsData[pkmn.Value[0].Base];
				}
				return true;
			} catch (SQLiteException e) {
				GameDebug.LogError("SQL Exception Message:" + e.Message);
				GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetPokemonEvolutionsFromSQL(SQLiteConnection con)
		{
			try
			{
				Dictionary<Pokemons, List<Monster.Data.EvolutionTrigger>> p = new Dictionary<Pokemons, List<Monster.Data.EvolutionTrigger>>();
				foreach (Pokemons x in PokemonData.Keys)//for(int n = 1; n <= Enum.GetValues(typeof(Pokemons)).Length; n++)
				{
					//p.Add(x, new List<Monster.Data.EvolutionTrigger>());
					PokemonEvolutionsData[x] = new Monster.Data.PokemonEvolution[0];
				}
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = //"select * from pokemon_evolution_view";
					@"select pokemon_species.id, pokemon_species.identifier, pokemon_species.generation_id, pokemon_species.evolves_from_species_id, pokemon_species.evolution_chain_id, pokemon_species.""order"",
				pokemon_evolution.evolution_trigger_id, pokemon_evolution.trigger_item_id, pokemon_evolution.minimum_level, pokemon_evolution.gender_id, pokemon_evolution.location_id, pokemon_evolution.held_item_id, pokemon_evolution.time_of_day, pokemon_evolution.known_move_id, pokemon_evolution.known_move_type_id, pokemon_evolution.minimum_happiness, pokemon_evolution.minimum_beauty, pokemon_evolution.minimum_affection, pokemon_evolution.relative_physical_stats, pokemon_evolution.party_species_id, pokemon_evolution.party_type_id, pokemon_evolution.trade_species_id, pokemon_evolution.needs_overworld_rain, pokemon_evolution.turn_upside_down
				from pokemon_evolution
				left join pokemon_species on pokemon_evolution.evolved_species_id = pokemon_species.id
				--group by pokemon_species.""order"" --pokemon_species.id --pokemon_species.evolution_chain_id
				order by pokemon_species.evolution_chain_id, pokemon_species.id;
				--pokemon_species.evolves_from_species_id, pokemon_species.""order"";";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					while(reader.Read()) //if(reader.Read())
					{
						Pokemons id = (Pokemons)int.Parse((string)reader["evolves_from_species_id"].ToString());
						if (!p.ContainsKey(id))
							p.Add(id, new List<Monster.Data.EvolutionTrigger>());
						//if list.conains evolve id && evolve by level && (location id == null || gender id == null || )
						p[id].Add(
							new PokemonUnity.Monster.Data.EvolutionTrigger(
								species: (Pokemons)int.Parse((string)reader["id"].ToString())
								,evo: (Monster.Data.EvoTrigger)int.Parse((string)reader["evolution_trigger_id"].ToString())
								,trigger: string.IsNullOrEmpty((string)reader["trigger_item_id"].ToString())		? Items.NONE : (Items)int.Parse((string)reader["trigger_item_id"].ToString())
								,minLevel:string.IsNullOrEmpty((string)reader["minimum_level"].ToString())			? (int?)null : int.Parse((string)reader["minimum_level"].ToString())
								,gender: string.IsNullOrEmpty((string)reader["gender_id"].ToString())				? (bool?)null : (bool)((string)reader["gender_id"].ToString() == "1")
								,location:string.IsNullOrEmpty((string)reader["location_id"].ToString())			? (int?)null : int.Parse((string)reader["location_id"].ToString())
								,held: string.IsNullOrEmpty((string)reader["held_item_id"].ToString())				? Items.NONE : (Items)int.Parse((string)reader["held_item_id"].ToString())
								,time:string.IsNullOrEmpty((string)reader["time_of_day"].ToString())				? (int?)null : ((string)reader["time_of_day"].ToString() == "day" ? 1 : 0) //int.Parse((string)reader["time_of_day"].ToString())
								,knownMove: string.IsNullOrEmpty((string)reader["known_move_id"].ToString())		? Moves.NONE: (Moves)int.Parse((string)reader["known_move_id"].ToString())
								,knownType: string.IsNullOrEmpty((string)reader["known_move_type_id"].ToString())	? Types.NONE : (Types)int.Parse((string)reader["known_move_type_id"].ToString())
								,happiness:string.IsNullOrEmpty((string)reader["minimum_happiness"].ToString())		? (int?)null : int.Parse((string)reader["minimum_happiness"].ToString())
								,beauty:string.IsNullOrEmpty((string)reader["minimum_beauty"].ToString())			? (int?)null : int.Parse((string)reader["minimum_beauty"].ToString())
								,affection:string.IsNullOrEmpty((string)reader["minimum_affection"].ToString())		? (int?)null : int.Parse((string)reader["minimum_affection"].ToString())
								,physicalStat:string.IsNullOrEmpty((string)reader["relative_physical_stats"].ToString()) ? (int?)null : int.Parse((string)reader["relative_physical_stats"].ToString())
								,partySpecies: string.IsNullOrEmpty((string)reader["party_species_id"].ToString())	? Pokemons.NONE : (Pokemons)int.Parse((string)reader["party_species_id"].ToString())
								,partyType: string.IsNullOrEmpty((string)reader["party_type_id"].ToString())		? Types.NONE : (Types)int.Parse((string)reader["party_type_id"].ToString())
								,tradeSpecies: string.IsNullOrEmpty((string)reader["trade_species_id"].ToString())	? Pokemons.NONE : (Pokemons)int.Parse((string)reader["trade_species_id"].ToString())
								,overworldRain: (string)reader["needs_overworld_rain"].ToString() == "1"
								,turnUpsideDown: (string)reader["turn_upside_down"].ToString() == "1"
							)
						);
					}
				}
				//Step 5: Closing up
				reader.Close();
				reader.Dispose();
				#endregion
				//PokemonEvolutionsData.Add(Pokemons.NONE, new Monster.Data.PokemonEvolution[] { });
				foreach (var pkmn in p)
				{
					//ToDo: Fix bad duplicates
					//if (pkmn.Key == Pokemons.ROCKRUFF)
					//{
					//	PokemonEvolutionsData[pkmn.Key] = new Monster.Data.PokemonEvolution[pkmn.Value.Count+1];
					//	PokemonEvolutionsData[pkmn.Key][2] = new Monster.Data.PokemonEvolution(pkmn.Value[2].Species, EvolutionMethod.Lycanroc, pkmn.Value[2].MinLevel);
					//}
					//else if (pkmn.Key == Pokemons.MAGNETON || pkmn.Key == Pokemons.NOSEPASS)// || pkmn.Key == Pokemons.ESPURR
					//	PokemonEvolutionsData[pkmn.Key] = new Monster.Data.PokemonEvolution[pkmn.Value.Count-1];
					//else if (pkmn.Key == Pokemons.EEVEE)
					//	PokemonEvolutionsData[pkmn.Key] = new Monster.Data.PokemonEvolution[pkmn.Value.Count-2];
					//else
						PokemonEvolutionsData[pkmn.Key] = new Monster.Data.PokemonEvolution[pkmn.Value.Count];
					for(int i = 0; i < pkmn.Value.Count; i++)
					{
						//if (!pkmn.Value[i].Location.HasValue && (pkmn.Key == Pokemons.MAGNETON || pkmn.Key == Pokemons.NOSEPASS || pkmn.Value[i].Species == Pokemons.LEAFEON || pkmn.Value[i].Species == Pokemons.GLACEON))// || pkmn.Key == Pokemons.ESPURR
						//	continue;
						#region Happiness Evolution
						if (pkmn.Value[i].Time.HasValue && pkmn.Value[i].Time.Value == 0 //"night"
							&& pkmn.Value[i].Happiness.HasValue)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.HappinessNight, pkmn.Value[i].Happiness.Value);
							continue;
						}
						else if (pkmn.Value[i].Time.HasValue && pkmn.Value[i].Time.Value == 1 //"day"
								&& pkmn.Value[i].Happiness.HasValue)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.HappinessDay, pkmn.Value[i].Happiness.Value);
							continue;
						}
						else if (pkmn.Value[i].Happiness.HasValue)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Happiness, pkmn.Value[i].Happiness.Value);
							continue;
						}
						#endregion

						#region Item Evolution
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.use_item && pkmn.Value[i].Gender.HasValue && pkmn.Value[i].Gender.Value == true //"1"
							&& pkmn.Value[i].Trigger != Items.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.ItemFemale, pkmn.Value[i].Trigger);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.use_item && pkmn.Value[i].Gender.HasValue && pkmn.Value[i].Gender.Value == false //"2"
							&& pkmn.Value[i].Trigger != Items.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.ItemMale, pkmn.Value[i].Trigger);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.use_item && pkmn.Value[i].Trigger != Items.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Item, pkmn.Value[i].Trigger);
							continue;
						}
						#endregion

						#region Location Evolution
						if (pkmn.Value[i].Location.HasValue)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Location, pkmn.Value[i].Location.Value);
							continue;
						}
						#endregion

						#region Trade Evolution
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.trade && pkmn.Value[i].TradeSpecies != Pokemons.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.TradeSpecies, pkmn.Value[i].TradeSpecies);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.trade && pkmn.Value[i].Held != Items.NONE && pkmn.Value[i].TradeSpecies == Pokemons.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.TradeItem, pkmn.Value[i].Held);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.trade && pkmn.Value[i].Held == Items.NONE && pkmn.Value[i].TradeSpecies == Pokemons.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Trade, null);
							continue;
						}
						#endregion

						#region Hold Item
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Time.HasValue && pkmn.Value[i].Time.Value == 1 //"day"
								&& pkmn.Value[i].Held != Items.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.HoldItemDay, pkmn.Value[i].Held);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Time.HasValue && pkmn.Value[i].Time.Value == 0 //"night"
							&& pkmn.Value[i].Held != Items.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.HoldItemNight, pkmn.Value[i].Held);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Held != Items.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.HoldItem, pkmn.Value[i].Held);
							continue;
						}
						#endregion

						#region Beauty/Affection
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Beauty.HasValue)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Beauty, pkmn.Value[i].Beauty.Value);
							continue;
						}
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Affection.HasValue)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Affection, pkmn.Value[i].KnownType);
							continue;
						}
						#endregion

						#region Move
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].KnownMove != Moves.NONE && pkmn.Value[i].PartySpecies == Pokemons.NONE)
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Moves>(Pokemons.{name.ToUpper()}, EvolutionMethod.Move, Moves.{tempMove.ToUpper()}),";
							//break;
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Move, pkmn.Value[i].KnownMove);
							continue;
						}
						#endregion

						#region Party
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].PartySpecies != Pokemons.NONE)
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Pokemons>(Pokemons.{name.ToUpper()}, EvolutionMethod.Party, Pokemons.{pkmn.Value[i].Species.ToUpper()}),";
							//break;
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Party, pkmn.Value[i].PartySpecies);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].KnownType != Types.NONE)
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Types>(Pokemons.{name.ToUpper()}, EvolutionMethod.Move, Types.{tempType.ToUpper()}),";
							//break;
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Type, pkmn.Value[i].PartyType);
							continue;
						}
						#endregion

						#region Attack > Defense > Equal =
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].PhysicalStat.HasValue && pkmn.Value[i].PhysicalStat.Value == 1)           //Attack Greater Than Defense (Attack > Defense)    1
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.AttackGreater),";
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.AttackGreater, pkmn.Value[i].PartyType);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].PhysicalStat.HasValue && pkmn.Value[i].PhysicalStat.Value == -1)    //Defense Greater Than Attack (Attack < Defense)    -1
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.DefenseGreater),";
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.DefenseGreater, pkmn.Value[i].PartyType);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].PhysicalStat.HasValue && pkmn.Value[i].PhysicalStat.Value == 0)    //Attack Equal To Attack (Attack = Defense)         0
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.AtkDefEqual),";
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.AtkDefEqual, pkmn.Value[i].PartyType);
							continue;
						}
						#endregion

						#region Silcoon
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Species == Pokemons.SILCOON)
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Silcoon),";
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Silcoon);
							continue;
						}
						#endregion

						#region Cascoon
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Species == Pokemons.CASCOON)
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Cascoon),";
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Cascoon);
							continue;
						}
						#endregion

						#region Ninjask
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Species == Pokemons.NINJASK)
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Ninjask),";
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Ninjask);
							continue;
						}
						#endregion

						#region Shedinja
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Species == Pokemons.SHEDINJA)
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Shedinja),";
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Shedinja);
							continue;
						}
						#endregion

						#region Level
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Trigger == Items.NONE && pkmn.Value[i].MinLevel.HasValue && !pkmn.Value[i].Happiness.HasValue && !pkmn.Value[i].TurnUpsideDown)
						{
							if (pkmn.Value[i].Gender.HasValue && pkmn.Value[i].Gender.Value == true) //"1"
							{
								//MethodCode = MethodCode + $"\n\tnew PokemonEvolution<int>(Pokemons.{name.ToUpper()}, EvolutionMethod.LevelFemale, {csv.Context.Record[4]}),";
								PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.LevelFemale, pkmn.Value[i].MinLevel);
								continue;
							}
							else if (pkmn.Value[i].Gender.HasValue && pkmn.Value[i].Gender.Value == false) //"2"
							{
								//MethodCode = MethodCode + $"\n\tnew PokemonEvolution<int>(Pokemons.{name.ToUpper()}, EvolutionMethod.LevelMale, {csv.Context.Record[4]}),";
								PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.LevelMale, pkmn.Value[i].MinLevel);
								continue;
							}
							else if (pkmn.Value[i].Time.HasValue) //"Lycanroc"
							{
								//MethodCode = MethodCode + $"\n\tnew PokemonEvolution<int>(Pokemons.{name.ToUpper()}, EvolutionMethod.Lycanroc, {csv.Context.Record[4]}),";
								PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Lycanroc, pkmn.Value[i].MinLevel);
								continue;
							}
							else
							{
								//MethodCode = MethodCode + $"\n\tnew PokemonEvolution<int>(Pokemons.{name.ToUpper()}, EvolutionMethod.Level, {csv.Context.Record[4]}),";
								PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Level, pkmn.Value[i].MinLevel);
								continue;
							}
						}
						#endregion
						//continue;
					}
				}
				return true;
			} catch (SQLiteException e) {
				GameDebug.LogError("SQL Exception Message:" + e.Message);
				GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetPokemonItemsFromSQL(SQLiteConnection con)
		{
			try
			{
				Dictionary<Pokemons, List<Monster.Data.PokemonWildItems>> p = new Dictionary<Pokemons, List<Monster.Data.PokemonWildItems>>();
				foreach (Pokemons x in PokemonData.Keys)//for(int n = 1; n <= PokemonData.Keys.Length; n++)
				{
					p.Add(x, new List<Monster.Data.PokemonWildItems>());
				}
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = "select * from pokemon_items group by pokemon_id, item_id";
				//	@"select *
				//from pokemon_items
				//group by pokemon_id, item_id";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					while(reader.Read()) //if(reader.Read())
					{
						//if (!p.ContainsKey((Pokemons)int.Parse((string)reader["pokemon_id"].ToString())))
						//	p.Add((Pokemons)int.Parse((string)reader["pokemon_id"].ToString()),
						//		new List<Monster.Data.PokemonWildItems>());
						p[(Pokemons)int.Parse((string)reader["pokemon_id"].ToString())].Add(
							new PokemonUnity.Monster.Data.PokemonWildItems(
								itemId: (Items)int.Parse((string)reader["item_id"].ToString())
								,generation: int.Parse((string)reader["version_id"].ToString())
								,rarity: int.Parse((string)reader["rarity"].ToString())
							)
						);
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
					//PokemonItemsData.Add(Pokemons.NONE, new Monster.Data.PokemonWildItems[] { });
					foreach (var pkmn in p)
					{
						PokemonItemsData.Add(pkmn.Key, pkmn.Value.ToArray());
					}
				}
				return true;
			} catch (SQLiteException e) {
				GameDebug.LogError("SQL Exception Message:" + e.Message);
				GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetNaturesFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = "select * from natures order by game_index ASC";
				//	@"";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					//NatureData.Add(Natures.UNSET, new Nature());
					while(reader.Read()) //if(reader.Read())
					{
						NatureData.Add((Natures)(int.Parse((string)reader["id"].ToString())-1),
							new Nature(
								nature: (Natures)(int.Parse((string)reader["id"].ToString())-1)
								,increase: (Stats)(int.Parse((string)reader["increased_stat_id"].ToString())-1)
								,decrease: (Stats)(int.Parse((string)reader["decreased_stat_id"].ToString())-1)
								,like: (Flavours)int.Parse((string)reader["likes_flavor_id"].ToString())
								,dislike: (Flavours)int.Parse((string)reader["hates_flavor_id"].ToString())
							)
						);
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
				}
				return true;
			} catch (SQLiteException e) {
				GameDebug.LogError("SQL Exception Message:" + e.Message);
				GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetTypesFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = @"select a.id, a.damage_class_id, t.damage_type_id, t.target_type_id, t.damage_factor from types as a
					left join type_efficacy as t on a.id = t.damage_type_id
					where damage_type_id is not null
					order by a.id ASC";
				//	@"";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					Dictionary<Types, Category?> t = new Dictionary<Types, Category?>();
					Dictionary<Types, List<KeyValuePair<Types, int>>> a = new Dictionary<Types, List<KeyValuePair<Types, int>>>();
					while (reader.Read()) //if(reader.Read())
					{
						if (!t.ContainsKey((Types)int.Parse((string)reader["id"].ToString())))
							t.Add((Types)int.Parse((string)reader["id"].ToString()),
								string.IsNullOrEmpty(reader["damage_factor"].ToString()) ?
								(Category?)null : (Category?)int.Parse((string)reader["damage_factor"].ToString()));
						//if (!a[(Types)int.Parse((string)reader["id"].ToString())]
						//	.Contains(int.Parse((string)reader["damage_type_id"].ToString())))
						//	t[(Locations)int.Parse((string)reader["id"].ToString())]
						//		.Add(int.Parse((string)reader["damage_type_id"].ToString()));

						if (!string.IsNullOrEmpty(reader["id"].ToString()))
						{
							if (!a.ContainsKey((Types)int.Parse((string)reader["id"].ToString())))
								a.Add((Types)int.Parse((string)reader["id"].ToString()), new List<KeyValuePair<Types, int>>());
							a[(Types)int.Parse((string)reader["id"].ToString())]
								.Add(new KeyValuePair<Types, int>(
								key: (Types)int.Parse((string)reader["target_type_id"].ToString())
								,value: int.Parse((string)reader["damage_factor"].ToString())
							));
						}
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
					foreach (var area in a)
					{
						Dictionary<Types, byte> x = new Dictionary<Types, byte>();
						foreach (KeyValuePair<Types, int> y in area.Value)
							x.Add(key: y.Key, value: (byte)y.Value);
						TypeData.Add(area.Key,
							new Monster.Data.Type(
								atk: area.Key
								//,table: a.Where(x => x.Key == area.Key).Select<KeyValuePair<Method, int>>(n => n.Value).ToArray()
								, table: x//.Single(x => x.Key == area.Key).Value.ToDictionary<Types, byte>(x => x.Key == area.Key)
								, category: t[area.Key]
							)
						);
					}
					TypeData.Add(Types.UNKNOWN, new Monster.Data.Type());
					TypeData.Add(Types.SHADOW, new Monster.Data.Type());
				}
				return true;
			} catch (SQLiteException e) {
				GameDebug.LogError("SQL Exception Message:" + e.Message);
				GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetMovesFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = //"select * from moves_view";
					@"select moves.id, moves.identifier, moves.generation_id, moves.type_id, moves.power, moves.pp, moves.accuracy, moves.priority, moves.target_id, moves.damage_class_id, moves.effect_id, moves.effect_chance, moves.contest_type_id, moves.contest_effect_id, moves.super_contest_effect_id,
				contest_effects.appeal, contest_effects.jam, super_contest_effects.appeal as super_appeal,
				move_meta.meta_category_id, move_meta.meta_ailment_id, move_meta.min_hits, move_meta.max_hits, move_meta.min_turns, move_meta.max_turns, move_meta.drain, move_meta.healing, move_meta.crit_rate, move_meta.ailment_chance, move_meta.flinch_chance, move_meta.stat_chance,
				--move_flags.identifier as move_flag,
				move_flag_map_view.move_flag_group,
				move_effect_prose.short_effect, move_effect_prose.effect,
				--move_flag_prose.name as flag_name, move_flag_prose.description as flag_description,
				move_targets.identifier as target_identifier,
				move_target_prose.name as target_name, move_target_prose.description as target_description,
				move_names.name,
				move_flavor_text.flavor_text
				from moves
				left join (
					select move_id, group_concat(DISTINCT move_flag_id) as move_flag_group
					from move_flag_map
					group by move_id
				) as move_flag_map_view on move_flag_map_view.move_id = moves.id
				--left join move_flags on move_flags.id = moves.id
				left join move_meta on move_meta.move_id = moves.id
				left join contest_effects on moves.contest_effect_id = contest_effects.id
				left join super_contest_effects on moves.super_contest_effect_id = super_contest_effects.id
				left join move_targets on move_targets.id = moves.target_id
				left join move_target_prose on move_target_prose.move_target_id=move_targets.id AND move_target_prose.local_language_id=9
				left join move_effect_prose on move_effect_prose.move_effect_id = moves.effect_id AND move_effect_prose.local_language_id=9
				left join move_names on move_names.move_id = moves.id AND move_names.local_language_id=9
				left join move_flavor_text on move_flavor_text.move_id = moves.id AND move_flavor_text.version_group_id=18 AND move_flavor_text.language_id=9
				--where moves.id > 725
				order by moves.id ASC;";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					MoveData.Add(Moves.NONE, new Attack.Data.MoveData());
					while(reader.Read()) //if(reader.Read())
					{
						string[] f = ((string)reader["move_flag_group"].ToString()).Split(',');
						/*object debug = null;
						debug = (Moves)int.Parse((string)reader["id"].ToString());
						debug = (Category)int.Parse((string)reader["damage_class_id"].ToString());
						debug = int.Parse((string)reader["generation_id"].ToString());
						debug = string.IsNullOrEmpty((string)reader["accuracy"].ToString()) ? (int?)null : int.Parse((string)reader["accuracy"].ToString());
						debug = string.IsNullOrEmpty((string)reader["power"].ToString()) ? (int?)null : int.Parse((string)reader["power"].ToString());
						debug = string.IsNullOrEmpty((string)reader["pp"].ToString()) ? (byte)0 : (byte)int.Parse((string)reader["pp"].ToString());
						debug = int.Parse((string)reader["priority"].ToString());
						//debug = (Flag)new Flag(;
							debug = f.Contains("14");
							debug = f.Contains("16");
							debug = f.Contains("18");
							debug = f.Contains("2");
							debug = f.Contains("1");
							//debug = f.Contains("1");
							debug = f.Contains("21");
							debug = f.Contains("11");
							debug = f.Contains("12");
							//a = f.Contains("1");
							debug = f.Contains("10");
							debug = f.Contains("13");
							debug = f.Contains("7");
							debug = f.Contains("19");
							debug = f.Contains("20");
							debug = f.Contains("15");
							debug = f.Contains("4");
							debug = f.Contains("17");
							debug = f.Contains("8");
							debug = f.Contains("3");
							debug = f.Contains("5");
							debug = f.Contains("6");
							debug = f.Contains("9");
						debug = (Targets)int.Parse((string)reader["target_id"].ToString());
						debug = (Types)int.Parse((string)reader["type_id"].ToString());
						debug = string.IsNullOrEmpty((string)reader["contest_type_id"].ToString()) ? (Contests?)null : (Contests)int.Parse((string)reader["contest_type_id"].ToString());
						//debug = int.Parse((string)reader["decreased_stat_id"].ToString());
						//debug = int.Parse((string)reader["decreased_stat_id"].ToString());
						debug = (Effects)int.Parse((string)reader["effect_id"].ToString());
						debug = string.IsNullOrEmpty((string)reader["effect_chance"].ToString()) ? (int?)null : int.Parse((string)reader["effect_chance"].ToString());
						//debug = int.Parse((string)reader["decreased_stat_id"].ToString());
						debug = string.IsNullOrEmpty((string)reader["contest_effect_id"].ToString()) ? (int?)null : int.Parse((string)reader["contest_effect_id"].ToString());//(ContestEffects)
						debug = string.IsNullOrEmpty((string)reader["super_contest_effect_id"].ToString()) ? (int?)null : int.Parse((string)reader["super_contest_effect_id"].ToString());//(SuperContestEffects)
						debug = string.IsNullOrEmpty((string)reader["appeal"].ToString()) ? (int?)null : int.Parse((string)reader["appeal"].ToString());
						debug = string.IsNullOrEmpty((string)reader["super_appeal"].ToString()) ? (int?)null : int.Parse((string)reader["super_appeal"].ToString());
						debug = string.IsNullOrEmpty((string)reader["jam"].ToString()) ? (int?)null : int.Parse((string)reader["jam"].ToString());*/
						MoveData.Add((Moves)int.Parse((string)reader["id"].ToString()),
							new MoveData(
								id: (Moves)int.Parse((string)reader["id"].ToString())
								,category: (Category)int.Parse((string)reader["damage_class_id"].ToString())
								,generationId: int.Parse((string)reader["generation_id"].ToString())
								,accuracy: string.IsNullOrEmpty((string)reader["accuracy"].ToString()) ? (int?)null : int.Parse((string)reader["accuracy"].ToString())
								,baseDamage: string.IsNullOrEmpty((string)reader["power"].ToString()) ? (int?)null : int.Parse((string)reader["power"].ToString())
								,pp: string.IsNullOrEmpty((string)reader["pp"].ToString()) ? (byte)0 : (byte)int.Parse((string)reader["pp"].ToString())
								,priority: int.Parse((string)reader["priority"].ToString())
								,flags: (Flag)new Flag(
									authentic: f.Contains("14")
									,bite: f.Contains("16")
									,bullet: f.Contains("18")
									,charge: f.Contains("2")
									,contact: f.Contains("1")
									//,crit: f.Contains("1")
									,dance: f.Contains("21")
									,defrost: f.Contains("11")
									,distance: f.Contains("12")
									//,flinch: f.Contains("1")
									,gravity: f.Contains("10")
									,heal: f.Contains("13")
									,mirror: f.Contains("7")
									,mental: f.Contains("19")
									,nonsky: f.Contains("20")
									,powder: f.Contains("15")
									,protect: f.Contains("4")
									,pulse: f.Contains("17")
									,punch: f.Contains("8")
									,recharge: f.Contains("3")
									,reflectable: f.Contains("5")
									,snatch: f.Contains("6")
									,sound: f.Contains("9"))
								,target: (Targets)int.Parse((string)reader["target_id"].ToString())
								,type: (Types)int.Parse((string)reader["type_id"].ToString())
								,contestType: string.IsNullOrEmpty((string)reader["contest_type_id"].ToString()) ? (Contests?)null : (Contests)int.Parse((string)reader["contest_type_id"].ToString())
								//,function: int.Parse((string)reader["decreased_stat_id"].ToString())
								//,functionAsString: int.Parse((string)reader["decreased_stat_id"].ToString())
								,effects: (Effects)int.Parse((string)reader["effect_id"].ToString())
								,chance: string.IsNullOrEmpty((string)reader["effect_chance"].ToString()) ? (int?)null : int.Parse((string)reader["effect_chance"].ToString())
								//,description: int.Parse((string)reader["decreased_stat_id"].ToString())
								//ToDo:,contestEffects: string.IsNullOrEmpty((string)reader["contest_effect_id"].ToString()) ? (int?)null : (ContestEffects)int.Parse((string)reader["contest_effect_id"].ToString())
								//ToDo:,superContestEffects: string.IsNullOrEmpty((string)reader["super_contest_effect_id"].ToString()) ? (int?)null : (SuperContestEffects)int.Parse((string)reader["super_contest_effect_id"].ToString())
								,appeal: string.IsNullOrEmpty((string)reader["appeal"].ToString()) ? (int?)null : int.Parse((string)reader["appeal"].ToString())
								,superAppeal: string.IsNullOrEmpty((string)reader["super_appeal"].ToString()) ? (int?)null : int.Parse((string)reader["super_appeal"].ToString())
								,jamming: string.IsNullOrEmpty((string)reader["jam"].ToString()) ? (int?)null : int.Parse((string)reader["jam"].ToString())
							)
						);
						if(!string.IsNullOrEmpty((string)reader["meta_category_id"].ToString()))
							MoveMetaData.Add((Moves)int.Parse((string)reader["id"].ToString()),
								new MetaData(
									move: (Moves)int.Parse((string)reader["id"].ToString())
									,category: (MetaCategory)int.Parse((string)reader["meta_category_id"].ToString())
									,ailment: (MoveMetaAilments)int.Parse((string)reader["meta_ailment_id"].ToString())
									,minHits:  string.IsNullOrEmpty((string)reader["min_hits"].ToString()) ? (int?)null : int.Parse((string)reader["min_hits"].ToString())
									,maxHits:  string.IsNullOrEmpty((string)reader["max_hits"].ToString()) ? (int?)null : int.Parse((string)reader["max_hits"].ToString())
									,minTurns: string.IsNullOrEmpty((string)reader["min_turns"].ToString()) ? (int?)null : int.Parse((string)reader["min_turns"].ToString())
									,maxTurns: string.IsNullOrEmpty((string)reader["max_turns"].ToString()) ? (int?)null : int.Parse((string)reader["max_turns"].ToString())
									,drain: int.Parse((string)reader["drain"].ToString())
									,healing: int.Parse((string)reader["healing"].ToString())
									,critRate: int.Parse((string)reader["crit_rate"].ToString())
									,ailmentChance: int.Parse((string)reader["ailment_chance"].ToString())
									,flinchChance: int.Parse((string)reader["flinch_chance"].ToString())
								)
							);
						//else
						//	MoveMetaData.Add((Moves)int.Parse((string)reader["id"].ToString()),
						//		new MetaData(
						//			move: (Moves)int.Parse((string)reader["id"].ToString())
						//			//,category: MetaCategory.DAMAGE
						//			//,ailment: MoveMetaAilments.NONE
						//			//,minHits:  (int?)null
						//			//,maxHits:  (int?)null
						//			//,minTurns: (int?)null
						//			//,maxTurns: (int?)null
						//			//,drain: 0
						//			//,healing: 0
						//			//,critRate: 0
						//			//,ailmentChance: 0
						//			//,flinchChance: 0
						//		)
						//	);
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
				}
				return true;
			} catch (SQLiteException e) {
				GameDebug.LogError("SQL Exception Message:" + e.Message);
				GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetItemsFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = //"select * from item_views";
					@"select items.id, items.identifier, items.category_id, items.cost, items.fling_power, items.fling_effect_id,
				item_flag_map_view.item_flag_group,
				item_game_indices_view.generation_group, item_game_indices_view.game_index_group,
				item_prose.short_effect, item_prose.effect,
				item_categories.pocket_id,
				item_pockets.identifier as item_pocket_identifier,
				item_names.name,
				item_flavor_text.flavor_text
				from items
				left join(
					select item_id, group_concat(DISTINCT item_flag_id) as item_flag_group
					from item_flag_map
					group by item_id
					) as item_flag_map_view on item_flag_map_view.item_id = items.id
				left join(
					select item_id, group_concat(DISTINCT generation_id) as generation_group, group_concat(DISTINCT game_index) as game_index_group
					from item_game_indices
					group by item_id
					) as item_game_indices_view on item_game_indices_view.item_id = items.id
				left join item_categories on item_categories.id = items.category_id
				left join item_pockets on item_pockets.id = item_categories.pocket_id
				left join item_prose on item_prose.item_id = items.id AND item_prose.local_language_id = 9
				left join item_fling_effect_prose on item_fling_effect_prose.item_fling_effect_id = items.fling_effect_id AND item_fling_effect_prose.local_language_id = 9
				left join item_names on item_names.item_id = items.id AND item_names.local_language_id = 9
				left join item_flavor_text on item_flavor_text.item_id = items.id AND item_flavor_text.version_group_id = 18 AND item_flavor_text.language_id = 9
				order by items.id ASC;";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					ItemData.Add(Items.EXP_ALL, new ItemData());
					ItemData.Add(Items.NONE, new ItemData());
					while(reader.Read()) //if(reader.Read())
					{
						string[] f = ((string)reader["item_flag_group"].ToString()).Split(',');
						ItemData.Add((Items)int.Parse((string)reader["id"].ToString()),
							new ItemData(
								itemId: (Items)int.Parse((string)reader["id"].ToString())
								,price: int.Parse((string)reader["cost"].ToString())
								,itemCategory: (ItemCategory)int.Parse((string)reader["category_id"].ToString())
								,itemFlag: (ItemFlag)new ItemFlag(
									countable: f.Contains("1")
									,consumable: f.Contains("2")
									,useableOverworld: f.Contains("3")
									,useableBattle: f.Contains("4")
									,holdable: f.Contains("5")
									,holdablePassive: f.Contains("6")
									,holdableActive: f.Contains("7")
									,underground: f.Contains("8"))
								,power: string.IsNullOrEmpty((string)reader["fling_power"].ToString()) ? (int?)null : int.Parse((string)reader["fling_power"].ToString())
								,itemFlingEffect: string.IsNullOrEmpty((string)reader["fling_effect_id"].ToString()) ? ItemFlingEffect.NONE : (ItemFlingEffect)int.Parse((string)reader["fling_effect_id"].ToString())
								//,pocket: (Flavours)int.Parse((string)reader["pocket_id"].ToString())
								,gens: reader["generation_group"].ToString().Split(',').Select(x => int.Parse(x)).ToArray()
							)
						);
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
				}
				return true;
			} catch (SQLiteException e) {
				GameDebug.LogError("SQL Exception Message:" + e.Message);
				GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetBerriesFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = //"select * from berry_views order by game_index ASC";
					@"select b.id, b.item_id, b.firmness_id, b.natural_gift_power, b.natural_gift_type_id, b.size, b.max_harvest, b.growth_time, b.soil_dryness, b.smoothness
				,i.cool, i.beauty, i.cute, i.smart, i.tough
				from berries as b
				left join (select
					b.id as berry_id
					,CAST(AVG(CASE WHEN i.contest_type_id = 1 THEN i.flavor
					END) as int) as cool
					,CAST(AVG(CASE WHEN i.contest_type_id = 2 THEN i.flavor
					END) as int) as beauty
					,CAST(AVG(CASE WHEN i.contest_type_id = 3 THEN i.flavor
					END) as int) as cute
					,CAST(AVG(CASE WHEN i.contest_type_id = 4 THEN i.flavor
					END) as int) as smart
					,CAST(AVG(CASE WHEN i.contest_type_id = 5 THEN i.flavor
					END) as int) as tough
					from berries as b
					left join berry_flavors as i on b.id = i.berry_id
					group by b.id
				) as i on b.id = i.berry_id;";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					//NatureData.Add(Natures.UNSET, new Nature());
					while(reader.Read()) //if(reader.Read())
					{
						BerryData.Add((Items)(int.Parse((string)reader["item_id"].ToString())-1),
							new Berry(
								berry: (Items)int.Parse((string)reader["item_id"].ToString())
								,firmness: (FirmnessLevel)int.Parse((string)reader["firmness_id"].ToString())
								,power: int.Parse((string)reader["natural_gift_power"].ToString())
								,type: (Types)int.Parse(string.IsNullOrEmpty((string)reader["natural_gift_type_id"].ToString()) ? "0" : (string)reader["natural_gift_type_id"].ToString())
								,size: int.Parse((string)reader["size"].ToString())
								,max: int.Parse((string)reader["max_harvest"].ToString())
								,growth: int.Parse((string)reader["growth_time"].ToString())
								,soil: int.Parse((string)reader["soil_dryness"].ToString())
								,smooth: int.Parse((string)reader["smoothness"].ToString())
								,cool: int.Parse((string)reader["cool"].ToString())
								,beauty: int.Parse((string)reader["beauty"].ToString())
								,cute: int.Parse((string)reader["cute"].ToString())
								,smart: int.Parse((string)reader["smart"].ToString())
								,tough: int.Parse((string)reader["tough"].ToString())
							)
						);
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
				}
				return true;
			} catch (SQLiteException e) {
				GameDebug.LogError("SQL Exception Message:" + e.Message);
				GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetRegionsFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = //"select * from natures order by game_index ASC";
					@"select l.id, l.region_id, l.identifier,
				group_concat(DISTINCT g.generation_id) as generation_group, --g.generation_id,
				n.name, n.subtitle,
				group_concat(DISTINCT v.version_group_id) as version_group
				from locations as l
				left join location_game_indices as g on l.id = g.location_id
				left join location_names as n on l.id = n.location_id AND n.local_language_id = 9
				left join version_group_regions as v on l.region_id = v.region_id
				group by l.id; --having count(*) > 1;";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					Dictionary<Regions, List<Locations>> r = new Dictionary<Regions, List<Locations>>();//ToDo: List<Locations,int[]>(Locations.Id,version_group.ToArray())
					r.Add(Regions.NOT_IN_OVERWORLD, new List<Locations>());
					r[Regions.NOT_IN_OVERWORLD].Add(Locations.NOT_IN_OVERWORLD);
					//r.Add(Regions.NOT_IN_OVERWORLD, Locations.NOT_IN_OVERWORLD);
					while (reader.Read()) //if(reader.Read())
					{
						if (!string.IsNullOrEmpty(reader["region_id"].ToString()) && !r.ContainsKey((Regions)int.Parse((string)reader["region_id"].ToString())))
							r.Add((Regions)int.Parse((string)reader["region_id"].ToString()), new List<Locations>());
						r[string.IsNullOrEmpty(reader["region_id"].ToString()) ? Regions.NOT_IN_OVERWORLD : (Regions)int.Parse((string)reader["region_id"].ToString())]
							.Add((Locations)int.Parse((string)reader["id"].ToString()));
						//RegionData.Add((Regions)int.Parse((string)reader["region_id"].ToString()),
						//	r.ToArray()
						//	//new Location(
						//	//	location: (Locations)int.Parse((string)reader["id"].ToString())
						//	//	//,region: (Regions)int.Parse((string)reader["region_id"].ToString())
						//	//	//,gen: int.Parse((string)reader["generation_id"].ToString())
						//	//)
						//);
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
					//RegionData.Add(Regions.NOT_IN_OVERWORLD, new Locations[] { Locations.NOT_IN_OVERWORLD });
					foreach (var reg in r)
					{
						//RegionData.Add((Regions)int.Parse((string)reader["region_id"].ToString()), reg.Value.ToArray());
						RegionData.Add((Regions)reg.Key, reg.Value.ToArray());
					}
				}
				return true;
			} catch (SQLiteException e) {
				GameDebug.LogError("SQL Exception Message:" + e.Message);
				GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetLocationsFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = //"select * from natures order by game_index ASC";
					@"select l.id, l.location_id, l.identifier,
				location_area_encounter_rates_view.location_area_id, location_area_encounter_rates_view.version_group, location_area_encounter_rates_view.encounter_method_id, location_area_encounter_rates_view.rate
				from location_areas as l
				left join(
					select location_area_id, group_concat(DISTINCT version_id) as version_group, encounter_method_id, rate
					from location_area_encounter_rates
					group by location_area_id, encounter_method_id, rate
				) location_area_encounter_rates_view on l.id = location_area_encounter_rates_view.location_area_id;";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					Dictionary<Locations, List<int>> l = new Dictionary<Locations, List<int>>();
					Dictionary<int, List<KeyValuePair<Method, int>>> a = new Dictionary<int, List<KeyValuePair<Method, int>>>();
					while (reader.Read()) //if(reader.Read())
					{
						if (!l.ContainsKey((Locations)int.Parse((string)reader["location_id"].ToString())))
							l.Add((Locations)int.Parse((string)reader["location_id"].ToString()), new List<int>());
						if (!l[(Locations)int.Parse((string)reader["location_id"].ToString())]
							.Contains(int.Parse((string)reader["id"].ToString())))
							l[(Locations)int.Parse((string)reader["location_id"].ToString())]
								.Add(int.Parse((string)reader["id"].ToString()));
						//AreaData.Add(int.Parse((string)reader["location_area_id"].ToString()),
						//	new Area(
						//		id: int.Parse((string)reader["location_area_id"].ToString())
						//		,method: (Method)int.Parse((string)reader["encounter_method_id"].ToString())
						//		,rate: int.Parse((string)reader["rate"].ToString())
						//	)
						//);
						if (!string.IsNullOrEmpty(reader["location_area_id"].ToString()))
						{
							if (!a.ContainsKey(int.Parse((string)reader["location_area_id"].ToString())))
								a.Add(int.Parse((string)reader["location_area_id"].ToString()), new List<KeyValuePair<Method, int>>());
							a[int.Parse((string)reader["location_area_id"].ToString())]
								.Add(new KeyValuePair<Method, int>(//ToDo: int[] { rate, version_group }
								//id: int.Parse((string)reader["location_area_id"].ToString())
								key: (Method)int.Parse((string)reader["encounter_method_id"].ToString())
								,value: int.Parse((string)reader["rate"].ToString())
							));
						}
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
					LocationData.Add(Locations.NOT_IN_OVERWORLD, new List<int>() { }.ToArray());
					foreach (var loc in l)
					{
						//LocationData.Add((Locations)int.Parse((string)reader["location_id"].ToString()), loc.Value.ToArray());
						LocationData.Add((Locations)loc.Key, loc.Value.ToArray());
					}
					foreach (var area in a)
					{
						AreaData.Add(area.Key,//int.Parse((string)reader["location_area_id"].ToString()),
							new Area(
								id: area.Key
								//,rate: a.Where(x => x.Key == area.Key).Select<KeyValuePair<Method, int>>(n => n.Value).ToArray()
								,rate: a.Single(x => x.Key == area.Key).Value.ToArray()
							)
						);
					}
				}
				return true;
			} catch (SQLiteException e) {
				GameDebug.LogError("SQL Exception Message:" + e.Message);
				GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetEncountersFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = string.Format(//"select * from natures order by game_index ASC";
					@"select --e.id, e.version_id, e.location_area_id, e.encounter_slot_id, e.pokemon_id, e.min_level, e.max_level,
				MIN(e.id) as id, e.location_area_id, group_concat(DISTINCT e.pokemon_id) as pokemon_group, MIN(e.min_level) as min_level, MAX(e.max_level) as max_level, group_concat(DISTINCT e.encounter_slot_id) as encounter_slot_group, group_concat(DISTINCT e.version_id) as version_group,
				s.encounter_method_id, s.slot, s.rarity, group_concat(DISTINCT s.version_group_id) as encounter_slot_version_group,
				--i.encounter_condition_value_group,
				group_concat(DISTINCT i.encounter_condition_value_id) as encounter_condition_value_group,
				a.location_id,
				l.region_id,
				g.generation_id
				--group_concat(DISTINCT g.generation_id) as generation_group
				--,n.name, n.subtitle
				from encounters as e
				--left join encounter_slots
				left join (
					select id, version_group_id, rarity, encounter_method_id,
					case when (slot is not null) AND version_group_id = 15 then slot + 1 else slot end as slot
					from encounter_slots
				) 
				as s on s.id = e.encounter_slot_id
				--left join encounter_condition_value_map_view as i on e.id = i.encounter_id;
				--left join (
				--	select encounter_id, group_concat(DISTINCT encounter_condition_value_id) as encounter_condition_value_group
				--	from encounter_condition_value_map
				--	group by encounter_id
				--) as i on e.id = i.encounter_id
				left join encounter_condition_value_map as i on e.id = i.encounter_id
				--left join (
				--	select location_area_id, group_concat(DISTINCT version_id) as version_group, encounter_method_id, rate
				--	from location_area_encounter_rates
				--	group by location_area_id, encounter_method_id, rate
				--) as r on e.location_area_id = r.location_area_id
				left join location_areas as a on a.id=e.location_area_id
				left join locations as l on l.id=a.location_id
				left join location_game_indices as g on l.id = g.location_id
				left join location_names as n on l.id = n.location_id AND n.local_language_id=9
				--where i.encounter_condition_value_group IS NOT NULL
				--where i.encounter_condition_value_id IS NOT NULL
				where e.location_area_id = {0}
				group by e.location_area_id, s.encounter_method_id, s.slot, s.rarity; --e.pokemon_id;", GameData.Area);
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					Dictionary<Method, List<int>> e = new Dictionary<Method, List<int>>();
					while (reader.Read()) //if(reader.Read())
					{
						if (!e.ContainsKey((Method)int.Parse((string)reader["encounter_method_id"].ToString())))
							e.Add((Method)int.Parse((string)reader["encounter_method_id"].ToString()), new List<int>());
						e[(Method)int.Parse((string)reader["encounter_method_id"].ToString())]
							.Add(int.Parse((string)reader["id"].ToString())
						);
						EncounterData.Add(int.Parse((string)reader["id"].ToString()),
							new EncounterData(
								id: int.Parse((string)reader["id"].ToString())
								,area: int.Parse((string)reader["location_area_id"].ToString())
								,method: (Method)int.Parse((string)reader["encounter_method_id"].ToString())
								,slotId: int.Parse((string)reader["slot"].ToString())
								//,pokemon: (Pokemons)int.Parse((string)reader["pokemon_id"].ToString())
								,pokemon: reader["pokemon_group"].ToString().Split(',').Select(x => (Pokemons)int.Parse(x)).ToArray()
								, conditions: reader["encounter_condition_value_group"].ToString().Split(',').Select(x => (ConditionValue)int.Parse(x)).ToArray()
								,generation: int.Parse((string)reader["generation_id"].ToString())
								,minLevel: int.Parse((string)reader["min_level"].ToString())
								,maxLevel: int.Parse((string)reader["max_level"].ToString())
								,rarity: int.Parse((string)reader["rarity"].ToString())
								,versions: reader["encounter_slot_version_group"].ToString().Split(',').Select(x => (Versions)int.Parse(x)).ToArray()
							)
						);
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
					//EncounterData.Add(Method.NONE, new List<Encounter>() { }.ToArray());
					foreach (var en in e)
					{
						MethodData.Add((Method)int.Parse((string)reader["encounter_method_id"].ToString()), en.Value.ToArray());
					}
				}
				return true;
			} catch (SQLiteException e) {
				GameDebug.LogError("SQL Exception Message:" + e.Message);
				GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetMachinesFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = //"select * from natures order by game_index ASC";
					@"select row_number() over (order by m.machine_number) as id, m.machine_number as tm_num, m.item_id, group_concat(DISTINCT m.move_id) as move_group, group_concat(DISTINCT m.version_group_id) as version_group,
				g.identifier, g.type_id
				--n.name, n.subtitle
				from machines as m
				left join moves as g on g.id = m.move_id
				--left join location_names as n on l.id = n.location_id AND n.local_language_id = 9;
				group by m.item_id, m.move_id;";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					Dictionary<Method, List<int>> e = new Dictionary<Method, List<int>>();
					while (reader.Read()) //if(reader.Read())
					{
						if (!e.ContainsKey((Method)int.Parse((string)reader["encounter_method_id"].ToString())))
							e.Add((Method)int.Parse((string)reader["encounter_method_id"].ToString()), new List<int>());
						e[(Method)int.Parse((string)reader["encounter_method_id"].ToString())]
							.Add(int.Parse((string)reader["id"].ToString())
						);
						EncounterData.Add(int.Parse((string)reader["id"].ToString()),
							new EncounterData(
								id: int.Parse((string)reader["id"].ToString())
								,area: int.Parse((string)reader["location_area_id"].ToString())
								,method: (Method)int.Parse((string)reader["encounter_method_id"].ToString())
								,slotId: int.Parse((string)reader["slot"].ToString())
								//,pokemon: (Pokemons)int.Parse((string)reader["pokemon_id"].ToString())
								,pokemon: reader["pokemon_group"].ToString().Split(',').Select(x => (Pokemons)int.Parse(x)).ToArray()
								, conditions: reader["encounter_condition_value_group"].ToString().Split(',').Select(x => (ConditionValue)int.Parse(x)).ToArray()
								,generation: int.Parse((string)reader["generation_id"].ToString())
								,minLevel: int.Parse((string)reader["min_level"].ToString())
								,maxLevel: int.Parse((string)reader["max_level"].ToString())
								,rarity: int.Parse((string)reader["rarity"].ToString())
								,versions: reader["encounter_slot_version_group"].ToString().Split(',').Select(x => (Versions)int.Parse(x)).ToArray()
							)
						);
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
					//EncounterData.Add(Method.NONE, new List<Encounter>() { }.ToArray());
					foreach (var en in e)
					{
						MethodData.Add((Method)int.Parse((string)reader["encounter_method_id"].ToString()), en.Value.ToArray());
					}
				}
				return true;
			} catch (SQLiteException e) {
				GameDebug.LogError("SQL Exception Message:" + e.Message);
				GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetPokedexFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = //"select distinct move_id, pokemon_id, [level], pokemon_move_method_id from pokemon_moves --where version_group_id=18 order by pokemon_id ASC";
					@"select p.id, p.identifier, ifnull(p.region_id,0) as region_id,
				group_concat(DISTINCT d.species_id --ORDER BY d.pokedex_number ASC
				) as species_group,
				n.name, n.description,
				group_concat(DISTINCT v.version_group_id) as version_group
				from pokedexes as p
				left join pokemon_dex_numbers as d on p.id=d.pokedex_id
				left join pokedex_prose as n on p.id = n.pokedex_id AND n.local_language_id = 9
				left join pokedex_version_groups as v on p.id = v.pokedex_id
				where p.is_main_series=1 --and v.version_group_id=18
				group by p.id
				order by p.id ASC, d.pokedex_number ASC";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using (reader)
				{
					//Dictionary<Pokemons, List<Monster.Data.PokemonMoveset>> p = new Dictionary<Pokemons, List<Monster.Data.PokemonMoveset>>();
					//while (reader.Read()) //if(reader.Read())
					//{
					//	if (!p.ContainsKey((Pokemons)int.Parse((string)reader["pokemon_id"].ToString())))
					//		p.Add((Pokemons)int.Parse((string)reader["pokemon_id"].ToString()),
					//			new List<Monster.Data.PokemonMoveset>());
					//	p[(Pokemons)int.Parse((string)reader["id"].ToString())].Add(
					//		new PokemonUnity.Monster.Data.PokemonMoveset(
					//			dexId: (Pokedexes)int.Parse((string)reader["id"].ToString())
					//			//,regionId: (Regions)int.Parse((string)reader["region_id"].ToString())
					//			//,dexArray: int.Parse((string)reader["species_group"].ToString())
					//			//,generation: int.Parse((string)reader["version_group_id"].ToString())
					//		)
					//	);
					//}
					////Step 5: Closing up
					//reader.Close();
					//reader.Dispose();
					  #endregion
					//PokemonMovesData.Add(Pokemons.NONE, new Monster.Data.PokemonMoveTree(new Monster.Data.PokemonMoveset[] { }));
					//foreach (var pkmn in p)
					//{
					//	PokemonMovesData.Add(pkmn.Key, new Monster.Data.PokemonMoveTree(pkmn.Value.ToArray()));
					//}
				}
				return true;
			}
			catch (SQLiteException e)
			{
				GameDebug.LogError("SQL Exception Message:" + e.Message);
				GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		#endregion
	}
}