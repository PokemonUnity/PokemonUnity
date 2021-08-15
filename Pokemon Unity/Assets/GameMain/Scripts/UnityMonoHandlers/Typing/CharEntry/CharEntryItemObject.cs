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
    public class CharEntryItemObject : ObjectBase
    {
        public static CharEntryItemObject Create(object target)
        {
            CharEntryItemObject displayItemObject = ReferencePool.Acquire<CharEntryItemObject>();
            displayItemObject.Initialize(target);
            return displayItemObject;
        }

        protected override void Release(bool isShutdown)
        {
            CharEntryItem displayItem = (CharEntryItem)Target;
            if (displayItem == null)
            {
                return;
            }

            Object.Destroy(displayItem.gameObject);
        }
    }
}
