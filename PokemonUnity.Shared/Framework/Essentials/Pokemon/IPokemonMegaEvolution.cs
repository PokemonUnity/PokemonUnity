using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Saving;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Utility;
using PokemonUnity.Monster;

namespace PokemonEssentials.Interface.PokeBattle
{
	/// <summary>
	/// Mega Evolutions and Primal Reversions are treated as form changes in
	/// Essentials. The code below is just more of what's in the Pokemon_MultipleForms
	/// script section, but specifically and only for the Mega Evolution and Primal
	/// Reversion forms.
	/// Extensions of <seealso cref="IPokemon"/>
	/// </summary>
	public interface IPokemonMegaEvolution
	{
		bool hasMegaForm();

		bool isMega();

		void makeMega();

		void makeUnmega();

		string megaName();

		int megaMessage();

		bool hasPrimalForm();

		bool isPrimal();

		void makePrimal();

		void makeUnprimal();
	}
}