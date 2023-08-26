using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    /// <summary>
    /// Calculates the direction of the given forward fector assuming the following static orientation
    /// <code>
    /// _\_FORWARDS_/_  <br/>
    /// __\________/__  <br/>
    /// ___\__up__/___  <br/>
    /// ____\____/____  <br/>
    /// _____\__/_____  <br/>
    /// left__\/__right <br/>
    /// ______/\______  <br/>
    /// _____/__\_____  <br/>
    /// ____/____\____  <br/>
    /// ___/______\___  <br/>
    /// __/__down__\__  <br/>
    /// </code>
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="forward"></param>
    /// <param name="up"></param>
    /// <returns></returns>
    public static EMovementDirection ToDirectionEnum(this Vector3 vector, Vector3 forward, Vector3 up) {
        float angle = Vector3.SignedAngle(forward, vector, up);
        if (angle >= -45f && angle <= 45F) return EMovementDirection.Up;
        if (angle > 45f && angle < 135f) return EMovementDirection.Right;
        if (angle <= -135f || angle >= 135f) return EMovementDirection.Down;
        if (angle < -45f && angle > -135f) return EMovementDirection.Left;
        return EMovementDirection.Nowhere;
    }

    public static EMovementDirection ToDirectionEnum(this Vector3 vector) {
        return vector.ToDirectionEnum(Vector3.forward, Vector3.up);
    }

    ///<summary>Uses Vector3.forward and Vector3.up to determine direction</summary>
    /// <returns>Returns empty string if no direction could be determined</returns>
    public static string ToDirectionString(this Vector3 vector) {
        return vector.ToDirectionString(Vector3.forward, Vector3.up);
    }

    /// <returns>Returns empty string if no direction could be determined</returns>
    public static string ToDirectionString(this Vector3 vector, Vector3 forward, Vector3 up) {
        return vector.ToDirectionEnum(forward, up) switch {
            EMovementDirection.Up => "Up",
            EMovementDirection.Right => "Right",
            EMovementDirection.Down => "Down",
            EMovementDirection.Left => "Left",
            EMovementDirection.Nowhere => "",
            _ => "",
        };
    }

    public static bool IsBasicallyEqualTo(this Vector3 value, Vector3 other) => Vector3.Distance(value, other).IsBasicallyZero();

    public static Vector3 RoundIfBasicallyZero(this Vector3 value) {
        if (value.x.IsBasicallyZero())
            value = new Vector3(0f, value.y, value.z);
        if (value.y.IsBasicallyZero())
            value = new Vector3(value.x, 0f, value.z);
        if (value.z.IsBasicallyZero())
            value = new Vector3(value.x, value.y, 0f);
        return value;
    }
}
