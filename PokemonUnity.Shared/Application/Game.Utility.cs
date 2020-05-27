using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Saving;
using System.IO;
//using System.Security.Cryptography;

namespace PokemonUnity
{
	/// <summary>
	/// Variables that are stored when game is saved, and other temp values used for gameplay.
	/// This class should be called once, when the game boots-up.
	/// During boot-up, game will check directory for save files and load data.
	/// Game class will overwrite all the other class default values when player triggers a load state.
	/// </summary>
	public partial class Game 
	{
		//public static string GetFileGuid(string filepath)
		//{
		//	using (var md5 = MD5.Create())
		//	{
		//		using (var stream = File.OpenRead(filepath))
		//		{
		//			return md5.ComputeHash(stream);
		//		}
		//	}
		//}
		//public static string GetFileGuid(FileStream stream)
		//{
		//	using (var md5 = MD5.Create())
		//	{
		//		using (stream)
		//		{
		//			return md5.ComputeHash(stream);
		//		}
		//	}
		//}
		//public static string CalculateMD5(string filename)
		//{
		//	using (var md5 = MD5.Create())
		//	{
		//		using (var stream = File.OpenRead(filename))
		//		{
		//			var hash = md5.ComputeHash(stream);
		//			return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
		//		}
		//	}
		//}

		/**/
		//On Project start...
		//all XML files are opened, and locked
		//one by one, they're scanned and checked to see if they're up to date (match latest with compile time info)
		//if not the latest, perform an update by fetching and downloading latest info...
		//else continue, and load the XML data into the variables
		public static event EventHandler<OnLoadEventArgs> OnLoad;
		public class OnLoadEventArgs : EventArgs
		{
			public int Check;
			public int Total;
			public int Piece;
			public int TotalPieces;
		}
		static System.Data.SQLite.SQLiteConnection con  = new System.Data.SQLite.SQLiteConnection(@"Data Source=..\..\..\\veekun-pokedex.sqlite");

		public static string LockFileStream (string filepath)
		{
			UnicodeEncoding uniEncoding = new UnicodeEncoding();
			//int recordNumber = 13;
			//int byteCount = uniEncoding.GetByteCount(recordNumber.ToString());
			string tempString;

			using (FileStream fileStream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
			{
				// Write the original file data.
				//if (fileStream.Length == 0) //Get GUID
				//{
				//	fileStream.Write(uniEncoding.GetBytes(tempString),
				//		0, uniEncoding.GetByteCount(tempString));
				//}

				byte[] readText = new byte[fileStream.Length];

				//if ((tempString = Console.ReadLine()).Length == 0)
				//{
				//	break;
				//}
				try
				{
					fileStream.Seek(0, SeekOrigin.Begin);
					fileStream.Read(
						readText, 0, (int)fileStream.Length);
					tempString = new String(
						uniEncoding.GetChars(
						readText, 0, readText.Length));
				}

				// Catch the IOException generated if the 
				// specified part of the file is locked.
				//catch (IOException e)
				//{
				//	Console.WriteLine("{0}: The read " +
				//		"operation could not be performed " +
				//		"because the specified part of the " +
				//		"file is locked.",
				//		e.GetType().Name);
				//}
				finally
				{
					//xmlString = tempString;
				}
				return tempString;
			}
		}
	}
}