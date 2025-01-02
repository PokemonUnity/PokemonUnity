//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using ProtoBuf;
using System;

namespace PokemonUnity
{
    [Serializable, ProtoContract(Name = @"SCHeartBeat")]
    public class SCHeartBeat : SCPacketBase
    {
        public SCHeartBeat()
        {

        }

        public override int Id
        {
            get
            {
                return 2;
            }
        }

        public override void Clear()
        {

        }
    }
}
