﻿//Original Scripts by IIColour (IIColour_Spectrum)

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
        return; // FIXME
        Player = PlayerMovement.Singleton.gameObject;
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
        return; // FIXME
        if (lockPlayerCamera)
        {
            PlayerMovement.Singleton.PlayerCamera.transform.position = lockedPosition;
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
                if (PlayerMovement.Singleton.setCheckBusyWith(this.gameObject))
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
                    PlayerMovement.Singleton.unsetCheckBusyWith(this.gameObject);
                }
            }
        }
        else
        {
            if (examineText.Length > 0)
            {
                if (PlayerMovement.Singleton.setCheckBusyWith(this.gameObject))
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
                    PlayerMovement.Singleton.unsetCheckBusyWith(this.gameObject);
                }
            }
        }
    }

    public IEnumerator bump()
    {
        if (!isLocked && !PlayerMovement.Singleton.IsInputPaused())
        {
            if (PlayerMovement.Singleton.setCheckBusyWith(gameObject))
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
                    PlayerMovement.Singleton.IsRunning = false;
                    PlayerMovement.Singleton.Speed = PlayerMovement.Singleton.WalkSpeed;
                    PlayerMovement.Singleton.SwitchAnimation("walk");

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
                    PlayerMovement.Singleton.IsRunning = false;
                    PlayerMovement.Singleton.Speed = PlayerMovement.Singleton.WalkSpeed;
                    PlayerMovement.Singleton.SwitchAnimation("walk");

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
                    PlayerMovement.Singleton.forceMoveForward();
                }
                
                //float fadeTime = sceneTransition.FadeOut() + 0.4f;
                WeatherHandler.fadeSound();
                float fadeTime = ScreenFade.SlowedSpeed + 0.4f;
                //fadeCutouts for doorways not yet implemented
                if (fadeSprite == null)
                {
                    StartCoroutine(ScreenFade.Singleton.Fade(false, ScreenFade.SlowedSpeed));
                }
                else
                {
                    StartCoroutine(ScreenFade.Singleton.FadeCutout(false, ScreenFade.DefaultSpeed, fadeSprite));
                }
                
                if (!dontFadeMusic)
                {
                    BackgroundMusicHandler.Singleton.PlayMain(null, 0);
                }
                yield return new WaitForSeconds(fadeTime);


                //reset camera and doorway transforms
                /*PlayerMovement.player.mainCamera.transform.localPosition =
                    PlayerMovement.player.mainCameraDefaultPosition;*/
                PlayerMovement.Singleton.PlayerCamera.fieldOfView = PlayerMovement.Singleton.MainCameraDefaultFOV;
                transform.localPosition = initPosition;
                transform.localRotation = initRotation;
                transform.localScale = initScale;

                if (transferSceneName.Length > 0)
                {
                    NonResettingHandler.saveDataToGlobal();

                    GlobalVariables.Singleton.playerPosition = transferPosition;
                    GlobalVariables.Singleton.playerDirection = transferDirection;
                    GlobalVariables.Singleton.playerForwardOnLoad = movesForward;
                    GlobalVariables.Singleton.playerExiting = true;
                    GlobalVariables.Singleton.fadeIn = true;
                    SceneManager.LoadScene(transferSceneName);
                }
                else
                {
                    //uncheck busy with to ensure events at destination can be run.
                    PlayerMovement.Singleton.unsetCheckBusyWith(gameObject);

                    //transfer to current scene, no saving/loading nessecary
                    PlayerMovement.Singleton.SwitchAnimation("walk");
                    PlayerMovement.Singleton.Speed = PlayerMovement.Singleton.WalkSpeed;

                    PlayerMovement.Singleton.transform.position = transferPosition;
                    PlayerMovement.Singleton.UpdateDirection(transferDirection);
                    
                    
                    PlayerMovement.Singleton.Follower.direction = GlobalVariables.Singleton.playerDirection;
                    PlayerMovement.Singleton.Follower.transform.localPosition = -Direction.Vectorize(transferDirection);
                    if (movesForward)
                    {
                        PlayerMovement.Singleton.forceMoveForward();
                    }

                    GlobalVariables.Singleton.fadeIn = true;
                    //SceneTransition.gameScene.FadeIn();
                    StartCoroutine(ScreenFade.Singleton.Fade(true, ScreenFade.SlowedSpeed));

                    yield return new WaitForSeconds(0.1f);
                    PlayerMovement.Singleton.PauseMovement();
                    yield return new WaitForSeconds(0.8f);
                    PlayerMovement.Singleton.UnpauseMovement();
                }
            }
        }
    }

    private IEnumerator lockCameraPosition()
    {
        lockPlayerCamera = true;
        lockedPosition = PlayerMovement.Singleton.PlayerCamera.transform.position;
        yield return new WaitForSeconds(1f);
        lockPlayerCamera = false;
    }
}