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

		bool pbEggGenerated();

		int pbDayCareDeposited();

		void pbDayCareDeposit(int index);

		bool pbDayCareGetLevelGain(int index, int nameVariable, int levelVariable);

		void pbDayCareGetDeposited(int index, int nameVariable, int costVariable);

		bool pbIsDitto(IPokemon pokemon);

		bool pbDayCareCompatibleGender(IPokemon pokemon1, IPokemon pokemon2);

		int pbDayCareGetCompat();

		void pbDayCareGetCompatibility(int variable);

		void pbDayCareWithdraw(int index);

		void pbDayCareChoose(string text, int variable);

		void pbDayCareGenerateEgg();

		//void Events_OnStepTaken (object sender, EventArgs e);
	}
}