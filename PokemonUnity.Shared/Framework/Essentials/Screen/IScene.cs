using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.Screen;

namespace PokemonEssentials.Interface.Screen
{
	/// <summary>
	/// </summary>
	/// Screen should be renamed to `State`, as it's more in line with FSM
	public interface IScreen
	{
		//IEnumerator update();
	}

	/// <summary>
	/// Common UI functions used in both the Bag and item storage screens.
	/// Allows the user to choose a number. The window _helpwindow_ will
	/// display the _helptext_.
	/// </summary>
	public interface UIHelper {
		int ChooseNumber(IWindow helpwindow, string helptext, int maximum);

		void DisplayStatic(IWindow msgwindow, string message);

		/// <summary>
		/// Letter by letter display of the message <paramref name="msg"/> by the window <paramref name="helpwindow"/>.
		/// </summary>
		/// <param name="helpwindow"></param>
		/// <param name="msg"></param>
		/// <param name="brief"></param>
		/// <returns></returns>
		IEnumerator Display(IWindow helpwindow, string msg, bool brief);

		/// <summary>
		/// Letter by letter display of the message <paramref name="msg"/> by the window <paramref name="helpwindow"/>,
		/// used to ask questions.
		/// </summary>
		/// <param name="helpwindow"></param>
		/// <param name="msg"></param>
		/// <returns>Returns true if the user chose yes, false if no.</returns>
		bool Confirm(IWindow helpwindow, string msg);

		int ShowCommands(IWindow helpwindow, string helptext, string[] commands);
	}

	public interface IEntity
	{
	}
	public interface IHasDisplayMessage
	{
		void Refresh();

		void Display(string v);
		//bool Confirm(string v);
		//ToDo: rename to "DisplayConfirmMessage"?
		bool DisplayConfirm(string v);
	}
	/// <summary>
	/// A scene basically represents unity (or any frontend) where code pauses
	/// for user interaction (animation, and user key inputs).
	/// </summary>
	/// <remarks>
	/// When code has a scene variable calling a method in middle of script
	/// everything essentially comes to a halt as the frontend takes over
	/// and the code awaits a result or response to begin again.
	/// </remarks>
	public interface IScene : IHasDisplayMessage
	{
		/// <summary>
		/// Represents the unique id for given scene.
		/// Used for loading scenes in unity.
		/// </summary>
		int Id { get; }

		//void Refresh();

		/// <summary>
		/// Shows the player's Poké Ball being thrown to capture a Pokémon.
		/// </summary>
		//void pokeballThrow(Items ball, int shakes,bool critical,IBattler targetBattler,IScene scene,IBattler battler, int burst = -1, bool showplayer = false);
		//void Display(string v);
		//bool Confirm(string v);
	}
}