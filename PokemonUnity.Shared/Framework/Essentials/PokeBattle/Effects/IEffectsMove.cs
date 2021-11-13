using PokemonUnity;
using PokemonUnity.Attack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonEssentials.Interface.PokeBattle.Effects
{
	/// <summary>
	/// These effects apply to the usage of a move
	/// </summary>
	public interface IEffectsMove { 
		bool SkipAccuracyCheck	{ get; set; }
		bool SpecialUsage		{ get; set; }
		bool PassedTrying		{ get; set; }
		int TotalDamage			{ get; set; }
	}
}