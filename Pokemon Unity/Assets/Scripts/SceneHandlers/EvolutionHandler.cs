//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EvolutionHandler : MonoBehaviour
{
    private DialogBoxHandlerNew dialog;
    private GUIParticleHandler particles;

    private int pokemonFrame = 0;
    private int evolutionFrame = 0;
    private Sprite[] pokemonSpriteAnimation;
    private Sprite[] evolutionSpriteAnimation;
    private Image pokemonSprite;
    private Image evolutionSprite;

    private Image topBorder;
    private Image bottomBorder;
    private Image glow;

    private Pokemon selectedPokemon;
    private string evolutionMethod;
    private int evolutionID;

    public AudioClip
        evolutionBGM,
        evolvingClip,
        evolvedClip,
        forgetMoveClip;

    public Sprite smokeParticle;

    private bool stopAnimations = false;


    private bool evolving = true;
    private bool evolved = false;


    //Evolution Animation Coroutines
    private Coroutine c_animateEvolution;
    private Coroutine c_pokemonGlow;
    private Coroutine c_glowGrow;
    private Coroutine c_pokemonPulsate;
    private Coroutine c_glowPulsate;

    void Awake()
    {
        dialog = transform.GetComponent<DialogBoxHandlerNew>();
        particles = transform.GetComponent<GUIParticleHandler>();

        pokemonSprite = transform.Find("PokemonSprite").GetComponent<Image>();
        evolutionSprite = transform.Find("EvolutionSprite").GetComponent<Image>();

        topBorder = transform.Find("TopBorder").GetComponent<Image>();
        bottomBorder = transform.Find("BottomBorder").GetComponent<Image>();
        glow = transform.Find("Glow").GetComponent<Image>();
    }

    void Start()
    {
        gameObject.SetActive(false);
    }


    private IEnumerator animatePokemon()
    {
        pokemonFrame = 0;
        evolutionFrame = 0;
        while (true)
        {
            if (pokemonSpriteAnimation.Length > 0)
            {
                if (pokemonFrame < pokemonSpriteAnimation.Length - 1)
                {
                    pokemonFrame += 1;
                }
                else
                {
                    pokemonFrame = 0;
                }
                pokemonSprite.sprite = pokemonSpriteAnimation[pokemonFrame];
            }
            if (evolutionSpriteAnimation.Length > 0)
            {
                if (evolutionFrame < evolutionSpriteAnimation.Length - 1)
                {
                    evolutionFrame += 1;
                }
                else
                {
                    evolutionFrame = 0;
                }
                evolutionSprite.sprite = evolutionSpriteAnimation[evolutionFrame];
            }
            yield return new WaitForSeconds(0.08f);
        }
    }


    public IEnumerator control(Pokemon pokemonToEvolve, string methodOfEvolution)
    {
        selectedPokemon = pokemonToEvolve;
        evolutionMethod = methodOfEvolution;
        evolutionID = selectedPokemon.getEvolutionID(evolutionMethod);
        string selectedPokemonName = selectedPokemon.getName();

        pokemonSpriteAnimation = selectedPokemon.GetFrontAnim_();
        evolutionSpriteAnimation = Pokemon.GetFrontAnimFromID_(evolutionID, selectedPokemon.getGender(),
            selectedPokemon.getIsShiny());
        pokemonSprite.sprite = pokemonSpriteAnimation[0];
        evolutionSprite.sprite = evolutionSpriteAnimation[0];
        StartCoroutine(animatePokemon());

        pokemonSprite.rectTransform.sizeDelta = new Vector2(pokemonSprite.sprite.texture.width, pokemonSprite.sprite.texture.height);
        evolutionSprite.rectTransform.sizeDelta = new Vector2(0, 0);

        pokemonSprite.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        evolutionSprite.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        topBorder.rectTransform.sizeDelta = new Vector2(342, 0);
        bottomBorder.rectTransform.sizeDelta = new Vector2(342, 0);

        glow.rectTransform.sizeDelta = new Vector2(0, 0);

        stopAnimations = false;


        StartCoroutine(ScreenFade.main.Fade(true, 1f));
        yield return new WaitForSeconds(1f);

        dialog.DrawDialogBox();
        yield return StartCoroutine(dialog.DrawText("What?"));
        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
        {
            yield return null;
        }
        yield return StartCoroutine(dialog.DrawText("\n" + selectedPokemon.getName() + " is evolving!"));
        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
        {
            yield return null;
        }

        dialog.UndrawDialogBox();
        evolving = true;

        AudioClip cry = selectedPokemon.GetCry();
        SfxHandler.Play(cry);
        yield return new WaitForSeconds(cry.length);

        BgmHandler.main.PlayOverlay(evolutionBGM, 753100);
        yield return new WaitForSeconds(0.4f);

        c_animateEvolution = StartCoroutine(animateEvolution());
        SfxHandler.Play(evolvingClip);

        yield return new WaitForSeconds(0.4f);

        while (evolving)
        {
            if (Input.GetButtonDown("Back"))
            {
                evolving = false;
                StopCoroutine(c_animateEvolution);

                //fadeTime = sceneTransition.FadeOut();
                //yield return new WaitForSeconds(fadeTime);
                yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));

                stopAnimateEvolution();

                //fadeTime = sceneTransition.FadeIn();
                //yield return new WaitForSeconds(fadeTime);
                yield return StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));


                dialog.DrawDialogBox();
                yield return StartCoroutine(dialog.DrawText("Huh?"));
                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                yield return StartCoroutine(dialog.DrawText("\n" + selectedPokemon.getName() + " stopped evolving."));
                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                dialog.UndrawDialogBox();
            }

            yield return null;
        }

        if (evolved)
        {
            selectedPokemon.evolve(evolutionMethod);
            GlobalVariables.global.resetFollower();

            yield return new WaitForSeconds(3.2f);

            cry = selectedPokemon.GetCry();
            BgmHandler.main.PlayMFX(cry);
            yield return new WaitForSeconds(cry.length);
            AudioClip evoMFX = Resources.Load<AudioClip>("Audio/mfx/GetGreat");
            BgmHandler.main.PlayMFXConsecutive(evoMFX);

            dialog.DrawDialogBox();
            yield return StartCoroutine(dialog.DrawTextSilent("Congratulations!"));
            yield return new WaitForSeconds(0.8f);
            StartCoroutine(
                dialog.DrawTextSilent("\nYour " + selectedPokemonName + " evolved into " +
                                      PokemonDatabase.getPokemon(evolutionID).getName() + "!"));

            //wait for MFX to stop
            float extraTime = (evoMFX.length - 0.8f > 0) ? evoMFX.length - 0.8f : 0;
            yield return new WaitForSeconds(extraTime);

            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }

            string newMove = selectedPokemon.MoveLearnedAtLevel(selectedPokemon.getLevel());
            if (!string.IsNullOrEmpty(newMove) && !selectedPokemon.HasMove(newMove))
            {
                yield return StartCoroutine(LearnMove(selectedPokemon, newMove));
            }

            dialog.UndrawDialogBox();
            bool running = true;
            while (running)
            {
                if (Input.GetButtonDown("Select") || Input.GetButtonDown("Back"))
                {
                    running = false;
                }

                yield return null;
            }

            yield return new WaitForSeconds(0.4f);
        }

        StartCoroutine(ScreenFade.main.Fade(false, 1f));
        BgmHandler.main.ResumeMain(1.4f, PlayerMovement.player.accessedMapSettings.getBGM());
        yield return new WaitForSeconds(1.2f);

        this.gameObject.SetActive(false);
    }


    private void stopAnimateEvolution()
    {
        stopAnimations = true;
        StopCoroutine(c_animateEvolution);
        if (c_glowGrow != null)
        {
            StopCoroutine(c_glowGrow);
        }
        if (c_glowPulsate != null)
        {
            StopCoroutine(c_glowPulsate);
        }
        if (c_pokemonGlow != null)
        {
            StopCoroutine(c_pokemonGlow);
        }
        if (c_pokemonPulsate != null)
        {
            StopCoroutine(c_pokemonPulsate);
        }
        particles.cancelAllParticles();
        glow.rectTransform.sizeDelta = new Vector2(0, 0);
        bottomBorder.rectTransform.sizeDelta = new Vector2(342, 0);
        topBorder.rectTransform.sizeDelta = new Vector2(342, 0);

        pokemonSprite.rectTransform.sizeDelta = new Vector2(pokemonSprite.sprite.texture.width, pokemonSprite.sprite.texture.height);
        evolutionSprite.rectTransform.sizeDelta = new Vector2(0, 0);
        pokemonSprite.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        BgmHandler.main.PlayOverlay(null, 0, 0.5f);
    }

    private IEnumerator animateEvolution()
    {
        StartCoroutine(smokeSpiral());
        StartCoroutine(borderDescend());
        yield return new WaitForSeconds(0.6f);
        c_pokemonGlow = StartCoroutine(pokemonGlow());
        yield return new WaitForSeconds(0.4f);
        c_glowGrow = StartCoroutine(glowGrow());
        yield return new WaitForSeconds(0.4f);
        evolutionSprite.color = new Color(1, 1, 1, 0.5f);
        c_pokemonPulsate = StartCoroutine(pokemonPulsate(19));
        yield return new WaitForSeconds(0.4f);
        yield return c_glowPulsate = StartCoroutine(glowPulsate(7));
        evolved = true;
        evolving = false;
        SfxHandler.Play(evolvedClip);
        StartCoroutine(glowDissipate());
        StartCoroutine(brightnessExplosion());
        StartCoroutine(borderRetract());
        yield return new WaitForSeconds(0.4f);
        yield return StartCoroutine(evolutionUnglow());
        yield return new WaitForSeconds(0.4f);
    }


    private IEnumerator brightnessExplosion()
    {
        //GlobalVariables.global.fadeTex = whiteTex;
        float speed = ScreenFade.slowedSpeed;
        StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.slowedSpeed, Color.white));

        float increment = 0f;
        while (increment < 1)
        {
            increment += (1f / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            yield return null;
        }
        StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.slowedSpeed, Color.white));
        //sceneTransition.FadeIn(1.2f);
    }

    private IEnumerator glowGrow()
    {
        float increment = 0f;
        float speed = 0.8f;
        float endSize = 96f;

        glow.color = new Color(0.8f, 0.8f, 0.5f, 0.2f);
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            glow.rectTransform.sizeDelta = new Vector2(endSize * increment, endSize * increment);
            yield return null;
        }
    }

    private IEnumerator glowDissipate()
    {
        float increment = 0f;
        float speed = 1.5f;
        float startSize = glow.rectTransform.sizeDelta.x;
        float sizeDifference = 280f - startSize;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            glow.rectTransform.sizeDelta = new Vector2(startSize + sizeDifference * increment,
                startSize + sizeDifference * increment);
            glow.color = new Color(glow.color.r, glow.color.g, glow.color.b, 0.2f - (0.2f * increment));
            yield return null;
        }
    }

    private IEnumerator glowPulsate(int repetitions)
    {
        float increment = 0f;
        float speed = 0.9f;
        float maxSize = 160f;
        float minSize = 96f;
        float sizeDifference = maxSize - minSize;
        bool glowShrunk = true;

        for (int i = 0; i < repetitions; i++)
        {
            increment = 0f;
            while (increment < 1)
            {
                increment += (1 / speed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                if (glowShrunk)
                {
                    glow.rectTransform.sizeDelta = new Vector2(minSize + sizeDifference * increment,
                        minSize + sizeDifference * increment);
                }
                else
                {
                    glow.rectTransform.sizeDelta = new Vector2(maxSize - sizeDifference * increment,
                        maxSize - sizeDifference * increment);
                }
                yield return null;
            }
            if (glowShrunk)
            {
                glowShrunk = false;
            }
            else
            {
                glowShrunk = true;
            }
        }
    }

    private IEnumerator pokemonPulsate(int repetitions)
    {
        float increment = 0f;
        float baseSpeed = 1.2f;
        float speed = baseSpeed;
        Vector3 originalPosition = pokemonSprite.transform.localPosition;
        Vector3 centerPosition = new Vector3(0.5f, 0.47f, originalPosition.z);
        float distance = centerPosition.y - originalPosition.y;
        bool originalPokemonShrunk = false;
        for (int i = 0; i < repetitions; i++)
        {
            increment = 0f;
            speed *= 0.85f;
            while (increment < 1)
            {
                if (speed < 0.15f)
                {
                    speed = 0.15f;
                }
                increment += (1 / speed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                if (originalPokemonShrunk)
                {
                    pokemonSprite.rectTransform.sizeDelta = new Vector2(pokemonSprite.sprite.texture.width * increment, pokemonSprite.sprite.texture.height * increment);
                    /*
                    pokemonSprite.transform.localPosition = new Vector3(originalPosition.x,
                        centerPosition.y - (distance * increment), originalPosition.z);
                        */
                    evolutionSprite.rectTransform.sizeDelta = new Vector2(evolutionSprite.sprite.texture.width - evolutionSprite.sprite.texture.width * increment,
                        evolutionSprite.sprite.texture.height - evolutionSprite.sprite.texture.height * increment);
                    /*
                    evolutionSprite.transform.localPosition = new Vector3(originalPosition.x,
                        originalPosition.y + (distance * increment), originalPosition.z);
                        */
                }
                else
                {
                    pokemonSprite.rectTransform.sizeDelta = new Vector2(pokemonSprite.sprite.texture.width - pokemonSprite.sprite.texture.width * increment, pokemonSprite.sprite.texture.height - pokemonSprite.sprite.texture.height * increment);
                    /*
                    pokemonSprite.transform.localPosition = new Vector3(originalPosition.x,
                        originalPosition.y + (distance * increment), originalPosition.z);
                        */
                    evolutionSprite.rectTransform.sizeDelta = new Vector2(evolutionSprite.sprite.texture.width * increment, evolutionSprite.sprite.texture.height * increment);
                    /*
                    evolutionSprite.transform.localPosition = new Vector3(originalPosition.x,
                        centerPosition.y - (distance * increment), originalPosition.z);
                        */
                }
                yield return null;
            }
            if (originalPokemonShrunk)
            {
                originalPokemonShrunk = false;
            }
            else
            {
                originalPokemonShrunk = true;
            }
        }
        pokemonSprite.transform.localPosition = originalPosition;
        evolutionSprite.transform.localPosition = originalPosition;
    }

    private IEnumerator pokemonGlow()
    {
        float increment = 0f;
        float speed = 1.8f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            pokemonSprite.color = new Color(0.5f + (0.5f * increment), 0.5f + (0.5f * increment),
                0.5f + (0.5f * increment), 0.5f);
            yield return null;
        }
    }

    private IEnumerator evolutionUnglow()
    {
        float increment = 0f;
        float speed = 1.8f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            evolutionSprite.color = new Color(1f - (0.5f * increment), 1f - (0.5f * increment), 1f - (0.5f * increment),
                0.5f);
            yield return null;
        }
    }

    private IEnumerator borderDescend()
    {
        float increment = 0f;
        float speed = 0.4f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            topBorder.rectTransform.sizeDelta = new Vector2(342, 40 * increment);
            bottomBorder.rectTransform.sizeDelta = new Vector2(342, 64 * increment);
            yield return null;
        }
    }

    private IEnumerator borderRetract()
    {
        float increment = 0f;
        float speed = 0.4f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            topBorder.rectTransform.sizeDelta = new Vector2(342, 40 - 40 * increment);
            bottomBorder.rectTransform.sizeDelta = new Vector2(342, 64 - 64 * increment);
            yield return null;
        }
    }

    private IEnumerator smokeSpiral()
    {
        StartCoroutine(smokeTrail(-1, 0.6f, 0.56f, 16f));
        yield return new WaitForSeconds(0.08f);
        StartCoroutine(smokeTrail(1, 0.36f, 0.44f, 18f));
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(smokeTrail(-1, 0.6f, 0.36f, 16f));
        yield return new WaitForSeconds(0.26f);
        StartCoroutine(smokeTrail(1, 0.36f, 0.54f, 16f));
        yield return new WaitForSeconds(0.08f);
        StartCoroutine(smokeTrail(-1, 0.6f, 0.42f, 18f));
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(smokeTrail(1, 0.36f, 0.34f, 16f));
        yield return new WaitForSeconds(0.26f);
        StartCoroutine(smokeTrail(-1, 0.6f, 0.52f, 16f));
        yield return new WaitForSeconds(0.08f);
        StartCoroutine(smokeTrail(1, 0.36f, 0.4f, 18f));
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(smokeTrail(-1, 0.6f, 0.32f, 16f));
        yield return new WaitForSeconds(0.26f);
        StartCoroutine(smokeTrail(-1, 0.6f, 0.5f, 16f));
        yield return new WaitForSeconds(0.08f);
        StartCoroutine(smokeTrail(1, 0.36f, 0.38f, 18f));
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(smokeTrail(-1, 0.6f, 0.3f, 16f));
        yield return new WaitForSeconds(0.26f);
    }

    private IEnumerator smokeTrail(int direction, float positionX, float positionY, float maxSize)
    {
        float positionYmodified;
        float sizeModified;
        for (int i = 0; i < 8; i++)
        {
            positionYmodified = positionY + Random.Range(-0.03f, 0.03f);
            sizeModified = (((float) i / 7f) * maxSize + maxSize) / 2f;
            if (!stopAnimations)
            {
                Image particle = particles.createParticle(smokeParticle, ScaleToScreen(positionX, positionYmodified),
                    ScaleToScreen(positionX + Random.Range(0.01f, 0.04f), positionYmodified - 0.02f),
                    sizeModified, 0, 0.6f, 0, sizeModified * 0.33f);

                if (particle != null)
                {
                    particle.color = new Color((float) i / 7f * 0.7f, (float) i / 7f * 0.7f, (float) i / 7f * 0.7f,
                        0.3f + ((float) i / 7f * 0.3f));
                }
                else
                {
                    Debug.Log("Particle Discarded");
                }
            }
            if (direction > 0)
            {
                positionX += 0.03f;
            }
            else
            {
                positionX -= 0.03f;
            }
            positionY -= 0.0025f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private Vector2 ScaleToScreen(float x, float y)
    {
        Vector2 vector = new Vector2(x * 342f - 171f, (1 - y) * 192f - 96f);
        return vector;
    }


    private IEnumerator LearnMove(Pokemon selectedPokemon, string move)
    {
        int chosenIndex = 1;
        if (chosenIndex == 1)
        {
            bool learning = true;
            while (learning)
            {
                //Moveset is full
                if (selectedPokemon.getMoveCount() == 4)
                {
                    dialog.DrawDialogBox();
                    yield return
                        StartCoroutine(
                            dialog.DrawText(selectedPokemon.getName() + " wants to learn the \nmove " + move + "."));
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    dialog.DrawDialogBox();
                    yield return
                        StartCoroutine(
                            dialog.DrawText("However, " + selectedPokemon.getName() + " already \nknows four moves."));
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    dialog.DrawDialogBox();
                    yield return
                        StartCoroutine(dialog.DrawText("Should a move be deleted and \nreplaced with " + move + "?"));

                    yield return StartCoroutine(dialog.DrawChoiceBox());
                    chosenIndex = dialog.chosenIndex;
                    dialog.UndrawChoiceBox();
                    if (chosenIndex == 1)
                    {
                        dialog.DrawDialogBox();
                        yield return StartCoroutine(dialog.DrawText("Which move should \nbe forgotten?"));
                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }

                        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));

                        //Set SceneSummary to be active so that it appears
                        Scene.main.Summary.gameObject.SetActive(true);
                        StartCoroutine(Scene.main.Summary.control(new Pokemon[] { selectedPokemon }, learning:learning, newMoveString:move));
                        //Start an empty loop that will only stop when SceneSummary is no longer active (is closed)
                        while (Scene.main.Summary.gameObject.activeSelf)
                        {
                            yield return null;
                        }

                        string replacedMove = Scene.main.Summary.replacedMove;
                        yield return StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));

                        if (!string.IsNullOrEmpty(replacedMove))
                        {
                            dialog.DrawDialogBox();
                            yield return StartCoroutine(dialog.DrawTextSilent("1, "));
                            yield return new WaitForSeconds(0.4f);
                            yield return StartCoroutine(dialog.DrawTextSilent("2, "));
                            yield return new WaitForSeconds(0.4f);
                            yield return StartCoroutine(dialog.DrawTextSilent("and... "));
                            yield return new WaitForSeconds(0.4f);
                            yield return StartCoroutine(dialog.DrawTextSilent("... "));
                            yield return new WaitForSeconds(0.4f);
                            yield return StartCoroutine(dialog.DrawTextSilent("... "));
                            yield return new WaitForSeconds(0.4f);
                            SfxHandler.Play(forgetMoveClip);
                            yield return StartCoroutine(dialog.DrawTextSilent("Poof!"));
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }

                            dialog.DrawDialogBox();
                            yield return
                                StartCoroutine(
                                    dialog.DrawText(selectedPokemon.getName() + " forgot how to \nuse " + replacedMove +
                                                    "."));
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                            dialog.DrawDialogBox();
                            yield return StartCoroutine(dialog.DrawText("And..."));
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }

                            dialog.DrawDialogBox();
                            AudioClip mfx = Resources.Load<AudioClip>("Audio/mfx/GetAverage");
                            BgmHandler.main.PlayMFX(mfx);
                            StartCoroutine(dialog.DrawTextSilent(selectedPokemon.getName() + " learned \n" + move + "!"));
                            yield return new WaitForSeconds(mfx.length);
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                            dialog.UndrawDialogBox();
                            learning = false;
                        }
                        else
                        {
                            //give up?
                            chosenIndex = 0;
                        }
                    }
                    if (chosenIndex == 0)
                    {
                        //NOT ELSE because this may need to run after (chosenIndex == 1) runs
                        dialog.DrawDialogBox();
                        yield return StartCoroutine(dialog.DrawText("Give up on learning the move \n" + move + "?"));

                        yield return StartCoroutine(dialog.DrawChoiceBox());
                        chosenIndex = dialog.chosenIndex;
                        dialog.UndrawChoiceBox();
                        if (chosenIndex == 1)
                        {
                            learning = false;
                            chosenIndex = 0;
                        }
                    }
                }
                //Moveset is not full, can fit the new move easily
                else
                {
                    selectedPokemon.addMove(move);

                    dialog.DrawDialogBox();
                    AudioClip mfx = Resources.Load<AudioClip>("Audio/mfx/GetAverage");
                    BgmHandler.main.PlayMFX(mfx);
                    StartCoroutine(dialog.DrawTextSilent(selectedPokemon.getName() + " learned \n" + move + "!"));
                    yield return new WaitForSeconds(mfx.length);
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    dialog.UndrawDialogBox();
                    learning = false;
                }
            }
        }
        if (chosenIndex == 0)
        {
            //NOT ELSE because this may need to run after (chosenIndex == 1) runs
            //cancel learning loop
            dialog.DrawDialogBox();
            yield return StartCoroutine(dialog.DrawText(selectedPokemon.getName() + " did not learn \n" + move + "."));
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }
        }
    }
}