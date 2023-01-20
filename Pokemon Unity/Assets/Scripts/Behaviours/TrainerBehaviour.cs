//Original Scripts by IIColour (IIColour_Spectrum)

using System.Collections;
using PokemonUnity;
using PokemonUnity.Localization;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;
using PokemonUnity.Utility;
using PokemonEssentials;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using MarkupAttributes;

[RequireComponent(typeof(SpriteAnimatorBehaviour))]
public class TrainerBehaviour : MonoBehaviour, INeedDirection
{
    public TrainerSO Trainer;
    public float SightDistance;
    [SerializeField] DirectionSurrogate directionSurrogate;
    SpriteAnimatorBehaviour animator;

    #region Old variables

    [Foldout("Deprecated")]

    [Header("Trainer Settings")]
    
    public PokemonInitialiser[] trainerParty = new PokemonInitialiser[1];
    private IPokemon[] party;

    [Header("Music")]
    
    public AudioClip battleBGM;
    public int samplesLoopStart;

    public AudioClip victoryBGM;
    public int victorySamplesLoopStart;

    public AudioClip lowHpBGM;
    public int lowHpBGMSamplesLoopStart;

    [Header("Environment")]
    public PokemonMapBehaviour.Environment environment;

    [Header("English Dialogs")]
    [FormerlySerializedAs("tightSpotDialog")]
    public string[] en_tightSpotDialog;

    [FormerlySerializedAs("playerVictoryDialog")] 
    public string[] en_playerVictoryDialog;
    [FormerlySerializedAs("playerLossDialog")] 
    public string[] en_playerLossDialog;
    
    [Header("French Dialogs")]
    public string[] fr_tightSpotDialog;

    public string[] fr_playerVictoryDialog;
    [EndGroup("Deprecated")]
    public string[] fr_playerLossDialog;

    #endregion

    void OnValidate() {
        if (Trainer == null) Debug.LogError("No Trainer provided", gameObject);
        if (DirectionSurrogate == null) Debug.LogError("No Direction Surrogate provided", gameObject);
        else DirectionSurrogate.GizmoDrawLength = SightDistance;

        if (animator == null) {
            animator = GetComponent<SpriteAnimatorBehaviour>();
            if (animator == null)
                Debug.LogError("No mount sprite found or provided", gameObject);
        }
        if (animator != null && Trainer != null) {
            animator.Animations = Trainer.Class.Animations;
            SwitchAnimation("Idle");
            DirectionSurrogate.OnDirectionUpdated.AddListener(SwitchAnimation);
        }
    }

    public DirectionSurrogate DirectionSurrogate => directionSurrogate;

    public Vector3 FacingDirection => directionSurrogate.FacingDirection;

    public void SwitchAnimation(string animationName) {
        //animator.SwitchAnimation(FacingDirection, animationName);
        animator.SwitchAnimation(animationName);
    }

    public void SwitchAnimation(Vector3 facingDirection) {
        animator.SwitchAnimation(facingDirection);
    }
}


[System.Serializable]
public class PokemonInitialiser
{
    public int ID;
    public int level;
    public bool? gender;
    public string heldItem;
    public int ability;
    public string[] moveset;
}