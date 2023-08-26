using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;

namespace PokemonUnity
{
	public interface IMovedEvent
	{
		float X { get; }
		float Y { get; }
		//float Z { get; }
		float Direction { get; }
		bool? Through { get; }
	}
}