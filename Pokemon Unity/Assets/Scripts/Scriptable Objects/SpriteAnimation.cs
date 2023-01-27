using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "New Sprite Animation", menuName = "Pokemon Unity/Sprite Animation")]
public class SpriteAnimation : ScriptableObject {
    public string Name = "";
    [Description("Set to 0 to stop animation")]
    public float FPS = 1;
    public int StartingFrame = 0;
    public bool Loop = true;
    public Sprite[] spriteSheet;

    int frame = -1;
    float passedTime = 0f;

    public int Frame { get => frame; }

    public float SecondsPerFrame { get => 1f / FPS; }

    public Sprite Animate(float deltaTime) {
        if (frame == -1) frame = StartingFrame;
        if (FPS == 0f) return spriteSheet[frame];
        if (isDoneAnimating()) return spriteSheet[frame];

        passedTime += deltaTime;
        if (shouldGoToNextFrame(passedTime)) {
            if (frame >= spriteSheet.Length - 1)
                frame = 0;
            else
                frame++;
            passedTime = 0f;
        }
        return spriteSheet[frame];
    }

    bool isDoneAnimating() => frame == spriteSheet.Length - 1 && !Loop;

    bool shouldGoToNextFrame(float passedTime) {
        return passedTime >= SecondsPerFrame;
    }

    //[Button]
    //[SerializeField] 
    //void LoadSprites(SpriteAtlas spritesToLoad) {
    //    if (spritesToLoad == null) return;
    //    spritesToLoad.
    //}
}
