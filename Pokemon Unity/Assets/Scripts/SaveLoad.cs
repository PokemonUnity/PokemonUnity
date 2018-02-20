//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    public static SaveData[] savedGames = new SaveData[]
    {
        null, null, null
    };


    public static void Save()
    {
        if (SaveData.currentSave != null)
        {
            if (SaveData.currentSave.getFileIndex() >= 0 && SaveData.currentSave.getFileIndex() < savedGames.Length)
            {
                SaveData.currentSave.playerTime += SaveData.currentSave.startTime.Subtract(System.DateTime.UtcNow);
                SaveData.currentSave.lastSave = System.DateTime.UtcNow;// new System.DateTime(,System.DateTimeKind.Utc);
                savedGames[SaveData.currentSave.getFileIndex()] = SaveData.currentSave;
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + "/playerData.pkud");
                bf.Serialize(file, SaveLoad.savedGames);
                file.Close();
            }
        }
    }

    public static bool Load()
    {
        Debug.Log(Application.persistentDataPath);
        if (File.Exists(Application.persistentDataPath + "/playerData.pkud"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData.pkud", FileMode.Open);
            SaveLoad.savedGames = (SaveData[]) bf.Deserialize(file);
            file.Close();
            return true;
        }
        return false;
    }

    public static int getSavedGamesCount()
    {
        int count = 0;
        for (int i = 0; i < savedGames.Length; i++)
        {
            if (savedGames[i] != null)
            {
                count += 1;
            }
        }
        return count;
    }

    public static void resetSaveGame(int index)
    {
        savedGames[index] = null;

        if (index < 2)
        {
            for (int i = index; i < 2; i++)
            {
                SaveLoad.savedGames[i] = SaveLoad.savedGames[i + 1];
                SaveLoad.savedGames[i + 1] = null;
            }
        }

        bool sGN1 = savedGames[0] == null;
        bool sGN2 = savedGames[1] == null;
        bool sGN3 = savedGames[2] == null;

        Debug.Log(sGN1.ToString() + ", " + sGN2.ToString() + ", " + sGN3.ToString());

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerData.pkud");
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
    }
}