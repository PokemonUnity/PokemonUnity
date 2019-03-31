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

public partial class Game
{
	public class TrainerPC
	{
		//public static PC
		private Player trainer { get; set; }
		private int activeBox { get; set; }
		public string Name { get { return Game.PC_boxNames[activeBox] ?? "Box " + (activeBox + 1).ToString(); } }
		public int Texture { get { return Game.PC_boxTexture[activeBox]; } }
		public Pokemon[] Pokemons
		{
			get
			{
				Pokemon[] p = new Pokemon[30];
				for (int t = 0; t < 30; t++)
				{
					p[t] = Game.PC_Poke[activeBox, t];
				}
				return p;
			}
		}
		/// <summary>
		/// </summary>
		/// ToDo: Add filter to add/remove items...
		public List<Item> Items { get { return Game.PC_Items; } set { Game.PC_Items = value; } }

		public TrainerPC this[int i]
		{
			get
			{
				i = i % Settings.STORAGEBOXES;
				this.activeBox = i;
				//Pokemon[] p = new Pokemon[30];
				//for (int t = 0; t < 30; t++)
				//{
				//	p[t] = Game.PC_Poke[i, t];
				//}
				//this.Pokemons = p;
				//this.Texture = Game.PC_boxTexture[i];
				//this.Name = Game.PC_boxNames[i] ?? "Box " + (i + 1).ToString();
				return this;
			}
		}

		public TrainerPC()
		{
		}

		public TrainerPC(Player t, int? box = null) : this()
		{
			trainer = t;
			if (box.HasValue)
				activeBox = box.Value % Settings.STORAGEBOXES;
		}

		public bool hasSpace()
		{
			if (getBoxCount().HasValue && getBoxCount().Value < 30) return true;
			else return false;
		}

		public int? getBoxCount()
		{
			int result = 0;
			for (int i = 0; i < Pokemons.Length; i++)
			{
				if (Pokemons[i] != null || Pokemons[i].Species != PokemonUnity.Pokemons.NONE)
				{
					result += 1;
				}
			}
			return result;
		}

		public int? getIndexOfFirstEmpty()
		{
			//int result = 0;
			for (int i = 0; i < Pokemons.Length; i++)
			{
				if (Pokemons[i] == null || Pokemons[i].Species == PokemonUnity.Pokemons.NONE)
				{
					return i;
				}
			}
			return null;
		}

		/*public int getBoxCount(int box)
		{
			int result = 0;
			for (int i = 0; i < Pokemons[box].Length; i++)
			{
				if (Pokemons[box,i] != null || Party[i].Species != Pokemons.NONE)
				{
					result += 1;
				}
			}
			return result;
		}*/

		/// <summary>
		/// Add a new pokemon directly to active box. 
		/// If pokemon could not be added return false.
		/// </summary>
		/// <param name="acquiredPokemon"></param>
		/// <returns></returns>
		public bool addPokemon(Pokemon acquiredPokemon)
		{
			//attempt to add to the earliest available opening in active box. no array packing needed.
			if (hasSpace())
			{
				//Pokemons[getIndexOfFirstEmpty().Value] = acquiredPokemon;
				Game.PC_Poke[activeBox, getIndexOfFirstEmpty().Value] = acquiredPokemon;
				return true;
			}
			//if could not add a pokemon, return false. Party and PC are both full.
			return false;
		}

		public void swapPokemon(int box1, int pos1, int box2, int pos2)
		{
			Pokemon temp = Game.PC_Poke[box1, pos1];
			Game.PC_Poke[box1, pos1] = Game.PC_Poke[box2, pos2];
			Game.PC_Poke[box2, pos2] = temp;
		}
	}
}

namespace PokemonUnity.Character
{	
}