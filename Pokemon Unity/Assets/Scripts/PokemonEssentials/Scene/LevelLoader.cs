using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Character;
using PokemonUnity.Monster;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// This class acts as Unity's scene manager and is used to load scenes in the game engine.
	/// </summary>
	/// https://www.youtube.com/watch?v=CE9VOZivb3I
	[ExecuteInEditMode]
	public partial class LevelLoader : MonoBehaviour
	{
		#region Variables
		public float transitionTime = .5f;
		public global::UnityEngine.CanvasGroup canvasGroup;
		private IDictionary<Type, Scenes> sceneMapping = new Dictionary<Type, Scenes>();
		//{
		//	{ typeof(PokemonEssentials.Interface.Screen.IIntroEventScene), Scenes.Intro }, //"Intro"
		//};
		#endregion

		#region Unity Monobehavior
		void Awake()
		{
			GameEvents.current.onLoadLevel += Scene_onLoadLevel;
		}

		void OnDestroy()
		{
			GameEvents.current.onLoadLevel -= Scene_onLoadLevel;
		}
		#endregion

		#region Methods
		//public void LoadNextLevel(int id)
		//{
		//	Scene_onLoadLevel(id);
		//}

		public void LoadNextLevel(IScene scene)
		{
			Scene_onLoadLevel(scene.Id);
		}

		IEnumerator LoadLevel(int level)
		{
			//begin fade to black...
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
			//play animation
			LeanTween.alphaCanvas(canvasGroup, 1f, transitionTime);

			// wait
			yield return new WaitForSeconds(transitionTime);

			//load scene
			SceneManager.LoadScene(level);

			//ToDo: collect garbage while waiting for scene to load...
			GC.Collect();

			//ToDo: check start fade, and use matching ending or random fade...

			//undo fade to black...
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
			//play animation
			//ToDo: check start fade, and use matching ending or random fade...
			LeanTween.alphaCanvas(canvasGroup, 0f, transitionTime);
		}

		//IEnumerator LoadLevel(IScene level)
		//{
		//	//begin fade to black...
		//	canvasGroup.interactable = true;
		//	canvasGroup.blocksRaycasts = true;
		//	//play animation
		//	//ToDo: use conditional to give different fade options...
		//	LeanTween.alphaCanvas(canvasGroup, 1f, transitionTime);
		//
		//	// wait
		//	yield return new WaitForSeconds(transitionTime);
		//
		//	//load scene
		//	SceneManager.LoadScene(level.Id);
		//
		//	//undo fade to black...
		//	canvasGroup.interactable = false;
		//	canvasGroup.blocksRaycasts = false;
		//	//play animation
		//	//ToDo: check start fade, and use matching ending or random fade...
		//	LeanTween.alphaCanvas(canvasGroup, 0f, transitionTime);
		//}

		private void Scene_onLoadLevel(int level)
		{
			//SceneManager.LoadScene(level);
			StartCoroutine(LoadLevel(level));
		}

		private void Scene_onLoadLevel(IScene level)
		{
			//SceneManager.LoadScene(level);
			//StartCoroutine(LoadLevel(level.Id));
			StartCoroutine(LoadLevel((int)GetSceneType(level)));
		}

		private void Scene_onLoadLevel(IOnLoadLevelEventArgs args)
		{
			//SceneManager.LoadScene(level);
			//StartCoroutine(LoadLevel(args.Scene.Id));
			Scene_onLoadLevel(args.Scene);
		}

		private Scenes GetSceneType(IScene scene)
		{
			Type type = scene.GetType();
			foreach (var entry in sceneMapping)
			{
				if (entry.Key.IsAssignableFrom(type))
				{
					return entry.Value;
				}
			}
			return Scenes.None; //throw new ArgumentException("No unity scene found for the given interface type");
		}
		#endregion
	}
}
namespace PokemonEssentials.Interface.EventArg
{
	public class  OnLoadLevelEventArgs : EventArgs, PokemonEssentials.Interface.EventArg.IEventArgs
	{
		public static readonly int EventId = typeof(OnLoadLevelEventArgs).GetHashCode();

		public int Id { get { return EventId; } }
		public IScene Scene { get; set; }
	}
}