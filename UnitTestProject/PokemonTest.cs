using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class PokemonTest
    {
        #region HP/Status...
        [TestMethod]
        public void Pokemon_SetHP_To_Zero()
        {
            Pokemon pokemon = new Pokemon();
            pokemon.HP = 0;
            Assert.AreEqual(0, pokemon.HP);
        }

        [TestMethod]
        public void Pokemon_SetHP_Throw_Error_GreaterThan_MaxHP() { Assert.Inconclusive(); }
        /*{
            Pokemon pokemon = new Pokemon();
            pokemon.HP = 25;
            Assert.AreEqual(error, pokemon.HP);
        }*/

        [TestMethod]
        public void Pokemon_SetStatus_To_Burn() { Assert.Inconclusive(); }

        [TestMethod]
        public void Pokemon_FullyHeal() 
        {
            Pokemon pokemon = new Pokemon();
            pokemon.HP = 0;
            pokemon.HealHP();
            Assert.AreEqual(pokemon.TotalHP, pokemon.HP);
        }

        [TestMethod]
        public void Pokemon_MakeFainted() 
        {
            Pokemon pokemon = new Pokemon();
            pokemon.HP = 0;
            Assert.IsTrue(pokemon.isFainted());
        }

        [TestMethod]
        public void Pokemon_SetPokerus_To_Infected()
        {
            Pokemon pokemon = new Pokemon();
            pokemon.GivePokerus();
            Assert.IsTrue(pokemon.PokerusStage.Value);
        }
        #endregion

        #region Level/stats...
        #endregion

        #region Moves...
        #endregion

        #region Misc
        [TestMethod, Ignore]
        public void Pokemon_TestPokemon_SetForm_To_Form2()
        {
            Pokemon pokemon = new Pokemon(PokemonData.Pokemon.NONE);
            pokemon.Form = 2;
            Assert.AreEqual("test1", pokemon.Name);
        }
        #endregion

        #region
        #endregion
    }
}
