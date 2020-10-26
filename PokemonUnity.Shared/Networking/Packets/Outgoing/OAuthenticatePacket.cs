using PokemonUnity.Saving;

namespace PokemonUnity.Networking.Packets.Outgoing
{
    [System.Serializable]
    public class OAuthenticatePacket : IOutPacket
    {
        public SaveData SaveData { get; private set; }

        public OAuthenticatePacket(SaveData saveData)
        {
            SaveData = saveData;
        }
    }
}