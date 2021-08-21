using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PokemonUnity.Utility
{
	static class Extensions
	{
		public static T ToEnum<T>(this string value, T defaultValue)
		{
			if (string.IsNullOrEmpty(value))
			{
				return defaultValue;
			}

			//T result;
			//return Enum.TryParse<T>(value, true, out result) ? result : defaultValue;
			//return Convert.ChangeType((Enum.Parse(typeof(T), value, true) ?? defaultValue), typeof(T));
			return (T)((Enum.Parse(typeof(T), value, true) ?? defaultValue));
		}

		public static void Move<T>(this List<T> l, int moveItemIndex, int destinationIndex)
		{
			T i = l[moveItemIndex];
			l.RemoveAt(moveItemIndex);
			l.Insert(destinationIndex, i);
		}

		//public static Texture2D Copy(this Texture2D t)
		//{
		//	//Texture2D newT = new Texture2D(Core.GraphicsDevice, t.width, t.height);
		//	//
		//	//Color[] cArr = new Color[newT.width * newT.height - 1 + 1];
		//	//t.GetData(cArr);
		//	//
		//	//newT.SetData(cArr);
		//	//
		//	//return newT;
		//	return t;
		//}

		public static string GetSplit(this string fullString, int valueIndex, string seperator)
		{
			if (!fullString.Contains(seperator))
				return fullString;
			else
			{
				string[] parts = fullString.Split(new[] { seperator }, StringSplitOptions.None);
				if (parts.Count() - 1 >= valueIndex)
					return parts[valueIndex];
				else
					return fullString;
			}
		}

		public static string GetSplit(this string fullString, int valueIndex)
		{
			return GetSplit(fullString, valueIndex, ",");
		}

		public static int CountSplits(this string fullString, string seperator)
		{
			int i = 0;
			if (fullString.Contains(seperator))
			{
				foreach (char c in fullString)
				{
					if (c == System.Convert.ToChar(seperator))
						i += 1;
				}
			}
			return i + 1;
		}

		public static int CountSplits(this string fullString)
		{
			return CountSplits(fullString, ",");
		}

		public static int CountSeperators(this string fullstring, string seperator)
		{
			int i = 0;
			if (fullstring.Contains(seperator))
			{
				foreach (char c in fullstring)
				{
					if (c == System.Convert.ToChar(seperator))
						i += 1;
				}
			}
			return i;
		}

		public static string ArrayToString<T>(this T[] Array, bool NewLine = false)
		{
			if (NewLine)
			{
				string s = "";
				for (var i = 0; i <= Array.Length - 1; i++)
				{
					if (i != 0)
						s += Environment.NewLine;

					s += Array[i].ToString();
				}
				return s;
			}
			else
			{
				string s = "{";
				for (var i = 0; i <= Array.Length - 1; i++)
				{
					if (i != 0)
						s += ",";

					s += Array[i].ToString();
				}
				s += "}";
				return s;
			}
		}

		public static string ToNumberString(this bool @bool)
		{
			if (@bool)
				return "1";
			else
				return "0";
		}

		public static string[] ToArray(this string s, string Seperator)
		{
			return s.Replace(Environment.NewLine, Seperator).Split(System.Convert.ToChar(Seperator));
		}

		public static int ToPositive(this int i)
		{
			if (i < 0)
				i *= -1;
			return i;
		}

		public static int Clamp(this int i, int min, int max)
		{
			if (i > max)
				i = max;
			if (i < min)
				i = min;
			return i;
		}

		public static int Clamp(this float f, int min, int max)
		{
			if (f > max)
				return max;
			if (f < min)
				return min;
			return (int)f;
		}

		public static float Clamp(this float s, float min, float max)
		{
			if (s > max)
				s = max;
			if (s < min)
				s = min;
			return s;
		}

		public static decimal Clamp(this decimal d, decimal min, decimal max)
		{
			if (d > max)
				d = max;
			if (d < min)
				d = min;
			return d;
		}

		public static double Clamp(this double d, double min, double max)
		{
			if (d > max)
				d = max;
			if (d < min)
				d = min;
			return d;
		}

		//public static string CropStringToWidth(this string s, SpriteFont font, float scale, int width)
		//{
		//    string fulltext = s;

		//    if ((font.MeasureString(fulltext).X * scale) <= width)
		//        return fulltext;
		//    else if (!fulltext.Contains(" "))
		//    {
		//        string newText = "";
		//        while (fulltext.Length > 0)
		//        {
		//            if ((font.MeasureString(newText + fulltext[0].ToString()).X * scale) > width)
		//            {
		//                newText += Environment.NewLine;
		//                newText += fulltext[0].ToString();
		//                fulltext.Remove(0, 1);
		//            }
		//            else
		//            {
		//                newText += fulltext[0].ToString();
		//                fulltext.Remove(0, 1);
		//            }
		//        }
		//        return newText;
		//    }

		//    string output = "";
		//    string currentLine = "";
		//    string currentWord = "";

		//    while (fulltext.Length > 0)
		//    {
		//        if (fulltext.StartsWith(Environment.NewLine))
		//        {
		//            if (currentLine != "")
		//                currentLine += " ";
		//            currentLine += currentWord;
		//            output += currentLine + Environment.NewLine;
		//            currentLine = "";
		//            currentWord = "";
		//            fulltext = fulltext.Remove(0, 2);
		//        }
		//        else if (fulltext.StartsWith(" "))
		//        {
		//            if (currentLine != "")
		//                currentLine += " ";
		//            currentLine += currentWord;
		//            currentWord = "";
		//            fulltext = fulltext.Remove(0, 1);
		//        }
		//        else
		//        {
		//            currentWord += fulltext[0];
		//            if ((font.MeasureString(currentLine + currentWord).X * scale) >= width)
		//            {
		//                if (currentLine == "")
		//                {
		//                    output += currentWord + Environment.NewLine;
		//                    currentWord = "";
		//                    currentLine = "";
		//                }
		//                else
		//                {
		//                    output += currentLine + Environment.NewLine;
		//                    currentLine = "";
		//                }
		//            }
		//            fulltext = fulltext.Remove(0, 1);
		//        }
		//    }

		//    if (currentWord != "")
		//    {
		//        if (currentLine != "")
		//            currentLine += " ";
		//        currentLine += currentWord;
		//    }
		//    if (currentLine != "")
		//        output += currentLine;

		//    return output;
		//}

		//public static string CropStringToWidth(this string s, SpriteFont font, int width)
		//{
		//    return CropStringToWidth(s, font, 1.0f, width);
		//}

		//public static Color ToColor(this Vector3 v)
		//{
		//    return new Color(System.Convert.ToInt32(v.x * 255), System.Convert.ToInt32(v.y * 255), System.Convert.ToInt32(v.z * 255));
		//}
		//
		//public static Texture2D ReplaceColors(this Texture2D t, Color[] InputColors, Color[] OutputColors)
		//{
		//	//Texture2D newTexture = new Texture2D(Core.GraphicsDevice, t.width, t.height);
		//	//
		//	//if (InputColors.Length == OutputColors.Length & InputColors.Length > 0)
		//	//{
		//	//    Color[] Data = new Color[t.width * t.height - 1 + 1];
		//	//    List<Color> newData = new List<Color>();
		//	//    t.GetData(0, null/* TODO Change to default(_) if this is not a reference type */, Data, 0, t.width * t.height);
		//	//
		//	//    for (var i = 0; i <= Data.Length - 1; i++)
		//	//    {
		//	//        Color c = Data[i];
		//	//        if (InputColors.Contains(c))
		//	//        {
		//	//            for (var iC = 0; iC <= InputColors.Length - 1; iC++)
		//	//            {
		//	//                if (InputColors[iC] == c)
		//	//                {
		//	//                    c = OutputColors[iC];
		//	//                    break;
		//	//                }
		//	//            }
		//	//        }
		//	//        newData.Add(c);
		//	//    }
		//	//
		//	//    newTexture.SetData(newData.ToArray());
		//	//}
		//	//else
		//	//    newTexture = t;
		//	//
		//	//return newTexture;
		//	return t;
		//}

		public static double xRoot(this int root, double number)
		{
			double powered = 1 / (double)root;

			double returnNumber = Math.Pow(number, powered);

			return returnNumber;
		}

		public static string[] SplitAtNewline(this string s)
		{
			if (!s.Contains("§"))
				return s.Replace(StringHelper.CrLf, "§").Replace(StringHelper.LineFeed, '§').Split(System.Convert.ToChar("§"));
			else
			{
				List<string> Data = new List<string>();

				if (s == "")
					return (new List<string>()).ToArray();

				int i = 0;
				while (s != "" & i < s.Length)
				{
					if (!s.Substring(i).StartsWith(StringHelper.CrLf) | !s.Substring(i).StartsWith(StringHelper.LineFeed.ToString()))
						i += 1;
					else
					{
						Data.Add(s.Substring(0, i));
						i += 2;
						s = s.Remove(0, i);
						i = 0;
					}
				}

				Data.Add(s.Substring(0, i));

				return Data.ToArray();
			}
		}

		public static string[] Split(this string s, string splitAt)
		{
			return s.Split(new[] { splitAt }, System.StringSplitOptions.None);
		}

		public static List<T> Randomize<T>(this List<T> L)
		{
			return Randomize<T>(L.ToArray()).ToList();
		}

		public static T[] Randomize<T>(this T[] L)
		{
			T temp;
			int indexRand;
			int indexLast = L.Count() - 1;
			for (int index = 0; index <= indexLast; index++)
			{
				indexRand = PokemonUnity.Core.Rand.Next(index, indexLast);
				temp = L[indexRand];
				L[indexRand] = L[index];
				L[index] = temp;
			}
			return L;
		}

		public static int GetRandomChance(List<int> chances)
		{
			int totalNumber = 0;
			foreach (int c in chances)
				totalNumber += c;

			int r = PokemonUnity.Core.Rand.Next(0, totalNumber + 1);

			int x = 0;
			for (var i = 0; i <= chances.Count - 1; i++)
			{
				x += chances[i];
				if (r <= x)
					return i;
			}

			return -1;
		}

		//public static Vector2 ProjectPoint(this Vector3 v, Matrix View, Matrix Projection)
		//{
		//    Matrix mat = Matrix.Identity * View * Projection;
		//
		//    Vector4 v4 = Vector4.Transform(v, mat);
		//
		//    return new Vector2(System.Convert.ToSingle(((v4.x / (double)v4.w + 1) * (windowSize.width / (double)2))), System.Convert.ToSingle(((1 - v4.y / (double)v4.w) * (windowSize.height / (double)2))));
		//}

		public static int ToInteger(this float s)
		{
			return System.Convert.ToInt32(s);
		}

		///// <summary>
		///// Inverts the Color.
		///// </summary>
		//public static Color Invert(this Color c)
		//{
		//    return new Color(255 - c.r, 255 - c.g, 255 - c.b, c.a);
		//}

		public static string ReplaceDecSeparator(this string s)
		{
			return s.Replace(StringHelper.DecSeparator, ".");
		}

		public static string InsertDecSeparator(this string s)
		{
			return s.Replace(".", StringHelper.DecSeparator);
		}

		///// <summary>
		///// Converts a System.Drawing.Color into a Xna.Framework.Color.
		///// </summary>
		//public static Color ToXNA(this System.Drawing.Color c)
		//{
		//    return new Color(c.R, c.G, c.B, c.A);
		//}
	}
}