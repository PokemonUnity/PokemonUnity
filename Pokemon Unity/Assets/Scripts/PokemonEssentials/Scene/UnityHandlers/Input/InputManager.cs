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

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// This class acts as a bridge between the Unity Input class and the PokemonUnity Input class.
	/// </summary>
	//public class InputManager : PokemonUnity.Input, IInput
	public class InputManager : global::UnityEngine.MonoBehaviour, IInput
	{
		#region Variables
		public event Action<object, IButtonEventArgs> OnKeyPress; //{ add { foreach (UserInputController controller in userInputControllers) controller.OnKeyPress += value; } remove { foreach (UserInputController controller in userInputControllers) controller.OnKeyPress -= value; } }
		//public event ButtonEventArgs OnKeyPress;
		//public event Action<object, IButtonEventArgs> OnKeyPress;
		//public event Action<object, IButtonEventArgs> OnKeyDown;
		//public event Action<object, IButtonEventArgs> OnKeyUp;
		public event EventHandler UpdateInput;
		private static InputManager instance; //Maybe use GameManager as Singleton?

		//private readonly Hashtable m_KeyTable = new Hashtable();
		[global::UnityEngine.SerializeField] private IList<UserInputController> userInputControllers = new List<UserInputController>();
		#endregion

		#region Unity Monobehavior
		void Awake()
		{
			//OnKeyPress += OnButtonStateChange;

			if (instance == null)
			{
				instance = this;
			}
			else if (instance != this)
			{
				Destroy(gameObject);
			}
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

		public bool press(int num, int player = 1)
		{
			if (userInputControllers.Count < player)
				return false;
			player--; //Index is 0 based
			//return userInputControllers.Any(controller => controller.press(num));
			return userInputControllers[player].press(num);
		}

		public bool trigger(int num, int player = 1)
		{
			if (userInputControllers.Count < player)
				return false;
			player--; //Index is 0 based
			//return userInputControllers.Any(controller => controller.trigger(num));
			return userInputControllers[player].trigger(num);
		}

		public bool repeat(int num, int player = 1)
		{
			if (userInputControllers.Count < player)
				return false;
			player--; //Index is 0 based
			//return userInputControllers.Any(controller => controller.repeat(num));
			return userInputControllers[player].repeat(num);
		}

		public void AddController(UserInputController controller)
		{
			controller.PlayerIndex = userInputControllers.Count + 1;
			userInputControllers.Add(controller);
		}

		public void RemoveController(UserInputController controller)
		{
			//userInputControllers.Remove(controller);
			RemoveController(controller.PlayerIndex);
		}

		private void RemoveController(int player)
		{
			if (userInputControllers.Count < player)
				return;
			userInputControllers.RemoveAt(player - 1);
		}

		void IInput.update()
		{
			//UpdateInput?.Invoke(this, EventArgs.Empty);
			foreach (UserInputController controller in userInputControllers)
				controller.update();
		}

		//bool IInput.press(PokemonUnity.Interface.InputKeys num)
		bool IInput.press(int num)
		{
			//return KeyPressed((int)num);
			return this.press((int)num, 1);
		}

		//bool IInput.trigger(PokemonUnity.Interface.InputKeys num)
		bool IInput.trigger(int num)
		{
			//return KeyPressed((int)num);
			return this.trigger((int)num, 1);
		}

		//bool IInput.repeat(PokemonUnity.Interface.InputKeys num)
		bool IInput.repeat(int num)
		{
			//return KeyPressed((int)num);
			return this.repeat((int)num, 1);
		}
		#endregion
	}
}