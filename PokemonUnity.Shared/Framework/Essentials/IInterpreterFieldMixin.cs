using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Interface;
using System;

namespace PokemonEssentials.Interface
{
	/// <summary>
	/// Logic that's used by Game while player is in `Overworld_Field`
	/// </summary>
	public interface IInterpreterFieldMixin : IInterpreter
	{
		//object[] Params { get; }

		//object getVariable(params int[] arg);
		object getVariable(object arg = null);
		PokemonEssentials.Interface.PokeBattle.IPokemon GetPokemon(int id);
		bool Headbutt();
		bool PushThisBoulder();
		/// <summary>
		/// Used in boulder events. Allows an event to be pushed. To be used in
		/// a script event command.
		/// </summary>
		void PushThisEvent();
		//void SetEventTime(params int[] arg);
		void SetEventTime(object arg);
		/// <summary>
		/// Used when player encounters a trainer, and battle logic get triggered
		/// </summary>
		/// <param name="symbol"></param>
		/// <returns>Plays TrainerType Audio Theme and returns true</returns>
		bool TrainerIntro(TrainerTypes symbol);
		void TrainerEnd();
		void setTempSwitchOff(string c);
		void setTempSwitchOn(string c);
		//void setVariable(params int[] arg);
		void setVariable(object arg);
		bool tsOff(string c);
		bool tsOn(string c);
	}
}