//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;

public class PokemonData
{
    private int ID;
    private string name;

    // Temporary
    private PokemonUnity.Monster.Data.PokemonData m_data;
    //

    private string ability1;
    private string ability2;
    private string hiddenAbility;

    private float maleRatio; //-1f is interpreted as genderless

    private float luminance;
    private Color lightColor;

    //private string[] heldItem;

    private int[] movesetLevels;
    private string[] movesetMoves;

    private string[] tmList;

    private int[] evolutionID;
    private string[] evolutionRequirements;


    public PokemonData(PokemonUnity.Pokemons pokemon)
    {
        m_data = PokemonUnity.Game.PokemonData[pokemon];

        //ToDO: tempo
        luminance = 0;
        lightColor = Color.clear;
    }

    public override string ToString()
    {
        string result = ID.ToString() + ", " + name + ", " + m_data.Type[0].ToString() + ", ";
        result += m_data.Type[1].ToString() + ", ";
        result += ability1 + ", ";
        if (ability2 != null)
        {
            result += ability2 + ", ";
        }
        result += hiddenAbility + ", ";
        return result;
    }

    public int getID()
    {
        return (int)m_data.ID;
    }

    public string getName()
    {
        return m_data.ID.toString();
    }

    public float getMaleRatio()
    {
        return maleRatio;
    }

    public PokemonUnity.Types getType1()
    {
        return m_data.Type[0];
    }

    public PokemonUnity.Types getType2()
    {
        return m_data.Type[1];
    }

    public string getAbility(int ability)
    {
        // Will need to test this
        //if (!(ability > 4) && !(ability < 0)) // 0 - 3
        //    return m_data.Ability[ability].ToString();

        if (ability == 0)
        {
            return m_data.Ability[0].ToString();
        }
        else if (ability == 1)
        {
            if (m_data.Ability[1].ToString() == null)
            {
                return m_data.Ability[0].ToString();
            }
            return m_data.Ability[1].ToString();
        }
        else if (ability == 2)
        {
            if (m_data.Ability[2].ToString() == null)
            {
                return m_data.Ability[0].ToString();
            }
            return m_data.Ability[2].ToString();
        }
        return null;
    }

    // Remove?
    public int getBaseFriendship()
    {
        return m_data.BaseFriendship;
    }

    public int getBaseExpYield()
    {
        return m_data.BaseExpYield;
    }

    public int[] getBaseStats()
    {
        return new int[] {m_data.BaseStatsHP, m_data.BaseStatsATK, m_data.BaseStatsDEF, m_data.BaseStatsSPA, m_data.BaseStatsSPD, m_data.BaseStatsSPE};
    }

    public bool hasLight()
    {
        if (luminance > 0)
        {
            return true;
        }
        return false;
    }

    public float getLuminance()
    {
        return luminance;
    }

    public Color getLightColor()
    {
        return lightColor;
    }

    // Remove?
    public int[] getMovesetLevels()
    {
        return movesetLevels;
    }

    public string[] getMovesetMoves()
    {
        return movesetMoves;
    }

    public string[] getTmList()
    {
        return tmList;
    }

    public int[] getEvolutions()
    {
        return evolutionID;
    }

    public string[] getEvolutionRequirements()
    {
        return evolutionRequirements;
    }

    public PokemonUnity.Monster.LevelingRate getLevelingRate()
    {
        return m_data.GrowthRate;
    }

    public int getCatchRate()
    {
        return m_data.CatchRate;
    }

    // Remove GenerateMoveset also
    public string[] GenerateMoveset(int level)
    {
        //Set moveset based off of the highest level moves possible.
        string[] moveset = new string[4];
        PokemonUnity.Monster.Pokemon temp = new PokemonUnity.Monster.Pokemon((PokemonUnity.Pokemons)ID, (byte)level, false);
        temp.GenerateMoveset(level);
        for (int i = 0; i < temp.moves.Length; i++)
        {
            PokemonUnity.Moves move = temp.moves[i].MoveId;
            moveset[i] = move != PokemonUnity.Moves.NONE ? move.toString() : null;
        }
        return moveset;
    }
}