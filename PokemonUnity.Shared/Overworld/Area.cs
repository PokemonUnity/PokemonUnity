using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Overworld
{
	public struct Area
	{
		///// <summary>
		///// </summary>
		//public int Rate { get; private set; }
		//public Method Method { get; private set; }
		/// <summary>
		/// </summary>
		public KeyValuePair<Method,int>[] Rate { get; private set; }
		/// <summary>
		/// Returns list of Encounters
		/// <seealso cref="Encounters(Method)"/>
		/// </summary>
		//public int[] Encounters { get { return Game.MethodData[Method]; } }//{ get { return Game.EncounterData[Id]; } }
		//public int[] this[Method method] { get { return Game.MethodData[method]; } }//{ get { return Game.EncounterData[Id]; } }
		public int[] this[Method method] { get { return Encounters(method); } }
		/*// <summary>
		/// </summary>
		public string Identifier { get; private set; }*/
		/// <summary>
		/// </summary>
		public int Id { get; private set; }

		public Area(int id, KeyValuePair<Method,int>[] rate)//Method method, int rate = 0, string identifier = null)
		{
			Id = id;
			Rate = rate;
			//Method = method;
			//Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
		}
		/// <summary>
		/// </summary>
		public int[] Encounters (Method method) //{ get 
		{ return Game.MethodData[method]; } //}//{ get { return Game.EncounterData[Id]; } }
	}
}
