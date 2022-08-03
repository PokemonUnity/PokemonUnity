//Thanks to Omnicron for the reference to
// https://pastebin.com/YY0XKMCQ
//For BinarryFormatting any System.Serializable into a custom file

using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using PokemonUnity;
using PokemonUnity.Localization;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;
using PokemonUnity.Utility;
using PokemonEssentials;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using UnityEngine.SceneManagement;

public static class GlobalSaveManager
{
    private const string BuildVersion = "0.0.1";
    //If UseAppdata = true, Pokemon Unity will save the save files into %AppData%/Roaming/Pokemon Unity/Saves
    //If UseAppdata = false, Pokemon Unity will save the save files into Assets/Saves
    private const bool UseAppdate = false;
    private static string saveLocation = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\Pokemon Unity\Saves\";

    private static UnityEngine.GameObject Player;
    private static List<CustomSaveEvent> EventSaves = new List<CustomSaveEvent>();

    private static EventListener EventListener = new EventListener();

    public static void RegisterPlayer(UnityEngine.GameObject player)
    {
        Player = player;
        UnityEngine.Debug.Log("Registered Player.");
    }

    /// <summary>
    /// Registers an event into the EventSaves List<>
    /// </summary>
    /// <param name="customEvent">The event that needs to be registered</param>
    public static void RegisterEvent(CustomSaveEvent customEvent)
    {
        EventSaves.Add(customEvent);
        EventSaves = EventSaves.OrderBy(x => x.EventTime).ToList();
        UnityEngine.Debug.Log(customEvent.ToString());
    }

    /// <summary>
    /// Get's the relevant CustomSaveEvents for the current Scene.
    /// </summary>
    /// <param name="sceneIndex">The scene index that the Player is currently on.</param>
    /// <returns></returns>
    public static List<CustomSaveEvent> GetRelaventSaveData(int sceneIndex)
    {
        return EventSaves.Where(x => x.SceneIndex == sceneIndex).ToList();
    }

    /// <summary>
    /// Saves the game into the computer's %AppData% folder.
    /// </summary>
    public static void Save()
    {
        if (!UseAppdate)
        {
            saveLocation = UnityEngine.Application.dataPath + "/Saves/";
        }

        //Pokemon[][] Party = SaveData.currentSave.PC.boxes;
        Bag PlayerBag = SaveData.currentSave.Bag;

        CustomSaveData DataToSave = new CustomSaveData(
			(SeriV3)Player.transform.position, Player.GetComponent<PlayerMovement>().direction,
            (SeriV3)Player.transform.Find("Follower").transform.position, Player.GetComponentInChildren<FollowerMovement>().direction,
            SceneManager.GetActiveScene().buildIndex, //Party, 
                PlayerBag, EventSaves, "");

        BinaryFormatter bf = new BinaryFormatter();
        try
        {
            int saveAmount = Directory.GetFiles(saveLocation, "*pku", SearchOption.TopDirectoryOnly).Length;

            if (saveAmount < 0)
                saveAmount = 0;

            FileStream file = File.Open(saveLocation + @"Save" + saveAmount.ToString() + ".pku", FileMode.OpenOrCreate, FileAccess.Write);
            bf.Serialize(file, DataToSave);
            file.Close();
            UnityEngine.Debug.Log("Save file created.");
        }
        catch(Exception)
        {
            UnityEngine.Debug.Log("Pokemon Unity save directory does not exist, creating new one...");
            Directory.CreateDirectory(saveLocation.Substring(0, saveLocation.Length -1));
            UnityEngine.Debug.Log("Trying to save again...");

            FileStream file = File.Open(saveLocation + @"Save" + (Directory.GetFiles(saveLocation, "*pku", SearchOption.TopDirectoryOnly).Length).ToString() + ".pku", FileMode.OpenOrCreate, FileAccess.Write);
            bf.Serialize(file, DataToSave);
            file.Close();

            UnityEngine.Debug.Log("Save file created.");
        }
    }

    /// <summary>
    /// Loads the Save file that corresponeds with the saveInex.
    /// </summary>
    /// <param name="saveIndex">The index of the save (starting from 0, FE: "Save0.pku")</param>
    public static void Load(int saveIndex)
    {
        if (!UseAppdate)
        {
            saveLocation = UnityEngine.Application.dataPath + "/Saves/";
            if(!Directory.Exists(saveLocation.Substring(0, saveLocation.Length - 1)))
            {
                Directory.CreateDirectory(saveLocation.Substring(0, saveLocation.Length - 1));
            }
        }

        BinaryFormatter bf = new BinaryFormatter();
        try
        {
            FileStream file = File.Open(saveLocation + "Save" + saveIndex.ToString() + ".pku", FileMode.Open, FileAccess.Read);
            CustomSaveData DataToLoad = (CustomSaveData)bf.Deserialize(file);

            if (null != DataToLoad)
            {

                //EventSaves contains all the Events that the Player has encountered
                EventSaves = DataToLoad.SaveData;

                if (SaveData.currentSave == null)
                {
                    SaveData.currentSave = new SaveData(-1);
                }

                if (SceneManager.GetActiveScene().buildIndex != DataToLoad.ActiveScene)
                {
                    SceneManager.LoadScene(DataToLoad.ActiveScene);
                }
                else
                {
                    //Loads the Trainer's Party into the CurrentSave
                    //SaveData.currentSave.PC.boxes = DataToLoad.Party;
                    //Loads the Bag (containing the Items that the player owns) into the CurrentSave
                    SaveData.currentSave.Bag = DataToLoad.PlayerBag;

                    //Loading Player
                    UnityEngine.GameObject Player = UnityEngine.GameObject.FindGameObjectWithTag("Player");
                    Player.transform.position = DataToLoad.PlayerPosition;
                    Player.GetComponent<PlayerMovement>().direction = DataToLoad.PlayerDirection;
                    //Loading Follower
                    UnityEngine.GameObject Follower = Player.transform.Find("Follower").gameObject;
                    Follower.transform.position = DataToLoad.FollowerPosition;
                    Follower.GetComponent<FollowerMovement>().direction = DataToLoad.FollowerDirection;

                    EventSaves = EventSaves.OrderBy(x => x.EventTime).ToList();
                }
            }

            file.Dispose();
        }
        catch(FileNotFoundException)
        {
            UnityEngine.Debug.Log("Couldn't find \"Save" + saveIndex + ".pku\".");
        }
    }

    /// <summary>
    /// Gets the amount of save files that's specified in Amount.
    /// </summary>
    /// <param name="Amount">The amount of save files that needs to be loaded (0 for all the save files).</param>
    /// <returns>A List containing the relevant amount of save files.</returns>
    static List<CustomSaveData> GetSaves(int Amount)
    {
        List<CustomSaveData> saveFiles = new List<CustomSaveData>();
        foreach (string file in Directory.GetFiles(saveLocation))
        {
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                if (Path.GetExtension(file) == "pku")
                {
                    using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        try
                        {
                            CustomSaveData saveData = (CustomSaveData)bf.Deserialize(fileStream);
                            if (saveData.BuildVersion == BuildVersion)
                            {
                                saveFiles.Add(saveData);
                            }
                            else
                            {
                                //Try to convert the created file into the current version
                            }
                        }
                        catch (Exception e)
                        {
                            UnityEngine.Debug.Log(e.ToString());
                        }
                    }
                }
            }
            catch (IOException e)
            {
                throw e;
            }
        }

        saveFiles = saveFiles.OrderBy(x => x.TimeCreated).ToList();
        if (Amount == 0 || saveFiles.Count < Amount)
        {
            return saveFiles;
        }
        else
        {
            return saveFiles.Take(Amount).ToList();
        }
    }

    [System.Serializable]
    private class CustomSaveData
    {
        public string BuildVersion = GlobalSaveManager.BuildVersion;
        public string SaveName = string.Empty;

        public DateTime TimeCreated;

        //Player
        public SeriV3 PlayerPosition;
        public int PlayerDirection;
        //Follower
        public SeriV3 FollowerPosition;
        public int FollowerDirection;

        public int ActiveScene;

        /// <summary>
        /// When the Player blacks out they need to return to the last visited Pokemon Center
        /// </summary>
        private int pCenterScene;
        private SeriV3 pCenterPosition;
        private int pCenterDirection;

        private SeriV3 pCenterFposition;
        private int pCenterFdirection;

        //public Pokemon[][] Party;
        public Bag PlayerBag;

        public List<CustomSaveEvent> SaveData;

        public void AddPokemonCenter(int scene, SeriV3 position, int direction, SeriV3 fPosition, int fDirection)
        {
            pCenterScene = scene;

            pCenterPosition = position;
            pCenterDirection = direction;

            pCenterFposition = fPosition;
            pCenterFdirection = fDirection;
        }

        public CustomSaveData(
            SeriV3 playerPosition, int playerRotation, 
            SeriV3 followerPosition, int followerDirection,
            int activeScene,
            //Pokemon[][] party, 
            Bag playerBag, 
            List<CustomSaveEvent> saveData,
            string saveName)
        {
            PlayerPosition = playerPosition;
            PlayerDirection = playerRotation;

            FollowerPosition = followerPosition;
            FollowerDirection = followerDirection;

            ActiveScene = activeScene;

            //Party = party;
            PlayerBag = playerBag;

            SaveData = saveData;
            SaveName = saveName;

            TimeCreated = DateTime.Now;
        }
    }
}

[System.Serializable]
public enum SaveEventType
{
    ITEM,
    BATTLE,
    INTERACTION,
    UNKNOWN
}
