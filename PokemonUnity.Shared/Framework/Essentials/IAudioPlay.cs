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
		/// <param name="str">a string showing the filename.
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
		/// <param name="str">an <seealso cref="IAudioObject"/> object.</param>
		/// <param name="volume">Volume of the file, up to 100</param>
		/// <param name="pitch">Pitch of the file, normally 100</param>
		/// <remarks>
		/// Possible formats for <paramref name="param"/>:<para>
		/// filename                        (volume and pitch 100)</para><para>
		/// filename:volume           (pitch 100)</para>
		/// filename:volume:pitch
		/// </remarks>
		/// <returns></returns>
		IAudioObject pbResolveAudioFile(IAudioObject str, int? volume = null, float? pitch = null);

		// ###############################################################################

		/// <summary>
		/// Plays a BGM file.
		/// </summary>
		/// <param name="param">a string showing the filename (relative to Audio/BGM/).</param>
		/// <param name="volume">Volume of the file, up to 100</param>
		/// <param name="pitch">Pitch of the file, normally 100</param>
		/// <remarks>
		/// Possible formats for <paramref name="param"/>:<para>
		/// filename                        (volume and pitch 100)</para><para>
		/// filename:volume           (pitch 100)</para>
		/// filename:volume:pitch
		/// </remarks>
		void pbBGMPlay(string param, int? volume = null, float? pitch = null);
		/// <summary>
		/// Plays a BGM file.
		/// </summary>
		/// <param name="param">an <seealso cref="IAudioObject"/> object.</param>
		/// <param name="volume">Volume of the file, up to 100</param>
		/// <param name="pitch">Pitch of the file, normally 100</param>
		/// <remarks>
		/// Possible formats for <paramref name="param"/>:<para>
		/// filename                        (volume and pitch 100)</para><para>
		/// filename:volume           (pitch 100)</para>
		/// filename:volume:pitch
		/// </remarks>
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

		/// <summary>
		/// Plays an ME file.
		/// </summary>
		/// <param name="param">a string showing the filename (relative to Audio/ME/).</param>
		/// <param name="volume">Volume of the file, up to 100</param>
		/// <param name="pitch">Pitch of the file, normally 100</param>
		/// <remarks>
		/// Possible formats for <paramref name="param"/>:<para>
		/// filename                        (volume and pitch 100)</para><para>
		/// filename:volume           (pitch 100)</para>
		/// filename:volume:pitch
		/// </remarks>
		void pbMEPlay(string param, int? volume = null, float? pitch = null);
		/// <summary>
		/// Plays an ME file.
		/// </summary>
		/// <param name="param">an <seealso cref="IAudioObject"/> object.</param>
		/// <param name="volume">Volume of the file, up to 100</param>
		/// <param name="pitch">Pitch of the file, normally 100</param>
		/// <remarks>
		/// Possible formats for <paramref name="param"/>:<para>
		/// filename                        (volume and pitch 100)</para><para>
		/// filename:volume           (pitch 100)</para>
		/// filename:volume:pitch
		/// </remarks>
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

		/// <summary>
		/// Plays a BGS file.
		/// </summary>
		/// <param name="param">a string showing the filename (relative to Audio/BGS/).</param>
		/// <param name="volume">Volume of the file, up to 100</param>
		/// <param name="pitch">Pitch of the file, normally 100</param>
		/// <remarks>
		/// Possible formats for <paramref name="param"/>:<para>
		/// filename                        (volume and pitch 100)</para><para>
		/// filename:volume           (pitch 100)</para>
		/// filename:volume:pitch
		/// </remarks>
		void pbBGSPlay(string param, int? volume = null, float? pitch = null);
		/// <summary>
		/// Plays a BGS file.
		/// </summary>
		/// <param name="param">an <seealso cref="IAudioObject"/> object.</param>
		/// <param name="volume">Volume of the file, up to 100</param>
		/// <param name="pitch">Pitch of the file, normally 100</param>
		/// <remarks>
		/// Possible formats for <paramref name="param"/>:<para>
		/// filename                        (volume and pitch 100)</para><para>
		/// filename:volume           (pitch 100)</para>
		/// filename:volume:pitch
		/// </remarks>
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

		/// <summary>
		/// Plays an SE file.
		/// </summary>
		/// <param name="param">a string showing the filename (relative to Audio/SE/).</param>
		/// <param name="volume">Volume of the file, up to 100</param>
		/// <param name="pitch">Pitch of the file, normally 100</param>
		/// <remarks>
		/// Possible formats for <paramref name="param"/>:<para>
		/// filename                        (volume and pitch 100)</para><para>
		/// filename:volume           (pitch 100)</para>
		/// filename:volume:pitch
		/// </remarks>
		void pbSEPlay(string param, int? volume = null, float? pitch = null);
		/// <summary>
		/// Plays an SE file.
		/// </summary>
		/// <param name="param">an <seealso cref="IAudioObject"/> object.</param>
		/// <param name="volume">Volume of the file, up to 100</param>
		/// <param name="pitch">Pitch of the file, normally 100</param>
		/// <remarks>
		/// Possible formats for <paramref name="param"/>:<para>
		/// filename                        (volume and pitch 100)</para><para>
		/// filename:volume           (pitch 100)</para>
		/// filename:volume:pitch
		/// </remarks>
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