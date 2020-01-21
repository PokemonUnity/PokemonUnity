using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;

namespace PokemonUnity.Monster.Data
{
    /// <summary>
    /// The moves that all Pokémon of the species learn as they level up. 
    /// </summary>
    public struct PokemonMoveset
    {
        public LearnMethod TeachMethod;
        /// <summary>
        /// Level at which the move is learned 
        /// (0 means the move can only be learned when 
        /// a Pokémon evolves into this species).
        /// Default level is 0; 
        /// only really important if learn method is <see cref="LearnMethod.levelup"/>
        /// </summary>
        /// Level needed to learn Move
        /// leaves door open to new game designs, if players want to limit move methods to a level requirement
        /// use to set min or max level needed for designated learn method
        public int Level;
        /// <summary>
        /// Move learned upon leveling-up
        /// </summary>
        public Moves MoveId;
        //public int Generation;
        public PokemonMoveset(Moves moveId, LearnMethod method = LearnMethod.levelup, int level = 0, int? generation = null) //: this()
        {
            this.Level = level;
            this.MoveId = moveId;
            this.TeachMethod = method;
			//if (generation.HasValue) this.Generation = generation.Value;
			//else this.Generation = 0;
        }
    }
}