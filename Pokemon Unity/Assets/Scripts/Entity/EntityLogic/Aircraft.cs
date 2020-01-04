//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace PokemonUnity
{
    /// <summary>
    /// 战机类。
    /// </summary>
    public abstract class Aircraft : TargetableObject
    {
        [SerializeField]
        private AircraftData m_AircraftData = null;

        [SerializeField]
        protected Thruster m_Thruster = null;

        [SerializeField]
        protected List<Weapon> m_Weapons = new List<Weapon>();

        [SerializeField]
        protected List<Armor> m_Armors = new List<Armor>();

#if UNITY_2017_3_OR_NEWER
        protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);

            m_AircraftData = userData as AircraftData;
            if (m_AircraftData == null)
            {
                Log.Error("Aircraft data is invalid.");
                return;
            }

            Name = GameFramework.Utility.Text.Format("Aircraft ({0})", Id.ToString());

            GameEntry.Entity.ShowThruster(m_AircraftData.GetThrusterData());

            List<WeaponData> weaponDatas = m_AircraftData.GetAllWeaponDatas();
            for (int i = 0; i < weaponDatas.Count; i++)
            {
                GameEntry.Entity.ShowWeapon(weaponDatas[i]);
            }

            List<ArmorData> armorDatas = m_AircraftData.GetAllArmorDatas();
            for (int i = 0; i < armorDatas.Count; i++)
            {
                GameEntry.Entity.ShowArmor(armorDatas[i]);
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnHide(bool isShutdown, object userData)
#else
        protected internal override void OnHide(bool isShutdown, object userData)
#endif
        {
            base.OnHide(isShutdown, userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
#else
        protected internal override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
#endif
        {
            base.OnAttached(childEntity, parentTransform, userData);

            if (childEntity is Thruster)
            {
                m_Thruster = (Thruster)childEntity;
                return;
            }

            if (childEntity is Weapon)
            {
                m_Weapons.Add((Weapon)childEntity);
                return;
            }

            if (childEntity is Armor)
            {
                m_Armors.Add((Armor)childEntity);
                return;
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnDetached(EntityLogic childEntity, object userData)
#else
        protected internal override void OnDetached(EntityLogic childEntity, object userData)
#endif
        {
            base.OnDetached(childEntity, userData);

            if (childEntity is Thruster)
            {
                m_Thruster = null;
                return;
            }

            if (childEntity is Weapon)
            {
                m_Weapons.Remove((Weapon)childEntity);
                return;
            }

            if (childEntity is Armor)
            {
                m_Armors.Remove((Armor)childEntity);
                return;
            }
        }

        protected override void OnDead(Entity attacker)
        {
            base.OnDead(attacker);

            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), m_AircraftData.DeadEffectId)
            {
                Position = CachedTransform.localPosition,
            });
            GameEntry.Sound.PlaySound(m_AircraftData.DeadSoundId);
        }

        public override ImpactData GetImpactData()
        {
            return new ImpactData(m_AircraftData.Camp, m_AircraftData.HP, 0, m_AircraftData.Defense);
        }
    }
}
