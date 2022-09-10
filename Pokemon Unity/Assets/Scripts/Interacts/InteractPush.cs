//Original Scripts by IIColour (IIColour_Spectrum)

using System.Collections;
using UnityEngine;

public class InteractPush : MonoBehaviour
{
    private DialogBoxHandlerNew Dialog;
    private PlayerMovement Player;

    private AudioSource pushSound;

    public string examineText;
    public string interactText;
    public string examineTextStrengthActive;

    public float speed = 0.4f;

    private Transform hitBox;

    private Collider[] hitColliders;
    private Collider currentCollider;
    private Collider currentObjectCollider;
    private Collider currentInteraction;

    private MapCollider currentMap;
    private MapCollider destinationMap;

    public Vector3 startPosition;
    public Vector3 destinationPosition1;
    public Vector3 destinationPosition2;

    // Use this for initialization
    void Awake()
    {
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandlerNew>();

        pushSound = gameObject.GetComponent<AudioSource>();
        hitBox = transform.Find("Boulder_Object");
    }

    void Start()
    {
        Player = PlayerMovement.player;
    }

    public IEnumerator interact()
    {
        if (!Player.strength)
        {
            PokemonEssentials.Interface.PokeBattle.IPokemon targetPokemon = null; //SaveData.currentSave.PC.getFirstFEUserInParty("Strength");
            if (targetPokemon != null)
            {
                if (Player.setCheckBusyWith(this.gameObject))
                {
                    Dialog.DrawDialogBox();
                        //yield return StartCoroutine blocks the next code from running until coroutine is done.
                    yield return Dialog.StartCoroutine(Dialog.DrawText( interactText));

                    //You CAN NOT get a value from a Coroutine. As a result, the coroutine runs and resets a public int in it's own script.
                    yield return Dialog.StartCoroutine(Dialog.DrawChoiceBox()); //it then assigns a value to that int
                    Dialog.UndrawChoiceBox();
                    if (Dialog.chosenIndex == 1)
                    {
                        Dialog.DrawDialogBox();
                        yield return
                            Dialog.StartCoroutine(Dialog.DrawText(
                                //targetPokemon.Name + " used " + targetPokemon.getFirstFEInstance("Strength") + "!"));
                                targetPokemon.Name + " used " + "Strength" + "!"));
                        while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                        Dialog.UndrawDialogBox();

                        //Activate strength
                        Player.activateStrength();

                        yield return new WaitForSeconds(0.5f);
                    }
                    Dialog.UndrawDialogBox();
                }
            }
            else
            {
                if (Player.setCheckBusyWith(this.gameObject))
                {
                    Dialog.DrawDialogBox();
                        //yield return StartCoroutine blocks the next code from running until coroutine is done.
                    yield return Dialog.StartCoroutine(Dialog.DrawText( examineText));
                    while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.UndrawDialogBox();
                }
            }
        }
        else
        {
            if (Player.setCheckBusyWith(this.gameObject))
            {
                Dialog.DrawDialogBox();
                    //yield return StartCoroutine blocks the next code from running until coroutine is done.
                yield return Dialog.StartCoroutine(Dialog.DrawText( examineTextStrengthActive));
                while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                Dialog.UndrawDialogBox();
            }
        }
        yield return new WaitForSeconds(0.2f);
        Player.unsetCheckBusyWith(this.gameObject);
    }

    public IEnumerator bump()
    {
        if (Player.strength)
        {
            if (Player.setCheckBusyWith(this.gameObject))
            {
                Vector3 movement = new Vector3(0, 0, 0);

                if (Player.direction == 0)
                {
                    movement = new Vector3(0, 0, 2);
                }
                else if (Player.direction == 1)
                {
                    movement = new Vector3(2, 0, 0);
                }
                else if (Player.direction == 2)
                {
                    movement = new Vector3(0, 0, -1);
                }
                else if (Player.direction == 3)
                {
                    movement = new Vector3(-1, 0, 0);
                }

                if (checkDestination(movement))
                {
                    //if destination is clear
                    pushSound.volume = PlayerPrefs.GetFloat("sfxVolume");
                    pushSound.Play();
                    yield return StartCoroutine(move());
                }
                Player.unsetCheckBusyWith(this.gameObject);
            }
        }
    }

    private IEnumerator move()
    {
        Vector3 distance = destinationPosition1 - startPosition;
        float increment = 0;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
                //speed is determined by how many squares are crossed in one second
            if (increment > 1)
            {
                increment = 1;
            }
            hitBox.position = destinationPosition1;
            transform.position = startPosition + (distance * increment);
            yield return null;
        }
    }

    private bool checkDestination(Vector3 movement)
    {
        startPosition = transform.position;
        destinationPosition1 = startPosition + movement;

        int direction = 0;
        //Debug.Log("move: "+movement.x +" "+movement.z);
        if (Mathf.RoundToInt(movement.x) != 0)
        {
            // moving right or left
            direction = 1;
            destinationPosition2 = destinationPosition1 + new Vector3(0, 0, 1);
        }
        else if (Mathf.RoundToInt(movement.z) != 0)
        {
            //moving up or down
            destinationPosition2 = destinationPosition1 + new Vector3(1, 0, 0);
        }

        //Debug.Log("s: "+startPosition +" d1: "+destinationPosition1+" d2: "+destinationPosition2);

        //return a list of every collision at xyz position with a spherical radius of 0.4f
        hitColliders =
            Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z), 0.4f);

        //Check current map
        RaycastHit[] hitRays =
            Physics.RaycastAll(transform.position + Vector3.up + Player.getForwardVectorRaw(direction), Vector3.down, 3f);
        int closestIndex = -1;
        float closestDistance = float.PositiveInfinity;
        if (hitRays.Length > 0)
        {
            for (int i = 0; i < hitRays.Length; i++)
            {
                if (hitRays[i].collider.gameObject.GetComponent<MapCollider>() != null)
                {
                    if (hitRays[i].distance < closestDistance)
                    {
                        closestDistance = hitRays[i].distance;
                        closestIndex = i;
                    }
                }
            }
        }
        if (closestIndex != -1)
        {
            currentMap = hitRays[closestIndex].collider.gameObject.GetComponent<MapCollider>();

            //Check destiantion map
            hitRays = Physics.RaycastAll(transform.position + Vector3.up + Player.getForwardVectorRaw(direction),
                Vector3.down, 3f);
            closestIndex = -1;
            closestDistance = float.PositiveInfinity;
            if (hitRays.Length > 0)
            {
                for (int i = 0; i < hitRays.Length; i++)
                {
                    if (hitRays[i].collider.gameObject.GetComponent<MapCollider>() != null)
                    {
                        if (hitRays[i].distance < closestDistance)
                        {
                            closestDistance = hitRays[i].distance;
                            closestIndex = i;
                        }
                    }
                }
            }
            if (closestIndex != -1)
            {
                destinationMap = hitRays[closestIndex].collider.gameObject.GetComponent<MapCollider>();
            }
            else
            {
                destinationMap = currentMap;
            }


            //check destination for objects
            currentObjectCollider = null; //empty currentOBJC
            hitColliders = Physics.OverlapSphere(destinationPosition1, 0.25f);
            if (hitColliders.Length > 0)
            {
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].name.ToLowerInvariant().Contains("object"))
                    {
                        //if hits object
                        currentObjectCollider = hitColliders[i];
                    }
                }
            }
            if (currentObjectCollider == null)
            {
                hitColliders = Physics.OverlapSphere(destinationPosition2, 0.25f);
                if (hitColliders.Length > 0)
                {
                    for (int i = 0; i < hitColliders.Length; i++)
                    {
                        if (hitColliders[i].name.ToLowerInvariant().Contains("object"))
                        {
                            //if hits object
                            currentObjectCollider = hitColliders[i];
                        }
                    }
                }
            }
            if (currentObjectCollider == null)
            {
                //if both positions are free
                //ensure the slopes of the destination are both 0
                float slope1 = MapCollider.getSlopeOfPosition(destinationPosition1, direction);
                float slope2 = MapCollider.getSlopeOfPosition(destinationPosition2, direction);

                //Make sure that destination Position is at most a single square away from start.
                //this way we can ensure that the movement of the object will be one square at most
                movement = new Vector3(Mathf.Clamp(movement.x, -1, 1), Mathf.Clamp(movement.y, -1, 1),
                    Mathf.Clamp(movement.z, -1, 1));
                destinationPosition1 = startPosition + movement;

                if (slope1 == 0 && slope2 == 0)
                {
                    //if both squares in the destination are not impassable tiles
                    //Debug.Log (destinationPosition1);
                    if (destinationMap.getTileTag(destinationPosition1) != 1 &&
                        destinationMap.getTileTag(destinationPosition1) != 2 &&
                        destinationMap.getTileTag(destinationPosition2) != 1 &&
                        destinationMap.getTileTag(destinationPosition2) != 2)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}