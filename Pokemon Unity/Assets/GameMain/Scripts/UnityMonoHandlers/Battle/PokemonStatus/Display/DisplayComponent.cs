//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace PokemonUnity
{
    public class DisplayComponent : GameFrameworkComponent
    {
        [SerializeField]
        private DisplayItem m_DisplayItemTemplate = null;

        [SerializeField]
        private Transform m_DisplayInstanceRoot = null;

        [SerializeField]
        private int m_InstancePoolCapacity = 16;

        private IObjectPool<DisplayItemObject> m_DisplayItemObjectPool = null;
        private List<DisplayItem> m_ActiveDisplayItems = null;
        private Canvas m_CachedCanvas = null;

        private void Start()
        {
            if (m_DisplayInstanceRoot == null)
            {
                Log.Error("You must set HP bar instance root first.");
                return;
            }

            m_CachedCanvas = m_DisplayInstanceRoot.GetComponent<Canvas>();
            m_DisplayItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<DisplayItemObject>("DisplayItem", m_InstancePoolCapacity);
            m_ActiveDisplayItems = new List<DisplayItem>();
        }

        private void OnDestroy()
        {

        }

        private void Update()
        {
            for (int i = m_ActiveDisplayItems.Count - 1; i >= 0; i--)
            {
                DisplayItem displayItem = m_ActiveDisplayItems[i];
                if (displayItem.Refresh())
                {
                    continue;
                }

                HideDisplay(displayItem);
            }
        }

        public void ShowDisplay(Entity entity, float fromHPRatio, float toHPRatio)
        {
            if (entity == null)
            {
                Log.Warning("Entity is invalid.");
                return;
            }

            DisplayItem displayItem = GetActiveDisplayItem(entity);
            if (displayItem == null)
            {
                displayItem = CreateDisplayItem(entity);
                m_ActiveDisplayItems.Add(displayItem);
            }

            displayItem.Init(entity, m_CachedCanvas, fromHPRatio, toHPRatio);
        }

        private void HideDisplay(DisplayItem displayItem)
        {
            displayItem.Reset();
            m_ActiveDisplayItems.Remove(displayItem);
            m_DisplayItemObjectPool.Unspawn(displayItem);
        }

        private DisplayItem GetActiveDisplayItem(Entity entity)
        {
            if (entity == null)
            {
                return null;
            }

            for (int i = 0; i < m_ActiveDisplayItems.Count; i++)
            {
                if (m_ActiveDisplayItems[i].Owner == entity)
                {
                    return m_ActiveDisplayItems[i];
                }
            }

            return null;
        }

        private DisplayItem CreateDisplayItem(Entity entity)
        {
            DisplayItem displayItem = null;
            DisplayItemObject displayItemObject = m_DisplayItemObjectPool.Spawn();
            if (displayItemObject != null)
            {
                displayItem = (DisplayItem)displayItemObject.Target;
            }
            else
            {
                displayItem = Instantiate(m_DisplayItemTemplate);
                Transform transform = displayItem.GetComponent<Transform>();
                transform.SetParent(m_DisplayInstanceRoot);
                transform.localScale = Vector3.one;
                m_DisplayItemObjectPool.Register(DisplayItemObject.Create(displayItem), true);
            }

            return displayItem;
        }
    }
}
