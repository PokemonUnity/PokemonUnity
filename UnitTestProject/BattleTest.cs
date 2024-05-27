using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Attack;
using PokemonUnity.Monster;
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
		#region Battle Simulation
		[TestMethod]
		public void Battle_Initialization_CorrectSetup()
		{
			// Arrange
			var mockScene = new Mock<IScene>();
			IPokemon[] party1 = { new Mock<IPokemon>().Object };
			IPokemon[] party2 = { new Mock<IPokemon>().Object };
			ITrainer player =	new Mock<ITrainer>().Object;
			ITrainer opponent = new Mock<ITrainer>().Object;

			// Act
			var battle = new Battle(mockScene.Object, party1, party2, player, opponent);

			// Assert
			Assert.IsNotNull(battle);
			Assert.AreEqual(mockScene.Object, battle.scene);
			Assert.AreEqual(party1, battle.party1);
			Assert.AreEqual(party2, battle.party2);
			Assert.AreEqual(player, battle.player);
			Assert.AreEqual(opponent, battle.opponent);
		}

		[TestMethod]
		public void Battle_Abort_ThrowsBattleAbortedException()
		{
			// Arrange
			var battle = new Battle();

			// Act & Assert
			Assert.Throws<BattleAbortedException>(() => battle.Abort());
		}

		[TestMethod]
		public void Battle_Random_ReturnsValueWithinRange()
		{
			// Arrange
			var battle = new Battle();
			int index = 10;

			// Act
			int result = battle.Random(index);

			// Assert
			Assert.GreaterOrEqual(result, 0);
			Assert.Less(result, index);
		}

		[TestMethod]
		public void Battle_StorePokemon_DisplaysCorrectMessageForNonShadowPokemon()
		{
			// Arrange
			var mockScene = new Mock<IScene>();
			var mockPokemon = new Mock<IPokemon>();
			mockPokemon.Setup(p => p.isShadow).Returns(false);
			var battle = new Battle();
			battle.scene = mockScene.Object;

			// Act
			battle.StorePokemon(mockPokemon.Object);

			// Assert
			mockScene.Verify(s => s.DisplayPaused(It.IsAny<string>()), Times.AtLeastOnce);
		}

		[TestMethod]
		public void Battle_ThrowPokeball_IncreasesTurnCount()
		{
			// Arrange
			var battle = new Battle();
			int initialTurnCount = battle.turncount;
			int idxPokemon = 0;
			Items ball = Items.POKE_BALL;

			// Act
			battle.ThrowPokeball(idxPokemon, ball);

			// Assert
			Assert.Greater(battle.turncount, initialTurnCount);
		}
		#endregion

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

		#region AI
		// This test checks if shadow moves are indeed preferred by increasing the score as expected.
		[TestMethod]
		public void GetMoveScore_ShadowMove_IncreasesScore()
		{
			var battle = new Battle();
			var move = new Mock<IBattleMove>();
			move.Setup(m => m.Type).Returns(Types.SHADOW);
			var attacker = new Mock<IBattler>();
			var opponent = new Mock<IBattler>();
			int skill = 100;

			int score = battle.GetMoveScore(move.Object, attacker.Object, opponent.Object, skill);

			Assert.Greater(score, 100); // Expecting score to be greater than the base score of 100
		}

		[TestMethod]
		public void GetMoveScore_ShadowMove_DecreasesScore()
		{
			var battle = new Battle();
			var move = new Mock<IBattleMove>();
			move.Setup(m => m.Type).Returns(Types.SHADOW);
			var attacker = new Mock<IBattler>();
			var opponent = new Mock<IBattler>();
			int skill = 100;

			int score = battle.GetMoveScore(move.Object, attacker.Object, opponent.Object, skill);

			Assert.Less(score, 100); // Expecting score to be less than the base score of 100
		}

		// This test verifies that the AI with high skill will prioritize moves that can induce sleep, adjusting the score accordingly.
		// This test checks if sleep moves are indeed preferred by increasing the score as expected.
		[TestMethod]
		public void GetMoveScore_SleepEffect_HighSkill_IncreasesScore()
		{
			var battle = new Battle();
			var move = new Mock<IBattleMove>();
			move.Setup(m => m.Effect).Returns(Attack.Effects.x002); // Sleep effect
			var attacker = new Mock<IBattler>();
			var opponent = new Mock<IBattler>();
			opponent.Setup(o => o.effects.Yawn).Returns(0);
			opponent.Setup(o => o.HasWorkingAbility(It.IsAny<Abilities>())).Returns(false);
			int skill = TrainerAI.highSkill;

			int score = battle.GetMoveScore(move.Object, attacker.Object, opponent.Object, skill);

			Assert.Greater(score, 100); // Expecting score to be increased due to sleep effect
		}

		// This test ensures that the AI with the best skill level correctly evaluates the Attract move's effectiveness.
		[TestMethod]
		public void GetMoveScore_AttractEffect_BestSkill_AdjustsScore()
		{
			var battle = new Battle();
			var move = new Mock<IBattleMove>();
			move.Setup(m => m.Effect).Returns(Attack.Effects.x079); // Attract effect
			var attacker = new Mock<IBattler>();
			attacker.Setup(a => a.Gender).Returns(Genders.MALE);
			var opponent = new Mock<IBattler>();
			opponent.Setup(o => o.Gender).Returns(Genders.FEMALE);
			opponent.Setup(o => o.effects.Attract).Returns(-1);
			int skill = TrainerAI.bestSkill;

			int score = battle.GetMoveScore(move.Object, attacker.Object, opponent.Object, skill);

			Assert.AreNotEqual(0, score); // Score should be adjusted based on the effectiveness of Attract
		}

		// This test checks if the AI with medium skill properly increases the score for moves that can poison the opponent.
		[TestMethod]
		public void GetMoveScore_PoisonEffect_MediumSkill_IncreasesScore()
		{
			var battle = new Battle();
			var move = new Mock<IBattleMove>();
			move.Setup(m => m.Effect).Returns(Attack.Effects.x003); // Poison effect
			var attacker = new Mock<IBattler>();
			var opponent = new Mock<IBattler>();
			opponent.Setup(o => o.CanPoison(It.IsAny<IBattler>(), It.IsAny<bool>())).Returns(true);
			int skill = TrainerAI.mediumSkill;

			int score = battle.GetMoveScore(move.Object, attacker.Object, opponent.Object, skill);

			Assert.Greater(score, 100); // Expecting score to be increased for poison effect
		}

		// This test ensures that moves with no extra effect are evaluated correctly by an AI with the minimum skill level.
		[TestMethod]
		public void GetMoveScore_NoExtraEffect_MinimumSkill_BaseScore()
		{
			var battle = new Battle();
			var move = new Mock<IBattleMove>();
			move.Setup(m => m.Effect).Returns(Attack.Effects.x00); // No extra effect
			var attacker = new Mock<IBattler>();
			var opponent = new Mock<IBattler>();
			int skill = TrainerAI.minimumSkill;

			int score = battle.GetMoveScore(move.Object, attacker.Object, opponent.Object, skill);

			Assert.AreEqual(100, score); // Base score expected as there's no extra effect
		}
		#endregion
	}
}