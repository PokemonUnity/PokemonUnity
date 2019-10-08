//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class InteractSign : MonoBehaviour
{
    private DialogBoxHandler Dialog;

    public string signText;
    public Color signTint = new Color(0.5f, 0.5f, 0.5f, 1f);
    public DialogBoxHandler.PrintTextMethod printTextMethod = DialogBoxHandler.PrintTextMethod.Typewriter;

    // Use this for initialization
    void Awake()
    {
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandler>();
    }

    public IEnumerator interact()
    {
        if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
        {
            StartCoroutine(Dialog.drawSignBox(signTint));
            if (printTextMethod == DialogBoxHandler.PrintTextMethod.Typewriter)
            {
                StartCoroutine(Dialog.drawTextSilent(signText));
            }
            else if (printTextMethod == DialogBoxHandler.PrintTextMethod.Instant)
            {
                Dialog.drawTextInstant(signText);
            }

            yield return null;

            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back") &&
                   Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") >= 0)
            {
                yield return null;
            }

            StartCoroutine(Dialog.undrawSignBox());

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