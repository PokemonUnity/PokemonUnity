using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonUnity.Attack.Data
{
	[Serializable]
	public struct MoveData
	{
		#region Variables
		public Category Category { get; private set; }
		public Moves ID { get; private set; }
		public int GenerationId { get; private set; }
		/// <summary>
		/// The move's accuracy, as a percentage.
		/// An accuracy of 0 means the move doesn't perform an accuracy check
		/// (i.e. it cannot be evaded).
		/// </summary>
		public int? Accuracy { get; private set; }
		/// <summary>
		/// Base Damage
		/// </summary>
		public int? Power { get; private set; }
		/// <summary>
		/// </summary>
		/// <remarks>If <see cref="Types.SHADOW"/> PP is EqualTo NULL</remarks>
		public byte PP { get; private set; }
		public int Priority { get; private set; }
		public Flag Flags { get; private set; }
		public Targets Target { get; private set; }
		public Types Type { get; private set; }
		public Contests? ContestType { get; private set; }
		//public short Function { get; private set; }
		//public string FunctionAsString { get; private set; }
		/// <summary>
		/// The probability that the move's additional effect occurs, as a percentage.
		/// If the move has no additional effect (e.g. all status moves), this value is 0.
		/// Note that some moves have an additional effect chance of 100 (e.g.Acid Spray),
		/// which is not the same thing as having an effect that will always occur.
		/// Abilities like Sheer Force and Shield Dust only affect additional effects, not regular effects.
		/// </summary>
		public Effects Effect { get; private set; }
		public int? EffectChance { get; private set; }
		//public int? ContestEffect { get; private set; }
		//public int? ContestSuperEffect { get; private set; }
		//public string Name { get; private set; }
		//public string Description { get; private set; }
		public int? SuperAppeal { get; private set; }
		public int? Appeal { get; private set; }
		public int? Jamming { get; private set; }
		public string Name { get { return string.Format("MOVE_NAME_{0}", (int)ID); } } //Game._INTL(ID.ToString(TextScripts.Name)); } }
		public string Description { get { return string.Format("MOVE_DESC_{0}", (int)ID); } } //ID.ToString(TextScripts.Description); } }
		#endregion

		public MoveData(Moves id = Moves.NONE, Category category = Category.STATUS, int generationId = 0,
			int? accuracy = null, int? baseDamage = null, byte pp = 0, int priority = 0, Flag? flags = null,
			Targets target = Targets.SPECIFIC_MOVE, Types type = Types.NONE, Contests? contestType = null,//Contests.NONE,
			//ToDo: ContestEffects contestEffects, SuperContestEffects superContestEffects,
			short function = 0, string functionAsString = null, Effects effects = Effects.NONE, int? chance = null,
			string name = null, string description = null, int? appeal = null, int? superAppeal = null, int? jamming = null)
		{
			Category = category;
			ID = id;
			GenerationId = generationId;
			Accuracy = accuracy;
			Power = baseDamage;
			PP = pp;
			Priority = priority;
			Flags = flags ?? new Flag();
			Target = target;
			Type = type;
			ContestType = contestType;
			//Function = function;
			//FunctionAsString = functionAsString;
			Effect = effects;
			EffectChance = chance;
			//Name = name;
			//Description = description;
			Appeal = appeal;
			SuperAppeal = superAppeal;
			Jamming = jamming;
		}

		//public string ToString(TextScripts text)
		//{
		//	//ToDo: Create an Interface and Move Function to Application Library
		//	//return ID.ToString(text);
		//	return ID.ToString();
		//}
	}
}