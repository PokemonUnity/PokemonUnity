using PokemonEssentials.Interface;
using UnityEngine;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// </summary>
	/// Maybe a ScriptableObject, to store the recorded audio?
	[RequireComponent(typeof(AudioClip))]
	public class MicrophoneRecorder : global::UnityEngine.MonoBehaviour, IWaveData
	{
		public AudioClip recordedClip;
		/// <summary>
		/// Average loudness or power of the sound data.
		/// </summary>
		public byte intensity { get; private set; } // Placeholder for actual intensity calculation
		/// <summary>
		///  Time in milliseconds
		/// </summary>
		public int time { get { return recordedClip ? (int)(recordedClip.length * 1000) : 0; } }

		/// <summary>
		/// Play the recorded audio
		/// </summary>
		public void play()
		{
			if (recordedClip)
			{
				AudioSource audioSource = GetComponent<AudioSource>();
				if (!audioSource)
				{
					audioSource = gameObject.AddComponent<AudioSource>();
				}
				audioSource.clip = recordedClip;
				audioSource.Play();
			}
			else
			{
				Core.Logger?.LogError("No recorded audio clip available.");
			}
		}
		#region Microphone Recording Logic
		//ToDo: Should move to audio manager class, instead of being on recorded audio track?
		/// <summary>
		/// Start recording from the default microphone
		/// </summary>
		/// <param name="recordLengthSeconds"></param>
		public void StartRecording(float recordLengthSeconds = 10)
		{
			if (Microphone.devices.Length > 0)
			{
				// Microphone available
				recordedClip = Microphone.Start(null, false, Mathf.CeilToInt(recordLengthSeconds), 44100);
			}
			else
			{
				// Microphone not available - handle error or request permission
				Core.Logger?.LogError("Failed to start recording: Microphone not available");
			}
		}

		/// <summary>
		/// Stop recording and store the AudioClip
		/// </summary>
		public void StopRecording()
		{
			if (Microphone.IsRecording(null))
			{
				Microphone.End(null);
			}
		}
		#endregion

		/// <summary>
		/// Example of calculating intensity (simplified version)
		/// </summary>
		private void CalculateIntensity()
		{
			if (recordedClip)
			{
				float[] samples = new float[recordedClip.samples * recordedClip.channels];
				recordedClip.GetData(samples, 0);
				float sum = 0;
				for (int i = 0; i < samples.Length; i++)
				{
					sum += samples[i] * samples[i]; // Square the sample
				}
				intensity = (byte)(Mathf.Sqrt(sum / samples.Length) * 256); // Normalize to byte
			}
		}

		/// <summary>
		/// Unity doesn't directly support saving AudioClip data to a file out of the box.
		/// </summary>
		/// <param name="clip"></param>
		/// <param name="path"></param>
		public static void SaveToWav(AudioClip clip, string path)
		{
			// This method would need to implement WAV file formatting and saving logic
			// refer to online resources or libraries that support audio file handling
		}
	}
}