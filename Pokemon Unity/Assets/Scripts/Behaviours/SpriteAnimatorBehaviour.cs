using UnityEngine;
using System.Collections.Generic;
using System;
using EasyButtons;

public class SpriteAnimatorBehaviour : MonoBehaviour
{
    [Description("If null, attemps to grab a SpriteRenderer from this GameObject")]
    public string DefaultAnimationAction = "Idle";
    public SpriteRenderer SpriteRenderer;
    public SpriteRenderer ReflectionSpriteRenderer;
    public bool Paused = false;
    new SpriteAnimation animation;
    
    public List<SpriteAnimation> Animations;

    void Awake() {
        if (SpriteRenderer == null) 
            SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (Paused) return;
        SpriteRenderer.sprite = animation.Animate(Time.deltaTime);
        if (ReflectionSpriteRenderer != null) 
            ReflectionSpriteRenderer.sprite = SpriteRenderer.sprite;
    }

    public void SwitchAnimation(int index) {
        if (Animations.Count == 0) Debug.LogWarning($"No animations provided for {name}");
        animation = Animations[index];
        SpriteRenderer.sprite = animation.spriteSheet[0];
    }

    public void SwitchAnimation(Vector3 facingDirection) {
        SwitchAnimation(facingDirection, DefaultAnimationAction);
    }

    public void SwitchAnimation(Vector3 facingDirection, string newAnimationName) {
        string animationName = newAnimationName + facingDirection.ToDirectionString(Vector3.forward, Vector3.up);
        SwitchAnimation(animationName);
    }

    public void SwitchAnimation(string name) {
        int index = Animations.FindIndex((SpriteAnimation animation) => animation.Name.ToLowerAndTrim() == name.ToLowerAndTrim());
        if (index == -1)
            throw new AnimationDoesntExist(name, gameObject);

        SwitchAnimation(index);
    }

    public class AnimationDoesntExist : Exception {
        public AnimationDoesntExist(string name, GameObject gameObject) {
            Debug.LogError($"The animation '{name}' does not exist", gameObject);
        }
    }
}
