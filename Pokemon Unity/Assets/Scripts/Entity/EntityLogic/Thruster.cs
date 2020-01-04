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
    /// <summary>
    /// 推进器类。
    /// </summary>
    public class Thruster : Entity
    {
        private const string AttachPoint = "Thruster Point";

        [SerializeField]
        private ThrusterData m_ThrusterData = null;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);

            m_ThrusterData = userData as ThrusterData;
            if (m_ThrusterData == null)
            {
                Log.Error("Thruster data is invalid.");
                return;
            }

            GameEntry.Entity.AttachEntity(this, m_ThrusterData.OwnerId, AttachPoint);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
#else
        protected internal override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
#endif
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            Name = GameFramework.Utility.Text.Format("Thruster of {0}", parentEntity.Name);
            CachedTransform.localPosition = Vector3.zero;
        }
    }
}
