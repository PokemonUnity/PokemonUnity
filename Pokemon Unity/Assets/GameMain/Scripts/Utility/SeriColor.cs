//Original Scripts by http://answers.unity3d.com/users/4275/vicenti.html

using System;
using System.Collections;
using System.Runtime.Serialization;
using PokemonEssentials.Interface;
#if (DEBUG == false || UNITY_EDITOR == true)
using UnityEngine;
using UnityEditor;
#endif

namespace PokemonUnity.Utility
{
	//public class SerializableColor
	//{
	//	public float _r;
	//	public float _g;
	//	public float _b;
	//	public float _a;
	//
	//	public Color Color
	//	{
	//		get
	//		{
	//			return new Color(_r, _g, _b, _a);
	//		}
	//		set
	//		{
	//			_r = value.r;
	//			_g = value.g;
	//			_b = value.b;
	//			_a = value.a;
	//		}
	//	}
	//
	//	public SerializableColor()
	//	{
	//		// (Optional) Default to white with an empty initialization
	//		_r = 1f;
	//		_g = 1f;
	//		_b = 1f;
	//		_a = 1f;
	//	}
	//
	//	public SerializableColor(float r, float g, float b, float a = 0f)
	//	{
	//		_r = r;
	//		_g = g;
	//		_b = b;
	//		_a = a;
	//	}
	//
	//	public SerializableColor(Color color)
	//	{
	//		_r = color.r;
	//		_g = color.g;
	//		_b = color.b;
	//		_a = color.a;
	//	}
	//}

	/// <summary>
	/// Since unity doesn't flag the Color as serializable, we
	/// need to create our own version. This one will automatically convert
	/// between Color and SerializableColor
	/// </summary>
	[System.Serializable]
	public struct SeriColor : ISerializable, PokemonEssentials.Interface.IColor
	{
		/// <summary>
		/// Serializable Color
		/// </summary>
		public UnityEngine.Color Color { get; set; }
		//public UnityEngine.Color Color
		//{
		//	get
		//	{
		//		return new UnityEngine.Color(r, g, b, a);
		//	}
		//	set
		//	{
		//		r = value.r;
		//		g = value.g;
		//		b = value.b;
		//		a = value.a;
		//	}
		//}

		public float r
		{
			get { return Color.r; }
			set { Color = Color.WithR(value); }
		}

		public float g
		{
			get { return Color.g; }
			set { Color = Color.WithG(value); }
		}

		public float b
		{
			get { return Color.b; }
			set { Color = Color.WithB(value); }
		}

		public float a
		{
			get { return Color.a; }
			set { Color = Color.WithA(value); }
		}

		float IColor.red { get { return r; } }

		float IColor.green { get { return g; } }

		float IColor.blue { get { return b; } }

		float IColor.alpha { get { return a; } set { a = value; } }

		//public SeriColor()
		//{
		//	Color = new UnityEngine.Color();
		//	// (Optional) Default to white with an empty initialization
		//	r = 1f;
		//	g = 1f;
		//	b = 1f;
		//	a = 1f;
		//}

		public SeriColor(UnityEngine.Color color)
		{
			//_r = color.r;
			//_g = color.g;
			//_b = color.b;
			//_a = color.a;
			Color = color;
		}

		public SeriColor(float r, float g, float b, float a = 0f)
		{
			Color = new UnityEngine.Color();
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		public void set(float red, float green, float blue, float alpha)
		{
			Color = new UnityEngine.Color(red, green, blue, alpha);
		}

		/// <summary>
		/// The special constructor is used to deserialize values. 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public SeriColor(SerializationInfo info, StreamingContext context)
		{
			// Reset the property value using the GetValue method.
			Color = new UnityEngine.Color(
				(float)info.GetValue("r", typeof(float)),
				(float)info.GetValue("g", typeof(float)),
				(float)info.GetValue("b", typeof(float)),
				(float)info.GetValue("a", typeof(float))
			);
		}


		public static implicit operator UnityEngine.Color(SeriColor c)
		{
			return c.Color;
		}

		public static implicit operator SeriColor(UnityEngine.Color c)
		{
			return new SeriColor(c);
		}

		//public static UnityEngine.Color operator *(SeriColor c, float f)
		//{
		//	return c.Color * f;
		//}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			// Use the AddValue method to specify serialized values.
			info.AddValue("r", Color.r, typeof(float));
			info.AddValue("g", Color.g, typeof(float));
			info.AddValue("b", Color.b, typeof(float));
			info.AddValue("a", Color.a, typeof(float));
		}
		public override string ToString()
		{
			return string.Format("[r={0}, g={1}, b={2}, a={3}]", r, g, b, a);
		}
	}

	/// <summary>
	/// Custom extension for Color.
	/// Used with replacing a value of a single hue
	/// </summary>
	public static partial class Utex
	{
		public static UnityEngine.Color WithR(this UnityEngine.Color c, float r)
		{
			return new UnityEngine.Color(r, c.g, c.b, c.a);
		}

		public static UnityEngine.Color WithG(this UnityEngine.Color c, float g)
		{
			return new UnityEngine.Color(c.r, g, c.b, c.a);
		}

		public static UnityEngine.Color WithB(this UnityEngine.Color c, float b)
		{
			return new UnityEngine.Color(c.r, c.g, b, c.a);
		}

		public static UnityEngine.Color WithA(this UnityEngine.Color c, float a)
		{
			return new UnityEngine.Color(c.r, c.g, c.b, a);
		}
	}
}