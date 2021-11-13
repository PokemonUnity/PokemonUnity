using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonEssentials.Interface
{
	interface IGameAudio
	{
		void getPlayMusic();
		void getPlaySound();

		bool Audio_bgm_playing { get; }

		string Audio_bgm_name { get; }

		float Audio_bgm_pitch { get; }

		void Audio_bgm_play(string name, int volume, float pitch, int position = 0);

		void Audio_bgm_fadein(int ms);

		void Audio_bgm_fade(int ms);

		void Audio_bgm_stop();

		int Audio_bgm_get_position { get; }

		int Audio_bgm_get_volume { get; }

		void Audio_bgm_set_volume(int volume);

		void Audio_me_play(string name, int volume, float pitch, int position = 0);

		void Audio_me_fade(int ms);

		void Audio_me_stop();

		void Audio_bgs_play(string name, int volume, float pitch, int position = 0);

		void Audio_bgs_fade(int ms);

		void Audio_bgs_stop();

		void Audio_se_play(string name, int volume, float pitch, int position = 0);

		void Audio_se_stop();
	}

	interface IAudioContext
	{
		int context { get; set; }

		IAudioContext initialize();

		void dispose();
	}

	/*
	// ####################################
	// Needed because RGSS doesn't call at_exit procs on exit
	// Exit is not called when game is reset (using F12)
	if (!$AtExitProcs) $AtExitProcs=[];

	void exit(int code=0) {
	  foreach (var p in $AtExitProcs) {
		p.call;
	  }
	  raise new SystemExit(code);
	}

	void at_exit(&block) {
	  $AtExitProcs.Add(new Proc(&block));
	}

	// ####################################
	// Works around a problem with FileTest.exist
	// if directory contains accent marks
	bool safeExists (f) {
	  ret=false;
	  File.open(f,"rb") { ret=true } rescue null;
	  return ret;
	}
		*/

	interface IAudioState
	{
		void setWaitingBGM(IAudioBGM bgm, int volume, float pitch, int position);

		bool bgmActive { get; }
		bool meActive { get; }

		IAudioBGM waitingBGM { get; set; }
		IAudioBGM context { get; }
		IAudioME meContext { get; }
		IAudioBGS bgsContext { get; }
		IAudioSE seContext { get; }
		//void system { get; }
		IAudioBGM bgm { get; set; }
		string name { get; set; }
		float pitch { get; set; }
		int volume { get; set; }
	}




	// ###################################################
	//if safeExists("audio.dll");

	//public interface IGraphics
	//{
	//	//if (!defined(audiomodule_update)) {
	//	//  interface << self
	//	//    alias audiomodule_update update;
	//	//  }
	//	//}
	//
	//	void update();
	//}



	//public interface IAudio 
	//{
	//	//musicstate=null;
	//	//soundstate=null;
	//
	//	void update();
	//
	//	void bgm_play(string name, int volume = 80, float pitch = 100, int? position = null);
	//
	//	void bgm_fade(int ms);
	//
	//	void bgm_stop();
	//
	//	int bgm_position();
	//
	//	void me_play(string name, int volume = 80, float pitch = 100);
	//
	//	void me_fade(int ms);
	//
	//	void me_stop();
	//
	//	void bgs_play(string name, int volume = 80, float pitch = 100);
	//
	//	void bgs_fade(int ms);
	//
	//	void bgs_stop();
	//}
}