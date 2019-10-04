using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;

namespace PokemonUnity.Attack
{
    public partial class Move
    {
        public class MoveTarget
        {
            public bool hasMultipleTargets(Move move)
            {
                return move.Targets == Target.AllOpposing || move.Targets == Target.AllNonUsers;
            }
            public bool targetsOneOpponent(Move move)
            {
                return move.Targets == Target.SingleNonUser || move.Targets == Target.RandomOpposing
                    || move.Targets == Target.SingleOpposing || move.Targets == Target.OppositeOpposing;
            }
        }
    }
}