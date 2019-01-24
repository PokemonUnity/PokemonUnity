using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Saving;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Networking.Trading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace PokemonUnity.Networking
{
    public static class NetworkManager
    {
        private const string encryptionKey = "pku123";
        private const string ipAdress = "192.168.0.0";
        private const int port = 1000;

        private static TcpClient tcpClient;

        public static void Start()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(ipAdress);

            // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
            // an exception that occurs when the host IP Address is not compatible with the address family
            // (typical in the IPv6 case).
            foreach (IPAddress adress in hostEntry.AddressList)
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAdress), port);
                TcpClient tempSocket = new TcpClient(endPoint.AddressFamily);

                tempSocket.Connect(endPoint);
                if (tempSocket.Connected)
                {
                    tcpClient = tempSocket;
                    break;
                }
                else
                {
                    continue;
                }
            }
        }

        public static void InitiateTrade(int PlayerID)
        {
            if (!tcpClient.Connected)
            {
                Start();
            }

            //Preparing/Sending message
            using (NetworkStream Stream = tcpClient.GetStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                TradePacket tradePacket = new TradePacket(TradeCommand.INITIATE, PlayerID);

                formatter.Serialize(Stream, tradePacket);
                Stream.Flush();
            }
        }

        public static void SetPokemon(SeriPokemon Pokemon)
        {
            if (!tcpClient.Connected)
            {
                Start();
            }

            //Preparing/Sending message
            using (NetworkStream Stream = tcpClient.GetStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                TradePacket tradePacket = new TradePacket(TradeCommand.SET_POKEMON, Pokemon);

                formatter.Serialize(Stream, tradePacket);
                Stream.Flush();
            }
        }
    }
}
