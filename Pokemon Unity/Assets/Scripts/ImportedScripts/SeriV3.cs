//Original Scripts by http://answers.unity3d.com/users/4275/vicenti.html

using UnityEngine;
using System;
using System.Collections;
using PokemonUnity.Saving.SerializableClasses;
using System.Runtime.Serialization;
#if UNITY_EDITOR
using UnityEditor;

#endif


[System.Serializable]
public class SeriV3 : ISerializable
{
    //Serializable Vector 3

    public static implicit operator Vector3(SeriV3 v)
    {
        return v.v3;
    }

    public static explicit operator SeriV3(Vector3 v)
    {
        return new SeriV3(v);
    }

    public SeriV3(Vector3 v)
    {
        v3 = v;
    }

    public static Vector3 operator *(SeriV3 v, float f)
    {
        return v.v3 * f;
    }

    public Vector3 v3;

    public float x
    {
        get { return v3.x; }
        set { v3 = v3.WithX(value); }
    }

    public float y
    {
        get { return v3.y; }
        set { v3 = v3.WithY(value); }
    }

    public float z
    {
        get { return v3.z; }
        set { v3 = v3.WithZ(value); }
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        // Use the AddValue method to specify serialized values.
        info.AddValue("x", v3.x, typeof(float));
        info.AddValue("y", v3.y, typeof(float));
        info.AddValue("z", v3.z, typeof(float));
    }

    // The special constructor is used to deserialize values. 
    public SeriV3(SerializationInfo info, StreamingContext context)
    {
        // Reset the property value using the GetValue method.
        v3 = new Vector3(
            (float) info.GetValue("x", typeof(float)),
            (float) info.GetValue("y", typeof(float)),
            (float) info.GetValue("z", typeof(float))
        );
    }

    public override string ToString()
    {
        return string.Format("[x={0}, y={1}, z={2}]", x, y, z);
    }
}

// my real version of this class has a thousand things in it, these are just the necessary ones for seriv3
public static class Utex
{
    public static Vector3 WithX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    public static Vector3 WithY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    public static Vector3 WithZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }
}

