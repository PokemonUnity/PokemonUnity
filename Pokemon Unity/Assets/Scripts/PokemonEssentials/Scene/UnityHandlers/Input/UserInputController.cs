using System;
using System.Collections.Generic;
using PokemonEssentials.Interface;
//using PokemonEssentials.Interface.PokeBattle.Rules;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// Used for detecting user input.
	/// </summary>
	/// This could easily be a ScriptableObject that's assigned to a Player's controller (for multi-player game)
	/// https://sites.google.com/a/unity3d.com/unity-input-advisory-board/design-overview
	public class UserInputController : global::UnityEngine.MonoBehaviour, IInput
	{
		private int playerIndex;
		//ToDo: Add a table map for runtime custom key binding
		private Dictionary<int, bool>	currentKeyState;
		private Dictionary<int, bool>	previousKeyState;
		private Dictionary<int, float>	keyHoldTime;
		public Dictionary<int, bool>	CurrentKeyState		{ get { return currentKeyState; } }
		public Dictionary<int, bool>	PreviousKeyState	{ get { return previousKeyState; } }
		public Dictionary<int, float>	KeyHoldTime			{ get { return keyHoldTime; } }
		public int PlayerIndex								{ get { return playerIndex; } set { playerIndex = value; } }

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
					//previousKeyState[num] = currentKeyState.ContainsKey(num) ? currentKeyState[num] : false;
					if (currentKeyState.ContainsKey(num))
						previousKeyState[num] = currentKeyState[num];
					else {
						currentKeyState.Add(num, false);
						previousKeyState.Add(num, false);
					}
					currentKeyState[num] = isPressed;

					if (!keyHoldTime.ContainsKey(num))
						keyHoldTime.Add(num, 0); //keyHoldTime[num] = 0;

					if (isPressed)
						keyHoldTime[num] += global::UnityEngine.Time.deltaTime;
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
				return (keyHoldTime[num] % 0.5f) < global::UnityEngine.Time.deltaTime;

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