using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.UX;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Field
{
	public interface IDayCare
	{
		KeyValuePair<IPokemon, int>[] Slot { get; }
		IPokemon this[int index] { get; }

		bool EggGenerated();

		int DayCareDeposited();

		void DayCareDeposit(int index);

		bool DayCareGetLevelGain(int index, int nameVariable, int levelVariable);

		void DayCareGetDeposited(int index, int nameVariable, int costVariable);

		bool IsDitto(IPokemon pokemon);

		bool DayCareCompatibleGender(IPokemon pokemon1, IPokemon pokemon2);

		int DayCareGetCompat();

		void DayCareGetCompatibility(int variable);

		void DayCareWithdraw(int index);

		void DayCareChoose(string text, int variable);

		void DayCareGenerateEgg();

		//void Events_OnStepTaken (object sender, EventArgs e);
		//void Events_OnStepTaken(object sender, EventArg.OnStepTakenFieldMovementEventArgs e);
	}
}