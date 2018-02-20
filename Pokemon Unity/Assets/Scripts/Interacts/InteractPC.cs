//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class InteractPC : MonoBehaviour
{
    private DialogBoxHandler Dialog;

    private SpriteRenderer spriteLight;

    private AudioSource PCaudio;

    public AudioClip offClip;
    public AudioClip onClip;
    public AudioClip openClip;
    public AudioClip selectClip;

    private Light PClight;

    void Awake()
    {
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandler>();
        spriteLight = transform.Find(gameObject.name + "_SpriteLight").GetComponent<SpriteRenderer>();
        PCaudio = GetComponent<AudioSource>();
        PClight = GetComponentInChildren<Light>();
    }

    private IEnumerator onAnim()
    {
        float increment = 0;
        float fadeSpeed = 0.17f;
        float initialIntensity = PClight.intensity;
        while (increment < 1)
        {
            increment += (1 / fadeSpeed) * Time.deltaTime;
            spriteLight.color = new Color(1, 1, 1, increment);
            PClight.intensity = initialIntensity * increment;
            yield return null;
        }
        while (increment > 0)
        {
            increment -= (1 / fadeSpeed) * Time.deltaTime;
            spriteLight.color = new Color(1, 1, 1, increment);
            PClight.intensity = initialIntensity * increment;
            yield return null;
        }
        while (increment < 1)
        {
            increment += (1 / fadeSpeed) * Time.deltaTime;
            spriteLight.color = new Color(1, 1, 1, increment);
            PClight.intensity = initialIntensity * increment;
            yield return null;
        }
    }


    public IEnumerator interact()
    {
        if (PlayerMovement.player.direction == 0)
        {
            if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
            {
                spriteLight.enabled = true;
                PClight.enabled = true;
                SfxHandler.Play(onClip);
                yield return StartCoroutine("onAnim");
                Dialog.drawDialogBox();
                yield return Dialog.StartCoroutine("drawTextSilent", SaveData.currentSave.playerName + " turned on the PC!");
                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                int accessedPC = -1;
                while (accessedPC != 0)
                {
                    Dialog.drawDialogBox();
                    yield return Dialog.StartCoroutine("drawText", "Which PC should be accessed?");
                    Dialog.drawChoiceBox(new string[] {"Someone's", "Switch off"});
                    yield return Dialog.StartCoroutine("choiceNavigate");
                    Dialog.undrawChoiceBox();
                    accessedPC = Dialog.chosenIndex;
                    int accessedBox = -1;
                    if (accessedPC != 0)
                    {
                        //if not turning off computer
                        Dialog.drawDialogBox();
                        SfxHandler.Play(openClip);
                        yield return
                            Dialog.StartCoroutine("drawTextSilent", "The Pokémon Storage System \\was accessed.");
                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                        while (accessedBox != 0 && accessedPC != 0)
                        {
                            //if not turning off computer
                            string[] choices = new string[] {"Move", "Log off"};
                            string[] choicesFlavour = new string[]
                            {
                                "You may rearrange Pokémon in and \\between your party and Boxes.",
                                "Log out of the Pokémon Storage \\System."
                            };
                            Dialog.drawChoiceBox(choices);
                            Dialog.drawDialogBox();
                            Dialog.drawTextInstant(choicesFlavour[0]);
                            yield return new WaitForSeconds(0.2f);
                            yield return StartCoroutine(Dialog.choiceNavigate(choices, choicesFlavour));
                            accessedBox = Dialog.chosenIndex;
                            //SceneTransition sceneTransition = Dialog.transform.GetComponent<SceneTransition>();

                            if (accessedBox == 1)
                            {
                                //access Move
                                SfxHandler.Play(selectClip);
                                StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));
                                yield return new WaitForSeconds(ScreenFade.defaultSpeed + 0.4f);
                                //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f) + 0.4f);
                                SfxHandler.Play(openClip);
                                //Set ScenePC to be active so that it appears
                                Scene.main.PC.gameObject.SetActive(true);
                                StartCoroutine(Scene.main.PC.control());
                                //Start an empty loop that will only stop when ScenePC is no longer active (is closed)
                                while (Scene.main.PC.gameObject.activeSelf)
                                {
                                    yield return null;
                                }
                                yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                                //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                            }

                            Dialog.undrawChoiceBox();
                        }
                    }
                }
                Dialog.undrawDialogBox();
                spriteLight.enabled = false;
                PClight.enabled = false;
                SfxHandler.Play(offClip);
                yield return new WaitForSeconds(0.2f);
                PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
            }
        }
    }
}
