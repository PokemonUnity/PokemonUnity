using System;
using System.Linq;

namespace PokemonUnity
{
	public static class TrainerExtension
	{
		public static bool IsNotNullOrNone(this PokemonEssentials.Interface.PokeBattle.ITrainer trainer)
		{
			return trainer != null || trainer.trainertype != TrainerTypes.WildPokemon;
		}

		public static bool IsNotNullOrNone(this Trainer[] trainer)
		{
			return trainer != null || trainer.All(x => x.trainertype != TrainerTypes.WildPokemon); //|| trainer.ID != TrainerTypes.WildPokemon);
		}

		public static bool IsNotNullOrNone(this PokemonEssentials.Interface.PokeBattle.ITrainer[] trainer)
		{
			return trainer != null || trainer.Any(x => x.trainertype != TrainerTypes.WildPokemon); //|| trainer.ID != TrainerTypes.WildPokemon);
		}
	}
}