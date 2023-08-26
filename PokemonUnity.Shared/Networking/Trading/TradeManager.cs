using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Networking.Packets;

namespace PokemonUnity.Networking
{
    public static class TradeManager
    {
        public static void ReceivePacket(/*ITradePacket incomingPacket*/)
        {
            //Perform action with data received
        }

        public static void InitiateTrade()
        {
            OutgoingPacket iniateTrade = new OutgoingPacket(Packets.Outgoing.TradeCommand.INITIATE);
            NetworkManager.Send(iniateTrade);
        }

        public static void SetPokemon(SeriPokemon pokemonToSet)
        {
            OutgoingPacket setPokemon = new OutgoingPacket(Packets.Outgoing.TradeCommand.SET_POKEMON, pokemonToSet);
            NetworkManager.Send(setPokemon);
        }

        public static void LockPokemon(SeriPokemon pokemonToLock)
        {
            OutgoingPacket lockPokemon = new OutgoingPacket(Packets.Outgoing.TradeCommand.LOCK_POKEMON, pokemonToLock);
            NetworkManager.Send(lockPokemon);
        }
    }
}