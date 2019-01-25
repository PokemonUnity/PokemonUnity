using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Networking
{
    [System.Serializable]
    public class TradePacket
    {
        public TradeCommand Command { get; private set; }
        public object Message { get; private set; }

        public TradePacket(TradeCommand Command, object Message)
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
