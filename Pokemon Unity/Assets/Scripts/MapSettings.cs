//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class MapSettings : MonoBehaviour
{
    public AudioClip mapBGMClip;
    public int mapBGMLoopStartSamples = 0;
    public AudioClip mapBGMNightClip = null;
    public int mapBGMNightLoopStartSamples = 0;
    public string mapName;
    public string discordImageKey;
    public string discordDetails;
    public Texture mapNameBoxTexture;
    public Color mapNameColor = new Color(0.066f, 0.066f, 0.066f, 1);

    public enum PokemonRarity
    {
        VeryCommon, // ~5.33% encounter chance
        Common,     // ~4.53% encounter chance
        Uncommon,   // ~3.60% encounter chance
        Rare,       // ~1.77% encounter chance
        VeryRare    // ~0.66% encounter chance
    }

    public enum Environment
    {
        Field,
        Mountain,
        Forest,
        Snow,
        IndoorA,
        IndoorB,
        IndoorC,
        Cave,
        CaveDark,
        CaveDarker
    }

    public Environment environment;
    public Environment environment2;

    public WildPokemonInitialiser[] encounters = new WildPokemonInitialiser[0];

    // returns the BGM to an external script
    public AudioClip getBGM()
    {
        if (mapBGMNightClip != null)
        {
            float time = System.DateTime.Now.Hour + ((float) System.DateTime.Now.Minute / 60f);
            if (time >= 20 || time < 3.5f)
            {
                //night
                return mapBGMNightClip;
            }
        }
        return mapBGMClip;
    }

    public int getBGMLoopStartSamples()
    {
        if (mapBGMNightClip != null)
        {
            float time = System.DateTime.Now.Hour + ((float) System.DateTime.Now.Minute / 60f);
            if (time >= 20 || time < 3.5f)
            {
                //night
                return mapBGMNightLoopStartSamples;
            }
        }
        return mapBGMLoopStartSamples;
    }

    public Sprite getBattleBackground()
    {
        return getBattleBackground(0);
    }

    public Sprite getBattleBackground(int currentTag)
    {
        float time = System.DateTime.Now.Hour + ((float) System.DateTime.Now.Minute / 60f);
        string timeString = "";
        if (time >= 20 || time < 3.5f)
        {
            //night
            timeString = "Night";
        }
        else if (time >= 17.5f)
        {
            //eve
            timeString = "Eve";
        }

        Environment check = (currentTag == 3) ? environment2 : environment;
        string checkString = check.ToString();

        if (currentTag == 2)
        {
            checkString = "Water";
        }

        //Check for bridge at player						        //0.5f to adjust for stair height
        //cast a ray directly downwards from the player		        //1f to check in line with player's head
        RaycastHit[] hitColliders =
            Physics.RaycastAll(PlayerMovement.player.transform.position + new Vector3(0, 1.5f, 0), Vector3.down);
        BridgeHandler bridge = null;
        //cycle through each of the collisions
        if (hitColliders.Length > 0)
        {
            //if bridge has not been found yet
            for (int i = 0; i < hitColliders.Length && bridge == null; i++)
            {
                //if a collision's gameObject has a BridgeHandler, it is a bridge.
                if (hitColliders[i].collider.gameObject.GetComponent<BridgeHandler>() != null)
                {
                    bridge = hitColliders[i].collider.gameObject.GetComponent<BridgeHandler>();
                    i = hitColliders.Length; //stop iterating after bridge found
                }
            }
        }
        if (bridge != null)
        {
            check = bridge.bridgeEnvironment;
            checkString = check.ToString();
        }

        //if outdoor environment
        if (check == Environment.Field || check == Environment.Mountain ||
            check == Environment.Forest || check == Environment.Snow)
        {
            if (currentTag == 2)
            {
                checkString = "Water";
            }
            //return with timestring attached
            return Resources.Load<Sprite>("BattleBackgrounds/Backgrounds/" + checkString + timeString);
        }

        //if (currentTag == 2) { checkString = "Water"; } //re-enable this line if you want the Water background to appear in inside environments
        //else return without timestring
        return Resources.Load<Sprite>("BattleBackgrounds/Backgrounds/" + checkString);
    }

    public Sprite getBattleBase()
    {
        return getBattleBase(0);
    }

    public Sprite getBattleBase(int currentTag)
    {
        float time = System.DateTime.Now.Hour + ((float) System.DateTime.Now.Minute / 60f);
        string timeString = "";
        if (time >= 20 || time < 3.5f)
        {
            //night
            timeString = "Night";
        }
        else if (time >= 17.5f)
        {
            //eve
            timeString = "Eve";
        }

        Environment check = (currentTag == 3) ? environment2 : environment; //If Tag=3, check = env2; else check=env
        string checkString = check.ToString();

        if (currentTag == 2)
        {
            checkString = "Water";
        }


        //Check for bridge at player						        //0.5f to adjust for stair height
        //cast a ray directly downwards from the player		        //1f to check in line with player's head
        RaycastHit[] hitColliders =
            Physics.RaycastAll(PlayerMovement.player.transform.position + new Vector3(0, 1.5f, 0), Vector3.down);
        BridgeHandler bridge = null;
        //cycle through each of the collisions
        if (hitColliders.Length > 0)
        {
            //if bridge has not been found yet
            for (int i = 0; i < hitColliders.Length && bridge == null; i++)
            {
                //if a collision's gameObject has a BridgeHandler, it is a bridge.
                if (hitColliders[i].collider.gameObject.GetComponent<BridgeHandler>() != null)
                {
                    bridge = hitColliders[i].collider.gameObject.GetComponent<BridgeHandler>();
                    i = hitColliders.Length; //stop iterating after bridge found
                }
            }
        }
        if (bridge != null)
        {
            check = bridge.bridgeEnvironment;
            checkString = check.ToString();
        }

        //if outdoor environment
        if (check == Environment.Field || check == Environment.Mountain ||
            check == Environment.Forest || check == Environment.Snow)
        {
            //return with timestring attached
            return Resources.Load<Sprite>("BattleBackgrounds/Bases/" + checkString + timeString);
        }

        //else return without timestring
        return Resources.Load<Sprite>("BattleBackgrounds/Bases/" + checkString);
    }

    // returns the probability of encounter for individual Pokemon
    public float getEncounterProbability(PokemonRarity pokemonRarity)
    {
        float x = 1.25f;       // Default value if none specified
        switch (pokemonRarity)
        {
            case PokemonRarity.VeryCommon:
                x = 10f;    
                break;
            case PokemonRarity.Common:
                x = 8.5f;
                break;
            case PokemonRarity.Uncommon:
                x = 6.75f;
                break;
            case PokemonRarity.Rare:
                x = 3.33f;
                break;
            case PokemonRarity.VeryRare:
                x = 1.25f;
                break;
            default:
                break;
        }
        return x / 187.5f;
    }

    public WildPokemonInitialiser[] getEncounterList(WildPokemonInitialiser.Location location)
    {
        WildPokemonInitialiser[] list = new WildPokemonInitialiser[encounters.Length];
        int listIndex = 0;

        float time = System.DateTime.Now.Hour + ((float) System.DateTime.Now.Minute / 60f);

        for (int i = 0; i < encounters.Length; i++)
        {   //If Pokemon is meant to be found in current location, create list based on time of day
            if (encounters[i].encounterLocation == location)    
            {
                if (time >= 20 || time < 3.5f)
                {
                    //night
                    if (encounters[i].encounterNight)
                    {
                        list[listIndex] = encounters[i];
                        listIndex += 1;
                    }
                }
                else if (time < 10f)
                {
                    //morning
                    if (encounters[i].encounterMorning)
                    {
                        list[listIndex] = encounters[i];
                        listIndex += 1;
                    }
                }
                else
                {
                    //day
                    if (encounters[i].encounterDay)
                    {
                        list[listIndex] = encounters[i];
                        listIndex += 1;
                    }
                }
            }
        }

        WildPokemonInitialiser[] packedList = new WildPokemonInitialiser[listIndex];

        for (int i = 0; i < packedList.Length; i++)
        {
            packedList[i] = list[i];
        }

        return packedList;
    }

    public float getTotalEncounterChance(WildPokemonInitialiser.Location location)
    {
        WildPokemonInitialiser[] list = getEncounterList(location);

        float totalEncounterLikelihood = 0f; //add up the total Likelihood
        for (int i = 0; i < list.Length; i++)
        {
            totalEncounterLikelihood += getEncounterProbability(list[i].pokemonRarity);
        }
        return totalEncounterLikelihood;
    }

    // After successful encounter, determine who shows up
    public Pokemon getRandomEncounter(WildPokemonInitialiser.Location location)
    {
        WildPokemonInitialiser[] list = getEncounterList(location);
        float totalEncounterLikelihood = getTotalEncounterChance(location);
        float randomEncounterNumber = Random.Range(0, totalEncounterLikelihood);

        float[] splitChancesList = new float[list.Length];
        int encounterNumber = 0;
        
        float runningTally = 0.0f;
        for (int i = 0; i < list.Length; i++) 
        {
            // Assign each Pokemon encounter chance a section
            runningTally += getEncounterProbability(list[i].pokemonRarity);
            splitChancesList[i] += runningTally;   
        }   // An array with probability boundaries is made ex: {.3,.7} means 0->.3 = Pokemon One on the list.
        for (int i = 0; i < list.Length; i++) //Compare random number to array
        {
            if (splitChancesList[i] >= randomEncounterNumber)
            {
                encounterNumber = i;
                break;
            }
        }

        /*		DEBUG
        Debug.Log("Total chance: " + totalEncounterLikelihood.ToString() + " Random: " + randomEncounterNumber.ToString());
        for (int i=0; i < list.Length; i++)
        {
            Debug.Log("Pokemon ID in List: " +list[i].ID.ToString());
        }
        //
            string debugtext = "";
            for(int i = 0; i < chanceSplitList.Length; i++){
                debugtext += PokemonDatabase.getPokemon(chanceSplitList[i].ID).getName() + ", ";}
            Debug.Log(encounterIndex+": "+debugtext + "("+PokemonDatabase.getPokemon(chanceSplitList[encounterIndex].ID).getName()+")");
            //*/

        return new Pokemon(list[encounterNumber].ID, Pokemon.Gender.CALCULATE,
            Random.Range(list[encounterNumber].minLevel, list[encounterNumber].maxLevel + 1),
            null, null, null, -1);
        
    }
}


[System.Serializable]
public class WildPokemonInitialiser
{
    public enum Location
    {
        Grass,
        Standard,
        Surfing,
        OldRod,
        GoodRod,
        SuperRod
    }

    public int ID;
    public int minLevel;
    public int maxLevel;

    //public int encounterLikelihood;
    public MapSettings.PokemonRarity pokemonRarity = MapSettings.PokemonRarity.Uncommon;

    public Location encounterLocation;

    public bool encounterMorning = true;
    public bool encounterDay = true;
    public bool encounterNight = true;
}