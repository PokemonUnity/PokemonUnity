using System;
using System.Linq;

namespace PokemonUnity
{
	public static class TrainerExtension
	{
		public static bool IsNotNullOrNone(this PokemonUnity.Character.TrainerData trainer)
		{
			return trainer != null || trainer.ID != TrainerTypes.WildPokemon;
		}

		//public static bool IsNotNullOrNone(this PokemonUnity.Combat.Trainer[] trainer)
		//{
		//	return trainer != null || trainer.All(x => x.trainertype != TrainerTypes.WildPokemon); //|| trainer.ID != TrainerTypes.WildPokemon);
		//}

		public static bool IsNotNullOrNone(this PokemonEssentials.Interface.PokeBattle.ITrainer[] trainer)
		{
			return trainer != null || trainer.All(x => x.trainertype != TrainerTypes.WildPokemon); //|| trainer.ID != TrainerTypes.WildPokemon);
		}
	}
}