using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Character;
using PokemonUnity.Monster.Data;

namespace PokemonUnity.Offline.Monster
{
    public partial class Pokemon : PokemonUnity.Monster.Pokemon
    {
        #region Constructor
        /// <summary>
        /// Uses PokemonData to initialize a Pokemon from base stats
        /// </summary>
		/// ToDo: Make Private
        public Pokemon() : base()
        {
        }

        /// <summary>
        /// Uses PokemonData to initialize a Pokemon from base stats
        /// </summary>
        /// <param name="pokemon"></param>
        public Pokemon(Pokemons pokemon) : base(pokemon)
        {
        }
        #endregion
    }
}