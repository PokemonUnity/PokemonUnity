using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Attack.Data;

namespace PokemonUnity.Attack
{
	/// <summary>
	/// Use this class to store and reflect changes to an individual move; 
	/// <see cref="MoveData"/> and <see cref="MoveMetaData"/> should remain immutable in 
	/// <seealso cref="IDatabase.MoveData"/> and <seealso cref="IDatabase.MoveMetaData"/>
	/// </summary>
	/// Does this need to be serialized?
	[System.Serializable]
	public class Move : PokemonEssentials.Interface.IMove
	{
		#region Properties
		private MoveData _base { get { return Kernal.MoveData[id]; } } //{ get; set; }
		private int pp { get; set; }
		/// <summary>
		/// The amount of PP remaining for this move
		/// </summary>
		/// Could possibly replace SET as a new constructor
		public int PP
		{
			get { return (byte)this.pp; } //ToDo: If greater than totalPP throw error?
			set
			{
				this.pp = value < 0 ? (byte)0 : (value > this.TotalPP ? TotalPP : value);
				//this.pp = (this.PP + value).Clamp(0, this.TotalPP);
			}
		}
		/// <summary>
		/// Gets the maximum PP for this move.
		/// </summary>
		public int TotalPP
		{
			get
			{
				return (_base.PP + (int)Math.Floor(_base.PP * PPups / 5d));
			}
		}
		/// <summary>
		/// The number of PP Ups used for this move
		/// </summary>
		public int PPups { get; set; }
		#region Values to be Overridden while in Battle?
		/// <summary>
		/// Base Damage
		/// </summary>
		public int? Power			{ get; private set; } //{ get { return _base.basedamage; } }    
		public int? Accuracy		{ get; private set; } //{ get { return _base.Accuracy; } }  
		public Types Type			{ get; private set; } //{ get { return _base.Type; } }
		public Targets Targets		{ get; private set; } //{ get { return _base.Target; } }
		public Flag Flag			{ get; private set; } //{ get { return _base.Flags; } }      
		public int Priority			{ get; private set; } //{ get { return _base.Priority; } }      
		public int? Appeal			{ get; private set; } //{ get { return _base.Appeal; } } 
		public int? SuperAppeal		{ get; private set; } //{ get { return _base.SuperAppeal; } } 
		public int? Jamming			{ get; private set; } //{ get { return _base.Jamming; } } 
		public Effects Effect		{ get; private set; } //{ get { return _base.Effect; } }   
		public int? EffectChance	{ get; private set; } //{ get { return _base.EffectChance; } }        
		public Category Category	{ get; private set; } //{ get { return _base.Category; } }  
		#endregion
		public Moves id				{ get; private set; }
		/// <summary>
		/// For Aerilate, Pixilate, Refrigerate
		/// </summary>
		public bool PowerBoost		{ get; private set; }
		#endregion

		/// <summary>
		/// Initializes this object to the specified move ID.
		/// </summary>
		public Move(Moves move = Moves.NONE)
		{
			initialize(move);
		}

		/// <summary>
		/// Initializes this object to the specified move ID, PP and added PPup
		/// </summary>
		/// Either this or {public get, public set}
		public Move(Moves move, int ppUp, byte pp) : this(move: move)
		{
			//_base = Kernal.MoveData[move];
			PPups	= ppUp;
			PP		= pp;
		}

		/// <summary>
		/// Initializes this object to the specified move ID.
		/// </summary>
		public PokemonEssentials.Interface.IMove initialize(Moves move = Moves.NONE)
		{
			/*if (move != Moves.NONE)*/
			//_base	= Kernal.MoveData[move];
			id		= move;
			PPups	= 0;
			PP		= _base.PP;

			//ToDo: "ResetMoves()" Method?... For end of every battle or pokemon switch...
			Type			= _base.Type;
			Targets			= _base.Target;
			Flag			= _base.Flags;
			Power			= _base.Power;
			Accuracy		= _base.Accuracy;
			Priority		= _base.Priority;
			Category		= _base.Category;
			Appeal			= _base.Appeal;
			SuperAppeal		= _base.SuperAppeal;
			Jamming			= _base.Jamming;
			Effect			= _base.Effect;
			EffectChance	= _base.EffectChance;
			return (PokemonEssentials.Interface.IMove)this;
		}
	}
}