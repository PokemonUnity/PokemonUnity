//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class MapSettings : MonoBehaviour
{
    public string RPCImageKey;
    public AudioClip mapBGMClip;
    public int mapBGMLoopStartSamples = 0;
    public AudioClip mapBGMNightClip = null;
    public int mapBGMNightLoopStartSamples = 0;
    public string mapName;
    public Sprite mapNameBoxTexture;
    public bool mapNameAppears = true;
    public Color mapNameColor = new Color(0.066f, 0.066f, 0.066f, 1);

    public enum PokemonRarity
    {
        VeryCommon,
        Common,
        Uncommon,
        Rare,
        VeryRare
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
        CaveDarker,
        Grass,
        FieldAut
    }

    public Environment environment;
    public Environment environment2;

    public PokemonRarity pokemonRarity = PokemonRarity.Uncommon;

    public WildPokemonInitialiser[] encounters = new WildPokemonInitialiser[0];

    public int sunnyWeatherProbability = 1;
    public WeatherProbability[] weathers;

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

    public GameObject getBattleBackground(int currentTag = 0)
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
            return Resources.Load<GameObject>("Prefabs/BattleBackgrounds/" + checkString);
        }

        //if (currentTag == 2) { checkString = "Water"; } //re-enable this line if you want the Water background to appear in inside environments
        //else return without timestring
        return Resources.Load<GameObject>("Prefabs/BattleBackgrounds/" + checkString);
    }

    public Sprite getBattleBase(int currentTag = 0)
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
            //return with timestring attached
            return Resources.Load<Sprite>("BattleBackgrounds/Bases/" + checkString + timeString);
        }

        //else return without timestring
        return Resources.Load<Sprite>("BattleBackgrounds/Bases/" + checkString);
    }

    // returns the probability of encounter
    public float getEncounterProbability()
    {
        float x = 1.25f;
        if (pokemonRarity == PokemonRarity.VeryCommon)
        {
            x = 10f;
        }
        else if (pokemonRarity == PokemonRarity.Common)
        {
            x = 8.5f;
        }
        else if (pokemonRarity == PokemonRarity.Uncommon)
        {
            x = 6.75f;
        }
        else if (pokemonRarity == PokemonRarity.Rare)
        {
            x = 3.33f;
        }
        return x / 187.5f;
    }

    public WildPokemonInitialiser[] getEncounterList(WildPokemonInitialiser.Location location)
    {
        WildPokemonInitialiser[] list = new WildPokemonInitialiser[encounters.Length];
        int listIndex = 0;

        float time = System.DateTime.Now.Hour + ((float) System.DateTime.Now.Minute / 60f);

        for (int i = 0; i < encounters.Length; i++)
        {
            if (encounters[i].encounterLocation == location)
            {
                if (time >= 20 || time < 3.5f)
                {
                    //night
                    if (encounters[i].encounterNight)
                    {
                        if (WeatherHandler.currentWeather == null)
                        {
                            if (encounters[i].encounterSunny)
                            {
                                list[listIndex] = encounters[i];
                                listIndex += 1;
                            }
                        }
                        else if (encounters[i].encounterWeather)
                        {
                            list[listIndex] = encounters[i];
                            listIndex += 1;
                        }
                    }
                }
                else if (time < 10f)
                {
                    //morning
                    if (encounters[i].encounterMorning)
                    {
                        if (WeatherHandler.currentWeather == null)
                        {
                            if (encounters[i].encounterSunny)
                            {
                                list[listIndex] = encounters[i];
                                listIndex += 1;
                            }
                        }
                        else if (encounters[i].encounterWeather)
                        {
                            list[listIndex] = encounters[i];
                            listIndex += 1;
                        }
                    }
                }
                else
                {
                    //day
                    if (encounters[i].encounterDay)
                    {
                        if (WeatherHandler.currentWeather == null)
                        {
                            if (encounters[i].encounterSunny)
                            {
                                list[listIndex] = encounters[i];
                                listIndex += 1;
                            }
                        }
                        else if (encounters[i].encounterWeather)
                        {
                            list[listIndex] = encounters[i];
                            listIndex += 1;
                        }
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

    public Pokemon getRandomEncounter(WildPokemonInitialiser.Location location)
    {
        WildPokemonInitialiser[] list = getEncounterList(location);

        int totalEncounterLikelihood = 0; //add up the total Likelihood
        for (int i = 0; i < list.Length; i++)
        {
            totalEncounterLikelihood += list[i].encounterLikelihood;
        }

        WildPokemonInitialiser[] chanceSplitList = new WildPokemonInitialiser[totalEncounterLikelihood];
        int listIndex = 0;
        for (int i = 0; i < list.Length; i++)
        {
            //loop through each position of list
            for (int i2 = 0; i2 < list[i].encounterLikelihood; i2++)
            {
                //add encounter once for every Likelihood
                chanceSplitList[listIndex] = list[i];
                listIndex += 1;
            }
        }
        //randomly pick a number from the list's length
        int encounterIndex = Random.Range(0, chanceSplitList.Length);

        /*/		DEBUG
            string debugtext = "";
            for(int i = 0; i < chanceSplitList.Length; i++){
                debugtext += PokemonDatabase.getPokemon(chanceSplitList[i].ID).getName() + ", ";}
            Debug.Log(encounterIndex+": "+debugtext + "("+PokemonDatabase.getPokemon(chanceSplitList[encounterIndex].ID).getName()+")");
            //*/
        Debug.Log("debug wild battle "+chanceSplitList.Length);

        return new Pokemon(chanceSplitList[encounterIndex].ID, Pokemon.Gender.CALCULATE,
            Random.Range(chanceSplitList[encounterIndex].minLevel, chanceSplitList[encounterIndex].maxLevel + 1),
            null, null, null, -1);
    }
}

[System.Serializable]
public class WeatherProbability
{
    public Weather weather;
    public int probability;
}

[System.Serializable]
public class WildPokemonInitialiser
{
    public enum Location
    {
        Grass,
        Grass2,
        Grass3,
        Grass4,
        Grass5,
        Standard,
        Surfing,
        OldRod,
        GoodRod,
        SuperRod
    }

    public int ID;
    public int minLevel;
    public int maxLevel;

    public int encounterLikelihood;

    public Location encounterLocation;

    public bool encounterMorning = true;
    public bool encounterDay = true;
    public bool encounterNight = true;
    public bool encounterSunny = true;
    public bool encounterWeather = true;
}