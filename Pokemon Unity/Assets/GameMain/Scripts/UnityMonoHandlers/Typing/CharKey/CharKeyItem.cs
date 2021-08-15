using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace PokemonUnity
{
	[ExecuteInEditMode]
	public class CharKeyItem : CommonButton
	{
		//private const float AnimationSeconds = 0.3f;
		//private const float KeepSeconds = 0.4f;
		//private const float FadeOutSeconds = 0.3f;

		[SerializeField]
		private Image charKeySelectable = null;
		[SerializeField]
		private Text charKeyText = null;
		[SerializeField]
		private int charKeyIndex = 0;
		public int CharKeyIndex
		{
			get { return charKeyIndex; }
			set { charKeyIndex = value; }
		}

		//private Slider m_CharKey = null;
		//private Canvas m_ParentCanvas = null;
		private TypingForm parentForm = null;
		//private RectTransform m_CachedTransform = null;
		//private CanvasGroup m_CachedCanvasGroup = null;
		/*private Entity m_Owner = null;
		private int m_OwnerId = 0;

		public Entity Owner
		{
			get
			{
				return m_Owner;
			}
		}*/

		//public void Init(Entity owner, Canvas parentCanvas, float fromHPRatio, float toHPRatio)
		public void Init(int index, TypingForm parentform)
		{
			//if (owner == null)
			if (parentform == null)
			{
				Log.Error("Owner is invalid.");
				return;
			}

			//m_ParentCanvas = parentCanvas;
			parentForm = parentform;

			gameObject.SetActive(true);
			StopAllCoroutines();

			/*m_CachedCanvasGroup.alpha = 1f;
			if (m_Owner != owner || m_OwnerId != owner.Id)
			{
				m_CharKey.value = fromHPRatio;
				m_Owner = owner;
				m_OwnerId = owner.Id;
			}*/
			charKeyIndex = index;

			Refresh();

			//StartCoroutine(CharKeyCo(toHPRatio, AnimationSeconds, KeepSeconds, FadeOutSeconds));
		}

		public bool Refresh()
		{
			/*if (m_CachedCanvasGroup.alpha <= 0f)
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
			}*/

			charKeyText.text = TypingForm.PageCharArray[parentForm.pageIndex][CharKeyIndex].ToString();

			return true;
		}

		public void Reset()
		{
			StopAllCoroutines();
			//m_CachedCanvasGroup.alpha = 1f;
			//m_CharKey.value = 1f;
			//m_Owner = null;
			gameObject.SetActive(false);
		}

		private void Awake()
		{
			//m_CachedTransform = GetComponent<RectTransform>();
			//if (m_CachedTransform == null)
			//{
			//	Log.Error("RectTransform is invalid.");
			//	return;
			//}

			//m_CachedCanvasGroup = GetComponent<CanvasGroup>();
			//if (m_CachedCanvasGroup == null)
			//{
			//	Log.Error("CanvasGroup is invalid.");
			//	return;
			//}

			charKeyText = GetComponent<Text>();
			if (charKeyText == null)
			{
				Log.Error("Text component is invalid.");
				return;
			}

			charKeySelectable = GetComponentInChildren<Image>(true);
			if (charKeySelectable == null)
			{
				Log.Error("Selectable component is invalid.");
				return;
			}
		}

		//private IEnumerator CharKeyCo(float value, float animationDuration, float keepDuration, float fadeOutDuration)
		//{
		//	yield return m_CharKey.SmoothValue(value, animationDuration);
		//	yield return new WaitForSeconds(keepDuration);
		//	yield return m_CachedCanvasGroup.FadeToAlpha(0f, fadeOutDuration);
		//}

		public void OnPointerEnter(PointerEventData eventData)
		{
			//if (eventData.button != PointerEventData.InputButton.Left)
			//{
			//	return;
			//}

			//StopAllCoroutines();
			//StartCoroutine(m_CanvasGroup.FadeToAlpha(OnHoverAlpha, FadeTime));
			//m_OnHover.Invoke();
			if (eventData.pointerEnter)
				charKeySelectable.enabled = true;
		}
		
		public void OnPointerExit(PointerEventData eventData)
		{
			//if (eventData.button != PointerEventData.InputButton.Left)
			//{
			//	return;
			//}

			//StopAllCoroutines();
			//StartCoroutine(m_CanvasGroup.FadeToAlpha(OnHoverAlpha, FadeTime));
			//m_OnHover.Invoke();
			if (!eventData.pointerEnter)
				charKeySelectable.enabled = false;
		}
	}
}
