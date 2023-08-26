using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Track", menuName = "Pokemon Unity/Audio Track")]
public class AudioTrack : ScriptableObject, PokemonEssentials.Interface.IAudioObject {
    //[SerializeField] string Name;
    //[SerializeField] int volume;
    //[SerializeField] float pitch;

    public new string name { get; set; }
    public int volume { get; set; }
    public float pitch { get; set; }

    public AudioClip Clip = null;
    public int LoopStartSamples = 0;
    public int SamplesPosition = 0;

    public PokemonEssentials.Interface.IAudioObject initialize(string name, float volume = 100, float pitch = 100, AudioClip clip = null, int loopStartSamples = 0, int samplesPosition = 0) {
        this.name = name;
        this.volume = (int)volume;
        this.pitch = pitch;
        return this;
    }

    public static AudioTrack NullTrack => (AudioTrack)CreateInstance<AudioTrack>().initialize("Null Audio", 0, 0);
}