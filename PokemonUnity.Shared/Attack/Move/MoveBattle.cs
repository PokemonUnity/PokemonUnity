using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;

namespace PokemonUnity.Attack
{
    public partial class Move
    {
        /// <summary>
        /// Clones Pokemon's Move stats, and uses those values for pokemon battles.
        /// </summary>
        public class MoveBattle
        {
            #region Variables
            private MoveData _baseData { get; set; }
            private Move _baseMove { get; set; }
            private string _baseBattle { get; set; }

            //private string function { get { return _baseData.Function; } }		//= movedata.function
            private int basedamage { get { return _baseData.BaseDamage; } }     //= movedata.BaseDamage
            private Types type { get { return _baseData.Type; } }               //= movedata.type
            private int accuracy { get { return _baseData.Accuracy; } }         //= movedata.accuracy
                                                                                //private float addlEffect { get { return _baseData.AddlEffect; } }   //= movedata.addlEffect
            private Target target { get { return _baseData.Target; } }          //= movedata.target
            private int priority { get { return _baseData.Priority; } }         //= movedata.priority
            private Flags flags { get { return _baseData.Flags; } }             //= movedata.flags
            private Category category { get { return _baseData.Category; } }    //= movedata.category
            private Move thismove { get; set; }                                 //= move
            private int totalpp { get; set; }
            /// <summary>
            /// For Aerilate, Pixilate, Refrigerate
            /// </summary>
            private bool powerboost;
            #endregion

            #region Property
            /// <summary>
            /// Can be changed with Mimic/Transform
            /// </summary>
            public int PP { get; private set; }                                 //= move.pp
            public int TotalPP
            {
                get
                {
                    if (totalpp > 0) return totalpp;
                    if (thismove != null) return thismove.TotalPP;
                    return 0;
                }
            }

            Moves Id { get { return thismove.MoveId; } }
            #endregion

            //ToDo = Interface to call Move's function
            //public MoveBattle 
        }
    }
}