//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Trainer))]
public class InteractTrainer : MonoBehaviour
{
    private Trainer trainer;

    public enum Gender
    {
        Unimportant,
        Male,
        Female
    }

    public enum TrainerBehaviour
    {
        Idle,
        Turn,
        Patrol
    }

    public Gender trainerGender;
    public TrainerBehaviour trainerBehaviour;

    public string trainerSpriteName;
    private Sprite[] spriteSheet;

    public AudioClip introBGM;
    public int samplesLoopStart;

    public int sightRange;
    public bool[] turnableDirections = new bool[4];
    public WalkCommand[] patrol = new WalkCommand[1];

    public string[] trainerConfrontDialog;
    public string[] trainerDefeatDialog;
    public string[] trainerPostDefeatDialog;

    public bool trainerSurfing = false;

    public bool busy = false;
    public bool defeated = false;
    private bool recentlyDefeated = false;
    //distinction between recent and general defeated, as a recently defeated trainer should stop patrolling/searching
    //for other trainers. A trainer you defeated in the not-so-recent past should be looking around, although they will
    //remember their defeat at your hands.


    private SpriteRenderer pawnSprite;
    private SpriteRenderer pawnReflectionSprite;
    public Transform hitBox;

    public int direction = 2;

    public MapCollider currentMap;
    public MapCollider destinationMap;

    private Transform sight;
    private GameObject exclaim;
    private BoxCollider[] sightCollider;
    private int frame;
    private int frames;
    private int framesPerSec;
    private float secPerFrame;
    private bool animPause = true;


    private DialogBoxHandler Dialog;
    //private SceneTransition sceneTransition;

    void Awake()
    {
        trainer = transform.GetComponent<Trainer>();

        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandler>();

        sight = transform.Find("Sight");
        sightCollider = sight.GetComponents<BoxCollider>();

        exclaim = transform.Find("Exclaim").gameObject;

        pawnSprite = transform.Find("Pawn").GetComponent<SpriteRenderer>();
        pawnReflectionSprite = transform.Find("PawnReflection").GetComponent<SpriteRenderer>();
        hitBox = transform.Find("NPC_Object");

        spriteSheet = Resources.LoadAll<Sprite>("OverworldNPCSprites/" + trainerSpriteName);
    }

    void Start()
    {
        hitBox.localPosition = new Vector3(0, 0, 0);
        sight.localPosition = new Vector3(0, 0, 0);

        exclaim.SetActive(false);

        StartCoroutine("animateSprite");


        //Check current map
        RaycastHit[] hitRays = Physics.RaycastAll(transform.position + Vector3.up, Vector3.down);
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
        }
        else
        {
            Debug.Log("no map found for: " + gameObject.name);
        }

        if (trainerBehaviour == TrainerBehaviour.Turn)
        {
            StartCoroutine("turnAtRandom");
        }
        else
        {
            turnableDirections[0] = false;
            turnableDirections[1] = false;
            turnableDirections[2] = false;
            turnableDirections[3] = false;
            if (trainerBehaviour == TrainerBehaviour.Patrol)
            {
                StartCoroutine("patrolAround");
            }
        }

        updateDirection(direction);
        StartCoroutine(refireSightColliders());
    }

    public void updateDirection(int newDirection)
    {
        direction = newDirection;
        if (trainerBehaviour == TrainerBehaviour.Turn)
        {
            StartCoroutine(updateSightColliders(true));
        }
        else
        {
            StartCoroutine(updateSightColliders(false));
        }
    }

    private IEnumerator updateSightColliders(bool refire)
    {
        float range = (float) sightRange;
        if (range < 1f)
        {
            range = 1f;
        }

        Vector3[] centers = new Vector3[]
        {
            new Vector3(0, 0.5f, 0.5f + (0.5f * (range - 1f))), new Vector3(0.5f + (0.5f * (range - 1f)), 0.5f, 0),
            new Vector3(0, 0.5f, -0.5f - (0.5f * (range - 1f))), new Vector3(-0.5f - (0.5f * (range - 1f)), 0.5f, 0)
        };
        Vector3[] sizes = new Vector3[]
        {
            new Vector3(0.5f, (range + 1) - 0.5f, (range + 1) - 0.5f),
            new Vector3((range + 1) - 0.5f, (range + 1) - 0.5f, 0.5f),
            new Vector3(0.5f, (range + 1) - 0.5f, (range + 1) - 0.5f),
            new Vector3((range + 1) - 0.5f, (range + 1) - 0.5f, 0.5f)
        };

        if (refire)
        {
            for (int i = 0; i < 4; i++)
            {
                sightCollider[i].center = new Vector3(0, 0, 0);
                sightCollider[i].size = new Vector3(0, 0, 0);
            }
            yield return null;
        }
        for (int i = 0; i < 4; i++)
        {
            sightCollider[i].center = new Vector3(0, 0, 0);
            sightCollider[i].size = new Vector3(0, 0, 0);
            if (trainerBehaviour == TrainerBehaviour.Turn)
            {
                if (turnableDirections[i] && sightRange > 0)
                {
                    sightCollider[i].center = centers[i];
                    sightCollider[i].size = sizes[i];
                }
            }
            else if (i == direction)
            {
                if (sightRange > 0)
                {
                    sightCollider[i].center = centers[i];
                    sightCollider[i].size = sizes[i];
                }
            }
        }
    }

    public IEnumerator refireSightColliders()
    {
        while (!defeated)
        {
            while (!busy)
            {
                StartCoroutine(updateSightColliders(true));
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }
    }

    //Better exclaimation not yet implemented
    private IEnumerator exclaimAnimation()
    {
        float increment = -1f;
        float speed = 0.15f;

        exclaim.SetActive(true);

        while (increment < 0.3f)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 0.3f)
            {
                increment = 0.3f;
            }
            exclaim.transform.localScale = new Vector3(1, 1.3f + (-1.3f * increment * increment), 1);
            yield return null;
        }

        exclaim.transform.localScale = new Vector3(1, 1, 1);

        yield return new WaitForSeconds(1.2f);
        exclaim.SetActive(false);
    }

    private IEnumerator animateSprite()
    {
        frame = 0;
        frames = 4;
        framesPerSec = 8;
        secPerFrame = 1f / framesPerSec;
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                while (PlayerMovement.player.busyWith != null && PlayerMovement.player.busyWith != this.gameObject)
                {
                    yield return null;
                }
                if (animPause && frame % 2 != 0)
                {
                    frame -= 1;
                }
                pawnSprite.sprite = spriteSheet[direction * frames + frame];
                pawnReflectionSprite.sprite = pawnSprite.sprite;
                yield return new WaitForSeconds(secPerFrame / 5);
            }
            if (!animPause)
            {
                frame += 1;
                if (frame >= frames)
                {
                    frame = 0;
                }
            }
            yield return null;
        }
    }

    private IEnumerator turnAtRandom()
    {
        float waitTime;
        while (!recentlyDefeated)
        {
            while (!busy)
            {
                waitTime = Random.Range(-0.8f, 1.6f);
                waitTime = 1.1f + (waitTime * waitTime * waitTime);

                turnableDirections[direction] = true; //ensure the current direction can't be turned off

                updateDirection(Random.Range(0, 4));
                while (!turnableDirections[direction])
                {
                    //ensure trainer is facing valid direction
                    updateDirection(Random.Range(0, 4));
                }
                yield return new WaitForSeconds(waitTime);
            }
            yield return null;
        }
    }

    private IEnumerator patrolAround()
    {
        Vector3 initialPosition = transform.position;

        while (!recentlyDefeated)
        {
            for (int i = 0; i < patrol.Length; i++)
            {
                while (busy)
                {
                    yield return null;
                }
                if (!recentlyDefeated)
                {
                    updateDirection(patrol[i].direction);
                    yield return null; //2 frame delay to prevent taking a step before initialising battle.
                    yield return null;
                }

                for (int i2 = 0; i2 < patrol[i].steps; i2++)
                {
                    Vector3 movement = getForwardsVector();
                    while (movement == new Vector3(0, 0, 0))
                    {
                        movement = getForwardsVector();
                        yield return new WaitForSeconds(0.1f);
                    }

                    while (busy)
                    {
                        yield return null;
                    }

                    if (!recentlyDefeated)
                    {
                        yield return StartCoroutine(move(movement));
                    }
                }

                if (patrol[i].endWait > 0)
                {
                    yield return new WaitForSeconds(patrol[i].endWait);
                }

                if (recentlyDefeated)
                {
                    i = patrol.Length;
                }
            }
            if (!recentlyDefeated)
            {
                transform.position = initialPosition;
            }
            yield return null;
        }
    }


    public Vector3 getForwardsVector()
    {
        return getForwardsVector(transform.position, direction);
    }

    public Vector3 getForwardsVector(Vector3 position, int direction)
    {
        Vector3 forwardsVector = new Vector3(0, 0, 0);
        if (direction == 0)
        {
            forwardsVector = new Vector3(0, 0, 1f);
        }
        else if (direction == 1)
        {
            forwardsVector = new Vector3(1f, 0, 0);
        }
        else if (direction == 2)
        {
            forwardsVector = new Vector3(0, 0, -1f);
        }
        else if (direction == 3)
        {
            forwardsVector = new Vector3(-1f, 0, 0);
        }

        Vector3 movement = forwardsVector;

        //Check destination map																	//0.5f to adjust for stair height
        //cast a ray directly downwards from the position directly in front of the npc			//1f to check in line with player's head
        RaycastHit[] mapHitColliders = Physics.RaycastAll(position + movement + new Vector3(0, 1.5f, 0), Vector3.down);
        RaycastHit mapHit = new RaycastHit();
        //cycle through each of the collisions
        if (mapHitColliders.Length > 0)
        {
            for (int i = 0; i < mapHitColliders.Length; i++)
            {
                //if a collision's gameObject has a mapCollider, it is a map. set it to be the destination map.
                if (mapHitColliders[i].collider.gameObject.GetComponent<MapCollider>() != null)
                {
                    mapHit = mapHitColliders[i];
                    destinationMap = mapHit.collider.gameObject.GetComponent<MapCollider>();
                    i = mapHitColliders.Length;
                }
            }
        }

        //check for a bridge at the destination
        RaycastHit bridgeHit = MapCollider.getBridgeHitOfPosition(position + movement + new Vector3(0, 1.5f, 0));
        if (bridgeHit.collider != null)
        {
            //modify the forwards vector to align to the bridge.
            movement -= new Vector3(0, (position.y - bridgeHit.point.y), 0);
        }
        //if no bridge at destination
        else if (mapHit.collider != null)
        {
            //modify the forwards vector to align to the mapHit.
            movement -= new Vector3(0, (position.y - mapHit.point.y), 0);
        }


        float currentSlope = Mathf.Abs(MapCollider.getSlopeOfPosition(position, direction));
        float destinationSlope = Mathf.Abs(MapCollider.getSlopeOfPosition(position + forwardsVector, direction));
        float yDistance = Mathf.Abs((position.y + movement.y) - position.y);
        yDistance = Mathf.Round(yDistance * 100f) / 100f;

        //if either slope is greater than 1 it is too steep.
        if (currentSlope <= 1 && destinationSlope <= 1)
        {
            //if yDistance is greater than both slopes there is a vertical wall between them
            if (yDistance <= currentSlope || yDistance <= destinationSlope)
            {
                //check destination tileTag for impassibles
                int destinationTileTag = destinationMap.getTileTag(position + movement);
                if (destinationTileTag == 1)
                {
                    return Vector3.zero;
                }
                else
                {
                    if (trainerSurfing)
                    {
                        //if a surf trainer, normal tiles are impassible
                        if (destinationTileTag != 2)
                        {
                            return Vector3.zero;
                        }
                    }
                    else
                    {
                        //if not a surf trainer, surf tiles are impassible
                        if (destinationTileTag == 2)
                        {
                            return Vector3.zero;
                        }
                    }
                }

                //check destination for objects/player/follower
                bool destinationPassable = true;
                Collider[] hitColliders = Physics.OverlapSphere(position + movement, 0.4f);
                if (hitColliders.Length > 0)
                {
                    for (int i = 0; i < hitColliders.Length; i++)
                    {
                        if (hitColliders[i].name == "Player_Transparent" ||
                            hitColliders[i].name == "Follower_Transparent" ||
                            hitColliders[i].name.ToLowerInvariant().Contains("_object"))
                        {
                            destinationPassable = false;
                        }
                    }
                }

                if (destinationPassable)
                {
                    return movement;
                }
            }
        }
        return Vector3.zero;
    }


    private IEnumerator move(Vector3 movement)
    {
        float increment = 0f;
        float speed = PlayerMovement.player.walkSpeed;
        Vector3 startPosition = transform.position;
        Vector3 destinationPosition = startPosition + movement;

        animPause = false;
        while (increment < 1f)
        {
            //increment increases slowly to 1 over the frames
            if (PlayerMovement.player.busyWith == null || PlayerMovement.player.busyWith == this.gameObject)
            {
                increment += (1f / speed) * Time.deltaTime;
                    //speed is determined by how many squares are crossed in one second
                if (increment > 1)
                {
                    increment = 1;
                }
                transform.position = startPosition + (movement * increment);
                hitBox.position = destinationPosition;
                sight.position = destinationPosition;
            }
            yield return null;
        }
        animPause = true;
    }

    public IEnumerator spotPlayer()
    {
        if (!defeated && !busy)
        {
            //if the player isn't busy with any other object
            if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
            {
                busy = true;
                BgmHandler.main.PlayOverlay(introBGM, samplesLoopStart);
                //DISPLAY "!"
                yield return StartCoroutine(exclaimAnimation());
                yield return new WaitForSeconds(1.2f);

                //approach player
                Vector3 movement = getForwardsVector();
                while (movement != new Vector3(0, 0, 0))
                {
                    yield return StartCoroutine(move(movement));
                    movement = getForwardsVector();
                }

                int flippedDirection = direction + 2;
                if (flippedDirection > 3)
                {
                    flippedDirection -= 4;
                }
                PlayerMovement.player.direction = flippedDirection;

                StartCoroutine(interact());
            }
        }
    }


    private IEnumerator interact()
    {
        if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
        {
            busy = true;

            //calculate Player's position relative to target object's and set direction accordingly.
            float xDistance = this.transform.position.x - PlayerMovement.player.transform.position.x;
            float zDistance = this.transform.position.z - PlayerMovement.player.transform.position.z;
            if (xDistance >= Mathf.Abs(zDistance))
            {
                //Mathf.Abs() converts zDistance to a positive always.
                updateDirection(3);
            } //this allows for better accuracy when checking orientation.
            else if (xDistance <= Mathf.Abs(zDistance) * -1)
            {
                updateDirection(1);
            }
            else if (zDistance >= Mathf.Abs(xDistance))
            {
                updateDirection(2);
            }
            else
            {
                updateDirection(0);
            }

            if (!defeated)
            {
                //Play INTRO BGM
                BgmHandler.main.PlayOverlay(introBGM, samplesLoopStart);

                //Display all of the confrontation Dialog.
                for (int i = 0; i < trainerConfrontDialog.Length; i++)
                {
                    Dialog.drawDialogBox();
                    yield return Dialog.StartCoroutine(Dialog.drawText( trainerConfrontDialog[i]));
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.undrawDialogBox();
                }

                //custom cutouts not yet implemented
                StartCoroutine(ScreenFade.main.FadeCutout(false, ScreenFade.slowedSpeed, null));

                //Automatic LoopStart usage not yet implemented
                Scene.main.Battle.gameObject.SetActive(true);
                if (trainer.battleBGM != null)
                {
                    BgmHandler.main.PlayOverlay(trainer.battleBGM, trainer.samplesLoopStart);
                }
                else
                {
                    BgmHandler.main.PlayOverlay(Scene.main.Battle.defaultTrainerBGM,
                        Scene.main.Battle.defaultTrainerBGMLoopStart);
                }
                Scene.main.Battle.gameObject.SetActive(false);
                yield return new WaitForSeconds(1.6f);

                Scene.main.Battle.gameObject.SetActive(true);
                StartCoroutine(Scene.main.Battle.control(trainer));

                while (Scene.main.Battle.gameObject.activeSelf)
                {
                    yield return null;
                }

                //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));

                if (Scene.main.Battle.victor == 0)
                {
                    defeated = true;
                    recentlyDefeated = true;
                    //Display all of the defeated Dialog. (if any)
                    for (int i = 0; i < trainerDefeatDialog.Length; i++)
                    {
                        Dialog.drawDialogBox();
                        yield return Dialog.StartCoroutine(Dialog.drawText( trainerDefeatDialog[i]));
                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                        Dialog.undrawDialogBox();
                    }
                }
            }
            else
            {
                //Display all of the post defeat Dialog.
                for (int i = 0; i < trainerPostDefeatDialog.Length; i++)
                {
                    Dialog.drawDialogBox();
                    yield return Dialog.StartCoroutine(Dialog.drawText( trainerPostDefeatDialog[i]));
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.undrawDialogBox();
                }
            }

            busy = false;
            PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
        }
    }
}