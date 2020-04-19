using System.Collections;

namespace PokemonUnity
{
	public static class TrainerExtension
	{
		public static bool IsNotNullOrNone(this PokemonUnity.Character.Trainer trainer)
		{
			return trainer != null || trainer.ID != TrainerTypes.WildPokemon;
		}
	}
}