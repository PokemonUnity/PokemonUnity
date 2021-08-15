using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MovementBase
{
  //Bumped State
  private readonly float bumpedCooldown = 1.0f/3.0f;
  private float timeSinceLastBump = 0.0f;
  private bool movedSinceLastBump = true;
  protected override void ExtendHitbox()
  {
    if (MoveState == EMovementState.Idle || MoveState == EMovementState.Bumped)
    {
      baseCollider.size = new Vector3(1.0f, 1.6f, 1.0f); // 1.5f in y so that things transitioning from stairs will hit us, but not if we're 1 block below
      baseCollider.center = new Vector3(0.0f, 0.8f, 0.0f);
    }
    else
    {
      baseCollider.size = new Vector3(1.0f + Mathf.Abs(lastMoveInput.x), 2.0f, 1.0f + Mathf.Abs(lastMoveInput.y));
      baseCollider.center = new Vector3(-distanceMoved.x + lastMoveInput.x * 0.5f, 1.0f, -distanceMoved.y + lastMoveInput.y * 0.5f);
    }
  }
  new protected void Start()
  {
    moveMultiplier = 2.0f;
    base.Start();
  }
  protected override OverworldMoveAnimate.EOverworldMoveState GetAnimMoveState()
  {
    switch(MoveState)
    {
      case EMovementState.Idle: return OverworldMoveAnimate.EOverworldMoveState.Idle;
      case EMovementState.Moving: return OverworldMoveAnimate.EOverworldMoveState.Moving;
      case EMovementState.Running: return OverworldMoveAnimate.EOverworldMoveState.Running;
      case EMovementState.Bumped: return OverworldMoveAnimate.EOverworldMoveState.Bumped;
    }
    return OverworldMoveAnimate.EOverworldMoveState.Idle;
  }

  protected override void HandleMovement()
  {
    if(MoveState == EMovementState.Bumped)
    {
      //print("cooling" + timeSinceLastBump);
      timeSinceLastBump -= Time.deltaTime;
      if(timeSinceLastBump <= 0.0f)
      {
        MoveState = EMovementState.Idle;
      }
      return;
    }
    if (MoveState != EMovementState.Idle)
    {
      DoMove();
      return;
    }
    Vector2 wantedMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    EMovementState wantedState = wantedMovement == Vector2.zero ? EMovementState.Idle : EMovementState.Moving;
    if (wantedState == EMovementState.Idle)
    {
      movedSinceLastBump = true; // Player released the key, consider this "moving" for the sake of bumping
      return;
    }
    // If the B button is pressed, while wanting to move
    if (wantedState == EMovementState.Moving && Input.GetAxis("Deny") != 0.0f)
    {
      wantedState = EMovementState.Running;
    }
    Vector2 move = new Vector2(0.0f, 0.0f);
    if (wantedMovement.x != 0.0f)
    {
      move.x = wantedMovement.x > 0.0f ? 1.0f : -1.0f;
    }
    else
    {
      move.y = wantedMovement.y > 0.0f ? 1.0f : -1.0f;
    }
    RaycastHit hit = CheckMovement(move);
    if (hit.collider != null)
    {
      if (movedSinceLastBump)
      {
        wantedState = EMovementState.Bumped;
        timeSinceLastBump = bumpedCooldown;
        movedSinceLastBump = false;
      }
      else
      {
        wantedState = EMovementState.Idle;
      }
    }

    if(wantedState != EMovementState.Idle && wantedState != EMovementState.Bumped)
    {
      movedSinceLastBump = true;
    }
    lastMoveInput = move;
    MoveState = wantedState;
  }

  protected override void DoMove()
  {
    Vector2 move = lastMoveInput * Time.deltaTime * baseSpeed;
    if (MoveState == EMovementState.Moving && Input.GetAxis("Deny") != 0.0f)
    {
      MoveState = EMovementState.Running;
    }
    if (MoveState == EMovementState.Running)
    {
      move *= moveMultiplier;
    }
    distanceMoved += move;
    // string debugMessage = distanceMoved.magnitude + " " + lastMoveInput.magnitude + " | " + lastMoveInput * playerBaseSpeed;
    if(distanceMoved.magnitude >= lastMoveInput.magnitude) 
    {
      // gone past target tile, correct to the tile for the next move, and reset to idle
      move = lastMoveInput - distanceMoved;
      distanceMoved = new Vector2(0, 0);
      MoveState = EMovementState.Idle;
      RoundPosition();
      return;
    }

    gameObject.transform.position += new Vector3(move.x, 0.0f, move.y);
    // Vertical movement raycast for ground snapping
    Vector3 raycastStart = gameObject.transform.position + raycastOffset;
    Vector3 raycastDirection = new Vector3(0.0f, -1.5f, 0.0f);
    Ray colliderRay = new Ray(raycastStart, raycastDirection);
    //Debug.DrawRay(raycastStart, raycastDirection, Color.red, 1.0f, true);
    if (Physics.Raycast(colliderRay, out RaycastHit hit, 1.5f))
    {
      gameObject.transform.position = new Vector3(gameObject.transform.position.x, hit.point.y, gameObject.transform.position.z);
    }
    
  }
}
