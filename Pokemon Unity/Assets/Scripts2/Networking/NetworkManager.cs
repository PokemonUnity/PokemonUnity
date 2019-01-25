using System.Net;
using System.Net.Sockets;

namespace PokemonUnity.Networking
{
    public static class NetworkManager
    {
        private const string encryptionKey = "pku123";
        private const string ipAdress = "192.168.0.0";
        private const int port = 1000;

        private static TcpClient tcpClient;
        public static bool IsRunning = false;

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
                    IsRunning = true;
                    break;
                }
                else
                {
                    continue;
                }
            }
        }

        public static void Disconnect()
        {
            if (tcpClient.Connected)
            {
                tcpClient.Close();
                tcpClient = null;
            }
        }

        public static TcpClient GetConnection()
        {
            return tcpClient;
        }
    }
}
