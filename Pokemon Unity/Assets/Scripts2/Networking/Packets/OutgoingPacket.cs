namespace PokemonUnity.Networking.Packets
{
    [System.Serializable]
    public class OutgoingPacket
    {
        public OutgoingPacketType Type;
        public IOutPacket PacketContainer;
    }

    public interface IOutPacket { };

    [System.Serializable]
    public enum OutgoingPacketType
    {
        TRADE,
        BATTLE,
        AUTH
    }
}
