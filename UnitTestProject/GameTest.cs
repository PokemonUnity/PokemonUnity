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
		[TestMethod]
		public void Database_Load_Pokemon_Test()
		{
			System.Collections.Generic.List<string[]> s = new System.Collections.Generic.List<string[]>();
			s = PokemonUnity.Game.GetArrayFromSQL("select * from pokemon_views limit 3 --order by id ASC");
			//CollectionAssert.AllItemsAreNotNull(s);
			Assert.AreEqual(3, s.Count);
		}

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
		#endregion
	}
}