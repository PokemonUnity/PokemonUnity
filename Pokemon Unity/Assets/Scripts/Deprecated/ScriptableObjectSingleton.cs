//using System;
//using System.IO;
//using UnityEditor;
//using UnityEngine;

//Copy / Mimic this class in another ScriptableObject class to make it a singleton
// Change 'ScriptableObjectSingleton' type to whatever type the other class is
//public class ScriptableObjectSingleton : ScriptableObject {
//    #region Singleton

//    static ScriptableObjectSingleton singleton;

//    public static string GUID() => "246726d7819bc454386e461dba4af80c"; // This needs to be updated when GUID's change

//    public static string SingletonPath() => AssetDatabase.GUIDToAssetPath(GUID());

//    public static ScriptableObjectSingleton LoadSingleton() {
//        string path = SingletonPath();
//        int resourceFolderIndex = path.IndexOf("Resources");
//        if (resourceFolderIndex == -1) throw new NotInResourcesFolderError(path);
//        resourceFolderIndex = resourceFolderIndex + "Resources".Length + 1;
//        string resourcePath = path.Substring(resourceFolderIndex, path.Length - resourceFolderIndex);
//        resourcePath = Path.GetFileNameWithoutExtension(resourcePath);
//        ScriptableObjectSingleton gameSettings = Resources.Load<ScriptableObjectSingleton>(resourcePath);
//        return gameSettings;
//        //if (gameSettings == null) Debug.LogError("Failed to load ScriptableObject for some unknown reason");
//        //return Instantiate(gameSettings);
//    }

//    class NotInResourcesFolderError : Exception {
//        public NotInResourcesFolderError(string path) {
//            Debug.LogError("ScriptableObject is not in a Resources folder. Cannot automatically create a Singleton. Path: " + path);
//        }
//    }

//    public static ScriptableObjectSingleton Singleton {
//        get {
//            if (singleton == null) {
//                singleton = LoadSingleton();
//                Debug.Log(singleton.name + " loaded");
//                Initialize(singleton);
//            }
//            return singleton;
//        }
//        private set { singleton = value; }
//    }

//    static void Initialize(ScriptableObjectSingleton scriptableObject) {
//        // implement
//    }

//    void Awake() {
//        if (Singleton != null) Destroy(this);
//    }

//    #endregion
//}
