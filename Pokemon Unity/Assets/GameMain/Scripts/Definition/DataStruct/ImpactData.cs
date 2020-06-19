//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace PokemonUnity
{
    public struct ImpactData
    {
        private readonly CampType m_Camp;
        private readonly int m_HP;
        private readonly int m_Attack;
        private readonly int m_Defense;

        public ImpactData(CampType camp, int hp, int attack, int defense)
        {
            m_Camp = camp;
            m_HP = hp;
            m_Attack = attack;
            m_Defense = defense;
        }

        public CampType Camp
        {
            get
            {
                return m_Camp;
            }
        }

        public int HP
        {
            get
            {
                return m_HP;
            }
        }

        public int Attack
        {
            get
            {
                return m_Attack;
            }
        }

        public int Defense
        {
            get
            {
                return m_Defense;
            }
        }
    }
}
