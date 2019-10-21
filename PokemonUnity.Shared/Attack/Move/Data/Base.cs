using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;

namespace PokemonUnity.Attack.Data
{
    public struct MoveData
    {
        #region Variables
        public Category Category { get; private set; }
        public int num { get; private set; }
        public Moves ID { get; private set; }
        public int GenerationId { get; private set; }
        /// <summary>
        /// The move's accuracy, as a percentage. 
        /// An accuracy of 0 means the move doesn't perform an accuracy check 
        /// (i.e. it cannot be evaded).
        /// </summary>
        public int Accuracy { get; private set; }
		/// <summary>
		/// Power
		/// </summary>
        public int BaseDamage { get; private set; }
        public byte PP { get; private set; }
        public int Priority { get; private set; }
        public Move.Flags Flags { get; private set; }
        public Target Target { get; private set; }
        public Types Type { get; private set; }
        public Contest ContestType { get; private set; }
        public short Function { get; private set; }
        public string FunctionAsString { get; private set; }
        /// <summary>
        /// The probability that the move's additional effect occurs, as a percentage. 
        /// If the move has no additional effect (e.g. all status moves), this value is 0.
        /// Note that some moves have an additional effect chance of 100 (e.g.Acid Spray), 
        /// which is not the same thing as having an effect that will always occur. 
        /// Abilities like Sheer Force and Shield Dust only affect additional effects, not regular effects.
        /// </summary>
        public int Effects { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        //ToDo: Missing from Database
        public int Appeal { get; private set; }
        public int Jamming { get; private set; }
        #endregion

		public MoveData(Category category, int num, Moves iD, int generationId, int accuracy, int baseDamage, byte pP, int priority, Move.Flags flags, Target target, Types type, Contest contestType, short function, string functionAsString, int effects, string name, string description, int appeal, int jamming)
		{
			Category = category;
			this.num = num;
			ID = iD;
			GenerationId = generationId;
			Accuracy = accuracy;
			BaseDamage = baseDamage;
			PP = pP;
			Priority = priority;
			Flags = flags;
			Target = target;
			Type = type;
			ContestType = contestType;
			Function = function;
			FunctionAsString = functionAsString;
			Effects = effects;
			Name = name;
			Description = description;
			Appeal = appeal;
			Jamming = jamming;
		}
	}
}