﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonUnity;
using PokemonUnity.Character;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Saving;
using PokemonUnity.Utility;
using System.Collections.Generic;
using PokemonUnity.Saving.SerializableClasses;

namespace Tests
{
	[TestClass] 
	public class PlayerTest
	{
		//[TestMethod] 
		//Game isnt automatically saved, just because player is created. Let them save game from in-game menu
		//Not needed anymore because of how save mechanic is set-up, `new player()` doesnt touch it...
		//public void Player_NewPlayer_IsNot_EqualTo_SaveFile()
		//{
		//    //Player trainer = new Player();
		//    Assert.Fail("No Player Test");
		//}

		public static SaveData New_Save_File_With_Standard_Unit_Test_Values()
		{
			SaveData newSave;
			#region Fill The Save File
			//string saveName = "First Save";
			int activeScene = 2;

			Languages language = Languages.English;
			byte windowBorder = 2;
			byte dialogBorder = 9;
			byte textSpeed = 1;
			float mvol = 0.4f;
			float svol = 0.9f;

			string playerName = "Red";
			int trainerID = 55323;
			int secretID = 64123;
			bool isMale = true;
			TrainerData red = new TrainerData(playerName, isMale, tID: trainerID, sID: secretID);

			Pokemon[] playerParty = new Pokemon[]
			{
				new Pokemon(Pokemons.CRANIDOS).SetCatchInfos(red),
				new Pokemon(Pokemons.UMBREON).SetCatchInfos(red),
				new Pokemon(Pokemons.TURTWIG).SetCatchInfos(red),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE)
			};
			Game.GameData.Player = new Player(red, playerParty);
			//red = Game.GameData.Player.Trainer;

			//bool?[] pokedex = new bool?[] { null, false, true, false, null };
			byte[,] pokedex = new byte[,] { { 0, 0 } };
			TimeSpan playerTime = new TimeSpan(4, 20, 53);
			//SeriV3 playerPosition = new SeriV3(0, 1, 0);
			//IVector playerPosition = new Vector(0, 1, 0);
			//int playerDirection = 2;
			//IQuaternion playerDirection = new Quaternion(0, 1, 0, 0);
			//SeriV3 followerPosition = new SeriV3(1, 0, 0);
			//int followerDirection = 1;
			//Pokemon[,] playerPC = new Pokemon[Core.STORAGEBOXES, 30];
			Pokemon[][] playerPC = new Pokemon[Core.STORAGEBOXES][];//, 30];
			//for (int i = 0; i < playerPC.GetLength(1); i++)
			for (int i = 0; i < playerPC.Length; i++)
			{
				playerPC[i] = new Pokemon[30];
				//for (int j = 0; j < playerPC.GetLength(0); j++)
				for (int j = 0; j < playerPC[i].Length; j++)
				{
					//This should be done by aleady
					//i believe the default value on new Pokemon[,] will return none
					//playerPC[i, j] = new Pokemon(Pokemons.NONE);
					playerPC[i][j] = new Pokemon(Pokemons.NONE);
				}
			}
			playerPC[0][3] = new Pokemon(Pokemons.CRANIDOS).SetCatchInfos(red);
			playerPC[1][2] = new Pokemon(Pokemons.EMPOLEON).SetCatchInfos(red);
			playerPC[3][3] = new Pokemon(Pokemons.GARCHOMP).SetCatchInfos(red);
			
			//Game.Bag_Items = new List<Items>()
			List<Items> playerBag = new List<Items>()//Game.Bag_Items;
			//Created random inventory list for player bag
			{
				Items.ADAMANT_ORB,
				Items.ACRO_BIKE,	
				Items.POKE_BALL,	
				Items.POKE_BALL,	
				Items.POKE_BALL,	
				Items.POKE_BALL,	
				Items.POKE_BALL,	
				Items.GREAT_BALL
			};

			List<KeyValuePair<Items, int>> playerPcItem = new List<KeyValuePair<Items, int>>()
			//Created random inventory list for player bag
			{
				new KeyValuePair<Items, int>( Items.ADAMANT_ORB,    1 ),
				new KeyValuePair<Items, int>( Items.ACRO_BIKE,      1 ),
				new KeyValuePair<Items, int>( Items.POKE_BALL,      5 ),
				new KeyValuePair<Items, int>( Items.POKE_BALL,		0 ),
				new KeyValuePair<Items, int>( Items.POKE_BALL,		0 ),
				new KeyValuePair<Items, int>( Items.POKE_BALL,		0 ),
				new KeyValuePair<Items, int>( Items.POKE_BALL,		0 ),
				new KeyValuePair<Items, int>( Items.GREAT_BALL,     4 )
			};

			PokemonUnity.Character.PC pc = new PokemonUnity.Character.PC(pkmns: playerPC, items: playerPcItem.ToArray(), names: new string[] { "Box 1", "Box 2", "Grassy Box" }, textures: new int[] { 0, 1, 2 });
			
			//debug code to test badge box
			/*SaveDataOld.currentSave.gymsEncountered = new bool[]
			{
				true, true, false, true, true, true,
				false, false, false, false, false, false
			};
			SaveDataOld.currentSave.gymsBeaten = new bool[]
			{
				true, true, false, false, false, true,
				false, false, false, false, false, false
			};*/
			//System.DateTime?[] gymsBeatTime = new System.DateTime?[]
			//{
			//	new System.DateTime(System.DateTime.Now.Year, 4, 27) /*"Apr. 27th, 2015"*/, new System.DateTime(System.DateTime.Now.Year, 4, 30) /*"Apr. 30th, 2015"*/, null, null, null, new System.DateTime(System.DateTime.Now.Year, 5,1) /*"May. 1st, 2015"*/,
			//	null, null, null, null, null, null
			//};

			KeyValuePair<GymBadges, DateTime?>[] gymBadge = new KeyValuePair<GymBadges, DateTime?>[]
			{
				new KeyValuePair<GymBadges, DateTime?>(GymBadges.Rock, new System.DateTime(System.DateTime.Now.Year, 4, 27) ) //"Apr. 27th, 2015"
			};

			//List<SaveEvent> eventList = new List<SaveEvent>();
			//eventList.Add(new SaveEvent(SaveEventType.ITEM, "Item - GreatBall", new SeriV3(4, 0, 2), 2));
			//eventList.Add(new SaveEvent(SaveEventType.ITEM, "Item - MasterBall", new SeriV3(4, 1, 9), 9));
			//eventList.Add(new SaveEvent(SaveEventType.ITEM, "Item - RareCandy", new SeriV3(2, 7, 3), 2));

			newSave = new SaveData
			(
				//saveName,
		
				language: language,
				windowBorder: windowBorder,
				dialogBorder: dialogBorder,
				textSpeed: textSpeed,
				mvol: mvol,
				svol: svol
		
				//name: playerName,
				//trainer: trainerID,
				//secret: secretID,
				//gender: isMale,
				//
				//pokedex: pokedex,
				//time: playerTime,
				//position: playerPosition,
				//
				//map: activeScene,
				//
				//party: red.Party.Serialize(), 
				//pc: new SeriPC(pc),
				//bag: playerBag
		
				//,eventLog: eventList
			);
			GameState g = new GameState
			(
				player: new Player(
					name: playerName
					,gender: isMale
					,party: playerParty
					,bag: playerBag.ToArray()
					,trainerid: trainerID
					,secretid: secretID
					,pc_poke: playerPC//pc.AllBoxes
					,pc_items: playerPcItem.ToArray()//pc.Items
					,pc_box: pc.ActiveBox
					,pc_names: pc.BoxNames
					,pc_textures: pc.BoxTextures
					,money: 2481
					,repel: 439
					,rival: "blue"
					//,dex: pokedex
					,time: playerTime
					,position: playerPosition
					//,rotation: playerDirection
					,follower: 2
					//,creator:
					,map: activeScene
					//,pokecenter:
					,gym: gymBadge
				)

				//,name: playerName

				//,pc: new SeriPC(pc)
			)
			{
				//PlayerItemData		= state.PlayerItemData
				//,PlayerBerryData	= state.PlayerBerryData
				//,PlayerNPCData		= state.PlayerNPCData
				//,PlayerApricornData	= state.PlayerApricornData
				//,RepelSteps			= state.Player.RepelSteps
				//,Rival				= state.Rival
				//,PlayerPosition		= state.PlayerPosition
				//,Checkpoint			= (Locations)state.Checkpoint
				//,Player				= state.Player
				//,PC					= state.PlayerPC
				//,Bag				= state.PlayerBag
				//Area = data.ActiveMapId;
				//PC_Poke = data.PC.GetPokemonsFromSeri();
				//PC_boxNames = data.PC.BoxNames;
				//PC_boxTexture = data.PC.BoxTextures;
				//PC_Items = new List<Items>(data.PC.GetItemsFromSeri());
				//Bag_Items = data.PlayerBag;
			};
			#endregion
			//SaveManager.Overwrite(newSave, 0);
			newSave[0] = g;
			return newSave;
		}

		public void Overwrite_New_Save_File_With_Standard_Unit_Test_Values()
		{
			//SaveManager.Overwrite(New_Save_File_With_Standard_Unit_Test_Values(), 0);
			SaveManager.CreateSaveFileAndSerialize(New_Save_File_With_Standard_Unit_Test_Values());
		}

		[TestMethod]
		public void Create_New_Save_File()
		{
			//Assert.Inconclusive("Code is temporarily stripped from project");
			//SaveData newSave = New_Save_File_With_Standard_Unit_Test_Values();
			Overwrite_New_Save_File_With_Standard_Unit_Test_Values();

			//This is a bad test... the variable was just created.
			//Make a file, save it on hard drive, test to see if the value can be stored to a variable
			//Unless i am mistaken on what the test is supposed to be looking for
			//i assume that it is bad because it was matching against inline code
			//rather than actually testing for functionality of code inside assembly...
			//Assert.IsNotNull(newSave);
			Assert.IsTrue(System.IO.File.Exists(SaveManager.playerSave), "Could not find save file");
		}

		//Check to see if no Exceptions are thrown
		[TestMethod]
		public void Save_Into_File()
		{
			//Assert.Inconclusive("Code is temporarily stripped from project");
			//SaveData newSave = New_Save_File_With_Standard_Unit_Test_Values();
			Overwrite_New_Save_File_With_Standard_Unit_Test_Values();

			//If Overwrite doesn't find the file, it'll automatically save it
			//SaveManager.Overwrite(newSave, 0);
			//There was no assert made for this test...
			//Try to test if file exist, or if data can be read/saved without any errors...
			Assert.IsFalse(string.IsNullOrEmpty(System.IO.File.ReadAllText(SaveManager.playerSave)),"Save file returned null or empty string.");
		}

		[TestMethod]
		public void Load_Save_File()
		{
			//Assert.Inconclusive("Code is temporarily stripped from project");
			#region Save File Setup
			SaveData newSave = New_Save_File_With_Standard_Unit_Test_Values();

			//SaveManager.Overwrite(2, newSave);
			SaveManager.CreateSaveFileAndSerialize(newSave);
			#endregion
			//Assert.IsNotNull(SaveManager.GetSaves()[2]);
			Assert.IsNotNull(SaveManager.GetSave());
		}

		[TestMethod]
		public void Compare_Save_Files()
		{
			//Assert.Inconclusive("Code is temporarily stripped from project");
			SaveData newSave = New_Save_File_With_Standard_Unit_Test_Values();

			//SaveManager.Overwrite(newSave, 1);
			SaveManager.CreateSaveFileAndSerialize(newSave);

			SaveData loadedData = SaveManager.GetSave();

			#region Assert On Save Values
			//ToDo: Use AssertCollection to test below in one Assert, instead of many
			/*if (newSave.BuildVersion != loadedData.BuildVersion)
				Assert.Fail("Build versions are not the same");

			//if (newSave.SaveName != loadedData.SaveName)
			//	Assert.Fail("SaveNames are not the same");

			if (newSave.TimeCreated != loadedData.TimeCreated)
				Assert.Fail("TimeCreated are not the same; " + string.Format("Save: {0} | Load: {1}",newSave.TimeCreated, loadedData.TimeCreated));

			if (newSave.GameStates.PlayerName != loadedData.GameStates.PlayerName)
				Assert.Fail("PlayerNames are not the same");

			if (newSave.TrainerID != loadedData.TrainerID)
				Assert.Fail("TrainerID's are not the same");

			if (newSave.SecretID != loadedData.SecretID)
				Assert.Fail("SecretID are not the same");

			if (newSave.IsMale != loadedData.IsMale)
				Assert.Fail("Genders are not the same");

			if (newSave.Pokedex.Length != loadedData.Pokedex.Length)
				Assert.Fail("Pokedex Length are not the same");
			for (int i = 0; i < newSave.Pokedex.Length; i++)
			{
				if (newSave.Pokedex[i] != loadedData.Pokedex[i])
					Assert.Fail("Pokedex entries are not the same on index: " + i);
			}

			if (newSave.PlayTime != loadedData.PlayTime)
				Assert.Fail("PlayerTimes are not the same");

			if (
				(newSave.PlayerPosition.x != loadedData.PlayerPosition.x) &&
				(newSave.PlayerPosition.y != loadedData.PlayerPosition.y) &&
				(newSave.PlayerPosition.z != loadedData.PlayerPosition.z)
			)
				Assert.Fail("PlayerPositions are not the same");
			//if (newSave.PlayerDirection != loadedData.PlayerDirection)
			//	Assert.Fail("PlayerDirections are not the same");

			//if (
			//    (newSave.FollowerPosition.x != loadedData.FollowerPosition.x) &&
			//    (newSave.FollowerPosition.y != loadedData.FollowerPosition.y) &&
			//    (newSave.FollowerPosition.z != loadedData.FollowerPosition.z)
			//    )
			//    Assert.Fail("FollowerPositions are not the same");
			//if (newSave.FollowerDirection != loadedData.FollowerDirection)
			//	Assert.Fail("FollowerDirections are not the same");

			if (newSave.ActiveMapId != loadedData.ActiveMapId)
				Assert.Fail("ActiveScenes are no the same");

			if (newSave.PlayerParty.Length != loadedData.PlayerParty.Length)
				Assert.Fail("PlayerPartys are not the same, newSave contains: " + newSave.PlayerParty.Length + ", whilst loadedData contains: " + loadedData.PlayerParty.Length);
			for (int i = 0; i < newSave.PlayerParty.Length; i++)
			{
				if (newSave.PlayerParty[i].Species != loadedData.PlayerParty[i].Species)
					Assert.Fail("PlayerParty's are not the same on index: " + i);
			}

			if (newSave.PlayerPC.GetUpperBound(0) != loadedData.PlayerPC.GetUpperBound(0))
				Assert.Fail("PC's are not the same on bound: 0");
			if (newSave.PlayerPC.GetUpperBound(1) != loadedData.PlayerPC.GetUpperBound(1))
				Assert.Fail("PC's are not the same on bound: 1");
			for (int i = 0; i < newSave.PlayerPC.GetUpperBound(0); i++)
			{
				for (int j = 0; j < newSave.PlayerPC.GetUpperBound(1); j++)
				{
					if (newSave.PlayerPC[i, j].Species != loadedData.PlayerPC[i, j].Species)
						Assert.Fail("PC's are not the same on indexes: " + i + ", " + j);
				}
			}

			if (newSave.PlayerBag.Length != loadedData.PlayerBag.Length)
				Assert.Fail("PlayerBag's sizes are not the same");
			for (int i = 0; i < newSave.PlayerBag.Length; i++)
			{
				if (newSave.PlayerBag[i] != loadedData.PlayerBag[i])
					Assert.Fail("PlayerBag's are not the same on index: " + i);
			}*/
			#endregion
			//CollectionAssert.AreNotEqual( 
			//	new Pokemons[] { Pokemons.CHARMANDER, Pokemons.NONE, Pokemons.NONE, Pokemons.NONE, Pokemons.NONE, Pokemons.NONE },
			//	new Pokemons[] { player.Party[0].Species, player.Party[1].Species, player.Party[2].Species, player.Party[3].Species, player.Party[4].Species, player.Party[5].Species }
			//);
			Assert.AreEqual(newSave, loadedData); 
		}

		#region Player Properties
		[TestMethod]
		public void Player_Name_Not_Null()
		{
			//Player trainer = new Player();
			Assert.Inconclusive("No Player Test");
		}
		[TestMethod]
		public void Player_Badge_Count()
		{
			//Player trainer = new Player();
			Assert.Inconclusive("No Player Test");
		}
		[TestMethod]
		public void Player_Badge_ShouldEqual_GymIsDefeated()
		{
			//Player trainer = new Player();
			Assert.Inconclusive("No Player Test");
		}
		[TestMethod] //ToDo: Maybe this test should be duplicated on TrainerTest too
		public void Player_Party_NotNull()
		{
			//Player trainer = new Player();
			//Null party is not the same thing as Pokemon.NONE. 
			//Party should be six Pokemons(type) regardless of value, just not null
			Assert.Inconclusive("No Player Test");
		}
		[TestMethod] //Maybe Simulate a battle, and see if number changes? Seen/Caught
		public void Player_Pokedex_Count()
		{
			//Player trainer = new Player();
			Assert.Inconclusive("No Player Test");
		}
		[TestMethod]
		public void Player_HoursPlayed_NotNull()
		{
			//Player trainer = new Player();
			Assert.Inconclusive("No Player Test");
		}
		[TestMethod]
		public void Player_HoursPlayed_Test()
		{
			//Player trainer = new Player();
			//ToDo: System.Timer, create a player, wait a while, test again and see if it changes?
			Assert.Inconclusive("No Player Test");
		}
		#endregion

		#region Game Start => Loading A Save File
		[TestMethod]
		public void Player_Load_Name()
		{
			//Assert.Inconclusive("Code is temporarily stripped from project");
			Overwrite_New_Save_File_With_Standard_Unit_Test_Values();

			GameState newSave = SaveManager.GetSave()[0];

			Assert.AreEqual("Red", newSave.PlayerName);
		}
		//[TestMethod]
		//public void Player_Load_Badges()
		//{
		//    //Player trainer = new Player();
		//    Assert.Fail("No Player Test");
		//}
		[TestMethod]
		public void SaveData_Load_Party()
		{
			//Assert.Inconclusive("Code is temporarily stripped from project");
			Overwrite_New_Save_File_With_Standard_Unit_Test_Values();

			byte saveSlot = 0;
			GameState newSave = SaveManager.GetSave()[saveSlot];
			////Party of pokemons should still equal 6, even if other three are empty...
			//Pokemon[] expectedPlayerParty = new Pokemon[]
			//{
			//    new Pokemon(Pokemons.CRANIDOS),
			//    new Pokemon(Pokemons.UMBREON),
			//    new Pokemon(Pokemons.TURTWIG),
			//	//add null values until 6 pokemons is met, otherwise the test may cause false results
			//	new Pokemon(Pokemons.NONE),
			//	new Pokemon(Pokemons.NONE),
			//	new Pokemon(Pokemons.NONE)
			//};
			//Pokemon[] actualPlayerParty = new Pokemon[newSave.PlayerParty.Length];
			//for (int i = 0; i < actualPlayerParty.Length; i++)
			//{
			//    actualPlayerParty[i] = newSave.PlayerParty[i];
			//}
			////Dont assert on party length, repeating last message but... all parties should be same length...
			//if (expectedPlayerParty.Length != actualPlayerParty.Length)
			//    Assert.Fail("Party Lengths do not match up.");
			//for (int i = 0; i < expectedPlayerParty.Length; i++)
			//{
			//    if (expectedPlayerParty[i].Name != actualPlayerParty[i].Name)
			//        Assert.Fail("Pokemon Party's Pokemon do not match up on ID: " + i);
			//}
			CollectionAssert.AreEqual(new Pokemons[] { Pokemons.CRANIDOS, Pokemons.UMBREON, Pokemons.TURTWIG, Pokemons.NONE, Pokemons.NONE, Pokemons.NONE },
				new Pokemons[] { (Pokemons)newSave.PlayerParty[0].Species, (Pokemons)newSave.PlayerParty[1].Species, (Pokemons)newSave.PlayerParty[2].Species,
					(Pokemons)newSave.PlayerParty[3].Species, (Pokemons)newSave.PlayerParty[4].Species, (Pokemons)newSave.PlayerParty[5].Species }, newSave.PlayerParty.ToString());
		}
		[TestMethod]
		public void Player_Load_Party()
		{
			//Assert.Inconclusive("Code is temporarily stripped from project");
			Overwrite_New_Save_File_With_Standard_Unit_Test_Values();

			byte saveSlot = 0;
			//Game.Save(New_Save_File_With_Standard_Unit_Test_Values(), saveSlot);

			//Game.Load(saveSlot);
			//Game.InitLoadFile()[saveSlot];
			////Party of pokemons should still equal 6, even if other three are empty...
			//Pokemon[] expectedPlayerParty = new Pokemon[]
			//{
			//    new Pokemon(Pokemons.CRANIDOS),
			//    new Pokemon(Pokemons.UMBREON),
			//    new Pokemon(Pokemons.TURTWIG),
			//	//add null values until 6 pokemons is met, otherwise the test may cause false results
			//	new Pokemon(Pokemons.NONE),
			//	new Pokemon(Pokemons.NONE),
			//	new Pokemon(Pokemons.NONE)
			//};
			//Pokemon[] actualPlayerParty = new Pokemon[newSave.PlayerParty.Length];
			//for (int i = 0; i < actualPlayerParty.Length; i++)
			//{
			//    actualPlayerParty[i] = newSave.PlayerParty[i];
			//}
			////Dont assert on party length, repeating last message but... all parties should be same length...
			//if (expectedPlayerParty.Length != actualPlayerParty.Length)
			//    Assert.Fail("Party Lengths do not match up.");
			//for (int i = 0; i < expectedPlayerParty.Length; i++)
			//{
			//    if (expectedPlayerParty[i].Name != actualPlayerParty[i].Name)
			//        Assert.Fail("Pokemon Party's Pokemon do not match up on ID: " + i);
			//}
			CollectionAssert.AreEqual(new Pokemons[] { Pokemons.CRANIDOS, Pokemons.UMBREON, Pokemons.TURTWIG, Pokemons.NONE, Pokemons.NONE, Pokemons.NONE },
				new Pokemons[] { Game.GameData.Player.Party[0].Species, Game.GameData.Player.Party[1].Species, Game.GameData.Player.Party[2].Species,
					Game.GameData.Player.Party[3].Species, Game.GameData.Player.Party[4].Species, Game.GameData.Player.Party[5].Species });
		}
		[TestMethod]
		public void Player_Load_Pokedex()
		{
			Assert.Inconclusive("Need better test...");
			//Assert.Inconclusive("Code is temporarily stripped from project");
			Overwrite_New_Save_File_With_Standard_Unit_Test_Values();

			GameState newSave = SaveManager.GetSave()[0];
			//this is testing for inferior pokedex... but was still functional code.
			//i removed pokedex script because i found the expanded version to be more attractive...
			bool?[] expectedPokedex = new bool?[] { null, false, true, false, null };

			if (expectedPokedex.Length != newSave.Pokedex.Length)
				Assert.Fail("Expected- and actual Pokedex's length didn't match up.");
			for (int i = 0; i < expectedPokedex.Length; i++)
			{
				//may need to redo test again to match active code...
				Assert.Inconclusive("Test needs to be redone to match code changes; Pokedex is no longer bool?[] and is now byte[][].");
				//if (expectedPokedex[i] != newSave.Pokedex[i])
				//	Assert.Fail("Expected- and actual Pokedex's index didn't match up on index: " + i);
			}
		}
		[TestMethod]
		public void Player_Load_HoursPlayed()
		{
			//Assert.Inconclusive("Code is temporarily stripped from project");
			Overwrite_New_Save_File_With_Standard_Unit_Test_Values();

			GameState newSave = SaveManager.GetSave()[0];
			TimeSpan expectedTimeSpan = new TimeSpan(4, 20, 53);

			Assert.AreEqual(expectedTimeSpan, newSave.PlayTime);
		}

		#region Game Settings (different from Player Settings)
		//ToDo: One of these should be good enough... 
		// i dont think we need each variable to pass
		// to validate that the mechanic is functional
		[TestMethod]
		public void Player_Load_Settings_Language()
		{
			//Assert.Inconclusive("Code is temporarily stripped from project");
			Overwrite_New_Save_File_With_Standard_Unit_Test_Values();

			SaveData newSave = SaveManager.GetSave();
			Languages expectedLanguage = Languages.English;

			//We need to cast newSave.Language to the correct variable, since we save it in an (int)
			Assert.AreEqual(expectedLanguage, (Languages)newSave.Language);
			//Assert.Fail("Player Settings are saved separate from Player Game State");
		}
		/*[TestMethod]
		public void Player_Load_Settings_Text_WindowBorder()
		{
			Overwrite_New_Save_File_With_Standard_Unit_Test_Values();

			SaveData newSave = SaveManager.GetSave(0);
			byte expectedWindowBorder = 2;

			//Assert.AreEqual(expectedWindowBorder, newSave.WindowBorder);
			Assert.Fail("Player Settings are saved separate from Player Game State");
		}
		[TestMethod]
		public void Player_Load_Settings_Text_DialogBorder()
		{
			Overwrite_New_Save_File_With_Standard_Unit_Test_Values();

			SaveData newSave = SaveManager.GetSave(0);
			byte expectedDialogBorder = 9;

			//Assert.AreEqual(expectedDialogBorder, newSave.DialogBorder);
			Assert.Fail("Player Settings are saved separate from Player Game State");
		}
		[TestMethod]
		public void Player_Load_Settings_VolumeValues()
		{
			Overwrite_New_Save_File_With_Standard_Unit_Test_Values();

			SaveData newSave = SaveManager.GetSave(0);
			float expectedMvol = 0.4f;
			float expectedSvol = 0.9f;

			//Pretty sure it's only one Assert per test for MS Unit Test. 
			//It wont spit out multiple results, just the one that hits first.
			//Assert.AreEqual(expectedMvol, newSave.mVol);
			//Assert.AreEqual(expectedSvol, newSave.sVol);
			Assert.Fail("Player Settings are saved separate from Player Game State");
		}
		[TestMethod]
		public void Player_Load_Settings_TextSpeed()
		{
			Overwrite_New_Save_File_With_Standard_Unit_Test_Values();

			SaveData newSave = SaveManager.GetSave(0);
			byte expectedTextSpeed = 1;

			//Assert.AreEqual(expectedTextSpeed, newSave.TextSpeed);
			Assert.Fail("Player Settings are saved separate from Player Game State");
		}*/
		#endregion
		//[TestMethod] //ToDo: Should be map data, and stuff... Map0 == new game (professsor intro speech)
		//public void Player_Load_SpawnLocation()
		//{
		//    //Player trainer = new Player();
		//    Assert.Fail("No Player Test");
		//}
		#endregion
	}
}