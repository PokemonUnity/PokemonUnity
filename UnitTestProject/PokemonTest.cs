using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;

namespace Tests
{
    [TestClass]
    public class PokemonTest
    {
        [TestMethod]
        public void Initialize_Pokemon_Test()
        {		
			//Only good if this function is ran first before any others
			Assert.IsTrue(Game.InitPokemons());
        }
        [TestMethod]
        public void Pokemon_Data_IsLoaded()
        {
			Assert.IsTrue(Game.PokemonData.Count > 1);
        }
        [TestMethod]
        public void Initialize_PokemonMoves_Test()
        {		
			//Only good if this function is ran first before any others
			Assert.IsTrue(Game.InitPokemonMoves());
        }
        [TestMethod]
        public void PokemonMoves_Data_IsLoaded()
        {
			Assert.IsTrue(Game.PokemonMovesData.Count > 1);
        }

        //Create 2 assert test; 1 for regular pokemon, and one for pokemon.NONE
        //Pokemon.NONE cannot receive any changes to data, as it does not exist...

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
			Pokemon pokemon = new Pokemon(Pokemons.NONE); //Step counter for test pokemon is 1000
			if (!pokemon.isEgg) Assert.Fail("new Pokemon isnt an Egg");
			int i = 0;
			while (pokemon.EggSteps != 0)//(pokemon.isEgg)
			{
				pokemon.AddSteps(); //goes down by 1
				if (i > 1001) Assert.Fail("Infinite Loop; Results Undetermined");
			}
			Assert.AreEqual(false,pokemon.isEgg);
		}
		
        [TestMethod]
		public void Pokemon_ChanceFor_HiddenAbility_If_Egg()
		{
			Pokemons pkmn = Pokemons.BULBASAUR;
			Abilities Hidden = Game.PokemonData[pkmn].Ability[2];
            if (Hidden == Abilities.NONE)
            {
                Assert.Fail("This pokemon does not have a hidden ability");
            }
			int i = 0;
            Pokemon pokemon;
			while (true)
			{
				pokemon = new Pokemon(pkmn);
				//pokemon.HatchEgg();
				bool HA = pokemon.Ability == Hidden;
				if(HA) break; i++;
				if (i > 30) Assert.Fail("Infinite Loop; Results Undetermined");
			}
            Assert.AreEqual(Hidden, pokemon.Ability);
		}

        //Test max value for pokemon stats
        [TestMethod]
        public void Pokemon_EV_GreaterThan_MaxEV_Equals_MaxEV()
        {
			Assert.Inconclusive("Not implemented yet");
			Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
			System.Collections.Generic.List<Pokemons> pkmns = 
				//new System.Collections.Generic.List<Pokemons>((Pokemons[])Enum.GetValues(typeof(Pokemons)));
				new System.Collections.Generic.List<Pokemons>(Game.PokemonData.Keys);
			int x = 3;
			//Add values enough to exceed limit, check to see if capped
			for (int i = 0; i < 700; i++)
			{
				//x = Core.Rand.Next(pkmns.Count);
				//while (x == 0)// || !pkmns.Contains((Pokemons)x)
				//{
				//	//x = Core.Rand.Next();
				//	//if(!pkmns.Contains((Pokemons)x))
				//	//	x = Core.Rand.Next();
				//	x = Core.Rand.Next(pkmns.Count);
				//}
				//pokemon.GainEffort((Pokemons)x);
				pokemon.GainEffort((Pokemons)pkmns[x]);
			}
            Assert.AreEqual(Pokemon.EVSTATLIMIT, pokemon.EV[(int)Stats.SPATK]);
		}

        [TestMethod]
        public void Pokemon_CombinedEV_Fail_GreaterThan_EV_MaxLimit()
        {
            //All EV points when added together cannot be greater than a sum of MaxEVLimit
			Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
			System.Collections.Generic.List<Pokemons> pkmns = 
				//new System.Collections.Generic.List<Pokemons>((Pokemons[])Enum.GetValues(typeof(Pokemons)));
				new System.Collections.Generic.List<Pokemons>(Game.PokemonData.Keys);
			int x = 0;
			//Add EV till max is hit, and add together and compare total
			for (int i = 0; i < 700; i++)
			{
				x = Core.Rand.Next(pkmns.Count);
				while (x == 0)// || !pkmns.Contains((Pokemons)x)
				{
					//x = Core.Rand.Next();
					//if(!pkmns.Contains((Pokemons)x))
					//	x = Core.Rand.Next();
					x = Core.Rand.Next(pkmns.Count);
				}
				//pokemon.GainEffort((Pokemons)x);
				pokemon.GainEffort((Pokemons)pkmns[x]);
			}
            int ev = pokemon.EV[0] + pokemon.EV[1] + pokemon.EV[2] + pokemon.EV[3] + pokemon.EV[4] + pokemon.EV[5];
            Assert.AreEqual(Pokemon.EVLIMIT, ev);
			//Assert.Inconclusive("Not implemented yet");
		}
        #endregion

        #region Moves...
        [TestMethod]
        public void Pokemon_DefaultMoves_NotNull()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            Assert.IsTrue(pokemon.countMoves() > 0);
        }
		[TestMethod]
		public void Pokemon_RNG_DefaultMoves_For_Egg()
		{
			Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
			if (!pokemon.isEgg) Assert.Fail("new Pokemon isnt an Egg");
			Moves[] before = new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId };
			pokemon.GenerateMoveset();
			Assert.AreNotSame(before,new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
		}
		[TestMethod]
		public void Pokemon_RNG_Moves_IsDifferent_For_HatchingEgg() 
		{
		    Pokemons pkmn = Pokemons.SQUIRTLE;
		    System.Collections.Generic.List<Moves> egg = new System.Collections.Generic.List<Moves>(); //ml.AddRange(pokemon.getMoveList(LearnMethod.egg));
		    System.Collections.Generic.List<Moves> lv = new System.Collections.Generic.List<Moves>(Game.PokemonMovesData[pkmn].LevelUp.Keys);
		    foreach (Moves item in Game.PokemonMovesData[pkmn].Egg)
		    {
		        if (!lv.Contains(item)) egg.Add(item);
		    }
		    if (egg.Count < 1) Assert.Fail("Pokemon does not any contain egg-only move");
		    for (int i = 0; i < 10; i++)
		    {
		        Pokemon pokemon = new Pokemon(pkmn);
		        if (!pokemon.isEgg) Assert.Fail("new Pokemon isnt an Egg");
		        //pokemon.GenerateMoveset();
		        //Hatch Egg Here...
		        pokemon.HatchEgg();
		        if (pokemon.isEgg) Assert.Fail("Pokemon is still an egg.");
		        foreach (Move move in pokemon.moves)
		        {
					//Pass test if pokemon has moves learned from Egg-state.
		            if (move.MoveId != Moves.NONE 
						&&  egg.Contains(move.MoveId)
					)
					{
						//Assert.IsTrue if Pokemon.Move Contains exclusive egg-only moves.
						//CollectionAssert.Contains(egg, move.MoveId, "Pokemon contains exclusive egg only move");
		                Assert.IsTrue(true);//"Pokemon contains exclusive egg only move"
		                Assert.IsTrue(egg.Contains(move.MoveId));//"Pokemon contains exclusive egg only move"
		                Assert.AreEqual(true, egg.Contains(move.MoveId));//"Pokemon contains exclusive egg only move"
						Assert.Inconclusive("Test is malfunctioning and refuses to mark as success");
					}
		        }
		    }
		    Assert.Fail("Pokemon does not contain egg-only move");
		}
		[TestMethod]
		public void Pokemon_Reseting_Moves_IsNotEqual()
		{
			Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
			Moves[] before = new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId };
			pokemon.resetMoves();
			Assert.AreNotSame(before,new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
		}
		/// <summary>
		/// </summary>
		/// ToDo: Resetting pokemon moves should randomly shuffle between all available that pokemon can possibly learn for their level
		[TestMethod]
        public void Pokemon_RNG_Moves_NotEqual_PreviousMoves()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            Moves[] before = new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId };
            pokemon.GenerateMoveset();
			CollectionAssert.AreNotEqual(before, new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
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
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void Pokemon_TeachMove_Add_NewMove()
        {
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            int i = 0;
            while (pokemon.countMoves() == 4)
            {
                pokemon.GenerateMoveset();
				i++;
                if (i > 5) Assert.Fail("Infinite Loop; Results Undetermined");
            }
            Moves[] before = new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId };
			bool success;
			pokemon.LearnMove(Moves.RAZOR_LEAF, out success, true); //if success is false, fail test
			if (!success) Assert.Fail("Bool returns false, which means learning skill was unsuccessful");
			CollectionAssert.AreNotEqual(before, new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
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
                pokemon.GenerateMoveset();
				i++;
                if (i > 25) Assert.Fail("Infinite Loop; Results Undetermined");
            }
            Moves[] before = new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId };
			bool success;
			pokemon.LearnMove(Moves.OVERHEAT, out success); //if success is true, fail test
            CollectionAssert.AreEqual(before, new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
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
                pokemon.GenerateMoveset();
				i++;
                if (i > 1000) Assert.Fail("Infinite Loop; Results Undetermined");
            }
            Moves[] before = new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId };
            bool success;
			pokemon.LearnMove(Moves.TACKLE, out success); //if success is true, fail test
			if (success) Assert.Fail("Bool returns true, which means learning skill was successful");
			CollectionAssert.AreEqual(before, new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
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
                pokemon.GenerateMoveset();
				i++;
                if (i > 15) Assert.Fail("Infinite Loop; Results Undetermined");
            }
			bool success;
            pokemon.LearnMove(Moves.TACKLE, out success);
            int before = pokemon.countMoves();
            pokemon.DeleteMove(Moves.TACKLE);
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
		//[TestMethod]
		//public void Pokemon_Return_MoveList_CanLearn_At_CurrentLevel()
		//{
		//    //Pokemon pokemon = new Pokemon();
		//    //Pokemon.PokemonData.GetPokemon(Pokemons.NONE).MoveTree.LevelUp.Where(x => x.Value <= this.Level).Select(x => x.Key)
		//    //list of moves can learn at level
		//    //Assert.AreSame(new Moves[] { }, new Pokemon().getMoveList());
		//    Assert.IsTrue(new Pokemon(Pokemons.BULBASAUR, level: 25).getMoveList(LearnMethod.levelup).Length > 0);
		//}
		//[TestMethod]
		//public void Pokemon_PokemonTest_CantLearn_Move_NotCompatible_With_TeachMethod()
		//{
		//    //list of moves a pokemon can learn for a given technique
		//    //attempt to teach move
		//    //confirm moves are unchanged 
		//    Assert.Inconclusive();
		//}
		#endregion

		#region Evolving/evolution
		/*The various triggers for a Pokémon's evolution are almost as varied as the Pokémon themselves, and some Pokémon have a unique evolution method. 
		 * The most common of them is Evolution by leveling up at or above a certain level. Other methods include the following:

			* leveling up
				* leveling up when friendship has reached a high level (220 or greater), sometimes only at certain times
				* leveling up while holding an item, sometimes only at certain times
				* leveling up while knowing a certain move or a move of a certain type
				* leveling up in a certain location
				* leveling up with a certain Pokémon or Pokémon of a certain type in the party
				* leveling up while upside-down
				* leveling up during certain types of weather
			* being traded
				* being traded while holding a specific item
				* being traded for a specific Pokémon
			* using an evolutionary stone
		Some evolutions are dependent on the Pokémon's gender. For example, only female Combee can evolve into Vespiquen—male Combee cannot evolve at all. 
		Similarly, all Snorunt can evolve into Glalie, but only female Snorunt can evolve into Froslass. On the other hand, 
		male Burmy can only evolve into Mothim, while female Burmy can only evolve into Wormadam.*/
		//Use eevee to test different evolve code, as it's a perfect practice test
		//[TestMethod]
		//public void Pokemon_TestPokemon_CanEvolve()
		//{
		//    Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
		//    //Add exp
		//    pokemon.Exp.AddExperience(50000);
		//    //Assert is true
		//    if (pokemon.hasEvolveMethod(EvolutionMethod.Level))
		//        Assert.IsTrue(false);
		//    else Assert.Fail("Unable to test if pokemon can evolve, as it does not have an evolution through leveling-up");
		//}
		[TestMethod]
        public void Pokemon_TestPokemon_EvolvePokemon()
        {
            Assert.Inconclusive();
            Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
            Assert.AreEqual(Pokemons.IVYSAUR, pokemon.Species);
        }
		#endregion

		#region Shadow/Heart Guage
		/* The Trainer of a Shadow Pokémon cannot do any of the following with it until it is purified:

			* Level it up (when it is purified, it gains all the exp. accumulated as a Shadow Pokémon, which is only when its Heart Gauge is less than three bars full).
			* Evolve it (evolves when gaining exp.)
			* Use a Rare Candy on it.
			* Change the order of its moves.
			* Delete its moves.
			* Give it a nickname (due to not having an OT or ID).
			* Trade it.
			* Participate in Battle Mode with it.
			* Enter it in battles at Phenac Stadium.
			* Enter it in battles at Orre Colosseum.
		Shadow Pokémon are also unable to gain effort values from battling, although vitamins can still be used on them.*/
        [TestMethod]
        public void Pokemon_TestPokemon_Set_To_Shadow()
        {
            Pokemon pokemon = new Pokemon(Pokemons.NONE);
			//Shadow can be done by counting hatch steps in negative values?
            //Assert.IsTrue(pokemon.isShadow);
			Assert.Inconclusive("Not implemented yet");
		}
        [TestMethod]
        public void Pokemon_TestPokemon_Shadow_Fail_To_Purify_If_HeartGuage_Not_Zero()
        {
			Assert.Inconclusive("Not implemented yet");
            Pokemon pokemon = new Pokemon(Pokemons.NONE);
            if (!pokemon.isShadow) Assert.Fail("Is not Shadow, cannot purify");
			//Check guage
			//Attempt to purify
			//Fail if goal isnt met
            //Assert.IsTrue(pokemon.isShadow);
        }
		#endregion

		#region Shiny
		/*The table below summarizes the rates at which Shiny Pokémon can be found by the methods that will be detailed below. 
		 * The Shiny Charm can directly add to the odds for most methods, with hidden Pokémon being affected uniquely.
							|Gen.II	|Gen.III	|Gen.IV	|Gen.V	|Gen.VI				|Gen.VII
							------------------------------------------------------------------
Base rate					|1/8192						|1/4096	
							------------------------------------------------------------------
Breeding a Shiny Pokémon	|		|			|		|		|					|
(if the offspring is the 	|1/64	|—			|—		|—		|—					|—
opposite gender)			|		|			|		|		|					|
							------------------------------------------------------------------
Masuda method				|—		|—			|5/8192	|6/8192	|6/4096				
							------------------------------------------------------------------
Poké Radar chaining 		|		|			|		|		|					|
(single patch): ≥40			|—		|—			|41/8192|—		|?					|—
							------------------------------------------------------------------
Shiny Charm					|—		|—			|—		|+2/8192|+2/4096			
							------------------------------------------------------------------
Friend Safari				|—		|—			|—		|—		|5/4096				|—
							------------------------------------------------------------------
Consecutive fishing: ≥20	|—		|—			|—		|—		|41/4096			|—
							------------------------------------------------------------------
Hidden Pokémon: 			|		|			|		|		|					|
Search Level 200 + X		|—		|—			|—		|—		|0.08% + X*0.01%	|—
							------------------------------------------------------------------
SOS Battles: ≥31			|—		|—			|—		|—		|—					|13/4096
							------------------------------------------------------------------*/
		#endregion

        #region Gender..
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
        public void Pokemon_GenderRatio_OneEighthFemale()
        {
            //Convert GenderRatio to Male/Female Results
            //for loop, count to 100, if results is equal to or greater than threshold, fail
            Pokemons pkmn = Pokemons.BULBASAUR;
            GenderRatio genders = Game.PokemonData[pkmn].GenderEnum;
            int females = 0;
            //Confirm test criteria by making sure data fits
            if (genders == GenderRatio.FemaleOneEighth || !Game.PokemonData[pkmn].IsSingleGendered)
            {
                for (int i = 0; i < 100; i++)
                {
                    Pokemon pokemon = new Pokemon(pkmn);
                    if (pokemon.Gender.HasValue && !pokemon.Gender.Value) females++;
                }
                Assert.IsTrue(100 - females > 100 - 18); //12.5 is 1/8th 
            }
            else
                Assert.Fail("Testing for gender ratio of {0}; but pokemon gender chances are {1}", "One-Eighth Percent Females", genders.ToString());
        }
		#endregion

		#region Forms
		/*
		 * Unowns and their different letters, should reference same pokemon id (same with pikachu)
		 * Learn moves based on form... (Deoxys)
		 * From change based on items...
		 * Form change based on gender
		 * Evolutions.. (some evolve into form based on specific criteria)
		 * Some forms are fusions...
		 * Some forms are purely cosmetic and change based on frontend/ui (connect overworld/weather mechanic to backend)
		 * Some forms are battle only forms, and battle mechanic is going to be frontend only (will need to redo/reference code)
		 * Pokemon Vivillion form is based on player's physical GPS location (pc IP Address)
		 * ToDo: Connect different forms to return correct UI (texture/model matches pokemon id) 
		 */
        [TestMethod]
        public void Pokemon_TestPokemon_SetForm_To_Form2()
        {
            Pokemon pokemon = new Pokemon(Pokemons.NONE);
            pokemon.Form = 2;
            //Assert.AreEqual("test1", pokemon.Name);
			Assert.Inconclusive("Not implemented yet");
		}
        [TestMethod]
        //Changing form changes base stats
        public void Pokemon_TestPokemon_SetForm_IncreasesAtkStats()
        {
            Pokemon pokemon = new Pokemon(Pokemons.NONE);
            pokemon.Form = 1;
            //Assert.AreNotEqual(Pokemon.PokemonData.GetPokemon(pokemon.Species).BaseStatsATK, pokemon.ATK);
            //Assert.Fail("Need to find way to compare Pokemon.baseStats to Form.baseStats");
			Assert.Inconclusive("Not implemented yet");
		}
		#endregion

		#region Misc
		[TestMethod]
        public void Pokemon_WildPokemon_With_Item()
        {
			//Instantiate wild pokemon, maybe 100 times
			//check to see if wild pokemon is created with held item
			//pass if held item is true
            //Maybe this one isnt needed?...
			//Wild pokemons are (any/instantiated) pkmns without trainers?
            Assert.Inconclusive();
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
			Assert.Inconclusive("Should research more on ribbon mechanic");
            Pokemon pokemon = new Pokemon(Pokemons.NONE);
            //Set ribbons to tier 3
            //Check if contains other tiers rank 1 and 2
            Assert.IsTrue(pokemon.Ribbons.Contains(Ribbon.HOENNTOUGHHYPER));
            //Create 2nd assert for regular pokemon
            //Assert.Fail("Pokemon NONE cannot obtain ribbons");
			Assert.Inconclusive("Not implemented yet");
		}
        [TestMethod]
        public void Pokemon_TestPokemon_Add_Ribbons_OfSameType_Increases_RankTier()
        {
			Assert.Inconclusive("Should research more on ribbon mechanic");
            Pokemon pokemon = new Pokemon(Pokemons.NONE);
            //Add Ribbon.HOENNTOUGH x4
            Assert.IsTrue(pokemon.Ribbons.Contains(Ribbon.HOENNTOUGHHYPER));
            //Create 2nd assert for regular pokemon
            //Assert.Fail("Pokemon NONE cannot obtain ribbons");
			Assert.Inconclusive("Not implemented yet");
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
        public void Pokemon_Mail_Test_Pokemon_HoldMessage()
        {
            Pokemon pokemon = new Pokemon(Pokemons.NONE);
			//Write message, Pick Mail design
			//Save to pokemon
			//Check pokemon and confirm it's still there
            //Assert.IsNotNull(pokemon.Mail);
			Assert.Inconclusive("Not implemented yet");
		}
        #endregion
    }
}