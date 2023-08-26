using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PokemonEssentials.Interface.EventArg;
using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;
using PokemonUnity.Saving.SerializableClasses;

namespace PokemonUnity.Character
{
	/// <summary>
	/// </summary>
	/// Daycare Data
	[System.Serializable] public struct DayCareSlotData
	{
		#region Variables
		/// <summary>
		/// Pokemon Occupying slot, if slot is occupied at all
		/// </summary>
		public SeriPokemon? Pokemon	{ get; private set; }
		/// <summary>
		/// Steps Taken Since Depositing 
		/// </summary>
		public int Steps			{ get; private set; }
		#endregion

		#region Constructor
		public DayCareSlotData(SeriPokemon pokemon, int steps)
		{
			Pokemon = pokemon;
			Steps = steps;
		}
		#endregion
	}

	//ToDo: Missing Variable for DayCare, maybe `Pokemon[,]` for multipe locations?
	// or even better `KeyValuePair<int,DayCareSlotData[]>` with each int as location id
	[System.Serializable] public struct DayCareData
	{
		#region Variables
		/// <summary>
		/// </summary>
		public DayCareSlotData[] Slot	{ get; private set; }
		/// <summary>
		/// Flag (egg available) 
		/// </summary>
		public bool HasEgg				{ get; private set; }
		/// <summary>
		/// RNG Seed
		/// </summary>
		//public int Seed				{ get; }
		#endregion

		#region Constructor
		public DayCareData(DayCareSlotData slot1, DayCareSlotData slot2, bool hasEgg)
		{
			Slot = new DayCareSlotData[2];
			Slot[0] = slot1;
			Slot[1] = slot2;
			HasEgg = hasEgg;
		}
		#endregion
	}
}