using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonUnity;
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

namespace PokemonUnity.Tests
{
    [TestClass]
    public class MoveTests
    {
		// test the Move class for correct comparison behavior
		#region
        [TestMethod]
        public void TestEqualityOperatorWithSameId()
        {
            var move1 = new Move(Moves.TACKLE);
            var move2 = new Move(Moves.TACKLE);

            Assert.IsTrue(move1 == move2);
        }

        [TestMethod]
        public void TestInequalityOperatorWithDifferentIds()
        {
            var move1 = new Move(Moves.TACKLE);
            var move2 = new Move(Moves.GROWL);

            Assert.IsTrue(move1 != move2);
        }

        [TestMethod]
        public void TestEqualsMethodWithSameId()
        {
            var move1 = new Move(Moves.TACKLE);
            var move2 = new Move(Moves.TACKLE);

            Assert.IsTrue(move1.Equals(move2));
        }

        [TestMethod]
        public void TestEqualsMethodWithNullObject()
        {
            var move = new Move(Moves.TACKLE);

            Assert.IsFalse(move.Equals(null));
        }

        [TestMethod]
        public void TestGetHashCodeWithSameId()
        {
            var move1 = new Move(Moves.TACKLE);
            var move2 = new Move(Moves.TACKLE);

            Assert.AreEqual(move1.GetHashCode(), move2.GetHashCode());
        }
		#endregion

		//Test that the Move class correctly updates the PP and PPups properties when initialized with a move ID, PP, and PPups.
		#region
		#endregion

		//Test that the Move class correctly calculates the TotalPP property based on the base PP and PPups.
		#region
        [TestMethod]
        public void TestTotalPP_NoPPUps()
        {
            var move = new Move(Moves.TACKLE);
            move.PPups = 0; // Assuming TACKLE has a base PP of 35
            Assert.AreEqual(35, move.TotalPP);
        }

        [TestMethod]
        public void TestTotalPP_MaxPPUps()
        {
            var move = new Move(Moves.TACKLE);
            move.PPups = 3; // Assuming maximum PP Ups is 3 and each increases PP by 20%
            Assert.AreEqual(42, move.TotalPP); // 35 base PP + 20% * 3 = 42
        }

        [TestMethod]
        public void TestTotalPP_SinglePPUp()
        {
            var move = new Move(Moves.TACKLE);
            move.PPups = 1; // Assuming each PP Up increases PP by 20%
            Assert.AreEqual(42, move.TotalPP); // 35 base PP + 20% = 42
        }

        [TestMethod]
        public void TestTotalPP_InvalidNegativePPUps()
        {
            var move = new Move(Moves.TACKLE);
            move.PPups = -1; // Negative PP Ups should be treated as 0
            Assert.AreEqual(35, move.TotalPP); // Base PP should remain unaffected
        }

        [TestMethod]
        public void TestTotalPP_ExceedingMaxPPUps()
        {
            var move = new Move(Moves.TACKLE);
            move.PPups = 5; // Assuming the maximum PP Ups allowed is 3
            Assert.AreEqual(42, move.TotalPP); // Should not exceed the PP increase for 3 PP Ups
        }
		#endregion

		//Test that the Move class correctly updates the Power, Accuracy, Type, Targets, Flag, Priority, Appeal, SuperAppeal, Jamming, Effect, and EffectChance properties when initialized with a move ID.
		#region
		#endregion

		//Test that the Move class correctly converts between itself and the SeriMove class using implicit operators.
		#region
		#endregion
    }
}