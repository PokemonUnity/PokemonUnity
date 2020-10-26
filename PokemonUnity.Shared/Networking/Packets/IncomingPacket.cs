namespace PokemonUnity.Networking.Packets
{
    [System.Serializable]
    public class IncomingPacket
    {
        public IncomingPacketType Type;
        public IInPacket PacketContainer;
    }

    public interface IInPacket { };

    [System.Serializable]
    public enum IncomingPacketType
    {
        TRADE,
        BATTLE,
        AUTH
    }
}