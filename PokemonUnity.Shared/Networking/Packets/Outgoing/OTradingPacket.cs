namespace PokemonUnity.Networking.Packets.Outgoing
{
    [System.Serializable]
    public class OTradePacket : IOutPacket
    {
        public TradeCommand Command { get; private set; }
        public object Message { get; private set; }

        public OTradePacket(TradeCommand Command, object Message)
        {
            this.Command = Command;
            this.Message = Message;
        }
    }

    [System.Serializable]
    public enum TradeCommand
    {
        INITIATE,
        SET_POKEMON,
        LOCK_POKEMON,
        CONFIRM_TRADE,
        TRADE_SUCCES
    }
}