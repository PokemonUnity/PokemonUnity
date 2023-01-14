//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class InteractShop : MonoBehaviour
{
    private DialogBoxHandlerNew Dialog;

    private NPCHandler thisNPC;
    public string interactDialog = "Welcome! \nWhat do you need?";
    public string returnDialog = "Is there anything else I may do\nfor you?";
    public string leaveDialog = "Please come again!";

    public string[] itemCatalog;
    //custom prices not yet implemented
    public int[] customPrices;


    void Awake()
    {
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandlerNew>();

        if (transform.GetComponent<NPCHandler>() != null)
        {
            thisNPC = transform.GetComponent<NPCHandler>();
        }
    }


    public IEnumerator interact()
    {
        if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
        {
            if (thisNPC != null)
            {
                int direction;
                //calculate player's position relative to this npc's and set direction accordingly.
                float xDistance = thisNPC.transform.position.x - PlayerMovement.player.transform.position.x;
                float zDistance = thisNPC.transform.position.z - PlayerMovement.player.transform.position.z;
                if (xDistance >= Mathf.Abs(zDistance))
                {
                    //Mathf.Abs() converts zDistance to a positive always.
                    direction = 3;
                } //this allows for better accuracy when checking orientation.
                else if (xDistance <= Mathf.Abs(zDistance) * -1)
                {
                    direction = 1;
                }
                else if (zDistance >= Mathf.Abs(xDistance))
                {
                    direction = 2;
                }
                else
                {
                    direction = 0;
                }
                thisNPC.setDirection(direction);
            }

            string[] choices = new string[]
            {
                "Shop", "Leave"
            };

            Dialog.DrawDialogBox();
            yield return StartCoroutine(Dialog.DrawText(interactDialog));
            yield return StartCoroutine(Dialog.DrawChoiceBox(choices));
            int chosenIndex = Dialog.chosenIndex;
            Dialog.UndrawChoiceBox();
            while (chosenIndex != 0)
            {
                if (chosenIndex == 1)
                {
                    Dialog.UndrawDialogBox();
                    yield return StartCoroutine(PlayerMovement.player.moveCameraTo(new Vector3(7, 0, 0), 0.35f));

                    Scene.main.Bag.gameObject.SetActive(true);
                    StartCoroutine(Scene.main.Bag.control(itemCatalog));

                    while (Scene.main.Bag.gameObject.activeSelf)
                    {
                        yield return null;
                    }

                    yield return StartCoroutine(PlayerMovement.player.moveCameraTo(new Vector3(0, 0, 0), 0.35f));
                }

                Dialog.DrawDialogBox();
                yield return StartCoroutine(Dialog.DrawText(returnDialog));
                yield return StartCoroutine(Dialog.DrawChoiceBox(choices));
                chosenIndex = Dialog.chosenIndex;
                Dialog.UndrawChoiceBox();
            }

            Dialog.DrawDialogBox();
            yield return StartCoroutine(Dialog.DrawText(leaveDialog));
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }
            Dialog.UndrawDialogBox();
        }
        PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
    }
}