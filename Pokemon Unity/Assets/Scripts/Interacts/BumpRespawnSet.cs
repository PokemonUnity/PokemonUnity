//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class BumpRespawnSet : MonoBehaviour
{
    public Vector3 respawnPositionOffset;
    public int respawnDirection;
    public string[] respawnText;

    private IEnumerator bump()
    {
        SaveData.currentSave.respawnScenePosition = new SeriV3(transform.position + respawnPositionOffset);
        SaveData.currentSave.respawnSceneDirection = respawnDirection;
        SaveData.currentSave.respawnText = respawnText;
        SaveData.currentSave.respawnSceneName = Application.loadedLevelName;
        yield return null;
    }
}