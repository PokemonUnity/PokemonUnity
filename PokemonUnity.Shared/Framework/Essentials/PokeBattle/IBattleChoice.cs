namespace PokemonEssentials.Interface.PokeBattle
{
	public interface IBattleChoice
	{
		PokemonUnity.Combat.ChoiceAction Action { get; }
		/// <summary>
		/// Index of Action being used
		/// </summary>
		int Index { get; }
		IMove Move { get; }
		int Target { get; }
	}
}