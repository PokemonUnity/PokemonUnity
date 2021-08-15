using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Saving
{
    [System.Serializable]
    public enum SaveEventType
    {
        ITEM,
        BATTLE,
        INTERACTION,
        UNKNOWN
    }
}
