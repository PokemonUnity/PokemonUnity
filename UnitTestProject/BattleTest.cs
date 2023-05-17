using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace Tests
{
    [TestClass]
    public class BattleTest
    {
        #region EncounterChances
        [TestMethod]
		public void Battle_WildPokemon_Encounter_Chance()
		{
			//Sample wild encounter trigger
			//Test chances of encounter a specific pokemon
			Assert.Inconclusive();
		}
		#endregion

		#region Weather
		//Fake Battle Test: Check Weather Values
		#endregion
	}
}
