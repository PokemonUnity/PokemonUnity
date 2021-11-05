using PokemonEssentials.Interface.Field;
using PokemonUnity.UX;
using System;

namespace PokemonEssentials.Interface
{
	public interface IGameTemp
	{
		int background_bitmap { get; }

		bool battle_abort { get; set; }
		bool battle_calling { get; set; }
		bool battle_can_escape { get; set; }
		bool battle_can_lose { get; set; }
		bool battle_event_flags { get; set; }
		bool battle_main_phase { get; set; }
		Action battle_proc { get; set; }
		int battle_troop_id { get; set; }
		int battle_turn { get; set; }
		string battleback_name { get; set; }
		int choice_cancel_type { get; set; }
		int choice_max { get; set; }
		Action<object> choice_proc { get; set; }
		int choice_start { get; set; }
		int common_event_id { get; set; }
		bool debug_calling { get; set; }
		int? forcing_battler { get; set; }
		bool gameover { get; set; }
		bool in_battle { get; set; }
		bool in_menu { get; set; }
		int last_file_index { get; set; }
		IAudioBGM map_bgm { get; set; }
		int map_refresh { get; set; }
		bool menu_beep { get; set; }
		bool menu_calling { get; set; }
		Action message_proc { get; set; }
		string message_text { get; set; }
		bool message_window_showing { get; set; }
		int name_actor_id { get; set; }
		bool name_calling { get; set; }
		int name_max_char { get; set; }
		int num_input_digits_max { get; set; }
		int num_input_start { get; set; }
		int num_input_variable_id { get; set; }
		int player_new_direction { get; set; }
		int player_new_map_id { get; set; }
		ITilePosition player_new_position { get; set; }
		float player_new_x { get; set; }
		float player_new_y { get; set; }
		bool player_transferring { get; set; }
		bool save_calling { get; set; }
		bool shop_calling { get; set; }
		int shop_goods { get; set; }
		bool to_title { get; set; }
		string transition_name { get; set; }
		bool transition_processing { get; set; }
	}
}