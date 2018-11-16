//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

[System.Obsolete]
public class TrainerOld : MonoBehaviour
{
    public enum Class
    {
        Trainer,
        AceTrainer
    };

    private static string[] classString = new string[]
    {
        "Trainer",
        "Ace Trainer"
    };

    private static int[] classPrizeMoney = new int[]
    {
        100,
        60
    };

    public Sprite[] uniqueSprites = new Sprite[0];

    public Class trainerClass;
    public string trainerName;

    public int customPrizeMoney = 0;

    public bool isFemale = false;
	

    public AudioClip battleBGM;
    public int samplesLoopStart;

    public AudioClip victoryBGM;
    public int victorySamplesLoopStart;

    public string[] tightSpotDialog;

    public string[] playerVictoryDialog;
    public string[] playerLossDialog;
	
    public string GetName()
    {
        return (!string.IsNullOrEmpty(trainerName))
            ? classString[(int) trainerClass] + " " + trainerName
            : classString[(int) trainerClass];
    }

    public Sprite[] GetSprites()
    {
        Sprite[] sprites = new Sprite[0];
        if (uniqueSprites.Length > 0)
        {
            sprites = uniqueSprites;
        }
        else
        {
            //Try to load female sprite if female
            if (isFemale)
            {
                sprites = Resources.LoadAll<Sprite>("NPCSprites/" + classString[(int) trainerClass] + "_f");
            }
            //Try to load regular sprite if male or female load failed
            if (!isFemale || (isFemale && sprites.Length < 1))
            {
                sprites = Resources.LoadAll<Sprite>("NPCSprites/" + classString[(int) trainerClass]);
            }
        }
        //if all load calls failed, load null as an array
        if (sprites.Length == 0)
        {
            sprites = new Sprite[] {Resources.Load<Sprite>("null")};
        }
        return sprites;
    }

    public int GetPrizeMoney()
    {
        int prizeMoney = (customPrizeMoney > 0) ? customPrizeMoney : classPrizeMoney[(int) trainerClass];
        int averageLevel = 0;
        return averageLevel * prizeMoney;
    }
}

