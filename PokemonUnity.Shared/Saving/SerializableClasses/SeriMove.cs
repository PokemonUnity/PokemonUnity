using PokemonUnity.Attack;

namespace PokemonUnity.Saving.SerializableClasses
{
	[System.Serializable]
	public struct SeriMove
	{
		public int Move { get; private set; }
		public byte PP { get; private set; }

		public int PPups { get; private set; }

		public static implicit operator SeriMove(Move move)
		{
			SeriMove seriMove = new SeriMove
			{
				Move = (int)move.MoveId,
				PP = move.PP,
				PPups = move.PPups
			};
			return seriMove;
		}

		public static implicit operator Move(SeriMove move)
		{
			Move normalMove = new Move((Moves)move.Move, move.PPups, move.PP);
			return normalMove;
		}
	}
}
