//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
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
        if (SaveData.currentSave.IsNotNullFile())
        {
            if (SaveData.currentSave.getFileIndex() >= 0 && SaveData.currentSave.getFileIndex() < savedGames.Length)
            {
                SaveData.currentSave.Save();
                savedGames[SaveData.currentSave.getFileIndex()] = SaveData.currentSave;
                BinaryFormatter bf = new BinaryFormatter();
                if (File.Exists(Application.persistentDataPath + "/playerData.pkud"))
                    File.Delete(Application.persistentDataPath + "/playerData.pkud");

                FileStream file = File.Create(Application.persistentDataPath + "/playerData.pkud");
                SaveData.SaveFile[] savedFiles = new SaveData.SaveFile[savedGames.Length];
                for (int i = 0; i < savedGames.Length; i++)
                {
                    if (savedGames[i] != null)
                    {
                        savedGames[i].Save();
                        savedFiles[i] = savedGames[i].savefile;
                    }
                }
                bf.Serialize(file, savedFiles);
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
            SaveData.SaveFile[] savedFiles = (SaveData.SaveFile[])bf.Deserialize(file);
            for (int i = 0; i < savedFiles.Length; i++)
            {
                if (savedFiles[i] != null)
                    savedGames[i] = new SaveData(savedFiles[i]);
            }
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