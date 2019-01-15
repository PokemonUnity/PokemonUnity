using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Saving.SerializableClasses
{
    using PokemonUnity.Attack;
    public class SeriMove
    {
        public static implicit operator SeriMove(Move move)
        {
            SeriMove seriMove = new SeriMove();
            return seriMove;
        }

        public static implicit operator Move(SeriMove move)
        {
            Move normalMove = new Move();
            return normalMove;
        }
    }
}
