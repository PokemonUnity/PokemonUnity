//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace PokemonUnity
{
    /// <summary>
    /// 阵营类型。
    /// </summary>
    public enum CampType
    {
        Unknown = 0,

        /// <summary>
        /// 第一玩家阵营。
        /// </summary>
        Player,

        /// <summary>
        /// 第一敌人阵营。
        /// </summary>
        Enemy,

        /// <summary>
        /// 第一中立阵营。
        /// </summary>
        Neutral,

        /// <summary>
        /// 第二玩家阵营。
        /// </summary>
        Player2,

        /// <summary>
        /// 第二敌人阵营。
        /// </summary>
        Enemy2,

        /// <summary>
        /// 第二中立阵营
        /// </summary>
        Neutral2,
    }
}
