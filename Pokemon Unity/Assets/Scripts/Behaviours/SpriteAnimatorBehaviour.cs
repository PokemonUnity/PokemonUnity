using UnityEngine;
using System.Collections.Generic;
using System;
using EasyButtons;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimatorBehaviour : MonoBehaviour
{
    public int ActiveAnimationIndex = 0;
    [Description("If null, attemps to grab a SpriteRenderer from this GameObject")]
    public SpriteRenderer spriteRenderer;
    public List<SpriteAnimation> Animations;
    new SpriteAnimation animation;
    public bool Paused = false;

    void Awake() {
        if (spriteRenderer == null) 
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        SwitchAnimation(ActiveAnimationIndex);
    }

    void Update() {
        if (Paused) return;
        spriteRenderer.sprite = animation.Animate(Time.deltaTime);
    }

    public void SwitchAnimation(int index) {
        ActiveAnimationIndex = index;
        animation = Animations[index];
    }

    public void SwitchAnimation(string name) {
        int index = Animations.FindIndex((SpriteAnimation animation) => animation.Name == name);
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
