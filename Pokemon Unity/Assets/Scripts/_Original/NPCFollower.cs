//Original Scripts by IIColour (IIColour_Spectrum)

using System;
using UnityEngine;
using System.Collections;
using Random = System.Random;

[System.Serializable]
public class NPCFollower : MonoBehaviour
{
    private NPCHandler npcHandler;
    
    private DialogBoxHandlerNew Dialog;
    private PlayerMovement Player;

    private Vector3 startPosition;
    private Vector3 destinationPosition;

    private Vector3 position;

    public bool moving = false;
    public float speed;

    public Transform pawn;
    public Transform pawnReflection;
    public Transform hitBox;
    
    private SpriteRenderer sRenderer;
    private SpriteRenderer sReflectionRenderer;
    private Sprite[] spriteSheet;

    private SpriteRenderer pawnShadow;

    public bool hide;
    public bool canMove = true;

    public int walkFPS = 7;
    public int runFPS = 12;

    // Use this for initialization
    void Awake()
    {
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandlerNew>();
        Player = PlayerMovement.player;

        pawn = transform.Find("Pawn");
        pawnReflection = transform.Find("PawnReflection");

        hitBox = transform.Find("NPC_Transparent");
        
        if (hitBox == null) hitBox = transform.Find("NPC_Object");

        sRenderer = pawn.GetComponent<SpriteRenderer>();
        sReflectionRenderer = pawnReflection.GetComponent<SpriteRenderer>();

        pawnShadow = transform.Find("PawnShadow").GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Player = PlayerMovement.player;
        startPosition = transform.position;
        
        spriteSheet = Resources.LoadAll<Sprite>("OverworldNPCSprites/" + GetComponent<NPCHandler>().npcSpriteName);
        npcHandler = GetComponent<NPCHandler>();
    }

    private void LateUpdate()
    {
        if (!hide)
        {
            float scale;

            Transform cam = PlayerMovement.player.transform.Find("Camera") != null
                ? PlayerMovement.player.transform.Find("Camera")
                : GameObject.Find("Camera").transform;

            Vector3 position = cam.position - PlayerMovement.player.getCamOrigin();

            if (transform.position.z > position.z)
            {
                scale = 0.0334f * (Math.Abs(transform.position.z - position.z))+0.9f;
                if (transform.position.z > position.z + 3)
                {
                    scale = 1;
                }
            }
            else
            {
                scale = 0.9f;
            }
        
            //scale = 0.0334f * (transform.position.z - PlayerMovement.player.transform.position.z)+0.9f;
        
            pawn.transform.localScale = new Vector3(scale,scale,scale);
        
            Camera camera = PlayerMovement.player.transform.Find("Camera") != null
                ? PlayerMovement.player.transform.Find("Camera").GetComponent<Camera>()
                : GameObject.Find("Camera").GetComponent<Camera>();
        
            pawn.transform.LookAt(camera.transform);

            //if (PlayerMovement.player.transform.Find("Camera") == null)
            pawn.transform.localRotation = Quaternion.Euler(camera.transform.rotation.x-50, 180, 0);
		
            pawn.transform.Rotate( new Vector3(0, 180, 0), Space.Self );
        
            //pawnSprite.transform.rotation = PlayerMovement.player.transform.Find("Pawn").transform.rotation; 
        }
    }

    public IEnumerator move(Vector3 destination, float sentSpeed)
    {
        if (canMove)
        {
            hide = false;
            pawnShadow.enabled = true;
            speed = sentSpeed;
            npcHandler.framesPerSec = Player.framesPerSec;
            startPosition = transform.position; //add follower's position offset
            destinationPosition = destination;
            Vector3 movement = destinationPosition - startPosition;
            if (Mathf.Round(movement.x) > 0)
            {
                npcHandler.direction = 1;
            }
            else if (Mathf.Round(movement.x) < 0)
            {
                npcHandler.direction = 3;
            }
            else if (Mathf.Round(movement.z) > 0)
            {
                npcHandler.direction = 0;
            }
            else if (Mathf.Round(movement.z) < 0)
            {
                npcHandler.direction = 2;
            }
            npcHandler.animPause = false;
            while (Player.increment < 1)
            {
                //because fak trying to use this thing's own increment. shit doesn't work for some reason.
                transform.position = startPosition + (destinationPosition - startPosition) * Player.increment;
                hitBox.position = destinationPosition;
                yield return null;
            } 
            npcHandler.animPause = true;
            transform.position = destinationPosition;
            hitBox.position = destinationPosition;
        }
        else if (hide)
        {
            while (Player.increment < 1)
            {
                transform.position = Player.transform.position;
                hitBox.position = Player.transform.position;
                yield return null;
            }
        }
        else
        {
            startPosition = transform.position;
            while (Player.increment < 1)
            {
                transform.position = startPosition;
                hitBox.position = startPosition;
                yield return null;
            }
        }
    }

    public void hideFollower()
    {
        hide = true;
        transform.position = Player.transform.position;
    }

    public void Hide()
    {
        canMove = false;
        sRenderer.sprite = null;
        sReflectionRenderer.sprite = null;
        pawnShadow.enabled = false;
        sRenderer.sprite = null;
        sReflectionRenderer.sprite = null;
        hide = true;
        transform.position = PlayerMovement.player.transform.position;
    }

    public void ActivateMove()
    {
        canMove = true;
    }

    public void reflect(bool setState)
    {
        //Debug.Log ("F Reflect");
        sReflectionRenderer.enabled = setState;
    }

    private void playClip(AudioClip clip)
    {
        AudioSource PlayerAudio = PlayerMovement.player.getAudio();
        
        PlayerAudio.clip = clip;
        PlayerAudio.volume = PlayerPrefs.GetFloat("sfxVolume");
        PlayerAudio.Play();
    }

    public IEnumerator jump()
    {
        float increment = 0f;
        float parabola = 0;
        float height = 2.1f;
        Vector3 startPosition = pawn.position;

        playClip(PlayerMovement.player.jumpClip);

        while (increment < 1)
        {
            increment += (1 / PlayerMovement.player.walkSpeed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            parabola = -height * (increment * increment) + (height * increment);
            pawn.position = new Vector3(pawn.position.x, startPosition.y + parabola, pawn.position.z);
            yield return null;
        }
        pawn.position = new Vector3(pawn.position.x, startPosition.y, pawn.position.z);

        playClip(PlayerMovement.player.landClip);
    }
}
