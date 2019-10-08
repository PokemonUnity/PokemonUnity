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
    public class MyAircraftData : AircraftData
    {
        [SerializeField]
        private string m_Name = null;

        public MyAircraftData(int entityId, int typeId)
            : base(entityId, typeId, CampType.Player)
        {

        }

        /// <summary>
        /// 角色名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }
    }
}
