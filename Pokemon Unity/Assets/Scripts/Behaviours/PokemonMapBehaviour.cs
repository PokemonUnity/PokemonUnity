//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using System;

[AddComponentMenu("Pokemon Unity/Pokemon Map")]
public class PokemonMapBehaviour : MonoBehaviour
{
    #region Variables

    public PokemonMap Map;
    //public Sprite mapNameBoxTexture;
    public bool ShouldMapNameBoxAppear = true;

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

    #endregion

    #region Events

    public static void UpdateMap(PokemonMapBehaviour map) {
        if (map == null) {
            Debug.LogError("Pokemon Map is null", map.gameObject);
            return;
        }

        AudioTrack mapBackgroundMusic = map.GetBackgroundMusic();

        //if audio is not already playing
        if (BackgroundMusicHandler.Singleton.Clip != mapBackgroundMusic.Clip) {
            //BackgroundMusicHandler.Singleton.Clip = mapBackgroundMusic.Clip;
            BackgroundMusicHandler.Singleton.PlayMain(mapBackgroundMusic);
        }

        if (map.ShouldMapNameBoxAppear)
            MapNameBoxBehaviour.Singleton.AppearAndDisappear(map.Map.MapNameBox);

        map.UpdateWeather();
        //map.UpdateWeather2(); ?

        // FIXME: original function is found in PlayerMovement
        //Update Discord Rich Presence
        //UpdateRPC();
    }

    // FIXME: understand the difference between UpdateWeather() and UpdateWeather2()
    public void UpdateWeather() {
        //Update Weather FIXME
        //if (GameObject.Find("Weather") != null) {
        //    if (newMap.weathers.Length > 0) {
        //        int probabilityTotal = newMap.sunnyWeatherProbability;
        //        int currentProba = 0;
        //        Weather weather = null;

        //        foreach (WeatherProbability w in newMap.weathers) {
        //            probabilityTotal += w.probability;
        //        }

        //        Debug.Log("Probability Total : " + probabilityTotal);

        //        float randValue = UnityEngine.Random.Range(1, probabilityTotal);

        //        Debug.Log("[Weather] Rand value = " + randValue);

        //        if (newMap.sunnyWeatherProbability > 0 && randValue > currentProba && randValue <= currentProba + newMap.sunnyWeatherProbability) {
        //            // Do nothing
        //        } else {
        //            Debug.Log("[Weather] Choosing random weather");
        //            currentProba = newMap.sunnyWeatherProbability;
        //            for (int i = 0; i < newMap.weathers.Length; ++i) {
        //                if (newMap.weathers[i].probability == 0) continue;

        //                if (randValue > currentProba && randValue <= currentProba + newMap.weathers[i].probability) {
        //                    weather = newMap.weathers[i].weather;
        //                    Debug.Log("[Weather] Selected weather : " + weather.name);
        //                    break;
        //                }

        //                currentProba += newMap.weathers[i].probability;
        //            }
        //        }

        //        if (GameObject.Find("Weather") != null) {
        //            GameObject.Find("Weather").GetComponent<WeatherHandler>().setWeatherValue(weather);
        //        }
        //    } else {
        //        Debug.Log("[Weather] Weather List empty");
        //        GameObject.Find("Weather").GetComponent<WeatherHandler>().setWeatherValue(null);
        //    }
        //}
    }

    // FIXME: understand the difference between UpdateWeather() and UpdateWeather2()
    public void UpdateWeather2() {
        //Update Weather
        //if (newMap.weathers.Length > 0) {
        //    int probabilityTotal = newMap.sunnyWeatherProbability;
        //    int currentProba = 0;
        //    Weather weather = null;

        //    foreach (WeatherProbability w in newMap.weathers) {
        //        probabilityTotal += w.probability;
        //    }

        //    Debug.Log("Probability Total : " + probabilityTotal);

        //    float randValue = UnityEngine.Random.Range(1, probabilityTotal);

        //    Debug.Log("[Weather] Rand value = " + randValue);

        //    if (newMap.sunnyWeatherProbability > 0 && randValue > currentProba && randValue <= currentProba + newMap.sunnyWeatherProbability) {
        //        // Do nothing
        //    } else {
        //        Debug.Log("[Weather] Choosing random weather");
        //        currentProba = newMap.sunnyWeatherProbability;
        //        for (int i = 0; i < newMap.weathers.Length; ++i) {
        //            if (newMap.weathers[i].probability == 0) continue;

        //            if (randValue > currentProba && randValue <= currentProba + newMap.weathers[i].probability) {
        //                weather = newMap.weathers[i].weather;
        //                Debug.Log("[Weather] Selected weather : " + weather.name);
        //                break;
        //            }

        //            currentProba += newMap.weathers[i].probability;
        //        }
        //    }

        //    GameObject.Find("Weather").GetComponent<WeatherHandler>().setWeather(weather);
        //} else {
        //    Debug.Log("[Weather] Weather List empty");
        //    GameObject weather = GameObject.Find("Weather");
        //    if (weather != null) weather.GetComponent<WeatherHandler>().setWeather(null);
        //}
    }

    #endregion

    #region Audio

    public AudioTrack GetBackgroundMusic() {
        if (Map.NightBackgroundMusic != null && Map.NightBackgroundMusic.Clip != null) {
            float time = System.DateTime.Now.Hour + ((float)System.DateTime.Now.Minute / 60f);
            if (time >= 20 || time < 3.5f) {
                //night
                return Map.NightBackgroundMusic;
            }
        }
        return Map.DayBackgroundMusic;
    }
    
    #endregion

    #region Battle

    public GameObject getBattleBackground(int currentTag = 0) {
        float time = System.DateTime.Now.Hour + ((float)System.DateTime.Now.Minute / 60f);
        string timeString = "";
        if (time >= 20 || time < 3.5f) {
            //night
            timeString = "Night";
        } else if (time >= 17.5f) {
            //eve
            timeString = "Eve";
        }

        Environment check = (currentTag == 3) ? environment2 : environment;
        string checkString = check.ToString();

        if (currentTag == 2) {
            checkString = "Water";
        }

        //Check for bridge at player						        //0.5f to adjust for stair height
        //cast a ray directly downwards from the player		        //1f to check in line with player's head
        RaycastHit[] hitColliders =
            Physics.RaycastAll(PlayerMovement.Singleton.transform.position + new Vector3(0, 1.5f, 0), Vector3.down);
        BridgeHandler bridge = null;
        //cycle through each of the collisions
        if (hitColliders.Length > 0) {
            //if bridge has not been found yet
            for (int i = 0; i < hitColliders.Length && bridge == null; i++) {
                //if a collision's gameObject has a BridgeHandler, it is a bridge.
                if (hitColliders[i].collider.gameObject.GetComponent<BridgeHandler>() != null) {
                    bridge = hitColliders[i].collider.gameObject.GetComponent<BridgeHandler>();
                    i = hitColliders.Length; //stop iterating after bridge found
                }
            }
        }
        if (bridge != null) {
            check = bridge.bridgeEnvironment;
            checkString = check.ToString();
        }

        //if outdoor environment
        if (check == Environment.Field || check == Environment.Mountain ||
            check == Environment.Forest || check == Environment.Snow) {
            if (currentTag == 2) {
                checkString = "Water";
            }
            //return with timestring attached
            return Resources.Load<GameObject>("Prefabs/BattleBackgrounds/" + checkString);
        }

        //if (currentTag == 2) { checkString = "Water"; } //re-enable this line if you want the Water background to appear in inside environments
        //else return without timestring
        return Resources.Load<GameObject>("Prefabs/BattleBackgrounds/" + checkString);
    }

    public Sprite getBattleBase(int currentTag = 0) {
        float time = System.DateTime.Now.Hour + ((float)System.DateTime.Now.Minute / 60f);
        string timeString = "";
        if (time >= 20 || time < 3.5f) {
            //night
            timeString = "Night";
        } else if (time >= 17.5f) {
            //eve
            timeString = "Eve";
        }

        Environment check = (currentTag == 3) ? environment2 : environment;
        string checkString = check.ToString();

        if (currentTag == 2) {
            checkString = "Water";
        }


        //Check for bridge at player						        //0.5f to adjust for stair height
        //cast a ray directly downwards from the player		        //1f to check in line with player's head
        RaycastHit[] hitColliders =
            Physics.RaycastAll(PlayerMovement.Singleton.transform.position + new Vector3(0, 1.5f, 0), Vector3.down);
        BridgeHandler bridge = null;
        //cycle through each of the collisions
        if (hitColliders.Length > 0) {
            //if bridge has not been found yet
            for (int i = 0; i < hitColliders.Length && bridge == null; i++) {
                //if a collision's gameObject has a BridgeHandler, it is a bridge.
                if (hitColliders[i].collider.gameObject.GetComponent<BridgeHandler>() != null) {
                    bridge = hitColliders[i].collider.gameObject.GetComponent<BridgeHandler>();
                    i = hitColliders.Length; //stop iterating after bridge found
                }
            }
        }
        if (bridge != null) {
            check = bridge.bridgeEnvironment;
            checkString = check.ToString();
        }

        //if outdoor environment
        if (check == Environment.Field || check == Environment.Mountain ||
            check == Environment.Forest || check == Environment.Snow) {
            //return with timestring attached
            return Resources.Load<Sprite>("BattleBackgrounds/Bases/" + checkString + timeString);
        }

        //else return without timestring
        return Resources.Load<Sprite>("BattleBackgrounds/Bases/" + checkString);
    }


    #endregion

    #region Encounters

    // returns the probability of encounter
    public float getEncounterProbability() {
        float x = 1.25f;
        if (pokemonRarity == PokemonRarity.VeryCommon) {
            x = 10f;
        } else if (pokemonRarity == PokemonRarity.Common) {
            x = 8.5f;
        } else if (pokemonRarity == PokemonRarity.Uncommon) {
            x = 6.75f;
        } else if (pokemonRarity == PokemonRarity.Rare) {
            x = 3.33f;
        }
        return x / 187.5f;
    }

    public WildPokemonInitialiser[] getEncounterList(WildPokemonInitialiser.Location location) {
        WildPokemonInitialiser[] list = new WildPokemonInitialiser[encounters.Length];
        int listIndex = 0;

        float time = System.DateTime.Now.Hour + ((float)System.DateTime.Now.Minute / 60f);

        for (int i = 0; i < encounters.Length; i++) {
            if (encounters[i].encounterLocation == location) {
                if (time >= 20 || time < 3.5f) {
                    //night
                    if (encounters[i].encounterNight) {
                        if (WeatherHandler.currentWeather == null) {
                            if (encounters[i].encounterSunny) {
                                list[listIndex] = encounters[i];
                                listIndex += 1;
                            }
                        } else if (encounters[i].encounterWeather) {
                            list[listIndex] = encounters[i];
                            listIndex += 1;
                        }
                    }
                } else if (time < 10f) {
                    //morning
                    if (encounters[i].encounterMorning) {
                        if (WeatherHandler.currentWeather == null) {
                            if (encounters[i].encounterSunny) {
                                list[listIndex] = encounters[i];
                                listIndex += 1;
                            }
                        } else if (encounters[i].encounterWeather) {
                            list[listIndex] = encounters[i];
                            listIndex += 1;
                        }
                    }
                } else {
                    //day
                    if (encounters[i].encounterDay) {
                        if (WeatherHandler.currentWeather == null) {
                            if (encounters[i].encounterSunny) {
                                list[listIndex] = encounters[i];
                                listIndex += 1;
                            }
                        } else if (encounters[i].encounterWeather) {
                            list[listIndex] = encounters[i];
                            listIndex += 1;
                        }
                    }
                }
            }
        }

        WildPokemonInitialiser[] packedList = new WildPokemonInitialiser[listIndex];

        for (int i = 0; i < packedList.Length; i++) {
            packedList[i] = list[i];
        }

        return packedList;
    }

    public PokemonEssentials.Interface.PokeBattle.IPokemon getRandomEncounter(WildPokemonInitialiser.Location location) {
        WildPokemonInitialiser[] list = getEncounterList(location);

        int totalEncounterLikelihood = 0; //add up the total Likelihood
        for (int i = 0; i < list.Length; i++) {
            totalEncounterLikelihood += list[i].encounterLikelihood;
        }

        WildPokemonInitialiser[] chanceSplitList = new WildPokemonInitialiser[totalEncounterLikelihood];
        int listIndex = 0;
        for (int i = 0; i < list.Length; i++) {
            //loop through each position of list
            for (int i2 = 0; i2 < list[i].encounterLikelihood; i2++) {
                //add encounter once for every Likelihood
                chanceSplitList[listIndex] = list[i];
                listIndex += 1;
            }
        }
        //randomly pick a number from the list's length
        int encounterIndex = UnityEngine.Random.Range(0, chanceSplitList.Length);

        //		DEBUG
        //string debugtext = "";
        //for(int i = 0; i < chanceSplitList.Length; i++){
        //    debugtext += PokemonDatabase.getPokemon(chanceSplitList[i].ID).getName() + ", ";}
        //Debug.Log(encounterIndex+": "+debugtext + "("+PokemonDatabase.getPokemon(chanceSplitList[encounterIndex].ID).getName()+")");
        Debug.Log("debug wild battle " + chanceSplitList.Length);

        //return new Pokemon(chanceSplitList[encounterIndex].ID, Pokemon.Gender.CALCULATE,
        //    Random.Range(chanceSplitList[encounterIndex].minLevel, chanceSplitList[encounterIndex].maxLevel + 1),
        //    null, null, null, -1);
        return null;
    }

    #endregion

}

[Serializable]
public class WeatherProbability {
    public Weather weather;
    public int probability;
}

[Serializable]
public class WildPokemonInitialiser {
    public enum Location {
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