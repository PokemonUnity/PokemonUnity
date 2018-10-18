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
        SaveDataOld.currentSave.respawnScenePosition = new SeriV3(transform.position + respawnPositionOffset);
        SaveDataOld.currentSave.respawnSceneDirection = respawnDirection;
        SaveDataOld.currentSave.respawnText = respawnText;
        SaveDataOld.currentSave.respawnSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        yield return null;
    }
}