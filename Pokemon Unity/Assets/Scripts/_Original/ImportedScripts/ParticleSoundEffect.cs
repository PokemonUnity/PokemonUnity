using System;
using UnityEngine;
using System.Collections;
 
public class ParticleSoundEffect : MonoBehaviour 
{
 
    public AudioClip SoundEffect;

    private bool effectPlayed = false;
    private float timeDelay = 0;
    private ParticleSystem parentSystem;
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.clip = SoundEffect;
        source.loop = false;
    }

    void Start() 
    {
        timeDelay = GetComponent<ParticleSystem>().main.startDelayMultiplier;
        parentSystem = transform.parent.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        source.volume = PlayerPrefs.GetFloat("sfxVolume");
    }

    void OnParticleCollision(GameObject other)
    {
        if (!Scene.main.Battle.gameObject.activeSelf && !BgmHandler.main.PlayingOverlay())
        {
            source.Play();
        }
    }
}
