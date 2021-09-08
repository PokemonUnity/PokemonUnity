using System;

namespace PokemonUnity
{
	public interface IGameDebug
	{
		event EventHandler<GameDebug.OnDebugEventArgs> OnDebug;

		/// <summary>
		/// Create and open data stream to file used for storing log entries.
		/// </summary>
		/// <param name="logfilePath">File Directory</param>
		/// <param name="logBaseName">Name of the File</param>
		void Init(string logfilePath, string logBaseName);
		/// <summary>
		/// Silently write into our log file
		/// </summary>
		/// <param name="message"></param>
		void Log(string message);
		/// <summary>
		/// Pauses and interrupts game to be displayed to user. 
		/// Typically responses to user commands.
		/// </summary>
		/// <param name="message"></param>
		void LogError(string message);
		/// <summary>
		/// Displays to user, but doesnt pause or interrupt game. 
		/// Typically flashes on screen and goes away.
		/// </summary>
		/// <param name="message"></param>
		void LogWarning(string message);
		/// <summary>
		/// Save and close data stream to file.
		/// </summary>
		void Shutdown();
	}

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
	public static class GameDebug
	{
		private static string Debug
		{
			set
			{
//#if !DEBUG
//				UnityEngine.Debug.Log(value);
//#endif
				if (Core.DEBUG && OnDebug != null) OnDebug.Invoke(Game.GameData, new OnDebugEventArgs { Message = value, Error = null });
			}
		}
		private static string DebugWarning
		{
			set
			{
//#if !DEBUG
//				UnityEngine.Debug.LogWarning(value);
//#endif
				if (Core.DEBUG && OnDebug != null) OnDebug.Invoke(Game.GameData, new OnDebugEventArgs { Message = value, Error = false });
			}
		}
		//ToDo: Errors should be known whether in debug or production?
		private static string DebugError
		{
			set
			{
//#if !DEBUG
//				UnityEngine.Debug.LogError(value);
//#endif
				if (OnDebug != null) OnDebug.Invoke(Game.GameData, new OnDebugEventArgs { Message = value, Error = true });
			}
		}
		static System.IO.StreamWriter logFile = null;
		/// <summary>
		/// Determines whether or not to store Debug Log to a file, or display only
		/// </summary>
		static bool DebugToFile { get { return logFile != null; } }
		public static event EventHandler<OnDebugEventArgs> OnDebug;
		public class OnDebugEventArgs : EventArgs
		{
			public static readonly int EventId = typeof(OnDebugEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			/// <summary>
			/// If true, pause/stop game to display error message on screen.
			/// </summary>
			public bool? Error { get; set; }
			public string Message { get; set; }
		}
		public static void Init(string logfilePath, string logBaseName)
		{
#if !DEBUG
				forwardToDebug = UnityEngine.Application.isEditor;
				UnityEngine.Application.logMessageReceived += LogCallback;
#endif

			// Try creating logName; attempt a number of suffixxes
			string name = "";
			for (var i = 0; i < 10; i++)
			{
				name = logBaseName + (i == 0 ? "" : "_" + i) + ".log";
				try
				{
					logFile = System.IO.File.CreateText(logfilePath + "/" + name);
					logFile.AutoFlush = true;
					break;
				}
				catch
				{
					name = "<none>";
				}
			}
			GameDebug.Log("GameDebug initialized. Logging to " + logfilePath + "/" + name);
		}

		public static void Shutdown()
		{
#if !DEBUG
				UnityEngine.Application.logMessageReceived -= LogCallback;
#endif
			if (DebugToFile)
				logFile.Close();
			logFile = null;
		}

		//static void LogCallback(string message, string stack, UnityEngine.LogType logtype)
		//{
		//	switch (logtype)
		//	{
		//		default:
		//		case UnityEngine.LogType.Log:
		//			GameDebug._Log(message);
		//			break;
		//		case UnityEngine.LogType.Warning:
		//			GameDebug._LogWarning(message);
		//			break;
		//		case UnityEngine.LogType.Error:
		//			GameDebug._LogError(message);
		//			break;
		//	}
		//}

		public static void Log(string message)
		{
			Debug = message;
			if (DebugToFile)
				_Log(message);
		}

		static void _Log(string message)
		{
			//Console.Write(message); //UnityEngine.Time.frameCount + ": " + 
			//if (logFile != null)
				logFile.WriteLine(message + "\n"); //UnityEngine.Time.frameCount + ": " + 
		}

		public static void LogError(string message)
		{
			DebugError = message;
			if (DebugToFile)
				_LogError(message);
		}

		static void _LogError(string message)
		{
			//Console.Write("[ERR] " + message); //UnityEngine.Time.frameCount + 
			//if (logFile != null)
				logFile.WriteLine("[ERR] " + message + "\n");
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
				logFile.WriteLine("[WARN] " + message + "\n");
		}
	}
}