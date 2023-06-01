using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.UX;

namespace PokemonEssentials.Interface
{
	/// <summary>
	/// <seealso cref="IGamePlayer"/>
	/// </summary>
	public interface IGamePlayerRunMovement
	{
		int fullPattern();

		void setDefaultCharName(string chname, int pattern);

		bool CanRun();

		bool IsRunning();

		string character_name { get; }

		//alias update_old update;

		IEnumerator update();
	}
}