using PokemonUnity;
using PokemonUnity.Inventory;

static class PKUName
{
    // -- ENUM --
    public static string toString(this Moves move)
    {
        char[] result = move.ToString().ToLower().ToCharArray();

        switch (move)
        {
            case Moves.NONE: return null;
            case Moves.SAFEGUARD: return "Safe Guard";
            default:
                {
                    return new string(ChangeCharsFromEnum(result));
                }
        }
    }
    public static string toString(this Items item)
    {
        char[] result = item.ToString().ToLower().ToCharArray();

        switch (item)
        {
            // Need to change to Poke Ball
            case Items.NONE: return null;
            case Items.POKE_BALL: return "Poké Ball";
            case Items.POKE_DOLL: return "Poké Doll";
            default:
                {
                    return new string(ChangeCharsFromEnum(result));
                }
        }
    }
    public static string toString(this Pokemons pokemon)
    {
        char[] result = pokemon.ToString().ToLower().ToCharArray();

        switch (pokemon)
        {
            case Pokemons.NONE: return null;
            case Pokemons.HO_OH: return "Ho-Oh";
            default:
                {
                    return new string(ChangeCharsFromEnum(result));
                }
        }
    }
    // -- STRING --
    public static Moves ToMoves(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return Moves.NONE;
        switch (value)
        {
            case "Safe Guard": return Moves.SAFEGUARD;
            case "TM 1": return Moves.BULK_UP; // Tempo
            default:
                return (Moves)System.Enum.Parse(typeof(Moves), new string(ChangeCharsFromString(value.ToUpper().ToCharArray())));
        }
    }
    public static Items ToItems(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return PokemonUnity.Inventory.Items.NONE;
        switch (value)
        {
            case "Poké Ball": return PokemonUnity.Inventory.Items.POKE_BALL;
            case "Poké Doll": return PokemonUnity.Inventory.Items.POKE_DOLL;
            default:
                return (PokemonUnity.Inventory.Items)
                    System.Enum.Parse(typeof(PokemonUnity.Inventory.Items), new string(ChangeCharsFromString(value.ToUpper().ToCharArray())));
        }
    }
    // -- METHOD --

    // Make sure set sting all upper case.
    private static char[] ChangeCharsFromString(char[] value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] == ' ')
            {
                value[i] = '_';
            }

        }
        return value;
    }
    // Make sure set sting all lower case.
    private static char[] ChangeCharsFromEnum(char[] value)
    {
        value[0] = char.ToUpper(value[0]);
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] == '_')
            {
                value[i] = ' ';
                value[i + 1] = char.ToUpper(value[i + 1]);
            }

        }
        return value;
    }
}