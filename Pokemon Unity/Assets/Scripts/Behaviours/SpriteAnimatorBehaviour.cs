using UnityEngine;
using System.Collections.Generic;
using System;
using EasyButtons;

public class SpriteAnimatorBehaviour : MonoBehaviour
{
    public int ActiveAnimationIndex = 0;
    [Description("If null, attemps to grab a SpriteRenderer from this GameObject")]
    public SpriteRenderer SpriteRenderer;
    public SpriteRenderer ReflectionSpriteRenderer;
    public bool Paused = false;
    new SpriteAnimation animation;
    
    public List<SpriteAnimation> Animations;

    void Awake() {
        if (SpriteRenderer == null) 
            SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        SwitchAnimation(ActiveAnimationIndex);
    }

    void Update() {
        if (Paused) return;
        SpriteRenderer.sprite = animation.Animate(Time.deltaTime);
        if (ReflectionSpriteRenderer != null) 
            ReflectionSpriteRenderer.sprite = SpriteRenderer.sprite;
    }

    public void SwitchAnimation(int index) => SwitchAnimation(index, null);
    public void SwitchAnimation(string name) => SwitchAnimation(name, null);

    public void SwitchAnimation(int index, float? fps = null) {
        ActiveAnimationIndex = index;
        animation = Animations[index];
        if (fps.HasValue) animation.FPS = fps.Value;
    }

    public void SwitchAnimation(string name, float? fps = null) {
        int index = Animations.FindIndex((SpriteAnimation animation) => animation.Name.ToLowerAndTrim() == name.ToLowerAndTrim());
        if (index == -1)
            throw new AnimationDoesntExist(name, gameObject);

        SwitchAnimation(index, fps);
    }

    public class AnimationDoesntExist : Exception {
        public AnimationDoesntExist(string name, GameObject gameObject) {
            Debug.LogError($"The animation '{name}' does not exist", gameObject);
        }
    }
}
