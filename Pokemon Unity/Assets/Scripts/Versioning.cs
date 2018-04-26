using UnityEngine;

[System.Serializable]
public class Versioning
{
    public string version;
    public int branch;

    /*public IEnumerator GetVersion(TextAsset versioningJSON)
    {
        return "v"+JsonUtility.FromJson<Versioning>(versioningJSON.ToString()).version
            +JsonUtility.FromJson<Versioning>(versioningJSON.ToString()).branch;

    }*/
}