using System;
using PokemonEssentials.Interface;

namespace PokemonUnity
{
	public partial class Game_System : IGameSystem
	{
		/// <summary>
		/// map event interpreter
		/// </summary>
		//public InterpreterMixin map_interpreter { get; set; }
		/// <summary>
		/// battle event interpreter
		/// </summary>
		//public InterpreterMixin battle_interpreter { get; set; }
		/// <summary>
		/// timer
		/// </summary>
		public int timer { get; set; }
		/// <summary>
		/// timer working flag
		/// </summary>
		public bool timer_working { get; set; }
		/// <summary>
		/// save forbidden
		/// </summary>
		public bool save_disabled { get; set; }
		/// <summary>
		/// menu forbidden
		/// </summary>
		public bool menu_disabled { get; set; }
		/// <summary>
		/// encounter forbidden
		/// </summary>
		public bool encounter_disabled { get; set; }
		/// <summary>
		/// text option: positioning
		/// </summary>
		public int message_position { get; set; }
		/// <summary>
		/// text option: window frame
		/// </summary>
		public int message_frame { get; set; }
		/// <summary>
		/// save count
		/// </summary>
		public int save_count { get; set; }
		/// <summary>
		/// magic number
		/// </summary>
		public int magic_number { get; set; }
		public int autoscroll_x_speed { get; set; }
		public int autoscroll_y_speed { get; set; }
		public int bgm_position { get; set; }
		public int bgs_position { get; set; }
		public bool bgm_paused { get; set; }
		public bool bgs_paused { get; set; }

		public IAudioBGM defaultBGM { get; set; }
		public IAudioBGM playing_bgm { get; set; }
		public IAudioBGM memorized_bgm { get; set; }
		public IAudioBGS playing_bgs { get; set; }
		public IAudioBGS memorized_bgs { get; set; }
		private string _windowskin_name;


		public Game_System()
		{
			//if (Core.RPGVX)
			//{
			//	@map_interpreter = new Game_Interpreter(0, true);
			//	@battle_interpreter = new Game_Interpreter(0, false);
			//}
			//else
			//{
			//	@map_interpreter = new Interpreter(0, true);
			//	@battle_interpreter = new Interpreter(0, false);
			//}
			@timer = 0;
			@timer_working = false;
			@save_disabled = false;
			@menu_disabled = false;
			@encounter_disabled = false;
			@message_position = 2;
			@message_frame = 0;
			@save_count = 0;
			@magic_number = 0;
			@autoscroll_x_speed = 0;
			@autoscroll_y_speed = 0;
			@bgm_position = 0;
			@bgs_position = 0;
		}

		// ###############################################################################

		public void bgm_play(IAudioBGM bgm)
		{
			bgm_play_internal(bgm, 0);
		}
		// :nodoc:
		public void bgm_play_internal2(string name, float volume, float pitch, int position)
		{
			float vol = volume;
			if (Game.GameData.PokemonSystem != null) vol *= (Game.GameData.PokemonSystem.bgmvolume / 100.0f);
			vol = (int)vol;
			try
			{
				Audio.bgm_play(name, vol, pitch, position);
			}
			catch //ArgumentError;
			{
				Audio.bgm_play(name, vol, pitch);
			}
		}
		// :nodoc:
		public void bgm_play_internal(IAudioBGM bgm, int position)
		{
			if (!@bgm_paused)
			{
				@bgm_position = position;
			}
			@playing_bgm = bgm == null ? null : bgm.clone();
			if (bgm != null && bgm.name != "")
			{
				if (FileTest.audio_exist("Audio/BGM/" + bgm.name))
				{
					if (defaultBGM == null) bgm_play_internal2("Audio/BGM/" + bgm.name, bgm.volume, bgm.pitch,
					  @bgm_position);
				}
			}
			else
			{
				if (!@bgm_paused)
				{
					@bgm_position = position;
				}
				@playing_bgm = null;
				if (defaultBGM == null) Audio.bgm_stop();
			}
			if (defaultBGM != null)
			{
				bgm_play_internal2("Audio/BGM/" + @defaultBGM.name,
				  @defaultBGM.volume, @defaultBGM.pitch, @bgm_position);
			}
			//Graphics.frame_reset();
		}
		// :nodoc:
		public void bgm_pause(float fadetime = 0f)
		{
			int pos = Audio.bgm_position; //rescue 0;
			if (fadetime > 0.0)
			{
				this.bgm_fade(fadetime);
			}
			@bgm_position = pos;
			@bgm_paused = true;
		}
		// :nodoc:
		public void bgm_unpause()
		{
			@bgm_position = 0;
			@bgm_paused = false;
		}
		// :nodoc:
		public void bgm_resume(IAudioBGM bgm)
		{
			if (@bgm_paused)
			{
				this.bgm_play_internal(bgm, @bgm_position);
				@bgm_position = 0;
				@bgm_paused = false;
			}
		}
		// :nodoc:
		public void bgm_stop()
		{
			if (!@bgm_paused)
			{
				@bgm_position = 0;
			}
			@playing_bgm = null;
			if (defaultBGM == null) Audio.bgm_stop();
		}
		// :nodoc:
		public void bgm_fade(float time)
		{
			if (!@bgm_paused)
			{
				@bgm_position = 0;
			}
			@playing_bgm = null;
			if (defaultBGM == null) Audio.bgm_fade((int)Math.Floor(time * 1000));
		}

		//public IAudioBGM playing_bgm()
		//{
		//	return @playing_bgm;
		//}

		//  Saves the currently playing background music for later playback.
		public void bgm_memorize()
		{
			@memorized_bgm = @playing_bgm;
		}

		//  Plays the currently memorized background music
		public void bgm_restore()
		{
			bgm_play(@memorized_bgm);
		}

		//  Returns an RPG.AudioFile object for the currently playing background music
		public IAudioBGM getPlayingBGM()
		{
			return @playing_bgm != null ? @playing_bgm.clone() : null;
		}

		public void setDefaultBGM(string bgm, float volume = 80, float pitch = 100)
		{
			if (!string.IsNullOrEmpty(bgm))
			{
				setDefaultBGM(new RPG.AudioFile(bgm, volume, pitch) as IAudioBGM, volume, pitch);
			}
			//if (bgm != null && bgm.name != "")
			//{
			//	@defaultBGM = null;
			//	this.bgm_play(bgm);
			//	@defaultBGM = bgm.clone();
			//}
			//else
			//{
			//	@defaultBGM = null;
			//	this.bgm_play(@playing_bgm);
			//}
		}

		public void setDefaultBGM(IAudioBGM bgm, float volume = 80, float pitch = 100)
		{
			//if (bgm is string)
			//{
			//	bgm = new RPG.AudioFile(bgm, volume, pitch);
			//}
			if (bgm != null && bgm.name != "")
			{
				@defaultBGM = null;
				this.bgm_play(bgm);
				@defaultBGM = bgm.clone();
			}
			else
			{
				@defaultBGM = null;
				this.bgm_play(@playing_bgm);
			}
		}

		// ###############################################################################

		public void me_play(string me)
		{
			if (!string.IsNullOrEmpty(me))
			{
				me_play(new RPG.AudioFile(me) as IAudioME);
			}
			//if (me != null && me.name != "")
			//{
			//	if (FileTest.audio_exist("Audio/ME/" + me.name))
			//	{
			//		int vol = me.volume;
			//		if (Game.GameData.PokemonSystem) vol *= (Game.GameData.PokemonSystem.bgmvolume / 100.0f);
			//		vol = vol.to_i;
			//		Audio.me_play("Audio/ME/" + me.name, vol, me.pitch);
			//	}
			//}
			//else
			//{
			//	Audio.me_stop();
			//}
			//Graphics.frame_reset();
		}

		public void me_play(IAudioME me)
		{
			//if (me is String)
			//{
			//	me = new RPG.AudioFile(me);
			//}
			if (me != null && me.name != "")
			{
				if (FileTest.audio_exist("Audio/ME/" + me.name))
				{
					float vol = me.volume;
					if (Game.GameData.PokemonSystem != null) vol *= (Game.GameData.PokemonSystem.bgmvolume / 100.0f);
					vol = (int)vol;
					Audio.me_play("Audio/ME/" + me.name, vol, me.pitch);
				}
			}
			else
			{
				Audio.me_stop();
			}
			//Graphics.frame_reset();
		}

		public void me_fade(float time)
		{
			//if (!@bgm_paused)
			//{
			//	@bgm_position = 0;
			//}
			//@playing_bgm = null;
			//if (defaultBGM == null) Audio.me_fade((int)Math.Floor(time * 1000));
		}

		public void me_stop()
		{
			//if (!@bgm_paused)
			//{
			//	@bgm_position = 0;
			//}
			//@playing_bgm = null;
			//if (defaultBGM == null) Audio.me_stop();
		}

		// ###############################################################################

		public void bgs_play(IAudioBGS bgs)
		{
			@playing_bgs = bgs == null ? null : bgs.clone();
			if (bgs != null && bgs.name != "")
			{
				if (FileTest.audio_exist("Audio/BGS/" + bgs.name))
				{
					float vol = bgs.volume;
					if (Game.GameData.PokemonSystem != null) vol *= (Game.GameData.PokemonSystem.sevolume / 100.0f);
					vol = (int)vol;
					Audio.bgs_play("Audio/BGS/" + bgs.name, vol, bgs.pitch);
				}
			}
			else
			{
				@bgs_position = 0;
				@playing_bgs = null;
				Audio.bgs_stop();
			}
			//Graphics.frame_reset();
		}
		// :nodoc:
		public void bgs_pause(float fadetime = 0.0f)
		{
			if (fadetime > 0.0)
			{
				this.bgs_fade(fadetime);
			}
			else
			{
				this.bgs_stop();
			}
			@bgs_paused = true;
		}
		// :nodoc:
		public void bgs_unpause()
		{
			@bgs_paused = false;
		}
		// :nodoc:
		public void bgs_resume(IAudioBGS bgs)
		{
			if (@bgs_paused)
			{
				this.bgs_play(bgs);
				@bgs_paused = false;
			}
		}

		public void bgs_stop()
		{
			@bgs_position = 0;
			@playing_bgs = null;
			Audio.bgs_stop();
		}

		public void bgs_fade(float time)
		{
			@bgs_position = 0;
			@playing_bgs = null;
			Audio.bgs_fade((int)Math.Floor(time * 1000));
		}

		//public IAudioBGS playing_bgs()
		//{
		//	return @playing_bgs;
		//}

		public void bgs_memorize()
		{
			@memorized_bgs = @playing_bgs;
		}

		public void bgs_restore()
		{
			bgs_play(@memorized_bgs);
		}

		public IAudioBGS getPlayingBGS()
		{
			return @playing_bgs != null ? @playing_bgs.clone() : null;
		}

		// ###############################################################################

		public void se_play(string se)
		{
			if (!string.IsNullOrEmpty(se))
			{
				se_play(new RPG.AudioFile(se));
			}
			//if (se != null && se.name != "")
			//{
			//	if (FileTest.audio_exist("Audio/SE/" + se.name))
			//	{
			//		int vol = se.volume;
			//		if (Game.GameData.PokemonSystem) vol *= (Game.GameData.PokemonSystem.sevolume / 100.0f);
			//		vol = vol.to_i;
			//		Audio.se_play("Audio/SE/" + se.name, vol, se.pitch);
			//	}
			//}
		}

		public void se_play(IAudioSE se)
		{
			//if (se is String)
			//{
			//	se = new RPG.AudioFile(se);
			//}
			if (se != null && se.name != "")
			{
				if (FileTest.audio_exist("Audio/SE/" + se.name))
				{
					float vol = se.volume;
					if (Game.GameData.PokemonSystem != null) vol *= (Game.GameData.PokemonSystem.sevolume / 100.0f);
					vol = (int)vol;
					Audio.se_play("Audio/SE/" + se.name, vol, se.pitch);
				}
			}
		}

		public void se_stop()
		{
			Audio.se_stop();
		}

		// ###############################################################################

		public IAudioBGM battle_bgm
		{
			get
			{
				if (@battle_bgm == null)
				{
					return Game.GameData.DataSystem.battle_bgm;
				}
				else
				{
					return @battle_bgm;
				}
			}

			set
			{
				@battle_bgm = value;
			}
		}

		public IAudioME battle_end_me
		{
			get
			{
				if (@battle_end_me == null)
				{
					return Game.GameData.DataSystem.battle_end_me;
				}
				else
				{
					return @battle_end_me;
				}
			}

			set
			{
				@battle_end_me = value;
			}
		}

		// ###############################################################################

		public string windowskin_name
		{
			get
			{
				if (_windowskin_name == null)
				{
					return Game.GameData.DataSystem.windowskin_name;
				}
				else
				{
					return _windowskin_name;
				}
			}

			set
			{
				_windowskin_name = value;
			}
		}

		public void update()
		{
			if (@timer_working && @timer > 0)
			{
				@timer -= 1;
			}
			//if (Input.trigger(Input.F5) && Game is IGameMessage m && m.pbCurrentEventCommentInput(1, "Cut Scene"))
			//{
			//	@event = @map_interpreter.get_character(0);
			//	@map_interpreter.pbSetSelfSwitch(@event.id, "A", true);
			//	@map_interpreter.command_end();
			//	@event.start();
			//}
		}
	}
}