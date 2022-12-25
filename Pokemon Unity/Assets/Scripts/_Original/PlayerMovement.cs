//Original Scripts by IIColour (IIColour_Spectrum)

using System;
using System.Collections;
using System.Linq;
using PokemonUnity;
using PokemonUnity.Localization;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;
using PokemonUnity.Utility;
using PokemonEssentials;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using UnityEngine;
//using DiscordPresence;
using Random = System.Random;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement player;

    private DialogBoxHandlerNew Dialog;
    private MapNameBoxHandler MapName;

    //before a script runs, it'll check if the player is busy with another script's GameObject.
    public GameObject busyWith = null;

    public bool moving = false;
    public bool still = true;
    public bool running = false;
    public bool surfing = false;
    public bool bike = false;
    public bool strength = false;
    public float walkSpeed = 0.3f; //time in seconds taken to walk 1 square.
    public float runSpeed = 0.15f;
    public float surfSpeed = 0.2f;
    public float speed;
    public int direction = 2; //0 = up, 1 = right, 2 = down, 3 = left

    private bool jumping = false;

    public bool canInput = true;

    public float increment = 1f;

    private GameObject follower;
    public FollowerMovement followerScript;

    public NPCFollower npcFollower;

    private Transform pawn;
    private Transform pawnReflection;
    //private Material pawnReflectionSprite;
    
    //Player sprites
    private SpriteRenderer pawnSprite;
    private SpriteRenderer hairSprite;
    private SpriteRenderer eyeSprite;
    
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
    private UnityEngine.Sprite[] spriteSheet;
    private UnityEngine.Sprite[] haircutSpriteSheet;
    private UnityEngine.Sprite[] eyeSpriteSheet;
    private UnityEngine.Sprite[] mountSpriteSheet;

    private int frame;
    private int frames;
    public int framesPerSec;
    private float secPerFrame;
    private bool animPause;
    private bool overrideAnimPause;

    public int walkFPS = 7;
    public int runFPS = 12;

    private int mostRecentDirectionPressed = 0;
    private float directionChangeInputDelay = 0.08f;

    private Vector3 cam_origin;

//	private SceneTransition sceneTransition;

    private AudioSource PlayerAudio;

    public AudioClip bumpClip;
    public AudioClip jumpClip;
    public AudioClip landClip;


    void Awake()
    {
        PlayerAudio = transform.GetComponent<AudioSource>();

        //set up the reference to this script.
        player = this;

        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandlerNew>();
        MapName = GameObject.Find("GUI").GetComponent<MapNameBoxHandler>();

        canInput = true;
        speed = walkSpeed;

        follower = transform.Find("Follower").gameObject;
        followerScript = follower.GetComponent<FollowerMovement>();

        mainCamera = transform.Find("Camera").GetComponent<Camera>();

        //mainCamera.fieldOfView = 35.6f;

        mainCameraDefaultPosition = mainCamera.transform.localPosition;
        mainCameraDefaultFOV = mainCamera.fieldOfView;

        pawn = transform.Find("Pawn");
        pawnReflection = transform.Find("PawnReflection");
        pawnSprite = pawn.GetComponent<SpriteRenderer>();
        hairSprite = pawn.Find("hair").GetComponent<SpriteRenderer>();
        eyeSprite = pawn.Find("eyes").GetComponent<SpriteRenderer>();
        
        pawnReflectionSprite = pawnReflection.GetComponent<SpriteRenderer>();

        //pawnReflectionSprite = transform.FindChild("PawnReflection").GetComponent<MeshRenderer>().material;

        hitBox = transform.Find("Player_Transparent");

        mount = transform.Find("Mount").GetComponent<SpriteRenderer>();
        mountPosition = mount.transform.localPosition;
    }

    void Start()
    {
        cam_origin = transform.Find("Camera").localPosition;
        
        if (!surfing)
        {
            updateMount(false);
        }

        updateAnimation("walk", walkFPS);
        StartCoroutine("animateSprite");
        animPause = true;

        reflect(false);
        followerScript.reflect(false);

        updateDirection(direction);
        
        updateCosmetics();

        StartCoroutine(control());


        //Check current map
        RaycastHit[] hitRays = Physics.RaycastAll(transform.position + Vector3.up, Vector3.down);
        int closestIndex;
        float closestDistance;

        CheckHitRaycastDistance(hitRays, out closestIndex, out closestDistance);

        if (closestIndex >= 0)
        {
            currentMap = hitRays[closestIndex].collider.gameObject.GetComponent<MapCollider>();
        }
        else
        {
            //if no map found
            //Check for map in front of player's direction
            hitRays = Physics.RaycastAll(transform.position + Vector3.up + getForwardVectorRaw(), Vector3.down);

            CheckHitRaycastDistance(hitRays, out closestIndex, out closestDistance);

            if (closestIndex >= 0)
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
            AudioClip audioClip = accessedMapSettings.getBGM();
            int loopStartSamples = accessedMapSettings.getBGMLoopStartSamples();

            if (accessedAudio != audioClip)
            {
                //if audio is not already playing
                accessedAudio = audioClip;
                accessedAudioLoopStartSamples = loopStartSamples;
                BgmHandler.main.PlayMain(accessedAudio, accessedAudioLoopStartSamples);
            }
            if (accessedMapSettings.mapNameAppears)
            {
                MapName.display(accessedMapSettings.mapName, accessedMapSettings.mapNameColor);
            }

            //Update Weather
            if (GameObject.Find("Weather") != null)
            {
                if (accessedMapSettings.weathers.Length > 0)
                {
                    int probabilityTotal = accessedMapSettings.sunnyWeatherProbability;
                    int currentProba = 0;
                    Weather weather = null;

                    foreach (WeatherProbability w in accessedMapSettings.weathers)
                    {
                        probabilityTotal += w.probability;
                    }
                    
                    Debug.Log("Probability Total : "+probabilityTotal);

                    float randValue = UnityEngine.Random.Range(1, probabilityTotal);
                    
                    Debug.Log("[Weather] Rand value = "+randValue);

                    if (accessedMapSettings.sunnyWeatherProbability > 0 && randValue > currentProba && randValue <= currentProba + accessedMapSettings.sunnyWeatherProbability)
                    {
                        // Do nothing
                    }
                    else
                    {
                        Debug.Log("[Weather] Choosing random weather");
                        currentProba = accessedMapSettings.sunnyWeatherProbability;
                        for (int i = 0; i < accessedMapSettings.weathers.Length; ++i)
                        {
                            if (accessedMapSettings.weathers[i].probability == 0) continue;
                        
                            if (randValue > currentProba && randValue <= currentProba + accessedMapSettings.weathers[i].probability)
                            {
                                weather = accessedMapSettings.weathers[i].weather;
                                Debug.Log("[Weather] Selected weather : "+weather.name);
                                break;
                            }

                            currentProba += accessedMapSettings.weathers[i].probability;
                        }
                    }

                    if (GameObject.Find("Weather") != null)
                    {
                        GameObject.Find("Weather").GetComponent<WeatherHandler>().setWeatherValue(weather);
                    }
                }
                else
                {
                    Debug.Log("[Weather] Weather List empty");
                    GameObject.Find("Weather").GetComponent<WeatherHandler>().setWeatherValue(null);
                }
            }

            //Update Discord Rich Presence
            UpdateRPC();
        }

        //NonResettingHandler Run
        if (GameObject.Find("Non-ResettingObjects") != null)
            GameObject.Find("Non-ResettingObjects").GetComponent<NonResettingHandler>().Run();

        //check position for transparent bumpEvents
        Collider transparentCollider = null;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.4f);

        transparentCollider = hitColliders.LastOrDefault(collider => collider.name.ToLowerInvariant().Contains("_transparent") &&
                !collider.name.ToLowerInvariant().Contains("player") &&
                !collider.name.ToLowerInvariant().Contains("follower"));

        if (transparentCollider != null)
        {
            //send bump message to the object's parent object
            transparentCollider.transform.parent.gameObject.SendMessage("bump", SendMessageOptions.DontRequireReceiver);
            Debug.Log("Bumping collider at start");
        }
        
        

        //DEBUG
        if (accessedMapSettings != null)
        {
            string pkmnNames = "";
            foreach(var encounter in accessedMapSettings.getEncounterList(WildPokemonInitialiser.Location.Standard))
            {
                pkmnNames += PokemonDatabase.getPokemon(encounter.ID).getName() + ", ";
            }
            Debug.Log("Wild Pokemon for map \"" + accessedMapSettings.mapName + "\": " + pkmnNames);
        }
        //

        GlobalVariables.global.resetFollower();
    }

    public void CheckHitRaycastDistance(RaycastHit[] hitRays, out int closestIndex, out float closestDistance)
    {
        closestIndex = -1;
        float closestDist = closestDistance = float.PositiveInfinity;
        
        foreach(RaycastHit hitRay in hitRays.Where(x => x.collider.gameObject.GetComponent<MapCollider>() != null && x.distance < closestDist))
        {
            closestDistance = hitRay.distance;
            closestIndex = Array.IndexOf(hitRays, hitRay);
        }
    }

    void Update()
    {
        //check for new inputs, so that the new direction can be set accordingly
        if (UnityEngine.Input.GetButtonDown("Horizontal"))
        {
            if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0)
            {
                //	Debug.Log("NEW INPUT: Right");
                mostRecentDirectionPressed = 1;
            }
            else if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0)
            {
                //	Debug.Log("NEW INPUT: Left");
                mostRecentDirectionPressed = 3;
            }
        }
        else if (UnityEngine.Input.GetButtonDown("Vertical"))
        {
            if (UnityEngine.Input.GetAxisRaw("Vertical") > 0)
            {
                //	Debug.Log("NEW INPUT: Up");
                mostRecentDirectionPressed = 0;
            }
            else if (UnityEngine.Input.GetAxisRaw("Vertical") < 0)
            {
                //	Debug.Log("NEW INPUT: Down");
                mostRecentDirectionPressed = 2;
            }
        }
    }

    public void UpdateRPC()
    {
        //Update Discord RPC
        float hour = System.DateTime.Now.Hour;

        string dayTime;

        if (hour < 6)
        {
            dayTime = "Night";
        }
        else if (hour < 12)
        {
            dayTime = "Morning";
        }
        else if (hour < 13)
        {
            dayTime = "Midday";
        }
        else if (hour < 18)
        {
            dayTime = "Afternoon";
        }
        else if (hour < 20)
        {
            dayTime = "Evening";
        }
        else
        {
            dayTime = "Night";
        }

        //Discord API Library
        //PresenceManager.UpdatePresence(
        //    detail: accessedMapSettings.mapName+" - "+dayTime, 
        //    state: "Exploring the region",
        //    largeKey: accessedMapSettings.RPCImageKey, 
        //    largeText: "",
        //    smallKey: "", 
        //    smallText: ""
        //);
    }

    private bool isDirectionKeyHeld(int directionCheck)
    {
        if ((directionCheck == 0 && UnityEngine.Input.GetAxisRaw("Vertical") > 0) ||
            (directionCheck == 1 && UnityEngine.Input.GetAxisRaw("Horizontal") > 0) ||
            (directionCheck == 2 && UnityEngine.Input.GetAxisRaw("Vertical") < 0) ||
            (directionCheck == 3 && UnityEngine.Input.GetAxisRaw("Horizontal") < 0))
        {
            return true;
        }
        return false;
    }

    private IEnumerator control()
    {
        bool still;

        yield return new WaitForSeconds(0.4f);
        
        //unpauseInput();
        while (true)
        {
            still = true;
                //the player is still, but if they've just finished moving a space, moving is still true for this frame (see end of coroutine)
            if (canInput)
            {
                if (!surfing && !bike)
                {
                    if (UnityEngine.Input.GetButton("Run"))
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
                if (UnityEngine.Input.GetButton("Start")) //Start
                {
                    //open Pause Menu
                    if (moving || UnityEngine.Input.GetButtonDown("Start")) //Start
                    {
                        if (setCheckBusyWith(Scene.main.Pause.gameObject))
                        {
                            animPause = true;
                            Scene.main.Pause.gameObject.SetActive(true);
                            StartCoroutine(Scene.main.Pause.control());
                            while (Scene.main.Pause.gameObject.activeSelf)
                            {
                                yield return null;
                            }
                            unsetCheckBusyWith(Scene.main.Pause.gameObject);
                        }
                    }
                }
                else if (UnityEngine.Input.GetButtonDown("Select"))
                {
                    interact();
                }
                //if pausing/interacting/etc. is not being called, then moving is possible.
                //		(if any direction input is being entered)
                else if (UnityEngine.Input.GetAxisRaw("Horizontal") != 0 || UnityEngine.Input.GetAxisRaw("Vertical") != 0)
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
                else if (UnityEngine.Input.GetKeyDown("g"))
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

    public Vector3 getCamOrigin()
    {
        return cam_origin;
    }

    public void updateDirection(int dir)
    {
        direction = dir;

        pawnReflectionSprite.sprite = pawnSprite.sprite = spriteSheet[direction * frames + frame];
        hairSprite.sprite = haircutSpriteSheet[direction * frames + frame];
        eyeSprite.sprite = eyeSpriteSheet[direction * frames + frame];

        if (mount.enabled)
        {
            mount.sprite = mountSpriteSheet[direction];
        }
    }

    private void updateMount(bool enabled, string spriteName = "")
    {
        mount.enabled = enabled;
        if (!mount.enabled)
        {
            mountSpriteSheet = Resources.LoadAll<UnityEngine.Sprite>("PlayerSprites/" + spriteName);
            mount.sprite = mountSpriteSheet[direction];
        }
    }

    public void updateAnimation(string newAnimationName, int fps)
    {
        if (animationName != newAnimationName)
        {
            animationName = newAnimationName;
            spriteSheet =
                Resources.LoadAll<UnityEngine.Sprite>("PlayerSprites/" + SaveData.currentSave.getPlayerSpritePrefix() +
                                          newAnimationName);
            haircutSpriteSheet = 
                Resources.LoadAll<UnityEngine.Sprite>("PlayerSprites/hairs/" + SaveData.currentSave.playerHaircut.getFileName() + '_' +
                                                           newAnimationName);
            eyeSpriteSheet = 
                Resources.LoadAll<UnityEngine.Sprite>("PlayerSprites/eyes/" + SaveData.currentSave.getPlayerSpritePrefix() +
                                          newAnimationName);
            //TODO Add eyes too
            
            //pawnReflectionSprite.SetTexture("_MainTex", Resources.Load<Texture>("PlayerSprites/"+SaveData.currentSave.getPlayerSpritePrefix()+newAnimationName));
            framesPerSec = fps;
            secPerFrame = 1f / (float) framesPerSec;
            frames = Mathf.RoundToInt((float) spriteSheet.Length / 4f);
            if (frame >= frames)
            {
                frame = 0;
            }
        }
    }

    public void updateCosmetics()
    {
        hairSprite.color = SaveData.currentSave.playerHairColor.Color;
        eyeSprite.color = SaveData.currentSave.playerEyeColor.Color;
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
                hairSprite.sprite = haircutSpriteSheet[direction * frames + frame];
                eyeSprite.sprite = eyeSpriteSheet[direction * frames + frame];
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
        yield return StartCoroutine(checkBusinessBeforeUnpause(waitTime, true));
    }
    
    public IEnumerator checkBusinessBeforeUnpause(float waitTime, bool busyConstraint)
    {
        yield return new WaitForSeconds(waitTime);
        if (busyConstraint)
        {
            if (PlayerMovement.player.busyWith == null)
            {
                unpauseInput();
            }
            else
            {
                Debug.Log("Busy with " + PlayerMovement.player.busyWith);
            }
        }
    }

    public void pauseInput(float secondsToWait = 0f)
    {
        canInput = false;
        if (animationName == "run")
        {
            updateAnimation("walk", walkFPS);
        }
        if (running || bike)
        {
            speed = walkSpeed;
        }
        running = false;

        if (secondsToWait == 0f)
        {
            StartCoroutine(checkBusinessBeforeUnpause(secondsToWait, false));
        }
        else
        {
            StartCoroutine(checkBusinessBeforeUnpause(secondsToWait, true));
        }
        
    }

    public void unpauseInput()
    {
        Debug.Log("unpaused");
        canInput = true;
    }

    public bool isInputPaused()
    {
        return !canInput;
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

    public AudioSource getAudio()
    {
        return PlayerAudio;
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
            foreach (RaycastHit hitCollider in hitColliders)
            {
                //if map has not been found yet
                if (mapHit.collider == null)
                {
                    //if a collision's gameObject has a mapCollider, it is a map. set it to be the destination map.
                    if (hitCollider.collider.gameObject.GetComponent<MapCollider>() != null)
                    {
                        mapHit = hitCollider;
                        destinationMap = mapHit.collider.gameObject.GetComponent<MapCollider>();
                    }
                }
                else if ((bridgeHit.collider != null && checkForBridge) || mapHit.collider != null)
                {
                    //if both have been found
                    break; //stop searching
                }
                //if bridge has not been found yet
                if (bridgeHit.collider == null && checkForBridge && hitCollider.collider.gameObject.GetComponent<BridgeHandler>() != null)
                {
                    //if a collision's gameObject has a BridgeHandler, it is a bridge.
                    bridgeHit = hitCollider;
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
        if ((currentSlope <= 1 && destinationSlope <= 1) && (yDistance <= currentSlope || yDistance <= destinationSlope))
        {
            //if yDistance is greater than both slopes there is a vertical wall between them
            return movement;
        }
        return Vector3.zero;
    }

    ///Make the player move one space in the direction they are facing
    private IEnumerator moveForward()
    {
        Vector3 movement = getForwardVector();

        bool ableToMove = false;

        Collider objectCollider = null;

        //without any movement, able to move should stay false
        if (movement != Vector3.zero)
        {
            //check destination for objects/transparents
            Collider transparentCollider = null;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + movement + new Vector3(0, 0.5f, 0), 0.4f);

            for (int i = 0; i < hitColliders.Length; i++)
            {
                Debug.Log("Collider: "+hitColliders[i].ToString());
                if (hitColliders[i].name.ToLowerInvariant().Contains("_object"))
                {
                    objectCollider = hitColliders[i];
                }
                else if (hitColliders[i].name.ToLowerInvariant().Contains("_transparent") && !hitColliders[i].name.ToLowerInvariant().Contains("follower"))
                {
                    transparentCollider = hitColliders[i];
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
                            
                            if (accessedMapSettings.mapNameAppears)
                            {
                                MapName.display(accessedMapSettings.mapName, accessedMapSettings.mapNameColor);
                            }
                            
                            Debug.Log(destinationMap.name + "   " + accessedAudio.name);
                            
                            //Update Weather
                            if (accessedMapSettings.weathers.Length > 0)
                            {
                                int probabilityTotal = accessedMapSettings.sunnyWeatherProbability;
                                int currentProba = 0;
                                Weather weather = null;

                                foreach (WeatherProbability w in accessedMapSettings.weathers)
                                {
                                    probabilityTotal += w.probability;
                                }
                                
                                Debug.Log("Probability Total : "+probabilityTotal);

                                float randValue = UnityEngine.Random.Range(1, probabilityTotal);
                                
                                Debug.Log("[Weather] Rand value = "+randValue);

                                if (accessedMapSettings.sunnyWeatherProbability > 0 && randValue > currentProba && randValue <= currentProba + accessedMapSettings.sunnyWeatherProbability)
                                {
                                    // Do nothing
                                }
                                else
                                {
                                    Debug.Log("[Weather] Choosing random weather");
                                    currentProba = accessedMapSettings.sunnyWeatherProbability;
                                    for (int i = 0; i < accessedMapSettings.weathers.Length; ++i)
                                    {
                                        if (accessedMapSettings.weathers[i].probability == 0) continue;
                                    
                                        if (randValue > currentProba && randValue <= currentProba + accessedMapSettings.weathers[i].probability)
                                        {
                                            weather = accessedMapSettings.weathers[i].weather;
                                            Debug.Log("[Weather] Selected weather : "+weather.name);
                                            break;
                                        }

                                        currentProba += accessedMapSettings.weathers[i].probability;
                                    }
                                }
                                
                                GameObject.Find("Weather").GetComponent<WeatherHandler>().setWeather(weather);
                            }
                            else
                            {
                                Debug.Log("[Weather] Weather List empty");
                                GameObject.Find("Weather").GetComponent<WeatherHandler>().setWeather(null);
                            }
                            
                            this.UpdateRPC();
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
            if (objectCollider == null || (objectCollider.transform.parent.GetComponent<InteractDoorway>() == null || 
                                           objectCollider.transform.parent.GetComponent<InteractDoorway>() != null && objectCollider.transform.parent.GetComponent<InteractDoorway>().isLocked))
            {
                Invoke("playBump", 0.05f);
            }
            moving = false;
            animPause = true;
        }
    }

    public IEnumerator move(Vector3 movement, bool encounter = true, bool lockFollower = false)
    {
        if (movement != Vector3.zero)
        {
            Vector3 startPosition = hitBox.position;
            moving = true;
            increment = 0;

            if (!lockFollower)
            {
                StartCoroutine(followerScript.move(startPosition, speed));
            }

            if (npcFollower != null)
            {
                StartCoroutine(npcFollower.move(startPosition, speed));
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
                        StartCoroutine(PlayerMovement.player.wildEncounter(WildPokemonInitialiser.Location.Surfing));
                    }
                    else
                    {
                        //land tile
                        StartCoroutine(PlayerMovement.player.wildEncounter(WildPokemonInitialiser.Location.Standard));
                    }
                }
            }
        }
    }

    public IEnumerator moveCameraTo(Vector3 targetPosition, float duration)
    {
        targetPosition += mainCameraDefaultPosition;

        LeanTween.move(mainCamera.gameObject, targetPosition, duration);
        yield return new WaitForSeconds(duration);

        mainCamera.transform.localPosition = targetPosition;
    }

    public void forceMoveForward(int spaces = 1)
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
            Collider objectCollider = null;
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
        if (GlobalVariables.global.followerOut)
        {
            followerScript.ActivateMove();
        }
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
                StartCoroutine(surfCheck());
            }
        }
    }

    public IEnumerator shakeCamera(int iteration, float duration, AudioClip clip = null)
    {
        Transform camera = transform.Find("Camera") != null ? transform.Find("Camera") : GameObject.Find("Camera").transform;

        for (int i = 0; i < iteration; ++i)
        {
            float increment = 0f;
            float distance = 0.2f;
            Vector3 startPosition = camera.position;

            playClip(clip);

            LeanTween.moveX(camera.gameObject, camera.position.x+distance, duration/4);
            yield return new WaitForSeconds(duration/4);
            LeanTween.moveX(camera.gameObject, camera.position.x-distance*2, duration/2);
            yield return new WaitForSeconds(duration/2);
            LeanTween.moveX(camera.gameObject, camera.position.x+distance, duration/4);
            yield return new WaitForSeconds(duration/4);
            
            camera.position = startPosition;
        }
        
        yield return null;
    }
    
    public void playerJump()
    {
        if (!jumping)
        {
            StartCoroutine(jump());
        }
    }

    public IEnumerator jump()
    {
        jumping = true;
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
        pawn.position = new Vector3(pawn.position.x, startPosition.y, pawn.position.z);

        playClip(landClip);
        
        jumping = false;
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
        StartCoroutine(stillMount());
        yield return StartCoroutine(jump());
        followerScript.canMove = true;
        mount.transform.localPosition = mountPosition;
        updateMount(false);
    }

    private IEnumerator surfCheck()
    {
        IPokemon targetPokemon = null; //SaveData.currentSave.PC.getFirstFEUserInParty("Surf");
        if (targetPokemon != null)
        {
            if (getForwardVector(direction, false) != Vector3.zero)
            {
                if (setCheckBusyWith(this.gameObject))
                {
                    Dialog.DrawDialogBox();
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText("The water is dyed a deep blue. Would you \nlike to surf on it?"));
                    yield return Dialog.StartCoroutine(Dialog.DrawChoiceBox());
                    Dialog.UndrawChoiceBox();
                    int chosenIndex = Dialog.chosenIndex;
                    if (chosenIndex == 1)
                    {
                        Dialog.DrawDialogBox();
                        yield return
                            //Dialog.StartCoroutine(Dialog.DrawText(targetPokemon.Name + " used " + targetPokemon.getFirstFEInstance("Surf") + "!"));
                            Dialog.StartCoroutine(Dialog.DrawText(targetPokemon.Name + " used " + "Surf" + "!"));
                        while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
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

                        followerScript.StartCoroutine(followerScript.withdrawToBall());
                        StartCoroutine(stillMount());
                        forceMoveForward();
                        yield return StartCoroutine(jump());

                        updateAnimation("surf", walkFPS);
                        speed = surfSpeed;
                    }
                    Dialog.UndrawDialogBox();
                    unsetCheckBusyWith(this.gameObject);
                }
            }
        }
        else
        {
            if (setCheckBusyWith(this.gameObject))
            {
                Dialog.DrawDialogBox();
                yield return Dialog.StartCoroutine(Dialog.DrawText("The water is dyed a deep blue."));
                while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                Dialog.UndrawDialogBox();
                unsetCheckBusyWith(this.gameObject);
            }
        }
        yield return new WaitForSeconds(0.2f);
    }

    public IEnumerator wildEncounter(WildPokemonInitialiser.Location encounterLocation)
    {
        if (accessedMapSettings.getEncounterList(encounterLocation).Length > 0)
        {
            if (UnityEngine.Random.value <= accessedMapSettings.getEncounterProbability())
            {
                if (setCheckBusyWith(Scene.main.Battle.gameObject))
                {
                    IPokemon wildpkm = accessedMapSettings.getRandomEncounter(encounterLocation);
            
                    Scene.main.Battle.PlayWildBGM(wildpkm);

                    //SceneTransition sceneTransition = Dialog.transform.GetComponent<SceneTransition>();

                    yield return StartCoroutine(ScreenFade.main.FadeCutout(false, ScreenFade.slowedSpeed, null));
                    //yield return new WaitForSeconds(sceneTransition.FadeOut(1f));
                    Scene.main.Battle.gameObject.SetActive(true);
                    StartCoroutine(Scene.main.Battle.control(wildpkm));

                    while (Scene.main.Battle.gameObject.activeSelf)
                    {
                        yield return null;
                    }

                    //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                    yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));

                    unsetCheckBusyWith(Scene.main.Battle.gameObject);
                }
            }
        }
    }
    
    public IEnumerator startWildBattle()
    {
        if (setCheckBusyWith(Scene.main.Battle.gameObject))
        {
            IPokemon wildpkm = accessedMapSettings.getRandomEncounter(WildPokemonInitialiser.Location.Grass);
            
            Scene.main.Battle.PlayWildBGM(wildpkm);

            //SceneTransition sceneTransition = Dialog.transform.GetComponent<SceneTransition>();

            yield return StartCoroutine(ScreenFade.main.FadeCutout(false, ScreenFade.slowedSpeed, null));
            //yield return new WaitForSeconds(sceneTransition.FadeOut(1f));
            Scene.main.Battle.gameObject.SetActive(true);
            StartCoroutine(Scene.main.Battle.control(wildpkm));

            while (Scene.main.Battle.gameObject.activeSelf)
            {
                yield return null;
            }

            //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
            yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));

            unsetCheckBusyWith(Scene.main.Battle.gameObject);
        }
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