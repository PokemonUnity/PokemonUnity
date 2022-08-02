//Original Scripts by IIColour (IIColour_Spectrum)

using System.Collections;
using PokemonUnity;
using PokemonUnity.Localization;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;
using PokemonUnity.Utility;
using PokemonEssentials;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using UnityEngine;
using UnityEngine.Serialization;

public class Trainer : MonoBehaviour
{
    public enum Class
    {
        Trainer,
        AceTrainer,
        Youngster,
        Rival,
        NeoRocketGrunt,
        BugCatcher,
        Champion,
        Hiker,
        Lass,
        Fisherman,
        GymLeader
    };

    private static string[] classString = new string[]
    {
        "Trainer",
        "Ace Trainer",
        "Youngster",
        "Rival",
        "Team Neo-Rocket Grunt",
        "Bug Catcher",
        "Champion",
        "Hiker",
        "Lass",
        "Fisherman",
        "Gym Leader"
    };

    private static int[] classPrizeMoney = new int[]
    {
        100,
        60,
        16,
        50,
        80,
        16,
        200,
        32,
        16,
        32,
        120
    };

    [Header("Trainer Settings")]
    
    public string uniqueSprites;

    public Class trainerClass;
    public string trainerName;

    public int customPrizeMoney = 0;

    public bool isFemale = false;

    public PokemonInitialiser[] trainerParty = new PokemonInitialiser[1];
    private IPokemon[] party;

    [Header("Music")]
    
    public AudioClip battleBGM;
    public int samplesLoopStart;

    public AudioClip victoryBGM;
    public int victorySamplesLoopStart;

    public AudioClip lowHpBGM;
    public int lowHpBGMSamplesLoopStart;

    [Header("Environment")]
    public MapSettings.Environment environment;

    
    [Space]
    
    [Header("Dialogs")]
    
    [Space]
    
    [Header("English Dialogs")]
    [FormerlySerializedAs("tightSpotDialog")]
    public string[] en_tightSpotDialog;

    [FormerlySerializedAs("playerVictoryDialog")] 
    public string[] en_playerVictoryDialog;
    [FormerlySerializedAs("playerLossDialog")] 
    public string[] en_playerLossDialog;
    
    [Space]
    
    [Header("French Dialogs")]
    public string[] fr_tightSpotDialog;

    public string[] fr_playerVictoryDialog;
    public string[] fr_playerLossDialog;

    public Trainer(IPokemon[] party)
    {
        this.trainerClass = Class.Trainer;
        this.trainerName = "";

        this.party = party;
    }

    void Awake()
    {
        party = new IPokemon[trainerParty.Length];
    }

    void Start()
    {
        for (int i = 0; i < trainerParty.Length; i++)
        {
            //party[i] = new Pokemon(trainerParty[i].ID, trainerParty[i].gender, trainerParty[i].level, "Poké Ball",
            //    trainerParty[i].heldItem, trainerName, trainerParty[i].ability);
            
            int moveIndex = 0;
            for (int k = 0; k < trainerParty[i].moveset.Length && k < 4; ++k)
            {
                if (trainerParty[i].moveset[k].Length > 0)
                {
                    //party[i].replaceMove(moveIndex, trainerParty[i].moveset[k]);
                    moveIndex++;
                }
                else
                {
                    break;
                }
            }
            
           
        }
    }


    public IPokemon[] GetParty()
    {
        return party;
    }

    public string GetName()
    {
        return (!string.IsNullOrEmpty(trainerName))
            ? classString[(int) trainerClass] + " " + trainerName
            : classString[(int) trainerClass];
    }

    public UnityEngine.Sprite[] GetSprites()
    {
        UnityEngine.Sprite[] sprites = new UnityEngine.Sprite[0];
        if (uniqueSprites.Length > 0)
        {
            sprites = Resources.LoadAll<UnityEngine.Sprite>("TrainerBattlers/" + uniqueSprites);
        }
        else
        {
            //Try to load female sprite if female
            if (isFemale)
            {
                sprites = Resources.LoadAll<UnityEngine.Sprite>("TrainerBattlers/" + classString[(int) trainerClass] + "_f");
            }
            //Try to load regular sprite if male or female load failed
            if (!isFemale || (isFemale && sprites.Length < 1))
            {
                sprites = Resources.LoadAll<UnityEngine.Sprite>("TrainerBattlers/" + classString[(int) trainerClass]);
            }
        }
        //if all load calls failed, load null as an array
        if (sprites.Length == 0)
        {
            sprites = new UnityEngine.Sprite[] {Resources.Load<UnityEngine.Sprite>("null")};
        }
        
        for (int i = 0; i < sprites.Length; ++i)
        {
            sprites[i] = UnityEngine.Sprite.Create(sprites[i].texture, sprites[i].rect, new Vector2(0.5f, 0), 16f);
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

    public void HealParty()
    {
        foreach (IPokemon pokemon in party)
        {
            pokemon.Heal();
        }
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
    public string[] moveset;
}