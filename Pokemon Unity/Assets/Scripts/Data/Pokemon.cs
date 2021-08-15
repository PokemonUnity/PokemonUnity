//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;

//[System.Serializable]
public class Pokemon
{
    private PokemonUnity.Monster.Pokemon m_Pokemon;

    private Gender gender;
    public enum Gender
    {
        GENDERLESS = 0,
        FEMALE,
        MALE,
        RANDOM, // To Random
        // ToDo: Remove Below
        NONE,
        CALCULATE
    }

    private int rareValue;

    private string metDate;
    private string metMap;

    public PokemonUnity.Monster.Pokemon GetPokemon() { return m_Pokemon; }
    // Tempo
    public int GetRareValue() { return rareValue; }

    private bool? GetGenderData(Gender gender)
    {
        switch (gender)
        {
            case Gender.MALE: return true;
            case Gender.FEMALE: return false;
            case Gender.GENDERLESS: return null;
            default:
                return null;
        }
    }

    public Pokemon()
    {
        m_Pokemon = new PokemonUnity.Monster.Pokemon(PokemonUnity.Pokemons.NONE);
    }
    public Pokemon(PokemonUnity.Monster.Pokemon pokemon, int rareValue, string metDate, string metMap)
    {
        m_Pokemon = pokemon;
        this.rareValue = rareValue;
        this.metDate = metDate; this.metMap = metMap;
    }
    public Pokemon(PokemonUnity.Pokemons pokemonID, Gender gender, int level, string nickname = null)
    {
        m_Pokemon = new PokemonUnity.Monster.Pokemon(pokemonID, (byte)level, false);
        m_Pokemon.SetNickname(nickname);
        m_Pokemon.setGender(GetGenderData(gender));
        this.gender = gender;
        calculateStats();
        m_Pokemon.GenerateMoveset();
        m_Pokemon.ballUsed = PokemonUnity.Inventory.Items.POKE_BALL;

        PokemonUnity.Character.TrainerData data = new PokemonUnity.Character.TrainerData(SaveData.currentSave.savefile.playerName, SaveData.currentSave.savefile.isMale);
        
        m_Pokemon.SetCatchInfos(data);
    }

    //Recalculate the pokemon's Stats.
    public void calculateStats()
    {
        m_Pokemon.calcStats();
    }

    public string swapHeldItem(string newItem)
    {
        return ConverterNames.GetItemName(m_Pokemon.SwapItem(ConverterNames.ChangeItemToEnum(newItem)));
    }

    public void addExp(int expAdded)
    {
        int HP;
        if (this.getCurrentHP() != this.getHP())
        {
            // Use percent
            float hpPercent = ((float)this.getCurrentHP() / (float)this.getHP());
            // -----------

            // Use maxhp - oldmaxhp to restore your currenthp
            //int oldtotalhp = getHP();
            //int oldcurrenthp = getCurrentHP();
            // -----------

            m_Pokemon.Experience.AddExperience(expAdded);

            // Use percent
            HP = Mathf.RoundToInt(hpPercent * this.getHP());
            // -----------

            // Use maxhp - oldmaxhp to restore your currenthp
            //HP = oldcurrenthp + (getHP() - oldtotalhp);
            // -----------
        }
        else
        {
            m_Pokemon.Experience.AddExperience(expAdded);
            HP = getHP();
        }
        m_Pokemon.HP = HP;
        calculateStats(); // Not sure if it do anything
    }

    public bool addEVs(string stat, float amount)
    {
        new System.NotImplementedException();
        return false; //returns false if total or relevant EV cap was reached before running.

    }

    ///Evolution//////////////////////////////////////////////////////
    public int getEvolutionID(PokemonUnity.Monster.EvolutionMethod currentMethod, object value)
    {
        PokemonUnity.Monster.Data.PokemonEvolution[] evolutions =
            PokemonUnity.Game.PokemonEvolutionsData[(PokemonUnity.Pokemons)getID()];

        for (int i = 0; i < evolutions.Length; i++)
        {
            if (evolutions[i].EvolveMethod == currentMethod)
            {
                // Tempo For now
                if (evolutions[i].EvolveValue.ToString() == value.ToString())
                {
                    //Debug.Log("Matched: EValue: " + evolutions[i].EvolveValue + " Value: " + value);
                    return i;
                }
            }
        }

        return -1;
    }

    public int getEvolutionSpecieID(int index)
    {
        PokemonUnity.Monster.Data.PokemonEvolution[] thisdata = 
            PokemonUnity.Game.PokemonEvolutionsData[(PokemonUnity.Pokemons)getID()];
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

    public bool evolve(int index)
    {
        PokemonUnity.Monster.Data.PokemonEvolution[] evolutions =
            PokemonUnity.Game.PokemonEvolutionsData[(PokemonUnity.Pokemons)getID()];

        PokemonUnity.Monster.EvolutionMethod method = evolutions[index].EvolveMethod;

        switch (method)
        {
            case PokemonUnity.Monster.EvolutionMethod.Level:
            case PokemonUnity.Monster.EvolutionMethod.Item:
                float hpPercent = ((float)this.getCurrentHP() / (float)this.getHP());
                m_Pokemon.EvolvePokemon(evolutions[index].Species);
                calculateStats(); // Not sure if it do anything
                m_Pokemon.HP = Mathf.RoundToInt(hpPercent * getHP());
                return true;

            default:
                return false;
        }
    }
    //////////////////////////////////////////////////////////////////

    //return a string that contains all of this pokemon's data & NT = Not implemented
    public override string ToString()
    {
        string result = getID() + ": " + this.getName() + "(" + PokemonDatabase.getPokemon(getID()).getName() +
                        "), " +
                        m_Pokemon.Gender.ToString() + ", Level " + m_Pokemon.Level + ", EXP: " + getExp() + ", To next: " + (getExpNext() - getExp()) +
                        ", Friendship: " + m_Pokemon.Happiness + ", RareValue=" + rareValue + ", Pokerus=" + "NT" /*pokerus.ToString()*/ +
                        ", Shiny=" + getIsShiny().ToString() +
                        ", Status: " + getStatus().ToString() + ", Ball: " + getCaughtBall() + ", Item: " + getHeldItem() +
                        ", met at Level " + m_Pokemon.ObtainLevel + " on " + metDate + " at " + metMap +
                        ", OT: " + "NT" + ", ID: " + "NT" +
                        ", IVs: " + GetIV(0) + "," + GetIV(1) + "," + GetIV(2) + "," + GetIV(4) + "," + GetIV(5) + "," + GetIV(3) +
                        ", EVs: " + GetEV(0) + "," + GetEV(1) + "," + GetEV(2) + "," + GetEV(4) + "," + GetEV(5) + "," + GetEV(3) +
                        ", Stats: " + m_Pokemon.HP + "/" + m_Pokemon.TotalHP + "," + getATK() + "," + getDEF() + "," + getSPA() + "," + getSPD() + "," + getSPE() +
                        ", Nature: " + getNature() + ", " + PokemonDatabase.getPokemon(getID()).getAbility(getAbility());
        result += ", [";
        for (int i = 0; i < 4; i++)
        {
            if (m_Pokemon.moves[i].MoveId != PokemonUnity.Moves.NONE)
            {
                result += ConverterNames.GetMoveName(m_Pokemon.moves[i].MoveId) + ": " + m_Pokemon.moves[i].PP + "/" + m_Pokemon.moves[i].TotalPP + ", ";
            }
        }
        result = result.Remove(result.Length - 2, 2);
        result += "]";

        return result;
    }

    ////Status////////////////////////////////////////////////////////
    //Heal the pokemon
    public void healFull()
    {
        m_Pokemon.Heal();
    }

    ///returns the excess hp
    public int healHP(float amount)
    {
        int excess = 0;
        int intAmount = Mathf.RoundToInt(amount);
        m_Pokemon.HP += intAmount;
        if (m_Pokemon.HP > m_Pokemon.TotalHP)
        {
            excess = m_Pokemon.HP - m_Pokemon.TotalHP;
            m_Pokemon.HP = m_Pokemon.TotalHP;
        }
        if (getStatus() == PokemonUnity.Status.FAINT && m_Pokemon.HP > 0)
        {
            m_Pokemon.Status = PokemonUnity.Status.NONE;
        }
        return intAmount - excess;
    }

    public int healPP(int move, float amount)
    {
        int excess = 0;
        int intAmount = Mathf.RoundToInt(amount);
        m_Pokemon.moves[move].PP += (byte)intAmount;
        if (m_Pokemon.moves[move].PP > m_Pokemon.moves[move].TotalPP)
        {
            excess = m_Pokemon.moves[move].PP - m_Pokemon.moves[move].TotalPP;
            m_Pokemon.HealPP(move);
        }
        return intAmount - excess;
    }
    
    public bool setStatus(PokemonUnity.Status status)
    {
        if (getStatus() == PokemonUnity.Status.NONE)
        {
            m_Pokemon.Status = status;
            return true;
        }
        else
        {
            if (status == PokemonUnity.Status.NONE || status == PokemonUnity.Status.FAINT)
            {
                m_Pokemon.Status = status;
                return true;
            }
        }
        return false;
    }
    ////Move//////////////////////////////////////////////////////////
    public string getFirstFEInstance(string moveName)
    {
        for (int i = 0; i < m_Pokemon.moves.Length; i++)
        {
            if (MoveDatabase.getMove(m_Pokemon.moves[i].MoveId).getFieldEffect() == moveName)
            {
                return ConverterNames.GetMoveName(m_Pokemon.moves[i].MoveId);
            }
        }
        return null;
    }

    public string[] getMoveset()
    {
        string[] result = new string[4];
        PokemonUnity.Moves[] temp = getMovesets();
        for (int i = 0; i < 4; i++)
        {
            result[i] = ConverterNames.GetMoveName(temp[i]);
        }
        return result;
    }
    public PokemonUnity.Moves[] getMovesets()
    {
        PokemonUnity.Moves[] result = new PokemonUnity.Moves[4];
        for (int i = 0; i < 4; i++)
        {
            result[i] = m_Pokemon.moves[i].MoveId;
        }

        return result;
    }

    public void swapMoves(int target1, int target2)
    {
        PokemonUnity.Attack.Move temp = m_Pokemon.moves[target1];
        m_Pokemon.moves[target1] = m_Pokemon.moves[target2];
        m_Pokemon.moves[target2] = temp;
    }

    /// Returns false if no room to add the new move OR move already is learned.
    public bool addMove(PokemonUnity.Moves newMove)
    {
        if (!HasMove(newMove))
        {
            for (int i = 0; i < 4; i++)
            {
                if (m_Pokemon.moves[i].MoveId == PokemonUnity.Moves.NONE)
                {
                    m_Pokemon.moves[i] = new PokemonUnity.Attack.Move(newMove);
                    return true;
                }
            }
        }
        return false;
    }

    public void replaceMove(int index, PokemonUnity.Moves newMove)
    {
        if (index >= 0 && index < 4)
        {
            m_Pokemon.moves[index] = new PokemonUnity.Attack.Move(newMove);
        }
    }

    public int getMoveCount()
    {
        return m_Pokemon.countMoves();
    }

    public bool HasMove(PokemonUnity.Moves move)
    {
        if (move == PokemonUnity.Moves.NONE)
        {
            return false;
        }
        for (int i = 0; i < m_Pokemon.moves.Length; i++)
        {
            if (m_Pokemon.moves[i].MoveId == move)
            {
                return true;
            }
        }
        return false;
    }

    // Find move that Pokemon can learn But only Machine for now
    public bool CanLearnMove(PokemonUnity.Moves move, PokemonUnity.Monster.LearnMethod method)
    {
        PokemonUnity.Moves[] movelist = m_Pokemon.getMoveList(method);
        for (int i = 0; i < movelist.Length; i++)
        {
            if (movelist[i] == move)
                return true;
        }
        return false;
    }

    // Find move that Pokemon can learn at level
    public string MoveLearnedAtLevel(int level)
    {
        PokemonUnity.Monster.Data.PokemonMoveTree movelist = PokemonUnity.Game.PokemonMovesData[(PokemonUnity.Pokemons)getID()];

        int index = movelist.LevelUp.IndexOfValue(level);

        // Tempo
        return index != -1 ? movelist.LevelUp.Keys[index].ToString() : null;
    }

    public int getMaxPP(int index)
    {
        return m_Pokemon.moves[index].TotalPP;
    }

    public int[] GetMaxPP()
    {
        int[] result = new int[m_Pokemon.moves.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = m_Pokemon.moves[i].TotalPP;
        }
        return result;
    }

    public int getPP(int index)
    {
        return m_Pokemon.moves[index].PP;
    }

    public int[] GetPP()
    {
        int[] result = new int[m_Pokemon.moves.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = m_Pokemon.moves[i].PP;
        }
        return result;
    }

    /////ID///////////////////////////////////////////////////////////
    public int getID()
    {
        return (int)m_Pokemon.Species;
    }

    public string getLongID()
    {
        string result = getID().ToString();
        while (result.Length < 3)
        {
            result = "0" + result;
        }
        return result;
    }

    public static string convertLongID(int ID)
    {
        string result = ID.ToString();
        while (result.Length < 3)
        {
            result = "0" + result;
        }
        return result;
    }

    //Get the pokemon's nickname, or regular name if it has none.
    public string getName()
    {
        return m_Pokemon.Name;
    }

    public Gender getGender()
    {
        return gender;
    }

    public int getLevel()
    {
        return m_Pokemon.Level;
    } 

    // Tempo
    public int getExpLevel(int lvl)
    {
        return PokemonUnity.Monster.Data.Experience.GetStartExperience(m_Pokemon.GrowthRate, lvl);
    }

    public int getExp()
    {
        return m_Pokemon.Experience.Current;
    } 

    public int getExpNext()
    {
        return m_Pokemon.Experience.NextLevel;
    } 

    public bool getIsShiny()
    {
        return m_Pokemon.IsShiny;
    }

    public PokemonUnity.Status getStatus()
    {
        return m_Pokemon.Status;
    }

    public string getCaughtBall()
    {
        return ConverterNames.GetItemName(m_Pokemon.ballUsed);
    }

    public string getHeldItem()
    {
        return ConverterNames.GetItemName(m_Pokemon.Item);
    }

    public string getMetDate()
    {
        return "Not Implement";
    }

    public string getMetMap()
    {
        return m_Pokemon.ObtainMap.ToString();
    }

    public int getMetLevel()
    {
        return m_Pokemon.ObtainLevel;
    }

    public string getOT()
    {
        return m_Pokemon.OT.Value.Name;
        //return m_Pokemon.TrainerId; // Need to check
    }

    public int getIDno()
    {
        return int.Parse(m_Pokemon.TrainerId); // Need to check
    }

    // Array of 6 Individual Values for HP, Atk, Def, Speed, Sp Atk, and Sp Def
    public int GetIV(int index)
    {
        if (index <= 5 && index >= 0)
            return m_Pokemon.IV[index];
        else
        {
            Debug.Log("Called - GetIV error");
            return -1;
        }

    }

    public int GetEV(int index)
    {
        if (index <= 5 && index >= 0)
            return m_Pokemon.EV[index];
        else
        {
            Debug.Log("Called - GetEV error");
            return -1;
        }
    }

    // Seem like this is ERROR ToDo: Fix it
    public int GetHighestIV()
    {
        int highestIVIndex = 0;
        //int highestIV = IV_HP;
        int highestIV = GetIV(0);
        //by default HP is highest. Check if others are higher. Use RareValue to consistantly break a tie
        //if (IV_ATK > highestIV || (IV_ATK == highestIV && rareValue > 10922))
        if (GetIV(1) > highestIV || (GetIV(1) == highestIV && rareValue > 10922))
        {
            highestIVIndex = 1;
            //highestIV = IV_ATK;
            highestIV = GetIV(1);
        }
        //if (IV_DEF > highestIV || (IV_DEF == highestIV && rareValue > 21844))
        if (GetIV(2) > highestIV || (GetIV(2) == highestIV && rareValue > 21844))
        {
            highestIVIndex = 2;
            //highestIV = IV_DEF;
            highestIV = GetIV(2);
        }
        //if (IV_SPA > highestIV || (IV_SPA == highestIV && rareValue > 32766))
        if (GetIV(4) > highestIV || (GetIV(4) == highestIV && rareValue > 32766))
        {
            highestIVIndex = 3;
            //highestIV = IV_SPA;
            highestIV = GetIV(4);
        }
        //if (IV_SPD > highestIV || (IV_SPD == highestIV && rareValue > 43688))
        if (GetIV(5) > highestIV || (GetIV(5) == highestIV && rareValue > 43688))
        {
            highestIVIndex = 4;
            //highestIV = IV_SPD;
            highestIV = GetIV(5);
        }
        //if (IV_SPE > highestIV || (IV_SPE == highestIV && rareValue > 54610))
        if (GetIV(3) > highestIV || (GetIV(3) == highestIV && rareValue > 54610))
        {
            highestIVIndex = 5;
            //highestIV = IV_SPE;
            highestIV = GetIV(3);
        }
        return highestIVIndex;
    }

    public string getNature()
    {
        return m_Pokemon.Nature.ToString();
    }

    public int getHP()
    {
        return m_Pokemon.TotalHP;
    }

    public int getCurrentHP() 
    {
        return m_Pokemon.HP;
    }

    public float getPercentHP()
    {
        return 1f - (((float)m_Pokemon.TotalHP - (float)m_Pokemon.HP) / (float)m_Pokemon.TotalHP);
    }

    public int getATK()
    {
        return m_Pokemon.ATK;
    }

    public int getDEF()
    {
        return m_Pokemon.DEF;
    }

    public int getSPA()
    {
        return m_Pokemon.SPA;
    }

    public int getSPD()
    {
        return m_Pokemon.SPD;
    }

    public int getSPE()
    {
        return m_Pokemon.SPE;
    }

    public int getAbility()
    {
        return m_Pokemon.abilityIndex;
    }

    // Sprite ####################################
    public Sprite[] GetFrontAnim_()
    {
        return GetAnimFromID_("PokemonSprites", getID(), gender, getIsShiny());
    }

    public static Sprite[] GetFrontAnimFromID_(int ID, Gender gender, bool isShiny)
    {
        return GetAnimFromID_("PokemonSprites", ID, gender, isShiny);
    }
    // Gender
    private static Sprite[] GetAnimFromID_(string folder, int ID, Gender gender, bool isShiny)
    {
        Sprite[] animation;
        string shiny = (isShiny) ? "s" : "";
        if (gender == Gender.FEMALE) // Female
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

    public static Sprite[] GetIconsFromID_(int ID, bool isShiny)
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

    public float GetCryPitch()
    {
        return (getStatus() == PokemonUnity.Status.FAINT) ? 0.9f : 1f - (0.06f * (1 - getPercentHP()));
    }

    public AudioClip GetCry()
    {
        return GetCryFromID(getID());
    }

    public static AudioClip GetCryFromID(int ID)
    {
        return Resources.Load<AudioClip>("Audio/cry/" + convertLongID(ID));
    }

    public Texture[] GetFrontAnim()
    {
        return GetAnimFromID("PokemonSprites", getID(), gender, getIsShiny());
    }

    public Texture GetIcons()
    {
        return GetIconsFromID(getID(), getIsShiny());
    }

    public Sprite[] GetSprite(bool getLight)
    {
        return GetSpriteFromID(getID(), getIsShiny(), getLight);
    }

    private static Texture[] GetAnimFromID(string folder, int ID, Gender gender, bool isShiny)
    {
        Texture[] animation;
        string shiny = (isShiny) ? "s" : "";
        if (gender == Gender.FEMALE)
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

    public static Texture GetIconsFromID(int ID, bool isShiny)
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

    public static Sprite[] GetSpriteFromID(int ID, bool isShiny, bool getLight)
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