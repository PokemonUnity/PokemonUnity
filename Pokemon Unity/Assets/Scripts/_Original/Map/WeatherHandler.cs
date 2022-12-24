using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherHandler : MonoBehaviour
{
    public static Weather currentWeather;
    public static Weather nextWeather;
    
    public bool fade;

    public bool disable;

    private GameObject currentParticleSystem;

    private AudioSource audioSource;
    private float transitionSpeed = 100;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        currentWeather = null;
        audioSource.volume = PlayerPrefs.GetFloat("sfxVolume");
    }

    void Update()
    {
        //Update Audio Volume
        if (currentWeather == null || currentWeather.ambientSound == null || fade)
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, 0, Time.deltaTime);
        }
        else if (Scene.main.Battle.gameObject.activeSelf || BgmHandler.main.PlayingOverlay() || disable)
        {
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, PlayerPrefs.GetFloat("sfxVolume"), Time.deltaTime);
        }
        
        
        //Update weather values
        if (currentWeather != null)
        {
            if (Scene.main.Battle.gameObject.activeSelf || disable)
            {
                RenderSettings.fog = false;
            }
            else
            {
                RenderSettings.fog = currentWeather.fogEnabled;
            }
            
            if (currentWeather.fogEnabled)
            {
                RenderSettings.fogColor = currentWeather.fogSettings.color;
                RenderSettings.fogMode = currentWeather.fogSettings.mode;
                RenderSettings.fogStartDistance = Mathf.MoveTowards(RenderSettings.fogStartDistance, currentWeather.fogSettings.start, Time.deltaTime * transitionSpeed);
                RenderSettings.fogEndDistance = Mathf.MoveTowards(RenderSettings.fogEndDistance, currentWeather.fogSettings.end, Time.deltaTime * transitionSpeed);
            }
        }
        else
        {
            float startTarget = 100;
            float endTarget = 200;
            float epsilon = 0.1f;
            
            RenderSettings.fogStartDistance = Mathf.MoveTowards(RenderSettings.fogStartDistance, startTarget, Time.deltaTime * transitionSpeed) ;
            RenderSettings.fogEndDistance = Mathf.MoveTowards(RenderSettings.fogEndDistance, endTarget, Time.deltaTime * transitionSpeed) ;

            if (Math.Abs(RenderSettings.fogStartDistance - startTarget) < epsilon && Math.Abs(RenderSettings.fogEndDistance - endTarget) < epsilon)
            {
                RenderSettings.fog = false;
            }
            
            RenderSettings.fogStartDistance = Mathf.MoveTowards(RenderSettings.fogStartDistance, 50, Time.deltaTime * transitionSpeed) ;
        }

        if (currentParticleSystem != null)
        {
            currentParticleSystem.transform.position = PlayerMovement.player.transform.position;
        }
    }

    public void setWeather(Weather weather)
    {
        nextWeather = weather;
        
        initWeather();
    }
    
    public void setWeatherValue(Weather weather)
    {
        nextWeather = weather;

        if (weather != null)
        {
            RenderSettings.fogStartDistance = weather.fogSettings.start;
            RenderSettings.fogEndDistance = weather.fogSettings.end;
        }
        
        initWeather();
    }

    private void initWeather()
    {
        if (currentWeather != nextWeather)
        {
            // Stop current weather
            if (currentWeather != null) {
                if (currentWeather.particle != null)
                {
                    // Stop Particle System
                    Destroy(currentParticleSystem);
                }
            
                if (currentWeather.ambientSound != null)
                {
                    // Stop Sound
                    if (nextWeather != null && nextWeather.ambientSound != null)
                    {
                        audioSource.Stop();
                    }
                }
            }
        
            currentWeather = nextWeather;

            // Start new weather
            if (currentWeather != null)
            {
                if (currentWeather.particle != null)
                {
                    // Start Particle System
                    currentParticleSystem = Instantiate(currentWeather.particle, transform);
                }
            
                if (currentWeather.ambientSound != null)
                {
                    // Start Sound
                    audioSource.clip = currentWeather.ambientSound;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
        }
        nextWeather = null;
    }

    public static void fadeSound()
    {
        if (GameObject.Find("Weather") != null)
        {
            GameObject.Find("Weather").GetComponent<WeatherHandler>().fade = true;
        }
    } 
}
