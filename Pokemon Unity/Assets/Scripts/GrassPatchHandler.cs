﻿//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class GrassPatchHandler : MonoBehaviour
{
    private GameObject overlay;
    private AudioSource walkSound;
    public AudioClip walkClip;

    /*/    DEBUG
    private static int encounterz = 0; //*/

    void Awake()
    {
        overlay = transform.FindChild("Overlay").gameObject;
        walkSound = transform.GetComponent<AudioSource>();
    }

    void Start()
    {
        overlay.SetActive(false);
    }


    void OnTriggerEnter(Collider other)
    {
        if (!other.name.Contains("_Object") && !other.name.Contains("_Transparent")) return;
        overlay.SetActive(true);

        if (other.transform.parent.name != "Player") return;
        SfxHandler.Play(walkClip, Random.Range(0.85f, 1.1f));
        StartCoroutine(PlayerMovement.player.wildEncounter(WildPokemonInitialiser.Location.Grass));
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("map")) return;
        Collider[] hitColliders =
            Physics.OverlapSphere(
                new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), 0.15f);
        bool deactivate = true;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (!hitColliders[i].name.Contains("Transparent") && !hitColliders[i].name.Contains("Object")) continue;
            deactivate = false;
            i = hitColliders.Length;
        }
        if (deactivate)
        {
            overlay.SetActive(false);
        }
    }
}