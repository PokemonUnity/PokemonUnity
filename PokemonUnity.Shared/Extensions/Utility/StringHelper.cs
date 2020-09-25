using System;

public static class StringHelper
{
	//private StringHelper()
	//{
	//	throw new InvalidOperationException("Cannot initialize static class.");
	//}

	public static string DecSeparator { get { return System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator; } }

	public static char GetChar(int charCode)
	{
		return char.ConvertFromUtf32(charCode)[0];
	}

	public static char Tab
	{
		get
		{
			return GetChar(9);
		}
	}

	public static char LineFeed
	{
		get
		{
			return GetChar(10);
		}
	}

	public static string CrLf
	{
		get
		{
			return string.Format("{0}{1}", GetChar(13), LineFeed);
		}
	}

	//public static bool IsNumeric(object obj)
	//{
	//    if (obj is string)
	//        return IsNumeric(System.Convert.ToString(obj));
	//
	//    return Microsoft.VisualBasic.IsNumeric(obj);
	//}

	public static bool IsNumeric(string str)
	{
		decimal discard;
		return decimal.TryParse(str, System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.CurrentInfo, out discard);
	}

	public static string ReplaceAt(this string text, int index, char newChar)
	{
		if (text == null) return text;
		char[] chars = text.ToCharArray();
		chars[index] = newChar;
		return new string(chars);
	}

	public static string ReplaceRegex(this string text, string replace, string input)
	{
		if (text == null) return text;
		return System.Text.RegularExpressions.Regex.Replace(text, replace, input, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
	}
}