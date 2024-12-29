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
			GameManager.current.onLoadLevel += Scene_onLoadLevel;
			SceneManager.sceneLoaded += SceneManager_sceneLoaded;
		}

		/// <summary>
		/// Start is called before the first frame update
		/// </summary>
		/// <remarks>
		/// If gameobject persist, should only ever be called once.
		/// </remarks>
		void Start()
		{
			// Begin all scenes with a fade in effect
			if (canvasGroup != null)
			{
				canvasGroup.gameObject.SetActive(true);
				canvasGroup.blocksRaycasts = false;
				canvasGroup.interactable = false;
				canvasGroup.alpha = 1f;
				LeanTween.alphaCanvas(canvasGroup, 0f, transitionTime);
			}
		}

		void OnDestroy()
		{
			GameManager.current.onLoadLevel -= Scene_onLoadLevel;
			SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
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
			SceneManager.LoadScene(level, LoadSceneMode.Single);

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

		/// <summary>
		/// Unity Scenes that are UI only, and do not require a player character to be loaded.
		/// </summary>
		/// <remarks>
		/// UI Scenes are scenes that are used to display information to the player,
		/// overlay on top of the game world, or are used to display the game's menu.
		/// Should still continue to Garbage Collect, check for memory leaks, and add black-out transitions.
		/// </remarks>
		/// <param name="scene">UI Scene</param>
		/// <returns></returns>
		IEnumerator LoadScene(IScene scene)
		{
			//begin fade to black...
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
			//play animation
			LeanTween.alphaCanvas(canvasGroup, 1f, transitionTime);

			// wait
			yield return new WaitForSeconds(transitionTime);

			//load scene
			//SceneManager.LoadScene(scene, LoadSceneMode.Additive);
			//use interface to instantiate gameObject and assign in-scene entity to variable
			//using interface variable to startScene and destroy previous scene during fade animation
			((object)scene as GameObject).gameObject.SetActive(true); //We can assume the scene is attached to a monobehavior class

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
			StartCoroutine(LoadLevel(level));
			//StartCoroutine(LoadScene(level));
		}

		private void Scene_onLoadLevel(IScene level)
		{
			//SceneManager.LoadScene(level);
			//StartCoroutine(LoadLevel(level.Id));
			Scene_onLoadLevel((int)GetSceneType(level));
		}

		private void Scene_onLoadLevel(Scenes level)
		{
			Scene_onLoadLevel((int)level);
		}

		private void Scene_onLoadLevel(IOnLoadLevelEventArgs args)
		{
			//SceneManager.LoadScene(level);
			//StartCoroutine(LoadLevel(args.Scene.Id));
			Scene_onLoadLevel(args.Scene);
		}

		private void SceneManager_sceneLoaded(global::UnityEngine.SceneManagement.Scene arg0, LoadSceneMode arg1)
		{
			//ToDo: Can extend the duration of transition fade or loading screen using loaded boolean...
			//ToDo: check if scene is loaded, and if so, remove from list of scenes to load...
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