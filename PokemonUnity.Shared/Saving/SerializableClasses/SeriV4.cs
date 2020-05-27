//Original Scripts by http://answers.unity3d.com/users/4275/vicenti.html

using System;
using System.Collections;
using System.Runtime.Serialization;
//#if (DEBUG == false || UNITY_EDITOR == true)
//using UnityEngine;
//using UnityEditor;
//#endif

namespace PokemonUnity
{
	[System.Serializable]
	public class Quaternion
	{
		public float x { get; set; }
		public float y { get; set; }
		public float z { get; set; }
		public float w { get; set; }

		public Quaternion(float rX, float rY, float rZ, float rW)
		{
			x = rX;
			y = rY;
			z = rZ;
			w = rW;
		}
	}

	/*// <summary>
	/// Since unity doesn't flag the Vector4 as serializable, we
	/// need to create our own version. This one will automatically convert
	/// between Vector4 and SerializableVector4
	/// </summary>
	[System.Serializable]
	public class SeriV4 : ISerializable
	{
		/// <summary>
		/// Serializable Vector 3
		/// </summary>
		public UnityEngine.Vector4 V4 { get; set; }

		public float x
		{
			get { return V4.x; }
			set { V4 = V4.WithX(value); }
		}

		public float y
		{
			get { return V4.y; }
			set { V4 = V4.WithY(value); }
		}

		public float z
		{
			get { return V4.z; }
			set { V4 = V4.WithZ(value); }
		}

		public float w
		{
			get { return V4.w; }
			set { V4 = V4.WithW(value); }
		}

		public SeriV4(UnityEngine.Vector4 v)
		{
			V4 = v;
		}

		public SeriV4(float rX, float rY, float rZ, float rW)
		{
			w = rW;
			x = rX;
			y = rY;
			z = rZ;
		}

		/// <summary>
		/// The special constructor is used to deserialize values. 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public SeriV4(SerializationInfo info, StreamingContext context)
		{
			// Reset the property value using the GetValue method.
			V4 = new UnityEngine.Vector4(
				(float)info.GetValue("x", typeof(float)),
				(float)info.GetValue("y", typeof(float)),
				(float)info.GetValue("z", typeof(float)),
				(float)info.GetValue("w", typeof(float))
			);
		}


		public static implicit operator UnityEngine.Vector4(SeriV4 v)
		{
			return v.V4;
		}

		public static explicit operator SeriV4(UnityEngine.Vector4 v)
		{
			return new SeriV4(v);
		}

		public static UnityEngine.Vector4 operator *(SeriV4 v, float f)
		{
			return v.V4 * f;
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			// Use the AddValue method to specify serialized values.
			info.AddValue("x", V4.x, typeof(float));
			info.AddValue("y", V4.y, typeof(float));
			info.AddValue("z", V4.z, typeof(float));
			info.AddValue("w", V4.z, typeof(float));
		}
		public override string ToString()
		{
			return string.Format("[w={0}, x={1}, y={2}, z={3}]", w, x, y, z);
		}
	}

	/// <summary>
	/// Custom extension for Vector4.
	/// Used with replacing a value of a single axis
	/// </summary>
	public static partial class Utex
	{
		public static UnityEngine.Vector4 WithX(this UnityEngine.Vector4 v, float x)
		{
			return new UnityEngine.Vector4(x, v.y, v.z, v.w);
		}

		public static UnityEngine.Vector4 WithY(this UnityEngine.Vector4 v, float y)
		{
			return new UnityEngine.Vector4(v.x, y, v.z, v.w);
		}

		public static UnityEngine.Vector4 WithZ(this UnityEngine.Vector4 v, float z)
		{
			return new UnityEngine.Vector4(v.x, v.y, z, v.w);
		}

		public static UnityEngine.Vector4 WithW(this UnityEngine.Vector4 v, float w)
		{
			return new UnityEngine.Vector4(v.x, v.y, v.z, w);
		}
	}*/
}