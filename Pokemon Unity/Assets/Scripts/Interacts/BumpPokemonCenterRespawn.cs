//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class BumpPokemonCenterRespawn : MonoBehaviour
{
    public InteractPokemonCenter pokemonCenter;

    private IEnumerator bump()
    {
        if (GlobalVariables.global.respawning)
        {
            GlobalVariables.global.respawning = false;
            StartCoroutine(pokemonCenter.respawnHeal());
        }
        yield return null;
    }
}