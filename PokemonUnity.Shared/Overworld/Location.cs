using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Overworld
{
	public struct Location
	{
		/// <summary>
		/// </summary>
		public int Region;
		/// <summary>
		/// </summary>
		//public int Generation;
		public int[] Versions;
		/// <summary>
		/// </summary>
		public int Area;
		/// <summary>
		/// </summary>
		public Locations Location;
		/// <summary>
		/// </summary>
		public int MatrixId;
		/// <summary>
		/// </summary>
		/// Town, Route, Dungeon, Underground, Safari
		/// ToDo: C? P? W? Union? Direct?
		public int Type;
	}
}