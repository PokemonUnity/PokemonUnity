namespace PokemonUnity.Combat.Data
{
	/// <summary>
	/// </summary>
	public struct Turn
	{
		public Choice[] Choices { get; private set; }
		public SuccessState[] SuccessStates { get; private set; }
		public DamageState[] DamageStates { get; private set; }
		public int[] FinalDamages { get; private set; }
		public int[] Orders { get; private set; }
	}
}