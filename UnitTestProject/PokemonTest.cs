using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Monster.Data;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;

namespace Tests
{
	[TestClass]
	public class PokemonTest
	{
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

		[TestMethod]
		//Setting the pokemon levels controls the experience points
		public void Pokemon_Set_ExperiencePoints_To_Match_Level()
		{
			Assert.Inconclusive("Missing pokemon method to assign level by integer value"); //Is it even needed?
			byte lv = 7;
			Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
			//pokemon.Level = lv;
			Assert.AreEqual(lv,Experience.GetLevelFromExperience(pokemon.GrowthRate, pokemon.Exp));
		}

		[TestMethod]
		// Setting the pokemon experience points controls the level
		public void Pokemon_Set_Level_To_Match_ExperiencePoints()
		{
			byte lv = 7;
			Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR, level: 3);
			pokemon.Experience.AddExperience(Experience.GetStartExperience(pokemon.GrowthRate, 300));
			pokemon.Experience.AddExperience(Experience.GetStartExperience(pokemon.GrowthRate, lv) - pokemon.Exp);
			Assert.AreEqual(lv, Experience.GetLevelFromExperience(pokemon.GrowthRate, pokemon.Exp));
		}

		[TestMethod]
		public void Pokemon_Spawn_At_Set_Level()
		{
			byte lv = 5;
			Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR, level: lv);
			Assert.AreEqual(lv, Experience.GetLevelFromExperience(pokemon.GrowthRate, pokemon.Exp));
		}

		//[TestMethod]
		//public void test_experience()
		//{
		//	/*def test_experience(use_update):
		//	pkmn, expected = voltorb_and_dict()
		//	for exp in 2197, 2200:
		//		if use_update:
		//			pkmn.update(exp=exp)
		//		else:
		//			pkmn.exp = exp
		//		assert pkmn.exp == exp
		//		assert pkmn.experience_rung.experience <= pkmn.exp
		//		assert pkmn.next_experience_rung.experience > pkmn.exp
		//		assert pkmn.experience_rung.level + 1 == pkmn.next_experience_rung.level
		//		assert (pkmn.experience_rung.growth_rate ==
		//			pkmn.next_experience_rung.growth_rate ==
		//			pkmn.species.growth_rate)
		//		assert pkmn.level == pkmn.experience_rung.level
		//		assert pkmn.exp_to_next == pkmn.next_experience_rung.experience - pkmn.exp
		//		rung_difference = (pkmn.next_experience_rung.experience -
		//			pkmn.experience_rung.experience)
		//		assert pkmn.progress_to_next == (
		//			pkmn.exp - pkmn.experience_rung.experience) / float(rung_difference)
		//		if exp == 2197:
		//			expected['level'] = 13
		//		else:
		//			expected['exp'] = exp
		//			expected['level'] = 13
		//		check_with_roundtrip(5, pkmn, expected)*/
		//	Assert.Inconclusive();
		//}

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

		[TestMethod]
		public void Pokemon_IV_SoftCaps_At_MaxLevel()
		{
			Assert.Inconclusive("Not implemented yet");
			//Each time a pokemon levels up, they should gain new IV points
			//If Pokemon is at MaxLevel, they should no longer gain anymore IV points
			//Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR, level:, moves:, ivs:);
			//Assert.AreEqual(Pokemon.EVSTATLIMIT, pokemon.IV[(int)Stats.SPATK]);
		}

		[TestMethod]
		public void Pokemon_NPC_TrainerPokemon_IV_IsRandom()
		{
			Assert.Inconclusive("Not implemented yet");
			//Create new pokemon with same information
			//check if stats is random each time
			//Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR, level:, moves:, ivs:);
			//Assert.AreEqual(Pokemon.EVSTATLIMIT, pokemon.IV[(int)Stats.SPATK]);
		}

		[TestMethod]
		public void Pokemon_NPC_TrainerPokemon_IV_LessThen_MaxIV()
		{
			Assert.Inconclusive("Not implemented yet");
			//create new pokemon with iv points capped
			//assert if points assigned is equal to amount given
			//Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR, level:, moves:, ivs:);
			//Assert.AreEqual(Pokemon.EVSTATLIMIT, pokemon.IV[(int)Stats.SPATK]);
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
			//Assert.AreEqual(Pokemon.EVLIMIT, evstring.Format("EV Limit:{0}; EV Total:{1}; EV:[{2},{3},{4},{5},{6},{7}]", Pokemon.EVLIMIT, ev, pokemon.EV[0], pokemon.EV[1], pokemon.EV[2], pokemon.EV[3], pokemon.EV[4], pokemon.EV[5]));
			Assert.IsTrue(Pokemon.EVLIMIT >= ev, string.Format("EV Limit:{0}; EV Total:{1}; EV:[{2},{3},{4},{5},{6},{7}]", Pokemon.EVLIMIT, ev, pokemon.EV[0], pokemon.EV[1], pokemon.EV[2], pokemon.EV[3], pokemon.EV[4], pokemon.EV[5]));
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
						&& egg.Contains(move.MoveId)
					)
					{
						//Assert.IsTrue if Pokemon.Move Contains exclusive egg-only moves.
						Assert.IsFalse(!true, "Test is malfunctioning and refuses to mark as success");//"Pokemon contains exclusive egg only move"
						Assert.IsTrue(!false, "Test is malfunctioning and refuses to mark as success");//"Pokemon contains exclusive egg only move"
						Assert.AreEqual(true,!false, "Test is malfunctioning and refuses to mark as success");//"Pokemon contains exclusive egg only move"
						//Assert.AreSame(true, true);//"Pokemon contains exclusive egg only move"
						Assert.IsTrue(egg.Contains(move.MoveId), "Test is malfunctioning and refuses to mark as success");//"Pokemon contains exclusive egg only move"
						Assert.AreEqual(true, egg.Contains(move.MoveId), "Test is malfunctioning and refuses to mark as success");//"Pokemon contains exclusive egg only move"
						CollectionAssert.Contains(egg, move.MoveId, "Test is malfunctioning and refuses to mark as success");//"Pokemon contains exclusive egg only move"
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
			//CollectionAssert.AreNotEqual(before, new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
			CollectionAssert.AreNotEquivalent(before, new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
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
			//CollectionAssert.AreNotEqual(before, new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
			CollectionAssert.AreNotEquivalent(before, new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
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
			//CollectionAssert.AreEqual(before, new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
			CollectionAssert.AreEquivalent(before, new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
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
			//CollectionAssert.AreEqual(before, new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
			CollectionAssert.AreEquivalent(before, new Moves[] { pokemon.moves[0].MoveId, pokemon.moves[1].MoveId, pokemon.moves[2].MoveId, pokemon.moves[3].MoveId });
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
		[TestMethod]
		public void Pokemon_TestPokemon_CanEvolve_AfterLevel()
		{
			Pokemon pokemon = new Pokemon(Pokemons.BULBASAUR);
			if (!pokemon.hasEvolveMethod(EvolutionMethod.Level))
				Assert.Fail("Unable to test if pokemon can evolve, as it does not have an evolution through leveling-up");
			//Add exp
			pokemon.Experience.AddExperience(105000);
			//Assert is true
			Assert.AreEqual(Pokemons.IVYSAUR, pokemon.CanEvolveAfter(EvolutionMethod.Level, pokemon.Level)[0]);
		}
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
		///	<see cref="Pokemons.UNOWN"/> = letter of the alphabet.
		///	<see cref="Pokemons.DEOXYS"/> = which of the four forms.
		///	<see cref="Pokemons.BURMY"/>/<see cref="Pokemons.WORMADAM"/> = cloak type. Does not change for Wormadam.
		///	<see cref="Pokemons.SHELLOS"/>/<see cref="Pokemons.GASTRODON"/> = west/east alt colours.
		///	<see cref="Pokemons.ROTOM"/> = different possesed appliance forms.
		///	<see cref="Pokemons.GIRATINA"/> = Origin/Altered form.
		///	<see cref="Pokemons.SHAYMIN"/> = Land/Sky form.
		///	<see cref="Pokemons.ARCEUS"/> = Type.
		///	<see cref="Pokemons.BASCULIN"/> = appearance.
		///	<see cref="Pokemons.DEERLING"/>/<see cref="Pokemons.SAWSBUCK"/> = appearance.
		///	<see cref="Pokemons.TORNADUS"/>/<see cref="Pokemons.THUNDURUS"/>/<see cref="Pokemons.LANDORUS"/> = Incarnate/Therian forms.
		///	<see cref="Pokemons.KYUREM"/> = Normal/White/Black forms.
		///	<see cref="Pokemons.KELDEO"/> = Ordinary/Resolute forms.
		///	<see cref="Pokemons.MELOETTA"/> = Aria/Pirouette forms.
		///	<see cref="Pokemons.GENESECT"/> = different Drives.
		///	<see cref="Pokemons.VIVILLON"/> = different Patterns.
		///	<see cref="Pokemons.FLABEBE"/>/<see cref="Pokemons.FLOETTE"/>/<see cref="Pokemons.FLORGES"/> = Flower colour.
		///	<see cref="Pokemons.FURFROU"/> = haircut.
		///	<see cref="Pokemons.PUMPKABOO"/>/<see cref="Pokemons.GOURGEIST"/> = small/average/large/super sizes. 
		///	<see cref="Pokemons.HOOPA"/> = Confined/Unbound forms.
		///	<see cref="Pokemons.CASTFORM"/>? = different weather forms
		///	<see cref="Pokemons.PIKACHU"/>
		//Test game doesnt crash if no form
		//Test pokedex to record/show (only) first form seen 
		//Test pokedex to record/show (different?) form captured 
		//Test pokemon has mega form
		//Moves learned changes with form
		[TestMethod]
		public void Pokemon_Form_SetForm_To_FormB()
		{
			Pokemon pokemon = new Pokemon(Pokemons.UNOWN);
			pokemon.Form = 1;
			CollectionAssert //Assert
				.AreEquivalent( //.AreEqual(
					new object[] { Pokemons.UNOWN, Forms.UNOWN_B }, 
					new object[] { pokemon.Species, Game.PokemonFormsData[pokemon.Species][pokemon.Form].Id },
					string.Format("Form: {0}, Id: {1}", pokemon.Form, Game.PokemonFormsData[pokemon.Species][pokemon.Form].Id)
				);
			//Assert.Inconclusive("Not implemented yet");
		}
		[TestMethod]
		public void Pokemon_Form_SetForm_Fails_On_NoForms()
		{
			Pokemon pokemon = new Pokemon(Pokemons.DEOXYS); //Normal
			pokemon.Form = 2; //Defense
			pokemon.Form = 5; //Not Indexed
			Assert.AreEqual(Forms.DEOXYS_DEFENSE, Game.PokemonFormsData[pokemon.Species][pokemon.Form].Id);
			//Assert.AreEqual(2, Game.PokemonFormsData[pokemon.Species][pokemon.Form].GetArrayId());
			//Assert.Inconclusive("Not implemented yet");
		}
		[TestMethod]
		//Changing form changes base stats
		public void Pokemon_Form_SetForm_ChangesStats()
		{
			Pokemon pokemon = new Pokemon(Pokemons.ROTOM);
			pokemon.AddExperience(100000, false);
			//if (Game.PokemonData[pokemon.Species].BaseStatsATK != pokemon.ATK) Assert.Fail("Bad Test; Attack not equal to Base Stats");
			int[] stat = new int[] { pokemon.HP, pokemon.ATK, pokemon.DEF, pokemon.SPA, pokemon.SPD, pokemon.SPE };
			//int[] stat = new int[] { pokemon.BaseStatsHP, pokemon.BaseStatsATK, pokemon.BaseStatsDEF, pokemon.BaseStatsSPA, pokemon.BaseStatsSPD, pokemon.BaseStatsSPE };
			pokemon.Form = 1; //Rotom_Heat
			if(Game.PokemonFormsData[pokemon.Species][pokemon.Form].Id != Forms.ROTOM_HEAT) Assert.Fail("Bad Test; Wrong Stats being modified.");
			//Assert.AreNotEqual(Game.PokemonData[pokemon.Species].BaseStatsATK, pokemon.ATK);
			//Assert.AreNotEqual(pokemon.ATK, stat, "No changes in Pokemon stats.");
			CollectionAssert.AreNotEquivalent(stat, new int[] { pokemon.HP, pokemon.ATK, pokemon.DEF, pokemon.SPA, pokemon.SPD, pokemon.SPE }, "No changes in Pokemon stats.");
			//CollectionAssert.AreNotEquivalent(stat, new int[] { pokemon.BaseStatsHP, pokemon.BaseStatsATK, pokemon.BaseStatsDEF, pokemon.BaseStatsSPA, pokemon.BaseStatsSPD, pokemon.BaseStatsSPE }, "No changes in Pokemon stats.");
			//Assert.Fail("Need to find way to compare Pokemon.baseStats to Form.baseStats");
			//Assert.Inconclusive("Not implemented yet");
		}
		//[TestMethod]
		//public void Pokemon_TestPokemon_GetPokemon_From_Form()
		//{
		//	//Maybe this one isnt needed?... 
		//	Assert.Inconclusive();
		//}
		#endregion

		#region Misc
		[TestMethod]
		public void Pokemon_WildPokemon_With_Item()
		{
			Pokemon pkmn = new Pokemon(Pokemons.RATICATE);//Pokemons.BUTTERFREE
			if (Game.PokemonItemsData[pkmn.Species].Length == 0)
				Assert.Fail("Pokemon does not contain any held items in wild");

			//Instantiate wild pokemon, maybe 100 times
			for (int i = 0; i < 100; i++)
			{
				//check to see if wild pokemon is created with held item
				pkmn.SwapItem(PokemonWildItems.GetWildHoldItem(pkmn.Species));
				//pass if held item is true
				if (pkmn.Item != Items.NONE)
					//Assert.AreNotEqual()
					//Assert.IsTrue(true, "test isnt passing, and i dont know why");
					Assert.AreNotEqual(Items.NONE, pkmn.Item, "test isnt passing, and i dont know why");
				//Maybe this one isnt needed?...
				//Wild pokemons are (any/instantiated) pkmns without trainers?
			}
			//Assert.Inconclusive();
			//Assert.Fail("Pokemon did not receive a held item");
			Assert.AreNotEqual(Items.NONE, pkmn.Item, "Pokemon did not receive a held item");
		}
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
			//Can only get the next ribbon if previous one has been acquired
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