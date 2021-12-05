using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat;
using PokemonUnity.Monster;
using PokemonUnity.Utility;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonEssentials.Interface
{
	namespace EventArg
	{
		#region Application EventArgs
		public interface IButtonEventArgs : IEventArgs
		{
			//ButtonEventArgs(int button, bool isDown) { Button = button; IsDown = isDown; }
			int Button { get; } // readonly
			bool IsDown { get; } // readonly
		}
		#endregion
	}

	public interface IGame : IGameBerryPlants, IGameDungeon, IGameFactory, IGameField, IGameHiddenMoves, IGameItem, IGameItemEffect, IGameOrgBattle, IGamePokeball, IGameResizer, IGameSafari, IGameTime
	{
		PokemonEssentials.Interface.IGlobalMetadata Global					{ get; }
		//PokemonEssentials.Interface.Field.IPokemonMapFactory MapFactory	{ get; }
		PokemonEssentials.Interface.Field.IMapFactory MapFactory			{ get; }
		PokemonEssentials.Interface.Field.IMapMetadata PokemonMap			{ get; }
		PokemonEssentials.Interface.Field.IMapMetadata MapData				{ get; }
		PokemonEssentials.Interface.Screen.IPokemonSystemOption PokemonSystem { get; }
		PokemonEssentials.Interface.ITempMetadata PokemonTemp				{ get; set; }
		PokemonEssentials.Interface.Field.IEncounters PokemonEncounters		{ get; }
		PokemonEssentials.Interface.Screen.IPokemonStorage PokemonStorage	{ get; }
		PokemonEssentials.Interface.Screen.IBag Bag							{ get; }
		PokemonEssentials.Interface.ISceneMap Scene							{ get; set; }
		PokemonEssentials.Interface.IGameTemp GameTemp						{ get; }
		PokemonEssentials.Interface.IGamePlayer Player						{ get; set; }
		PokemonEssentials.Interface.PokeBattle.ITrainer Trainer				{ get; set; }
		PokemonEssentials.Interface.RPGMaker.Kernal.ISystem DataSystem		{ get; set; }
		PokemonEssentials.Interface.ITileset[] DataTilesets					{ get; set; }
		PokemonEssentials.Interface.IGameCommonEvent[] DataCommonEvents		{ get; set; }
		PokemonEssentials.Interface.IGameSystem GameSystem					{ get; set; }
		PokemonEssentials.Interface.IGameSwitches GameSwitches				{ get; set; }
		PokemonEssentials.Interface.IGameSelfSwitches GameSelfSwitches		{ get; set; }
		PokemonEssentials.Interface.IGameVariable GameVariables				{ get; set; }
		PokemonEssentials.Interface.IGameScreen GameScreen					{ get; set; }
		PokemonEssentials.Interface.IGamePlayer GamePlayer					{ get; set; }
		PokemonEssentials.Interface.IGameMap GameMap						{ get; set; }
		//IGameMessage GameMessage											{ get; set; }
		//int SpeechFrame													{ get; }
	}
	public interface IGlobalMetadata : Field.IGlobalMetadata, IGlobalMetadataDependantEvents, IGlobalMetadataPokeRadar, IGlobalMetadataRoaming { }
	public interface ITempMetadata : Field.ITempMetadata, ITempMetadataBerryPlants, ITempMetadataDependantEvents, ITempMetadataField, ITempMetadataPokeRadar, ITempMetadataRoaming, ITempMetadataPokemonShadow { }

	public interface ITileset
	{
		int id { get; set; }
		string name { get; set; }
		string tileset_name { get; set; }
		string[] autotile_names { get; set; }
		string panorama_name { get; set; }
		int panorama_hue { get; set; }
		string fog_name { get; set; }
		int fog_hue { get; set; }
		int fog_opacity { get; set; }
		int fog_blend_type { get; set; }
		int fog_zoom { get; set; }
		int fog_sx { get; set; }
		int fog_sy { get; set; }
		string battleback_name { get; set; }
		int[] passages { get; set; }
		int[] priorities { get; set; }
		int[] terrain_tags { get; set; }
	}

	namespace RPGMaker.Kernal
	{
		#region Ignored this...
		/*public interface IAnimation
		{
			int id { get; set; }
			string name { get; set; }
			string animation_name { get; set; }
			int animation_hue { get; set; }
			int position { get; set; }
			int frame_max { get; set; }
			IAnimationFrame[] frames { get; set; }
			int[] timings { get; set; }
		}

		public interface IAnimationFrame
		{
			int cell_max { get; set; }
			int[,] cell_data { get; set; }
		}

		public interface IAnimationTiming
		{
			int frame { get; set; }
			IAudioSE se { get; set; }
			int flash_scope { get; set; }
			IColor flash_color { get; set; }
			int flash_duration { get; set; }
			int condition { get; set; }
		}*/
		#endregion

		public interface ISystem
		{
			int magic_number { get; set; }
			/// <summary>
			/// The initial party. An array of actor IDs.
			/// </summary>
			int[] party_members { get; set; }
			/// <summary>
			/// Element list. Text array using element IDs as subscripts, with the element in the 0 position being null.
			/// </summary>
			Dictionary<int, string> elements { get; set; }
			/// <summary>
			/// Switch list. Text array using switch IDs as subscripts, with the element in the 0 position being null.
			/// </summary>
			Dictionary<int, string> switches { get; set; }
			/// <summary>
			/// Variable list. Text array using variable IDs as subscripts, with the element in the 0 position being null.
			/// </summary>
			Dictionary<int, string> variables { get; set; }
			string windowskin_name { get; set; }
			/// <summary>
			/// The game title.
			/// </summary>
			string title_name { get; set; }
			string gameover_name { get; set; }
			string battle_transition { get; set; }
			IAudioBGM title_bgm { get; set; }
			IAudioBGM battle_bgm { get; set; }
			IAudioME battle_end_me { get; set; }
			IAudioME gameover_me { get; set; }
			IAudioSE cursor_se { get; set; }
			IAudioSE decision_se { get; set; }
			IAudioSE cancel_se { get; set; }
			IAudioSE buzzer_se { get; set; }
			IAudioSE equip_se { get; set; }
			IAudioSE shop_se { get; set; }
			IAudioSE save_se { get; set; }
			IAudioSE load_se { get; set; }
			IAudioSE battle_start_se { get; set; }
			IAudioSE escape_se { get; set; }
			IAudioSE actor_collapse_se { get; set; }
			IAudioSE enemy_collapse_se { get; set; }
			ISystemWords words { get; set; }
			/// <summary>
			/// Party settings for battle tests.
			/// </summary>
			ISystemTestBattler[] test_battlers { get; set; }
			/// <summary>
			/// The troop ID for battle tests.
			/// </summary>
			int test_troop_id { get; set; }
			/// <summary>
			/// The map ID of the player's initial position.
			/// </summary>
			int start_map_id { get; set; }
			/// <summary>
			/// The map X-coordinate of the player's initial position.
			/// </summary>
			float start_x { get; set; }
			/// <summary>
			/// The map Y-coordinate of the player's initial position.
			/// </summary>
			float start_y { get; set; }
			string battleback_name { get; set; }
			/// <summary>
			/// The battler graphic file name, for internal use.
			/// </summary>
			string battler_name { get; set; }
			/// <summary>
			/// The adjustment value for the battler graphic's hue (0..360), for internal use.
			/// </summary>
			int battler_hue { get; set; }
			/// <summary>
			/// The ID of the map currently being edited, for internal use.
			/// </summary>
			int edit_map_id { get; set; }
		}

		public interface ICommonEvent
		{
			/// <summary>
			/// The event ID.
			/// </summary>
			int id { get; set; }
			/// <summary>
			/// The event name.
			/// </summary>
			string name { get; set; }
			/// <summary>
			/// The event trigger (0: none, 1: autorun; 2: parallel).
			/// </summary>
			int trigger { get; set; }
			/// <summary>
			/// The condition switch ID.
			/// </summary>
			int switch_id { get; set; }
			/// <summary>
			/// List of event commands. 
			/// </summary>
			List<IEventCommand> list { get; set; }
		}

		public interface IMap
		{
			int tileset_id { get; set; }
			/// <summary>
			/// The map width.
			/// </summary>
			int width { get; set; }
			/// <summary>
			/// The map height.
			/// </summary>
			int height { get; set; }
			/// <summary>
			/// Scroll type (0: No Loop, 1: Vertical Loop, 2: Horizontal Loop, 3: Both Loop).
			/// </summary>
			int scroll_type { get; set; }
			/// <summary>
			/// Truth-value of whether BGM autoswitching is enabled.
			/// </summary>
			bool autoplay_bgm { get; set; }
			/// <summary>
			/// If BGM autoswitching is enabled, the name of that BGM (<see cref="IAudioObject"/>)
			/// </summary>
			IAudioBGM bgm { get; set; }
			/// <summary>
			/// Truth-value of whether BGS autoswitching is enabled.
			/// </summary>
			bool autoplay_bgs { get; set; }
			/// <summary>
			/// If BGS autoswitching is enabled, the name of that BGS (<see cref="IAudioObject"/>)
			/// </summary>
			IAudioBGS bgs { get; set; }
			/// <summary>
			/// Encounter list. A pokemon ID array.
			/// </summary>
			object[] encounter_list { get; set; }
			/// <summary>
			/// Number of steps between encounters.
			/// </summary>
			int encounter_step { get; set; }
			/// <summary>
			/// The map data. A 3-dimensional tile ID array 
			/// </summary>
			int[,,] data { get; set; }
			/// <summary>
			/// Map events. A hash that represents <see cref="IEvents"/> instances as values, using event IDs as the keys.
			/// </summary>
			object events { get; set; }
		}

		public interface IMapInfo
		{
			/// <summary>
			/// The map name.
			/// </summary>
			string name { get; set; }
			/// <summary>
			/// The parent map ID.
			/// </summary>
			int parent_id { get; set; }
			/// <summary>
			/// The map tree display order, used internally.
			/// </summary>
			int order { get; set; }
			/// <summary>
			/// The map tree expansion flag, used internally.
			/// </summary>
			bool expanded { get; set; }
			/// <summary>
			/// The X-axis scroll position, used internally.
			/// </summary>
			int scroll_x { get; set; }
			/// <summary>
			/// The Y-axis scroll position, used internally.
			/// </summary>
			int scroll_y { get; set; }
		}

		public interface IEvent
		{
			int id { get; set; }
			string name { get; set; }
			int x { get; set; }
			int y { get; set; }
			List<IEventPage> pages { get; set; }
		}

		public interface IEventPage
		{
			/// <summary>
			/// The event condition
			/// </summary>
			IEventPageCondition condition { get; set; }
			/// <summary>
			/// The event graphic 
			/// </summary>
			IEventPageGraphic graphic { get; set; }
			/// <summary>
			/// Type of movement (0: fixed, 1: random, 2: approach, 3: custom).
			/// </summary>
			int move_type { get; set; }
			/// <summary>
			/// Movement speed (1: x8 slower, 2: x4 slower, 3: x2 slower, 4: normal, 5: x2 faster, 6: x4 faster).
			/// </summary>
			int move_speed { get; set; }
			/// <summary>
			/// Movement frequency (1: lowest, 2: lower, 3: normal, 4: higher, 5: highest).
			/// </summary>
			int move_frequency { get; set; }
			/// <summary>
			/// Movement route. Referenced only when the movement type is set to Custom. <seealso cref="move_type"/>
			/// </summary>
			IMoveRoute move_route { get; set; }
			/// <summary>
			/// Truth value of the [Walking Anim.] option.
			/// </summary>
			bool walk_anime { get; set; }
			bool step_anime { get; set; }
			bool direction_fix { get; set; }
			bool through { get; set; }
			/// <summary>
			/// Priority type (0: below characters, 1: same as characters, 2: above characters).
			/// </summary>
			int priority_type { get; set; }
			//bool always_on_top { get; set; }
			/// <summary>
			/// Event trigger (0: action button, 1: player touch, 2: event touch, 3: autorun, 4: parallel process).
			/// </summary>
			int trigger { get; set; }
			/// <summary>
			/// List of event commands. 
			/// </summary>
			List<IEventCommand> list { get; set; }
		}

		public interface IEventPageCondition
		{
			/// <summary>
			/// value for whether the first [Switch] condition is valid.
			/// </summary>
			bool switch1_valid { get; set; }
			/// <summary>
			/// value for whether the second [Switch] condition is valid.
			/// </summary>
			bool switch2_valid { get; set; }
			/// <summary>
			/// value for whether the [Variable] condition is valid.
			/// </summary>
			bool variable_valid { get; set; }
			/// <summary>
			/// value for whether the [Self Switch] condition is valid.
			/// </summary>
			bool self_switch_valid { get; set; }
			/// <summary>
			/// value for whether the [Item] condition is valid.
			/// </summary>
			//bool item_valid { get; set; }
			//bool actor_valid { get; set; }
			/// <summary>
			/// If the first [Switch] condition is valid, the ID of that switch.
			/// </summary>
			int switch1_id { get; set; }
			/// <summary>
			/// If the second [Switch] condition is valid, the ID of that switch.
			/// </summary>
			int switch2_id { get; set; }
			/// <summary>
			/// If the [Variable] condition is valid, the ID of that variable.
			/// </summary>
			int variable_id { get; set; }
			/// <summary>
			/// If the [Variable] condition is valid, the standard value of that variable (x and greater).
			/// </summary>
			int variable_value { get; set; }
			/// <summary>
			/// If the [Self Switch] condition is valid, the letter of that self switch ("A".."D").
			/// </summary>
			char self_switch_ch { get; set; }
		}

		public interface IEventPageGraphic
		{
			/// <summary>
			/// The tile ID. If the specified graphic is not a tile, this value is 0.
			/// </summary>
			int tile_id { get; set; }
			/// <summary>
			/// The character's graphic file name.
			/// </summary>
			string character_name { get; set; }
			//int character_hue { get; set; }
			/// <summary>
			/// The character's index of the graphic file (0..7).
			/// </summary>
			int character_index { get; set; }
			/// <summary>
			/// The direction in which the character is facing (2: down, 4: left, 6: right, 8: up).
			/// </summary>
			int direction { get; set; }
			/// <summary>
			/// The character's pattern (0..2).
			/// </summary>
			int pattern { get; set; }
			//int opacity { get; set; }
			//int blend_type { get; set; }
		}

		public interface IEventCommand
		{
			/// <summary>
			/// The event code.
			/// </summary>
			int code { get; set; }
			/// <summary>
			/// The indent depth. Usually 0; the [Conditional Branch] command, among others, adds 1 with every step deeper.
			/// </summary>
			int indent { get; set; }
			/// <summary>
			/// Array containing the Move command arguments. The contents vary for each command.
			/// </summary>
			//List<IMoveCommand> parameters { get; set; }
			List<object> parameters { get; set; }
		}

		public interface IMoveRoute
		{
			/// <summary>
			/// value of the [Repeat Action] option.
			/// </summary>
			bool repeat { get; set; }
			/// <summary>
			/// value of the [Skip If Cannot Move] option.
			/// </summary>
			bool skippable { get; set; }
			/// <summary>
			/// value of the [Wait for Completion] option.
			/// </summary>
			bool wait { get; set; }
			/// <summary>
			/// Program contents. 
			/// </summary>
			List<IMoveCommand> list { get; set; }
		}

		public interface IMoveCommand
		{
			/// <summary>
			/// Move command code.
			/// </summary>
			int code { get; set; }
			/// <summary>
			/// Array containing the Move command arguments. The contents vary for each command.
			/// </summary>
			List<IMoveParameters> parameters { get; set; }
		}

		public interface IMoveParameters { }

		public interface ISystemWords
		{
			string gold { get; set; }
			string hp { get; set; }
			string sp { get; set; }
			string str { get; set; }
			string dex { get; set; }
			string agi { get; set; }
			string @int { get; set; }
			string atk { get; set; }
			string pdef { get; set; }
			string mdef { get; set; }
			string weapon { get; set; }
			string armor1 { get; set; }
			string armor2 { get; set; }
			string armor3 { get; set; }
			string armor4 { get; set; }
			string attack { get; set; }
			string skill { get; set; }
			string guard { get; set; }
			string item { get; set; }
			string equip { get; set; }
		}

		public interface ISystemTestBattler
		{
			int actor_id { get; set; }
			int level { get; set; }
			int weapon_id { get; set; }
			int armor1_id { get; set; }
			int armor2_id { get; set; }
			int armor3_id { get; set; }
			int armor4_id { get; set; }
		}
	}
}