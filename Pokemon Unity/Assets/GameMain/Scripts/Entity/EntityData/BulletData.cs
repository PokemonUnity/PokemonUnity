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
    public class BulletData : EntityData
    {
        [SerializeField]
        private int m_OwnerId = 0;

        [SerializeField]
        private CampType m_OwnerCamp = CampType.Unknown;

        [SerializeField]
        private int m_Attack = 0;

        [SerializeField]
        private float m_Speed = 0f;

        public BulletData(int entityId, int typeId, int ownerId, CampType ownerCamp, int attack, float speed)
            : base(entityId, typeId)
        {
            m_OwnerId = ownerId;
            m_OwnerCamp = ownerCamp;
            m_Attack = attack;
            m_Speed = speed;
        }

        public int OwnerId
        {
            get
            {
                return m_OwnerId;
            }
        }

        public CampType OwnerCamp
        {
            get
            {
                return m_OwnerCamp;
            }
        }

        public int Attack
        {
            get
            {
                return m_Attack;
            }
        }

        public float Speed
        {
            get
            {
                return m_Speed;
            }
        }
    }
}
