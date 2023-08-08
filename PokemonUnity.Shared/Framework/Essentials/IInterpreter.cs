using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface.RPGMaker.Kernal;

namespace PokemonEssentials.Interface
{
	/// <summary>
	/// This interpreter runs event commands. This class is used within the
	/// <see cref="IGameSystem"/> class and the <see cref="IGameEvent"/> class.
	/// </summary>
	public interface IInterpreter {
		/// <summary>
		/// </summary>
		/// <param name="depth">nest depth</param>
		/// <param name="main">main flag</param>
		IInterpreter initialize(int depth = 0, bool main = false);

		void clear();
		/// <summary>
		/// Event Setup
		/// </summary>
		/// <param name="list">list of event commands</param>
		/// <param name="event_id">event ID</param>
		/// <param name="map_id"></param>
		void setup(IList<IEventCommand> list, int event_id, int? map_id = null);

		bool running { get; }

		void setup_starting_event();
		/// <summary>
		/// Frame Update
		/// </summary>
		/// <returns></returns>
		System.Collections.IEnumerator update();
		/// <summary>
		/// Button Input
		/// </summary>
		void input_button();
		/// <summary>
		/// Setup Choices
		/// </summary>
		/// <param name="parameters"></param>
		void setup_choices(object parameters);

		bool command_dummy();

		bool ExecuteScript(string script);
		/// <summary>
		/// Event Command Execution
		/// </summary>
		bool execute_command { get; }

		// bool command_dummy() {
		// return true;
		//}
		/// <summary>
		/// End Event
		/// </summary>
		void command_end();
		/// <summary>
		/// Command Skip
		/// </summary>
		/// <returns></returns>
		bool command_skip();
		/// <summary>
		/// Get Character
		/// </summary>
		/// <param name="parameter">parameter</param>
		/// <returns></returns>
		IGameCharacter get_character(int parameter);
		/// <summary>
		/// Calculate Operated Value
		/// </summary>
		/// <param name="operation">operation</param>
		/// <param name="operand_type">operand type (0: invariable 1: variable)</param>
		/// <param name="operand">operand (number or variable ID)</param>
		/// <returns></returns>
		int operate_value(int operation, int operand_type, int operand);
		#region Command Scripts
		/// <summary>
		/// Show Text
		/// </summary>
		/// <returns></returns>
		bool command_101();
		/// <summary>
		/// Show Choices
		/// </summary>
		/// <returns></returns>
		bool command_102();
		/// <summary>
		/// When [**]
		/// </summary>
		/// <returns></returns>
		bool command_402();
		/// <summary>
		/// When Cancel
		/// </summary>
		/// <returns></returns>
		bool command_403();
		/// <summary>
		/// Input Number
		/// </summary>
		/// <returns></returns>
		bool command_103();
		/// <summary>
		/// Change Text Options
		/// </summary>
		/// <returns></returns>
		bool command_104();
		/// <summary>
		/// Button Input Processing
		/// </summary>
		/// <returns></returns>
		bool command_105();
		/// <summary>
		/// Wait
		/// </summary>
		/// <returns></returns>
		bool command_106();
		/// <summary>
		/// Conditional Branch
		/// </summary>
		/// <returns></returns>
		bool command_111();
		/// <summary>
		/// Else
		/// </summary>
		/// <returns></returns>
		bool command_411();
		/// <summary>
		/// Loop
		/// </summary>
		/// <returns></returns>
		bool command_112();
		/// <summary>
		/// Repeat Above
		/// </summary>
		/// <returns></returns>
		bool command_413();
		/// <summary>
		/// Break Loop
		/// </summary>
		/// <returns></returns>
		bool command_113();
		/// <summary>
		/// Exit Event Processing
		/// </summary>
		/// <returns></returns>
		bool command_115();
		/// <summary>
		/// Erase Event
		/// </summary>
		/// <returns></returns>
		bool command_116();
		/// <summary>
		/// Call Common Event
		/// </summary>
		/// <returns></returns>
		bool command_117();
		/// <summary>
		/// Label
		/// </summary>
		/// <returns></returns>
		bool command_118();
		/// <summary>
		/// Jump to Label
		/// </summary>
		/// <returns></returns>
		bool command_119();
		/// <summary>
		/// Control Switches
		/// </summary>
		/// <returns></returns>
		bool command_121();
		/// <summary>
		/// Control Variables
		/// </summary>
		/// <returns></returns>
		bool command_122();
		/// <summary>
		/// Control Self Switch
		/// </summary>
		/// <returns></returns>
		bool command_123();
		/// <summary>
		/// Control Timer
		/// </summary>
		/// <returns></returns>
		bool command_124();
		/*
		/// <summary>
		/// Change Gold
		/// </summary>
		/// <returns></returns>
		bool command_125();

		/// <summary>
		/// Change Items
		/// </summary>
		/// <returns></returns>
		bool command_126();

		/// <summary>
		/// Change Weapons
		/// </summary>
		/// <returns></returns>
		bool command_127();

		/// <summary>
		/// Change Armor
		/// </summary>
		/// <returns></returns>
		bool command_128();

		/// <summary>
		/// Change Party Member
		/// </summary>
		/// <returns></returns>
		bool command_129();
		*/
		/// <summary>
		/// Change Windowskin
		/// </summary>
		/// <returns></returns>
		bool command_131();
		/// <summary>
		/// Change Battle BGM
		/// </summary>
		/// <returns></returns>
		bool command_132();
		/// <summary>
		/// Change Battle End ME
		/// </summary>
		/// <returns></returns>
		bool command_133();
		/// <summary>
		/// Change Save Access
		/// </summary>
		/// <returns></returns>
		bool command_134();
		/// <summary>
		/// Change Menu Access
		/// </summary>
		/// <returns></returns>
		bool command_135();
		/// <summary>
		/// Change Encounter
		/// </summary>
		/// <returns></returns>
		bool command_136();
		/// <summary>
		/// Transfer Player
		/// </summary>
		/// <returns></returns>
		bool command_201();
		/// <summary>
		/// Set Event Location
		/// </summary>
		/// <returns></returns>
		bool command_202();
		/// <summary>
		/// Scroll Map
		/// </summary>
		/// <returns></returns>
		bool command_203();
		/// <summary>
		/// Change Map Settings
		/// </summary>
		/// <returns></returns>
		bool command_204();
		/// <summary>
		/// Change Fog Color Tone
		/// </summary>
		/// <returns></returns>
		bool command_205();
		/// <summary>
		/// Change Fog Opacity
		/// </summary>
		/// <returns></returns>
		bool command_206();
		/// <summary>
		/// Show Animation
		/// </summary>
		/// <returns></returns>
		bool command_207();
		/// <summary>
		/// Change Transparent Flag
		/// </summary>
		/// <returns></returns>
		bool command_208();
		/// <summary>
		/// Set Move Route
		/// </summary>
		/// <returns></returns>
		bool command_209();
		/// <summary>
		/// Wait for Move's Completion
		/// </summary>
		/// <returns></returns>
		bool command_210();
		/// <summary>
		/// Prepare for Transition
		/// </summary>
		/// <returns></returns>
		bool command_221();
		/// <summary>
		/// Execute Transition
		/// </summary>
		/// <returns></returns>
		bool command_222();
		/// <summary>
		/// Change Screen Color Tone
		/// </summary>
		/// <returns></returns>
		bool command_223();
		/// <summary>
		/// Screen Flash
		/// </summary>
		/// <returns></returns>
		bool command_224();
		/// <summary>
		/// Screen Shake
		/// </summary>
		/// <returns></returns>
		bool command_225();
		/// <summary>
		/// Show Picture
		/// </summary>
		/// <returns></returns>
		bool command_231();
		/// <summary>
		/// Move Picture
		/// </summary>
		/// <returns></returns>
		bool command_232();
		/// <summary>
		/// Rotate Picture
		/// </summary>
		/// <returns></returns>
		bool command_233();
		/// <summary>
		/// Change Picture Color Tone
		/// </summary>
		/// <returns></returns>
		bool command_234();
		/// <summary>
		/// Erase Picture
		/// </summary>
		/// <returns></returns>
		bool command_235();
		/// <summary>
		/// Set Weather Effects
		/// </summary>
		/// <returns></returns>
		bool command_236();

		bool command_247();
		/// <summary>
		/// Restore BGM/BGS
		/// </summary>
		/// <returns></returns>
		bool command_248();

		bool command_if(int value);
		/*
		bool command_301(); // Battle Processing
		bool command_601(); // If Win
		bool command_602(); // If Escape
		bool command_603(); // If Lose
		bool command_302(); // Shop Processing
		bool command_303(); // Name Processing
		bool command_311(); // Change HP
		bool command_312(); // Change SP
		bool command_313(); // Change State
		bool command_314(); // Recover All
		bool command_315(); // Change EXP
		bool command_316(); // Change Level
		bool command_317(); // Change Parameters
		bool command_318(); // Change Skills
		bool command_319(); // Change Equipment
		bool command_320(); // Change Actor Name
		bool command_321(); // Change Actor Class
		bool command_322(); // Change Actor Graphic
		bool command_331(); // Change Enemy HP
		bool command_332(); // Change Enemy SP
		bool command_333(); // Change Enemy State
		bool command_334(); // Enemy Recover All
		bool command_335(); // Enemy Appearance
		bool command_336(); // Enemy Transform
		bool command_337(); // Show Battle Animation
		bool command_338(); // Deal Damage
		bool command_339(); // Force Action
		bool command_340(); // Abort Battle
		*/
		/// <summary>
		/// Call Menu Screen
		/// </summary>
		/// <returns></returns>
		bool command_351();
		/// <summary>
		/// Call Save Screen
		/// </summary>
		/// <returns></returns>
		bool command_352();
		/// <summary>
		/// Game Over
		/// </summary>
		/// <returns></returns>
		bool command_353();
		/// <summary>
		/// Return to Title Screen
		/// </summary>
		/// <returns></returns>
		bool command_354();
		/// <summary>
		/// Script
		/// </summary>
		/// <returns></returns>
		bool command_355();
#endregion
	}
}