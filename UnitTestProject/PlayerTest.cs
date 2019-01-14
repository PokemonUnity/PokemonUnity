using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Attack;
using PokemonUnity.Item;

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