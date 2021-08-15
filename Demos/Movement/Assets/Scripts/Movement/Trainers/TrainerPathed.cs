using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerPathed : TrainerMovementBase
{
  public List<Vector3> path;
  private int currentPathTarget = 0;
  private bool pathingForwards = true;
  public bool loop = false;
  // Pathed trainers are dumb as rocks. 
  // I did not write a pathfinding routine, they just attempt to go to the next point by
  // traversing in a straight line towards the next spot in path. 
  // Once they reach the end of their path, they go backwards.
  // They DO do raycasts - if the player (or anything else) blocks them, they stop. They'll just keep trying to move that way
  // until they can.

  protected new void Start()
  {
    base.Start();
  }
  void DoNextPathTarget()
  {
    if(pathingForwards)
    {
      if(currentPathTarget + 1 < path.Count)
      {
        currentPathTarget += 1;
      }
      else
      {
        if(loop)
        {
          currentPathTarget = 0;
        }
        else
        {
          currentPathTarget -= 1;
          pathingForwards = false;
        }
      }
    }
    else
    {
      if (currentPathTarget - 1 >= 0)
      {
        currentPathTarget -= 1;
      }
      else
      {
        currentPathTarget += 1;
        pathingForwards = true;
      }
    }
  }
  protected override void HandleMovement()
  {
    base.HandleMovement();
    if (MoveState != EMovementState.Idle)
    {
      return;
    }
    
    Vector3 pathDistance = path[currentPathTarget] - gameObject.transform.position;
    if(pathDistance == Vector3.zero)
    {
      DoNextPathTarget();
      pathDistance = path[currentPathTarget] - gameObject.transform.position;
    }
    Vector2 move;
    if(pathDistance.x > 0)
    {
      move = new Vector2(1.0f, 0.0f);
    }
    else if(pathDistance.x < 0)
    {
      move = new Vector2(-1.0f, 0.0f);
    }
    else if(pathDistance.z > 0)
    {
      move = new Vector2(0.0f, 1.0f);
    }
    else
    {
      move = new Vector2(0.0f, -1.0f);
    }
    RaycastHit hit = CheckMovement(move);
    if(hit.collider != null)
    {
      MoveState = EMovementState.Idle;
    }
    else
    {
      MoveState = EMovementState.Moving;
    }
    lastMoveInput = move;
  }
}
