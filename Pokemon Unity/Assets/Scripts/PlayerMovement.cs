//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using PokemonUnity.Monster;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement player;

    private DialogBoxHandler Dialog;
    private MapNameBoxHandler MapName;

    private PauseMenu pauseMenu;
    public CanvasUIHandler CanvasUI;

    //before a script runs, it'll check if the player is busy with another script's GameObject.
    public GameObject busyWith = null;

    public bool moving = false;
    public bool still = true;
    public bool running = false;
    public bool surfing = false;
    public bool bike = false;
    // <summary>
    // Replaces <see cref="moving"/>, <see cref="still"/>, <see cref="running"/>, <see cref="surfing"/>, <see cref="bike"/> 
    // </summary>
    //private playerMoveMethod playerMoveAction = playerMoveMethod.Idle;
    private enum playerMoveMethod
    {
        Idle,
        Walking,
        Running,
        Biking,
        Surfing,
        /// <summary>
        /// Could just be "surfing" but on "underwater" map...
        /// </summary>
        Diving 
    }
    public bool strength = false;
    public float walkSpeed = 0.3f; //time in seconds taken to walk 1 square.
    public float runSpeed = 0.15f;
    public float surfSpeed = 0.2f;
    public float speed;
    public int direction = 2; //0 = up, 1 = right, 2 = down, 3 = left

    public bool canInput = true;

    public float increment = 1f;

    private GameObject follower;
    public FollowerMovement followerScript;

    private Transform pawn;
    private Transform pawnReflection;
    //private Material pawnReflectionSprite;
    private SpriteRenderer pawnSprite;
    private SpriteRenderer pawnReflectionSprite;

    public Transform hitBox;

    public MapCollider currentMap;
    public MapCollider destinationMap;

    public MapSettings accessedMapSettings;
    private AudioClip accessedAudio;
    private int accessedAudioLoopStartSamples;

    public Camera mainCamera;
    public Vector3 mainCameraDefaultPosition;
    public float mainCameraDefaultFOV;

    private SpriteRenderer mount;
    private Vector3 mountPosition;

    private string animationName;
    private Sprite[] spriteSheet;
    private Sprite[] mountSpriteSheet;

    private int frame;
    private int frames;
    private int framesPerSec;
    private float secPerFrame;
    private bool animPause;
    private bool overrideAnimPause;

    public int walkFPS = 7;
    public int runFPS = 12;

    private int mostRecentDirectionPressed = 0;
    private float directionChangeInputDelay = 0.08f;

	//private SceneTransition sceneTransition;

    private AudioSource PlayerAudio;

    public AudioClip bumpClip;
    public AudioClip jumpClip;
    public AudioClip landClip;


    void Awake()
    {
        PlayerAudio = transform.GetComponent<AudioSource>();

        //set up the reference to this script.
        player = this;

        Dialog = GameObject.Find("CanvasUI").GetComponent<DialogBoxHandler>();
        MapName = GameObject.Find("MapName").GetComponent<MapNameBoxHandler>();
        Debug.Log(GameObject.Find("CanvasUI").GetComponent<CanvasUIHandler>());
        CanvasUI = GameObject.FindGameObjectWithTag("CanvasUI").GetComponent<CanvasUIHandler>();
        //CanvasUI = GameObject.Find("CanvasUI").GetComponent<CanvasUIHandler>();
        CanvasUI.setInactive(CanvasUIHandler.UICanvas.PauseMenu);

        /* pauseMenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
        pauseMenu.setInactive(); */

        canInput = true;
        speed = walkSpeed;

        follower = transform.Find("Follower").gameObject;
        followerScript = follower.GetComponent<FollowerMovement>();

        mainCamera = transform.Find("Camera").GetComponent<Camera>();
        mainCameraDefaultPosition = mainCamera.transform.localPosition;
        mainCameraDefaultFOV = mainCamera.fieldOfView;

        pawn = transform.Find("Pawn");
        pawnReflection = transform.Find("PawnReflection");
        pawnSprite = pawn.GetComponent<SpriteRenderer>();
        pawnReflectionSprite = pawnReflection.GetComponent<SpriteRenderer>();

        //pawnReflectionSprite = transform.FindChild("PawnReflection").GetComponent<MeshRenderer>().material;

        hitBox = transform.Find("Player_Transparent");

        mount = transform.Find("Mount").GetComponent<SpriteRenderer>();
        mountPosition = mount.transform.localPosition;
    }

    void Start()
    {
        if (!surfing)//playerMoveAction != playerMoveMethod.Surfing
        {
            updateMount(false);
        }

        updateAnimation("walk", walkFPS);
        StartCoroutine("animateSprite");
        animPause = true;

        reflect(false);
        //followerScript.reflect(false);

        updateDirection(direction);

        StartCoroutine(control());


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
            //if no map found
            //Check for map in front of player's direction
            hitRays = Physics.RaycastAll(transform.position + Vector3.up + getForwardVectorRaw(), Vector3.down);
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
                currentMap = hitRays[closestIndex].collider.gameObject.GetComponent<MapCollider>();
            }
            else
            {
                Debug.Log("no map found");
            }
        }


        if (currentMap != null)
        {
            accessedMapSettings = currentMap.gameObject.GetComponent<MapSettings>();
            if (accessedAudio != accessedMapSettings.getBGM())
            {
                //if audio is not already playing
                accessedAudio = accessedMapSettings.getBGM();
                accessedAudioLoopStartSamples = accessedMapSettings.getBGMLoopStartSamples();
                BgmHandler.main.PlayMain(accessedAudio, accessedAudioLoopStartSamples);
            }
            if (accessedMapSettings.mapNameBoxTexture != null)
            {
                MapName.display(accessedMapSettings.mapNameBoxTexture, accessedMapSettings.mapName,
                    accessedMapSettings.mapNameColor);
            }
        }


        //check position for transparent bumpEvents
        Collider transparentCollider = null;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.4f);
        if (hitColliders.Length > 0)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].name.ToLowerInvariant().Contains("_transparent"))
                {
                    if (!hitColliders[i].name.ToLowerInvariant().Contains("player") &&
                        !hitColliders[i].name.ToLowerInvariant().Contains("follower"))
                    {
                        transparentCollider = hitColliders[i];
                    }
                }
            }
        }
        if (transparentCollider != null)
        {
            //send bump message to the object's parent object
            transparentCollider.transform.parent.gameObject.SendMessage("bump", SendMessageOptions.DontRequireReceiver);
        }

        //DEBUG
        /*if (accessedMapSettings != null)
        {
            WildPokemonInitialiserOld[] encounterList =
                accessedMapSettings.getEncounterList(WildPokemonInitialiserOld.Method.WALK);
            string namez = "";
            for (int i = 0; i < encounterList.Length; i++)
            {
<<<<<<< HEAD
                namez += ((PokemonUnity.Pokemons)encounterList[i].ID).toString() + ", ";
=======
                namez += PokemonDatabaseOld.getPokemon(encounterList[i].ID).getName() + ", ";
>>>>>>> beta
            }
            Debug.Log("Wild Pokemon for map \"" + accessedMapSettings.mapName + "\": " + namez);
        }*/
        //

        //GlobalVariables.global.resetFollower();
    }


    void Update()
    {
        //check for new inputs, so that the new direction can be set accordingly
        if (Input.GetButtonDown("Horizontal"))
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                //	Debug.Log("NEW INPUT: Right");
                mostRecentDirectionPressed = 1;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                //	Debug.Log("NEW INPUT: Left");
                mostRecentDirectionPressed = 3;
            }
        }
        else if (Input.GetButtonDown("Vertical"))
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                //	Debug.Log("NEW INPUT: Up");
                mostRecentDirectionPressed = 0;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                //	Debug.Log("NEW INPUT: Down");
                mostRecentDirectionPressed = 2;
            }
        }
    }

    private bool isDirectionKeyHeld(int directionCheck)
    {
        bool directionHeld = false;
        if (directionCheck == 0 && Input.GetAxisRaw("Vertical") < 0)
        {
            directionHeld = true;
        }
        else if (directionCheck == 1 && Input.GetAxisRaw("Horizontal") > 0)
        {
            directionHeld = true;
        }
        else if (directionCheck == 2 && Input.GetAxisRaw("Vertical") > 0)
        {
            directionHeld = true;
        }
        else if (directionCheck == 3 && Input.GetAxisRaw("Horizontal") < 0)
        {
            directionHeld = true;
        }
        return directionHeld;
    }

    private IEnumerator control() //ToDo: replace with enum
    {
        bool still;//replace with playerMoveAction
        while (true)
        {
            still = true;
            //the player is still, but if they've just finished moving a space, moving is still true for this frame (see end of coroutine)
            if (canInput)
            {
                if (!surfing && !bike)
                {
                    if (Input.GetButton("Run"))
                    {
                        running = true;
                        if (moving)
                        {
                            updateAnimation("run", runFPS);
                        }
                        else
                        {
                            updateAnimation("walk", walkFPS);
                        }
                        speed = runSpeed;
                    }
                    else
                    {
                        running = false;
                        updateAnimation("walk", walkFPS);
                        speed = walkSpeed;
                    }
                }
                if (Input.GetButton("Start"))
                {
                    //open Pause Menu
                    if (moving || Input.GetButtonDown("Start"))
                    {
                        //pauseMenu.openAnim();
                        if (setCheckBusyWith(CanvasUI.gameObject)){
                            animPause =true;
                            CanvasUI.switchScene(CanvasUIHandler.UICanvas.PauseMenu);
                            while (CanvasUI.active(CanvasUIHandler.UICanvas.PauseMenu))
                            {
                                yield return null;
                            }
                            unsetCheckBusyWith(CanvasUI.gameObject);
                        }
                        /* if (setCheckBusyWith(Scene.main.Pause.gameObject))
                        {
                            animPause = true;
                            Scene.main.Pause.gameObject.SetActive(true);
                            StartCoroutine(Scene.main.Pause.control());
                            while (Scene.main.Pause.gameObject.activeSelf)
                            {
                                yield return null;
                            }
                            unsetCheckBusyWith(Scene.main.Pause.gameObject);
                        } */
                    }
                }
                else if (Input.GetButtonDown("Select"))
                {
                    interact();
                }
                //if pausing/interacting/etc. is not being called, then moving is possible.
                //		(if any direction input is being entered)
                else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                {
                    //if most recent direction pressed is held, but it isn't the current direction, set it to be
                    if (mostRecentDirectionPressed != direction && isDirectionKeyHeld(mostRecentDirectionPressed))
                    {
                        updateDirection(mostRecentDirectionPressed);
                        if (!moving)
                        {
                            // unless player has just moved, wait a small amount of time to ensure that they have time to
                            yield return new WaitForSeconds(directionChangeInputDelay);
                        } // let go before moving (allows only turning)
                    }
                    //if a new direction wasn't found, direction would have been set, thus ending the update
                    else
                    {
                        //if current direction is not held down, check for the new direction to turn to
                        if (!isDirectionKeyHeld(direction))
                        {
                            //it's least likely to have held the opposite direction by accident
                            int directionCheck = (direction + 2 > 3) ? direction - 2 : direction + 2;
                            if (isDirectionKeyHeld(directionCheck))
                            {
                                updateDirection(directionCheck);
                                if (!moving)
                                {
                                    yield return new WaitForSeconds(directionChangeInputDelay);
                                }
                            }
                            else
                            {
                                //it's either 90 degrees clockwise, counter, or none at this point. prioritise clockwise.
                                directionCheck = (direction + 1 > 3) ? direction - 3 : direction + 1;
                                if (isDirectionKeyHeld(directionCheck))
                                {
                                    updateDirection(directionCheck);
                                    if (!moving)
                                    {
                                        yield return new WaitForSeconds(directionChangeInputDelay);
                                    }
                                }
                                else
                                {
                                    directionCheck = (direction - 1 < 0) ? direction + 3 : direction - 1;
                                    if (isDirectionKeyHeld(directionCheck))
                                    {
                                        updateDirection(directionCheck);
                                        if (!moving)
                                        {
                                            yield return new WaitForSeconds(directionChangeInputDelay);
                                        }
                                    }
                                }
                            }
                        }
                        //if current direction was held, then we want to attempt to move forward.
                        else
                        {
                            moving = true;
                        }
                    }

                    //if moving is true (including by momentum from the previous step) then attempt to move forward.
                    if (moving)
                    {
                        still = false;
                        yield return StartCoroutine(moveForward());
                    }
                }
                else if (Input.GetKeyDown("g") && Game.IS_DEBUG_ACTIVE)
                {
                    //DEBUG
                    Debug.Log(currentMap.getTileTag(transform.position));
                    if (followerScript.canMove)
                    {
                        followerScript.StartCoroutine("withdrawToBall");
                    }
                    else
                    {
                        followerScript.canMove = true;
                    }
                }
                /*else if (Input.GetKeyDown(",") && SaveData.currentSave.IS_DEBUG_ACTIVE+)
                {
                    //GlobalVariables.debug(GlobalVariables.GetDebugText());
                    //DEBUG
                    Debug.Log(currentMap.getTileTag(transform.position));
                    if (followerScript.canMove)
                    {
                        followerScript.StartCoroutine("withdrawToBall");
                    }
                    else
                    {
                        followerScript.canMove = true;
                    }
                }*/
            }
            if (still)
            {
                //if still is true by this point, then no move function has been called
                animPause = true;
                moving = false;
            } //set moving to false. The player loses their momentum.

            yield return null;
        }
    }

    public void updateDirection(int dir)
    {
        direction = dir;
        pawnSprite.sprite = spriteSheet[direction * frames + frame];
        pawnReflectionSprite.sprite = pawnSprite.sprite;
        //pawnReflectionSprite.SetTextureOffset("_MainTex", GetUVSpriteMap(direction*frames+frame));
        if (mount.enabled)
        {
            mount.sprite = mountSpriteSheet[dir];
        }
    }

    private void updateMount(bool enabled)
    {
        mount.enabled = enabled;
    }

    private void updateMount(bool enabled, string spriteName)
    {
        mount.enabled = enabled;
        mountSpriteSheet = Resources.LoadAll<Sprite>("PlayerSprites/" + spriteName);
        mount.sprite = mountSpriteSheet[direction];
    }

    public void updateAnimation(string newAnimationName, int fps)
    {
        if (animationName != newAnimationName)
        {
            animationName = newAnimationName;
            //if(SaveDataOld.currentSave.getPlayerSpritePrefix().Contains("custom")) {
            //    WWW www = new WWW("file://" + System.IO.Path.Combine(Application.streamingAssetsPath, SaveDataOld.currentSave.getPlayerSpritePrefix() + newAnimationName));
 			//
            //    if (!string.IsNullOrEmpty(www.error)) {
            //        Debug.Log (www.error);
            //        SaveDataOld.currentSave.playerOutfit = "hgss";
            //        PlayerPrefs.SetInt("customSprites", 0);
            //        if(SaveDataOld.currentSave.getCVariable("male") == 1) {
            //            spriteSheet = Resources.LoadAll<Sprite>("PlayerSprites/m_hgss_" + newAnimationName);
            //        }
            //        else {
            //            spriteSheet = Resources.LoadAll<Sprite>("PlayerSprites/f_hgss_" + newAnimationName);
            //        }
            //    } 
            //    else {
            //        /*Sprite custom1 = Sprite.Create(www.texture, new Rect(0f, 96f, 32f, 32f), new Vector2(0.5f, 0f));
            //        Sprite custom2 = Sprite.Create(www.texture, new Rect(32f, 96f, 32f, 32f), new Vector2(0.5f, 0f));
            //        Sprite custom3 = Sprite.Create(www.texture, new Rect(64f, 96f, 32f, 32f), new Vector2(0.5f, 0f));
            //        Sprite custom4 = Sprite.Create(www.texture, new Rect(96f, 96f, 32f, 32f), new Vector2(0.5f, 0f));
            //        Sprite custom5 = Sprite.Create(www.texture, new Rect(0f, 64f, 32f, 32f), new Vector2(0.5f, 0f));
            //        Sprite custom6 = Sprite.Create(www.texture, new Rect(32f, 64f, 32f, 32f), new Vector2(0.5f, 0f));
            //        Sprite custom7 = Sprite.Create(www.texture, new Rect(64f, 64f, 32f, 32f), new Vector2(0.5f, 0f));
            //        Sprite custom8 = Sprite.Create(www.texture, new Rect(96f, 64f, 32f, 32f), new Vector2(0.5f, 0f));
            //        Sprite custom9 = Sprite.Create(www.texture, new Rect(0f, 32f, 32f, 32f), new Vector2(0.5f, 0f));
            //        Sprite custom10 = Sprite.Create(www.texture, new Rect(32f, 32f, 32f, 32f), new Vector2(0.5f, 0f));
            //        Sprite custom11 = Sprite.Create(www.texture, new Rect(64f, 32f, 32f, 32f), new Vector2(0.5f, 0f));
            //        Sprite custom12 = Sprite.Create(www.texture, new Rect(96f, 32f, 32f, 32f), new Vector2(0.5f, 0f));
            //        Sprite custom13 = Sprite.Create(www.texture, new Rect(0f, 0f, 32f, 32f), new Vector2(0.5f, 0f));
            //        Sprite custom14 = Sprite.Create(www.texture, new Rect(32f, 0f, 32f, 32f), new Vector2(0.5f, 0f));
            //        Sprite custom15 = Sprite.Create(www.texture, new Rect(64f, 0f, 32f, 32f), new Vector2(0.5f, 0f));
            //        Sprite custom16 = Sprite.Create(www.texture, new Rect(96f, 0f, 32f, 32f), new Vector2(0.5f, 0f));
            //        spriteSheet = new Sprite[] {custom1,custom2,custom3,custom4,custom5,custom6,custom7,custom8,custom9,custom10,custom11,custom12,custom13,custom14,custom15,custom16};*/
            //        //I highly doubt this is the correct way to do it
            //        Debug.Log("Not implemented");
            //        if(SaveDataOld.currentSave.getCVariable("male") == 1) {
            //            spriteSheet = Resources.LoadAll<Sprite>("PlayerSprites/m_hgss_" + newAnimationName);
            //        }
            //        else {
            //            spriteSheet = Resources.LoadAll<Sprite>("PlayerSprites/f_hgss_" + newAnimationName);
            //        }
            //    }
            //}
            //else {
            //    spriteSheet =
            //        Resources.LoadAll<Sprite>("PlayerSprites/" + SaveDataOld.currentSave.getPlayerSpritePrefix() + newAnimationName);
            //    //pawnReflectionSprite.SetTexture("_MainTex", Resources.Load<Texture>("PlayerSprites/"+SaveData.currentSave.getPlayerSpritePrefix()+newAnimationName));
            //}
            spriteSheet = Resources.LoadAll<Sprite>("Sprites/PlayerSprites/m_hgss_" + newAnimationName);
            framesPerSec = fps;
            secPerFrame = 1f / (float) framesPerSec;
            frames = Mathf.RoundToInt((float) spriteSheet.Length / 4f);
            if (frame >= frames)
            {
                frame = 0;
            }
        }
    }

    public void reflect(bool setState)
    {
        pawnReflectionSprite.enabled = setState;
    }

    private Vector2 GetUVSpriteMap(int index)
    {
        int row = index / 4;
        int column = index % 4;

        return new Vector2(0.25f * column, 0.75f - (0.25f * row));
    }

    private IEnumerator animateSprite()
    {
        frame = 0;
        frames = 4;
        framesPerSec = walkFPS;
        secPerFrame = 1f / (float) framesPerSec;
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                if (animPause && frame % 2 != 0 && !overrideAnimPause)
                {
                    frame -= 1;
                }
                pawnSprite.sprite = spriteSheet[direction * frames + frame];
                pawnReflectionSprite.sprite = pawnSprite.sprite;
                //pawnReflectionSprite.SetTextureOffset("_MainTex", GetUVSpriteMap(direction*frames+frame));
                yield return new WaitForSeconds(secPerFrame / 4f);
            }
            if (!animPause || overrideAnimPause)
            {
                frame += 1;
                if (frame >= frames)
                {
                    frame = 0;
                }
            }
        }
    }

    public void setOverrideAnimPause(bool set)
    {
        overrideAnimPause = set;
    }

    ///Attempts to set player to be busy with "caller" and pauses input, returning true if the request worked.
    public bool setCheckBusyWith(GameObject caller)
    {
        if (PlayerMovement.player.busyWith == null)
        {
            PlayerMovement.player.busyWith = caller;
        }
        //if the player is definitely busy with caller object
        if (PlayerMovement.player.busyWith == caller)
        {
            pauseInput();
            Debug.Log("Busy with " + PlayerMovement.player.busyWith);
            return true;
        }
        return false;
    }

    ///Attempts to unset player to be busy with "caller". Will unpause input only if 
    ///the player is still not busy 0.1 seconds after calling.
    public void unsetCheckBusyWith(GameObject caller)
    {
        if (PlayerMovement.player.busyWith == caller)
        {
            PlayerMovement.player.busyWith = null;
        }
        StartCoroutine(checkBusinessBeforeUnpause(0.1f));
    }

    public IEnumerator checkBusinessBeforeUnpause(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (PlayerMovement.player.busyWith == null)
        {
            unpauseInput();
        }
        else
        {
            Debug.Log("Busy with " + PlayerMovement.player.busyWith);
        }
    }

    public void pauseInput()
    {
        canInput = false;
        if (animationName == "run")
        {
            updateAnimation("walk", walkFPS);
        }
        running = false;
    }

    public void pauseInput(float secondsToWait)
    {
        pauseInput();
        StartCoroutine(checkBusinessBeforeUnpause(secondsToWait));
    }

    public void unpauseInput()
    {
        Debug.Log("unpaused");
        canInput = true;
    }

    public bool isInputPaused()
    {
        if (canInput)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void activateStrength()
    {
        strength = true;
    }


    ///returns the vector relative to the player direction, without any modifications.
    public Vector3 getForwardVectorRaw()
    {
        return getForwardVectorRaw(direction);
    }

    public Vector3 getForwardVectorRaw(int direction)
    {
        //set vector3 based off of direction
        Vector3 forwardVector = new Vector3(0, 0, 0);
        if (direction == 0)
        {
            forwardVector = new Vector3(0, 0, 1f);
        }
        else if (direction == 1)
        {
            forwardVector = new Vector3(1f, 0, 0);
        }
        else if (direction == 2)
        {
            forwardVector = new Vector3(0, 0, -1f);
        }
        else if (direction == 3)
        {
            forwardVector = new Vector3(-1f, 0, 0);
        }
        return forwardVector;
    }


    public Vector3 getForwardVector()
    {
        return getForwardVector(direction, true);
    }

    public Vector3 getForwardVector(int direction)
    {
        return getForwardVector(direction, true);
    }

    public Vector3 getForwardVector(int direction, bool checkForBridge)
    {
        //set initial vector3 based off of direction
        Vector3 movement = getForwardVectorRaw(direction);

        //Check destination map	and bridge																//0.5f to adjust for stair height
        //cast a ray directly downwards from the position directly in front of the player		//1f to check in line with player's head
        RaycastHit[] hitColliders = Physics.RaycastAll(transform.position + movement + new Vector3(0, 1.5f, 0),
            Vector3.down);
        RaycastHit mapHit = new RaycastHit();
        RaycastHit bridgeHit = new RaycastHit();
        //cycle through each of the collisions
        if (hitColliders.Length > 0)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                //if map has not been found yet
                if (mapHit.collider == null)
                {
                    //if a collision's gameObject has a mapCollider, it is a map. set it to be the destination map.
                    if (hitColliders[i].collider.gameObject.GetComponent<MapCollider>() != null)
                    {
                        mapHit = hitColliders[i];
                        destinationMap = mapHit.collider.gameObject.GetComponent<MapCollider>();
                    }
                }
                else if (bridgeHit.collider != null && checkForBridge)
                {
                    //if both have been found
                    i = hitColliders.Length; //stop searching
                }
                //if bridge has not been found yet
                if (bridgeHit.collider == null && checkForBridge)
                {
                    //if a collision's gameObject has a BridgeHandler, it is a bridge.
                    if (hitColliders[i].collider.gameObject.GetComponent<BridgeHandler>() != null)
                    {
                        bridgeHit = hitColliders[i];
                    }
                }
                else if (mapHit.collider != null)
                {
                    //if both have been found
                    i = hitColliders.Length; //stop searching
                }
            }
        }

        if (bridgeHit.collider != null)
        {
            //modify the forwards vector to align to the bridge.
            movement -= new Vector3(0, (transform.position.y - bridgeHit.point.y), 0);
        }
        //if no bridge at destination
        else if (mapHit.collider != null)
        {
            //modify the forwards vector to align to the mapHit.
            movement -= new Vector3(0, (transform.position.y - mapHit.point.y), 0);
        }


        float currentSlope = Mathf.Abs(MapCollider.getSlopeOfPosition(transform.position, direction));
        float destinationSlope =
            Mathf.Abs(MapCollider.getSlopeOfPosition(transform.position + getForwardVectorRaw(), direction,
                checkForBridge));
        float yDistance = Mathf.Abs((transform.position.y + movement.y) - transform.position.y);
        yDistance = Mathf.Round(yDistance * 100f) / 100f;

        //Debug.Log("currentSlope: "+currentSlope+", destinationSlope: "+destinationSlope+", yDistance: "+yDistance);

        //if either slope is greater than 1 it is too steep.
        if (currentSlope <= 1 && destinationSlope <= 1)
        {
            //if yDistance is greater than both slopes there is a vertical wall between them
            if (yDistance <= currentSlope || yDistance <= destinationSlope)
            {
                return movement;
            }
        }
        return Vector3.zero;
    }

    ///Make the player move one space in the direction they are facing
    private IEnumerator moveForward()
    {
        Vector3 movement = getForwardVector();

        bool ableToMove = false;

        //without any movement, able to move should stay false
        if (movement != Vector3.zero)
        {
            //check destination for objects/transparents
            Collider objectCollider = null;
            Collider transparentCollider = null;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + movement + new Vector3(0, 0.5f, 0),
                0.4f);
            if (hitColliders.Length > 0)
            {
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].name.ToLowerInvariant().Contains("_object"))
                    {
                        objectCollider = hitColliders[i];
                    }
                    else if (hitColliders[i].name.ToLowerInvariant().Contains("_transparent"))
                    {
                        transparentCollider = hitColliders[i];
                    }
                }
            }

            if (objectCollider != null)
            {
                //send bump message to the object's parent object
                objectCollider.transform.parent.gameObject.SendMessage("bump", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                //if no objects are in the way
                int destinationTileTag = destinationMap.getTileTag(transform.position + movement);

                RaycastHit bridgeHit =
                    MapCollider.getBridgeHitOfPosition(transform.position + movement + new Vector3(0, 0.1f, 0));
                if (bridgeHit.collider != null || destinationTileTag != 1)
                {
                    //wall tile tag

                    if (bridgeHit.collider == null && !surfing && destinationTileTag == 2)
                    {
                        //(water tile tag)
                    }
                    else
                    {
                        if (surfing && destinationTileTag != 2f)
                        {
                            //disable surfing if not headed to water tile
                            updateAnimation("walk", walkFPS);
                            speed = walkSpeed;
                            surfing = false;
                            StartCoroutine("dismount");
                            BgmHandler.main.PlayMain(accessedAudio, accessedAudioLoopStartSamples);
                        }

                        if (destinationMap != currentMap)
                        {
                            //if moving onto a new map
                            currentMap = destinationMap;
                            accessedMapSettings = destinationMap.gameObject.GetComponent<MapSettings>();
                            if (accessedAudio != accessedMapSettings.getBGM())
                            {
                                //if audio is not already playing
                                accessedAudio = accessedMapSettings.getBGM();
                                accessedAudioLoopStartSamples = accessedMapSettings.getBGMLoopStartSamples();
                                BgmHandler.main.PlayMain(accessedAudio, accessedAudioLoopStartSamples);
                            }
                            destinationMap.BroadcastMessage("repair", SendMessageOptions.DontRequireReceiver);
                            if (accessedMapSettings.mapNameBoxTexture != null)
                            {
                                MapName.display(accessedMapSettings.mapNameBoxTexture, accessedMapSettings.mapName,
                                    accessedMapSettings.mapNameColor);
                            }
                            Debug.Log(destinationMap.name + "   " + accessedAudio.name);
                        }

                        if (transparentCollider != null)
                        {
                            //send bump message to the transparents's parent object
                            transparentCollider.transform.parent.gameObject.SendMessage("bump",
                                SendMessageOptions.DontRequireReceiver);
                        }

                        ableToMove = true;
                        yield return StartCoroutine(move(movement));
                    }
                }
            }
        }

        //if unable to move anywhere, then set moving to false so that the player stops animating.
        if (!ableToMove)
        {
            Invoke("playBump", 0.05f);
            moving = false;
            animPause = true;
        }
    }


    public IEnumerator move(Vector3 movement)
    {
        yield return StartCoroutine(move(movement, true, false));
    }

    public IEnumerator move(Vector3 movement, bool encounter)
    {
        yield return StartCoroutine(move(movement, encounter, false));
    }

    public IEnumerator move(Vector3 movement, bool encounter, bool lockFollower)
    {
        if (movement != Vector3.zero)
        {
            Vector3 startPosition = hitBox.position;
            moving = true;
            increment = 0;

            if (!lockFollower)
            {
                //StartCoroutine(followerScript.move(startPosition, speed));
            }
            animPause = false;
            while (increment < 1f)
            {
                //increment increases slowly to 1 over the frames
                increment += (1f / speed) * Time.deltaTime;
                    //speed is determined by how many squares are crossed in one second
                if (increment > 1)
                {
                    increment = 1;
                }
                transform.position = startPosition + (movement * increment);
                hitBox.position = startPosition + movement;
                yield return null;
            }

            //check for encounters unless disabled
            if (encounter)
            {
                
                int destinationTag = currentMap.getTileTag(transform.position);
                if (destinationTag != 1)
                {
                    //not a wall
                    if (destinationTag == 2)
                    {
                        //surf tile
<<<<<<< HEAD
                        //StartCoroutine(PlayerMovement.player.wildEncounter(WildPokemonInitialiser.Location.Surfing));
=======
                        StartCoroutine(PlayerMovement.player.wildEncounter(WildPokemonInitialiserOld.Method.SURF));
>>>>>>> beta
                    }
                    else
                    {
                        //land tile
<<<<<<< HEAD
                        //StartCoroutine(PlayerMovement.player.wildEncounter(WildPokemonInitialiser.Location.Standard));
=======
                        StartCoroutine(PlayerMovement.player.wildEncounter(WildPokemonInitialiserOld.Method.WALK));
>>>>>>> beta
                    }
                }
            }
        }
    }

    public IEnumerator moveCameraTo(Vector3 targetPosition, float cameraSpeed)
    {
        targetPosition += mainCameraDefaultPosition;
        Vector3 startPosition = mainCamera.transform.localPosition;
        Vector3 movement = targetPosition - startPosition;
        float increment = 0;
        if (cameraSpeed != 0)
        {
            while (increment < 1f)
            {
                //increment increases slowly to 1 over the frames
                increment += (1f / cameraSpeed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                mainCamera.transform.localPosition = startPosition + (movement * increment);
                yield return null;
            }
        }
        mainCamera.transform.localPosition = targetPosition;
    }


    public void forceMoveForward()
    {
        StartCoroutine(forceMoveForwardIE(1));
    }

    public void forceMoveForward(int spaces)
    {
        StartCoroutine(forceMoveForwardIE(spaces));
    }

    private IEnumerator forceMoveForwardIE(int spaces)
    {
        overrideAnimPause = true;
        for (int i = 0; i < spaces; i++)
        {
            Vector3 movement = getForwardVector();

            //check destination for transparents
<<<<<<< HEAD
            //Collider objectCollider = null;
=======
>>>>>>> beta
            Collider transparentCollider = null;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + movement + new Vector3(0, 0.5f, 0),
                0.4f);
            if (hitColliders.Length > 0)
            {
                for (int i2 = 0; i2 < hitColliders.Length; i2++)
                {
                    if (hitColliders[i2].name.ToLowerInvariant().Contains("_transparent"))
                    {
                        transparentCollider = hitColliders[i2];
                    }
                }
            }
            if (transparentCollider != null)
            {
                //send bump message to the transparents's parent object
                transparentCollider.transform.parent.gameObject.SendMessage("bump",
                    SendMessageOptions.DontRequireReceiver);
            }

            yield return StartCoroutine(move(movement, false));
        }
        overrideAnimPause = false;
    }

    private void interact()
    {
        Vector3 spaceInFront = getForwardVector();

        Collider[] hitColliders =
            Physics.OverlapSphere(
                (new Vector3(transform.position.x, (transform.position.y + 0.5f), transform.position.z) + spaceInFront),
                0.4f);
        Collider currentInteraction = null;
        if (hitColliders.Length > 0)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].name.Contains("_Transparent"))
                {
                    //Prioritise a transparent over a solid object.
                    if (hitColliders[i].name != ("Player_Transparent"))
                    {
                        currentInteraction = hitColliders[i];
                        i = hitColliders.Length;
                    } //Stop checking for other interactable events if a transparent was found.
                }
                else if (hitColliders[i].name.Contains("_Object"))
                {
                    currentInteraction = hitColliders[i];
                }
            }
        }
        if (currentInteraction != null)
        {
            //sent interact message to the collider's object's parent object
            currentInteraction.transform.parent.gameObject.SendMessage("interact",
                SendMessageOptions.DontRequireReceiver);
            currentInteraction = null;
        }
        else if (!surfing)
        {
            if (currentMap.getTileTag(transform.position + spaceInFront) == 2)
            {
                //water tile tag
                StartCoroutine("surfCheck");
            }
        }
    }

    public IEnumerator jump()
    {
        //float currentSpeed = speed;
        //speed = walkSpeed;
        float increment = 0f;
        float parabola = 0;
        float height = 2.1f;
        Vector3 startPosition = pawn.position;

        playClip(jumpClip);

        while (increment < 1)
        {
            increment += (1 / walkSpeed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            parabola = -height * (increment * increment) + (height * increment);
            pawn.position = new Vector3(pawn.position.x, startPosition.y + parabola, pawn.position.z);
            yield return null;
        }

        playClip(landClip);

        //speed = currentSpeed;
    }

    private IEnumerator stillMount()
    {
        Vector3 holdPosition = mount.transform.position;
        float hIncrement = 0f;
        while (hIncrement < 1)
        {
            hIncrement += (1 / speed) * Time.deltaTime;
            mount.transform.position = holdPosition;
            yield return null;
        }
        mount.transform.position = holdPosition;
    }

    private IEnumerator dismount()
    {
        StartCoroutine("stillMount");
        yield return StartCoroutine("jump");
        followerScript.canMove = true;
        mount.transform.localPosition = mountPosition;
        updateMount(false);
    }

    //private IEnumerator surfCheck()
    //{
    //    PokemonOld targetPokemon = SaveDataOld.currentSave.PC.getFirstFEUserInParty("Surf");
    //    if (targetPokemon != null)
    //    {
    //        if (getForwardVector(direction, false) != Vector3.zero)
    //        {
    //            if (setCheckBusyWith(this.gameObject))
    //            {
    //                Dialog.drawDialogBox();
    //                yield return
    //                    Dialog.StartCoroutine("drawText",
    //                        "The water is dyed a deep blue. Would you \nlike to surf on it?");
    //                Dialog.drawChoiceBox();
    //                yield return Dialog.StartCoroutine("choiceNavigate");
    //                Dialog.undrawChoiceBox();
    //                int chosenIndex = Dialog.chosenIndex;
    //                if (chosenIndex == 1)
    //                {
    //                    Dialog.drawDialogBox();
    //                    yield return
    //                        Dialog.StartCoroutine("drawText",
    //                            targetPokemon.getName() + " used " + targetPokemon.getFirstFEInstance("Surf") + "!");
    //                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
    //                    {
    //                        yield return null;
    //                    }
    //                    surfing = true;
    //                    updateMount(true, "surf");
	//
    //                    BgmHandler.main.PlayMain(GlobalVariables.global.surfBGM, GlobalVariables.global.surfBgmLoopStart);
	//
    //                    //determine the vector for the space in front of the player by checking direction
    //                    Vector3 spaceInFront = new Vector3(0, 0, 0);
    //                    if (direction == 0)
    //                    {
    //                        spaceInFront = new Vector3(0, 0, 1);
    //                    }
    //                    else if (direction == 1)
    //                    {
    //                        spaceInFront = new Vector3(1, 0, 0);
    //                    }
    //                    else if (direction == 2)
    //                    {
    //                        spaceInFront = new Vector3(0, 0, -1);
    //                    }
    //                    else if (direction == 3)
    //                    {
    //                        spaceInFront = new Vector3(-1, 0, 0);
    //                    }
	//
    //                    mount.transform.position = mount.transform.position + spaceInFront;
	//
    //                    followerScript.StartCoroutine("withdrawToBall");
    //                    StartCoroutine("stillMount");
    //                    forceMoveForward();
    //                    yield return StartCoroutine("jump");
	//
    //                    updateAnimation("surf", walkFPS);
    //                    speed = surfSpeed;
    //                }
    //                Dialog.undrawDialogBox();
    //                unsetCheckBusyWith(this.gameObject);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (setCheckBusyWith(this.gameObject))
    //        {
    //            Dialog.drawDialogBox();
    //            yield return Dialog.StartCoroutine("drawText", "The water is dyed a deep blue.");
    //            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
    //            {
    //                yield return null;
    //            }
    //            Dialog.undrawDialogBox();
    //            unsetCheckBusyWith(this.gameObject);
    //        }
    //    }
    //    yield return new WaitForSeconds(0.2f);
    //}

    public IEnumerator wildEncounter(WildPokemonInitialiserOld.Method encounterLocation)
    {
<<<<<<< HEAD
        Pokemon targetPokemon = SaveData.currentSave.Player.getFirstFEUserInParty(PokemonUnity.Moves.SURF);
        if (targetPokemon.IsNotNullOrNone())
        {
            if (getForwardVector(direction, false) != Vector3.zero)
            {
                if (setCheckBusyWith(this.gameObject))
                {
                    Dialog.drawDialogBox();
                    yield return
                        Dialog.StartCoroutine("drawText",
                            "The water is dyed a deep blue. Would you \nlike to surf on it?");
                    Dialog.drawChoiceBox();
                    yield return Dialog.StartCoroutine("choiceNavigate");
                    Dialog.undrawChoiceBox();
                    int chosenIndex = Dialog.chosenIndex;
                    if (chosenIndex == 1)
                    {
                        Dialog.drawDialogBox();
                        yield return
                            Dialog.StartCoroutine("drawText",
                                targetPokemon.Name + " used " + PokemonUnity.Moves.SURF.toString() + "!");
                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                        surfing = true;
                        updateMount(true, "surf");

                        BgmHandler.main.PlayMain(GlobalVariables.global.surfBGM, GlobalVariables.global.surfBgmLoopStart);

                        //determine the vector for the space in front of the player by checking direction
                        Vector3 spaceInFront = new Vector3(0, 0, 0);
                        if (direction == 0)
                        {
                            spaceInFront = new Vector3(0, 0, 1);
                        }
                        else if (direction == 1)
                        {
                            spaceInFront = new Vector3(1, 0, 0);
                        }
                        else if (direction == 2)
                        {
                            spaceInFront = new Vector3(0, 0, -1);
                        }
                        else if (direction == 3)
                        {
                            spaceInFront = new Vector3(-1, 0, 0);
                        }

                        mount.transform.position = mount.transform.position + spaceInFront;

                        followerScript.StartCoroutine("withdrawToBall");
                        StartCoroutine("stillMount");
                        forceMoveForward();
                        yield return StartCoroutine("jump");

                        updateAnimation("surf", walkFPS);
                        speed = surfSpeed;
                    }
                    Dialog.undrawDialogBox();
                    unsetCheckBusyWith(this.gameObject);
                }
            }
        }
        else
        {
            if (setCheckBusyWith(this.gameObject))
            {
                Dialog.drawDialogBox();
                yield return Dialog.StartCoroutine("drawText", "The water is dyed a deep blue.");
                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                Dialog.undrawDialogBox();
                unsetCheckBusyWith(this.gameObject);
            }
        }
        yield return new WaitForSeconds(0.2f);
    }

    public IEnumerator wildEncounter(WildPokemonInitialiser.Location encounterLocation)
    {
        if (accessedMapSettings.getEncounterList(encounterLocation).Length > 0)
        {
            if (Random.value <= accessedMapSettings.getEncounterProbability())
            {
                return null;
            }
        }
        return null;
=======
		/*if (accessedMapSettings.getEncounterList(encounterLocation).Length > 0)
        {
            if (Random.value <= accessedMapSettings.getEncounterProbability())
            {
                if (setCheckBusyWith(Scene.main.Battle.gameObject))
                {
                    BgmHandler.main.PlayOverlay(Scene.main.Battle.defaultWildBGM,
                        Scene.main.Battle.defaultWildBGMLoopStart);

                    //SceneTransition sceneTransition = Dialog.transform.GetComponent<SceneTransition>();

                    yield return StartCoroutine(ScreenFade.main.FadeCutout(false, ScreenFade.slowedSpeed, null));
                    //yield return new WaitForSeconds(sceneTransition.FadeOut(1f));
                    Scene.main.Battle.gameObject.SetActive(true);
                    //StartCoroutine(Scene.main.Battle.control(accessedMapSettings.getRandomEncounter(encounterLocation)));

                    while (Scene.main.Battle.gameObject.activeSelf)
                    {
                        yield return null;
                    }

                    //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                    yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));

                    unsetCheckBusyWith(Scene.main.Battle.gameObject);
                }
            }
        }*/ yield return null;
>>>>>>> beta
    }

    private void playClip(AudioClip clip)
    {
        PlayerAudio.clip = clip;
        PlayerAudio.volume = PlayerPrefs.GetFloat("sfxVolume");
        PlayerAudio.Play();
    }

    private void playBump()
    {
        if (!PlayerAudio.isPlaying)
        {
            if (!moving && !overrideAnimPause)
            {
                playClip(bumpClip);
            }
        }
    }
}