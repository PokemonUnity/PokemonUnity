//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using GameFramework.ObjectPool;
//using UnityGameFramework.Runtime;

namespace PokemonUnity
{
	public class CharKeyComponent : MonoBehaviour//GameFrameworkComponent
	{
		[SerializeField]
		private CharKeyItem m_CharKeyItemTemplate = null;

		[SerializeField]
		private Transform m_CharKeyInstanceRoot = null;

		[SerializeField]
		private int m_InstancePoolCapacity = 56;

		private IObjectPool<CharKeyItemObject> m_CharKeyItemObjectPool = null;
		private List<CharKeyItem> m_ActiveCharKeyItems = null;
		//private Canvas m_CachedCanvas = null;
		private TypingForm cachedTypingForm = null;

		private void Start()
		{
			if (m_CharKeyInstanceRoot == null)
			{
				//Log.Error("You must set Page for Character Key instance root first.");
				return;
			}

			//m_CachedCanvas = m_CharKeyInstanceRoot.GetComponent<Canvas>();
			cachedTypingForm = m_CharKeyInstanceRoot.GetComponentInParent<TypingForm>();
			//m_CharKeyItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<CharKeyItemObject>("CharKeyItem", m_InstancePoolCapacity);
			m_ActiveCharKeyItems = new List<CharKeyItem>();
		}

		private void OnDestroy()
		{

		}

		private void Update()
		{
			for (int i = m_ActiveCharKeyItems.Count - 1; i >= 0; i--)
			{
				CharKeyItem displayItem = m_ActiveCharKeyItems[i];
				if (displayItem.Refresh())
				{
					continue;
				}

				//HideCharKey(displayItem);
			}
		}

		//public void ShowCharKey(int entity, float fromHPRatio, float toHPRatio)
		public void ShowCharKey(int entity)
		{
			if (entity == null)
			{
				//Log.Warning("Entity is invalid.");
				return;
			}

			CharKeyItem displayItem = GetActiveCharKeyItem(entity);
			if (displayItem == null)
			{
				displayItem = CreateCharKeyItem(entity);
				m_ActiveCharKeyItems.Add(displayItem);
			}

			//displayItem.Init(entity, m_CachedCanvas, fromHPRatio, toHPRatio);
			displayItem.Init(entity, cachedTypingForm);
		}

		private void HideCharKey(CharKeyItem displayItem)
		{
			displayItem.Reset();
			m_ActiveCharKeyItems.Remove(displayItem);
			m_CharKeyItemObjectPool.Unspawn(displayItem);
		}

		public void HideCharKey(int entity)
		{
			if(m_ActiveCharKeyItems.Count <= entity)
				HideCharKey(m_ActiveCharKeyItems[entity]);
		}

		private CharKeyItem GetActiveCharKeyItem(int entity)
		{
			if (entity == null)
			{
				return null;
			}

			for (int i = 0; i < m_ActiveCharKeyItems.Count; i++)
			{
				if (m_ActiveCharKeyItems[i].CharKeyIndex == entity)
				{
					return m_ActiveCharKeyItems[i];
				}
			}

			return null;
		}

		private CharKeyItem CreateCharKeyItem(int entity)
		{
			CharKeyItem displayItem = null;
			CharKeyItemObject displayItemObject = m_CharKeyItemObjectPool.Spawn();
			if (displayItemObject != null)
			{
				displayItem = (CharKeyItem)displayItemObject.Target;
			}
			else
			{
				displayItem = GameObject.Instantiate<CharKeyItem>(m_CharKeyItemTemplate);
				Transform transform = displayItem.GetComponent<Transform>();
				transform.SetParent(m_CharKeyInstanceRoot);
				transform.localScale = Vector3.one;
				m_CharKeyItemObjectPool.Register(CharKeyItemObject.Create(displayItem), true);
			}

			return displayItem;
		}
	}
}
