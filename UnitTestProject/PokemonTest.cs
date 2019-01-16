using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Attack;
using PokemonUnity.Item;

namespace Tests
{
    [TestClass]
    public class PokemonTest
    {
        //Create 2 assert test; 1 for regular pokemon, and one for pokemon.NONE
        //Pokemon.NONE cannot receive any changes to data, as it does not exist...

        [TestMethod]
        public void Pokemon_Name_Test()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            Assert.AreEqual("Bulbasaur", pokemon.Name);
        }

        #region HP/Status...
        [TestMethod]
        public void Pokemon_SetHP_To_Zero()
        {
            Pokemon pokemon = new Pokemon();
            pokemon.HP = 0;
            Assert.AreEqual(0, pokemon.HP);
        }

        [TestMethod]//[ExpectedException(typeof(Exception))]
        public void Pokemon_SetHP_GreaterThan_MaxHP_Equals_MaxHP()
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
        public void Pokemon_SetStatus_To_Burn()
        {
            Pokemon pokemon = new Pokemon();
            pokemon.Status = Status.BURN;
            Assert.AreEqual(Status.BURN, pokemon.Status);
            //Assert.Inconclusive();
        }

        //[TestMethod]
        //public void Pokemon_Sleep_StatusTurn_Not_Zero() {
        //    Pokemon pokemon = new Pokemon();
        //    pokemon.Status = Status.SLEEP;
        //	//If remaining turns for sleep is 0 then the pokemon would be awake.
        //	//I think this test only works properly if done thru battle class
        //    Assert.AreNotEqual(0, pokemon.StatusCount);
        //}

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
        [TestMethod]
        public void Pokemon_Starting_Level_NotNegative()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            Assert.IsTrue(pokemon.Level >= 0);
        }

        [TestMethod]
        public void Pokemon_IsEgg_At_Default_Level()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            Assert.IsTrue(pokemon.isEgg);
        }

        [TestMethod]
        public void Pokemon_HatchEgg()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            pokemon.HatchEgg();
            Assert.IsFalse(pokemon.isEgg);
        }

        [TestMethod]
        public void Pokemon_Spawn_Wild_Not_Egg()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR, isEgg: false);
            Assert.IsFalse(pokemon.isEgg);
        }

        //[TestMethod]
        // Setting the pokemon levels controls the experience points
        //public void Pokemon_Set_ExperiencePoints_To_Match_Level()
        //{
        //	byte lv = 7;
        //	Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
        //	pokemon.Level = lv;
        //	Assert.AreEqual(lv,Pokemon.Experience.GetLevelFromExperience(pokemon.GrowthRate, pokemon.Exp.Current));
        //}

        [TestMethod]
        // Setting the pokemon experience points controls the level
        public void Pokemon_Set_Level_To_Match_ExperiencePoints()
        {
            byte lv = 7;
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR, level: 3);
            pokemon.Exp.AddExperience(Pokemon.Experience.GetStartExperience(pokemon.GrowthRate, 300));
            pokemon.Exp.AddExperience(Pokemon.Experience.GetStartExperience(pokemon.GrowthRate, lv) - pokemon.Exp.Current);
            Assert.AreEqual(lv, Pokemon.Experience.GetLevelFromExperience(pokemon.GrowthRate, pokemon.Exp.Current));
        }

        [TestMethod]
        public void Pokemon_Spawn_At_Set_Level()
        {
            byte lv = 5;
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR, level: lv);
            Assert.AreEqual(lv, Pokemon.Experience.GetLevelFromExperience(pokemon.GrowthRate, pokemon.Exp.Current));
        }

        //[TestMethod]
        //public void Pokemon_TestPokemon_Set_To_Egg() //Fail set to egg after hatch?
        //{
        //	Pokemon pokemon = new Pokemon(Pokemons.NONE);
        //	//If not egg
        //	if (!pokemon.isEgg) Assert.Fail();
        //	//else fail
        //	//Set to egg
        //
        //	//Assert if egg
        //	Assert.Inconclusive();
        //}

        //[TestMethod]
        //public void Pokemon_TestPokemon_Hatch_Egg()
        //{
        //	//Set to egg
        //	Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR)
        //	{
        //		//eggSteps = 5;
        //	};
        //	//If not egg fail
        //	if (!pokemon.isEgg) Assert.Fail();
        //	//for loop until egg hatches
        //	//Assert if egg
        //	Assert.Inconclusive();
        //	//When egg hatches, values need to be set:
        //	//pkmn.eggsteps = 0
        //	//pkmn.hatchedMap = 0
        //	//pkmn.obtainMode = 0
        //}

        [TestMethod]
        public void Pokemon_Egg_Hatches_When_Timer_Reaches_Zero()
        {
            Pokemon pokemon = new Pokemon(Pokemons.CHARMANDER);
            if (!pokemon.isEgg) Assert.Fail("new Pokemon isnt an Egg");
            int i = 0;
            while (pokemon.EggSteps != 0)//(pokemon.isEgg)
            {
                pokemon.AddSteps(); //goes down by 1
                if (i > 1001) Assert.Fail("Infinite Loop; Results Undetermined");
            }
            Assert.AreEqual(false, pokemon.isEgg);
        }

        [TestMethod]
        public void Pokemon_ChanceFor_HiddenAbility_If_Egg()
        {
            Pokemons pkmn = Pokemons.BULBASAUR;
            Abilities Hidden = Pokemon.PokemonData.GetPokemon(pkmn).Ability[2];
            int i = 0;
            while (true)
            {
                Pokemon pokemon = new Pokemon(pkmn);
                //pokemon.HatchEgg();
                bool HA = pokemon.Ability == Hidden;
                if (HA) Assert.IsTrue(HA); i++;
                if (i > 15) Assert.Fail("Infinite Loop; Results Undetermined");
            }
        }

        //Test max value for pokemon stats
        [TestMethod]
        public void Pokemon_EV_GreaterThan_MaxEV_Equals_MaxEV()
        {
            Pokemon pokemon = new Pokemon();
            Assert.AreEqual(Pokemon.EVSTATLIMIT, pokemon.EV[0]);
        }

        [TestMethod]
        public void Pokemon_CombinedEV_Fail_GreaterThan_EV_MaxLimit()
        {
            //All EV points when added together cannot be greater than a sum of MaxEVLimit
            Pokemon pokemon = new Pokemon();
            int ev = pokemon.EV[0] + pokemon.EV[1] + pokemon.EV[2] + pokemon.EV[3] + pokemon.EV[4] + pokemon.EV[5];
            Assert.AreEqual(Pokemon.EVLIMIT, ev);
        }
        #endregion

        #region Moves...
        [TestMethod]
        public void Pokemon_DefaultMoves_NotNull()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            Assert.IsTrue(pokemon.countMoves() > 0);
        }
        //[TestMethod]
        //public void Pokemon_RNG_DefaultMoves_For_Egg()
        //{
        //	Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
        //	if (!pokemon.isEgg) Assert.Fail("new Pokemon isnt an Egg");
        //	Moves[] before = new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId };
        //	pokemon.GenerateMoveset();
        //	Assert.AreNotSame(before,new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
        //}
        [TestMethod]
        public void Pokemon_RNG_Moves_IsDifferent_For_HatchingEgg() //_At_Levels_GreaterThan_Zero
        {
            Pokemons pkmn = Pokemons.SQUIRTLE;
            System.Collections.Generic.List<Moves> egg = new System.Collections.Generic.List<Moves>(); //ml.AddRange(pokemon.getMoveList(LearnMethod.egg));
            System.Collections.Generic.List<Moves> lv = new System.Collections.Generic.List<Moves>(Pokemon.PokemonData.GetPokemon(pkmn).GetMoveList(LearnMethod.levelup));
            foreach (Moves item in Pokemon.PokemonData.GetPokemon(pkmn).GetMoveList(LearnMethod.egg))
            {
                if (!lv.Contains(item)) egg.Add(item);
            }
            if (egg.Count < 1) Assert.Fail("Pokemon does not any contain egg-only move");
            for (int i = 0; i < 10; i++)
            {
                Pokemon pokemon = new Pokemon(pkmn);
                if (!pokemon.isEgg) Assert.Fail("new Pokemon isnt an Egg");
                //Moves[] before = new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId };
                //pokemon.GenerateMoveset();
                //Hatch Egg Here...
                pokemon.HatchEgg();
                //if(pokemon.Level <= 0) Assert.Fail("Pokemon is still level zero");
                if (pokemon.isEgg) Assert.Fail("Pokemon is still an egg.");
                foreach (Move move in pokemon.moves)
                {
                    if (move.MoveId != Moves.NONE &&
                        (
                            egg.Contains(move.MoveId)
                        )
                    )
                        Assert.IsTrue(true, "Pokemon contains exclusive egg only move");
                }
                //Moves[] before = new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId };
                //pokemon.GenerateMoveset();
            }
            //Assert.AreNotSame(before,new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
            //Assert.IsTrue if Pokemon.Move Contains exclusive egg-only moves.
            Assert.Fail("Pokemon does not contain egg-only move");
        }
        //[TestMethod]
        //public void Pokemon_Reseting_Moves_IsNotEqual()
        //{
        //	Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
        //	Moves[] before = new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId };
        //	pokemon.resetMoves();
        //	Assert.AreNotSame(before,new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
        //}
        /// <summary>
        /// 
        /// </summary>
        /// ToDo: Resetting pokemon moves should randomly shuffle between all available that pokemon can possibly learn for their level
        [TestMethod]
        public void Pokemon_RNG_Moves_NotEqual_PreviousMoves()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            Moves[] before = new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId };
            pokemon.GenerateMoveset();
            Assert.AreNotSame(before, new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
        }
        [TestMethod]
        public void Pokemon_Moves_Should_Not_Contain_Duplicates()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            for (int i = 0; i < 30; i++)
            {
                if (pokemon.moves[0].MoveId != Moves.NONE &&
                    (
                        (pokemon.moves[0].MoveId == pokemon.moves[1].MoveId) ||
                        (pokemon.moves[0].MoveId == pokemon.moves[2].MoveId) ||
                        (pokemon.moves[0].MoveId == pokemon.moves[3].MoveId)
                    )
                )
                    Assert.Fail();
                if (pokemon.moves[1].MoveId != Moves.NONE &&
                    (
                        (pokemon.moves[1].MoveId == pokemon.moves[2].MoveId) ||
                        (pokemon.moves[1].MoveId == pokemon.moves[3].MoveId)
                    )
                )
                    Assert.Fail();
                if (pokemon.moves[2].MoveId != Moves.NONE &&
                    (
                        (pokemon.moves[2].MoveId == pokemon.moves[3].MoveId)
                    )
                )
                    Assert.Fail();
                //Moves[] before = new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId };
                pokemon.GenerateMoveset();
            }
            //Assert.Inconclusive();
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void Pokemon_TeachMove_Add_NewMove()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            int i = 0;
            while (pokemon.countMoves() == 4)
            {
                pokemon.GenerateMoveset(); i++;
                if (i > 5) Assert.Fail("Infinite Loop; Results Undetermined");
            }
            Moves[] before = new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId };
            pokemon.LearnMove(Moves.RAZOR_LEAF, true);
            Assert.AreNotSame(before, new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
        }
        [TestMethod]
        /// <summary>
        /// Move list should be compatible with pokemon
        /// </summary>
        public void Pokemon_TeachMove_Fail_NotCompatible()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            int i = 0;
            while (pokemon.countMoves() == 4)
            {
                pokemon.GenerateMoveset(); i++;
                if (i > 5) Assert.Fail("Infinite Loop; Results Undetermined");
            }
            Moves[] before = new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId };
            pokemon.LearnMove(Moves.OVERHEAT);
            Assert.AreSame(before, new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
        }
        //[TestMethod]
        //public void Pokemon_PokemonTest_CantLearn_Move_NotCompatible_With_Pokemon()
        //{
        //	//list of moves can learn at level
        //	Assert.Inconclusive();
        //}
        [TestMethod]
        /// <summary>
        /// Move list must not be full to add move to pokemon
        /// </summary>
        public void Pokemon_Full_Moveset_Fail_TeachMove()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            int i = 0;
            while (pokemon.countMoves() != 4)
            {
                pokemon.GenerateMoveset(); i++;
                if (i > 5) Assert.Fail("Infinite Loop; Results Undetermined");
            }
            Moves[] before = new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId };
            pokemon.LearnMove(Moves.OVERHEAT);
            Assert.AreSame(before, new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
        }
        [TestMethod]
        /// <summary>
        /// Remove random move, packs the move list, and confirm move is missing
        /// </summary>
        public void Pokemon_ForgetMove_Minus_Move()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            int i = 0;
            while (pokemon.countMoves() == 4)
            {
                pokemon.GenerateMoveset(); i++;
                if (i > 5) Assert.Fail("Infinite Loop; Results Undetermined");
            }
            pokemon.LearnMove(Moves.OVERHEAT);
            int before = pokemon.countMoves();
            pokemon.DeleteMove(Moves.OVERHEAT);
            Assert.IsTrue(pokemon.countMoves() == before - 1);
        }
        //[TestMethod]
        //public void Pokemon_Replace_Move_Return_Different_Moves()
        //{
        //	Assert.Inconclusive();
        //}
        //[TestMethod]
        //public void Pokemon_Swap_Moves_Change_OrderOf_Moves()
        //{
        //	//Loop thru all moves, make sure they're all present
        //	Assert.Inconclusive();
        //}
        [TestMethod]
        public void Pokemon_Return_MoveList_CanLearn_At_CurrentLevel()
        {
            //Pokemon pokemon = new Pokemon();
            //Pokemon.PokemonData.GetPokemon(Pokemons.NONE).MoveTree.LevelUp.Where(x => x.Value <= this.Level).Select(x => x.Key)
            //list of moves can learn at level
            //Assert.AreSame(new Moves[] { }, new Pokemon().getMoveList());
            Assert.IsTrue(new Pokemon(Pokemons.BULBASAUR, level: 25).getMoveList(LearnMethod.levelup).Length > 0);
        }
        [TestMethod]
        public void Pokemon_PokemonTest_CantLearn_Move_NotCompatible_With_TeachMethod()
        {
            //list of moves a pokemon can learn for a given technique
            //attempt to teach move
            //confirm moves are unchanged 
            Assert.Inconclusive();
        }
        #endregion

        #region Evolving/evolution
        //Use eevee to test different evolve code, as it's a perfect practice test
        [TestMethod]
        public void Pokemon_TestPokemon_CanEvolve()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            //Add exp
            pokemon.Exp.AddExperience(50000);
            //Assert is true
            if (pokemon.hasEvolveMethod(EvolutionMethod.Level))
                Assert.IsTrue(false);
            else Assert.Fail("Unable to test if pokemon can evolve, as it does not have an evolution through leveling-up");
        }
        [TestMethod]
        public void Pokemon_TestPokemon_EvolvePokemon()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            Assert.AreEqual(Pokemons.IVYSAUR, pokemon.Species);
        }
        #endregion

        #region Misc
        [TestMethod]
        public void Pokemon_WildPokemon_With_Item()
        {
            //Maybe this one isnt needed?... 
            Assert.Fail();
        }
        [TestMethod]
        public void Pokemon_TestPokemon_SetForm_To_Form2()
        {
            Pokemon pokemon = new Pokemon(Pokemons.NONE);
            pokemon.Form = 2;
            Assert.AreEqual("test1", pokemon.Name);
        }
        [TestMethod]
        //Changing form changes base stats
        public void Pokemon_TestPokemon_SetForm_IncreasesAtkStats()
        {
            Pokemon pokemon = new Pokemon(Pokemons.NONE);
            pokemon.Form = 1;
            //Assert.AreNotEqual(Pokemon.PokemonData.GetPokemon(pokemon.Species).BaseStatsATK, pokemon.ATK);
            Assert.Fail("Need to find way to compare Pokemon.baseStats to Form.baseStats");
        }
        //[TestMethod]
        //public void Pokemon_TestPokemon_GetPokemon_From_Form()
        //{
        //	//Maybe this one isnt needed?... 
        //	Assert.Inconclusive();
        //}
        [TestMethod]
        public void Pokemon_TestPokemon_Set_Ribbons_Tier3_OutOf_4()
        {
            Pokemon pokemon = new Pokemon(Pokemons.NONE);
            //Set ribbons to tier 3
            //Check if contains other tiers rank 1 and 2
            Assert.IsTrue(pokemon.Ribbons.Contains(Ribbon.HOENNTOUGHHYPER));
            //Create 2nd assert for regular pokemon
            Assert.Fail("Pokemon NONE cannot obtain ribbons");
        }
        [TestMethod]
        public void Pokemon_TestPokemon_Add_Ribbons_OfSameType_Increases_RankTier()
        {
            Pokemon pokemon = new Pokemon(Pokemons.NONE);
            //Add Ribbon.HOENNTOUGH x4
            Assert.IsTrue(pokemon.Ribbons.Contains(Ribbon.HOENNTOUGHHYPER));
            //Create 2nd assert for regular pokemon
            Assert.Fail("Pokemon NONE cannot obtain ribbons");
        }
        [TestMethod]
        public void Pokemon_TestPokemon_Set_Markings()
        {
            Pokemon pokemon = new Pokemon(Pokemons.NONE)
            {
                Markings = new bool[] { false, true, true, false, false, false }
            };
            //bool[] marks = new bool[6]; marks[1] = marks[2] = true;
            Assert.IsTrue(pokemon.Markings[2]);
        }
        [TestMethod]
        public void Pokemon_TestPokemon_Set_To_Shadow()
        {
            Pokemon pokemon = new Pokemon(Pokemons.NONE);
            Assert.IsTrue(pokemon.isShadow);
        }
        [TestMethod]
        public void Pokemon_TestPokemon_Shadow_Fail_To_Purify_If_HeartGuage_Not_Zero()
        {
            Pokemon pokemon = new Pokemon(Pokemons.NONE);
            if (!pokemon.isShadow) Assert.Fail("Is not Shadow, cannot purify");
            Assert.IsTrue(pokemon.isShadow);
        }
        #endregion

        #region Gender..
        [TestMethod]
        public void Pokemon_Mail_Test_Pokemon_HoldMessage()
        {
            Pokemon pokemon = new Pokemon(Pokemons.NONE);
            Assert.IsNotNull(pokemon.Mail);
        }

        //[TestMethod]
        //public void Pokemon_GenderRatio_To_Gender()
        //{
        //	//Convert GenderRatio to Male/Female Results
        //	//for loop, count to 100, if results is equal to or greater than threshold, fail
        //	Pokemons pkmn = Pokemons.BULBASAUR;
        //	GenderRatio genders = Pokemon.PokemonData.GetPokemon(pkmn).MaleRatio;
        //	int females = 0;
        //	//Confirm test criteria by making sure data fits
        //	if (genders == GenderRatio.AlwaysFemale)
        //		for (int i = 0; i < 100; i++)
        //		{
        //			Pokemon pokemon = new Pokemon(pkmn);
        //			//Assert.IsTrue(pokemon.Ability == Pokemon.PokemonData.GetPokemon(pokemon.Species).Ability[2]); i++;
        //			//if (i > 5) Assert.Fail("Infinite Loop; Results Undetermined");
        //			if (pokemon.Gender.HasValue && !pokemon.Gender.Value) females++;
        //		}
        //	else
        //		Assert.Fail("Testing for gender ratio of... but pokemon gender chances are {0}", genders.ToString());
        //	Assert.IsTrue(females > 30);
        //}

        [TestMethod]
        public void Pokemon_GenderRatio_NotSingleGendered()
        {
            //Convert GenderRatio to Male/Female Results
            //for loop, count to 100, if results is equal to or greater than threshold, fail
            Pokemons pkmn = Pokemons.BULBASAUR;
            GenderRatio genders = Pokemon.PokemonData.GetPokemon(pkmn).MaleRatio;
            int females = 0;
            //Confirm test criteria by making sure data fits
            if (genders == GenderRatio.FemaleOneEighth || !Pokemon.PokemonData.GetPokemon(pkmn).IsSingleGendered)
            {
                for (int i = 0; i < 100; i++)
                {
                    Pokemon pokemon = new Pokemon(pkmn);
                    if (pokemon.Gender.HasValue && !pokemon.Gender.Value) females++;
                }
                Assert.IsTrue(100 - females < 100 - 18); //12.5 is 1/8th 
            }
            else
                Assert.Fail("Testing for gender ratio of {0}; but pokemon gender chances are {1}", "One-Eighth Percent Females", genders.ToString());
        }
        #endregion
    }
}
