using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;

namespace Tests
{
    [TestClass]
    public class GameTest
	{
		[TestMethod]
		public void Database_Load_Pokemon_Test()
		{
			System.Collections.Generic.List<string[]> s = new System.Collections.Generic.List<string[]>();
			s = PokemonUnity.Game.GetArrayFromSQL("select * from pokemon_views limit 3 --order by id ASC");
			CollectionAssert.AllItemsAreNotNull(s);
		}
	}
}
