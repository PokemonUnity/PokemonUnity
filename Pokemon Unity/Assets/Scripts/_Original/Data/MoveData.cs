//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class MoveData
{
    public enum Effect
    {
        //comments in brackets are the float parameters
        Chance, //if this check fails, no later effects will run
        Unique, //for the very specific move effects
        Sound,
        Punch,
        Powder,
        Gender, //(only works on 0= opposite, 1= opposite or genderless)
        Burn,
        Freeze,
        Paralyze,
        Poison,
        Sleep,
        Toxic,
        Flinch, //(chance)
        ATK,
        DEF,
        SPA,
        SPD,
        SPE,
        ACC,
        EVA,
        ATKself,
        DEFself,
        SPAself,
        SPDself,
        SPEself,
        ACCself,
        EVAself,
        xHits,
        Heal,
        HPDrain,
        SetDamage,
        //(x=set, 0=2-5)    (amount)      (amount)      (x=set, 0=level)
        Critical,
        Recoil,
        RecoilMax //based off of user's maxHp
    };

    public enum Category
    {
        PHYSICAL,
        SPECIAL,
        STATUS
    };

    public enum Contest
    {
        COOL,
        BEAUTIFUL,
        CUTE,
        CLEVER,
        TOUGH
    }

    public enum Target
    {
        SELF,
        ADJACENT,
        ANY,
        ADJACENTALLY,
        ADJACENTOPPONENT,
        ADJACENTALLYSELF,
        ALL,
        ALLADJACENT,
        ALLADJACENTOPPONENT,
        ALLOPPONENT,
        ALLALLY
    }

    private string name;

    private PokemonData.Type type;
    private Category category;
    private int power;
    private float accuracy;
    private int PP;
    private Target target;
    private int priority;
    private bool contact;
    private bool protectable;
    private bool magicCoatable;
    private bool snatchable;
    private Effect[] moveEffects;
    private float[] moveParameters;
    private Contest contest;
    private int appeal;
    private int jamming;
    private string description;
    private string fieldEffect;


    public MoveData(string name, PokemonData.Type type, Category category, int power, float accuracy,
        int PP, Contest contest, int appeal, int jamming, string description)
    {
        this.name = name;
        this.type = type;
        this.category = category;
        this.power = power;
        this.accuracy = accuracy;
        this.PP = PP;
        this.contest = contest;
        this.appeal = appeal;
        this.jamming = jamming;
        this.description = description;


        //debug filler data
        this.moveEffects = new Effect[0];
        this.moveParameters = new float[0];
        this.target = Target.ADJACENT;
        if (category == Category.PHYSICAL)
        {
            this.contact = true;
        }
        else
        {
            this.contact = false;
        }
        this.protectable = true;
        if (category == Category.STATUS)
        {
            this.magicCoatable = true;
        }
        else
        {
            this.magicCoatable = false;
        }
    }

    public MoveData(string name, PokemonData.Type type, Category category, int power, float accuracy,
        int PP, Contest contest, int appeal, int jamming, string description, string fieldEffect)
    {
        this.name = name;
        this.type = type;
        this.category = category;
        this.power = power;
        this.accuracy = accuracy;
        this.PP = PP;
        this.contest = contest;
        this.appeal = appeal;
        this.jamming = jamming;
        this.fieldEffect = fieldEffect;
        this.description = description;


        //debug filler data
        this.moveEffects = new Effect[0];
        this.moveParameters = new float[0];
        this.target = Target.ADJACENT;
        if (category == Category.PHYSICAL)
        {
            this.contact = true;
        }
        else
        {
            this.contact = false;
        }
        this.protectable = true;
        if (category == Category.STATUS)
        {
            this.magicCoatable = true;
        }
        else
        {
            this.magicCoatable = false;
        }
    }


    public MoveData(string name, PokemonData.Type type, Category category, int power, float accuracy, int PP,
        Target target,
        int priority, bool contact, bool protectable, bool magicCoatable, bool snatchable,
        Effect[] moveEffects, float[] moveParameters,
        Contest contest, int appeal, int jamming, string description)
    {
        this.name = name;
        this.type = type;
        this.category = category;
        this.power = power;
        this.accuracy = accuracy;
        this.PP = PP;
        this.target = target;
        this.priority = priority;
        this.contact = contact;
        this.protectable = protectable;
        this.magicCoatable = magicCoatable;
        this.snatchable = snatchable;
        this.moveEffects = moveEffects;
        this.moveParameters = moveParameters;
        this.contest = contest;
        this.appeal = appeal;
        this.jamming = jamming;
        this.description = description;
    }

    public MoveData(string name, PokemonData.Type type, Category category, int power, float accuracy, int PP,
        Target target,
        int priority, bool contact, bool protectable, bool magicCoatable, bool snatchable,
        Effect[] moveEffects, float[] moveParameters,
        Contest contest, int appeal, int jamming, string description, string fieldEffect)
    {
        this.name = name;
        this.type = type;
        this.category = category;
        this.power = power;
        this.accuracy = accuracy;
        this.PP = PP;
        this.target = target;
        this.priority = priority;
        this.contact = contact;
        this.protectable = protectable;
        this.magicCoatable = magicCoatable;
        this.snatchable = snatchable;
        this.moveEffects = moveEffects;
        this.moveParameters = moveParameters;
        this.contest = contest;
        this.appeal = appeal;
        this.jamming = jamming;
        this.description = description;
        this.fieldEffect = fieldEffect;
    }

    public string getName()
    {
        return name;
    }

    public PokemonData.Type getType()
    {
        return type;
    }

    public Category getCategory()
    {
        return category;
    }

    public int getPower()
    {
        return power;
    }

    public float getAccuracy()
    {
        return accuracy;
    }

    public int getPP()
    {
        return PP;
    }

    public Target getTarget()
    {
        return target;
    }

    public int getPriority()
    {
        return priority;
    }

    public bool getContact()
    {
        return contact;
    }

    public bool getProtectable()
    {
        return protectable;
    }

    public bool getMagicCoatable()
    {
        return magicCoatable;
    }

    public bool getSnatchable()
    {
        return snatchable;
    }

    public Effect[] getMoveEffects()
    {
        return moveEffects;
    }

    public float[] getMoveParameters()
    {
        return moveParameters;
    }

    public Contest getContest()
    {
        return contest;
    }

    public int getAppeal()
    {
        return appeal;
    }

    public int getJamming()
    {
        return jamming;
    }

    public string getDescription()
    {
        return description;
    }

    public string getFieldEffect()
    {
        return fieldEffect;
    }


    public bool hasMoveEffect(Effect effect)
    {
        for (int i = 0; i < moveEffects.Length; i++)
        {
            if (moveEffects[i] == effect)
            {
                return true;
            }
        }
        return false;
    }

    public float getMoveParameter(Effect effect)
    {
        for (int i = 0; i < moveEffects.Length; i++)
        {
            if (moveParameters.Length > i)
            {
                if (moveEffects[i] == effect)
                {
                    return moveParameters[i];
                }
            }
        }
        return 0f;
    }
}