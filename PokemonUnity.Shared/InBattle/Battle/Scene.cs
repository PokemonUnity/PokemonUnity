using PokemonUnity;
//using PokemonUnity.Pokemon;
using PokemonUnity.Inventory;
//using PokemonUnity.Attack;
using PokemonUnity.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Battle
{
	public partial class Battle 
	{
		//ToDo: Fix this
		//ToDo: Feed animation thru Battle 
		public class Scene
		{
			public void Fainted(int index) { }
			public void HPChanged(int index, int oldhp, bool animate) { }
			public void pbHPChanged(Pokemon pkmn, int _new) { }
			public void ChangePokemon() { }
			public void pbChangePokemon(Pokemon pkmn, Monster.Forms _new) { }
			public void pDamageAnimation(Pokemon pkmn, int _new) { }
			public void pbDamageAnimation(Pokemon pkmn, int _new) { }
			public void pbAnimation(Moves id, Pokemon attacker, Pokemon opponent, int hitnum) { }
		}
	}
}