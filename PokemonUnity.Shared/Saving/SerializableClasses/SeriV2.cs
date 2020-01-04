//Original Scripts by http://answers.unity3d.com/users/4275/vicenti.html

using System;
using System.Collections;
using System.Runtime.Serialization;
//#if (DEBUG == false || UNITY_EDITOR == true)
//using UnityEngine;
//using UnityEditor;
//#endif

namespace PokemonUnity.Utility
{
	public struct Point
	{
		public float x { get; set; }
		public float y { get; set; }

		public Point(float rX, float rY)
		{
			x = rX;
			y = rY;
		}
	}

	/*// <summary>
	/// Since unity doesn't flag the Vector2 as serializable, we
	/// need to create our own version. This one will automatically convert
	/// between Vector2 and SerializableVector2
	/// </summary>
	[System.Serializable]
	public class SeriV2 : ISerializable
	{
		/// <summary>
		/// Serializable Vector 2
		/// </summary>
		public UnityEngine.Vector2 v2 { get; set; }

		public float x
		{
			get { return v2.x; }
			set { v2 = v2.WithX(value); }
		}

		public float y
		{
			get { return v2.y; }
			set { v2 = v2.WithY(value); }
		}

		public SeriV2(UnityEngine.Vector2 v)
		{
			v2 = v;
		}

		public SeriV2(float rX, float rY)
		{
			x = rX;
			y = rY;
		}

		/// <summary>
		/// The special constructor is used to deserialize values. 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public SeriV2(SerializationInfo info, StreamingContext context)
		{
			// Reset the property value using the GetValue method.
			v2 = new UnityEngine.Vector2(
				(float)info.GetValue("x", typeof(float)),
				(float)info.GetValue("y", typeof(float))
			);
		}


		public static implicit operator UnityEngine.Vector2(SeriV2 v)
		{
			return v.v2;
		}

		public static explicit operator SeriV2(UnityEngine.Vector2 v)
		{
			return new SeriV2(v);
		}

		public static UnityEngine.Vector2 operator *(SeriV2 v, float f)
		{
			return v.v2 * f;
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			// Use the AddValue method to specify serialized values.
			info.AddValue("x", v2.x, typeof(float));
			info.AddValue("y", v2.y, typeof(float));
		}
		public override string ToString()
		{
			return string.Format("[x={0}, y={1}]", x, y);
		}
	}

	/// <summary>
	/// Custom extension for Vector2.
	/// Used with replacing a value of a single axis
	/// </summary>
	public static partial class Utex
	{
		public static UnityEngine.Vector2 WithX(this UnityEngine.Vector2 v, float x)
		{
			return new UnityEngine.Vector2(x, v.y);
		}

		public static UnityEngine.Vector2 WithY(this UnityEngine.Vector2 v, float y)
		{
			return new UnityEngine.Vector2(v.x, y);
		}
	}*/
}