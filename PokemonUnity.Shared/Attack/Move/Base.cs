using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;

namespace PokemonUnity.Attack
{
	/*public partial class Move
	{
		[System.Serializable]
		public partial class MoveData
		{
			#region Variables
			public Category Category { get; private set; }
			public int num { get; private set; }
			public Moves ID { get; private set; }
			/// <summary>
			/// The move's accuracy, as a percentage. 
			/// An accuracy of 0 means the move doesn't perform an accuracy check 
			/// (i.e. it cannot be evaded).
			/// </summary>
			public int Accuracy { get; private set; }
			public int BaseDamage { get; private set; }
			public byte PP { get; private set; }
			public int Priority { get; private set; }
			public Flags Flags { get; private set; }
			public Target Target { get; private set; }
			public Types Type { get; private set; }
			public Contests ContestType { get; private set; }
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

			public MoveData()
			{
				//ToDo: Load Text
				//Name = LanguageExtension.Translate(Text.Moves, ID.ToString()).Name;
				//Description = LanguageExtension.Translate(Text.Moves, ID.ToString()).Value;
			}

			internal MoveData getMove(Moves ID)
			{
				foreach (MoveData move in Database)
				{
					if (move.ID == ID) return move;
				}
				//if ((int)ID > 559) //Database only goes up to a certain gen, right now
					return Database[0]; //return the database value for "none"
				//throw new System.Exception("Move ID doesnt exist in the database. Please check MoveData constructor.");
			}
		}
	}*/
}