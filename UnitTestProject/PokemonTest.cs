using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
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
        public void Pokemon_SetHP_Throw_Error_GreaterThan_MaxHP() { Assert.Fail(); }
        /*{
            Pokemon pokemon = new Pokemon();
            pokemon.HP = 25;
            Assert.AreEqual(error, pokemon.HP);
        }*/

        [TestMethod]
        public void Pokemon_SetStatus_To_Burn() { Assert.Fail(); }

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
            Assert.AreEqual(true, pokemon.isFainted());
        }

        [TestMethod]
        public void Pokemon_SetPokerus_To_Infected()
        {
            Pokemon pokemon = new Pokemon();
            pokemon.GivePokerus();
            Assert.AreEqual(true, pokemon.PokerusStage.Value);
        }
        #endregion

        #region Level/stats...
        #endregion

        #region Moves...
        #endregion

        #region
        #endregion
    }
}
