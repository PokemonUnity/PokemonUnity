using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Saving;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.Inventory.Plants;
using PokemonUnity.Attack.Data;
using PokemonUnity.Character;

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
	public partial class Kernal //: IDatabase
	{
		#region Variables
		public static IDictionary<Pokemons, PokemonUnity.Monster.Data.PokemonEvolution[]> PokemonEvolutionsData { get; protected set; }
		public static IDictionary<Pokemons, PokemonUnity.Monster.Data.PokemonWildItems[]> PokemonItemsData { get; protected set; }
		public static IDictionary<Pokemons, PokemonUnity.Monster.Data.Form[]> PokemonFormsData { get; protected set; }
		public static IDictionary<Pokemons, PokemonUnity.Monster.Data.PokemonMoveTree> PokemonMovesData { get; protected set; }
		public static IDictionary<Pokemons, PokemonUnity.Monster.Data.PokemonData> PokemonData { get; protected set; }
		public static IDictionary<Natures,PokemonUnity.Monster.Nature> NatureData { get; protected set; }
		public static IDictionary<Types,PokemonUnity.Monster.Data.Type> TypeData { get; protected set; }
		public static IDictionary<Moves,Attack.Data.MoveData> MoveData { get; protected set; }
		public static IDictionary<Moves,Attack.Data.MetaData> MoveMetaData { get; protected set; }
		public static IDictionary<Attack.Data.Effects, PokemonEssentials.Interface.PokeBattle.IBattleMove> MoveEffectData { get; protected set; } //ToDo: Replace with Func<Attack.Data.Effects,Combat.Move> instead?
		public static IDictionary<Items,ItemData> ItemData { get; protected set; }
		public static IDictionary<Items,BerryData> BerryData { get; protected set; }
		public static IDictionary<Regions,Locations[]> RegionData { get; protected set; }
		public static IDictionary<Regions,Pokedex> PokedexData { get; protected set; }
		/// <summary>
		/// Key: <seealso cref="Locations"/> | Value: <seealso cref="IArea.Id"/>
		/// </summary>
		public static IDictionary<Locations,int[]> LocationData { get; protected set; }
		/// <summary>
		/// List of <see cref="Player.Area"/> that triggers <seealso cref="Overworld.IEncounterData"/>
		/// <para></para>
		/// Key: <seealso cref="Overworld.IArea.Id"/> | Value: <seealso cref="Player.Area"/>
		/// </summary>
		public static IDictionary<int,IArea> AreaData { get; protected set; }
		/// <summary>
		/// Key: <seealso cref="Method"/> | Value: <seealso cref="IEncounterData.Id"/>
		/// </summary>
		public static IDictionary<Method,int[]> MethodData { get; protected set; }
		/// <summary>
		/// Key: <seealso cref="IEncounterData.Id"/> | Value: <seealso cref="Overworld.IEncounterData"/>
		/// </summary>
		public static IDictionary<int,IEncounterData> EncounterData { get; protected set; }
		//public static IDictionary<Method,IEncounterData> EncounterMethodData { get; protected set; }
		public static IDictionary<TrainerTypes,TrainerMetaData> TrainerMetaData { get; protected set; }
		public static IDictionary<GymBadges,Character.BadgeData> BadgeData { get; protected set; }
		public static IDictionary<HiddenMoves,HiddenMoveData> HiddenMoveData { get; protected set; }
		public static IDictionary<int,MachineData> MachineData { get; protected set; }
		//public static IDictionary<Items,KeyValuePair<Moves,int[]>[]> MachineMetaData { get; protected set; }
		//public static IDictionary<int,Machine> MapData { get; protected set; }
		//public static IDictionary<int,int> TextureData1 { get; protected set; }
		//public static IDictionary<int,int> TextureData2 { get; protected set; }
		//public static IDictionary<Location,Script[]> ScriptData { get; protected set; }
		//public static IDictionary<Location,Script[]> LevelScriptData { get; protected set; }
		//public static IDictionary<Location,Text[]> TextData { get; protected set; }
		//public static IDictionary<Location,EventData> EventData { get; protected set; }
		//public static IDictionary<Location,Event[]> EventData { get; protected set; }
		//public static IDictionary<Location,EventFlags[]> MapFlagData { get; protected set; }
		#endregion

		#region Initialize Data From Database
		static Kernal()
		{
			PokemonData = new Dictionary<Pokemons, Monster.Data.PokemonData>();
			PokemonMovesData = new Dictionary<Pokemons, Monster.Data.PokemonMoveTree>();
			PokemonEvolutionsData = new Dictionary<Pokemons, Monster.Data.PokemonEvolution[]>();
			PokemonItemsData = new Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonWildItems[]>();
			PokemonFormsData = new Dictionary<Pokemons, Monster.Data.Form[]>();
			NatureData = new Dictionary<Natures, Nature>();
			TypeData = new Dictionary<Types, PokemonUnity.Monster.Data.Type>();
			MoveData = new Dictionary<Moves, Attack.Data.MoveData>();
			MoveMetaData = new Dictionary<Moves, Attack.Data.MetaData>();
			//MoveEffectData = new Dictionary<Attack.Data.Effects, Combat.Move>(); //Below
			MoveEffectData = new Dictionary<Attack.Data.Effects, PokemonEssentials.Interface.PokeBattle.IBattleMove>();
			ItemData = new Dictionary<Items, ItemData>();
			BerryData = new Dictionary<Items, BerryData>();
			RegionData = new Dictionary<Regions, Locations[]>();
			//PokedexData = new Dictionary<Regions, Pokedex>();
			LocationData = new Dictionary<Locations, int[]>();
			AreaData = new Dictionary<int, IArea>();
			MethodData = new Dictionary<Method, int[]>();
			EncounterData = new Dictionary<int, IEncounterData>();
			//MethodData = new Dictionary<Method, int[]>();
			//EncounterData = new Dictionary<int, Encounter>();
			TrainerMetaData = new Dictionary<TrainerTypes, TrainerMetaData>();
			//TrainerData = new Dictionary<int, Encounter>();
			BadgeData = new Dictionary<GymBadges, Character.BadgeData>();
			HiddenMoveData = new Dictionary<HiddenMoves, HiddenMoveData>();
			//Init in Region Method
			PokedexData = new Dictionary<Regions, Pokedex>();
		//	TrainerMetaData = new Dictionary<TrainerTypes, TrainerMetaData>();
		//	//TrainerData = new Dictionary<int, Encounter>();
		}
		#endregion
	}
}