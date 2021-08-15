using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Attack.Data;

namespace PokemonUnity.Offline.Attack
{
	/// <inheritdoc/>
	public class Move : PokemonUnity.Attack.Move
	{		
		/// <summary>
		/// Initializes this object to the specified move ID.
		/// </summary>
		public Move(Moves move = Moves.NONE) : base(move)
		{
		}

		/// <summary>
		/// Initializes this object to the specified move ID, PP and added PPup
		/// </summary>
		/// Either this or {public get, public set}
		public Move(Moves move, int ppUp, byte pp) : this (move: move)
		{
			//_base = Game.MoveData[move];
			PPups = ppUp;
			PP = pp;
		}

		#region Methods
		#endregion
	}
}