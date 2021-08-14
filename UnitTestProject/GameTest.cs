using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;

namespace Tests
{
    [TestClass]
    public class GameTest
	{
		//Test if Core.Rand Seed returns new value on each use
		//Test if Core.Rand keeps the same seed value when bool is true

		#region Data Loading and Tests
		[TestMethod]
		public void Initialize_Pokemon_Test()
		{
			Assert.IsTrue(Game.InitPokemons());
		}
		[TestMethod]
		public void Pokemon_Data_IsLoaded()
		{
			Assert.IsTrue(Game.PokemonData.Count > 1);
		}
		[TestMethod]
		public void Initialize_PokemonMoves_Test()
		{
			Assert.IsTrue(Game.InitPokemonMoves());
		}
		[TestMethod]
		public void PokemonMoves_Data_IsLoaded()
		{
			Assert.IsTrue(Game.PokemonMovesData.Count > 1);
		}
		[TestMethod]
		public void Initialize_PokemonEvolutions_Test()
		{
			Assert.IsTrue(Game.InitPokemonEvolutions());
		}
		[TestMethod]
		public void PokemonEvolutions_Data_IsLoaded()
		{
			Assert.IsTrue(Game.PokemonEvolutionsData.Count > 1);
		}
		[TestMethod]
		public void Initialize_PokemonForms_Test()
		{
			Assert.IsTrue(Game.InitPokemonForms());
		}
		[TestMethod]
		public void PokemonForms_Data_IsLoaded()
		{
			Assert.IsTrue(Game.PokemonFormsData.Count > 1);
		}
		[TestMethod]
		public void Initialize_PokemonItems_Test()
		{
			Assert.IsTrue(Game.InitPokemonItems());
		}
		[TestMethod]
		public void PokemonItems_Data_IsLoaded()
		{
			Assert.IsTrue(Game.PokemonItemsData.Count > 1);
		}
		[TestMethod]
		public void Initialize_Natures_Test()
		{
			Assert.IsTrue(Game.InitNatures(), string.Format("NatureData: {0}",Game.NatureData.Count.ToString()));
		}
		[TestMethod]
		public void Natures_Data_IsLoaded()
		{
			Assert.IsTrue(Game.NatureData.Count > 1, string.Format("NatureData: {0}",Game.NatureData.Count.ToString()));
		}
		[TestMethod]
		public void Initialize_Moves_Test()
		{
			Assert.IsTrue(Game.InitMoves(), string.Format("MoveData: {0} | MoveMetaData: {1}",Game.MoveData.Count.ToString(), Game.MoveMetaData.Count.ToString()));
		}
		[TestMethod]
		public void Moves_Data_IsLoaded()
		{
			Assert.IsTrue(Game.MoveData.Count > 1, string.Format("MoveData: {0} | MoveMetaData: {1}",Game.MoveData.Count.ToString(), Game.MoveMetaData.Count.ToString()));
		}
		[TestMethod]
		public void Initialize_Items_Test()
		{
			Assert.IsTrue(Game.InitItems(), string.Format("ItemData: {0}",Game.ItemData.Count.ToString()));
		}
		[TestMethod]
		public void Items_Data_IsLoaded()
		{
			Assert.IsTrue(Game.ItemData.Count > 1, string.Format("ItemData: {0}",Game.ItemData.Count.ToString()));
		}
		[TestMethod]
		public void Initialize_Berries_Test()
		{
			Assert.IsTrue(Game.InitBerries(), string.Format("BerryData: {0}",Game.BerryData.Count.ToString()));
		}
		[TestMethod]
		public void Berries_Data_IsLoaded()
		{
			Assert.IsTrue(Game.BerryData.Count > 1, string.Format("BerryData: {0}",Game.BerryData.Count.ToString()));
		}
		[TestMethod]
		public void Initialize_Regions_Test()
		{
			Assert.IsTrue(Game.InitRegions(), string.Format("RegionData: {0}",Game.RegionData.Count.ToString()));
		}
		[TestMethod]
		public void Regions_Data_IsLoaded()
		{
			Assert.IsTrue(Game.RegionData.Count > 1, string.Format("RegionData: {0}",Game.RegionData.Count.ToString()));
		}
		[TestMethod]
		public void Initialize_Types_Test()
		{
			Assert.IsTrue(Game.InitTypes(), string.Format("TypeData: {0}",Game.TypeData.Count.ToString()));
		}
		[TestMethod]
		public void Types_Data_IsLoaded()
		{
			Assert.IsTrue(Game.TypeData.Count > 1, string.Format("TypeData: {0}",Game.TypeData.Count.ToString()));
		}
		[TestMethod]
		public void Initialize_Locations_Test()
		{
			Assert.IsTrue(Game.InitLocations(), string.Format("LocationData: {0} | AreaData: {1}",Game.LocationData.Count.ToString(), Game.AreaData.Count.ToString()));
		}
		[TestMethod]
		public void Locations_Data_IsLoaded()
		{
			Assert.IsTrue(Game.LocationData.Count > 1, string.Format("LocationData: {0} | AreaData: {1}",Game.LocationData.Count.ToString(), Game.AreaData.Count.ToString()));
		}
		[TestMethod]
		public void Initialize_Encounters_Test()
		{
			//Set Area Id THEN Test if Encounters loaded
			Assert.IsTrue(Game.InitEncounters(), string.Format("EncounterData: {0} | MethodData: {1}",Game.EncounterData.Count.ToString(), Game.MethodData.Count.ToString()));
		}
		[TestMethod]
		public void Encounters_Data_IsLoaded()
		{
			//Set Area Id THEN Test if Encounters loaded
			//Assert.IsTrue(Game.EncounterData.Count > 1, string.Format("EncounterData: {0} | MethodData: {1}",Game.EncounterData.Count.ToString(), Game.MethodData.Count.ToString()));
			Assert.Inconclusive("Set Area Id THEN Test if Encounters loaded");
		}
		[TestMethod]
		public void Initialize_Trainers_Test()
		{
			Assert.IsTrue(Game.InitTrainers(), string.Format("TrainerMetaData: {0}",Game.TrainerMetaData.Count.ToString()));
		}
		[TestMethod]
		public void Trainers_Data_IsLoaded()
		{
			Assert.IsTrue(Game.TrainerMetaData.Count > 1, string.Format("TrainerMetaData: {0}",Game.TrainerMetaData.Count.ToString()));
		}
		#endregion

		#region PlayerMovement
		//[TestMethod]
		//public void Test_UseBike_While_SurfOrFly_Fails()
		//{
		//	Assert.Inconclusive();
		//}
		//[TestMethod]
		//public void Test_UseBike_While_Surf_Fails()
		//{
		//	Assert.Inconclusive();
		//}
		#endregion

		#region SaveData
		private static string _SaveStateFromFile { get; set; }

		/// <summary>
		/// Create Dummy Data and Save to File
		/// </summary>
		static GameTest()
		{
			//Create 3 SaveState of Dummy Data

			//Save to File
			//Game.Save();

			//Delete one
		}

		/// <summary>
		/// Test will confirm if saving content to file without error will pass or fail
		/// Other test will confirm if data can be read without errors
		/// </summary>
		public void Save_GameState_Data()
		{
			//FrameworkTest creates a save file identical to this
			//If this method's save file is equal to _SaveStateFromFile
			//Confirm remaining 2 SaveState
		}

		public void Delete_Save_Data_From_SaveFile()
		{

		}

		public void Load_Pokemon_Data_From_SaveFile()
		{

		}

		public void Load_PC_Data_From_SaveFile()
		{

		}

		public void Load_Party_Data_From_SaveFile()
		{
		}

		public void Load_SaveStateVersion_Data_From_SaveFile()
		{
		}
		#endregion

		#region Misc
		[TestMethod]
		public void Test_SeriPokemon_To_Pokemon()
		{
			PokemonUnity.Monster.Pokemon pkmn = new PokemonUnity.Monster.Pokemon(PokemonUnity.Pokemons.ABRA);
			PokemonUnity.Saving.SerializableClasses.SeriPokemon seri = pkmn;
			PokemonUnity.Monster.Pokemon p = seri;
			Assert.AreEqual<PokemonUnity.Monster.Pokemon>(pkmn, p);
		}
		#endregion
	}
}