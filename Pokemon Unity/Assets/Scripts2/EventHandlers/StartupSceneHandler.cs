using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.EventSystems;

class StartupSceneHandler : UnityEngine.MonoBehaviour, UnityEngine.EventSystems.ISubmitHandler, UnityEngine.EventSystems.IScrollHandler
{
	private static GameVariables PersistantPlayerData;
    private static UnityEngine.GameObject MainMenu;// = UnityEngine.GameObject.Find("MainMenu");
    /// <summary>
    /// This is the panel display that shows save data for currently selected CONTINUE option
    /// </summary>
    private static UnityEngine.GameObject FileDataPanel;// = MainMenu.transform.GetChild(0).gameObject;
    private static UnityEngine.GameObject MenuOptions;// = MainMenu.transform.GetChild(1).gameObject;
    //private static UnityEngine.UI.Text DialogUITextDump = MainMenu.GetComponent<UnityEngine.UI.Text>();
    //private static UnityEngine.UI.Text DialogUIScrollText = UnityEngine.GameObject.Find("DialogScrollText").GetComponent<UnityEngine.UI.Text>();
    void Awake()
    {
		PersistantPlayerData = new GameVariables();
		//ToDo: On Start-up, Load & Process GameVariables, to begin and instantiate game
        MainMenu = UnityEngine.GameObject.Find("MainMenu");
        FileDataPanel = MainMenu.transform.GetChild(0).gameObject;
        MenuOptions = MainMenu.transform.GetChild(1).gameObject;
    }
    void OnEnable1()
    {
        /* If no previous saved data was found: 
         * disable the right playerData window,
         * disable continue menu-option,
         * extend menu option width size,
         * transform menu positions to collapse 
         * to top and fill in empty gap
         */
        //Load Any/All GameSaves
        //"ContinuePanel"
        MenuOptions.transform.GetChild(0).gameObject.SetActive(GameVariables.SaveFileFound);
        FileDataPanel.SetActive(GameVariables.SaveFileFound);
        if (!GameVariables.SaveFileFound)
        {
            //"MainMenu"
            //Stretch menu to fit width across
            MenuOptions.GetComponent<UnityEngine.RectTransform>().anchorMax = new UnityEngine.Vector2(1, 1);
            //Move options up to fill in gap
            MenuOptions.transform.GetChild(1).gameObject.transform.localPosition += new UnityEngine.Vector3(0f, 70f, 0f);
            MenuOptions.transform.GetChild(2).gameObject.transform.localPosition += new UnityEngine.Vector3(0f, 70f, 0f);
            MenuOptions.transform.GetChild(3).gameObject.transform.localPosition += new UnityEngine.Vector3(0f, 70f, 0f);
            //UnityEngine.Debug.Log(MenuOptions.transform.GetChild(1).gameObject.transform.position);
            //UnityEngine.Debug.Log(MenuOptions.transform.GetChild(1).gameObject.transform.localPosition);
            //ToDo: Git was giving build error on `ForceUpdateRectTransforms()`; says it doesnt exist...
            //Refresh the changes to display the new position
            //MenuOptions.transform.GetChild(1).gameObject.GetComponent<UnityEngine.RectTransform>().ForceUpdateRectTransforms();
            //MenuOptions.transform.GetChild(2).gameObject.GetComponent<UnityEngine.RectTransform>().ForceUpdateRectTransforms();
            //MenuOptions.transform.GetChild(3).gameObject.GetComponent<UnityEngine.RectTransform>().ForceUpdateRectTransforms();
        }

    }
    void Start()
    {
    }


    void Update()
    {
        /* Ping GameNetwork server every 15-45secs
         * If game server is offline or unable to
         * ping connection:
         * Netplay toggle-icon bool equals false
         * otherwise toggle bool to true
         */
        //StartCoroutine(PingServerEveryXsec);
        //Use coroutine that has a while loop instead of using here
        /*while (MainMenu.activeSelf)
        {
            //While scene is enabled, run coroutine to ping server
            break;
        }*/
        //int index = (int)(UnityEngine.Time.timeSinceLevelLoad * Settings.framesPerSecond);
        //index = index % sprites[].Length;
    }


    /* If Continue option is available:
     * file slot data should reflect in the 
     * playerData window on the right side;
     * disable slot options with no data
     */
    void ContinueSavedGame()
    {
        //If Continue Option is select
        if (MenuOptions.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Toggle>().isOn)
        {
            //Get Toggle Value from Toggle group for which toggleOption is selected
            //use gamesave toggle to load game from that slot
        }
    }

    public void ChangeDataPanel(int slot)
    {
        //Refresh the panel for continue screen to reflect gamesave data
        UnityEngine.Debug.Log(slot);
    }

    public void OnScroll(PointerEventData eventData)
    {
        //throw new NotImplementedException();

    }

    public void OnSubmit(BaseEventData eventData)
    {
        //throw new NotImplementedException();
        switch (eventData.selectedObject.name)
        {
            //If the object is slots, submit continue
            case "":
            //If the object is continue, transistion to next scene
            case "1":
            default:
                break;
        }
    }

    /* If settings option is accessed, 
     * Use GameVariables.ChangeScene to transition
     */
}
