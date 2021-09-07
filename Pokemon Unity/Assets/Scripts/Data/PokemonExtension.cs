using PokemonUnity;
using PokemonUnity.Monster;
using UnityEngine;

// Tempo Use PKU's
public static class PokemonExtension
{
    public static bool IsNotNullOrNone(this Pokemon pokemon)
    {
    	return pokemon != null && pokemon.Species != Pokemons.NONE;
    }
	public static string convertLongID(this Pokemon pokemon)
	{
		string result = ((int)pokemon.Species).ToString();
		while (result.Length < 3)
		{
			result = "0" + result;
		}
		return result;
	}
    public static int healHP(this Pokemon pokemon, float amount)
    {
        int excess = 0;
        int intAmount = Mathf.RoundToInt(amount);
        pokemon.HP += intAmount;
        if (pokemon.HP > pokemon.TotalHP)
        {
            excess = pokemon.HP - pokemon.TotalHP;
            pokemon.HP = pokemon.TotalHP;
        }
        if (pokemon.Status == Status.FAINT && pokemon.HP > 0)
        {
            pokemon.Status = Status.NONE;
        }
        return intAmount - excess;
    }
    public static float getPercentHP(this Pokemon pokemon)
    {
        return 1f - (((float)pokemon.TotalHP - (float)pokemon.HP) / (float)pokemon.TotalHP);
    }
    public static int healPP(this Pokemon pokemon, int move, float amount)
    {
        int excess = 0;
        int intAmount = Mathf.RoundToInt(amount);
        pokemon.moves[move].PP += (byte)intAmount;
        if (pokemon.moves[move].PP > pokemon.moves[move].TotalPP)
        {
            excess = pokemon.moves[move].PP - pokemon.moves[move].TotalPP;
            pokemon.HealPP(move);
        }
        return intAmount - excess;
    }
    public static int getPP(this Pokemon pokemon, int index)
    {
        return pokemon.moves[index].PP;
    }
    public static int[] GetPP(this Pokemon pokemon)
    {
        int[] result = new int[pokemon.moves.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = pokemon.moves[i].PP;
        }
        return result;
    }
    public static int getMaxPP(this Pokemon pokemon, int index)
    {
        return pokemon.moves[index].TotalPP;
    }
    public static int[] GetMaxPP(this Pokemon pokemon)
    {
        int[] result = new int[pokemon.moves.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = pokemon.moves[i].TotalPP;
        }
        return result;
    }
    public static string convertLongID(this Pokemons Species)
    {
        string result = ((int)Species).ToString();
        while (result.Length < 3)
        {
            result = "0" + result;
        }
        return result;
    }
    public static int GetCount(this Pokemon[] partyOrPC)
	{
		int result = 0;
		for (int i = 0; i < partyOrPC.Length; i++)
		{
			if (partyOrPC[i].IsNotNullOrNone())// != null || partyOrPC[i].Species != Pokemons.NONE)
			{
				result += 1;
			}
		}
		return result;
	}
    public static bool setStatus(this Pokemon pokemon, Status status)
    {
        if (pokemon.Status == Status.NONE)
        {
            pokemon.Status = status;
            return true;
        }
        else
        {
            if (status == Status.NONE || status == Status.FAINT)
            {
                pokemon.Status = status;
                return true;
            }
        }
        return false;
    }
    // Array of 6 Individual Values for HP, Atk, Def, Speed, Sp Atk, and Sp Def
    public static int GetIV(this Pokemon pokemon, int index)
    {
        if (index <= 5 && index >= 0)
            return pokemon.IV[index];
        else
        {
            Debug.Log("Called - GetIV error");
            return -1;
        }

    }
    // Seem like this is ERROR ToDo: Fix it
    public static int GetHighestIV(this Pokemon pokemon)
    {
        int highestIVIndex = 0;
        // Tempo
        int rareValue = pokemon.PersonalId;
        //int highestIV = IV_HP;
        int highestIV = pokemon.GetIV(0);
        //by default HP is highest. Check if others are higher. Use RareValue to consistantly break a tie
        //if (IV_ATK > highestIV || (IV_ATK == highestIV && rareValue > 10922))
        if (pokemon.GetIV(1) > highestIV || (pokemon.GetIV(1) == highestIV && rareValue > 10922))
        {
            highestIVIndex = 1;
            //highestIV = IV_ATK;
            highestIV = pokemon.GetIV(1);
        }
        //if (IV_DEF > highestIV || (IV_DEF == highestIV && rareValue > 21844))
        if (pokemon.GetIV(2) > highestIV || (pokemon.GetIV(2) == highestIV && rareValue > 21844))
        {
            highestIVIndex = 2;
            //highestIV = IV_DEF;
            highestIV = pokemon.GetIV(2);
        }
        //if (IV_SPA > highestIV || (IV_SPA == highestIV && rareValue > 32766))
        if (pokemon.GetIV(4) > highestIV || (pokemon.GetIV(4) == highestIV && rareValue > 32766))
        {
            highestIVIndex = 3;
            //highestIV = IV_SPA;
            highestIV = pokemon.GetIV(4);
        }
        //if (IV_SPD > highestIV || (IV_SPD == highestIV && rareValue > 43688))
        if (pokemon.GetIV(5) > highestIV || (pokemon.GetIV(5) == highestIV && rareValue > 43688))
        {
            highestIVIndex = 4;
            //highestIV = IV_SPD;
            highestIV = pokemon.GetIV(5);
        }
        //if (IV_SPE > highestIV || (IV_SPE == highestIV && rareValue > 54610))
        if (pokemon.GetIV(3) > highestIV || (pokemon.GetIV(3) == highestIV && rareValue > 54610))
        {
            highestIVIndex = 5;
            //highestIV = IV_SPE;
            highestIV = pokemon.GetIV(3);
        }
        return highestIVIndex;
    }
    public static bool CanLearnMove(this Pokemon pokemon, Moves move, LearnMethod method)
	{
		Moves[] movelist = pokemon.getMoveList(method);
		for (int i = 0; i < movelist.Length; i++)
		{
			if (movelist[i] == move)
				return true;
		}
		return false;
	}
	public static void replaceMove(this Pokemon pokemon, int index, Moves newMove)
	{
		if (index >= 0 && index < 4)
		{
			pokemon.moves[index] = new PokemonUnity.Attack.Move(newMove);
		}
	}
	public static bool addMove(this Pokemon pokemon, Moves newMove)
	{
		if (!pokemon.hasMove(newMove))
		{
			for (int i = 0; i < 4; i++)
			{
				if (pokemon.moves[i].MoveId == Moves.NONE)
				{
					pokemon.moves[i] = new PokemonUnity.Attack.Move(newMove);
					return true;
				}
			}
		}
		return false;
	}
    public static void swapMoves(this Pokemon pokemon, int target1, int target2)
    {
        PokemonUnity.Attack.Move temp = pokemon.moves[target1];
        pokemon.moves[target1] = pokemon.moves[target2];
        pokemon.moves[target2] = temp;
    }

    public static Moves MoveLearnedAtLevel(this Pokemon pokemon, int level)
    {
        PokemonUnity.Monster.Data.PokemonMoveTree movelist = Game.PokemonMovesData[pokemon.Species];

        int index = movelist.LevelUp.IndexOfValue(level);

        // Tempo
        return index != -1 ? movelist.LevelUp.Keys[index] : Moves.NONE;
    }
    public static int getEvolutionID(this Pokemon pokemon, EvolutionMethod currentMethod, object value)
    {
        PokemonUnity.Monster.Data.PokemonEvolution[] evolutions =
            Game.PokemonEvolutionsData[pokemon.Species];

        for (int i = 0; i < evolutions.Length; i++)
        {
            if (evolutions[i].EvolveMethod == currentMethod)
            {
                // Tempo For now
                if (evolutions[i].EvolveValue.ToString() == value.ToString())
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public static int getEvolutionSpecieID(this Pokemon pokemon, int index)
    {
        PokemonUnity.Monster.Data.PokemonEvolution[] thisdata =
            PokemonUnity.Game.PokemonEvolutionsData[pokemon.Species];
        try
        {
            return (int)thisdata[index].Species;
        }
        catch
        {
            Debug.Log("Failed");
            return -1;
        }
    }

    public static bool evolve(this Pokemon pokemon, int index)
    {
        PokemonUnity.Monster.Data.PokemonEvolution[] evolutions =
            PokemonUnity.Game.PokemonEvolutionsData[pokemon.Species];

        PokemonUnity.Monster.EvolutionMethod method = evolutions[index].EvolveMethod;

        switch (method)
        {
            case EvolutionMethod.Level:
            case EvolutionMethod.Item:
                float hpPercent = ((float)pokemon.HP / (float)pokemon.TotalHP);
                pokemon.EvolvePokemon(evolutions[index].Species);
                pokemon.calcStats(); // Not sure if it do anything
                pokemon.HP = Mathf.RoundToInt(hpPercent * pokemon.HP);
                return true;

            default:
                return false;
        }
    }

    // --------------- Light? Need this? ------------
    public static bool hasLight(this Pokemons pokemon)
    {
        return false;
    }
    public static int getLuminance(this Pokemons pokemon)
    {
        return 0;
    }
    public static UnityEngine.Color getLightColor(this Pokemons pokemon)
    {
        return UnityEngine.Color.clear;
    }
    // ----------------------------------------------

    // --------------- Sprite & Audio ---------------
    public static Sprite[] GetFrontAnim_(this Pokemon pokemon)
    {
        return GetAnimFromID_("PokemonSprites", pokemon.Species, pokemon.Gender, pokemon.IsShiny);
    }

    public static Sprite[] GetFrontAnimFromID_(Pokemons ID, bool? gender, bool isShiny)
    {
        return GetAnimFromID_("PokemonSprites", ID, gender, isShiny);
    }
    // Gender
    private static Sprite[] GetAnimFromID_(string folder, Pokemons ID, bool? gender, bool isShiny)
    {
        Sprite[] animation;
        string shiny = (isShiny) ? "s" : "";
        if (gender == false) // Female
        {
            //Attempt to load Female Variant
            animation = Resources.LoadAll<Sprite>(folder + "/" + convertLongID(ID) + "f" + shiny + "/");
            if (animation.Length == 0)
            {
                Debug.LogWarning("Female Variant NOT Found");
                //Attempt to load Base Variant (possibly Shiny)
                animation = Resources.LoadAll<Sprite>(folder + "/" + convertLongID(ID) + shiny + "/");
            }
            //	else{ Debug.Log("Female Variant Found"); }
        }
        else
        {
            //Attempt to load Base Variant (possibly Shiny)
            animation = Resources.LoadAll<Sprite>(folder + "/" + convertLongID(ID) + shiny + "/");
        }
        if (animation.Length == 0 && isShiny)
        {
            Debug.LogWarning("Shiny Variant NOT Found");
            //No Shiny Variant exists, Attempt to load Regular Variant
            animation = Resources.LoadAll<Sprite>(folder + "/" + convertLongID(ID) + "/");
        }
        return animation;
    }

    public static Sprite[] GetIconsFromID_(Pokemons ID, bool isShiny)
    {
        string shiny = (isShiny) ? "s" : "";
        Sprite[] icons = Resources.LoadAll<Sprite>("PokemonIcons/icon" + convertLongID(ID) + shiny);
        if (icons == null)
        {
            Debug.LogWarning("Shiny Variant NOT Found");
            icons = Resources.LoadAll<Sprite>("PokemonIcons/icon" + convertLongID(ID));
        }
        return icons;
    }

    public static float GetCryPitch(this Pokemon pokemon)
    {
        return (pokemon.Status == PokemonUnity.Status.FAINT) ? 0.9f : 1f - (0.06f * (1 - pokemon.getPercentHP()));
    }

    public static AudioClip GetCry(this Pokemon pokemon)
    {
        return GetCryFromID(pokemon.Species);
    }

    public static AudioClip GetCryFromID(Pokemons ID)
    {
        return Resources.Load<AudioClip>("Audio/cry/" + convertLongID(ID));
    }

    public static Texture[] GetFrontAnim(this Pokemon pokemon)
    {
        return GetAnimFromID("PokemonSprites", pokemon.Species, pokemon.Gender, pokemon.IsShiny);
    }

    public static Texture GetIcons(this Pokemon pokemon)
    {
        return GetIconsFromID(pokemon.Species, pokemon.IsShiny);
    }

    public static Sprite[] GetSprite(this Pokemon pokemon, bool getLight)
    {
        return GetSpriteFromID(pokemon.Species, pokemon.IsShiny, getLight);
    }

    private static Texture[] GetAnimFromID(string folder, Pokemons ID, bool? gender, bool isShiny)
    {
        Texture[] animation;
        string shiny = (isShiny) ? "s" : "";
        if (gender == false)
        {
            //Attempt to load Female Variant
            animation = Resources.LoadAll<Texture>(folder + "/" + convertLongID(ID) + "f" + shiny + "/");
            if (animation.Length == 0)
            {
                Debug.LogWarning("Female Variant NOT Found (may not be required)");
                //Attempt to load Base Variant (possibly Shiny)
                animation = Resources.LoadAll<Texture>(folder + "/" + convertLongID(ID) + shiny + "/");
            }
            //	else{ Debug.Log("Female Variant Found");}
        }
        else
        {
            //Attempt to load Base Variant (possibly Shiny)
            animation = Resources.LoadAll<Texture>(folder + "/" + convertLongID(ID) + shiny + "/");
        }
        if (animation.Length == 0 && isShiny)
        {
            Debug.LogWarning("Shiny Variant NOT Found");
            //No Shiny Variant exists, Attempt to load Regular Variant
            animation = Resources.LoadAll<Texture>(folder + "/" + convertLongID(ID) + "/");
        }
        return animation;
    }

    public static Texture GetIconsFromID(Pokemons ID, bool isShiny)
    {
        string shiny = (isShiny) ? "s" : "";
        Texture icons = Resources.Load<Texture>("PokemonIcons/icon" + convertLongID(ID) + shiny);
        if (icons == null)
        {
            Debug.LogWarning("Shiny Variant NOT Found");
            icons = Resources.Load<Texture>("PokemonIcons/icon" + convertLongID(ID));
        }
        return icons;
    }

    public static Sprite[] GetSpriteFromID(Pokemons ID, bool isShiny, bool getLight)
    {
        string shiny = (isShiny) ? "s" : "";
        string light = (getLight) ? "Lights/" : "";
        Sprite[] spriteSheet = Resources.LoadAll<Sprite>("OverworldPokemonSprites/" + light + convertLongID(ID) + shiny);
        if (spriteSheet.Length == 0)
        {
            //No Light found AND/OR No Shiny found, load non-shiny
            if (isShiny)
            {
                if (getLight)
                {
                    Debug.LogWarning("Shiny Light NOT Found (may not be required)");
                }
                else
                {
                    Debug.LogWarning("Shiny Variant NOT Found");
                }
            }
            spriteSheet = Resources.LoadAll<Sprite>("OverworldPokemonSprites/" + light + convertLongID(ID));
        }
        if (spriteSheet.Length == 0)
        {
            //No Light found OR No Sprite found, return 8 blank sprites
            if (!getLight)
            {
                Debug.LogWarning("Sprite NOT Found");
            }
            else
            {
                Debug.LogWarning("Light NOT Found (may not be required)");
            }
            return new Sprite[8];
        }
        return spriteSheet;
    }
}
