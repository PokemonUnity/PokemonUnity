namespace PokemonUnity
{
	public interface IGame_System
	{
		/// <summary>
		/// map event interpreter
		/// </summary>
		//InterpreterMixin map_interpreter { get; set; }
		/// <summary>
		/// battle event interpreter
		/// </summary>
		//InterpreterMixin battle_interpreter { get; set; }
		int autoscroll_x_speed { get; set; }
		int autoscroll_y_speed { get; set; }
		IAudioObject battle_bgm { get; set; }
		IAudioObject battle_end_me { get; set; }
		int bgm_position { get; set; }
		/// <summary>
		/// encounter forbidden
		/// </summary>
		bool encounter_disabled { get; set; }
		/// <summary>
		/// magic number
		/// </summary>
		int magic_number { get; set; }
		/// <summary>
		/// menu forbidden
		/// </summary>
		bool menu_disabled { get; set; }
		/// <summary>
		/// text option: window frame
		/// </summary>
		int message_frame { get; set; }
		/// <summary>
		/// text option: positioning
		/// </summary>
		int message_position { get; set; }
		IAudioObject playing_bgm { get; set; }
		IAudioObject playing_bgs { get; set; }
		/// <summary>
		/// save count
		/// </summary>
		int save_count { get; set; }
		/// <summary>
		/// timer
		/// </summary>
		int timer { get; set; }
		/// <summary>
		/// timer working flag
		/// </summary>
		bool timer_working { get; set; }
		/// <summary>
		/// save forbidden
		/// </summary>
		bool save_disabled { get; set; }
		string windowskin_name { get; set; }

		void bgm_fade(float time);
		void bgm_memorize();
		void bgm_pause(float fadetime = 0);
		void bgm_play(IAudioObject bgm);
		void bgm_play_internal(IAudioObject bgm, int position);
		void bgm_play_internal2(string name, float volume, float pitch, float position);
		void bgm_restore();
		void bgm_resume(IAudioObject bgm);
		void bgm_stop();
		void bgm_unpause();
		void bgs_fade(float time);
		void bgs_memorize();
		void bgs_pause(float fadetime = 0);
		void bgs_play(IAudioObject bgs);
		void bgs_restore();
		void bgs_resume(IAudioObject bgs);
		void bgs_stop();
		void bgs_unpause();
		IAudioObject getPlayingBGM();
		IAudioObject getPlayingBGS();
		void me_play(IAudioObject me);
		void setDefaultBGM(IAudioObject bgm, float volume = 80, float pitch = 100);
		void se_play(IAudioObject se);
		void se_stop();
		void update();
	}
}