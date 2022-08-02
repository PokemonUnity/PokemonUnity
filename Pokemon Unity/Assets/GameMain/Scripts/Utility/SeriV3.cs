//Original Scripts by http://answers.unity3d.com/users/4275/vicenti.html

using System;
using System.Collections;
using System.Runtime.Serialization;
#if (DEBUG == false || UNITY_EDITOR == true)
using UnityEngine;
using UnityEditor;
#endif

namespace PokemonUnity.Utility
{
	//public interface IVector
	//{
	//	float x { get; }
	//	float y { get; }
	//	float z { get; }
	//}

	//[System.Serializable]
	//public struct Vector : IVector
	//{
	//	public float x { get; set; }
	//	public float y { get; set; }
	//	public float z { get; set; }
	//
	//	public Vector(float rX, float rY, float rZ)
	//	{
	//		x = rX;
	//		y = rY;
	//		z = rZ;
	//	}
	//}

	/// <summary>
	/// Since unity doesn't flag the Vector3 as serializable, we
	/// need to create our own version. This one will automatically convert
	/// between Vector3 and SerializableVector3
	/// </summary>
	[System.Serializable]
	public class SeriV3 : ISerializable, IVector
	{
		/// <summary>
		/// Serializable Vector 3
		/// </summary>
		public UnityEngine.Vector3 v3 { get; set; }

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

		public SeriV3(UnityEngine.Vector3 v)
		{
			v3 = v;
		}

		public SeriV3(float rX, float rY, float rZ)
		{
			x = rX;
			y = rY;
			z = rZ;
		}

		/// <summary>
		/// The special constructor is used to deserialize values. 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public SeriV3(SerializationInfo info, StreamingContext context)
		{
			// Reset the property value using the GetValue method.
			v3 = new UnityEngine.Vector3(
				(float)info.GetValue("x", typeof(float)),
				(float)info.GetValue("y", typeof(float)),
				(float)info.GetValue("z", typeof(float))
			);
		}


		public static implicit operator UnityEngine.Vector3(SeriV3 v)
		{
			return v.v3;
		}

		public static explicit operator SeriV3(UnityEngine.Vector3 v)
		{
			return new SeriV3(v);
		}

		public static UnityEngine.Vector3 operator *(SeriV3 v, float f)
		{
			return v.v3 * f;
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			// Use the AddValue method to specify serialized values.
			info.AddValue("x", v3.x, typeof(float));
			info.AddValue("y", v3.y, typeof(float));
			info.AddValue("z", v3.z, typeof(float));
		}
		public override string ToString()
		{
			return string.Format("[x={0}, y={1}, z={2}]", x, y, z);
		}
	}

	/// <summary>
	/// Custom extension for Vector3.
	/// Used with replacing a value of a single axis
	/// </summary>
	public static partial class Utex
	{
		public static UnityEngine.Vector3 WithX(this UnityEngine.Vector3 v, float x)
		{
			return new UnityEngine.Vector3(x, v.y, v.z);
		}

		public static UnityEngine.Vector3 WithY(this UnityEngine.Vector3 v, float y)
		{
			return new UnityEngine.Vector3(v.x, y, v.z);
		}

		public static UnityEngine.Vector3 WithZ(this UnityEngine.Vector3 v, float z)
		{
			return new UnityEngine.Vector3(v.x, v.y, z);
		}
	}
}