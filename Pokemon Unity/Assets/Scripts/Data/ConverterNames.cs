class ConverterNames
{
    public static string GetPokemonName(PokemonUnity.Pokemons pokemon)
    {
        char[] result = pokemon.ToString().ToLower().ToCharArray();

        switch (pokemon)
        {
            case PokemonUnity.Pokemons.NONE: return null;
            case PokemonUnity.Pokemons.HO_OH: return "Ho-Oh";
            default:
                {
                    return new string(ChangeCharsFromEnum(result));
                }
        }
    }
    public static string GetItemName(PokemonUnity.Inventory.Items item)
    {
        
        char[] result = item.ToString().ToLower().ToCharArray();

        switch (item)
        {
            // Need to change to Poke Ball
            case PokemonUnity.Inventory.Items.NONE: return null;
            case PokemonUnity.Inventory.Items.POKE_BALL: return "Poké Ball";
            case PokemonUnity.Inventory.Items.POKE_DOLL: return "Poké Doll";
            default:
                {
                    return new string(ChangeCharsFromEnum(result));
                }
        }
    }
    public static string GetMoveName(PokemonUnity.Moves move)
    {
        char[] result = move.ToString().ToLower().ToCharArray();

        switch (move)
        {
            case PokemonUnity.Moves.NONE: return null;
            case PokemonUnity.Moves.SAFEGUARD: return "Safe Guard";
            default:
                {
                    return new string(ChangeCharsFromEnum(result));
                }
        }
    }
    public static PokemonUnity.Moves ChangeMoveToEnum(string value)
    {
        if (string.IsNullOrEmpty(value))
            return PokemonUnity.Moves.NONE;
        switch (value)
        {
            case "Safe Guard": return PokemonUnity.Moves.SAFEGUARD;
            case "TM 1": return PokemonUnity.Moves.BULK_UP; // Tempo
            default:
                return (PokemonUnity.Moves)System.Enum.Parse(typeof(PokemonUnity.Moves), new string(ChangeCharsFromString(value.ToUpper().ToCharArray())));
        }
    }

    public static PokemonUnity.Inventory.Items ChangeItemToEnum(string value)
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

    // Change to correct Upper & Lower case 

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