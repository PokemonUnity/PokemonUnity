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
using static UnityEngine.InputSystem.InputAction;
using System.Collections.Generic;
using MarkupAttributes;

[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour, INeedDirection {
    public static PlayerMovement Singleton;
    public PlayerSO Player;

    #region Variables

    //before a script runs, it'll check if the player is busy with another script's GameObject.
    public GameObject busyWith = null;
    public bool canInput = true;
    private DialogBoxHandlerNew Dialog;

    #region Gizmo Variables

    [Foldout("Gizmos")]
    public bool DrawGizmos = false;
    public bool DrawMovementGizmos = false;
    public bool DrawInteractBox = false;
    [HideInInspector] public Ray PositionCheckRay;
    [HideInInspector] public Ray CollisionCheckRay;
    [HideInInspector] public Vector3 MovementRay;
    [HideInInspector] public bool MovementCheckIsValid = false;

    #endregion

    [Foldout("Interactions")]
    Vector3 interactPosition;
    public float InteractRadius = 0.4f;

    [Foldout("Camera")]
    public Camera PlayerCamera;
    public Vector3 MainCameraDefaultPosition;
    public float MainCameraDefaultFOV;
    private Vector3 camOrigin;

    [Foldout("Movement")]
    [SerializeField] DirectionSurrogate directionSurrogate;
    public bool IsMoving = false;
    Vector3 possibleMovement;
    bool shouldTryToMove = false;
    public bool IsStanding = true;
    public bool IsRunning = false;
    public bool IsSurfing = false;
    public bool IsBiking = false;
    public bool strength = false;
    public float WalkSpeed = 0.3f; //time in seconds taken to walk 1 square.
    public float RunSpeed = 0.15f;
    public float SurfSpeed = 0.2f;
    public float Speed;
    [Obsolete("Moved from int based direction to Vector3. See FacingDirection", false)]
    public int Direction = 2; //0 = up, 1 = right, 2 = down, 3 = left
    private bool jumping = false;
    public float Increment = 1f;
    public int WalkFPS = 7;
    public int RunFPS = 12;
    private Vector3 directionInput = Vector3.zero;

    [Foldout("Player")]
    //Player sprites
    [SerializeField] Transform pawn;
    [SerializeField] SpriteRenderer pawnSprite;
    [SerializeField] SpriteRenderer eyeSprite;
    [SerializeField] SpriteRenderer hairSprite;
    [SerializeField] SpriteRenderer pawnReflectionSprite;
    //private Material pawnReflectionSprite;
    [SerializeField] SpriteAnimatorBehaviour animator;
    //[SerializeField] UnityEngine.Sprite[] spriteSheet;
    public Transform Hitbox;

    [Foldout("Followers")]
    public FollowerMovement Follower;
    public NPCFollower NpcFollower;

    [Foldout("Map")]
    public MapCollider currentMapCollider;
    public PokemonMapBehaviour newMap;

    [Foldout("Audio")]
    private AudioSource playerAudio;
    public AudioClip bumpClip;
    public AudioClip jumpClip;
    public AudioClip landClip;

    [Foldout("Mount")]
    [SerializeField] MountBehaviour mount;
    Vector3 mountPosition;

    [Foldout("Animations")]
    private string animationName;
    private UnityEngine.Sprite[] haircutSpriteSheet;
    private UnityEngine.Sprite[] eyeSpriteSheet;
    private UnityEngine.Sprite[] mountSpriteSheet;
    private int frame;
    private int frames;
    public int framesPerSec;
    private bool overrideAnimPause;

    public DirectionSurrogate DirectionSurrogate { get => directionSurrogate; }
    
    public Vector3 FacingDirection { get => directionSurrogate.FacingDirection; }

    #endregion

    #region MonoBehaviour Functions

    void OnDrawGizmos() {
        if (!DrawGizmos) return;
        if (DrawMovementGizmos) {
            // position check ray
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(PositionCheckRay.origin, PositionCheckRay.origin + (PositionCheckRay.direction * 3f));
            // movement collision checker
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + possibleMovement);
            // possible movement
            Gizmos.color = MovementCheckIsValid ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, transform.position + possibleMovement);
        }
        if (DrawInteractBox) {
            Gizmos.DrawSphere(interactPosition, InteractRadius);
        }
    }

    void OnValidate() {
        if (Follower == null) Debug.LogError("No FollowerMovement provided", gameObject);
        // pawn stuff
        if (pawn == null) Debug.LogError("No Pawn Transform provided", gameObject);
        else {
            if (pawnSprite == null) pawnSprite = pawn.GetComponent<SpriteRenderer>();
        }
        //if (hairSprite == null) Debug.LogError("Couldn't find the Pawns hair", gameObject);
        //if (eyeSprite == null) Debug.LogError("Couldn't find the Pawns eyes", gameObject);
        if (pawnReflectionSprite == null) Debug.LogError("No Pawn Reflection SpriteRenderer provided", gameObject);
        //
        if (Hitbox == null) Debug.LogError("No hitBox Tranform provided", gameObject);
        if (mount == null) Debug.LogError("No mount sprite provided", gameObject);
        if (Player == null) Debug.LogError("No Player Scriptable Object provided", gameObject);
        if (animator == null) {
            animator = GetComponent<SpriteAnimatorBehaviour>();
            if (animator == null) 
                Debug.LogError("No mount sprite found or provided", gameObject);
        }
        if (animator != null && Player != null) {
            animator.Animations = Player.Animations;
            SwitchAnimation("Idle");
            DirectionSurrogate.OnDirectionUpdated.AddListener(SwitchAnimation);
        }
    }

    void Awake() {
        playerAudio = transform.GetComponent<AudioSource>();

        //set up the reference to this script.
        Singleton = this;

        // FIXME
        //Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandlerNew>();
        //MapName = GameObject.Find("GUI").GetComponent<MapNameBoxBehaviour>();

        canInput = true;
        Speed = WalkSpeed;

        PlayerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (PlayerCamera == null) Debug.LogError("Could not find a Camera tagged with 'MainCamera'");

        //mainCamera.fieldOfView = 35.6f;

        MainCameraDefaultPosition = PlayerCamera.transform.localPosition;
        MainCameraDefaultFOV = PlayerCamera.fieldOfView;

        // hmmm... delete me?
        mountPosition = mount.transform.localPosition;
    }

    void Start() {
        camOrigin = PlayerCamera.transform.localPosition;

        UpdateDirection(Direction);
        updateCosmetics();

        // mount
        if (!IsSurfing)
            mount.UpdateMount(false);

        // animation
        SwitchAnimation("Idle");

        // reflections
        Reflect(false);
        Follower.reflect(false);

        //StartCoroutine(control());

        //Check current map
        currentMapCollider = MapCollider.GetMap(transform.position, FacingDirection);

        if (currentMapCollider != null) {
            newMap = currentMapCollider.GetComponent<PokemonMapBehaviour>();
            PokemonMapBehaviour.UpdateMap(newMap);
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
        if (newMap != null)
        {
            string pkmnNames = "";
            foreach(var encounter in newMap.getEncounterList(WildPokemonInitialiser.Location.Standard))
            {
                pkmnNames += PokemonDatabase.getPokemon(encounter.ID).getName() + ", ";
            }
            Debug.Log("Wild Pokemon for map \"" + newMap.Map.Name + "\": " + pkmnNames);
        }
        //

        GlobalVariables.Singleton.resetFollower();
    }

    void Update() {
        interactPosition = new Vector3(transform.position.x, (transform.position.y + 0.5f), transform.position.z) + possibleMovement;
    }

    #endregion

    #region Input Handling

    public void PauseInput(float secondsToWait = 0f) {
        canInput = false;
        if (animationName == "run")
            SwitchAnimation("walk");

        if (IsRunning || IsBiking)
            Speed = WalkSpeed;

        IsRunning = false;

        if (secondsToWait == 0f)
            StartCoroutine(checkBusinessBeforeUnpause(secondsToWait, false));
        else
            StartCoroutine(checkBusinessBeforeUnpause(secondsToWait, true));
    }

    public void UnpauseInput() {
        Debug.Log("unpaused");
        canInput = true;
    }

    public bool IsInputPaused() => !canInput;

    public void HandleInputRun(CallbackContext context) {
        IsRunning = context.action.IsPressed();
    }

    public void HandleInputStart(CallbackContext context) {
        canInput = false;
        StartCoroutine(openPauseMenu());
    }

    public void HandleInputInteract(CallbackContext context) {
        interact();
    }

    public void HandleInputDebug(CallbackContext context) {
        Debug.Log(currentMapCollider.GetTileTag(transform.position));
        if (Follower.CanMove)
            Follower.StartCoroutine(Follower.withdrawToBall());
        else
            Follower.CanMove = true;
    }

    public void HandleInputMove(CallbackContext context) {
        var input = context.ReadValue<Vector2>();
        directionInput = new Vector3(input.x, 0f, input.y);
        this.shouldTryToMove = shouldTryToMove();
        if (this.shouldTryToMove && !IsMoving)
            move();

        bool shouldTryToMove() => context.action.phase != UnityEngine.InputSystem.InputActionPhase.Canceled;
    }

    #endregion

    #region Movement

    #region Direction

    [Obsolete("Moved from int based direction to Vector3", false)]
    public void UpdateDirection(int dir) {
        Direction = dir;

        return; //FIXME
        //pawnReflectionSprite.sprite = pawnSprite.sprite = spriteSheet[Direction * frames + frame];
        //hairSprite.sprite = haircutSpriteSheet[Direction * frames + frame];
        //eyeSprite.sprite = eyeSpriteSheet[Direction * frames + frame];

        //mount.UpdateDirection((EMovementDirection)dir);
    }

    public void UpdateDirection(Vector3 newDirection) {
        if (shouldChangeDirection(directionInput)) {
            Direction = (int)newDirection.ToMovementDirection(Vector3.forward, Vector3.up);
            directionSurrogate.UpdateDirection(newDirection);
            mount.UpdateDirection(FacingDirection);
            //FIXME
            //pawnReflectionSprite.sprite = pawnSprite.sprite = spriteSheet[direction * frames + frame];
            //hairSprite.sprite = haircutSpriteSheet[direction * frames + frame];
            //eyeSprite.sprite = eyeSpriteSheet[direction * frames + frame];
            return;
        }
        //Vector3.RotateTowards(DirectionSurrogate.forward, input.GetForwardVector(), Mathf.PI, 0f);

        bool shouldChangeDirection(Vector3 newDirection) => !IsMoving && newDirection.magnitude > 0 && newDirection != FacingDirection;
    }

    public Vector3 GetFacingDirection() => FacingDirection;

    [Obsolete]
    public Vector3 GetMovementVector(EMovementDirection direction) => GetMovementVector(direction, true);

    [Obsolete]
    public Vector3 GetMovementVector(EMovementDirection direction, bool checkForBridge) {
        return GetMovementVector(direction.ToVector(), checkForBridge);
    }

    public Vector3 GetMovementVector() => GetMovementVector(FacingDirection, true);

    public Vector3 GetMovementVector(Vector3 facingDirection, bool checkForBridge) {
        //set initial vector3 based off of direction
        Vector3 movement = facingDirection;

        //Check destination map	and bridge																//0.5f to adjust for stair height
        //cast a ray directly downwards from the position directly in front of the player		//1f to check in line with player's head

        Ray positionCheckRay = new Ray(transform.position + movement + new Vector3(0, 1.5f, 0), Vector3.down);
        RaycastHit[] hitColliders = Physics.RaycastAll(positionCheckRay, 3f);
        PositionCheckRay = positionCheckRay;

        // TODO: Theres definitely a better way to do this, I just know it
        RaycastHit mapHit = getMapHit(hitColliders);
        //if a collision's gameObject has a BridgeHandler, it is a bridge.
        RaycastHit bridgeHit = new RaycastHit();
        if (checkForBridge)
            bridgeHit = getBridgeHit(hitColliders);

        // modify the forwards vector to align to the colliding plane
        RaycastHit higherPriorityHit = bridgeHit.collider == null ? mapHit : bridgeHit;
        if (higherPriorityHit.collider == null) return Vector3.zero; // Hit nothing

        movement -= new Vector3(0, (transform.position.y - higherPriorityHit.point.y), 0);
        possibleMovement = movement;

        Vector3 positionTranslatedUpwards = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        Ray collisionCheckRay = new Ray(positionTranslatedUpwards, movement.normalized);
        CollisionCheckRay = positionCheckRay;
        hitColliders = Physics.RaycastAll(collisionCheckRay, movement.magnitude);

        // did not collide with anything moving from our position to our destination
        mapHit = getMapHit(hitColliders);
        bridgeHit = getBridgeHit(hitColliders);
        if (mapHit.collider == null && bridgeHit.collider == null) {
            MovementCheckIsValid = true;
            return movement;
        }

        MovementCheckIsValid = false;
        return Vector3.zero;

        RaycastHit getMapHit(RaycastHit[] hits) => Array.Find(hits, (RaycastHit hit) => hit.collider.gameObject.GetComponent<MapCollider>() != null);
        RaycastHit getBridgeHit(RaycastHit[] hits) => Array.Find(hits, (RaycastHit hit) => hit.collider.gameObject.GetComponent<BridgeHandler>() != null);
    }

    #endregion

    [Obsolete]
    public IEnumerator move(Vector3 movement, bool canEncounter = true, bool lockFollower = false) {
        move(canEncounter, lockFollower);
        yield break;
    }

    public void move(bool canEncounter = true, bool lockFollower = false) {
        if (!canInput) return;

        UpdateDirection(directionInput);
        Vector3 movement = GetMovementVector(FacingDirection, true);

        //yield return new WaitForSeconds(0.4f);

        Collider objectCollider = null;

        if (movement != Vector3.zero) {
            objectCollider = checkForObjectCollision(movement);
            //if no objects are in the way
            if (objectCollider == null) {
                MapCollider destinationMap = MapCollider.GetMap(transform.position, FacingDirection);
                EMapTile destinationTileTag = (EMapTile)destinationMap.GetTileTag(transform.position + movement);
                bool collidedWithBridge = checkForBridgeCollision(movement);
                if (canMove(collidedWithBridge, destinationTileTag)) {
                    if (canSurfAtDestination(collidedWithBridge, destinationTileTag)) {
                        // TODO: destination is water tile
                    } else {
                        //disable surfing if not headed to water tile
                        if (aboutToStopSurfing(destinationTileTag))
                            stopSurfing();

                        if (destinationMap != currentMapCollider) {
                            currentMapCollider = destinationMap;
                            PokemonMapBehaviour.UpdateMap(destinationMap.GetComponent<PokemonMapBehaviour>());
                        }

                        Vector3 startPosition = transform.position;

                        processTransparentCollision(movement);

                        IsMoving = true;
                        Increment = 0;

                        if (!lockFollower) Follower.move(startPosition, Speed);
                        if (NpcFollower != null) StartCoroutine(NpcFollower.move(startPosition, Speed));

                        updateAnimation();
                        LTDescr tween = LeanTween.move(gameObject, startPosition + movement, Speed);
                        tween.setOnComplete(() => {
                            IsMoving = false;
                            // move again if input is still being provided
                            if (shouldTryToMove)
                                move();
                            else
                                SwitchAnimation("Idle");
                        });
                        //Hitbox.position = startPosition + movement;

                        //yield return move(startPosition, movement);

                        if (canEncounter)
                            CheckForEncounters();

                        return;
                    }
                }
            } else {
                Debug.Log($"Collided with {objectCollider.gameObject.name}");
            }
        }

        // We never moved, for whatever reason
        if (!IsMoving) playerIsUnableToMove(objectCollider);

        IsMoving = false;
        SwitchAnimation("Idle");

        #region Helpers
        void updateAnimation() {
            if (!IsSurfing && !IsBiking) {
                if (IsRunning) {
                    if (IsMoving) {
                        SwitchAnimation("run");
                    } else {
                        SwitchAnimation("walk");
                    }
                    Speed = RunSpeed;
                } else {
                    SwitchAnimation("walk");
                    Speed = WalkSpeed;
                }
            }
        }
        bool checkForBridgeCollision(Vector3 movement) => MapCollider.GetBridgeHitOfPosition(transform.position + movement + new Vector3(0, 0.1f, 0)).collider != null;
        bool canMove(bool collidedWithBridge, EMapTile destinationTileTag) => collidedWithBridge || destinationTileTag != EMapTile.Impassable;
        bool canSurfAtDestination(bool collidedWithBridge, EMapTile destinationTileTag) => !collidedWithBridge && !IsSurfing && destinationTileTag == EMapTile.SurfableWater;
        bool aboutToStopSurfing(EMapTile destinationTileTag) => IsSurfing && destinationTileTag != EMapTile.SurfableWater;
        void processTransparentCollision(Vector3 movement) {
            Collider transparentCollider = checkForTransparentObjectCollision(movement);

            //send bump message to the transparents's parent object
            if (transparentCollider != null)
                transparentCollider.transform.parent.gameObject.SendMessage("bump", SendMessageOptions.DontRequireReceiver);
        }
        void CheckForEncounters() {
            EMapTile destinationTag = (EMapTile)currentMapCollider.GetTileTag(transform.position);
            if (destinationTag != EMapTile.Impassable)
                if (destinationTag == EMapTile.SurfableWater)
                    StartCoroutine(wildEncounter(WildPokemonInitialiser.Location.Surfing));
                else if (destinationTag == EMapTile.Default)
                    StartCoroutine(wildEncounter(WildPokemonInitialiser.Location.Standard));
        }
        void playerIsUnableToMove(Collider objectCollider) {
            if (objectCollider == null || collidingWithLockedDoor(objectCollider)) {
                Invoke("playBump", 0.05f);
            }
            SwitchAnimation("Idle");

            bool collidingWithLockedDoor(Collider objectCollider) => objectCollider.transform.parent.TryGetComponent(out InteractDoorway doorway) && doorway.isLocked;
        }
        #endregion
    }

    public void forceMoveForward(int spaces = 1) {
        StartCoroutine(forceMoveForwardIE(spaces));
    }

    IEnumerator forceMoveForwardIE(int spaces) {
        overrideAnimPause = true;
        for (int i = 0; i < spaces; i++) {
            Vector3 movement = GetMovementVector();

            //check destination for transparents
            Collider objectCollider = null;
            Collider transparentCollider = null;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + movement + new Vector3(0, 0.5f, 0),
                0.4f);
            if (hitColliders.Length > 0) {
                for (int i2 = 0; i2 < hitColliders.Length; i2++) {
                    if (hitColliders[i2].name.ToLowerInvariant().Contains("_transparent")) {
                        transparentCollider = hitColliders[i2];
                    }
                }
            }
            if (transparentCollider != null) {
                //send bump message to the transparents's parent object
                transparentCollider.transform.parent.gameObject.SendMessage("bump",
                    SendMessageOptions.DontRequireReceiver);
            }

            yield return StartCoroutine(move(movement, false));
        }
        overrideAnimPause = false;
        if (GlobalVariables.Singleton.isFollowerOut) {
            Follower.ActivateMove();
        }
    }

    #region Movement Actions

    public void playerJump() {
        if (!jumping)
            StartCoroutine(jump());
    }

    public IEnumerator jump() {
        jumping = true;
        float increment = 0f;
        float parabola = 0;
        float height = 2.1f;
        Vector3 startPosition = pawn.position;

        playClip(jumpClip);

        while (increment < 1) {
            increment += (1 / WalkSpeed) * Time.deltaTime;
            if (increment > 1)
                increment = 1;

            parabola = -height * (increment * increment) + (height * increment);
            pawn.position = new Vector3(pawn.position.x, startPosition.y + parabola, pawn.position.z);
            yield return null;
        }
        pawn.position = new Vector3(pawn.position.x, startPosition.y, pawn.position.z);

        playClip(landClip);

        jumping = false;
    }

    private IEnumerator dismount() {
        StartCoroutine(mount.StillMount(Speed));
        yield return StartCoroutine(jump());
        Follower.CanMove = true;
        mount.transform.localPosition = mountPosition;
        mount.UpdateMount(false);
    }

    private IEnumerator surfCheck() {
        IPokemon targetPokemon = null; //SaveData.currentSave.PC.getFirstFEUserInParty("Surf");
        if (targetPokemon != null) {
            if (GetMovementVector((EMovementDirection)Direction, false) != Vector3.zero) {
                if (setCheckBusyWith(this.gameObject)) {
                    Dialog.DrawDialogBox();
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText("The water is dyed a deep blue. Would you \nlike to surf on it?"));
                    yield return Dialog.StartCoroutine(Dialog.DrawChoiceBox());
                    Dialog.UndrawChoiceBox();
                    int chosenIndex = Dialog.chosenIndex;
                    if (chosenIndex == 1) {
                        Dialog.DrawDialogBox();
                        yield return
                            //Dialog.StartCoroutine(Dialog.DrawText(targetPokemon.Name + " used " + targetPokemon.getFirstFEInstance("Surf") + "!"));
                            Dialog.StartCoroutine(Dialog.DrawText(targetPokemon.Name + " used " + "Surf" + "!"));
                        while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                            yield return null;

                        IsSurfing = true;
                        mount.UpdateMount(true, "surf");

                        BackgroundMusicHandler.Singleton.PlayMain(GlobalVariables.Singleton.surfBGM, GlobalVariables.Singleton.surfBgmLoopStart);

                        //determine the vector for the space in front of the player by checking direction
                        Vector3 spaceInFront = new Vector3(0, 0, 0);
                        if (Direction == 0)
                            spaceInFront = new Vector3(0, 0, 1);
                        else if (Direction == 1)
                            spaceInFront = new Vector3(1, 0, 0);
                        else if (Direction == 2)
                            spaceInFront = new Vector3(0, 0, -1);
                        else if (Direction == 3)
                            spaceInFront = new Vector3(-1, 0, 0);

                        mount.transform.position = mount.transform.position + spaceInFront;

                        Follower.StartCoroutine(Follower.withdrawToBall());
                        StartCoroutine(mount.StillMount(Speed));
                        forceMoveForward();
                        yield return StartCoroutine(jump());

                        SwitchAnimation("surf");
                        Speed = SurfSpeed;
                    }
                    Dialog.UndrawDialogBox();
                    unsetCheckBusyWith(this.gameObject);
                }
            }
        } else {
            if (setCheckBusyWith(this.gameObject)) {
                Dialog.DrawDialogBox();
                yield return Dialog.StartCoroutine(Dialog.DrawText("The water is dyed a deep blue."));
                while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                    yield return null;

                Dialog.UndrawDialogBox();
                unsetCheckBusyWith(this.gameObject);
            }
        }
        yield return new WaitForSeconds(0.2f);
    }

    void stopSurfing() {
        SwitchAnimation("walk");
        Speed = WalkSpeed;
        IsSurfing = false;
        StartCoroutine(dismount());
        // FIXME
        //BgmHandler.main.PlayMain(backgroundMusic, accessedAudioLoopStartSamples);
    }

    #endregion

    #endregion

    #region Collisions

    Collider[] collisionRaycast(Vector3 checkOffset) {
        return Physics.OverlapSphere(transform.position + checkOffset + new Vector3(0, 0.5f, 0), 0.4f);
    }

    /// <summary>Returns the first collider hit</summary>
    Collider checkForTransparentObjectCollision(Vector3 checkOffset) {
        Collider[] hitColliders = collisionRaycast(checkOffset);
        for (int i = 0; i < hitColliders.Length; i++) {
            //Debug.Log("Collider: " + hitColliders[i].ToString());
            if (hitColliders[i].name.ToLowerInvariant().Contains("_transparent") &&
                !hitColliders[i].name.ToLowerInvariant().Contains("player") &&
                !hitColliders[i].name.ToLowerInvariant().Contains("follower")) {
                hitColliders[i].transform.parent.gameObject.SendMessage("bump", SendMessageOptions.DontRequireReceiver);
                return hitColliders[i];
            }
        }
        return null;
    }

    /// <summary>Returns the first collider hit</summary>
    Collider checkForObjectCollision(Vector3 checkOffset) {
        Collider[] hitColliders = collisionRaycast(checkOffset);
        for (int i = 0; i < hitColliders.Length; i++) {
            //Debug.Log("Collider: " + hitColliders[i].ToString());
            if (hitColliders[i].name.ToLowerInvariant().Contains("_object")) {
                return hitColliders[i];
            }
        }
        return null;
    }


    #endregion

    #region Animations & Visuals

    // This exposure was created for the Direction Surrogate
    public void SwitchAnimation(Vector3 direction) => animator.SwitchAnimation(direction);
    
    // Expose so outside sources can update this objects animation
    public void SwitchAnimation(string animationName) => animator.SwitchAnimation(animationName);

    public void updateCosmetics() {
        return; //FIXME
        hairSprite.color = SaveData.currentSave.playerHairColor.Color;
        eyeSprite.color = SaveData.currentSave.playerEyeColor.Color;
    }

    public void Reflect(bool setState) {
        return; //FIXME
        pawnReflectionSprite.enabled = setState;
    }

    private Vector2 GetUVSpriteMap(int index) {
        int row = index / 4;
        int column = index % 4;

        return new Vector2(0.25f * column, 0.75f - (0.25f * row));
    }

    public void setOverrideAnimPause(bool set) {
        overrideAnimPause = set;
    }

    #endregion

    #region Camera

    public Vector3 GetCamOrigin() => camOrigin;

    public IEnumerator moveCameraTo(Vector3 targetPosition, float duration) {
        targetPosition += MainCameraDefaultPosition;

        LeanTween.move(PlayerCamera.gameObject, targetPosition, duration);
        yield return new WaitForSeconds(duration);

        PlayerCamera.transform.localPosition = targetPosition;
    }

    public IEnumerator shakeCamera(int iteration, float duration, AudioClip clip = null) {
        Transform camera = transform.Find("Camera") != null ? transform.Find("Camera") : GameObject.Find("Camera").transform;

        for (int i = 0; i < iteration; ++i) {
            float increment = 0f;
            float distance = 0.2f;
            Vector3 startPosition = camera.position;

            playClip(clip);

            LeanTween.moveX(camera.gameObject, camera.position.x + distance, duration / 4);
            yield return new WaitForSeconds(duration / 4);
            LeanTween.moveX(camera.gameObject, camera.position.x - distance * 2, duration / 2);
            yield return new WaitForSeconds(duration / 2);
            LeanTween.moveX(camera.gameObject, camera.position.x + distance, duration / 4);
            yield return new WaitForSeconds(duration / 4);

            camera.position = startPosition;
        }

        yield return null;
    }


    #endregion

    #region Audio

    public AudioSource GetAudio() => playerAudio;

    private void playClip(AudioClip clip) {
        playerAudio.clip = clip;
        playerAudio.volume = PlayerPrefs.GetFloat("sfxVolume");
        playerAudio.Play();
    }

    private void playBump() {
        if (!playerAudio.isPlaying)
            if (!IsMoving && !overrideAnimPause)
                playClip(bumpClip);
    }

    #endregion

    #region Other

    public void UpdateRPC() {
        //Update Discord RPC
        float hour = System.DateTime.Now.Hour;

        string dayTime;

        if (hour < 6) {
            dayTime = "Night";
        } else if (hour < 12) {
            dayTime = "Morning";
        } else if (hour < 13) {
            dayTime = "Midday";
        } else if (hour < 18) {
            dayTime = "Afternoon";
        } else if (hour < 20) {
            dayTime = "Evening";
        } else {
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

    IEnumerator openPauseMenu() {
        if (IsMoving || UnityEngine.Input.GetButtonDown("Start")) //Start
        {
            if (setCheckBusyWith(Scene.main.Pause.gameObject)) {
                SwitchAnimation("Idle");
                Scene.main.Pause.gameObject.SetActive(true);
                StartCoroutine(Scene.main.Pause.control());
                while (Scene.main.Pause.gameObject.activeSelf) {
                    yield return null;
                }
                unsetCheckBusyWith(Scene.main.Pause.gameObject);
            }
        }
    }

    public void ActivateStrength() => strength = true;

    ///Attempts to set player to be busy with "caller" and pauses input, returning true if the request worked.
    public bool setCheckBusyWith(GameObject caller) {
        if (Singleton.busyWith == null)
            Singleton.busyWith = caller;

        //if the player is definitely busy with caller object
        if (Singleton.busyWith == caller) {
            PauseInput();
            Debug.Log("Busy with " + PlayerMovement.Singleton.busyWith);
            return true;
        }
        return false;
    }

    ///Attempts to unset player to be busy with "caller". Will unpause input only if 
    ///the player is still not busy 0.1 seconds after calling.
    public void unsetCheckBusyWith(GameObject caller) {
        if (Singleton.busyWith == caller)
            Singleton.busyWith = null;

        StartCoroutine(checkBusinessBeforeUnpause(0.1f));
    }

    public IEnumerator checkBusinessBeforeUnpause(float waitTime) {
        yield return StartCoroutine(checkBusinessBeforeUnpause(waitTime, true));
    }

    public IEnumerator checkBusinessBeforeUnpause(float waitTime, bool busyConstraint) {
        yield return new WaitForSeconds(waitTime);
        if (busyConstraint)
            if (PlayerMovement.Singleton.busyWith == null)
                UnpauseInput();
            else
                Debug.Log("Busy with " + PlayerMovement.Singleton.busyWith);
    }

    void interact() {
        Collider currentInteraction = GetInteractedObject();

        if (currentInteraction != null) {
            Debug.Log($"Interacting with {currentInteraction.name}");
            //sent interact message to the collider's object's parent object
            currentInteraction.transform.parent.gameObject.SendMessage("interact", SendMessageOptions.DontRequireReceiver);
            canInput = false;
        } else if (!IsSurfing) {
            if (currentMapCollider.GetTileTag(transform.position + possibleMovement) == (int)EMapTile.SurfableWater)
                StartCoroutine(surfCheck());
        }

        Collider GetInteractedObject() {
            Collider[] hitColliders = Physics.OverlapSphere(interactPosition, InteractRadius);
            if (hitColliders.Length > 0) {
                for (int i = 0; i < hitColliders.Length; i++) {
                    //Prioritise a transparent over a solid object.
                    if (hitColliders[i].name.Contains("_Transparent") && hitColliders[i].name != ("Player_Transparent"))
                        return hitColliders[i];
                    else if (hitColliders[i].name.Contains("_Object"))
                        return hitColliders[i];
                }
            }
            return null;
        }
    }

    public IEnumerator wildEncounter(WildPokemonInitialiser.Location encounterLocation) {
        if (newMap.getEncounterList(encounterLocation).Length > 0) {
            if (UnityEngine.Random.value <= newMap.getEncounterProbability()) {
                if (setCheckBusyWith(Scene.main.Battle.gameObject)) {
                    IPokemon wildpkm = newMap.getRandomEncounter(encounterLocation);

                    Scene.main.Battle.PlayWildBGM(wildpkm);

                    //SceneTransition sceneTransition = Dialog.transform.GetComponent<SceneTransition>();

                    yield return StartCoroutine(ScreenFade.Singleton.FadeCutout(false, ScreenFade.SlowedSpeed, null));
                    //yield return new WaitForSeconds(sceneTransition.FadeOut(1f));
                    Scene.main.Battle.gameObject.SetActive(true);
                    StartCoroutine(Scene.main.Battle.control(wildpkm));

                    while (Scene.main.Battle.gameObject.activeSelf)
                        yield return null;

                    //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                    yield return StartCoroutine(ScreenFade.Singleton.Fade(true, 0.4f));

                    unsetCheckBusyWith(Scene.main.Battle.gameObject);
                }
            }
        }
    }

    public IEnumerator startWildBattle() {
        if (setCheckBusyWith(Scene.main.Battle.gameObject)) {
            IPokemon wildpkm = newMap.getRandomEncounter(WildPokemonInitialiser.Location.Grass);

            Scene.main.Battle.PlayWildBGM(wildpkm);

            //SceneTransition sceneTransition = Dialog.transform.GetComponent<SceneTransition>();

            yield return StartCoroutine(ScreenFade.Singleton.FadeCutout(false, ScreenFade.SlowedSpeed, null));
            //yield return new WaitForSeconds(sceneTransition.FadeOut(1f));
            Scene.main.Battle.gameObject.SetActive(true);
            StartCoroutine(Scene.main.Battle.control(wildpkm));

            while (Scene.main.Battle.gameObject.activeSelf)
                yield return null;

            //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
            yield return StartCoroutine(ScreenFade.Singleton.Fade(true, 0.4f));

            unsetCheckBusyWith(Scene.main.Battle.gameObject);
        }
    }

    #endregion
}

public enum EMovementDirection {
    Up,
    Right,
    Down,
    Left,
    Nowhere
}

public static class EMovementDirectionExtensions {
    public static Vector3 ToVector(this EMovementDirection direction) {
        //set vector3 based off of direction
        Vector3 forwardVector = new Vector3(0, 0, 0);
        if (direction == EMovementDirection.Up) {
            forwardVector = new Vector3(0, 0, 1f);
        } else if (direction == EMovementDirection.Right) {
            forwardVector = new Vector3(1f, 0, 0);
        } else if (direction == EMovementDirection.Down) {
            forwardVector = new Vector3(0, 0, -1f);
        } else if (direction == EMovementDirection.Left) {
            forwardVector = new Vector3(-1f, 0, 0);
        }
        return forwardVector;
    }
}
