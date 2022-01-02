using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonEssentials.Interface
{
	public interface IGameAudioPlay
	{
		IAudioObject pbStringToAudioFile(string str);

		/// <summary>
		/// Converts an object to an audio file. 
		/// </summary>
		/// <param name="str">Either a string showing the filename or an RPG.AudioFile object.
		/// Possible formats for _str_:
		/// filename                        volume and pitch 100
		/// filename:volume           pitch 100
		/// filename:volume:pitch
		/// </param>
		/// <param name="volume">Volume of the file, up to 100</param>
		/// <param name="pitch">Pitch of the file, normally 100</param>
		/// <returns></returns>
		IAudioObject pbResolveAudioFile(string str, int? volume = null, float? pitch = null);
		IAudioObject pbResolveAudioFile(IAudioObject str, int? volume = null, float? pitch = null);

		// ###############################################################################

		// Plays a BGM file.
		// param -- Either a string showing the filename 
		// (relative to Audio/BGM/) or an RPG.AudioFile object.
		// Possible formats for _param_:
		// filename                        volume and pitch 100
		// filename:volume           pitch 100
		// filename:volume:pitch
		// volume -- Volume of the file, up to 100
		// pitch -- Pitch of the file, normally 100
		void pbBGMPlay(string param, int? volume = null, float? pitch = null);
		void pbBGMPlay(IAudioObject param, int? volume = null, float? pitch = null);

		// Fades out or stops BGM playback. 'x' is the time in seconds to fade out.
		void pbBGMFade(float x = 0);

		// Fades out or stops BGM playback. 'x' is the time in seconds to fade out.
		void pbBGMStop(float timeInSeconds = 0);

		// ###############################################################################

		// Plays an ME file.
		// param -- Either a string showing the filename 
		// (relative to Audio/ME/) or an RPG.AudioFile object.
		// Possible formats for _param_:
		// filename                        volume and pitch 100
		// filename:volume           pitch 100
		// filename:volume:pitch
		// volume -- Volume of the file, up to 100
		// pitch -- Pitch of the file, normally 100
		void pbMEPlay(string param, int? volume = null, float? pitch = null);
		void pbMEPlay(IAudioObject param, int? volume = null, float? pitch = null);

		// Fades out or stops ME playback. 'x' is the time in seconds to fade out.
		void pbMEFade(float x = 0);

		// Fades out or stops ME playback. 'x' is the time in seconds to fade out.
		void pbMEStop(float timeInSeconds = 0);

		// ###############################################################################

		// Plays a BGS file.
		// param -- Either a string showing the filename 
		// (relative to Audio/BGS/) or an RPG.AudioFile object.
		// Possible formats for _param_:
		// filename                        volume and pitch 100
		// filename:volume           pitch 100
		// filename:volume:pitch
		// volume -- Volume of the file, up to 100
		// pitch -- Pitch of the file, normally 100
		void pbBGSPlay(string param, int? volume = null, float? pitch = null);
		void pbBGSPlay(IAudioObject param, int? volume = null, float? pitch = null);

		// Fades out or stops BGS playback. 'x' is the time in seconds to fade out.
		void pbBGSFade(float x = 0);

		// Fades out or stops BGS playback. 'x' is the time in seconds to fade out.
		void pbBGSStop(float timeInSeconds = 0);

		// ###############################################################################

		// Plays an SE file.
		// param -- Either a string showing the filename 
		// (relative to Audio/SE/) or an RPG.AudioFile object.
		// Possible formats for _param_:
		// filename                  volume and pitch 100
		// filename:volume           pitch 100
		// filename:volume:pitch
		// volume -- Volume of the file, up to 100
		// pitch -- Pitch of the file, normally 100
		void pbSEPlay(string param, int? volume = null, float? pitch = null);
		void pbSEPlay(IAudioObject param, int? volume = null, float? pitch = null);

		// Stops SE playback.
		void pbSEFade(float x = 0);

		// Stops SE playback.
		void pbSEStop(float timeInSeconds = 0);

		// ###############################################################################

		// Plays a sound effect that plays when the player moves the cursor.
		void pbPlayCursorSE();

		// Plays a sound effect that plays when a decision is confirmed or a choice is made.
		void pbPlayDecisionSE();

		// Plays a sound effect that plays when a choice is canceled.
		void pbPlayCancelSE();

		// Plays a buzzer sound effect.
		void pbPlayBuzzerSE();
	}
}