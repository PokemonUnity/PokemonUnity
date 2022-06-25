//Original Scripts by IIColour (IIColour_Spectrum)

using System;
using UnityEngine;
using System.Collections;
using Random = System.Random;

public class FollowerMovement : MonoBehaviour
{
    private DialogBoxHandlerNew Dialog;
    private PlayerMovement Player;

    private Vector3 startPosition;
    private Vector3 destinationPosition;

    private Vector3 position;

    public AudioClip withdrawClip;

    public bool hasLight;
    public Color lightColor;
    public float lightIntensity;

    public bool moving = false;
    public float speed;
    public int direction = 2;

    public int pokemonID = 6;
    private int followerIndex = 0;

    public Transform pawn;
    private Transform pawnLight;
    public Transform pawnReflection;
    public Transform pawnLightReflection;
    public Transform hitBox;

    private Light followerLight;
    private SpriteRenderer sRenderer;
    private SpriteRenderer sLRenderer;
    private SpriteRenderer sReflectionRenderer;
    private SpriteRenderer sLReflectionRenderer;
    private Sprite[] spriteSheet;
    private Sprite[] lightSheet;

    private SpriteRenderer pawnShadow;

    public bool hide;
    public bool canMove = true;
    public Sprite pokeBall;

    private enum Hapiness
    {
        SAD,
        NORMAL,
        HAPPY
    }

    // Use this for initialization
    void Awake()
    {
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandlerNew>();
        Player = PlayerMovement.player;

        pawn = transform.Find("Pawn");
        pawnLight = pawn.Find("PawnLight");
        pawnReflection = transform.Find("PawnReflection");
        pawnLightReflection = pawnReflection.Find("PawnLightReflection");

        hitBox = transform.Find("Follower_Transparent");

        sRenderer = pawn.GetComponent<SpriteRenderer>();
        sLRenderer = pawnLight.GetComponent<SpriteRenderer>();
        sReflectionRenderer = pawnReflection.GetComponent<SpriteRenderer>();
        sLReflectionRenderer = pawnLightReflection.GetComponent<SpriteRenderer>();

        pawnShadow = transform.Find("PawnShadow").GetComponent<SpriteRenderer>();

        followerLight = GetComponentInChildren<Light>();
    }

    void Start()
    {
        Player = PlayerMovement.player;
        startPosition = transform.position;

        if (SaveData.currentSave.PC.boxes[0][0] == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (GlobalVariables.global.followerOut)
            {
                followerLight.color = lightColor;
                if (hasLight)
                {
                    followerLight.intensity = lightIntensity;
                }
                else
                {
                    followerLight.intensity = 0;
                }
            
                /*
                if (direction == 0)
                {
                    transform.Translate(Vector3.back);
                }
                else if (direction == 1)
                {
                    transform.Translate(Vector3.left);
                }
                else if (direction == 2)
                {
                    transform.Translate(Vector3.forward);
                }
                else if (direction == 3)
                {
                    transform.Translate(Vector3.right);
                }
                */
                transform.position = startPosition;
                changeFollower(followerIndex);
                StartCoroutine("animateSprite");
            }
            else
            {
                Hide();
            }
        }
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
            followerLight.enabled = true;
            pawnShadow.enabled = true;
            speed = sentSpeed;
            startPosition = transform.position; //add follower's position offset
            destinationPosition = destination;
            Vector3 movement = destinationPosition - startPosition;
            if (Mathf.Round(movement.x) > 0)
            {
                direction = 1;
            }
            else if (Mathf.Round(movement.x) < 0)
            {
                direction = 3;
            }
            else if (Mathf.Round(movement.z) > 0)
            {
                direction = 0;
            }
            else if (Mathf.Round(movement.z) < 0)
            {
                direction = 2;
            }
            while (Player.increment < 1)
            {
                //because fak trying to use this thing's own increment. shit doesn't work for some reason.
                transform.position = startPosition + (destinationPosition - startPosition) * Player.increment;
                hitBox.position = destinationPosition;
                yield return null;
            }
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

    public IEnumerator releaseFromBall()
    {
        if (SaveData.currentSave.PC.boxes[0][0] != null)
        {
            if (PlayerMovement.player.npcFollower == null)
            {
                GameObject ball = sRenderer.transform.parent.Find("pokeball").gameObject;
                Player = PlayerMovement.player;
                
                canMove = false;
                followerLight.enabled = false;
                sRenderer.sprite = null;
                sLRenderer.sprite = null;
                sReflectionRenderer.sprite = null;
                sLReflectionRenderer.sprite = null;
                pawnShadow.enabled = false;
                sRenderer.sprite = null;
                sLRenderer.sprite = null;
                sReflectionRenderer.sprite = null;
                sLReflectionRenderer.sprite = null;
                hide = true;
                ball.SetActive(false);
                
                followerLight.color = lightColor;
                if (hasLight)
                {
                    followerLight.intensity = lightIntensity;
                }
                else
                {
                    followerLight.intensity = 0;
                }

                switch (Player.direction)
                {
                    case 0:
                        transform.Translate(Vector3.back);
                        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z-1);
                        break;
                    case 1:
                        transform.Translate(Vector3.left);
                        transform.position = new Vector3(Player.transform.position.x-1, Player.transform.position.y, Player.transform.position.z);
                        break;
                    case 2:
                        transform.Translate(Vector3.forward);
                        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z+1);
                        break;
                    case 3:
                        transform.Translate(Vector3.right);
                        transform.position = new Vector3(Player.transform.position.x+1, Player.transform.position.y, Player.transform.position.z);
                        break;
                }

                direction = Player.direction;
                
                pawnShadow.enabled = true;
                ball.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                SfxHandler.Play(withdrawClip);
                
                hide = false;
                ball.SetActive(false);
                followerLight.enabled = true;
                changeFollower(followerIndex);
                StartCoroutine("animateSprite");
                canMove = true;
                
                GlobalVariables.global.followerOut = true;
            }
            else
            {
                if (Player.setCheckBusyWith(this.gameObject))
                {
                    Dialog.DrawBlackFrame();

                    if (Language.getLang() == Language.Country.FRANCAIS)
                    {
                        Dialog.StartCoroutine(Dialog.DrawText("Vous ne pouvez pas appeler " +
                                                              SaveData.currentSave.PC.boxes[0][followerIndex].getName() +
                                                              " pour le moment."));
                    }
                    else
                    {
                        Dialog.StartCoroutine(Dialog.DrawText("You can't release " +
                                                              SaveData.currentSave.PC.boxes[0][followerIndex].getName() +
                                                              " for now."));
                    }
                
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.UndrawDialogBox();

                    Player.unsetCheckBusyWith(this.gameObject);
                }
            }
        }

        yield return null;
    }

    public IEnumerator withdrawToBall()
    {
        GameObject ball = sRenderer.transform.parent.Find("pokeball").gameObject;
        StopCoroutine("animateSprite");
        canMove = false;
        followerLight.enabled = false;
        sRenderer.sprite = null;
        sLRenderer.sprite = null;
        sReflectionRenderer.sprite = null;
        sLReflectionRenderer.sprite = null;
        ball.SetActive(true);
        SfxHandler.Play(withdrawClip);
        float increment = 0f;
        float time = 0.4f;
        Vector3 lockedPosition = transform.position;
        while (increment < 1)
        {
            increment += (1 / time) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            transform.position = lockedPosition;
            yield return null;
        }
        pawnShadow.enabled = false;
        sRenderer.sprite = null;
        sLRenderer.sprite = null;
        sReflectionRenderer.sprite = null;
        sLReflectionRenderer.sprite = null;
        hide = true;
        ball.SetActive(false);
        transform.position = Player.transform.position;

        GlobalVariables.global.followerOut = false;
        //StartCoroutine("animateSprite");
    }

    public void Hide()
    {
        StopCoroutine("animateSprite");
        canMove = false;
        followerLight.enabled = false;
        sRenderer.sprite = null;
        sLRenderer.sprite = null;
        sReflectionRenderer.sprite = null;
        sLReflectionRenderer.sprite = null;
        pawnShadow.enabled = false;
        sRenderer.sprite = null;
        sLRenderer.sprite = null;
        sReflectionRenderer.sprite = null;
        sLReflectionRenderer.sprite = null;
        hide = true;
        transform.position = PlayerMovement.player.transform.position;
    }

    public void ActivateMove()
    {
        canMove = true;
    }

    public void updateFollower()
    {
        changeFollower(0);
    }

    public void changeFollower(int index)
    {
        if (followerLight == null)
        {
            followerLight = GetComponentInChildren<Light>();
        }
        followerIndex = index;
        pokemonID = SaveData.currentSave.PC.boxes[0][followerIndex].getID();
        spriteSheet = SaveData.currentSave.PC.boxes[0][followerIndex].GetNewSprite(false);

        hasLight = PokemonDatabase.getPokemon(pokemonID).hasLight();
        lightIntensity = PokemonDatabase.getPokemon(pokemonID).getLuminance();
        lightColor = PokemonDatabase.getPokemon(pokemonID).getLightColor();
        lightSheet = SaveData.currentSave.PC.boxes[0][followerIndex].GetNewSprite(true);

        if (lightSheet[0] == null)
        {
            sLRenderer.sprite = null;
            sLReflectionRenderer.sprite = null;
        }

        followerLight.color = lightColor;
        followerLight.intensity = lightIntensity;
    }

    public void reflect(bool setState)
    {
        //Debug.Log ("F Reflect");
        sReflectionRenderer.enabled = setState;
        sLReflectionRenderer.enabled = setState;
    }

    private IEnumerator animateSprite()
    {
        int frame = 0;
        int light_frame = 0;
        while (true)
        {
            for (int i = 0; i < 6; i++)
            {
                if (!hide)
                {
                    int newDirection;
                    switch (direction)
                    {
                        case 0:
                            newDirection = 3;
                            break;
                        case 1:
                            newDirection = 2;
                            break;
                        case 2:
                            newDirection = 0;
                            break;
                        case 3:
                            newDirection = 1;
                            break;
                        default:
                            newDirection = 0;
                            break;
                    }
                    sRenderer.sprite = spriteSheet[newDirection * 4 + frame];
                    if (lightSheet.Length >= 16)
                        sLRenderer.sprite = lightSheet[newDirection * 4 + frame];
                    pawnShadow.enabled = true;
                }
                else
                {
                    sRenderer.sprite = null;
                    sLRenderer.sprite = null;
                    pawnShadow.enabled = false;
                }
                sReflectionRenderer.sprite = sRenderer.sprite;
                sLReflectionRenderer.sprite = sLRenderer.sprite;
                if (i > 2)
                {
                    //pawn.localPosition = new Vector3(-0.016f, 0.808f, -0.4f);
                    //pawnLight.localPosition = new Vector3(0, 0.171f, -0.36f);
                }
                else
                {
                    //pawn.localPosition = new Vector3(-0.016f, 0.808f, -0.4f);
                    //pawnLight.localPosition = new Vector3(0, 0.201f, -0.305f);
                }

                float time = 0.055f;
                
                yield return new WaitForSeconds((PlayerMovement.player.moving && PlayerMovement.player.running) ? time / 2 : time);
            }

            frame++;
            if (frame > 3) frame = 0;
            light_frame = (light_frame == 0) ? 1 : 0;
        }
    }
    
    private float PlayCry(Pokemon pokemon)
    {
        SfxHandler.Play(pokemon.GetCry(), pokemon.GetCryPitch());
        return pokemon.GetCry().length / pokemon.GetCryPitch();
    }

    private void playClip(AudioClip clip)
    {
        AudioSource PlayerAudio = PlayerMovement.player.getAudio();
        
        PlayerAudio.clip = clip;
        PlayerAudio.volume = PlayerPrefs.GetFloat("sfxVolume");
        PlayerAudio.Play();
    }

    private IEnumerator PlayCryAndWait(Pokemon pokemon)
    {
        yield return new WaitForSeconds(PlayCry(pokemon));
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

    public IEnumerator interact()
    {
        if (!hide)
        {
            if (Player.setCheckBusyWith(this.gameObject))
            {
                //calculate Player's position relative to target object's and set direction accordingly. (Face the player)
                float xDistance = this.transform.position.x - Player.gameObject.transform.position.x;
                float zDistance = this.transform.position.z - Player.gameObject.transform.position.z;
                if (xDistance >= Mathf.Abs(zDistance))
                {
                    //Mathf.Abs() converts zDistance to a positive always.
                    direction = 3; //this allows for better accuracy when checking orientation.
                }
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

                // Interaction
                if (SaveData.currentSave.PC.boxes[0][followerIndex].getPercentHP() < 0.25f) // Low HP
                {
                    yield return StartCoroutine(interaction_tired());
                }
                else  // Casual Interaction
                {
                    Hapiness h = getFollowerWeatherHappiness();
                    float val = UnityEngine.Random.value;


                    if (h == Hapiness.SAD)
                    {
                        // Weather Interaction
                        yield return StartCoroutine(interaction_weather_sad());
                    }
                    else if (h == Hapiness.HAPPY)
                    {
                        if (val < 0.25f)
                        {
                            yield return StartCoroutine(interaction_1());
                        }
                        else if (val < 0.5f)
                        {
                            yield return StartCoroutine(interaction_2());
                        }
                        else // Weather Interaction
                        {
                            yield return StartCoroutine(interaction_weather_happy());
                        }
                    }
                    else
                    {
                        if (val < 0.50f)
                        {
                            yield return StartCoroutine(interaction_1());
                        }
                        else
                        {
                            yield return StartCoroutine(interaction_2());
                        }
                    }
                }

                yield return new WaitForSeconds(0.2f);
                
                // End
                
                Player.unsetCheckBusyWith(this.gameObject);
            }
        }
    }

    private Hapiness getFollowerWeatherHappiness()
    {
        if (WeatherHandler.currentWeather == null) return Hapiness.NORMAL;
        if (WeatherHandler.currentWeather.type == Weather.WeatherType.Rain)
        {
            if (PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType1() ==
                PokemonData.Type.WATER ||
                PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType2() ==
                PokemonData.Type.WATER)
            {
                return Hapiness.HAPPY;
            }
            
            if (PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType1() ==
                PokemonData.Type.FIRE ||
                PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType2() ==
                PokemonData.Type.FIRE)
            {
                return Hapiness.SAD;
            }
            
            if (PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType1() ==
                PokemonData.Type.ROCK ||
                PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType2() ==
                PokemonData.Type.ROCK)
            {
                return Hapiness.SAD;
            }
            
            if (PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType1() ==
                PokemonData.Type.GROUND ||
                PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType2() ==
                PokemonData.Type.GROUND)
            {
                return Hapiness.SAD;
            }
        }
        else if (WeatherHandler.currentWeather.type == Weather.WeatherType.Sand)
        {
            if (PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType1() ==
                PokemonData.Type.GROUND ||
                PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType2() ==
                PokemonData.Type.GROUND)
            {
                return Hapiness.HAPPY;
            }
            
            if (PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType1() ==
                PokemonData.Type.FIRE ||
                PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType2() ==
                PokemonData.Type.FIRE)
            {
                return Hapiness.SAD;
            }
            
            if (PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType1() ==
                PokemonData.Type.ROCK ||
                PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType2() ==
                PokemonData.Type.ROCK)
            {
                return Hapiness.SAD;
            }
            
            if (PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType1() ==
                PokemonData.Type.STEEL ||
                PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType2() ==
                PokemonData.Type.STEEL)
            {
                return Hapiness.SAD;
            }
            
            if (PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType1() ==
                PokemonData.Type.POISON ||
                PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType2() ==
                PokemonData.Type.POISON)
            {
                return Hapiness.SAD;
            }
            
            if (PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType1() ==
                PokemonData.Type.WATER ||
                PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][followerIndex].getID()).getType2() ==
                PokemonData.Type.WATER)
            {
                return Hapiness.SAD;
            }
        }
        
        return Hapiness.NORMAL;
    }
    
    public IEnumerator interaction_weather_happy()
    {
        yield return StartCoroutine(jump());
        yield return StartCoroutine(jump());
        yield return StartCoroutine(PlayCryAndWait(SaveData.currentSave.PC.boxes[0][followerIndex]));
        yield return new WaitForSeconds(0.2f);
        Dialog.DrawBlackFrame();
        switch (Language.getLang())
        {
            default:
                if (WeatherHandler.currentWeather.type == Weather.WeatherType.Rain)
                {
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText(
                            SaveData.currentSave.PC.boxes[0][followerIndex].getName() +
                            " is enjoying the rain."));
                }
                else
                {
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText(
                            SaveData.currentSave.PC.boxes[0][followerIndex].getName() +
                            " is enjoying the weather."));
                }
                
                break;
            case (Language.Country.FRANCAIS):
                if (WeatherHandler.currentWeather.type == Weather.WeatherType.Rain)
                {
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText(
                            SaveData.currentSave.PC.boxes[0][followerIndex].getName() +
                            " apprécie être sous la pluie."));
                }
                else
                {
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText(
                            SaveData.currentSave.PC.boxes[0][followerIndex].getName() +
                            " apprécie la météo."));
                }
                
                break;
        }
        //is enjoying walking with you
        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
        {
            yield return null;
        }
        Dialog.UndrawDialogBox();
    }
    
    public IEnumerator interaction_weather_sad()
    {
        yield return new WaitForSeconds(0.2f);
        Dialog.DrawBlackFrame();
        switch (Language.getLang())
        {
            default:
                if (WeatherHandler.currentWeather.type == Weather.WeatherType.Rain)
                {
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText(
                            SaveData.currentSave.PC.boxes[0][followerIndex].getName() +
                            " seems to hate being soaked."));
                }
                else if (WeatherHandler.currentWeather.name == "Sandstorm")
                {
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText(
                            SaveData.currentSave.PC.boxes[0][followerIndex].getName() +
                            " gets sand in the eyes."));
                }
                else
                {
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText(
                            SaveData.currentSave.PC.boxes[0][followerIndex].getName() +
                            " seems to complain about the weather."));
                }
                
                break;
            case (Language.Country.FRANCAIS):
                if (WeatherHandler.currentWeather.name == "Rain")
                {
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText(
                            SaveData.currentSave.PC.boxes[0][followerIndex].getName() +
                            " semble détester être trempé."));
                }
                else if (WeatherHandler.currentWeather.type == Weather.WeatherType.Sand)
                {
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText(
                            SaveData.currentSave.PC.boxes[0][followerIndex].getName() +
                            " a du sable dans les yeux."));
                }
                else
                {
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText(
                            SaveData.currentSave.PC.boxes[0][followerIndex].getName() +
                            " semble se plaindre de la météo."));
                }
                
                break;
        }
        //is enjoying walking with you
        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
        {
            yield return null;
        }
        Dialog.UndrawDialogBox();
    }

    public IEnumerator interaction_1()
    {
        yield return StartCoroutine(jump());
        yield return StartCoroutine(jump());
        yield return StartCoroutine(PlayCryAndWait(SaveData.currentSave.PC.boxes[0][followerIndex]));
        yield return new WaitForSeconds(0.2f);
        Dialog.DrawBlackFrame();
        switch (Language.getLang())
        {
            default:
                yield return
                    Dialog.StartCoroutine(Dialog.DrawText(
                        SaveData.currentSave.PC.boxes[0][followerIndex].getName() +
                        " seems very happy."));
                break;
            case (Language.Country.FRANCAIS):
                yield return
                    Dialog.StartCoroutine(Dialog.DrawText(
                        SaveData.currentSave.PC.boxes[0][followerIndex].getName() +
                        " semble très heureux."));
                break;
        }
         //is enjoying walking with you
        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
        {
            yield return null;
        }
        Dialog.UndrawDialogBox();
    }
    
    public IEnumerator interaction_2()
    {
        direction = (direction + 1) % 4;
        yield return new WaitForSeconds(0.5f);
        direction = (direction + 1) % 4;
        yield return new WaitForSeconds(0.5f);
        direction = (direction + 1) % 4;
        yield return new WaitForSeconds(0.5f);
        direction = (direction + 1) % 4;
        yield return new WaitForSeconds(0.5f);
        direction = (direction + 1) % 4;
        yield return new WaitForSeconds(0.25f);
        direction = (direction + 1) % 4;
        yield return new WaitForSeconds(0.25f);
        direction = (direction + 1) % 4;
        yield return new WaitForSeconds(0.25f);
        direction = (direction + 1) % 4;
        yield return new WaitForSeconds(0.1f);
        direction = (direction + 1) % 4;
        yield return new WaitForSeconds(0.1f);
        direction = (direction + 1) % 4;
        yield return new WaitForSeconds(0.1f);
        direction = (direction + 1) % 4;
        yield return new WaitForSeconds(0.1f);
        direction = (direction + 1) % 4;
        yield return StartCoroutine(jump());
        yield return StartCoroutine(PlayCryAndWait(SaveData.currentSave.PC.boxes[0][followerIndex]));
        yield return new WaitForSeconds(0.2f);
    }
    
    public IEnumerator interaction_tired()
    {
        yield return StartCoroutine(PlayCryAndWait(SaveData.currentSave.PC.boxes[0][followerIndex]));
        yield return new WaitForSeconds(0.2f);
        Dialog.DrawBlackFrame();
        switch (Language.getLang())
        {
            default:
                yield return
                    Dialog.StartCoroutine(Dialog.DrawText(
                        SaveData.currentSave.PC.boxes[0][followerIndex].getName() +
                        " is tired."));
                break;
            case (Language.Country.FRANCAIS):
                yield return
                    Dialog.StartCoroutine(Dialog.DrawText(
                        SaveData.currentSave.PC.boxes[0][followerIndex].getName() +
                        " est fatigué."));
                break;
        }
        //is enjoying walking with you
        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
        {
            yield return null;
        }
        Dialog.UndrawDialogBox();
    }
}