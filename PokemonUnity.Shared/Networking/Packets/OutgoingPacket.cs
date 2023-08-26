using PokemonUnity.Networking.Packets.Outgoing;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Saving;

namespace PokemonUnity.Networking.Packets
{
    [System.Serializable]
    public class OutgoingPacket
    {
        public OutgoingPacketType Type;
        public IOutPacket PacketContainer;
        public System.DateTime Time = System.DateTime.Now;

        #region Constructors

        #region Trading
        /// <summary>
        /// Creates a new empty Outgoing Trade Packet with the set TradeCommand
        /// </summary>
        /// <param name="tradeCommand"></param>
        public OutgoingPacket(TradeCommand tradeCommand)
        {
            Type = OutgoingPacketType.TRADE;
            PacketContainer = new OTradePacket(tradeCommand, "");
        }

        /// <summary>
        /// Creates a new Outgoing Trade Packet with the SET_POKEMON Trade Command
        /// </summary>
        /// <param name="pokemonToTrade">The SeriPokemon that needs to be set (visible to the other player)</param>
        public OutgoingPacket(TradeCommand command, SeriPokemon pokemonToTrade)
        {
            Type = OutgoingPacketType.TRADE;
            PacketContainer = new OTradePacket(command, pokemonToTrade);
        }
        #endregion

        #region Authenticating
        /// <summary>
        /// Creates a new Authentication Request Packet
        /// </summary>
        /// <param name="authenticator">The SaveData that needs to be compared to the online version for authentication</param>
        public OutgoingPacket(SaveData authenticator)
        {
            Type = OutgoingPacketType.AUTH;
            PacketContainer = new OAuthenticatePacket(authenticator);
        }
        #endregion

        #endregion
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