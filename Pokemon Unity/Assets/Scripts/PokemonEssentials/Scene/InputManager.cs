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
using PokemonUnity.UX;

namespace PokemonUnity
{
	/// <summary>
	/// </summary>
	/// This could easily be a ScriptableObject that's assigned to a Player's controller (for multi-player game)
	/// https://sites.google.com/a/unity3d.com/unity-input-advisory-board/design-overview
	[ExecuteInEditMode]
	public class InputManager : MonoBehaviour, IInput
	{
		#region Variables
		//public event ButtonEventArgs OnKeyPress;
		public event Action<object, IButtonEventArgs> OnKeyPress;
		public event Action<object, IButtonEventArgs> OnKeyDown;
		public event Action<object, IButtonEventArgs> OnKeyUp;
		public event EventHandler UpdateInput;

		private readonly Hashtable m_KeyTable = new Hashtable();
		#endregion

		#region Unity Monobehavior
		void Awake()
		{
			OnKeyPress += OnButtonStateChange;
		}
		void Update()
		{
			//UnityEngine.Input.
		}
		void FixedUpdate()
		{
			//https://www.youtube.com/watch?v=MfIsp28TYAQ
		}
		void OnDestroy()
		{
			OnKeyPress -= OnButtonStateChange;
		}
		#endregion

		#region Methods
		public bool KeyPressed(int i_Key) //System.Windows.Forms.Keys
		{
			if (m_KeyTable[i_Key] == null)
			{
				return false;
			}

			return (bool)m_KeyTable[i_Key];
		}

		private void ChangeState(int key, bool state) //System.Windows.Forms.Keys
		{
			m_KeyTable[key] = state;
		}

		private void OnButtonStateChange(object sender, PokemonEssentials.Interface.EventArg.IButtonEventArgs e)
		{
			//keyIsDown[e.Button] = e.IsDown;
			ChangeState(e.Button, e.IsDown);
		}

		void IInput.update()
		{
			//if (m_KeyTable.Count > 0) { //!= null
			//	foreach (KeyValuePair<int,object> i in m_KeyTable) {
			//		//  just noting that the state should be updated
			//		//  instead of thunking to Win32 256 times
			//		@stateUpdated[i]=false;
			//		if (KeyPressed(i.Key)) {
			//			//  If there is a repeat count, update anyway
			//			//  (will normally apply only to a very few keys)
			//			updateKeyState(i);
			//		}
			//	}
			//} else {
			//	m_KeyTable     .Clear();
			//	//@stateUpdated .Clear(); //=new Dictionary<int, bool>();
			//	//@keystate     .Clear(); //=new Dictionary<int, int>();
			//	//@triggerstate .Clear(); //=new Dictionary<int, bool>();
			//	//@releasestate .Clear(); //=new Dictionary<int, bool>();
			//	for (int i = 0; i < 256; i++) {
			//		//@stateUpdated[i]=true;
			//		//@keystate[i]=Input.getstate(i) ? 1 : 0;
			//		//@triggerstate[i]=false;
			//		//@releasestate[i]=false;
			//		@stateUpdated.Add(i,true);
			//		@keystate.Add(i,Input.getstate(i) ? 1 : 0);
			//		@triggerstate.Add(i,false);
			//		@releasestate.Add(i,false);
			//	}
			//}
		}

		bool IInput.press(InputKeys num)
		{
			return KeyPressed((int)num);
		}

		bool IInput.trigger(InputKeys num)
		{
			return KeyPressed((int)num);
		}

		bool IInput.repeat(InputKeys num)
		{
			return KeyPressed((int)num);
		}
		#endregion
	}

	internal class UserInputController
	{
		private static Hashtable m_KeyTable = new Hashtable();

		public static bool KeyPressed(int i_Key) //System.Windows.Forms.Keys
		{
			if (m_KeyTable[i_Key] == null)
			{
				return false;
			}

			return (bool)m_KeyTable[i_Key];
		}

		public static void ChangeState(int key, bool state) //System.Windows.Forms.Keys
		{
			m_KeyTable[key] = state;
		}
	}
}