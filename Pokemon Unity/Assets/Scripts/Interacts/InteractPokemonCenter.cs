//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class InteractPokemonCenter : MonoBehaviour
{
    private DialogBoxHandler Dialog;

    public AudioClip ballPlaceClip;
    public AudioClip healMFX;

    public Sprite[] ballHealSprites = new Sprite[5];

    private NPCHandler nurse;
    private SpriteRenderer screenSprite;
    private Light screenLight;
    private SpriteRenderer[] pokeBalls = new SpriteRenderer[6];

    //private AudioSource PokemonCenterAudio;

    void Awake()
    {
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandler>();

        nurse = transform.Find("NPC_nurse").GetComponent<NPCHandler>();
        screenSprite =
            transform.Find("PokeCenterScreen").Find("Screen_SpriteLight").GetComponent<SpriteRenderer>();
        screenLight = transform.Find("PokeCenterScreen").Find("Screen_Light").GetComponent<Light>();
        Transform healMachine = transform.Find("healMachine").transform;
        for (int i = 0; i < 6; i++)
        {
            pokeBalls[i] = healMachine.Find("Ball" + i).GetComponent<SpriteRenderer>();
        }

        //PokemonCenterAudio = transform.GetComponent<AudioSource>();
    }


    private IEnumerator flashScreen(float speed)
    {
        float increment = 0;
        float initialIntensity = screenLight.intensity;
        screenSprite.enabled = true;
        screenLight.enabled = true;
        while (increment < 1)
        {
            increment += (1f / (speed / 5f * 2f)) * Time.deltaTime;
            screenSprite.color = new Color(1, 1, 1, increment);
            screenLight.intensity = initialIntensity * increment;
            yield return null;
        }
        yield return new WaitForSeconds(speed / 5f);
        while (increment > 0)
        {
            increment -= (1f / (speed / 5f * 2f)) * Time.deltaTime;
            screenSprite.color = new Color(1, 1, 1, increment);
            screenLight.intensity = initialIntensity * increment;
            yield return null;
        }
        screenSprite.enabled = false;
        screenLight.enabled = false;
    }


    public IEnumerator interact()
    {
        if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
        {
            for (int i = 0; i < 6; i++)
            {
                pokeBalls[i].enabled = false;
            }

            Dialog.drawDialogBox();
            yield return StartCoroutine(Dialog.drawText("Hello, and welcome to \nthe Pokémon Center."));
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }
            Dialog.drawDialogBox();
            yield return StartCoroutine(Dialog.drawText("We restore your tired Pokémon \nto full health."));
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }
            Dialog.drawDialogBox();
            yield return StartCoroutine(Dialog.drawText("Would you like to rest your Pokémon?"));
            Dialog.drawChoiceBox();
            yield return StartCoroutine(Dialog.choiceNavigate());
            int chosenIndex = Dialog.chosenIndex;
            Dialog.undrawChoiceBox();

            if (chosenIndex == 1)
            {
                Dialog.drawDialogBox();
                yield return StartCoroutine(Dialog.drawText("Okay, I'll take your Pokémon for \na few seconds."));
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(PlayerMovement.player.followerScript.withdrawToBall());
                yield return new WaitForSeconds(0.5f);

                nurse.setDirection(3);

                yield return new WaitForSeconds(0.2f);

                //place balls on machine, healing as they get shown
                for (int i = 0; i < 6; i++)
                {
                    if (SaveDataOld.currentSave.PC.boxes[0][i] != null)
                    {
                        SaveDataOld.currentSave.PC.boxes[0][i].healFull();
                        pokeBalls[i].enabled = true;
                        SfxHandler.Play(ballPlaceClip);
                        yield return new WaitForSeconds(0.45f);
                    }
                }
                yield return new WaitForSeconds(0.25f);

                BgmHandler.main.PlayMFX(healMFX);
                //animate the balls to glow 4 times
                for (int r = 0; r < 4; r++)
                {
                    StartCoroutine(flashScreen(0.45f));
                    for (int i = 0; i < 5; i++)
                    {
                        for (int i2 = 0; i2 < 6; i2++)
                        {
                            pokeBalls[i2].sprite = ballHealSprites[i];
                        }
                        yield return new WaitForSeconds(0.09f);
                    }
                }

                //reset the ball sprites
                for (int i = 0; i < 6; i++)
                {
                    pokeBalls[i].sprite = ballHealSprites[0];
                }
                yield return new WaitForSeconds(1f);

                //remove the balls from the machine
                for (int i = 0; i < 6; i++)
                {
                    pokeBalls[i].enabled = false;
                }

                yield return new WaitForSeconds(0.2f);

                nurse.setDirection(2);

                Dialog.drawDialogBox();
                yield return StartCoroutine(Dialog.drawText("Thank you for waiting."));
                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                Dialog.drawDialogBox();
                yield return StartCoroutine(Dialog.drawText("We've restored your Pokémon \nto full health."));
                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }

                PlayerMovement.player.followerScript.canMove = true;
            }

            Dialog.drawDialogBox();
            yield return StartCoroutine(Dialog.drawText("We hope to see you again!"));
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }

            Dialog.undrawDialogBox();

            PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
        }
    }

    public IEnumerator respawnHeal()
    {
        if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
        {
            for (int i = 0; i < 6; i++)
            {
                pokeBalls[i].enabled = false;
            }

            yield return new WaitForSeconds(0.8f);

            Dialog.drawDialogBox();
            yield return StartCoroutine(Dialog.drawText("First, let's restore your Pokémon\nto full health."));
            yield return new WaitForSeconds(0.5f);

            nurse.setDirection(3);

            yield return new WaitForSeconds(0.2f);

            //place balls on machine, healing as they get shown
            for (int i = 0; i < 6; i++)
            {
                if (SaveDataOld.currentSave.PC.boxes[0][i] != null)
                {
                    SaveDataOld.currentSave.PC.boxes[0][i].healFull();
                    pokeBalls[i].enabled = true;
                    SfxHandler.Play(ballPlaceClip);
                    yield return new WaitForSeconds(0.45f);
                }
            }
            yield return new WaitForSeconds(0.25f);

            BgmHandler.main.PlayMFX(healMFX);
            //animate the balls to glow 4 times
            for (int r = 0; r < 4; r++)
            {
                StartCoroutine(flashScreen(0.45f));
                for (int i = 0; i < 5; i++)
                {
                    for (int i2 = 0; i2 < 6; i2++)
                    {
                        pokeBalls[i2].sprite = ballHealSprites[i];
                    }
                    yield return new WaitForSeconds(0.09f);
                }
            }

            //reset the ball sprites
            for (int i = 0; i < 6; i++)
            {
                pokeBalls[i].sprite = ballHealSprites[0];
            }
            yield return new WaitForSeconds(1f);

            //remove the balls from the machine
            for (int i = 0; i < 6; i++)
            {
                pokeBalls[i].enabled = false;
            }

            yield return new WaitForSeconds(0.2f);

            nurse.setDirection(2);

            Dialog.drawDialogBox();
            yield return StartCoroutine(Dialog.drawText("Your Pokémon have been healed to\nperfect health."));
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }
            Dialog.drawDialogBox();
            yield return
                StartCoroutine(Dialog.drawText("Please visit a Pokémon Center when your\nPokémon's HP goes down."));
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }
            Dialog.drawDialogBox();
            yield return
                StartCoroutine(
                    Dialog.drawText("If you're planning to travel any distance,\nyou should stock up on Potions."));
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }

            PlayerMovement.player.followerScript.canMove = true;

            Dialog.drawDialogBox();
            yield return StartCoroutine(Dialog.drawText("Good luck, Trainer!"));
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }

            Dialog.undrawDialogBox();

            PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
        }
    }
}