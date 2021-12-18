using System;

namespace PokemonUnity
{
	/// <summary>
	/// Logging of messages
	/// <para>
	/// There are three different types of messages:
	/// </para><para>
	/// Debug.Log/Warn/Error coming from unity (or code, e.g. packages, not using GameDebug) =>
	///    These get caught here and sent onto the console and into our log file
	///    </para><para>
	/// GameDebug.Log/Warn/Error coming from game =>
	///    These gets sent onto the console and into our log file.
	///    *IF* we are in editor, they are also sent to Debug.* so they show up in editor Console window
	///    </para><para>
	/// Console.Write =>
	///    Only used for things that should not be logged. Typically reponses to user commands. Only shown on Console.
	/// </para>
	/// </summary>
	public static class GameDebug //: IDebugger
	{
		public static event EventHandler<OnDebugEventArgs> OnLog;
		private static string Debug
		{
			set
			{
				if (Core.DEBUG && OnLog != null) OnLog.Invoke(null, new OnDebugEventArgs { Message = value, Error = null });
			}
		}
		private static string DebugWarning
		{
			set
			{
				if (Core.DEBUG && OnLog != null) OnLog.Invoke(null, new OnDebugEventArgs { Message = value, Error = false });
			}
		}
		//ToDo: Errors should be known whether in debug or production?
		private static string DebugError
		{
			set
			{
				if (OnLog != null) OnLog.Invoke(null, new OnDebugEventArgs { Message = value, Error = true });
			}
		}
		//private static System.IO.StreamWriter logFile = null;
		private static string logFile = null;
		/// <summary>
		/// Determines whether or not to store Debug Log to a file, or display only
		/// </summary>
		private static bool DebugToFile { get { return logFile != null; } }
		//private static readonly string filename = GetTempPath() + "TranslationLog.txt";
		//private static string GetTempPath()
		//{
		//	string path = System.Environment.GetEnvironmentVariable("TEMP");
		//	if (!path.EndsWith("\\")) path += "\\";
		//	return path;
		//}

		public static void Init(string logfilePath, string logBaseName)
		{
			// Try creating logName; attempt a number of suffixes
			string name = "";
			for (int i = 0; ; i++)
			{
				name = logBaseName + (i == 0 ? "" : "_" + i) + ".log";
				//try
				//{
					//logFile = System.IO.File.CreateText(logfilePath + "/" + name);
					//logFile = System.IO.File.OpenWrite(logfilePath + "/" + name);
					//logFile.AutoFlush = true;
					logFile = logfilePath + "/" + name;
					break;
				//}
				//catch
				//{
				//	name = "<none>";
				//}
			}
			Log("GameDebug initialized. Logging to " + logfilePath + "/" + name);
		}

		//public static void Shutdown()
		//{
		//	if (DebugToFile)
		//		logFile.Close();
		//	logFile = null;
		//}

		public static void Log(string message)
		{
			Debug = message;
			//Console.WriteLine("Log: " + message);
			if (DebugToFile)
				_Log(message);
		}

		public static void Log(string message, params object[] param)
		{
			//Debug = message;
			//Log(Game._INTL(message, param));
			Log(string.Format(message, param));
		}

		static void _Log(string message)
		{
			//Console.Write(message); //UnityEngine.Time.frameCount + ": " + 
			//if (logFile != null)
			//	logFile.WriteLine("[LOG] " + message + "\n");
				LogMessageToFile("[LOG] " + message + "\n"); 
		}

		public static void LogWarning(string message)
		{
			DebugWarning = message;
			if (DebugToFile)
				_LogWarning(message);
		}

		static void _LogWarning(string message)
		{
			//Console.Write("[WARN] " + message); //UnityEngine.Time.frameCount + 
			//if (logFile != null)
			//	logFile.WriteLine("[WARN] " + message + "\n");
				LogMessageToFile("[WARN] " + message + "\n");
		}

		public static void LogError(string message)
		{
			DebugError = message;
			//Console.WriteLine("Log Error: " + message);
			if (DebugToFile)
				_LogError(message);
		}

		static void _LogError(string message)
		{
			//Console.Write("[ERR] " + message); //UnityEngine.Time.frameCount + 
			//if (logFile != null)
			//	logFile.WriteLine("[ERR] " + message + "\n");
				LogMessageToFile("[ERR] " + message + "\n");
		}

		public static void LogMessageToFile(string msg)
		{
			using (System.IO.StreamWriter sw = System.IO.File.AppendText(logFile))
				try
				{
					msg = System.String.Format("{0:G}: {1}", System.DateTime.Now, msg);
					sw.WriteLine(msg);
				}
				catch(Exception ex)
				{
					DebugError = ex.Message;
				}
				//finally
				//{
				//	sw.Close();
				//}
		}

		public static void LogMessageFile(string msg)
		{
			msg = System.String.Format("{0:G}: {1}{2}", System.DateTime.Now, msg, System.Environment.NewLine);
			System.IO.File.AppendAllText(logFile, msg);
		}
	}
}