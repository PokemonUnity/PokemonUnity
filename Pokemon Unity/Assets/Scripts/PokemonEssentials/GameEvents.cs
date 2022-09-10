using System;
using System.IO;
using System.Linq;
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

namespace PokemonUnity
{
	/// <summary>
	/// </summary>
	/// https://www.youtube.com/watch?v=gx0Lt4tCDE0
	[ExecuteInEditMode]
	public class GameEvents : MonoBehaviour
	{
		#region Variables
		public static GameEvents current;
		public event Action onLevelLoaded;
		public event Action onChangePartyLineup;
		public event Action<int> onSelectPartyEntry;
		public event Action<int> onSelectRosterEntry;
		public event Action<int> onSelectRosterPool;
		//public event Action<int> onLoadLevel;
		public event Action<IScene> onLoadLevel;
		//public event Action<IOnLoadLevelEventArgs> onLoadLevel;
		#endregion

		#region Unity Monobehavior
		void Awake()
		{
			Debug.Log("Game Events is Awake!");
			current = this;
			//UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject
		}
		void Start()
		{
		}
		#endregion

		#region Methods
		//public void OnLoadLevel(int id)
		//{
		//	if (onLoadLevel != null) onLoadLevel(id);
		//}
		public void OnLoadLevel(IScene scene)
		{
			if (onLoadLevel != null) onLoadLevel(scene);
		}
		public void OnChangePartyLineup()
		{
			if (onChangePartyLineup != null) onChangePartyLineup();
		}
		public void OnSelectPartyEntry(int id)
		{
			if (onSelectPartyEntry != null) onSelectPartyEntry(id);
		}
		public void OnSelectRosterEntry(int id)
		{
			if (onSelectRosterEntry != null) onSelectRosterEntry(id);
		}
		public void OnSelectRosterPool(int id)
		{
			if (onSelectRosterPool != null) onSelectRosterPool(id);
		}
		#endregion
	}
}