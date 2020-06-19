//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace PokemonUnity
{
	public abstract class Entity : EntityLogic
	{
		[SerializeField]
		private EntityData m_EntityData = null;

		public int Id
		{
			get
			{
				return Entity.Id;
			}
		}

		public Animation CachedAnimation
		{
			get;
			private set;
		}

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			CachedAnimation = GetComponent<Animation>();
		}
		protected override void OnShow(object userData)
		{
			base.OnShow(userData);

			m_EntityData = userData as EntityData;
			if (m_EntityData == null)
			{
				Log.Error("Entity data is invalid.");
				return;
			}

			Name = GameFramework.Utility.Text.Format("[Entity {0}]", Id.ToString());
			CachedTransform.localPosition = m_EntityData.Position;
			CachedTransform.localRotation = m_EntityData.Rotation;
			CachedTransform.localScale = Vector3.one;
		}
		protected override void OnHide(bool isShutdown, object userData)
		{
			base.OnHide(isShutdown, userData);
		}
		protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
		{
			base.OnAttached(childEntity, parentTransform, userData);
		}
		protected override void OnDetached(EntityLogic childEntity, object userData)
		{
			base.OnDetached(childEntity, userData);
		}
		protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
		{
			base.OnAttachTo(parentEntity, parentTransform, userData);
		}
		protected override void OnDetachFrom(EntityLogic parentEntity, object userData)
		{
			base.OnDetachFrom(parentEntity, userData);
		}
		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(elapseSeconds, realElapseSeconds);
		}
	}
}
