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
	/// <summary>
	/// Extension of <see cref="IGame"/>
	/// </summary>
	public interface IGameAudioPlay : IGame
	{
		IAudioObject StringToAudioFile(string str);

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
		IAudioObject ResolveAudioFile(string str, int? volume = null, float? pitch = null);
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
		IAudioObject ResolveAudioFile(IAudioObject str, int? volume = null, float? pitch = null);

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
		void BGMPlay(string param, int? volume = null, float? pitch = null);
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
		void BGMPlay(IAudioObject param, int? volume = null, float? pitch = null);

		/// <summary>
		/// Fades out or stops BGM playback.
		/// </summary>
		/// <param name="x">'x' is the time in seconds to fade out.</param>
		void BGMFade(float x = 0);

		/// <summary>
		/// Fades out or stops BGM playback.
		/// </summary>
		/// <param name="timeInSeconds">'x' is the time in seconds to fade out.</param>
		void BGMStop(float timeInSeconds = 0);

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
		void MEPlay(string param, int? volume = null, float? pitch = null);
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
		void MEPlay(IAudioObject param, int? volume = null, float? pitch = null);

		/// <summary>
		/// Fades out or stops ME playback.
		/// </summary>
		/// <param name="x">'x' is the time in seconds to fade out.</param>
		void MEFade(float x = 0);

		/// <summary>
		/// Fades out or stops ME playback.
		/// </summary>
		/// <param name="timeInSeconds">'x' is the time in seconds to fade out.</param>
		void MEStop(float timeInSeconds = 0);

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
		void BGSPlay(string param, int? volume = null, float? pitch = null);
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
		void BGSPlay(IAudioObject param, int? volume = null, float? pitch = null);

		/// <summary>
		/// Fades out or stops BGS playback.
		/// </summary>
		/// <param name="x">'x' is the time in seconds to fade out.</param>
		void BGSFade(float x = 0);

		/// <summary>
		/// Fades out or stops BGS playback.
		/// </summary>
		/// <param name="timeInSeconds">'x' is the time in seconds to fade out.</param>
		void BGSStop(float timeInSeconds = 0);

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
		void SEPlay(string param, int? volume = null, float? pitch = null);
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
		void SEPlay(IAudioObject param, int? volume = null, float? pitch = null);

		/// <summary>
		/// Stops SE playback.
		/// </summary>
		/// <param name="x"></param>
		void SEFade(float x = 0);

		/// <summary>
		/// Stops SE playback.
		/// </summary>
		/// <param name="timeInSeconds"></param>
		void SEStop(float timeInSeconds = 0);

		// ###############################################################################

		/// <summary>
		/// Plays a sound effect that plays when the player moves the cursor.
		/// </summary>
		void PlayCursorSE();

		/// <summary>
		/// Plays a sound effect that plays when a decision is confirmed or a choice is made.
		/// </summary>
		void PlayDecisionSE();

		/// <summary>
		/// Plays a sound effect that plays when a choice is canceled.
		/// </summary>
		void PlayCancelSE();

		/// <summary>
		/// Plays a buzzer sound effect.
		/// </summary>
		void PlayBuzzerSE();
	}
}