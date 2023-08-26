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
    public class DisplayItemObject : ObjectBase
    {
        public static DisplayItemObject Create(object target)
        {
            DisplayItemObject displayItemObject = ReferencePool.Acquire<DisplayItemObject>();
            displayItemObject.Initialize(target);
            return displayItemObject;
        }

        protected override void Release(bool isShutdown)
        {
            DisplayItem displayItem = (DisplayItem)Target;
            if (displayItem == null)
            {
                return;
            }

            Object.Destroy(displayItem.gameObject);
        }
    }
}
