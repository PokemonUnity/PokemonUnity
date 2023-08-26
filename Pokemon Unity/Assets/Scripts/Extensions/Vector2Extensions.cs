using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extensions
{
    public static EMovementDirection ToMovementDirection(this Vector2 vector) {
        if (vector.x > 0 && vector.y == 0) return EMovementDirection.Right;
        if (vector.x < 0 && vector.y == 0) return EMovementDirection.Left;
        if (vector.x == 0 && vector.y > 0) return EMovementDirection.Up;
        if (vector.x == 0 && vector.y < 0) return EMovementDirection.Down;
        return EMovementDirection.Nowhere;
    }


}
