using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMoveAnimatePlayer : OverworldMoveAnimate
{
  public Texture2D runSpriteSheet;
  protected Sprite[] runSprites;
  protected new void Start()
  {
    base.Start();
    runSprites = Resources.LoadAll<Sprite>(runSpriteSheet.name); 
    // We could switch between the walk sprites and the run sprites at any time, so we keep both in memory
  }

  protected static SpriteAnimation RunBase = new SpriteAnimation(new List<KeyValuePair<float, int>>
  {
    new KeyValuePair<float, int>(0.0f, 0),
    new KeyValuePair<float, int>((2.5f/30.0f), 1),
    new KeyValuePair<float, int>((5.0f/30.0f), 0),
    new KeyValuePair<float, int>((7.5f/30.0f), 2),
  }, 10.0f / 30.0f);
  protected override void DoAnimation() 
  {
    switch(moveState)
    {
      case EOverworldMoveState.Idle: // Idle state is simple, can be optimized as just having a single movement
        sRenderer.sprite = walkSprites[3 * (int)direction];
        break;
      case EOverworldMoveState.Moving:
        int spIndex = GetSpriteIndex(WalkBase, direction);
        sRenderer.sprite = walkSprites[spIndex];
        break;
      case EOverworldMoveState.Running:
        sRenderer.sprite = runSprites[GetSpriteIndex(RunBase, direction)];
        break;
      case EOverworldMoveState.Bumped:
        int bumpedSpriteIndex = GetSpriteIndex(WalkBase, direction);
        if(currentFrame == 2) // End the animation early and transition back to idle
        {
          SetMoveState(EOverworldMoveState.Idle);
        }
        else
        {
          sRenderer.sprite = walkSprites[bumpedSpriteIndex];
        }
        break;
    }
  }
}
