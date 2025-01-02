using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonEssentials.Interface.PokeBattle.Effects
{
	public interface IActiveSide
	{
		IEffectsSide effects { get; set; }

		IActiveSide initialize();
	}



	public interface IActiveField
	{
		IEffectsField effects { get; set; }

		IActiveField initialize();
	}
}