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
	/// Game_System class and the Game_Event class.
	/// </summary>
	public interface IInterpreter {
		/// <summary>
		/// 
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

		bool pbExecuteScript(string script);
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
		// -----------------------------------------------------------------------------
		//  * Loop
		// -----------------------------------------------------------------------------
		bool command_112();
		// -----------------------------------------------------------------------------
		//  * Repeat Above
		// -----------------------------------------------------------------------------
		bool command_413();
		// -----------------------------------------------------------------------------
		//  * Break Loop
		// -----------------------------------------------------------------------------
		bool command_113();
		// -----------------------------------------------------------------------------
		//  * Exit Event Processing
		// -----------------------------------------------------------------------------
		bool command_115();
		// -----------------------------------------------------------------------------
		//  * Erase Event
		// -----------------------------------------------------------------------------
		bool command_116();
		// -----------------------------------------------------------------------------
		//  * Call Common Event
		// -----------------------------------------------------------------------------
		bool command_117();
		// -----------------------------------------------------------------------------
		//  * Label
		// -----------------------------------------------------------------------------
		bool command_118();
		// -----------------------------------------------------------------------------
		//  * Jump to Label
		// -----------------------------------------------------------------------------
		bool command_119();
		// -----------------------------------------------------------------------------
		//  * Control Switches
		// -----------------------------------------------------------------------------
		bool command_121();
		// -----------------------------------------------------------------------------
		//  * Control Variables
		// -----------------------------------------------------------------------------
		bool command_122();
		// -----------------------------------------------------------------------------
		//  * Control Self Switch
		// -----------------------------------------------------------------------------
		bool command_123();
		// -----------------------------------------------------------------------------
		//  * Control Timer
		// -----------------------------------------------------------------------------
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
		// -----------------------------------------------------------------------------
		//  * Change Windowskin
		// -----------------------------------------------------------------------------
		bool command_131();
		// -----------------------------------------------------------------------------
		//  * Change Battle BGM
		// -----------------------------------------------------------------------------
		bool command_132();
		// -----------------------------------------------------------------------------
		//  * Change Battle End ME
		// -----------------------------------------------------------------------------
		bool command_133();
		// -----------------------------------------------------------------------------
		//  * Change Save Access
		// -----------------------------------------------------------------------------
		bool command_134();
		// -----------------------------------------------------------------------------
		//  * Change Menu Access
		// -----------------------------------------------------------------------------
		bool command_135();
		// -----------------------------------------------------------------------------
		//  * Change Encounter
		// -----------------------------------------------------------------------------
		bool command_136();
		// -----------------------------------------------------------------------------
		//  * Transfer Player
		// -----------------------------------------------------------------------------
		bool command_201();
		// -----------------------------------------------------------------------------
		//  * Set Event Location
		// -----------------------------------------------------------------------------
		bool command_202();
		// -----------------------------------------------------------------------------
		//  * Scroll Map
		// -----------------------------------------------------------------------------
		bool command_203();
		// -----------------------------------------------------------------------------
		//  * Change Map Settings
		// -----------------------------------------------------------------------------
		bool command_204();
		// -----------------------------------------------------------------------------
		//  * Change Fog Color Tone
		// -----------------------------------------------------------------------------
		bool command_205();
		// -----------------------------------------------------------------------------
		//  * Change Fog Opacity
		// -----------------------------------------------------------------------------
		bool command_206();
		// -----------------------------------------------------------------------------
		//  * Show Animation
		// -----------------------------------------------------------------------------
		bool command_207();
		// -----------------------------------------------------------------------------
		//  * Change Transparent Flag
		// -----------------------------------------------------------------------------
		bool command_208();
		// -----------------------------------------------------------------------------
		//  * Set Move Route
		// -----------------------------------------------------------------------------
		bool command_209();
		// -----------------------------------------------------------------------------
		//  * Wait for Move's Completion
		// -----------------------------------------------------------------------------
		bool command_210();
		// -----------------------------------------------------------------------------
		//  * Prepare for Transition
		// -----------------------------------------------------------------------------
		bool command_221();
		// -----------------------------------------------------------------------------
		//  * Execute Transition
		// -----------------------------------------------------------------------------
		bool command_222();
		// -----------------------------------------------------------------------------
		//  * Change Screen Color Tone
		// -----------------------------------------------------------------------------
		bool command_223();
		// -----------------------------------------------------------------------------
		//  * Screen Flash
		// -----------------------------------------------------------------------------
		bool command_224();
		// -----------------------------------------------------------------------------
		//  * Screen Shake
		// -----------------------------------------------------------------------------
		bool command_225();
		// -----------------------------------------------------------------------------
		//  * Show Picture
		// -----------------------------------------------------------------------------
		bool command_231();
		// -----------------------------------------------------------------------------
		//  * Move Picture
		// -----------------------------------------------------------------------------
		bool command_232();
		// -----------------------------------------------------------------------------
		//  * Rotate Picture
		// -----------------------------------------------------------------------------
		bool command_233();
		// -----------------------------------------------------------------------------
		//  * Change Picture Color Tone
		// -----------------------------------------------------------------------------
		bool command_234();
		// -----------------------------------------------------------------------------
		//  * Erase Picture
		// -----------------------------------------------------------------------------
		bool command_235();
		// -----------------------------------------------------------------------------
		//  * Set Weather Effects
		// -----------------------------------------------------------------------------
		bool command_236();

		bool command_247();
		// -----------------------------------------------------------------------------
		//  * Restore BGM/BGS
		// -----------------------------------------------------------------------------
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