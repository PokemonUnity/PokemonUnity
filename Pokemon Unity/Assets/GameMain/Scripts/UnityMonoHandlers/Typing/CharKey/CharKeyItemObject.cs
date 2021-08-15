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
    public class CharKeyItemObject : ObjectBase
    {
        public static CharKeyItemObject Create(object target)
        {
            CharKeyItemObject displayItemObject = ReferencePool.Acquire<CharKeyItemObject>();
            displayItemObject.Initialize(target);
            return displayItemObject;
        }

        protected override void Release(bool isShutdown)
        {
            CharKeyItem displayItem = (CharKeyItem)Target;
            if (displayItem == null)
            {
                return;
            }

            Object.Destroy(displayItem.gameObject);
        }
    }
}
