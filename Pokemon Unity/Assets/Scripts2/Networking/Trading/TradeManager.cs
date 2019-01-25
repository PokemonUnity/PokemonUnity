using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using PokemonUnity.Saving.SerializableClasses;

namespace PokemonUnity.Networking
{
    public static class TradeManager
    {
        public static void InitiateTrade(int PlayerID)
        {
            if (!NetworkManager.IsRunning)
            {
                NetworkManager.Start();
            }

            //Preparing/Sending message
            using (NetworkStream Stream = NetworkManager.GetConnection().GetStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                TradePacket tradePacket = new TradePacket(TradeCommand.INITIATE, PlayerID);

                formatter.Serialize(Stream, tradePacket);
                Stream.Flush();
            }
        }

        public static void SetPokemon(SeriPokemon Pokemon)
        {
            if (!NetworkManager.IsRunning)
            {
                NetworkManager.Start();
            }

            //Preparing/Sending message
            using (NetworkStream Stream = NetworkManager.GetConnection().GetStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                TradePacket tradePacket = new TradePacket(TradeCommand.SET_POKEMON, Pokemon);

                formatter.Serialize(Stream, tradePacket);
                Stream.Flush();
            }
        }

        public static void ConfirmPokemon()
        {
            if (!NetworkManager.IsRunning)
            {
                NetworkManager.Start();
            }

            //Preparing/Sending message
            using (NetworkStream Stream = NetworkManager.GetConnection().GetStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                TradePacket tradePacket = new TradePacket(TradeCommand.CONFIRM_TRADE, "");

                formatter.Serialize(Stream, tradePacket);
                Stream.Flush();
            }
        }
    }
}
