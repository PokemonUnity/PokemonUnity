namespace PokemonUnity.Saving.SerializableClasses
{
	[System.Serializable]
	public struct SeriMove
	{
		public int Move { get; private set; }
		public int PP { get; private set; }

		public int PPups { get; private set; }

		public SeriMove(int move, int pp, int ppups)
		{
			Move = move;
			PPups = ppups;
			PP = pp;
		}

		//public static implicit operator SeriMove(PokemonEssentials.Interface.IMove move)
		//{
		//	SeriMove seriMove = new SeriMove
		//	{
		//		Move = (int)move.id,
		//		PP = move.PP,
		//		PPups = move.PPups
		//	};
		//	return seriMove;
		//}

		//public static implicit operator PokemonEssentials.Interface.IMove(SeriMove move)
		//{
		//	PokemonEssentials.Interface.IMove normalMove = new PokemonUnity.Attack.Move((Moves)move.Move, move.PPups, move.PP);
		//	return normalMove;
		//}
	}
}
