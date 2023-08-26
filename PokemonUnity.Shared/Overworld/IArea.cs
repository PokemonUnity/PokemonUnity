using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Overworld
{
	/// <summary>
	/// Used to represent which Map Id this object data is associated with
	/// </summary>
	public interface IArea
	{
		///// <summary>
		///// </summary>
		//int Rate { get; private set; }
		//Method Method { get; private set; }
		/// <summary>
		/// </summary>
		KeyValuePair<Method,int>[] Rate { get; }
		/// <summary>
		/// Returns list of Encounters
		/// <seealso cref="Encounters(Method)"/>
		/// </summary>
		//int[] Encounters { get { return Game.MethodData[Method]; } }//{ get { return Game.EncounterData[Id]; } }
		//int[] this[Method method] { get { return Game.MethodData[method]; } }//{ get { return Game.EncounterData[Id]; } }
		int[] this[Method method] { get; }
		/// <summary>
		/// </summary>
		//string Identifier { get; private set; }
		int Id { get;  }

		//Area(int id, KeyValuePair<Method,int>[] rate)//Method method, int rate = 0, string identifier = null)
		//{
		//	Id = id;
		//	Rate = rate;
		//	//Method = method;
		//	//Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
		//}
		/// <summary>
		/// </summary>
		int[] Encounters(Method method); //{ get; }
	}
}