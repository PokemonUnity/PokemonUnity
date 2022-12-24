//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class InteractPC : MonoBehaviour
{
    private DialogBoxHandlerNew Dialog;

    //private SpriteRenderer spriteLight;

    private GameObject pc_off;

    private AudioSource PCaudio;

    public AudioClip offClip;
    public AudioClip onClip;
    public AudioClip openClip;
    public AudioClip selectClip;

    private Light PClight;

    void Awake()
    {
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandlerNew>();
        //spriteLight = transform.Find(gameObject.name + "_SpriteLight").GetComponent<SpriteRenderer>();
        pc_off = transform.Find("pc").gameObject;
        PCaudio = GetComponent<AudioSource>();
        PClight = GetComponentInChildren<Light>();
    }

    private IEnumerator onAnim()
    {
        float fadeSpeed = 0.17f;

        pc_off.SetActive(false);
        PClight.enabled = true;
        yield return new WaitForSeconds(fadeSpeed);
        pc_off.SetActive(true);
        PClight.enabled = false;
        yield return new WaitForSeconds(fadeSpeed);
        pc_off.SetActive(false);
        PClight.enabled = true;
        yield return new WaitForSeconds(fadeSpeed);
    }


    public IEnumerator interact()
    {
        if (PlayerMovement.player.direction == 0)
        {
            if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
            {
                SfxHandler.Play(onClip);
                yield return StartCoroutine("onAnim");
                Dialog.DrawDialogBox();
                yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( SaveData.currentSave.playerName + " turned on the PC!"));
                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                int accessedPC = -1;
                while (accessedPC != 0)
                {
                    Dialog.DrawDialogBox();
                    yield return Dialog.StartCoroutine(Dialog.DrawText( "Which PC should be accessed?"));
                    yield return StartCoroutine(Dialog.DrawChoiceBox(new string[] {"Someone's", "Switch off"}));
                    Dialog.UndrawChoiceBox();
                    accessedPC = Dialog.chosenIndex;
                    int accessedBox = -1;
                    if (accessedPC != 0)
                    {
                        //if not turning off computer
                        Dialog.DrawDialogBox();
                        SfxHandler.Play(openClip);
                        yield return
                            Dialog.StartCoroutine(Dialog.DrawTextSilent( "The Pokémon Storage System \\was accessed."));
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
                                "You may rearrange Pokémon in and \nbetween your party and Boxes.",
                                "Log out of the Pokémon Storage System."
                            };
                            
                            Dialog.DrawDialogBox();
                            Dialog.DrawTextInstant(choicesFlavour[0]);
                            yield return new WaitForSeconds(0.2f);
                            yield return StartCoroutine(Dialog.DrawChoiceBox(choices, choicesFlavour));
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

                            Dialog.UndrawChoiceBox();
                        }
                    }
                }
                Dialog.UndrawDialogBox();
                pc_off.SetActive(true);
                PClight.enabled = false;
                SfxHandler.Play(offClip);
                yield return new WaitForSeconds(0.2f);
                PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
            }
        }
    }
}
