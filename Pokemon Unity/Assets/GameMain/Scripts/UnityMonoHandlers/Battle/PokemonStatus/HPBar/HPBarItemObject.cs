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
    public class HPBarItemObject : ObjectBase
    {
        public static HPBarItemObject Create(object target)
        {
            HPBarItemObject hpBarItemObject = ReferencePool.Acquire<HPBarItemObject>();
            hpBarItemObject.Initialize(target);
            return hpBarItemObject;
        }

        protected override void Release(bool isShutdown)
        {
            HPBarItem hpBarItem = (HPBarItem)Target;
            if (hpBarItem == null)
            {
                return;
            }

            Object.Destroy(hpBarItem.gameObject);
        }
    }
}
