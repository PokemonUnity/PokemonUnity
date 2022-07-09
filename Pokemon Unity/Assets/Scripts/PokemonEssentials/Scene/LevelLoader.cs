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

namespace PokemonUnity
{
	/// <summary>
	/// </summary>
	/// https://www.youtube.com/watch?v=CE9VOZivb3I
	[ExecuteInEditMode]
	public class LevelLoader : MonoBehaviour
	{
		#region Variables
		public float transitionTime = .5f;
		#endregion

		#region Unity Monobehavior
		void Awake()
		{
			GameEvents.current.onLoadLevel += Scene_onLoadLevel;
		}
		void Start()
		{
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
			//play animation
			//begin fade to black...

			// wait
			yield return new WaitForSeconds(transitionTime);

			//load scene
			SceneManager.LoadScene(level);
		}

		IEnumerator LoadLevel(IScene level)
		{
			//play animation
			//begin fade to black...

			// wait
			yield return new WaitForSeconds(transitionTime);

			//load scene
			SceneManager.LoadScene(level.Id);
		}

		private void Scene_onLoadLevel(int level)
		{
			//SceneManager.LoadScene(level);
			StartCoroutine(LoadLevel(level));
		}

		private void Scene_onLoadLevel(IScene level)
		{
			//SceneManager.LoadScene(level);
			StartCoroutine(LoadLevel(level));
		}

		private void Scene_onLoadLevel(IOnLoadLevelEventArgs args)
		{
			//SceneManager.LoadScene(level);
			StartCoroutine(LoadLevel(args.Scene.Id));
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

	public interface IOnLoadLevelEventArgs : IEventArgs
	{
		//readonly int EventId = typeof(OnMapCreateEventArgs).GetHashCode();

		//int Id { get { return EventId; } }
		//int Id { get { return Pokemon.GetHashCode(); } } //EventId;
		IScene Scene { get; set; }
		//ToDo: Make an Enum for Transition Animation Type?
	}
}