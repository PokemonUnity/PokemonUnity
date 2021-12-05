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
		private MoveData _base { get; set; }
		public int pp { get; set; } //ToDo: Set to private and rename across entire solution.
		/// <summary>
		/// The amount of PP remaining for this move
		/// </summary>
		/// Could possibly replace SET as a new constructor
		public byte PP
		{
			get { return this.pp; } //ToDo: If greater than totalPP throw error?
			set
			{
				this.pp = value < 0 ? (byte)0 : (value > this.TotalPP ? TotalPP : value);
				//this.pp = (this.PP + value).Clamp(0, this.TotalPP);
			}
		}
		/// <summary>
		/// Gets the maximum PP for this move.
		/// </summary>
		public byte TotalPP
		{
			get
			{
				return (byte)(_base.PP + (int)Math.Floor(_base.PP * PPups / 5d));
			}
		}
		/// <summary>
		/// The number of PP Ups used for this move
		/// </summary>
		public int PPups { get; set; }
		public int ppup { get; set; } //ToDo: Rename across entire solution
		public int totalpp { get; set; } //ToDo: Rename across entire solution
		#region Values to be Overridden while in Battle?
		/// <summary>
		/// Base Damage
		/// </summary>
		public int? Power			{ get; private set; } //{ get { return _base.Power; } }    
		public int? Accuracy		{ get; private set; } //{ get { return _base.Accuracy; } }  
		public Types type			{ get; private set; } //ToDo: Rename across entire solution
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
		public Moves id				{ get; private set; } //ToDo: Rename across entire solution
		public Moves MoveId			{ get; private set; } //{ get { return _base.ID; } }
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
			initialize();
		}

		/// <summary>
		/// Initializes this object to the specified move ID, PP and added PPup
		/// </summary>
		/// Either this or {public get, public set}
		public Move(Moves move, int ppUp, byte pp) : this(move: move)
		{
			//_base = Game.MoveData[move];
			PPups	= ppUp;
			PP		= pp;
		}

		/// <summary>
		/// Initializes this object to the specified move ID.
		/// </summary>
		public PokemonEssentials.Interface.IMove initialize(Moves move = Moves.NONE)
		{
			/*if (move != Moves.NONE)*/
			_base = Game.MoveData[move];
			MoveId = move;
			PPups = 0;
			PP = _base.PP;

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