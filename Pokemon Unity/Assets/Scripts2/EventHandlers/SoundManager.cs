using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SoundManager
{
	const float POKEMON_CRY_VOLUME_MULTIPLIER = 0.6F;

	private static Dictionary<string, AudioClip> _sounds = new Dictionary<string, AudioClip>();

	//public static float Volume = 1.0F;
	public static bool Muted = false;

	#region Unity Monobehavior
	public static AudioSource source { get; set; }
	private static IEnumerator PlaySound(AudioClip soundFile, /*float pitch, float pan,*/ float volume, bool stopMusic)
	{
		AudioClip originalClip = source.clip;
		int originalSamples = source.timeSamples;
		source.volume = volume;

		if (stopMusic && source.isPlaying)
		{
			//don't start new song until unpaused.
			source.Stop();
			//source.volume = volume;
			//source.clip = soundFile;
			//if (source.clip != null)
			//{
			//	source.timeSamples = originalSamples;
			//	originalSamples = 0;
			//}
			//source.Play();
		}

		source.timeSamples = 0;
		source.clip = soundFile;

		source.Play();

		while (source.isPlaying)
		{
			yield return null;
		}

		//continue previous audio that was cut off, here...
		//source.timeSamples = originalSamples;
		//source.clip = originalClip;
		//source.Play();
	}
	#endregion

	[System.Runtime.InteropServices.DllImport("winmm.dll")]
	private static extern int GetAudioOutputDevices();
	private static bool HasOutputDeviceAvailable()
	{
		return GetAudioOutputDevices() > 0;
	}

	public static void Clear()
	{
		_sounds.Clear();
	}

	public static void PlaySound(string soundFile)
	{
		PlaySound(soundFile, 0.0F, 0.0F, GameVariables.sVol, false);
	}

	public static void PlaySound(string soundFile, bool stopMusic)
	{
		PlaySound(soundFile, 0.0F, 0.0F, GameVariables.sVol, stopMusic);
	}

	public static void PlaySound(string soundFile, float pitch, float pan, float volume, bool stopMusic)
	{
		if (!Muted)
		{
			var key = soundFile.ToLowerInvariant();
			AudioClip sound = null;// TODO Change to default(_) if this is not a reference type
			if (!_sounds.TryGetValue(key, out sound))
			{

				// load sound
				var filePath = Path.Combine(GameController.GamePath, @"Content\Sounds", soundFile + ".wav");
				if (File.Exists(filePath))
				{
					using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate))
					{
						try
						{
							//sound = SoundEffect.FromStream(stream);
							sound = new AudioClip();
							_sounds.Add(key, sound);
						}
						catch (Exception ex)
						{
							GameVariables.DebugLog("Failed to load sound at \"" + soundFile + "\": " + ex.Message, true);
						}
					}
				}
			}

			if (sound != null)
			{
				if (HasOutputDeviceAvailable())
				{
					GameVariables.DebugLog("SoundEffect [" + soundFile + "]");

					//sound.Play(volume, pitch, pan)
					PlaySound(sound, volume, stopMusic);

					//if (stopMusic)
					//	MusicManager.PauseForSound(sound);
				}
				else
					GameVariables.DebugLog("Failed to play sound: no audio device available.", true);
			}
		}
	}

	public static void PlayPokemonCry(int pokemonId)
	{
		PlaySound("Cries/" + pokemonId.ToString(), 0F, 0F, GameVariables.sVol * POKEMON_CRY_VOLUME_MULTIPLIER, false);
	}

	public static void PlayPokemonCry(int pokemonId, float pitch, float pan)
	{
		PlaySound("Cries/" + pokemonId.ToString(), pitch, pan, GameVariables.sVol * POKEMON_CRY_VOLUME_MULTIPLIER, false);
	}

	public static void PlayPokemonCry(int pokemonId, float pitch, float pan, float volume)
	{
		PlaySound("Cries/" + pokemonId.ToString(), pitch, pan, volume * POKEMON_CRY_VOLUME_MULTIPLIER, false);
	}
}