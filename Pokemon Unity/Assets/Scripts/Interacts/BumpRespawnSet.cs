//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

namespace PokemonUnity.Legacy
{
	public class BumpRespawnSet : MonoBehaviour
	{
		public Vector3 respawnPositionOffset;
		public int respawnDirection;
		public string[] respawnText;

		private IEnumerator bump()
		{
			SaveData.currentSave.respawnScenePosition = new PokemonUnity.Utility.SeriV3(transform.position + respawnPositionOffset);
			SaveData.currentSave.respawnSceneDirection = respawnDirection;
			SaveData.currentSave.respawnText = respawnText;
			SaveData.currentSave.respawnSceneName = UnityEngine.Application.loadedLevelName;
			yield return null;
		}
	}
}