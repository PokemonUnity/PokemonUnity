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
//using System.Security.Cryptography.MD5;

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

		//public void SQLiteSample()
		//{
		//	DataTable t = new DataTable();
		//	// Open connection
		//	using (IDbConnection dbcon = new System.Data.SqlClient.SqlConnection(connectionString))
		//	{
		//		using (IDbCommand dbcmd = dbcon.CreateCommand())
		//		{
		//			dbcmd.CommandType = CommandType.StoredProcedure;
		//			dbcmd.CommandText = "getTrainerPokemon";//procedureName;
		//			foreach (SqlParameter parameter in parameters)
		//			{
		//				dbcmd.Parameters.Add(parameter);
		//			}
		//			dbcon.Open();
		//			result = dbcmd.ExecuteScalar();
		//			dbcon.Close();
		//		}
		//		using (SqlConnection c = new System.Data.SqlClient.SqlConnection(ConnectionString))
		//		{
		//			c.Open();
		//			// 2
		//			// Create new DataAdapter
		//			using (SqlDataAdapter a = new SqlDataAdapter(
		//				"SELECT * FROM EmployeeIDs", c))
		//			{
		//				// 3
		//				// Use DataAdapter to fill DataTable
		//				a.Fill(t);
		//				// 4
		//				// Render data onto the screen
		//				// dataGridView1.DataSource = t; // <-- From your designer
		//			}
		//		}
		//	}
		//}

		/// <summary>
		/// </summary>
		/// <param name="sqlQuery">SQL query string</param>
		/// <returns>Returns query results from database as string array</returns>
		public static List<string[]> GetArrayFromSQL(string sqlQuery/*, System.Type type*/)
		{
				//string test = @"..\..\..\\"// + Path.DirectorySeparatorChar 
				//	+ "veekun-pokedex.txt";
				////File.Create(test);
				//LockFileStream(test);
				//return null;
			try
			{
				//Step 1: Get Provider
				//DbProviderFactory f = SQLiteFactory.Instance;
				//Step 2: Create a connection
				//IDbConnection con;// = f.CreateConnection();
				//SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder();
				//builder.FailIfMissing = true;
				//builder.Version = 3;
				//builder.LegacyFormat = true;
				//builder.Pooling = true;
				string ConnectionString = "Data Source="//"URI=file:" //"Driver=SQLite3 ODBC Driver; Database="
				//builder.Uri = "file:"
				//builder.DataSource = "Data Source="
				//builder.ConnectionString = "Driver=SQLite3 ODBC Driver; Database="
					+ @"..\..\..\\" //+ Path.DirectorySeparatorChar
					//+ System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase),"veekun-pokedex.sqlite");
					//+ @"C:\Users\Username\Documents\Pokemon\Pokemon Unity\"//Assets\Resources\Databases"
					//+ Application.dataPath + Path.DirectorySeparatorChar
					//+ "Assets" + Path.DirectorySeparatorChar
					//+ "Scripts" + Path.DirectorySeparatorChar
					//+ "Resources" + Path.DirectorySeparatorChar
					//+ "Databases" + Path.DirectorySeparatorChar
					+ "veekun-pokedex.sqlite;";//Version=3;";// "Data Source=C:\\folder\
					//+ "new_pokedex.db;Version=3;";// "Data Source=C:\\folder\\HelloWorld.sqlite;Version=3;New=False;Compress=True";
				//System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(ConnectionString);//ConnectionString
				//con.Open();

				//using (con)
				//{
					//Step 3: Running a Command
					System.Data.SQLite.SQLiteCommand stmt = con.CreateCommand();

					#region DataReader
					stmt.CommandText = sqlQuery;
					System.Data.SQLite.SQLiteDataReader reader = stmt.ExecuteReader();

					//Step 4: Read the results
					using (reader)
					{
						//Dictionary<int, string[]> pairs = new Dictionary<int, string[]>();
						////int[] y = Enum.GetValues(type);
						//for (int n = 1; n <= Enum.GetValues(type).Length; n++)
						//{
						//}
						int x = reader.FieldCount;
						List<string[]> list = new List<string[]>();
						while (reader.Read())
						{
							string[] a = new string[x];
							for (int i = 0; i < x; i++)
							{
								a[i] = (string)reader[i].ToString();
							}
							list.Add(a);
						}
						return list;
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
					//return true;
				//}
				//con.Close();
				//};
			}
			catch (System.Data.SqlClient.SqlException e)
			{
				//Debug.Log("SQL Exception Message:" + e.Message);
				//Debug.Log("SQL Exception Code:" + e.ErrorCode.ToString());
				//Debug.Log("SQL Exception Help:" + e.HelpLink);
				//return null;
				throw;
			}
		}
	}
}