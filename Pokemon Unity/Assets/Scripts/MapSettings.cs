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
    public Texture mapNameBoxTexture;
    public Color mapNameColor = new Color(0.066f, 0.066f, 0.066f, 1);

    public enum PokemonRarity
    {
        /// <summary>
        /// ~5.33% encounter chance
        /// </summary>
        VeryCommon,
        /// <summary>
        /// ~4.53% encounter chance
        /// </summary>
        Common,
        /// <summary>
        /// ~3.60% encounter chance
        /// </summary>
        Uncommon,
        /// <summary>
        /// ~1.77% encounter chance
        /// </summary>
        Rare,
        /// <summary>
        /// ~0.66% encounter chance
        /// </summary>
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
        CaveDarker
    }

    public Environment environment;
    public Environment environment2;

    public PokemonRarity pokemonRarity = PokemonRarity.Uncommon;

    public WildPokemonInitialiserOld[] encounters = new WildPokemonInitialiserOld[0];

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

	[System.Obsolete("Please refer to any encounter scripts in `/Scripts2/Maps.cs` for alternate solution.")]
    public WildPokemonInitialiserOld[] getEncounterList(WildPokemonInitialiserOld.Method location)
    {
        WildPokemonInitialiserOld[] list = new WildPokemonInitialiserOld[encounters.Length];
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

        WildPokemonInitialiserOld[] packedList = new WildPokemonInitialiserOld[listIndex];

        for (int i = 0; i < packedList.Length; i++)
        {
            packedList[i] = list[i];
        }

        return packedList;
    }

    public Pokemon getRandomEncounter(WildPokemonInitialiser location)
    {
		WildPokemonInitialiser[] list = new WildPokemonInitialiser[0];// getEncounterList(location);

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

#if DEBUG
        //string debugtext = "";
		//for(int i = 0; i < chanceSplitList.Length; i++){
		//    debugtext += PokemonDatabaseOld.getPokemon(chanceSplitList[i].ID).getName() + ", ";}
		//Debug.Log(encounterIndex+": "+debugtext + "("+PokemonDatabaseOld.getPokemon(chanceSplitList[encounterIndex].ID).getName()+")");
#endif

		return new Pokemon();//chanceSplitList[encounterIndex].ID, PokemonOld.Gender.CALCULATE,
            //Random.Range(chanceSplitList[encounterIndex].minLevel, chanceSplitList[encounterIndex].maxLevel + 1),
            //null, null, null, -1);
    }
}

[System.Serializable]
public class WildPokemonInitialiserOld
{
    /// <summary>
    /// Encounter method
    /// </summary>
    public enum Method
    {
        /// <summary>
        /// Walking in tall grass or a cave
        /// </summary>
        WALK = 1,
        /// <summary>
        /// Walking in rustling grass
        /// </summary>
        GRASS_SPOTS = 9,
        /// <summary>
        /// Walking in dust clouds
        /// </summary>
        CAVE_SPOTS = 10,
        /// <summary>
        /// Walking in bridge shadows
        /// </summary>
        BRIDGE_SPOTS = 11,
        /// <summary>
        /// Walking in dark grass
        /// </summary>
        DARK_GRASS = 8,
        /// <summary>
        /// Walking in yellow flowers
        /// </summary>
        YELLOW_FLOWERS = 14,
        /// <summary>
        /// Walking in purple flowers
        /// </summary>
        PURPLE_FLOWERS = 15,
        /// <summary>
        /// Walking in red flowers
        /// </summary>
        RED_FLOWERS = 16,
        /// <summary>
        /// Walking on rough terrain
        /// </summary>
        ROUGH_TERRAIN = 17,
		/// <summary>
		/// Fishing with an <see cref="PokemonUnity.Item.Items.OLD_ROD"/>
		/// </summary>
		OLD_ROD = 2,
		/// <summary>
		/// Fishing with a <see cref="PokemonUnity.Item.Items.GOOD_ROD"/> 
		/// </summary>
		GOOD_ROD = 3,
		/// <summary>
		/// Fishing with a <see cref="PokemonUnity.Item.Items.SUPER_ROD"/> 
		/// </summary>
		SUPER_ROD = 4,
        /// <summary>
        /// Fishing in dark spots
        /// </summary>
        SUPER_ROD_SPOTS = 12,
        /// <summary>
        /// Surfing
        /// </summary>
        SURF = 5,
        /// <summary>
        /// Surfing in dark spots
        /// </summary>
        SURF_SPOTS = 13,
        /// <summary>
        /// Smashing rocks
        /// </summary>
        ROCK_SMASH = 6,
        /// <summary>
        /// Headbutting trees
        /// </summary>
        HEADBUTT = 7
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Condition
    {
        SWARM = 1,
        TIME = 2,
        RADAR = 3,
        SLOT = 4,
        RADIO = 5,
        SEASON = 6
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// default: swarm-no, time-day, radar-off, slot-none, radio-off
    /// </remarks>
    public enum ConditionValue
    {
        /// <summary>
        /// During a swarm
        /// <para>
        /// <seealso cref="Condition.SWARM"/>
        /// </para>
        /// </summary>
        SWARM_YES = 1,
        /// <summary>
        /// Not during a swarm
        /// <para>
        /// <seealso cref="Condition.SWARM"/>
        /// </para>
        /// </summary>
        SWARM_NO = 2,

        /// <summary>
        /// In the morning
        /// <para>
        /// <seealso cref="Condition.TIME"/>
        /// </para>
        /// </summary>
        TIME_MORNING = 3,
        /// <summary>
        /// During the day
        /// <para>
        /// <seealso cref="Condition.TIME"/>
        /// </para>
        /// </summary>
        TIME_DAY = 4,
        /// <summary>
        /// At night
        /// <para>
        /// <seealso cref="Condition.TIME"/>
        /// </para>
        /// </summary>
        TIME_NIGHT = 5,

        /// <summary>
        /// Using PokeRadar
        /// <para>
        /// <seealso cref="Condition.RADAR"/>
        /// </para>
        /// </summary>
        RADAR_ON = 6,
        /// <summary>
        /// Not using PokeRadar
        /// <para>
        /// <seealso cref="Condition.RADAR"/>
        /// </para>
        /// </summary>
        RADAR_OFF = 7,

        /// <summary>
        /// <para>
        /// <seealso cref="Condition.SLOT"/>
        /// </para>
        /// </summary>
        SLOT_NONE = 8,
        /// <summary>
        /// <para>
        /// <seealso cref="Condition.SLOT"/>
        /// </para>
        /// </summary>
        SLOT_RUBY = 9,
        /// <summary>
        /// <para>
        /// <seealso cref="Condition.SLOT"/>
        /// </para>
        /// </summary>
        SLOT_SAPPHIRE = 10,
        /// <summary>
        /// <para>
        /// <seealso cref="Condition.SLOT"/>
        /// </para>
        /// </summary>
        SLOT_EMERALD = 11,
        /// <summary>
        /// <para>
        /// <seealso cref="Condition.SLOT"/>
        /// </para>
        /// </summary>
        SLOT_FIRERED = 12,
        /// <summary>
        /// <para>
        /// <seealso cref="Condition.SLOT"/>
        /// </para>
        /// </summary>
        SLOT_LEAFGREEN = 13,

        /// <summary>
        /// Radio off
        /// <para>
        /// <seealso cref="Condition.RADIO"/>
        /// </para>
        /// </summary>
        RADIO_OFF = 14,
        /// <summary>
        /// Hoenn radio
        /// <para>
        /// <seealso cref="Condition.RADIO"/>
        /// </para>
        /// </summary>
        RADIO_HOENN = 15,
        /// <summary>
        /// Sinnoh radio
        /// <para>
        /// <seealso cref="Condition.RADIO"/>
        /// </para>
        /// </summary>
        RADIO_SINNOH = 16,

        /// <summary>
        /// During Spring
        /// <para>
        /// <seealso cref="Condition.SEASON"/>
        /// </para>
        /// </summary>
        SEASON_SPRING = 17,
        /// <summary>
        /// During Summer
        /// <para>
        /// <seealso cref="Condition.SEASON"/>
        /// </para>
        /// </summary>
        SEASON_SUMMER = 18,
        /// <summary>
        /// During Autumn
        /// <para>
        /// <seealso cref="Condition.SEASON"/>
        /// </para>
        /// </summary>
        SEASON_AUTUMN = 19,
        /// <summary>
        /// During Winter
        /// <para>
        /// <seealso cref="Condition.SEASON"/>
        /// </para>
        /// </summary>
        SEASON_WINTER = 20
    }

    public ConditionValue[] ConditionArray /*= new ConditionValue[] {
        ConditionValue.SWARM_NO,
        ConditionValue.TIME_DAY,
        ConditionValue.RADAR_OFF,
        ConditionValue.SLOT_NONE,
        ConditionValue.RADIO_OFF
    }*/;

    public int ID;
    public int minLevel;
    public int maxLevel;

    public int encounterLikelihood;

    public Method encounterLocation;

    public bool encounterMorning = true;
    public bool encounterDay = true;
    public bool encounterNight = true;
}