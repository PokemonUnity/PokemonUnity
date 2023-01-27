using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class BattleInterface : MonoBehaviour
{
    public static BattleInterface main;

    #region Assets

    // Interface Sprites
    [Header("Pokemon Panel Sprites")]
    public UnityEngine.Sprite genderMale;
    public UnityEngine.Sprite genderFemale;
    public UnityEngine.Sprite poisonned;
    public UnityEngine.Sprite paralyzed;
    public UnityEngine.Sprite frozen;
    public UnityEngine.Sprite burned;
    public UnityEngine.Sprite caught;

    [Space]
    [Header("Pokemon Party Sprites")]
    public UnityEngine.Sprite partyEmpty;
    public UnityEngine.Sprite party;
    public UnityEngine.Sprite partyStatus;
    public UnityEngine.Sprite partyKO;

    [Space]
    [Header("Command Menu Sprites")]
    public UnityEngine.Sprite CMButton;
    public UnityEngine.Sprite CMButtonSelected;
    public UnityEngine.Sprite battleLogo;
    public UnityEngine.Sprite pokemonLogo;
    public UnityEngine.Sprite bagLogo;
    public UnityEngine.Sprite runLogo;

    public Color CMButtonTextColor;
    public Color CMButtonTextSelectedColor;

    #endregion

    #region Private Properties

    [SerializeField] private RectTransform cursor;
    [SerializeField] private GameObject commandMenu;
    [SerializeField] private GameObject fightMenu;

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
        // Hide interface
        commandMenu.SetActive(false);
        fightMenu.SetActive(false);
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
                yield return null;
                break;
            }

            yield return null;
        } while (true);

        commandMenu.SetActive(false);
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
        } while (true);
    }

    // Fight Menu Frond-end Methods

    private void setFightMenuText(PokemonUnity.Combat.Pokemon battler)
    {
        int i = 0;
        foreach (Transform child in fightMenu.transform)
        {
            if (battler.moves[i].IsNotNullOrNone())
            {
                child.GetComponent<Image>().color = Color.white;
                child.Find("MoveName").GetComponent<TextMeshProUGUI>().text = battler.moves[i].Name;
                child.Find("EffectivenessText").GetComponent<TextMeshProUGUI>().text = "Effective";
                child.Find("PP").GetComponent<TextMeshProUGUI>().text = $"{battler.moves[i].PP}/{battler.moves[i].TotalPP}";

                child.Find("TypeLogo").gameObject.SetActive(true);
                child.Find("EffectivenessLogo").gameObject.SetActive(true);
            }
            else
            {
                child.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                child.Find("MoveName").GetComponent<TextMeshProUGUI>().text = String.Empty;
                child.Find("EffectivenessText").GetComponent<TextMeshProUGUI>().text = String.Empty;
                child.Find("PP").GetComponent<TextMeshProUGUI>().text = String.Empty;

                child.Find("TypeLogo").gameObject.SetActive(false);
                child.Find("EffectivenessLogo").gameObject.SetActive(false);
            }

            i++;
        }
    }

    private void refreshFightMenu()
    {
        int i = 0;
        foreach (Transform child in fightMenu.transform)
        {
            if (fightMenuIndex == i)
            {
                cursor.position = child.GetComponent<RectTransform>().position;
            }

            i++;
        }
    }

    public IEnumerator FightMenu(Pokemon battler, Action<int> result)
    {
        // TODO Interface Appearing Animation
        fightMenu.SetActive(true);
        setFightMenuText(battler);

        do
        {
            refreshFightMenu();

            if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (fightMenuIndex > 0)
                {
                    //Debug.Log("[CommandMenu] Going Up !");
                    fightMenuIndex--;
                }
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (fightMenuIndex < battler.moves.Length - 1 && battler.moves[fightMenuIndex + 1] != null && battler.moves[fightMenuIndex + 1].id != Moves.NONE)
                {
                    //Debug.Log("[CommandMenu] Going Down !");
                    fightMenuIndex++;
                }
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("[CommandMenu] Select !");
                result(fightMenuIndex);
                yield return null;
                break;
            }

            yield return null;
        } while (true);

        fightMenu.SetActive(false);
    }
}