//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
//using UnityGameFramework.Runtime;

namespace PokemonUnity
{
	public class ExpBarComponent //: GameFrameworkComponent
	{
		[SerializeField]
		private ExpBarItem m_ExpBarItemTemplate = null;

		[SerializeField]
		private Transform m_ExpBarInstanceRoot = null;

		[SerializeField]
		private int m_InstancePoolCapacity = 16;

		private IObjectPool<ExpBarItemObject> m_ExpBarItemObjectPool = null;
		private List<ExpBarItem> m_ActiveExpBarItems = null;
		private Canvas m_CachedCanvas = null;

		private void Start()
		{
			if (m_ExpBarInstanceRoot == null)
			{
				//Log.Error("You must set HP bar instance root first.");
				return;
			}

			m_CachedCanvas = m_ExpBarInstanceRoot.GetComponent<Canvas>();
			//m_ExpBarItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<ExpBarItemObject>("ExpBarItem", m_InstancePoolCapacity);
			m_ActiveExpBarItems = new List<ExpBarItem>();
		}

		private void OnDestroy()
		{

		}

		private void Update()
		{
			for (int i = m_ActiveExpBarItems.Count - 1; i >= 0; i--)
			{
				ExpBarItem expBarItem = m_ActiveExpBarItems[i];
				if (expBarItem.Refresh())
				{
					continue;
				}

				HideExpBar(expBarItem);
			}
		}

		public void ShowExpBar(Entity entity, float fromHPRatio, float toHPRatio)
		{
			if (entity == null)
			{
				//Log.Warning("Entity is invalid.");
				return;
			}

			ExpBarItem expBarItem = GetActiveExpBarItem(entity);
			if (expBarItem == null)
			{
				expBarItem = CreateExpBarItem(entity);
				m_ActiveExpBarItems.Add(expBarItem);
			}

			expBarItem.Init(entity, m_CachedCanvas, fromHPRatio, toHPRatio);
		}

		private void HideExpBar(ExpBarItem expBarItem)
		{
			expBarItem.Reset();
			m_ActiveExpBarItems.Remove(expBarItem);
			m_ExpBarItemObjectPool.Unspawn(expBarItem);
		}

		private ExpBarItem GetActiveExpBarItem(Entity entity)
		{
			if (entity == null)
			{
				return null;
			}

			for (int i = 0; i < m_ActiveExpBarItems.Count; i++)
			{
				if (m_ActiveExpBarItems[i].Owner == entity)
				{
					return m_ActiveExpBarItems[i];
				}
			}

			return null;
		}

		private ExpBarItem CreateExpBarItem(Entity entity)
		{
			ExpBarItem expBarItem = null;
			ExpBarItemObject expBarItemObject = m_ExpBarItemObjectPool.Spawn();
			if (expBarItemObject != null)
			{
				expBarItem = (ExpBarItem)expBarItemObject.Target;
			}
			else
			{
				expBarItem = GameObject.Instantiate<ExpBarItem>(m_ExpBarItemTemplate);
				Transform transform = expBarItem.GetComponent<Transform>();
				transform.SetParent(m_ExpBarInstanceRoot);
				transform.localScale = Vector3.one;
				m_ExpBarItemObjectPool.Register(ExpBarItemObject.Create(expBarItem), true);
			}

			return expBarItem;
		}
	}
}
