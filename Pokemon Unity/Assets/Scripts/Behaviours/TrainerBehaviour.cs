﻿//Original Scripts by IIColour (IIColour_Spectrum)

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
using System.Linq;
using System;
using UnityEngine.Events;

//namespace PokemonUnity.Unity {

[AddComponentMenu("Pokemon Unity/Trainer")]
[RequireComponent(typeof(SpriteAnimatorBehaviour))]
public class TrainerBehaviour : MonoBehaviour, INeedDirection, IDialog {
    public Trainer Trainer;
    [Foldout("Direction")]
    [SerializeField] DirectionSurrogate directionSurrogate;
    public float SightDistance;
    [Foldout("Dialog")]
    [SerializeField] DialogSeries dialogSeries;
    [SerializeField] DialogTriggerBehaviour dialogTrigger;
    public string CurrentDialogEpisodeName;

    public DialogSeries DialogSeries { get; set; }
    public DialogTriggerBehaviour DialogTriggers { get; }

    [Foldout("Interaction")]
    public bool ShouldTurnRandomly = false;
    public bool ShouldTurnOnInteraction = true;
    public bool ShouldTurnBack = false;
    public Interactable Interactable;
    [EndGroup("Interaction")]

    /// <summary>
    /// distinction between recent and general defeated, as a recently defeated trainer should stop patrolling/searching
    /// for other trainers. A trainer you defeated in the not-so-recent past should be looking around, although they will
    /// remember their defeat at your hands.
    /// </summary>
    bool recentlyDefeated = false;
    Ray playerDetectRay;
    SpriteAnimatorBehaviour animator;
    Quaternion previousRotation;

    [Foldout("Need to review")]
    public WalkCommand[] patrol = new WalkCommand[1];
    public float MaxWaitBeforeTurn = 5f;
    public float MinWaitBeforeTurn = 1f;

    private GameObject exclaim;
    public float Speed = 0.3f;

    #region MonoBehaviour Functions

    void OnDrawGizmos() {
        Gizmos.DrawRay(playerDetectRay);
    }

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
            DirectionSurrogate.OnDirectionUpdated.AddListener(updatePlayerDetectorDirection);
        }

        if (Trainer.Defeated)
            DirectionSurrogate.GizmoColor = Color.red;
        else
            DirectionSurrogate.GizmoColor = Color.green;

        if (Interactable == null) Debug.LogError("No Interactable provided", gameObject);
        Interactable.OnPreInteraction.AddListener(lookAtInteractor);
        Interactable.OnInteraction.AddListener((Interactor interactor) => TalkToInteractor(interactor, CurrentDialogEpisodeName));
        Interactable.OnPreInteraction.AddListener(lookToPreviousDirection);
    }

    void Update() {
        if (ShouldTurnRandomly)
            StartCoroutine(turnAtRandom());
        if (!Trainer.Defeated && !Interactable.Interacting)
            detectPlayer();
    }

    #endregion

    #region Direction & Animation

    public DirectionSurrogate DirectionSurrogate => directionSurrogate;

    public Vector3 FacingDirection => directionSurrogate.FacingDirection;

    public void LookAt(Transform target) => directionSurrogate.transform.LookAt(target);

    public void SwitchAnimation(string animationName) {
        //animator.SwitchAnimation(FacingDirection, animationName);
        animator.SwitchAnimation(animationName);
    }

    public void SwitchAnimation(Vector3 facingDirection) {
        animator.SwitchAnimation(facingDirection);
    }

    void updatePlayerDetectorDirection(Vector3 newDirection) => playerDetectRay.direction = newDirection;

    void lookAtInteractor(Interactor interactor) {
        if (ShouldTurnOnInteraction) {
            if (ShouldTurnBack)
                previousRotation = transform.rotation;

            directionSurrogate.UpdateDirection(interactor.transform);
        }
    }

    void lookToPreviousDirection(Interactor interactor) {
        if (ShouldTurnBack)
            directionSurrogate.UpdateDirection(previousRotation);
    }

    #endregion

    #region Interactions

    private IEnumerator turnAtRandom() {
        while (!Interactable.Interacting) {
            if (recentlyDefeated)
                yield break;

            directionSurrogate.RotateRandom();
            float waitTime = UnityEngine.Random.Range(MinWaitBeforeTurn, MaxWaitBeforeTurn);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void detectPlayer() {
        DirectionSurrogate.GizmoColor = Color.green;
        playerDetectRay.origin = transform.position + (Vector3.up * 0.5f);
        RaycastHit[] hits = Physics.RaycastAll(playerDetectRay, SightDistance); //, LayerMask.NameToLayer("Player"));
        if (hits.Length == 0) return;
        (var interactor, var playerMovement) = collidedWithAPlayer(hits);
        if (playerMovement == null) return;
        interactor.Interactable = Interactable;
        Interactable.Interacting = true;
        playerMovement.PauseMovement();
        ApproachInteractor(interactor);

        (Interactor, PlayerMovement) collidedWithAPlayer(RaycastHit[] hits) {
            foreach (RaycastHit hit in hits) {
                if (hit.collider.TryGetComponent(out Interactor interactor)) {
                    if (interactor.PassthroughGameObject == null) continue;
                    PlayerMovement playerMovement = interactor.PassthroughGameObject.GetComponent<PlayerMovement>();
                    if (playerMovement != null) return (interactor, playerMovement);
                }
            }
            return (null, null);
        }
    }

    public void ApproachInteractor(Interactor interactor) {
        StartCoroutine(ApproachAndTalkToInteractorWithStyle(interactor, () => {
            Interactable.Interact(interactor);
        }));
    }

    public void TalkToInteractor(Interactor interactor, string dialogEpisodeName) {
        if (!dialogSeries.DialogueEpisodes.ContainsKey(dialogEpisodeName)) {
            Debug.LogError($"Dialog Series '{dialogSeries.name}' does not contain the Episode '{dialogEpisodeName}'");
            return;
        }
        TalkToInteractor(interactor, dialogSeries.DialogueEpisodes[dialogEpisodeName]);
    }

    public void TalkToInteractor(Interactor interactor, DialogEpisode dialogEpisode) {
        DialogBoxBehaviour dialogBox = DialogBoxFactory.OpenDialog("Basic");
        if (!dialogTrigger.DialogEpisodeTriggers.ContainsKey(dialogEpisode.Name)) {
            Debug.LogError($"DialogTrigger doesn't exist with name '{dialogEpisode.Name}' in this Dialog Trigger", dialogTrigger);
            return;
        }
        DialogTrigger trigger = dialogTrigger.DialogEpisodeTriggers[dialogEpisode.Name];
        trigger.OnEpisodeEnd.AddListener((DialogEpisode episode) => interactor.FinishInteract());
        dialogBox.Dialog(dialogEpisode, trigger);
    }

    /// <summary>Walk towards the interactable (1 dimensional) assuming they are in the Trainers line of sight</summary>
    public IEnumerator ApproachAndTalkToInteractorWithStyle(Interactor interactor, Action onComplete) {
        //if the player isn't busy with any other object
        if (Trainer.Class.BattleBackgroundMusic != null)
            BackgroundMusicHandler.Singleton.PlayOverlay(Trainer.Class.BattleBackgroundMusic);
        // TODO
        //DISPLAY "!"
        //yield return StartCoroutine(exclaimAnimation());
        yield return new WaitForSeconds(0.6f);

        Vector3 oneUnitForward = Vector3.Scale(directionSurrogate.FacingDirection, GlobalVariables.UnitVectorSetting.Get());
        Vector3 finalDestination = interactor.transform.position - oneUnitForward;
        Vector3 startingPosition = transform.position;

        if (transform.position == finalDestination) yield break;

        SwitchAnimation("Walk");

        moveToInteractor(() => {
            SwitchAnimation("Idle");
            onComplete();
        });

        bool isAtDestination() => transform.position.IsBasicallyEqualTo(finalDestination);
        void moveToInteractor(Action onComplete) {
            if (isAtDestination())
                onComplete();
            else
                moveOneUnitForward().setOnComplete(() => moveToInteractor(onComplete));
        }
        LTDescr moveOneUnitForward() {
            return LeanTween.move(gameObject, transform.position + oneUnitForward, Speed);
        }
    }

    //private IEnumerator interact(Transform target) {
    //    if (target.setCheckBusyWith(this.gameObject)) {
    //        busy = true;

    //        target.LookAt(transform);

    //        if (!defeated) {
    //            // Play INTRO BGM
    //            BackgroundMusicHandler.Singleton.PlayOverlay(introBGM, samplesLoopStart);

    //            // Display all of the confrontation Dialog.
    //            for (int i = 0; i < en_trainerConfrontDialog.Length; i++) {
    //                Dialog.DrawDialogBox();
    //                yield return Dialog.StartCoroutine(Dialog.DrawText(en_trainerConfrontDialog[i]));
    //                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
    //                    yield return null;

    //                Dialog.UndrawDialogBox();
    //            }

    //            // custom cutouts not yet implemented
    //            StartCoroutine(ScreenFade.Singleton.FadeCutout(false, ScreenFade.SlowedSpeed, null));

    //            // Automatic LoopStart usage not yet implemented
    //            Scene.main.Battle.gameObject.SetActive(true);
    //            if (trainer.battleBGM != null) {
    //                Scene.main.Battle.battleBGM = trainer.battleBGM;
    //                Scene.main.Battle.battleBGMLoopStart = trainer.samplesLoopStart;
    //                BackgroundMusicHandler.Singleton.PlayOverlay(trainer.battleBGM, trainer.samplesLoopStart);
    //            } else {
    //                Scene.main.Battle.battleBGM = Scene.main.Battle.defaultTrainerBGM;
    //                Scene.main.Battle.battleBGMLoopStart = Scene.main.Battle.defaultTrainerBGMLoopStart;
    //                BackgroundMusicHandler.Singleton.PlayOverlay(Scene.main.Battle.defaultTrainerBGM,
    //                    Scene.main.Battle.defaultTrainerBGMLoopStart);
    //            }
    //            Scene.main.Battle.gameObject.SetActive(false);
    //            yield return new WaitForSeconds(1.6f);

    //            global::TrainerBehaviour player2 = null;

    //            if (PlayerMovement.Singleton.NpcFollower) {
    //                if (PlayerMovement.Singleton.NpcFollower.GetComponent<global::TrainerBehaviour>() != null) {
    //                    player2 = PlayerMovement.Singleton.NpcFollower.GetComponent<global::TrainerBehaviour>();
    //                }
    //            }

    //            Scene.main.Battle.gameObject.SetActive(true);
    //            StartCoroutine(Scene.main.Battle.control(trainer, doubleBattle, ally, player2));

    //            while (Scene.main.Battle.gameObject.activeSelf) {
    //                yield return null;
    //            }

    //            //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
    //            yield return StartCoroutine(ScreenFade.Singleton.Fade(true, 0.4f));

    //            if (Scene.main.Battle.victor == 0) {
    //                defeated = true;
    //                recentlyDefeated = true;
    //                //Display all of the defeated Dialog. (if any)
    //                for (int i = 0; i < en_trainerDefeatDialog.Length; i++) {
    //                    Dialog.DrawDialogBox();
    //                    yield return Dialog.StartCoroutine(Dialog.DrawText(en_trainerDefeatDialog[i]));
    //                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")) {
    //                        yield return null;
    //                    }
    //                    Dialog.UndrawDialogBox();
    //                }
    //            }
    //        } else //Display all of the post defeat Dialog.
    //            for (int i = 0; i < en_trainerPostDefeatDialog.Length; i++) {
    //                Dialog.DrawDialogBox();
    //                yield return Dialog.StartCoroutine(Dialog.DrawText(en_trainerPostDefeatDialog[i]));
    //                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")) {
    //                    yield return null;
    //                }
    //                Dialog.UndrawDialogBox();
    //            }

    //        busy = false;
    //        PlayerMovement.Singleton.unsetCheckBusyWith(this.gameObject);
    //    }
    //}

    #endregion

}


[System.Serializable]
public class PokemonInitialiser {
    public int ID;
    public int level;
    public bool? gender;
    public string heldItem;
    public int ability;
    public string[] moveset;
}
//}