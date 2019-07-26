using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;

public class CanvasUIHandler : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject CurrentCanvas;

    public UnityEngine.GameObject[] scenes;

    public UnityEngine.UI.Image fadeScreen;

    public bool fading = false;
    public UICanvas ActiveCanvasUI { get; private set; }
	/// <summary>
	/// This is text-script meant to be rendered by front-end UI
	/// </summary>
    #region Dialog values
    private string DialogText { get; set; }
	/// <summary>
	/// This is the text that's displayed to user on front-end.
	/// The text is properly formatted for real-time rendering display in Unity.
	/// </summary>
    private static UnityEngine.UI.Text DialogUIText { get; set; }// = UnityEngine.GameObject.Find("DialogText").GetComponent<UnityEngine.UI.Text>();
	/// <summary>
	/// This is the text that's hidden from user, and is used in background processing.
	/// The text is the final rendered display of the text properly formatted for UI in Unity
	/// </summary>
	private static UnityEngine.UI.Text DialogUITextDump { get; set; }// = UnityEngine.GameObject.Find("DialogTextDump").GetComponent<UnityEngine.UI.Text>();
    private static UnityEngine.UI.Text DialogUIScrollText;// = UnityEngine.GameObject.Find("DialogScrollText").GetComponent<UnityEngine.UI.Text>();
    private static bool InstantLine; //ToDo: Global Bool => Game.Instant, maybe?
    
    private static float secPerChar
    {
        get 
        {
            int txtSpd = Game.textSpeed + 1;
            return 1 / (16 + (txtSpd * txtSpd * 9));
        }
    }
    #endregion

	#region Unity Stuff
	void Awake()
    {
		Game.SetCanvasManager(this);
        UnityEngine.Debug.Log(UnityEngine.GameObject.Find("CanvasUI").GetComponent<CanvasUI>());
        CurrentCanvas = UnityEngine.GameObject.Find("CanvasUI").gameObject;
        //fadeScreen = UnityEngine.GameObject.Find("FadeScreen").GetComponent<UnityEngine.UI.Image>();

        /* DialogUIText = UnityEngine.GameObject.Find("DialogText").GetComponent<UnityEngine.UI.Text>();
        DialogUITextDump = UnityEngine.GameObject.Find("DialogTextDump").GetComponent<UnityEngine.UI.Text>();
        DialogUIScrollText = UnityEngine.GameObject.Find("DialogScrollText").GetComponent<UnityEngine.UI.Text>(); */
    }

    public void Start()
    {
        //ShowMenu(CurrentCanvas);
    }

    /// <summary>
    /// Not needed, just wanted to test features...
    /// </summary>
    void Update()
    {
        //Test dialog skin
        //if (UnityEngine.Input.anyKeyDown) RefreshWindowSkin();
    }

    public void ShowMenu(UnityEngine.GameObject screen)
    {
        if (CurrentCanvas != null) CurrentCanvas.SetActive(false);

        CurrentCanvas = screen;
        CurrentCanvas.SetActive(true);
    }
	#endregion

    #region Enumerator
    public enum UICanvas
    {
        NONE,
        /// <summary>
        /// DialogTextWindow:
        /// This is the Text Window for all dialog used throughout game 
        /// (between the Player and the Game), regardless of their purpose
        /// </summary>
        DialogWindow,
        /// <summary>
        /// StartupMenu:
        /// The main menu that occurs right after the StartupScene/Sequence
        /// </summary>
        MainMenu,
        /// <summary>
        /// StartupMenu_Settings:
        /// The settings option located on the <seealso cref="MainMenu"/> Screen
        /// </summary>
        SettingsMenu,
        /// <summary>
        /// PauseMenu:
        /// PauseMenu called when player press start
        /// </summary>
        PauseMenu,
        /// <summary>
        /// BoxScene:
        /// The scene called when the player opens the PC's boxes
        /// </summary>
        BoxScene
    }
	#endregion

	#region Methods
	/// <summary>
	/// Reassigns all the <seealso cref="UnityEngine.Sprite"/> images 
	/// for every <seealso cref="UnityEngine.GameObject"/> with the tag "DialogWindow"
	/// </summary>
	public static void RefreshWindowSkin()
    {
        if (Game.WindowSkinSprite != null)
        {
            UnityEngine.GameObject[] gos = UnityEngine.GameObject.FindGameObjectsWithTag("DialogWindow");
            foreach (UnityEngine.GameObject go in gos)
            {
                go.GetComponent<UnityEngine.UI.Image>().sprite = Game.WindowSkinSprite;
            }
        }
        else return;
    }

    /// <summary>
    /// Reassigns all the <seealso cref="UnityEngine.Sprite"/> images 
    /// for every <seealso cref="UnityEngine.GameObject"/> with the tag "DialogWindow"
    /// </summary>
    public static void RefreshDialogSkin()
    {
        if (Game.DialogSkinSprite != null)
        {
            UnityEngine.GameObject[] gos = UnityEngine.GameObject.FindGameObjectsWithTag("DialogWindow");
            foreach (UnityEngine.GameObject go in gos)
            {
                go.GetComponent<UnityEngine.UI.Image>().sprite = Game.DialogSkinSprite;
            }
        }
        else return;
    }

    public IEnumerator Fade(float time){

        UnityEngine.Debug.Log(fadeScreen);
        fading = true;

        fadeScreen.DOColor(new UnityEngine.Color(0f,0f,0f,1f),1f);
        yield return new UnityEngine.WaitForSeconds(time);
        fadeScreen.DOColor(new UnityEngine.Color(0f,0f,0f,0f),1f);
        fading = false;

    }

    public IEnumerator FadeFromTo(UnityEngine.GameObject from,UnityEngine.GameObject to){
        
        fading = true;

        fadeScreen.DOColor(new UnityEngine.Color(0f,0f,0f,1f),0.5f);
        yield return new UnityEngine.WaitForSeconds(1f);
        from.SetActive(false);
        to.SetActive(true);
        fadeScreen.DOColor(new UnityEngine.Color(0f,0f,0f,0f),0.5f);
        fading = false;
        


    }

    public void switchScene(UICanvas scene){
        switch(scene){
            case UICanvas.PauseMenu:
                foreach(UnityEngine.GameObject obj in scenes){
                    if (obj.GetComponent<PauseMenu>() != null){
                        StartCoroutine(obj.GetComponent<PauseMenu>().control());
                    }
                }
            break;
            case UICanvas.MainMenu:
                foreach(UnityEngine.GameObject obj in scenes){
                    if (obj.GetComponent<StartupSceneHandler>() != null){
                        obj.SetActive(true);
                    }
                }
            break;
            case UICanvas.BoxScene:
                foreach(UnityEngine.GameObject obj in scenes){
                    if (obj.GetComponent<PCHandler>() != null){
                        obj.SetActive(true);
                    }
                }
            break;
        }
    }

    public bool active(UICanvas scene){
        bool ret=false;
        switch(scene){
            case UICanvas.PauseMenu:
                foreach(UnityEngine.GameObject obj in scenes){
                    if (obj.GetComponent<PauseMenu>() != null){
                        ret = obj.activeSelf;
                    }
                }
            break;
            case UICanvas.MainMenu:
                foreach(UnityEngine.GameObject obj in scenes){
                    if (obj.GetComponent<StartupSceneHandler>() != null){
                        ret = obj.activeSelf;
                    }
                }
            break;
            case UICanvas.BoxScene:
                foreach(UnityEngine.GameObject obj in scenes){
                    if (obj.GetComponent<PCHandler>() != null){
                        ret = obj.activeSelf;
                    }
                }
            break;
        }
        return ret;
    }




    public void setInactive(UICanvas canvas){
        switch(canvas){
            case UICanvas.PauseMenu:
                foreach(UnityEngine.GameObject obj in scenes){
                    if (obj.GetComponent<PauseMenu>() != null){
                        obj.SetActive(false);
                    }
                }
            break;
            case UICanvas.BoxScene:
                foreach(UnityEngine.GameObject obj in scenes){
                    if (obj.GetComponent<PCHandler>() != null){
                        obj.SetActive(false);
                    }
                }
            break;
        }
        
    }

    void ChangeScene(UICanvas scene)
    {
        if (ActiveCanvasUI == scene) return;
        else
        {
            UnityEngine.CanvasGroup cgroup;
            UnityEngine.GameObject[] gos = UnityEngine.GameObject.FindGameObjectsWithTag("CanvasUI");
            switch (scene)
            {
                case UICanvas.DialogWindow:
                    foreach(UnityEngine.GameObject go in gos)
                    {
                        cgroup = go.GetComponent<UnityEngine.CanvasGroup>();
                        if (go.name == scene.ToString())
                        {
                            go.SetActive(true);
                            ActiveCanvasUI = scene;
                            cgroup.blocksRaycasts = cgroup.interactable = true;
                        }
                        else
                        {
                            go.SetActive(false);
                            cgroup.blocksRaycasts = cgroup.interactable = false;
                        }
                    }
                    break;
                case UICanvas.MainMenu:
                case UICanvas.SettingsMenu:
                    foreach(UnityEngine.GameObject go in gos)
                    {
                        if (go.name == scene.ToString())
                        {
                            go.SetActive(true);
                            ActiveCanvasUI = scene;
                            cgroup = go.GetComponent<UnityEngine.CanvasGroup>();
                            cgroup.blocksRaycasts = cgroup.interactable = true;
                        }
                        else go.SetActive(false);
                    }
                    break;
                case UICanvas.NONE:
                default:
                    foreach(UnityEngine.GameObject go in gos)
                    {
                        go.SetActive(false);
                    }
                    //ClearDialogText();
                    break;
            }
        }
    }
	#endregion
}