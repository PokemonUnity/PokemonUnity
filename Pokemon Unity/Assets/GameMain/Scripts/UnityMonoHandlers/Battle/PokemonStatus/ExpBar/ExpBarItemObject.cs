//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace PokemonUnity
{
    public class ExpBarItemObject : ObjectBase
    {
        public static ExpBarItemObject Create(object target)
        {
            ExpBarItemObject expBarItemObject = ReferencePool.Acquire<ExpBarItemObject>();
            expBarItemObject.Initialize(target);
            return expBarItemObject;
        }

        protected override void Release(bool isShutdown)
        {
            ExpBarItem expBarItem = (ExpBarItem)Target;
            if (expBarItem == null)
            {
                return;
            }

            Object.Destroy(expBarItem.gameObject);
        }
    }
}
