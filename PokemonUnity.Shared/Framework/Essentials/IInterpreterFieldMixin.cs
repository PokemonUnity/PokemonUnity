using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.UX;
using System;

namespace PokemonEssentials.Interface
{
	/// <summary>
	/// Logic that's used by Game while player is in `Overworld_Field`
	/// </summary>
	public interface IInterpreterFieldMixin
	{
		//object[] Params { get; }

		object getVariable(params int[] arg);
		PokemonEssentials.Interface.PokeBattle.IPokemon GetPokemon(int id);
		bool Headbutt();
		bool PushThisBoulder();
		void PushThisEvent();
		void SetEventTime(params int[] arg);
		/// <summary>
		/// Used when player encounters a trainer, and battle logic get triggered
		/// </summary>
		/// <param name="symbol"></param>
		/// <returns>Plays TrainerType Audio Theme and returns true</returns>
		bool TrainerIntro(TrainerTypes symbol);
		void TrainerEnd();
		void setTempSwitchOff(string c);
		void setTempSwitchOn(string c);
		void setVariable(params int[] arg);
		bool tsOff(string c);
		bool tsOn(string c);
	}
}