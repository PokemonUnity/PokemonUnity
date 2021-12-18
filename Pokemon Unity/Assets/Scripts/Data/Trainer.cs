//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using PokemonUnity.Monster;

public class Trainer : MonoBehaviour
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

    public PokemonInitialiser[] trainerParty = new PokemonInitialiser[1];
    private Pokemon[] party;

    public AudioClip battleBGM;
    public int samplesLoopStart;

    public AudioClip victoryBGM;
    public int victorySamplesLoopStart;

    public string[] tightSpotDialog;

    public string[] playerVictoryDialog;
    public string[] playerLossDialog;

    public Trainer(Pokemon[] party)
    {
        trainerClass = Class.Trainer;
        trainerName = "";

        this.party = party;
    }

    void Awake()
    {
        party = new Pokemon[trainerParty.Length];
    }

    void Start()
    {
        for (int i = 0; i < trainerParty.Length; i++)
        {
            // ToDo: Add CatchInfo by using TrainerID
            party[i] = new Pokemon((PokemonUnity.Pokemons)trainerParty[i].ID, (byte)trainerParty[i].level, false);
            party[i].setGender(trainerParty[i].gender);
            party[i].setItem(trainerParty[i].heldItem.ToItems());
            party[i].setAbility(trainerParty[i].ability);
            party[i].ballUsed = PokemonUnity.Inventory.Items.POKE_BALL;
        }
    }


    public Pokemon[] GetParty()
    {
        return party;
    }

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
        for (int i = 0; i < party.Length; i++)
        {
            averageLevel += party[i].Level;
        }
        averageLevel = Mathf.CeilToInt((float) averageLevel / (float) party.Length);
        return averageLevel * prizeMoney;
    }
}


[System.Serializable]
public class PokemonInitialiser
{
    public int ID;
    public int level;
    public bool? gender;
    public string heldItem;
    public int ability;
}