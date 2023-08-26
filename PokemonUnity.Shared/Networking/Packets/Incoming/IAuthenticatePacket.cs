namespace PokemonUnity.Networking.Packets.Incoming
{
    [System.Serializable]
    public class IAuthenticatePacket : IInPacket
    {
        public AuthOptions Authenticated { get; private set; }
        public string AuthenticationKey { get; private set; }

        public enum AuthOptions
        {
            SUCCES,
            FAILED,
            ERROR
        }
    }
}