//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace PokemonUnity
{
    public class HPBarItem : MonoBehaviour
    {
        private const float AnimationSeconds = 0.3f;
        private const float KeepSeconds = 0.4f;
        private const float FadeOutSeconds = 0.3f;

        [SerializeField]
        private Slider m_HPBar = null;

        private Canvas m_ParentCanvas = null;
        private RectTransform m_CachedTransform = null;
        private CanvasGroup m_CachedCanvasGroup = null;
        private Entity m_Owner = null;
        private int m_OwnerId = 0;

        public Entity Owner
        {
            get
            {
                return m_Owner;
            }
        }

        public void Init(Entity owner, Canvas parentCanvas, float fromHPRatio, float toHPRatio)
        {
            if (owner == null)
            {
                Log.Error("Owner is invalid.");
                return;
            }

            m_ParentCanvas = parentCanvas;

            gameObject.SetActive(true);
            StopAllCoroutines();

            m_CachedCanvasGroup.alpha = 1f;
            if (m_Owner != owner || m_OwnerId != owner.Id)
            {
                m_HPBar.value = fromHPRatio;
                m_Owner = owner;
                m_OwnerId = owner.Id;
            }

            Refresh();

            StartCoroutine(HPBarCo(toHPRatio, AnimationSeconds, KeepSeconds, FadeOutSeconds));
        }

        public bool Refresh()
        {
            if (m_CachedCanvasGroup.alpha <= 0f)
            {
                return false;
            }

            if (m_Owner != null && Owner.Available && Owner.Id == m_OwnerId)
            {
                Vector3 worldPosition = m_Owner.CachedTransform.position + Vector3.forward;
                Vector3 screenPosition = GameEntry.Scene.MainCamera.WorldToScreenPoint(worldPosition);

                Vector2 position;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)m_ParentCanvas.transform, screenPosition,
                    m_ParentCanvas.worldCamera, out position))
                {
                    m_CachedTransform.anchoredPosition = position;
                }
            }

            return true;
        }

        public void Reset()
        {
            StopAllCoroutines();
            m_CachedCanvasGroup.alpha = 1f;
            m_HPBar.value = 1f;
            m_Owner = null;
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            m_CachedTransform = GetComponent<RectTransform>();
            if (m_CachedTransform == null)
            {
                Log.Error("RectTransform is invalid.");
                return;
            }

            m_CachedCanvasGroup = GetComponent<CanvasGroup>();
            if (m_CachedCanvasGroup == null)
            {
                Log.Error("CanvasGroup is invalid.");
                return;
            }
        }

        private IEnumerator HPBarCo(float value, float animationDuration, float keepDuration, float fadeOutDuration)
        {
            yield return m_HPBar.SmoothValue(value, animationDuration);
            yield return new WaitForSeconds(keepDuration);
            yield return m_CachedCanvasGroup.FadeToAlpha(0f, fadeOutDuration);
        }

        /*// <summary>
        /// Invoked when the value of the slider changes.
        /// </summary>
        public void ValueChangeCheck()
        {
            //Debug.Log(hpSlider.value);

            //if (hpSlider.value <= (hpSlider.maxValue / 4)) { Fill.color = hpzone2; }
            if (.3f > (m_HPBar.normalizedValue)) { Fill.color = hpzone2; }
            //else if (hpSlider.value < (hpSlider.normalizedValue.CompareTo(0.5f))) //  / 2)) 
            else if (.75f > (m_HPBar.normalizedValue)) //  / 2)) 
            {
                //Change color of hp bar
                Fill.color = hpzone1;
                //Change background image for health slider
            }
            else
                Fill.color = hpzone0;

            //each time the silder's value is changed, write to text displaying the hp
            //currentHP.text = m_HPBar.value.ToString(); //Set text under hp to match slider currentHealth
        }*/
    }
}
