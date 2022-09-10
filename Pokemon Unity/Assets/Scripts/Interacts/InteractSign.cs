//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class InteractSign : MonoBehaviour
{
    private DialogBoxHandlerNew Dialog;

    private string signText;
    public string en_text;
    public string fr_text;
    
    public Color signTint = new Color(0.5f, 0.5f, 0.5f, 1f);
    public DialogBoxHandlerNew.PrintTextMethod printTextMethod = DialogBoxHandlerNew.PrintTextMethod.Typewriter;

    // Use this for initialization
    void Awake()
    {
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandlerNew>();
    }

    public IEnumerator interact()
    {
        if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
        {
            Dialog.DrawSignBox(signTint);

            switch (Language.getLang())
            {
                default:
                    signText = en_text;
                    break;
                case Language.Country.FRANCAIS:
                    signText = fr_text;
                    break;
            }
            
            if (printTextMethod == DialogBoxHandlerNew.PrintTextMethod.Typewriter)
            {
                StartCoroutine(Dialog.DrawTextSilent(signText));
            }
            else if (printTextMethod == DialogBoxHandlerNew.PrintTextMethod.Instant)
            {
                StartCoroutine(Dialog.DrawTextInstantSilent(signText));
            }

            yield return null;

            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back") &&
                   Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") >= 0)
            {
                yield return null;
            }

            StartCoroutine(Dialog.UndrawSignBox());

            yield return null;
            PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
        }
    }

    public IEnumerator bump()
    {
        if (PlayerMovement.player.direction == 0)
        {
            yield return StartCoroutine(interact());
        }
    }
}