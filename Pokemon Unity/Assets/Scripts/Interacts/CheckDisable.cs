using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDisable : MonoBehaviour
{

	public CustomEventDetails.Logic logic;

    public bool bool0;
    public float float0;
    public string string0;
    
	private void Awake ()
    {
        StartCoroutine(disable());
    }

    private IEnumerator disable()
    {
        yield return new WaitForSeconds(0.2f);

        if (checkLogic())
        {
            gameObject.SetActive(false);
        }

        yield return null;
    }

	private bool checkLogic()
	{
		bool passedCheck = false;

        switch (logic)
        {
            case CustomEventDetails.Logic.CVariableEquals:
                if (float0 == SaveData.currentSave.getCVariable(string0))
                {
                    passedCheck = true;
                }
                break;
            case CustomEventDetails.Logic.CVariableGreaterThan:
                if (SaveData.currentSave.getCVariable(string0) > float0)
                {
                    passedCheck = true;
                }
                break;
            case CustomEventDetails.Logic.CVariableLessThan:
                if (SaveData.currentSave.getCVariable(string0) < float0)
                {
                    passedCheck = true;
                }
                break;
            case CustomEventDetails.Logic.GymBadgeNoOwned:
                if (Mathf.FloorToInt(float0) < SaveData.currentSave.gymsBeaten.Length &&
                    Mathf.FloorToInt(float0) >= 0)
                {
                    //ensure input number is valid
                    if (SaveData.currentSave.gymsBeaten[Mathf.FloorToInt(float0)])
                    {
                        passedCheck = true;
                    }
                }
                break;
            case CustomEventDetails.Logic.GymBadgesEarned:
                int badgeCount = 0;
                for (int bi = 0; bi < SaveData.currentSave.gymsBeaten.Length; bi++)
                {
                    if (SaveData.currentSave.gymsBeaten[bi])
                    {
                        badgeCount += 1;
                    }
                }
                if (badgeCount >= float0)
                {
                    passedCheck = true;
                }
                break;
            case CustomEventDetails.Logic.PokemonIDIsInParty:
                for (int pi = 0; pi < 6; pi++)
                {
                    if (SaveData.currentSave.PC.boxes[0][pi] != null)
                    {
                        if (SaveData.currentSave.PC.boxes[0][pi].getID() ==
                            Mathf.FloorToInt(float0))
                        {
                            passedCheck = true;
                            pi = 6;
                        }
                    }
                }
                break;
            case CustomEventDetails.Logic.SpaceInParty:
                passedCheck = (bool0) ? !SaveData.currentSave.PC.hasSpace(0) : SaveData.currentSave.PC.hasSpace(0);
                break;
            
            case CustomEventDetails.Logic.GymBadgeNoNotOwned:
                if (Mathf.FloorToInt(float0) < SaveData.currentSave.gymsBeaten.Length &&
                    Mathf.FloorToInt(float0) >= 0)
                {
                    //ensure input number is valid
                    if (SaveData.currentSave.gymsBeaten[Mathf.FloorToInt(float0)])
                    {
                        passedCheck = false;
                    }
                }
                break;
        }

        return checkLogic();
    }
}
