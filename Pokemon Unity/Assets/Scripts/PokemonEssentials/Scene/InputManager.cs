using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Interface;
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
	/// This class acts as a bridge between the Unity Input class and the PokemonUnity Input class.
	/// </summary>
	[ExecuteInEditMode]
	public class InputManager : PokemonUnity.Input, IInput
	{
		#region Variables
		//public event ButtonEventArgs OnKeyPress;
		public event Action<object, IButtonEventArgs> OnKeyPress;
		//public event Action<object, IButtonEventArgs> OnKeyDown;
		//public event Action<object, IButtonEventArgs> OnKeyUp;
		//public event EventHandler UpdateInput;
		//
		//private readonly Hashtable m_KeyTable = new Hashtable();
		private IList<UserInputController> userInputControllers = new List<UserInputController>();
		#endregion

		#region Unity Monobehavior
		void Awake()
		{
			//OnKeyPress += OnButtonStateChange;
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
			//OnKeyPress -= OnButtonStateChange;
		}
		#endregion

		#region Methods
		//public bool KeyPressed(int i_Key) //System.Windows.Forms.Keys
		//{
		//	if (m_KeyTable[i_Key] == null)
		//	{
		//		return false;
		//	}
		//
		//	return (bool)m_KeyTable[i_Key];
		//}
		//
		//private void ChangeState(int key, bool state) //System.Windows.Forms.Keys
		//{
		//	m_KeyTable[key] = state;
		//}
		//
		//private void OnButtonStateChange(object sender, PokemonEssentials.Interface.EventArg.IButtonEventArgs e)
		//{
		//	//keyIsDown[e.Button] = e.IsDown;
		//	ChangeState(e.Button, e.IsDown);
		//}

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

		//bool IInput.press(PokemonUnity.Interface.InputKeys num)
		bool IInput.press(int num)
		{
			return KeyPressed((int)num);
		}

		//bool IInput.trigger(PokemonUnity.Interface.InputKeys num)
		bool IInput.trigger(int num)
		{
			return KeyPressed((int)num);
		}

		//bool IInput.repeat(PokemonUnity.Interface.InputKeys num)
		bool IInput.repeat(int num)
		{
			return KeyPressed((int)num);
		}
		#endregion
	}


	/// <summary>
	/// Used for detecting user input.
	/// </summary>
	/// This could easily be a ScriptableObject that's assigned to a Player's controller (for multi-player game)
	/// https://sites.google.com/a/unity3d.com/unity-input-advisory-board/design-overview
	public class UserInputController : MonoBehaviour, IInput
	{
		//ToDo: Add a table map for runtime custom key binding
		private Dictionary<int, bool>	currentKeyState;
		private Dictionary<int, bool>	previousKeyState;
		private Dictionary<int, float>	keyHoldTime;
		public Dictionary<int, bool>	CurrentKeyState		{ get { return currentKeyState; } }
		public Dictionary<int, bool>	PreviousKeyState	{ get { return previousKeyState; } }
		public Dictionary<int, float>	KeyHoldTime			{ get { return keyHoldTime; } }

		public event Action<object, PokemonEssentials.Interface.EventArg.IButtonEventArgs> OnKeyPress;

		#region Unity Monobehavior
		void Awake()
		{
			currentKeyState = new Dictionary<int, bool>();
			previousKeyState = new Dictionary<int, bool>();
			keyHoldTime = new Dictionary<int, float>();
		}

		// Update is called once per frame
		void Update()
		{
			update();
		}
		#endregion

		public void update()
		{
			//PlayerPrefs.GetInt(tag);
			// Example key update, using KeyCode for simplicity
			//foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
			foreach (PokemonUnity.Interface.InputKeys key in System.Enum.GetValues(typeof(PokemonUnity.Interface.InputKeys)))
			//switch (global::UnityEngine.Input.)
			{
				//default:
					int num = (int)key;
					bool isPressed = global::UnityEngine.Input.GetKey(key.ToString());
					previousKeyState[num] = currentKeyState.ContainsKey(num) ? currentKeyState[num] : false;
					currentKeyState[num] = isPressed;

					if (!keyHoldTime.ContainsKey(num))
						keyHoldTime[num] = 0;

					if (isPressed)
						keyHoldTime[num] += Time.deltaTime;
					else
						keyHoldTime[num] = 0;
					//break;
			}
		}

		public bool press(int num)
		{
			return currentKeyState.ContainsKey(num) && currentKeyState[num];
		}

		public bool trigger(int num)
		{
			return currentKeyState.ContainsKey(num) && currentKeyState[num] &&
				   previousKeyState.ContainsKey(num) && !previousKeyState[num];
		}

		public bool repeat(int num)
		{
			if (!currentKeyState.ContainsKey(num) || !currentKeyState[num])
				return false;

			// Repeat action every 0.5 seconds after holding down for 1 second
			if (keyHoldTime[num] > 1.0f)
				return (keyHoldTime[num] % 0.5f) < Time.deltaTime;

			return false;
		}

		public void ClearState()
		{
			currentKeyState.Clear();
			previousKeyState.Clear();
			keyHoldTime.Clear();
		}
	}
}