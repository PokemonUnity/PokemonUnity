//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class BumpPokemonCenterRespawn : MonoBehaviour
{
    public InteractPokemonCenter pokemonCenter;

    private IEnumerator bump()
    {
        if (GlobalVariables.Singleton.respawning)
        {
            GlobalVariables.Singleton.respawning = false;
            StartCoroutine(pokemonCenter.respawnHeal());
        }
        yield return null;
    }
}