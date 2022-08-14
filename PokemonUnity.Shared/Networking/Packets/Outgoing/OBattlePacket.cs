namespace PokemonUnity.Networking.Packets.Outgoing
{
	[System.Serializable]
	public class OBattlePacket : IOutPacket
	{
		public BattleCommand Command { get; private set; }
		public object Message { get; private set; }

		public OBattlePacket(BattleCommand Command, object Message)
		{
			this.Command = Command;
			this.Message = Message;
		}
	}

	[System.Serializable]
	public enum BattleCommand
	{
		INITIATE,
		SET_POKEMON,
		LOCK_POKEMON,
		CONFIRM_TRADE,
		TRADE_SUCCESS
	}
}