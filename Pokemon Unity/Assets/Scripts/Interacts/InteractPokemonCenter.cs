//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class InteractPokemonCenter : MonoBehaviour
{
    private DialogBoxHandlerNew Dialog;

    public AudioClip ballPlaceClip;
    public AudioClip healMFX;

    public Sprite[] ballHealSprites = new Sprite[5];

    private NPCHandler nurse;
    private SpriteRenderer screenSprite;
    private Light screenLight;
    private SpriteRenderer[] pokeBalls = new SpriteRenderer[6];

    private AudioSource PokemonCenterAudio;

    void Awake()
    {
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandlerNew>();

        nurse = transform.Find("NPC_nurse").GetComponent<NPCHandler>();
        screenSprite =
            transform.Find("PokeCenterScreen").Find("Screen_SpriteLight").GetComponent<SpriteRenderer>();
        screenLight = transform.Find("PokeCenterScreen").Find("Screen_Light").GetComponent<Light>();
        Transform healMachine = transform.Find("healMachine").transform;
        for (int i = 0; i < 6; i++)
        {
            pokeBalls[i] = healMachine.Find("Ball" + i).GetComponent<SpriteRenderer>();
        }

        PokemonCenterAudio = transform.GetComponent<AudioSource>();
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
            bool followerOut = GlobalVariables.global.followerOut;
            
            for (int i = 0; i < 6; i++)
            {
                pokeBalls[i].enabled = false;
            }

            Dialog.DrawDialogBox();
            switch (Language.getLang())
            {
                case Language.Country.ENGLISH:
                    yield return StartCoroutine(Dialog.DrawText("Hello, and welcome to the Pokémon Center."));
                    break;
                case Language.Country.FRANCAIS:
                    yield return StartCoroutine(Dialog.DrawText("Bonjour, et bienvenue au Centre Pokémon."));
                    break;
            }
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }
            Dialog.DrawDialogBox();
            switch (Language.getLang())
            {
                case Language.Country.ENGLISH:
                    yield return StartCoroutine(Dialog.DrawText("We restore your tired Pokémon to full health."));
                    break;
                case Language.Country.FRANCAIS:
                    yield return StartCoroutine(Dialog.DrawText("Nous pouvons soigner vos Pokémon exténués."));
                    break;
            }
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }
            Dialog.DrawDialogBox();
            switch (Language.getLang())
            {
                case Language.Country.ENGLISH:
                    yield return StartCoroutine(Dialog.DrawText("Would you like to rest your Pokémon?"));
                    break;
                case Language.Country.FRANCAIS:
                    yield return StartCoroutine(Dialog.DrawText("Voulez-vous soigner vos Pokémon ?"));
                    break;
            }
            yield return StartCoroutine(Dialog.DrawChoiceBox());
            int chosenIndex = Dialog.chosenIndex;
            Dialog.UndrawChoiceBox();

            if (chosenIndex == 1)
            {
                Dialog.DrawDialogBox();
                switch (Language.getLang())
                {
                    case Language.Country.ENGLISH:
                        yield return StartCoroutine(Dialog.DrawText("Okay, I'll take your Pokémon for a few seconds."));
                        break;
                    case Language.Country.FRANCAIS:
                        yield return StartCoroutine(Dialog.DrawText("Très bien, laissez moi prendre vos Pokémon un instant"));
                        break;
                }
                yield return new WaitForSeconds(0.1f);
                if (followerOut)
                {
                    StartCoroutine(PlayerMovement.player.followerScript.withdrawToBall());
                    yield return new WaitForSeconds(0.5f);
                }

                nurse.setDirection(0);

                yield return new WaitForSeconds(0.2f);

                //place balls on machine, healing as they get shown
                for (int i = 0; i < 6; i++)
                {
                    if (SaveData.currentSave.PC.boxes[0][i] != null)
                    {
                        SaveData.currentSave.PC.boxes[0][i].healFull();
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

                Dialog.DrawDialogBox();
                switch (Language.getLang())
                {
                    case Language.Country.ENGLISH:
                        yield return StartCoroutine(Dialog.DrawText("Thank you for waiting."));
                        break;
                    case Language.Country.FRANCAIS:
                        yield return StartCoroutine(Dialog.DrawText("Merci d'avoir patienté."));
                        break;
                }
                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                Dialog.DrawDialogBox();
                switch (Language.getLang())
                {
                    case Language.Country.ENGLISH:
                        yield return StartCoroutine(Dialog.DrawText("We've restored your Pokémon to full health."));
                        break;
                    case Language.Country.FRANCAIS:
                        yield return StartCoroutine(Dialog.DrawText("Vos Pokémon sont en pleine forme."));
                        break;
                }
                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                if (followerOut)
                {
                    StartCoroutine(PlayerMovement.player.followerScript.releaseFromBall());
                }
                PlayerMovement.player.followerScript.canMove = true;
            }

            Dialog.DrawDialogBox();
            switch (Language.getLang())
            {
                case Language.Country.ENGLISH:
                    yield return StartCoroutine(Dialog.DrawText("We hope to see you again!"));
                    break;
                case Language.Country.FRANCAIS:
                    yield return StartCoroutine(Dialog.DrawText("Au plaisir de vous revoir !"));
                    break;
            }
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }

            Dialog.UndrawDialogBox();

            PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
        }
    }

    public IEnumerator respawnHeal()
    {
        if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
        {
            bool followerOut = GlobalVariables.global.followerOut;
            
            for (int i = 0; i < 6; i++)
            {
                pokeBalls[i].enabled = false;
            }

            yield return new WaitForSeconds(0.8f);

            Dialog.DrawDialogBox();
            switch (Language.getLang())
            {
                case Language.Country.ENGLISH:
                    yield return StartCoroutine(Dialog.DrawText("First, let's restore your Pokémon to full health."));
                    break;
                case Language.Country.FRANCAIS:
                    yield return StartCoroutine(Dialog.DrawText("Tout d'abord, laissez moi soigner vos Pokémon."));
                    break;
            }
            yield return new WaitForSeconds(0.5f);

            nurse.setDirection(3);

            yield return new WaitForSeconds(0.2f);

            //place balls on machine, healing as they get shown
            for (int i = 0; i < 6; i++)
            {
                if (SaveData.currentSave.PC.boxes[0][i] != null)
                {
                    SaveData.currentSave.PC.boxes[0][i].healFull();
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

            Dialog.DrawDialogBox();
            switch (Language.getLang())
            {
                case Language.Country.ENGLISH:
                    yield return StartCoroutine(Dialog.DrawText("Your Pokémon have been healed to perfect health."));
                    break;
                case Language.Country.FRANCAIS:
                    yield return StartCoroutine(Dialog.DrawText("Vos Pokémon sont maintenant en pleine forme."));
                    break;
            }
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }
            Dialog.DrawDialogBox();
            switch (Language.getLang())
            {
                case Language.Country.ENGLISH:
                    yield return
                        StartCoroutine(Dialog.DrawText("Please visit a Pokémon Center when your\nPokémon's HP goes down."));
                    break;
                case Language.Country.FRANCAIS:
                    yield return
                        StartCoroutine(Dialog.DrawText("Veuillez nous rendre visite si votre \néquipe tombe K.O."));
                    break;
            }
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }
            Dialog.DrawDialogBox();
            switch (Language.getLang())
            {
                case Language.Country.ENGLISH:
                    yield return
                        StartCoroutine(
                            Dialog.DrawText("If you're planning to travel any distance,\nyou should stock up on Potions."));
                    break;
                case Language.Country.FRANCAIS:
                    yield return
                        StartCoroutine(
                            Dialog.DrawText("Si vous comptez voyager, vous devriez acheter des Potions."));
                    break;
            }
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }

            PlayerMovement.player.followerScript.canMove = true;

            Dialog.DrawDialogBox();
            switch (Language.getLang())
            {
                case Language.Country.ENGLISH:
                    yield return StartCoroutine(Dialog.DrawText("Good luck, Trainer!"));
                    break;
                case Language.Country.FRANCAIS:
                    if (SaveData.currentSave.isMale)
                        yield return StartCoroutine(Dialog.DrawText("Bonne chance, dresseur !"));
                    else
                        yield return StartCoroutine(Dialog.DrawText("Bonne chance, dresseuse !"));
                    break;
            }
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }

            Dialog.UndrawDialogBox();

            if (followerOut)
            {
                yield return StartCoroutine(PlayerMovement.player.followerScript.releaseFromBall());
            }

            PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
        }
    }
}