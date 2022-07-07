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
		/// Possible formats for <paramref name="str"/>:<para>
		/// filename                        volume and pitch 100</para><para>
		/// filename:volume           pitch 100</para>
		/// filename:volume:pitch
		/// </param>
		/// <param name="volume">Volume of the file, up to 100</param>
		/// <param name="pitch">Pitch of the file, normally 100</param>
		/// <returns></returns>
		IAudioObject pbResolveAudioFile(string str, int? volume = null, float? pitch = null);
		/// <summary>
		/// Converts an object to an audio file. 
		/// </summary>
		/// <param name="str">Either a string showing the filename or an RPG.AudioFile object.
		/// Possible formats for <paramref name="str"/>:<para>
		/// filename                        volume and pitch 100</para><para>
		/// filename:volume           pitch 100</para>
		/// filename:volume:pitch
		/// </param>
		/// <param name="volume">Volume of the file, up to 100</param>
		/// <param name="pitch">Pitch of the file, normally 100</param>
		/// <returns></returns>
		IAudioObject pbResolveAudioFile(IAudioObject str, int? volume = null, float? pitch = null);

		// ###############################################################################

		/// <summary>
		/// Plays a BGM file.
		/// </summary>
		/// <param name="param">Either a string showing the filename (relative to Audio/BGM/) or an RPG.AudioFile object.
		/// Possible formats for <paramref name="param"/>:<para>
		/// filename                        volume and pitch 100</para><para>
		/// filename:volume           pitch 100</para>
		/// filename:volume:pitch
		/// </param>
		/// <param name="volume">Volume of the file, up to 100</param>
		/// <param name="pitch">Pitch of the file, normally 100</param>
		void pbBGMPlay(string param, int? volume = null, float? pitch = null);
		/// <summary>
		/// Plays a BGM file.
		/// </summary>
		/// <param name="param">Either a string showing the filename (relative to Audio/BGM/) or an RPG.AudioFile object.
		/// Possible formats for <paramref name="param"/>:<para>
		/// filename                        volume and pitch 100</para><para>
		/// filename:volume           pitch 100</para>
		/// filename:volume:pitch
		/// </param>
		/// <param name="volume">Volume of the file, up to 100</param>
		/// <param name="pitch">Pitch of the file, normally 100</param>
		void pbBGMPlay(IAudioObject param, int? volume = null, float? pitch = null);

		/// <summary>
		/// Fades out or stops BGM playback. 
		/// </summary>
		/// <param name="x">'x' is the time in seconds to fade out.</param>
		void pbBGMFade(float x = 0);

		/// <summary>
		/// Fades out or stops BGM playback. 
		/// </summary>
		/// <param name="timeInSeconds">'x' is the time in seconds to fade out.</param>
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

		/// <summary>
		/// Fades out or stops ME playback. 
		/// </summary>
		/// <param name="x">'x' is the time in seconds to fade out.</param>
		void pbMEFade(float x = 0);

		/// <summary>
		/// Fades out or stops ME playback. 
		/// </summary>
		/// <param name="timeInSeconds">'x' is the time in seconds to fade out.</param>
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

		/// <summary>
		/// Fades out or stops BGS playback. 
		/// </summary>
		/// <param name="x">'x' is the time in seconds to fade out.</param>
		void pbBGSFade(float x = 0);

		/// <summary>
		/// Fades out or stops BGS playback. 
		/// </summary>
		/// <param name="timeInSeconds">'x' is the time in seconds to fade out.</param>
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

		/// <summary>
		/// Stops SE playback.
		/// </summary>
		/// <param name="x"></param>
		void pbSEFade(float x = 0);

		/// <summary>
		/// Stops SE playback.
		/// </summary>
		/// <param name="timeInSeconds"></param>
		void pbSEStop(float timeInSeconds = 0);

		// ###############################################################################

		/// <summary>
		/// Plays a sound effect that plays when the player moves the cursor.
		/// </summary>
		void pbPlayCursorSE();

		/// <summary>
		/// Plays a sound effect that plays when a decision is confirmed or a choice is made.
		/// </summary>
		void pbPlayDecisionSE();

		/// <summary>
		/// Plays a sound effect that plays when a choice is canceled.
		/// </summary>
		void pbPlayCancelSE();

		/// <summary>
		/// Plays a buzzer sound effect.
		/// </summary>
		void pbPlayBuzzerSE();
	}
}