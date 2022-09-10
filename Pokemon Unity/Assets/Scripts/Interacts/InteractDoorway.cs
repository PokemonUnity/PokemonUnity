//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using Classes;
using UnityEditor;
using UnityEngine.SceneManagement;

public class InteractDoorway : MonoBehaviour
{
    private GameObject Player;
    private DialogBoxHandlerNew Dialog;

    private Animator myAnimator;
    private SpriteRenderer objectSprite;
    private Light objectLight;
    private Collider hitBox;

    private AudioSource enterSound;

    public bool isLocked = false;
    public bool hasLight = false;
    public bool dontFadeMusic = false;

    private Vector3 initPosition;
    private Quaternion initRotation;
    private Vector3 initScale;

    public enum EntranceStyle
    {
        STANDSTILL,
        OPEN,
        SWINGLEFT,
        SWINGRIGHT,
        SLIDE
    }

    public EntranceStyle entranceStyle;

    public bool movesForward = false;
    
    public string transferSceneName;
    public Vector3 transferPosition;
    public int transferDirection;
    public string examineText;
    private string lockedExamineText;
    
    public Sprite fadeSprite;

    private bool lockPlayerCamera = false;
    private Vector3 lockedPosition;

    // Use this for initialization
    void Start()
    {
        Player = PlayerMovement.player.gameObject;
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandlerNew>();

        objectLight = this.GetComponentInChildren<Light>();
        if (objectLight != null)
        {
            if (!hasLight)
            {
                objectLight.enabled = false;
            }
            else
            {
                objectLight.enabled = true;
            }
        }

        enterSound = this.gameObject.GetComponent<AudioSource>();

        initPosition = transform.localPosition;
        initRotation = transform.localRotation;
        initScale = transform.localScale;
    }

    void LateUpdate()
    {
        if (lockPlayerCamera)
        {
            PlayerMovement.player.mainCamera.transform.position = lockedPosition;
        }
    }

    public IEnumerator interact()
    {
        if (isLocked)
        {
            switch (Language.getLang())
            {
                default:
                    lockedExamineText = "The door is locked.";
                    break;
                case Language.Country.FRANCAIS:
                    lockedExamineText = "La porte est verrouillée.";
                    break;
            }
            if (lockedExamineText.Length > 0)
            {
                if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
                {
                    Dialog.DrawBlackFrame();
                        //yield return StartCoroutine blocks the next code from running until coroutine is done.
                    yield return Dialog.StartCoroutine(Dialog.DrawText( lockedExamineText));
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        //these 3 lines stop the next bit from running until space is pressed.
                        yield return null;
                    }
                    Dialog.UndrawDialogBox();
                    yield return new WaitForSeconds(0.2f);
                    PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
                }
            }
        }
        else
        {
            if (examineText.Length > 0)
            {
                if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
                {
                    Dialog.DrawDialogBox();
                        //yield return StartCoroutine blocks the next code from running until coroutine is done.
                    yield return Dialog.StartCoroutine(Dialog.DrawText( examineText));
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        //these 3 lines stop the next bit from running until space is pressed.
                        yield return null;
                    }
                    Dialog.UndrawDialogBox();
                    yield return new WaitForSeconds(0.2f);
                    PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
                }
            }
        }
    }

    public IEnumerator bump()
    {
        if (!isLocked && !PlayerMovement.player.isInputPaused())
        {
            if (PlayerMovement.player.setCheckBusyWith(gameObject))
            {
                if (enterSound != null)
                {
                    if (!enterSound.isPlaying)
                    {
                        enterSound.volume = PlayerPrefs.GetFloat("sfxVolume");
                        enterSound.Play();
                    }
                }

                if (entranceStyle == EntranceStyle.SWINGRIGHT)
                {
                    PlayerMovement.player.running = false;
                    PlayerMovement.player.speed = PlayerMovement.player.walkSpeed;
                    PlayerMovement.player.updateAnimation("walk", PlayerMovement.player.walkFPS);

                    float increment = 0f;
                    float speed = 0.25f;
                    float yRotation = transform.localEulerAngles.y;
                    while (increment < 1)
                    {
                        increment += (1f / speed) * Time.deltaTime;
                        if (increment > 1)
                        {
                            increment = 1;
                        }
                        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                            yRotation + (90f * increment), transform.localEulerAngles.z);
                        /*PlayerMovement.player.mainCamera.fieldOfView = PlayerMovement.player.mainCameraDefaultFOV -
                                                                       ((PlayerMovement.player.mainCameraDefaultFOV /
                                                                         10f) * increment);*/
                        yield return null;
                    }

                    yield return new WaitForSeconds(0.2f);
                }
                else if (entranceStyle == EntranceStyle.SLIDE)
                {
                    PlayerMovement.player.running = false;
                    PlayerMovement.player.speed = PlayerMovement.player.walkSpeed;
                    PlayerMovement.player.updateAnimation("walk", PlayerMovement.player.walkFPS);

                    float increment = 0f;
                    float speed = 0.25f;
                    while (increment < 1)
                    {
                        increment += (1 / speed) * Time.deltaTime;
                        if (increment > 1)
                        {
                            increment = 1;
                        }
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y,
                            1f - (0.92f * increment));
                        /*PlayerMovement.player.mainCamera.fieldOfView = 20f - (2f * increment);*/
                        yield return null;
                    }
                    yield return new WaitForSeconds(0.2f);
                }


                if (entranceStyle != EntranceStyle.STANDSTILL)
                {
                    if (entranceStyle != EntranceStyle.OPEN)
                    {
                        StartCoroutine(lockCameraPosition());
                        yield return new WaitForSeconds(0.1f);
                    }
                    PlayerMovement.player.forceMoveForward();
                }
                
                //float fadeTime = sceneTransition.FadeOut() + 0.4f;
                WeatherHandler.fadeSound();
                float fadeTime = ScreenFade.slowedSpeed + 0.4f;
                //fadeCutouts for doorways not yet implemented
                if (fadeSprite == null)
                {
                    StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.slowedSpeed));
                }
                else
                {
                    StartCoroutine(ScreenFade.main.FadeCutout(false, ScreenFade.defaultSpeed, fadeSprite));
                }
                
                if (!dontFadeMusic)
                {
                    BgmHandler.main.PlayMain(null, 0);
                }
                yield return new WaitForSeconds(fadeTime);


                //reset camera and doorway transforms
                /*PlayerMovement.player.mainCamera.transform.localPosition =
                    PlayerMovement.player.mainCameraDefaultPosition;*/
                PlayerMovement.player.mainCamera.fieldOfView = PlayerMovement.player.mainCameraDefaultFOV;
                transform.localPosition = initPosition;
                transform.localRotation = initRotation;
                transform.localScale = initScale;

                if (transferSceneName.Length > 0)
                {
                    NonResettingHandler.saveDataToGlobal();

                    GlobalVariables.global.playerPosition = transferPosition;
                    GlobalVariables.global.playerDirection = transferDirection;
                    GlobalVariables.global.playerForwardOnLoad = movesForward;
                    GlobalVariables.global.playerExiting = true;
                    GlobalVariables.global.fadeIn = true;
                    SceneManager.LoadScene(transferSceneName);
                }
                else
                {
                    //uncheck busy with to ensure events at destination can be run.
                    PlayerMovement.player.unsetCheckBusyWith(gameObject);

                    //transfer to current scene, no saving/loading nessecary
                    PlayerMovement.player.updateAnimation("walk", PlayerMovement.player.walkFPS);
                    PlayerMovement.player.speed = PlayerMovement.player.walkSpeed;

                    PlayerMovement.player.transform.position = transferPosition;
                    PlayerMovement.player.updateDirection(transferDirection);
                    
                    
                    PlayerMovement.player.followerScript.direction = GlobalVariables.global.playerDirection;
                    PlayerMovement.player.followerScript.transform.localPosition = -Direction.Vectorize(transferDirection);
                    if (movesForward)
                    {
                        PlayerMovement.player.forceMoveForward();
                    }

                    GlobalVariables.global.fadeIn = true;
                    //SceneTransition.gameScene.FadeIn();
                    StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.slowedSpeed));

                    yield return new WaitForSeconds(0.1f);
                    PlayerMovement.player.pauseInput();
                    yield return new WaitForSeconds(0.8f);
                    PlayerMovement.player.unpauseInput();
                }
            }
        }
    }

    private IEnumerator lockCameraPosition()
    {
        lockPlayerCamera = true;
        lockedPosition = PlayerMovement.player.mainCamera.transform.position;
        yield return new WaitForSeconds(1f);
        lockPlayerCamera = false;
    }
}