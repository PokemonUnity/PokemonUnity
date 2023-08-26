//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace PokemonUnity
{
	public class CommonButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
		private const float FadeTime = 0.3f;
		private const float OnHoverAlpha = 0.7f;
		private const float OnClickAlpha = 0.6f;

		[SerializeField]
		private UnityEvent m_OnHover = null;

		[SerializeField]
		private UnityEvent m_OnClick = null;

		private CanvasGroup m_CanvasGroup = null;

		private void Awake()
		{
			//m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
		}

		private void OnDisable()
		{
			m_CanvasGroup.alpha = 1f;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}

			StopAllCoroutines();
			StartCoroutine(m_CanvasGroup.FadeToAlpha(OnHoverAlpha, FadeTime));
			m_OnHover.Invoke();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}

			StopAllCoroutines();
			StartCoroutine(m_CanvasGroup.FadeToAlpha(1f, FadeTime));
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}

			m_CanvasGroup.alpha = OnClickAlpha;
			m_OnClick.Invoke();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}

			m_CanvasGroup.alpha = OnHoverAlpha;
		}
	}
}
