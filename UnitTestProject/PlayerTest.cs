using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Attack;
using PokemonUnity.Item;
using PokemonUnity.Pokemon;
using PokemonUnity.Attack;
using PokemonUnity.Item;
using PokemonUnity.Saving;
using PokemonUnity.Saving.Location;
using System.Collections.Generic;

namespace Tests
{
    [TestClass] //ToDo: Move Test into a PlayerTest.cs
    public class PlayerTest
    {
        [TestMethod] //Game isnt automatically saved, just because player is created. Let them save game from in-game menu
        public void Player_NewPlayer_IsNot_EqualTo_SaveFile()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }

        [TestMethod]
        public void Create_New_Save_File()
        {
            SaveData newSave;

            #region Fill The Save File
            string saveName = "First Save";
            int activeScene = 2;

            string playerName = "Red";
            int trainerID = 55323;
            int secretID = 64123;
            bool isMale = false;

            bool?[] pokedex = new bool?[] { null, false, true, false, null };
            TimeSpan playerTime = new TimeSpan(4, 20, 53);

            SerializableVector3 playerPosition = new SerializableVector3(0, 1, 0);
            int playerDirection = 2;
            SerializableVector3 followerPosition = new SerializableVector3(1, 0, 0);
            int followerDirection = 1;

            Pokemon[] playerParty = new Pokemon[]
            {
                new Pokemon(Pokemons.CRANIDOS),
                new Pokemon(Pokemons.UMBREON),
                new Pokemon(Pokemons.TURTWIG)
            };
            Pokemon[,] playerPC = new Pokemon[,]
            {
                { new Pokemon(Pokemons.EMPOLEON), new Pokemon(Pokemons.MUNCHLAX) },
                { new Pokemon(Pokemons.MURKROW), new Pokemon(Pokemons.PILOSWINE) }
            };
            //Don't know how to initialize the Items List<> yet, leaving this for later
            List<Items> playerBag = new List<Items>();

            List<SaveEvent> eventList = new List<SaveEvent>();
            eventList.Add(new SaveEvent(SaveEventType.ITEM, "Item - GreatBall", new SerializableVector3(4, 0, 2), 2));
            eventList.Add(new SaveEvent(SaveEventType.ITEM, "Item - MasterBall", new SerializableVector3(4, 1, 9), 9));
            eventList.Add(new SaveEvent(SaveEventType.ITEM, "Item - RareCandy", new SerializableVector3(2, 7, 3), 2));

            newSave = new SaveData
            (
                saveName, activeScene,
                playerName, trainerID, secretID, isMale,
                pokedex, playerTime,
                playerPosition, playerDirection,
                followerPosition, followerDirection,
                playerParty, playerPC, playerBag,
                eventList
            );
            #endregion

            Assert.IsNotNull(newSave);
        }

        //Check to see if no Exceptions are thrown
        [TestMethod]
        public void Save_Into_File()
        {
            SaveData newSave;

            #region Fill The Save File
            string saveName = "First Save";
            int activeScene = 2;

            string playerName = "Red";
            int trainerID = 55323;
            int secretID = 64123;
            bool isMale = false;

            bool?[] pokedex = new bool?[] { null, false, true, false, null };
            TimeSpan playerTime = new TimeSpan(4, 20, 53);

            SerializableVector3 playerPosition = new SerializableVector3(0, 1, 0);
            int playerDirection = 2;
            SerializableVector3 followerPosition = new SerializableVector3(1, 0, 0);
            int followerDirection = 1;

            Pokemon[] playerParty = new Pokemon[]
            {
                new Pokemon(Pokemons.CRANIDOS),
                new Pokemon(Pokemons.UMBREON),
                new Pokemon(Pokemons.TURTWIG)
            };
            Pokemon[,] playerPC = new Pokemon[,]
            {
                { new Pokemon(Pokemons.EMPOLEON), new Pokemon(Pokemons.MUNCHLAX) },
                { new Pokemon(Pokemons.MURKROW), new Pokemon(Pokemons.PILOSWINE) }
            };
            //Don't know how to initialize the Items List<> yet, leaving this for later
            List<Items> playerBag = new List<Items>();

            List<SaveEvent> eventList = new List<SaveEvent>();
            eventList.Add(new SaveEvent(SaveEventType.ITEM, "Item - GreatBall", new SerializableVector3(4, 0, 2), 2));
            eventList.Add(new SaveEvent(SaveEventType.ITEM, "Item - MasterBall", new SerializableVector3(4, 1, 9), 9));
            eventList.Add(new SaveEvent(SaveEventType.ITEM, "Item - RareCandy", new SerializableVector3(2, 7, 3), 2));

            newSave = new SaveData
            (
                saveName, activeScene,
                playerName, trainerID, secretID, isMale,
                pokedex, playerTime,
                playerPosition, playerDirection,
                followerPosition, followerDirection,
                playerParty, playerPC, playerBag,
                eventList
            );
            #endregion

            //If Overwrite doesn't find the file, it'll automatically save it
            SaveManager.Overwrite(newSave, 0);
        }

        [TestMethod]
        public void Load_Save_File()
        {
            #region Save File Setup
            SaveData newSave;

            #region Fill The Save File
            string saveName = "First Save";
            int activeScene = 2;

            string playerName = "Red";
            int trainerID = 55323;
            int secretID = 64123;
            bool isMale = false;

            bool?[] pokedex = new bool?[] { null, false, true, false, null };
            TimeSpan playerTime = new TimeSpan(4, 20, 53);

            SerializableVector3 playerPosition = new SerializableVector3(0, 1, 0);
            int playerDirection = 2;
            SerializableVector3 followerPosition = new SerializableVector3(1, 0, 0);
            int followerDirection = 1;

            Pokemon[] playerParty = new Pokemon[]
            {
                new Pokemon(Pokemons.CRANIDOS),
                new Pokemon(Pokemons.UMBREON),
                new Pokemon(Pokemons.TURTWIG)
            };
            Pokemon[,] playerPC = new Pokemon[,]
            {
                { new Pokemon(Pokemons.EMPOLEON), new Pokemon(Pokemons.MUNCHLAX) },
                { new Pokemon(Pokemons.MURKROW), new Pokemon(Pokemons.PILOSWINE) }
            };
            //Don't know how to initialize the Items List<> yet, leaving this for later
            List<Items> playerBag = new List<Items>();

            List<SaveEvent> eventList = new List<SaveEvent>();
            eventList.Add(new SaveEvent(SaveEventType.ITEM, "Item - GreatBall", new SerializableVector3(4, 0, 2), 2));
            eventList.Add(new SaveEvent(SaveEventType.ITEM, "Item - MasterBall", new SerializableVector3(4, 1, 9), 9));
            eventList.Add(new SaveEvent(SaveEventType.ITEM, "Item - RareCandy", new SerializableVector3(2, 7, 3), 2));

            newSave = new SaveData
            (
                saveName, activeScene,
                playerName, trainerID, secretID, isMale,
                pokedex, playerTime,
                playerPosition, playerDirection,
                followerPosition, followerDirection,
                playerParty, playerPC, playerBag,
                eventList
            );
            #endregion

            SaveManager.Overwrite(newSave, 0);
            #endregion
            Assert.IsNotNull(SaveManager.GetSave(0));
        }

        [TestMethod]
        public void Compare_Save_Files()
        {
            SaveData newSave;

            #region Fill newSave
            string saveName = "First Save";
            int activeScene = 2;

            string playerName = "Red";
            int trainerID = 55323;
            int secretID = 64123;
            bool isMale = false;

            bool?[] pokedex = new bool?[] { null, false, true, false, null };
            TimeSpan playerTime = new TimeSpan(4, 20, 53);

            SerializableVector3 playerPosition = new SerializableVector3(0, 1, 0);
            int playerDirection = 2;
            SerializableVector3 followerPosition = new SerializableVector3(1, 0, 0);
            int followerDirection = 1;

            Pokemon[] playerParty = new Pokemon[]
            {
                new Pokemon(Pokemons.CRANIDOS),
                new Pokemon(Pokemons.UMBREON),
                new Pokemon(Pokemons.TURTWIG)
            };
            Pokemon[,] playerPC = new Pokemon[,]
            {
                { new Pokemon(Pokemons.EMPOLEON), new Pokemon(Pokemons.MUNCHLAX) },
                { new Pokemon(Pokemons.MURKROW), new Pokemon(Pokemons.PILOSWINE) }
            };
            //Don't know how to initialize the Items List<> yet, leaving this for later
            List<Items> playerBag = new List<Items>();

            List<SaveEvent> eventList = new List<SaveEvent>();
            eventList.Add(new SaveEvent(SaveEventType.ITEM, "Item - GreatBall", new SerializableVector3(4, 0, 2), 2));
            eventList.Add(new SaveEvent(SaveEventType.ITEM, "Item - MasterBall", new SerializableVector3(4, 1, 9), 9));
            eventList.Add(new SaveEvent(SaveEventType.ITEM, "Item - RareCandy", new SerializableVector3(2, 7, 3), 2));

            newSave = new SaveData
            (
                saveName, activeScene,
                playerName, trainerID, secretID, isMale,
                pokedex, playerTime,
                playerPosition, playerDirection,
                followerPosition, followerDirection,
                playerParty, playerPC, playerBag,
                eventList
            );
            #endregion

            SaveManager.Overwrite(newSave, 0);

            SaveData loadedData = SaveManager.GetSave(0);

            #region Assert On Save Values
            if (newSave.BuildVersion != loadedData.BuildVersion)
                Assert.Fail("Build versions are not the same");

            if (newSave.SaveName != loadedData.SaveName)
                Assert.Fail("SaveNames are not the same");

            if (newSave.TimeCreated != loadedData.TimeCreated)
                Assert.Fail("TimeCreated are not the same");

            if (newSave.PlayerName != loadedData.PlayerName)
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

            if (newSave.PlayerTime != loadedData.PlayerTime)
                Assert.Fail("PlayerTimes are not the same");

            if (
                (newSave.PlayerPosition.x != loadedData.PlayerPosition.x) &&
                (newSave.PlayerPosition.y != loadedData.PlayerPosition.y) &&
                (newSave.PlayerPosition.z != loadedData.PlayerPosition.z)
                )
                Assert.Fail("PlayerPositions are not the same");
            if (newSave.PlayerDirection != loadedData.PlayerDirection)
                Assert.Fail("PlayerDirections are not the same");

            if (
                (newSave.FollowerPosition.x != loadedData.FollowerPosition.x) &&
                (newSave.FollowerPosition.y != loadedData.FollowerPosition.y) &&
                (newSave.FollowerPosition.z != loadedData.FollowerPosition.z)
                )
                Assert.Fail("FollowerPositions are not the same");
            if (newSave.FollowerDirection != loadedData.FollowerDirection)
                Assert.Fail("FollowerDirections are not the same");

            if (newSave.ActiveScene != loadedData.ActiveScene)
                Assert.Fail("ActiveScenes are no the same");

            if (newSave.PlayerParty.Length != loadedData.PlayerParty.Length)
                Assert.Fail("PlayerPartys are not the same, newSave contains: " + newSave.PlayerParty.Length + ", whilst loadedData contains: " + loadedData.PlayerParty.Length);
            for (int i = 0; i < newSave.PlayerParty.Length; i++)
            {
                if (newSave.PlayerParty[i].Species != loadedData.PlayerParty[i].Species)
                    Assert.Fail("PlayerParty's are not the same on index: " + i);
            }

            /*if (newSave.PC.GetUpperBound(0) != loadedData.PC.GetUpperBound(0))
                Assert.Fail("PC's are not the same on bound: 0");
            if (newSave.PC.GetUpperBound(1) != loadedData.PC.GetUpperBound(1))
                Assert.Fail("PC's are not the same on bound: 1");
            for (int i = 0; i < newSave.PC.GetUpperBound(0); i++)
            {
                for (int j = 0; j < newSave.PC.GetUpperBound(1); j++)
                {
                    if (newSave.PC[i, j].Species != loadedData.PC[i, j].Species)
                        Assert.Fail("PC's are not the same on indexes: " + i + ", " + j);
                }
            }*/

            if (newSave.PlayerBag.Count != loadedData.PlayerBag.Count)
                Assert.Fail("PlayerBag's sizes are not the same");
            for (int i = 0; i < newSave.PlayerBag.Count; i++)
            {
                if (newSave.PlayerBag[i] != loadedData.PlayerBag[i])
                    Assert.Fail("PlayerBag's are not the same on index: " + i);
            }
            #endregion
        }

        #region Player Properties
        [TestMethod]
        public void Player_Name_Not_Null()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }
        [TestMethod]
        public void Player_Badge_Count()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }
        [TestMethod]
        public void Player_Badge_ShouldEqual_GymIsDefeated()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }
        [TestMethod] //ToDo: Maybe this test should be duplicated on TrainerTest too
        public void Player_Party_NotNull()
        {
            //Player trainer = new Player();
            //Null party is not the same thing as Pokemon.NONE. 
            //Party should be six Pokemons(type) regardless of value, just not null
            Assert.Fail("No Player Test");
        }
        [TestMethod] //Maybe Simulate a battle, and see if number changes? Seen/Caught
        public void Player_Pokedex_Count()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }
        [TestMethod]
        public void Player_HoursPlayed_NotNull()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }
        [TestMethod]
        public void Player_HoursPlayed_Test()
        {
            //Player trainer = new Player();
            //ToDo: System.Timer, create a player, wait a while, test again and see if it changes?
            Assert.Fail("No Player Test");
        }
        #endregion

        #region Game Start => Loading A Save File
        [TestMethod]
        public void Player_Load_Name()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }
        [TestMethod]
        public void Player_Load_Badges()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }
        [TestMethod]
        public void Player_Load_Party()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }
        [TestMethod]
        public void Player_Load_Pokedex()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }
        [TestMethod]
        public void Player_Load_HoursPlayed()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }
        [TestMethod]
        public void Player_Load_Settings_Language()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }
        [TestMethod]
        public void Player_Load_Settings_Text_WindowBorder()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }
        [TestMethod]
        public void Player_Load_Settings_Text_DialogBorder()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }
        [TestMethod]
        public void Player_Load_Settings_VolumeValues()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }
        [TestMethod]
        public void Player_Load_Settings_TextSpeed()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }
        [TestMethod] //ToDo: Should be map data, and stuff... Map0 == new game (professsor intro speech)
        public void Player_Load_SpawnLocation()
        {
            //Player trainer = new Player();
            Assert.Fail("No Player Test");
        }
        #endregion
    }

}