using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Character;
using PokemonUnity.Monster;
using UnityEngine;
using UnityEngine.UI;

namespace PokemonUnity.Utility
{
	/// <summary>
	/// Example script showcasing tweening UI elements with LeanTween in Unity
	/// </summary>
	/// https://www.youtube.com/watch?v=Ll3yujn9GVQ
	public class TweenerUI : MonoBehaviour
	{
		#region Variables
		public GameObject objectToAnimate;

		public UIAnimationTypes animationType;
		public LeanTweenType easeType;
		public float duration;
		public float delay;

		public bool loop;
		public bool pingpong;

		public bool startPositionOffset;
		public Vector3 from;
		public Vector3 to;

		private LTDescr _tweenObject;

		public bool showOnEnabled;
		public bool workOnDisabled;
		#endregion

		#region Unity Monobehavior
		void Awake()
		{
			if (objectToAnimate == null)
				objectToAnimate = gameObject;
		}
		void Start()
		{
		}
		void OnEnable()
		{
			if (showOnEnabled)
			{
				Show();
			}
		}
		#endregion

		#region Methods
		public void Show()
		{
			HandleTween();
		}
		public void HandleTween()
		{
			if (objectToAnimate == null)
			{
				objectToAnimate = gameObject;
			}

			switch (animationType)
			{
				case UIAnimationTypes.Fade:
					Fade();
					break;
				case UIAnimationTypes.Move:
					MoveAbsolute();
					break;
				case UIAnimationTypes.Scale:
				case UIAnimationTypes.ScaleX:
				case UIAnimationTypes.ScaleY:
					Scale();
					break;
				default:
					break;
			}

			_tweenObject.setDelay(delay);
			_tweenObject.setEase(easeType);

			if (loop)
			{
				_tweenObject.loopCount = int.MaxValue;
			}
			if (pingpong)
			{
				_tweenObject.setLoopPingPong();
			}
		}
		public void Fade()
		{
			if (gameObject.GetComponent<CanvasGroup>() == null)
			{
				gameObject.AddComponent<CanvasGroup>();
			}
			if (startPositionOffset)
			{
				objectToAnimate.GetComponent<CanvasGroup>().alpha = from.x;
			}
			_tweenObject = LeanTween.alphaCanvas(objectToAnimate.GetComponent<CanvasGroup>(), to.x, duration);
		}
		public void MoveAbsolute()
		{
			objectToAnimate.GetComponent<RectTransform>().anchoredPosition = from;

			_tweenObject = LeanTween.move(objectToAnimate.GetComponent<RectTransform>(), to, duration);
		}
		public void Scale()
		{
			if (startPositionOffset)
			{
				objectToAnimate.GetComponent<RectTransform>().localScale = from;
			}

			//_tweenObject = LeanTween.scale(objectToAnimate.GetComponent<RectTransform>(), to, duration);
			_tweenObject = LeanTween.scale(objectToAnimate, to, duration);
		}
		public void SwapDirection()
		{
			var temp = from;
			from = to;
			to = temp;
		}
		public void Disable()
		{
			SwapDirection();

			HandleTween();

			_tweenObject.setOnComplete(() =>
			{
				SwapDirection();
				gameObject.SetActive(false);
			});
		}
		//public void Disable(Action onCompletedAction)
		//{
		//	SwapDirection();
		//
		//	HandleTween();
		//
		//	_tweenObject.setOnComplete(onCompletedAction);
		//}
		#endregion
	}

	public enum UIAnimationTypes
	{
		Move,
		Scale,
		ScaleX,
		ScaleY,
		Fade
	}
}