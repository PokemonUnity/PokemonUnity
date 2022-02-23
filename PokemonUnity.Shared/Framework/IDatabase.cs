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
//using System.Data.SQLite;
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
	public interface IDatabase
	{
		#region Variables
		IDictionary<Pokemons, PokemonUnity.Monster.Data.PokemonEvolution[]> PokemonEvolutionsData { get; }
		IDictionary<Pokemons, PokemonUnity.Monster.Data.PokemonWildItems[]> PokemonItemsData { get; }
		IDictionary<Pokemons, PokemonUnity.Monster.Data.Form[]> PokemonFormsData { get; }
		IDictionary<Pokemons, PokemonUnity.Monster.Data.PokemonMoveTree> PokemonMovesData { get; }
		IDictionary<Pokemons, PokemonUnity.Monster.Data.PokemonData> PokemonData { get; }
		IDictionary<Natures,PokemonUnity.Monster.Nature> NatureData { get; }
		IDictionary<Types,PokemonUnity.Monster.Data.Type> TypeData { get; }
		IDictionary<Moves,Attack.Data.MoveData> MoveData { get; }
		IDictionary<Moves,Attack.Data.MetaData> MoveMetaData { get; }
		IDictionary<Attack.Data.Effects,PokemonEssentials.Interface.PokeBattle.IBattleMove> MoveEffectData { get; } //ToDo: Replace with Func<Attack.Data.Effects,Combat.Move> instead?
		IDictionary<Items,ItemData> ItemData { get; }
		IDictionary<Items,BerryData> BerryData { get; }
		IDictionary<Regions,Locations[]> RegionData { get; }
		IDictionary<Regions,Pokedex> PokedexData { get; }
		/// <summary>
		/// Key: <seealso cref="Locations"/> | Value: <seealso cref="Area.Id"/>
		/// </summary>
		IDictionary<Locations,int[]> LocationData { get; }
		/// <summary>
		/// List of <see cref="Player.Area"/> that triggers <seealso cref="Overworld.IEncounterData"/>
		/// <para></para>
		/// Key: <seealso cref="Overworld.IArea.Id"/> | Value: <seealso cref="Player.Area"/>
		/// </summary>
		IDictionary<int,IArea> AreaData { get; }
		/// <summary>
		/// Key: <seealso cref="Method"/> | Value: <seealso cref="IEncounterData.Id"/>
		/// </summary>
		IDictionary<Method,int[]> MethodData { get; }
		/// <summary>
		/// Key: <seealso cref="IEncounterData.Id"/> | Value: <seealso cref="Overworld.IEncounterData"/>
		/// </summary>
		IDictionary<int,IEncounterData> EncounterData { get; }
		//IDictionary<Method,IEncounterData> EncounterMethodData { get; }
		IDictionary<TrainerTypes,TrainerMetaData> TrainerMetaData { get; }
		IDictionary<GymBadges,Character.BadgeData> BadgeData { get; }
		IDictionary<HiddenMoves,HiddenMoveData> HiddenMoveData { get; }
		IDictionary<int,MachineData> MachineData { get; }
		//IDictionary<Items,KeyValuePair<Moves,int[]>[]> MachineMetaData { get; }
		//IDictionary<int,Machine> MapData { get; }
		//IDictionary<int,int> TextureData1 { get; }
		//IDictionary<int,int> TextureData2 { get; }
		//IDictionary<Location,Script[]> ScriptData { get; }
		//IDictionary<Location,Script[]> LevelScriptData { get; }
		//IDictionary<Location,Text[]> TextData { get; }
		//IDictionary<Location,EventData> EventData { get; }
		//IDictionary<Location,Event[]> EventData { get; }
		//IDictionary<Location,EventFlags[]> MapFlagData { get; }
		#endregion

		#region Initialize Data From Database
		bool InitPokemons(bool sql = true);
		bool InitPokemonMoves(bool sql = true);
		bool InitPokemonEvolutions(bool sql = true);
		bool InitPokemonItems(bool sql = true);
		bool InitPokemonForms(bool sql = true);
		bool InitNatures(bool sql = true);
		bool InitTypes(bool sql = true);
		bool InitMoves(bool sql = true);
		bool InitMoveEffects(bool sql = false);
		bool InitItems(bool sql = true);
		bool InitBerries(bool sql = true);
		bool InitRegions(bool sql = true);
		bool InitLocations(bool sql = true);
		bool InitEncounters(bool sql = true);
		bool InitMachines(bool sql = true);
		bool InitTrainers(bool sql = false);
		bool InitGymBadges(bool sql = false);
		bool InitPokedex(bool sql = true);
		//bool InitLoadFile(bool sql = true);
		#endregion
	}
}