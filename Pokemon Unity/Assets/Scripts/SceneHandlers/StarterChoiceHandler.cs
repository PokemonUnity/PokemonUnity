using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Localization;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;
using PokemonUnity.Utility;
using PokemonEssentials;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using UnityEngine;
using UnityEngine.Serialization;

public class StarterChoiceHandler : MonoBehaviour
{
    
    public SpriteRenderer[] starterSprites;
    public Vector3[] cameraPositions; // Local positions
    public float[] starterYPositions;

    public AudioClip pokeballOpen;
    public AudioClip fallClip, scrollClip;
    
    public int[] starterPokemonIds;
    
    
    private Camera camera;
    private GameObject display;

    private DialogBoxHandlerNew dialog;

    private void Start()
    {
        camera = transform.Find("StarterChoice_Camera").GetComponent<Camera>();
        dialog = GlobalVariables.global.transform.Find("GUI").GetComponent<DialogBoxHandlerNew>();
        display = GlobalVariables.global.transform.Find("MainCamera/MiscDisplay").gameObject;

        foreach (SpriteRenderer sprite in starterSprites)
        {
            LeanTween.moveLocalY(sprite.gameObject, starterYPositions[0], 0);
            LeanTween.scale(sprite.gameObject, Vector3.zero, 0);
        }

        LeanTween.moveLocal(camera.gameObject, cameraPositions[0], 0);

        display.SetActive(false);
        gameObject.SetActive(false);
    }
    
    private IEnumerator animatePokemon(SpriteRenderer pokemon, Sprite[] animation)
    {
        int frame = 0;
        while (animation != null)
        {
            if (animation.Length > 0)
            {
                if (frame < animation.Length - 1)
                {
                    frame++;
                }
                else
                {
                    frame = 0;
                }
                pokemon.sprite = animation[frame];
            }
            yield return new WaitForSeconds(0.08f);
        }
    }

    private IEnumerator releasePokemon(SpriteRenderer sprite, IPokemon pokemon)
    {
        float 
            spawnDuration = 0.5f,
            fallDuration = 0.3f;
        
        LeanTween.scale(sprite.gameObject, Vector3.one, spawnDuration);
        sprite.transform.Find("Pokeball Flash").GetComponent<ParticleSystem>().Play();
        SfxHandler.Play(pokeballOpen);
        yield return new WaitForSeconds(spawnDuration);

        yield return StartCoroutine(PlayCryAndWait(pokemon));

        LeanTween.moveLocalY(sprite.gameObject, starterYPositions[1], fallDuration);
        yield return new WaitForSeconds(fallDuration);

        SfxHandler.Play(fallClip);
        yield return StartCoroutine(shakeCamera(1, 0.1f));
        
        yield return null;
    }
    
    public IEnumerator shakeCamera(int iteration, float duration)
    {
        Transform camera = this.camera.transform;
        
        for (int i = 0; i < iteration; ++i)
        {
            float distance = 0.1f;
            Vector3 startPosition = camera.position;

            LeanTween.moveY(camera.gameObject, camera.position.y+distance, duration/4);
            yield return new WaitForSeconds(duration/4);
            LeanTween.moveY(camera.gameObject, camera.position.y-distance*2, duration/2);
            yield return new WaitForSeconds(duration/2);
            LeanTween.moveY(camera.gameObject, camera.position.y+distance, duration/4);
            yield return new WaitForSeconds(duration/4);
            
            camera.position = startPosition;
        }
        
        yield return null;
    }
    
    private float PlayCry(IPokemon pokemon)
    {
        //SfxHandler.Play(pokemon.GetCry(), pokemon.GetCryPitch());
        //return pokemon.GetCry().length / pokemon.GetCryPitch();
        return 0;
    }

    private IEnumerator PlayCryAndWait(IPokemon pokemon)
    {
        yield return new WaitForSeconds(PlayCry(pokemon));
    }

    public IEnumerator control()
    {
        display.SetActive(true);
        
        bool running = true;
        IPokemon[] starterPokemonList = new IPokemon[3];
        
        /* Setup Sprites */
        for (int i = 0; i < starterSprites.Length; ++i)
        {
            //starterPokemonList[i] = new Pokemon(starterPokemonIds[i], Pokemon.Gender.CALCULATE, 5, "Poké Ball", null, null, -1);
            starterPokemonList[i] = new PokemonUnity.Monster.Pokemon((Pokemons)starterPokemonIds[i], level: 5);
            //StartCoroutine(animatePokemon(starterSprites[i], starterPokemonList[i].GetFrontAnim_()));
        }

        /* Fade In */
        //TODO Fade
        
        /* Introduction Animation */
        
        yield return StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.slowedSpeed));

        yield return new WaitForSeconds(1);
        
        for (int i = 0; i < starterSprites.Length; ++i)
        {
            yield return StartCoroutine(releasePokemon(starterSprites[i], starterPokemonList[i]));
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.3f);

        LeanTween.moveLocal(camera.gameObject, cameraPositions[1], 0.4f);
        
        dialog.DrawBlackFrame();
        // TODO Language Variants
        dialog.DrawTextInstantSilentFunction("Which Pokémon will you choose ?");

        yield return new WaitForSeconds(0.4f);

        int starterIndex = 1;
        float inputDelay = 0.2f;
        
        while (running)
        {
            if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0) // Right 
            {
                if (starterIndex < starterSprites.Length - 1)
                {
                    starterIndex++;
                    SfxHandler.Play(scrollClip);
                    LeanTween.moveLocalX(camera.gameObject, starterSprites[starterIndex].transform.localPosition.x, inputDelay);
                    yield return new WaitForSeconds(inputDelay);
                }
            }
            else if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0) // Left 
            {
                if (starterIndex > 0)
                {
                    starterIndex--;
                    SfxHandler.Play(scrollClip);
                    LeanTween.moveLocalX(camera.gameObject, starterSprites[starterIndex].transform.localPosition.x, inputDelay);
                    yield return new WaitForSeconds(inputDelay);
                }
            }
            else if (UnityEngine.Input.GetButtonDown("Select"))
            {
                dialog.UndrawDialogBox();
                dialog.DrawBlackFrame();
                
                // TODO Language Variants

                string[] starterType =
                {
                    "Grass",
                    "Fire",
                    "Water"
                };
                
                yield return StartCoroutine(dialog.DrawTextSilent("Do you want to choose "+starterPokemonList[starterIndex].Name+" \nThe "+starterType[starterIndex]+" Pokémon?"));
                yield return StartCoroutine(dialog.DrawChoiceBox());

                if (dialog.chosenIndex == 1)
                {
                    SaveData.currentSave.setCVariable("starter", starterIndex+1);
                    
                    PlayCry(starterPokemonList[starterIndex]);
                    yield return new WaitForSeconds(0.3f);
                    
                    dialog.UndrawChoiceBox();
                    dialog.UndrawDialogBox();

                    running = false;
                }
                else
                {
                    dialog.UndrawChoiceBox();
                    dialog.UndrawDialogBox();
                    dialog.DrawBlackFrame();
                    dialog.DrawTextInstantSilentFunction("Which Pokémon will you choose ?");
                }
            }
            
            yield return null;
        }
        
        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.slowedSpeed));
        display.SetActive(false);
        
        yield return null;
    }
}
