//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using UnityEngine;

namespace PokemonUnity
{
    [Serializable]
    public class EffectData : EntityData
    {
        [SerializeField]
        private float m_KeepTime = 0f;

        public EffectData(int entityId, int typeId)
            : base(entityId, typeId)
        {
            m_KeepTime = 3f;
        }

        public float KeepTime
        {
            get
            {
                return m_KeepTime;
            }
        }
    }
}
