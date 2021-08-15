//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class BumpRespawnSet : MonoBehaviour
{
    public Vector3 respawnPositionOffset;
    public int respawnDirection;
    public string respawnText;

    private IEnumerator bump()
    {
        SaveData.currentSave.savefile.respawnScenePosition = new SeriV3(transform.position + respawnPositionOffset);
        SaveData.currentSave.savefile.respawnSceneDirection = respawnDirection;
        SaveData.currentSave.savefile.respawnText = respawnText;
        SaveData.currentSave.savefile.respawnSceneName = Application.loadedLevelName;
        yield return null;
    }
}