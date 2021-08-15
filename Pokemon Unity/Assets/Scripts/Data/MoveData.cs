//Original Scripts by IIColour (IIColour_Spectrum)

public class MoveData
{
    private PokemonUnity.Attack.Data.MoveData m_data;

    public MoveData(PokemonUnity.Moves move, string name)
    {
        m_data = PokemonUnity.Game.MoveData[move];
    }

    public PokemonUnity.Types getType()
    {
        return m_data.Type;
    }

    public PokemonUnity.Attack.Category getCategory()
    {
        return m_data.Category;
    }

    public int getPower()
    {
        return m_data.Power.GetValueOrDefault();
    }

    public float getAccuracy()
    {
        return m_data.Accuracy.GetValueOrDefault();
    }

    public int getPP()
    {
        return m_data.PP;
    }

    public string getDescription()
    {
        return m_data.Description;
    }

    public string getFieldEffect()
    {
        switch (this.m_data.ID)
        {
            case PokemonUnity.Moves.SURF: return "Surf";
            case PokemonUnity.Moves.STRENGTH: return "Strength";
            case PokemonUnity.Moves.ROCK_SMASH: return "Rock Smash";
            case PokemonUnity.Moves.CUT: return "Cut";
            default:
                return null;
        }
    }
}