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
    public class CharEntryComponent : GameFrameworkComponent
    {
        [SerializeField]
        private CharEntryItem m_CharEntryItemTemplate = null;

        [SerializeField]
        private Transform m_CharEntryInstanceRoot = null;

        [SerializeField]
        private int m_InstancePoolCapacity = 16;

        private IObjectPool<CharEntryItemObject> m_CharEntryItemObjectPool = null;
        private List<CharEntryItem> m_ActiveCharEntryItems = null;
        private Canvas m_CachedCanvas = null;

        private void Start()
        {
            if (m_CharEntryInstanceRoot == null)
            {
                Log.Error("You must set HP bar instance root first.");
                return;
            }

            m_CachedCanvas = m_CharEntryInstanceRoot.GetComponent<Canvas>();
            m_CharEntryItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<CharEntryItemObject>("CharEntryItem", m_InstancePoolCapacity);
            m_ActiveCharEntryItems = new List<CharEntryItem>();
        }

        private void OnDestroy()
        {

        }

        private void Update()
        {
            for (int i = m_ActiveCharEntryItems.Count - 1; i >= 0; i--)
            {
                CharEntryItem displayItem = m_ActiveCharEntryItems[i];
                if (displayItem.Refresh())
                {
                    continue;
                }

                HideCharEntry(displayItem);
            }
        }

        public void ShowCharEntry(Entity entity, float fromHPRatio, float toHPRatio)
        {
            if (entity == null)
            {
                Log.Warning("Entity is invalid.");
                return;
            }

            CharEntryItem displayItem = GetActiveCharEntryItem(entity);
            if (displayItem == null)
            {
                displayItem = CreateCharEntryItem(entity);
                m_ActiveCharEntryItems.Add(displayItem);
            }

            displayItem.Init(entity, m_CachedCanvas, fromHPRatio, toHPRatio);
        }

        private void HideCharEntry(CharEntryItem displayItem)
        {
            displayItem.Reset();
            m_ActiveCharEntryItems.Remove(displayItem);
            m_CharEntryItemObjectPool.Unspawn(displayItem);
        }

        private CharEntryItem GetActiveCharEntryItem(Entity entity)
        {
            if (entity == null)
            {
                return null;
            }

            for (int i = 0; i < m_ActiveCharEntryItems.Count; i++)
            {
                if (m_ActiveCharEntryItems[i].Owner == entity)
                {
                    return m_ActiveCharEntryItems[i];
                }
            }

            return null;
        }

        private CharEntryItem CreateCharEntryItem(Entity entity)
        {
            CharEntryItem displayItem = null;
            CharEntryItemObject displayItemObject = m_CharEntryItemObjectPool.Spawn();
            if (displayItemObject != null)
            {
                displayItem = (CharEntryItem)displayItemObject.Target;
            }
            else
            {
                displayItem = Instantiate(m_CharEntryItemTemplate);
                Transform transform = displayItem.GetComponent<Transform>();
                transform.SetParent(m_CharEntryInstanceRoot);
                transform.localScale = Vector3.one;
                m_CharEntryItemObjectPool.Register(CharEntryItemObject.Create(displayItem), true);
            }

            return displayItem;
        }
    }
}
