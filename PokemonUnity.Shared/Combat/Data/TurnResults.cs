namespace PokemonUnity.Combat.Data
{
	/// <summary>
	/// </summary>
	public struct Turn //: PokemonEssentials.Interface.Battle.
	{
		public PokemonEssentials.Interface.PokeBattle.IBattleChoice[] Choices { get; private set; }
		public PokemonEssentials.Interface.PokeBattle.ISuccessState[] SuccessStates { get; private set; }
		public PokemonEssentials.Interface.PokeBattle.IDamageState[] DamageStates { get; private set; }
		public int[] FinalDamages { get; private set; }
		public int[] Orders { get; private set; }
	}
}