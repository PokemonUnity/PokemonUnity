using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerMovementBase : MovementBase
{
  protected override OverworldMoveAnimate.EOverworldMoveState GetAnimMoveState()
  {
    switch (MoveState)
    {
      case EMovementState.Idle: return OverworldMoveAnimate.EOverworldMoveState.Idle;
      case EMovementState.Moving: return OverworldMoveAnimate.EOverworldMoveState.Moving;
    }
    return OverworldMoveAnimate.EOverworldMoveState.Idle;
  }
  // This demo doesn't have the code, but this is where you would put any movement events for triggering battles.
}
