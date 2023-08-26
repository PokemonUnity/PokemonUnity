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
	public class HPBarComponent //: GameFrameworkComponent
	{
		[SerializeField]
		private HPBarItem m_HPBarItemTemplate = null;

		[SerializeField]
		private Transform m_HPBarInstanceRoot = null;

		[SerializeField]
		private int m_InstancePoolCapacity = 16;

		private IObjectPool<HPBarItemObject> m_HPBarItemObjectPool = null;
		private List<HPBarItem> m_ActiveHPBarItems = null;
		private Canvas m_CachedCanvas = null;

		private void Start()
		{
			if (m_HPBarInstanceRoot == null)
			{
				//Log.Error("You must set HP bar instance root first.");
				return;
			}

			m_CachedCanvas = m_HPBarInstanceRoot.GetComponent<Canvas>();
			//m_HPBarItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<HPBarItemObject>("HPBarItem", m_InstancePoolCapacity);
			m_ActiveHPBarItems = new List<HPBarItem>();
		}

		private void OnDestroy()
		{

		}

		private void Update()
		{
			for (int i = m_ActiveHPBarItems.Count - 1; i >= 0; i--)
			{
				HPBarItem hpBarItem = m_ActiveHPBarItems[i];
				if (hpBarItem.Refresh())
				{
					continue;
				}

				HideHPBar(hpBarItem);
			}
		}

		public void ShowHPBar(Entity entity, float fromHPRatio, float toHPRatio)
		{
			if (entity == null)
			{
				//Log.Warning("Entity is invalid.");
				return;
			}

			HPBarItem hpBarItem = GetActiveHPBarItem(entity);
			if (hpBarItem == null)
			{
				hpBarItem = CreateHPBarItem(entity);
				m_ActiveHPBarItems.Add(hpBarItem);
			}

			hpBarItem.Init(entity, m_CachedCanvas, fromHPRatio, toHPRatio);
		}

		private void HideHPBar(HPBarItem hpBarItem)
		{
			hpBarItem.Reset();
			m_ActiveHPBarItems.Remove(hpBarItem);
			m_HPBarItemObjectPool.Unspawn(hpBarItem);
		}

		private HPBarItem GetActiveHPBarItem(Entity entity)
		{
			if (entity == null)
			{
				return null;
			}

			for (int i = 0; i < m_ActiveHPBarItems.Count; i++)
			{
				if (m_ActiveHPBarItems[i].Owner == entity)
				{
					return m_ActiveHPBarItems[i];
				}
			}

			return null;
		}

		private HPBarItem CreateHPBarItem(Entity entity)
		{
			HPBarItem hpBarItem = null;
			HPBarItemObject hpBarItemObject = m_HPBarItemObjectPool.Spawn();
			if (hpBarItemObject != null)
			{
				hpBarItem = (HPBarItem)hpBarItemObject.Target;
			}
			else
			{
				hpBarItem = GameObject.Instantiate<HPBarItem>(m_HPBarItemTemplate);
				Transform transform = hpBarItem.GetComponent<Transform>();
				transform.SetParent(m_HPBarInstanceRoot);
				transform.localScale = Vector3.one;
				m_HPBarItemObjectPool.Register(HPBarItemObject.Create(hpBarItem), true);
			}

			return hpBarItem;
		}
	}
}
