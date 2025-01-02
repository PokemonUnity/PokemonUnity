using UnityEngine;
using System.Collections.Generic;
using System;
using EasyButtons;

public class SpriteAnimatorBehaviour : MonoBehaviour
{
    [Description("If null, attemps to grab a SpriteRenderer from this GameObject")]
    public string AnimationAction = "Idle";
    public SpriteRenderer SpriteRenderer;
    public SpriteRenderer ReflectionSpriteRenderer;
    public bool Paused = false;
    new SpriteAnimation animation;
    int activeIndex;
    Vector3 lastFacingDirection;

    public List<SpriteAnimation> Animations;

    void Awake() {
        if (SpriteRenderer == null) 
            SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Start() {
        //Debug.Log(AnimationAction, gameObject);
        //if (AnimationAction.Length > 8)
        //    Debug.LogError("");
    }

    void Update() {
        if (Paused) return;
        SpriteRenderer.sprite = animation.Animate(Time.deltaTime);
        if (ReflectionSpriteRenderer != null) 
            ReflectionSpriteRenderer.sprite = SpriteRenderer.sprite;
    }

    public void SwitchAnimation(int index) {
        if (Animations.Count == 0) Debug.LogWarning($"No animations provided for {name}");
        activeIndex = index;
        animation = Animations[index];
        SpriteRenderer.sprite = animation.spriteSheet[0];
    }

    public void SwitchAnimation(Vector3 facingDirection) {
        SwitchAnimation(facingDirection, AnimationAction);
    }

    public void SwitchAnimation(Vector3 facingDirection, string newAnimationName) {
        lastFacingDirection = facingDirection;
        
        SwitchAnimation(newAnimationName);
    }

    public void SwitchAnimation(string newAnimationName) {
        //if (newAnimationName.Length > 8 && gameObject.name == "Follower")
        //    Debug.LogError("");
        AnimationAction = newAnimationName;
        switchAnimation();
    }

    void switchAnimation() {
        string fullAnimationName = AnimationAction + lastFacingDirection.ToDirectionString();
        if (Animations == null) {
            Debug.LogError("No Animations provided", gameObject);
            return;
        }

        fullAnimationName = fullAnimationName.ToLowerAndTrim();
        int index = Animations.FindIndex((SpriteAnimation animation) => animation.Name.ToLowerAndTrim() == fullAnimationName);
        if (index == -1)
            throw new AnimationDoesntExist(AnimationAction, gameObject);

        SwitchAnimation(index);
    }

    public class AnimationDoesntExist : Exception {
        public AnimationDoesntExist(string name, GameObject gameObject) {
            Debug.LogError($"The animation '{name}' does not exist", gameObject);
        }
    }
}
