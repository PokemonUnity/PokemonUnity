using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Move;
using PokemonUnity.Item;

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

        [TestMethod]//[ExpectedException(typeof(Exception))]
        public void Pokemon_SetHP_GreaterThan_MaxHP_Equals_MaxHP() //{ Assert.Inconclusive(); }
        {
            Pokemon pokemon = new Pokemon();
            pokemon.HP = pokemon.TotalHP + 1;
            /*try
            {
                pokemon.HP = pokemon.TotalHP + 1;
                Assert.Fail();
            }
            catch (Exception e)
            {
                //Assert.AreEqual(error, pokemon.HP);
                Assert.IsTrue(e is Exception);
            }*/
            Assert.AreEqual(pokemon.TotalHP, pokemon.HP);
        }

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
        [TestMethod]
        public void Pokemon_TestPokemon_SetForm_To_Form2()
        {
            Pokemon pokemon = new Pokemon(Pokemons.NONE);
            pokemon.Form = 2;
            Assert.AreEqual("test1", pokemon.Name);
        }
        [TestMethod]
        public void Pokemon_TestPokemon_SetForm_IncreasesAtkStats()
        {
            Pokemon pokemon = new Pokemon(Pokemons.NONE);
            pokemon.Form = 1;
            Assert.Inconclusive();//Assert.AreEqual(10, pokemon.ATK);
        }
        #endregion

        #region
        #endregion
    }
    /// <summary>
    /// Database sample on how to write code for different componenets of your database.
    /// </summary>
    [TestClass]
    public class FrameworkTest
    {
        Pokemon.PokemonData[] Database = new Pokemon.PokemonData[] {
            new Pokemon.PokemonData( Id: Pokemons.NONE, regionalDex: new int[1], type1: Types.NONE, type2: Types.NONE, ability1: Abilities.NONE, ability2: Abilities.NONE, hiddenAbility: Abilities.NONE,
                        genderRatio: GenderRatio.AlwaysMale /*0f*/, catchRate: 100, eggGroup1: EggGroups.NONE, eggGroup2: EggGroups.NONE, hatchTime: 1000,
                        height: 10f, weight: 150f, baseExpYield: 15, levelingRate: LevelingRate.ERRATIC,                        
                        pokedexColor: Color.NONE, baseFriendship: 50,
                        baseStatsHP: 10, //baseStatsATK: 5, baseStatsDEF: 5, baseStatsSPA: 5, baseStatsSPD: 5, baseStatsSPE: 5,
                        luminance: 0f, movesetLevels: new int[] { 1,2,3 }, movesetMoves: new Moves[4], tmList: null, 
                        evolution: new IPokemonEvolution[] {  new Pokemon.PokemonData.PokemonEvolution(Pokemons.ABRA, EvolutionMethod.Deaths), new Pokemon.PokemonData.PokemonEvolution<int>(Pokemons.ABRA, EvolutionMethod.Deaths, 25) },
                        //evolutionID: null, evolutionLevel: null, evolutionMethod: null, forms: 4,
                        heldItem: null) 
        };
        #region PokemonData
        [TestMethod]
        public void PokemonDatabase()
        {
        }
        #endregion

        #region
        #endregion
    }
}
