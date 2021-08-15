using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMovementState { Idle, Moving, Running, Bumped };

public class MovementBase : MonoBehaviour
{

  protected BoxCollider baseCollider;
  readonly static float collRayLength = 1.499f; // statically calc this to go fast 

  // Offset for all movement raycasts (from hitbox center). Both horizontal and vertical position. Starting a bit below the top of the 1x1 cube
  protected readonly static Vector3 raycastOffset = new Vector3(0.0f, 1.0f, 0.0f);
  public OverworldMoveAnimate movementAnimationComponent;

  // Movement
  protected Vector2 lastMoveInput = new Vector2(1.0f, 0.0f);
  protected Vector2 distanceMoved = new Vector2(0, 0);
  protected float baseSpeed = 3.0f;
  protected float moveMultiplier = 1.0f;

  protected EMovementState MoveState = EMovementState.Idle;
  protected EMovementState lastMoveState = EMovementState.Idle;
  // Start is called before the first frame update
  protected void Start()
  {
    baseCollider = gameObject.GetComponent<BoxCollider>();
    baseCollider.isTrigger = false;
  }

  protected virtual void ExtendHitbox()
  {
    if(MoveState == EMovementState.Idle)
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

  // Update is called once per frame
  void Update()
  {
    lastMoveState = MoveState;
    HandleMovement();
    ExtendHitbox();
    if (lastMoveState != MoveState && MoveState != EMovementState.Idle)
    {
      movementAnimationComponent.SetMoveState(GetAnimMoveState());
    }
    else if (MoveState == EMovementState.Idle && lastMoveState == EMovementState.Idle)
    {
      // Wait an extra frame to return to idle, so that we don't tear while running
      movementAnimationComponent.SetMoveState(GetAnimMoveState());
    }
    if (lastMoveInput.x != 0)
    {
      movementAnimationComponent.SetDirection(lastMoveInput.x < 0.0f ? OverworldMoveAnimate.EFacingDirection.Left : OverworldMoveAnimate.EFacingDirection.Right);
    }
    else
    {
      movementAnimationComponent.SetDirection(lastMoveInput.y < 0.0f ? OverworldMoveAnimate.EFacingDirection.Down : OverworldMoveAnimate.EFacingDirection.Up);
    }
  }

  protected virtual OverworldMoveAnimate.EOverworldMoveState GetAnimMoveState()
  {
    return OverworldMoveAnimate.EOverworldMoveState.Idle;
  }

  // HandleMovement is where most base movement logic is handled. In the base class, is basically does nothing.
  protected virtual void HandleMovement()
  {
    if(MoveState != EMovementState.Idle)
    {
      DoMove();
    }
  }

  // DoMove is the base function for handling movement from one tile to another. You can use this out of the box, or override it.
  protected virtual void DoMove()
  {
    Vector2 move = lastMoveInput * Time.deltaTime * baseSpeed;
    move *= moveMultiplier;
    distanceMoved += move;
    // string debugMessage = distanceMoved.magnitude + " " + lastMoveInput.magnitude + " | " + lastMoveInput * baseSpeed;
    if (distanceMoved.magnitude >= lastMoveInput.magnitude)
    {
      // gone past target tile, correct to the tile for the next move, and reset to idle
      move = lastMoveInput - distanceMoved;
      distanceMoved = new Vector2(0, 0);
      MoveState = EMovementState.Idle;
      RoundPosition();      
      return;
    }

    gameObject.transform.position += new Vector3(move.x, 0.0f, move.y);
    // While moving, our collider must be larger. It needs to be tall enough that if we're moving onto stairs, something ahead of us can
    // still collide with us. It also needs to overlap with both the square we were in, and the one we're travelling to.
    // A side affect of this is that this version of the collision can't really deal with bridges that are 1 sq tall very well.
    // If you need this behavior, and you figure out a version that works well with them, feel free to update this demo.
    baseCollider.size = new Vector3(1.0f + Mathf.Abs(lastMoveInput.x), 2.0f, 1.0f + Mathf.Abs(lastMoveInput.y));
    baseCollider.center = new Vector3(-distanceMoved.x + lastMoveInput.x * 0.5f, 1.0f, -distanceMoved.y + lastMoveInput.y * 0.5f);
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

  protected void RoundPosition()
  {
    gameObject.transform.position = new Vector3(Mathf.Round(gameObject.transform.position.x),
                                                gameObject.transform.position.y,
                                                Mathf.Round(gameObject.transform.position.z));
  }

  // Helper function for handling raycasts towards adjacent objects.
  // Raycasts are done from the top of the object
  protected RaycastHit CheckMovement(Vector2 move)
  {
    Vector3 raycastDirection = new Vector3(move.x, 0.0f, move.y);
    Vector3 offsetCenter = gameObject.transform.position + raycastOffset;
    Ray colliderRay = new Ray(offsetCenter, raycastDirection);
    // Debug.DrawRay(offsetCenter, raycastDirection.normalized * collRayLength, Color.green, 1.0f, true);
    Physics.Raycast(colliderRay, out RaycastHit hit, collRayLength);
    return hit;
  }
}
