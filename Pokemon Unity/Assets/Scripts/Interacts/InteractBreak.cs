//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class InteractBreak : MonoBehaviour
{
    private DialogBoxHandler Dialog;

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
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandler>();

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

    //public IEnumerator interact()
    //{
    //    PokemonOld targetPokemon = SaveDataOld.currentSave.PC.getFirstFEUserInParty(fieldEffect);
    //    if (targetPokemon != null)
    //    {
    //        if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
    //        {
    //            Dialog.drawDialogBox();
    //                //yield return StartCoroutine blocks the next code from running until coroutine is done.
    //            yield return Dialog.StartCoroutine("drawText", interactText);
    //            /* 			//This inactive code is used to print a third line of text.
    //                while(!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){	//these 3 lines stop the next bit from running until space is pressed.
    //                    yield return null;
    //                }
    //                Dialog.StartCoroutine("scrollText");
    //                yield return Dialog.StartCoroutine("drawText", "\\That'd be neat.");
    //        */
    //            Dialog.drawChoiceBox();
	//
    //            //You CAN NOT get a value from a Coroutine. As a result, the coroutine runs and resets a public int in it's own script.
    //            yield return Dialog.StartCoroutine(Dialog.choiceNavigate()); //it then assigns a value to that int
    //            Dialog.undrawChoiceBox();
    //            if (Dialog.chosenIndex == 1)
    //            {
    //                //check that int's value
	//
    //                Dialog.drawDialogBox();
    //                yield return
    //                    Dialog.StartCoroutine("drawText",
    //                        targetPokemon.getName() + " used " + targetPokemon.getFirstFEInstance(fieldEffect) + "!");
    //                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
    //                {
    //                    yield return null;
    //                }
    //                Dialog.undrawDialogBox();
	//
    //                yield return new WaitForSeconds(0.5f);
	//
    //                //Run the animation and remove the tree
    //                objectLight.enabled = true;
    //                if (!breakSound.isPlaying && !breaking)
    //                {
    //                    breakSound.volume = PlayerPrefs.GetFloat("sfxVolume");
    //                    breakSound.Play();
    //                }
    //                myAnimator.SetBool("break", true);
    //                myAnimator.SetBool("rewind", false);
    //                breaking = true;
	//
    //                yield return new WaitForSeconds(1f);
    //            }
    //            Dialog.undrawDialogBox();
    //            yield return new WaitForSeconds(0.2f);
    //            PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
    //        }
    //    }
    //    else
    //    {
    //        if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
    //        {
    //            Dialog.drawDialogBox();
    //                //yield return StartCoroutine blocks the next code from running until coroutine is done.
    //            yield return Dialog.StartCoroutine("drawText", examineText);
    //            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
    //            {
    //                yield return null;
    //            }
    //            Dialog.undrawDialogBox();
    //            yield return new WaitForSeconds(0.2f);
    //            PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
    //        }
    //    }
    //}

    public void repair()
    {
        myAnimator.SetBool("rewind", true);
        objectSprite.enabled = true;
        hitBox.enabled = true;
        breaking = false;
    }
}