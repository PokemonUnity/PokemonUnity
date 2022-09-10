//Original Scripts by IIColour (IIColour_Spectrum)

using System.Collections;
using UnityEngine;

public class InteractBreak : MonoBehaviour
{
    private DialogBoxHandlerNew Dialog;

    private Animator myAnimator;
    private SpriteRenderer objectSprite;
    private Collider hitBox;
    private Light objectLight;

    private bool breaking = false;

    private AudioSource breakSound;

    public string examineText;
    public string interactText;
    public string fieldEffect;

    // Use this for initialization
    void Start()
    {
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandlerNew>();

        myAnimator = (Animator) this.GetComponentInChildren<Animator>();
        objectSprite = this.GetComponentInChildren<SpriteRenderer>();
        hitBox = this.GetComponentInChildren<BoxCollider>();

        objectLight = this.GetComponentInChildren<Light>();

        breakSound = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("breakEnd"))
        {
            objectSprite.enabled = false;
            objectLight.enabled = false;
        }
        else if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("finished"))
        {
            myAnimator.SetBool("break", false);
            objectSprite.enabled = false;
            hitBox.enabled = false;
            breaking = false;
        }
    }

    public IEnumerator interact()
    {
        PokemonEssentials.Interface.PokeBattle.IPokemon targetPokemon = null; //SaveData.currentSave.PC.getFirstFEUserInParty(fieldEffect);
        if (targetPokemon != null)
        {
            if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
            {
                Dialog.DrawDialogBox();
                    //yield return StartCoroutine blocks the next code from running until coroutine is done.
                yield return Dialog.StartCoroutine(Dialog.DrawText(interactText));
                /* 			//This inactive code is used to print a third line of text.
                    while(!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back")){	//these 3 lines stop the next bit from running until space is pressed.
                        yield return null;
                    }
                    Dialog.StartCoroutine("scrollText");
                    yield return Dialog.StartCoroutine(Dialog.DrawText( "\\That'd be neat.");
            */
                yield return StartCoroutine(Dialog.DrawChoiceBox());

                //You CAN NOT get a value from a Coroutine. As a result, the coroutine runs and resets a public int in it's own script.
                Dialog.UndrawChoiceBox();
                if (Dialog.chosenIndex == 1)
                {
                    //check that int's value

                    Dialog.DrawDialogBox();
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText( //To
                            //targetPokemon.Name + " used " + targetPokemon.getFirstFEInstance(fieldEffect) + "!"));
                            targetPokemon.Name + " used " + fieldEffect + "!"));
                    while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.UndrawDialogBox();

                    yield return new WaitForSeconds(0.5f);

                    //Run the animation and remove the tree
                    objectLight.enabled = true;
                    if (!breakSound.isPlaying && !breaking)
                    {
                        breakSound.volume = PlayerPrefs.GetFloat("sfxVolume");
                        breakSound.Play();
                    }
                    myAnimator.SetBool("break", true);
                    myAnimator.SetBool("rewind", false);
                    breaking = true;

                    yield return new WaitForSeconds(1f);
                }
                Dialog.UndrawDialogBox();
                yield return new WaitForSeconds(0.2f);
                PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
            }
        }
        else
        {
            if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
            {
                Dialog.DrawDialogBox();
                    //yield return StartCoroutine blocks the next code from running until coroutine is done.
                yield return Dialog.StartCoroutine(Dialog.DrawText( examineText));
                while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                Dialog.UndrawDialogBox();
                yield return new WaitForSeconds(0.2f);
                PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
            }
        }
    }

    public void repair()
    {
        myAnimator.SetBool("rewind", true);
        objectSprite.enabled = true;
        hitBox.enabled = true;
        breaking = false;
    }
}