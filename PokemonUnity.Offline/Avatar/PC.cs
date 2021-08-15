using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Character;

namespace PokemonUnity.Offline.Avatar
{	
	public class PC : PokemonUnity.Character.PC
	{
		#region Constructors
		public PC() : base()
		{
			//items = new Dictionary<Items, int>() { { Inventory.Items.POTION, 1 } };
		}

		public PC(Pokemon[][] pkmns = null, KeyValuePair<Items,int>[] items = null, byte? box = null, string[] names = null, int[] textures = null) : this()
		{
		}
		#endregion

		#region Methods
		#endregion
	}
}