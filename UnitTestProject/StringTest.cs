using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Character;
using PokemonUnity.Inventory;

namespace Tests
{
    [TestClass]
    public class StringTest
	{
		[TestMethod]
		public void Pokemon_DefaultName_Test()
		{
			Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR, false);
			Assert.AreEqual("BULBASAUR", pokemon.Name); 
		}
        #region Nicknames
		[TestMethod]
		public void Pokemon_Nickname_Test()
		{
			//Name pokemon and confirm if name persist
			Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR, false);
			string nick = "testname";
			pokemon.SetNickname(nick);
			Assert.AreEqual(nick, pokemon.Name);
		}
        [TestMethod]
		public void Trainer_SetPokemon_Nickname()
		{
			//Assert.Inconclusive("Not implemented yet");
			//Trainer trainer = new Trainer(TrainerTypes.PLAYER);// { Party = new Pokemon[] { } };
			Player trainer = new Player();
			//make sure pokemon is not egg, as eggs cannot be named
			Pokemon pokemon = new Pokemon(Pokemons.ABRA, false);
			if (pokemon.IsNicknamed) Assert.Fail("Already named");
			string nick = "testname";
			//Name pokemon and add to trainer party...
			pokemon.SetNickname(nick);
			trainer.addPokemon(pokemon);
			//trainer.Party[0].Name
			Assert.AreEqual(nick, trainer.Party[0].Name);
		}
        #endregion
		//[TestMethod]
		public void Pokemon_Name_Test()
		{
			Assert.AreEqual("Bulbasaur", Pokemons.BULBASAUR.ToString(TextScripts.Name)); 
		}
		//[TestMethod]
		public void Pokemon_FlavorText_Test()
		{
			Assert.AreEqual("Bulbasaur", Pokemons.BULBASAUR.ToString(TextScripts.Description));
		}
		//[TestMethod]
		public void Move_Name_Test()
		{
			Assert.AreEqual("Absorb", Moves.ABSORB.ToString(TextScripts.Name));
		}
		//[TestMethod]
		public void Move_FlavorText_Test()
		{
			Assert.AreEqual("Absorb", Moves.ABSORB.ToString(TextScripts.Description));
		}
		//[TestMethod]
		public void Item_Name_Test()
		{
			Assert.AreEqual("Masterball", Items.MASTER_BALL.ToString(TextScripts.Name));
		}
		//[TestMethod]
		public void Item_FlavorText_Test()
		{
			Assert.AreEqual("Masterball", Items.MASTER_BALL.ToString(TextScripts.Description));
		}
		//[TestMethod]
		public void NPC_Name_Test()
		{
			Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
			//Assert.AreEqual("Bulbasaur", pokemon.Name);
			Assert.Inconclusive("Not implemented yet");
		}
		//[TestMethod]
		public void NPC_Script_Test()
		{
			Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
			//Assert.AreEqual("Bulbasaur", pokemon.Name);
			Assert.Inconclusive("Not implemented yet");
		}
		//[TestMethod]
		public void Script_Color_Test() //Unity thing...
		{
			Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
			//Assert.AreEqual("Bulbasaur", pokemon.Name);
			Assert.Inconclusive("Not implemented yet");
		}
		//[TestMethod]
		public void Dialog_Script_Insert_And_Replace_Test() 
		{
			//Replace placeholder text with correct variable in string
			Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
			//Assert.AreEqual("Bulbasaur", pokemon.Name);
			Assert.Inconclusive("Not implemented yet");
		}
	}
}
