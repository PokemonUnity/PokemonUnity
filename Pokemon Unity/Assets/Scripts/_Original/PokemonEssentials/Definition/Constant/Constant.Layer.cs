//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace PokemonUnity
{
    public static partial class Constant
    {
        /// <summary>
        /// 层。
        /// </summary>
        public static class Layer
        {
            public const string DefaultLayerName = "Default";
            public static readonly int DefaultLayerId = LayerMask.NameToLayer(DefaultLayerName);

            public const string UILayerName = "UI";
            public static readonly int UILayerId = LayerMask.NameToLayer(UILayerName);

            public const string TargetableObjectLayerName = "Targetable Object";
            public static readonly int TargetableObjectLayerId = LayerMask.NameToLayer(TargetableObjectLayerName);
        }
    }
}
