using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleInterface : MonoBehaviour
{
    public static BattleInterface main;
    
    #region Assets
    
    // Interface Sprites
    [Header("Pokemon Panel Sprites")]
    public Sprite genderMale;
    public Sprite  genderFemale;
    public Sprite  poisonned;
    public Sprite  paralyzed;
    public Sprite  frozen;
    public Sprite  burned;
    public Sprite  caught;

    [Space] [Header("Pokemon Party Sprites")]
    public Sprite partyEmpty;
    public Sprite party;
    public Sprite partyStatus;
    public Sprite partyKO;

    [Space] [Header("Command Menu Sprites")]
    public Sprite CMButton;
    public Sprite  CMButtonSelected;
    public Sprite  battleLogo;
    public Sprite  pokemonLogo;
    public Sprite  bagLogo;
    public Sprite  runLogo;

    public Color CMButtonTextColor;
    public Color CMButtonTextSelectedColor;
    
    #endregion
    
    #region Private Properties

    private RectTransform cursor;
    private GameObject commandMenu;
    private GameObject fightMenu;
    
    private int commandMenuIndex = 0;
    private int fightMenuIndex = 0;
    
    #endregion

    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Set variables
        commandMenu = transform.Find("CommandMenu").gameObject;
        cursor = transform.Find("Cursor").GetComponent<RectTransform>();
        
        // Hide interface
        commandMenu.SetActive(false);
    }

    public void Initialize()
    {
        commandMenuIndex = 0;
        fightMenuIndex = 0;
    }
    
    // General Front-end Methods

    // Command Menu Frond-end Methods

    private void setCommandMenuText(string[] texts)
    {
        int i = 1;
        foreach (Transform child in commandMenu.transform)
        {
            Image button = child.GetComponent<Image>();

            button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = texts[i++];
        }
    }
    
    private void refreshCommandMenu()
    {
        int i = 0;
        foreach (Transform child in commandMenu.transform)
        {
            Image button = child.GetComponent<Image>();

            if (commandMenuIndex == i)
            {
                button.sprite = CMButtonSelected;
                button.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = CMButtonTextSelectedColor;

                cursor.position = button.GetComponent<RectTransform>().position;
            }
            else
            {
                button.sprite = CMButton;
                button.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = CMButtonTextColor;
            }

            i++;
        }
    }

    public IEnumerator CommandMenu(int index, string[] texts, int mode, System.Action<int> result)
    {
        // TODO Interface Appearing Animation
        commandMenu.SetActive(true);
        setCommandMenuText(texts);

        do
        {
            refreshCommandMenu();
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (commandMenuIndex > 0)
                {
                    Debug.Log("[CommandMenu] Going Up !");
                    commandMenuIndex--;
                }
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (commandMenuIndex < 3)
                {
                    Debug.Log("[CommandMenu] Going Down !");
                    commandMenuIndex++;
                }
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("[CommandMenu] Select !");
                result(commandMenuIndex);
                break;
            }
            
            yield return null;
        } while(true);
        
        // TODO Interface Disappearing Animation        
    }

    public IEnumerator CommandMenu(int index, string[] texts, int mode, PokemonUnity.Combat.MenuCommands[] lastcmd, PokemonEssentials.Interface.Screen.ICommandMenuDisplay cw, System.Action<int, PokemonUnity.Combat.MenuCommands[]> result)
    {
        int ret = -1;
        do //;loop
        {
            //pbGraphicsUpdate();
            //pbInputUpdate();
            //pbFrameUpdate(cw);
            //  Update selected command
            //if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.LeftArrow) && (cw.index & 1) == 1)
            //{
            //    //(AudioHandler as IGameAudioPlay).pbPlayCursorSE();
            //    cw.index -= 1;
            //}
            //else if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.RightArrow) && (cw.index & 1) == 0)
            //{
            //    //(AudioHandler as IGameAudioPlay).pbPlayCursorSE();
            //    cw.index += 1;
            //}
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.UpArrow) && (cw.index > 0))
            {
                Debug.Log("[CommandMenu] Going Up !");
                //(AudioHandler as IGameAudioPlay).pbPlayCursorSE();
                cw.index--; //-= 2;
            }
            else if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.DownArrow) && (cw.index < 3))
            {
                Debug.Log("[CommandMenu] Going Down !");
                //(AudioHandler as IGameAudioPlay).pbPlayCursorSE();
                cw.index++; //+= 2;
            }
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Space))   // Confirm choice
            {
                Debug.Log("[CommandMenu] Select !");
                //(AudioHandler as IGameAudioPlay).pbPlayDecisionSE();
                ret = cw.index;
                @lastcmd[index] = (PokemonUnity.Combat.MenuCommands)ret;
                //return ret;
                result(ret, lastcmd); yield break;
            }
            else if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Escape) && index == 2 && @lastcmd[0] != (PokemonUnity.Combat.MenuCommands)2)  // Cancel
            {
                //(AudioHandler as IGameAudioPlay).pbPlayDecisionSE();
                //return -1;
                result(-1, lastcmd); yield break;
            }
            yield return new WaitForEndOfFrame(); //WaitForFixedUpdate(); //null;
        } while(true);  
    }
}
