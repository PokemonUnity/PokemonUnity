using System;

namespace PokemonUnity
{
	/// <summary>
	/// Static class for logging messages to console and file
	/// </summary>
	/// <remarks>
	/// Singleton for <see cref="IDebugger"/>
	/// </remarks>
	public static class GameDebug //: IDebugger
	{
		private static Debugger debugger = new Debugger();

		public static void Init(string logfilePath, string logBaseName)
		{
			debugger.Init(logfilePath, logBaseName);
		}

		public static void Shutdown()
		{
			debugger.Shutdown();
		}

		public static void Log(object sender, string message)
		{
			if (Core.DEBUG) debugger.OnLogMessageDelegate(sender, new OnDebugEventArgs { Message = message, Error = null });
		}
		public static void Log(string message)
		{
			Log(sender: null, message);
		}

		public static void Log(string message, params object[] param)
		{
			Log(string.Format(message, param));
		}

		public static void LogWarning(object sender, string message)
		{
			debugger.OnLogMessageDelegate(sender, new OnDebugEventArgs { Message = message, Error = false });
		}
		public static void LogWarning(string message)
		{
			LogWarning(sender: null, message);
		}

		public static void LogError(object sender, string message)
		{
			debugger.OnLogMessageDelegate(sender, new OnDebugEventArgs { Message = message, Error = true });
		}

		public static void LogError(string message)
		{
			LogError(sender: null, message);
		}
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
	///    Only used for things that should not be logged. Typically responses to user commands. Only shown on Console.
	/// </para>
	/// </summary>
	public class Debugger : IDebugger
	{
		public event EventHandler<OnDebugEventArgs> OnLog;
		//private System.IO.StreamWriter logFile = null;
		private string logFile = null;
		/// <summary>
		/// Determines whether or not to store Debug Log to a file, or display only
		/// </summary>
		private bool DebugToFile { get { return logFile != null; } }
		//private readonly string filename = GetTempPath() + "TranslationLog.txt";
		//private string GetTempPath()
		//{
		//	string path = System.Environment.GetEnvironmentVariable("TEMP");
		//	if (!path.EndsWith("\\")) path += "\\";
		//	return path;
		//}

		public void Init(string logfilePath, string logBaseName)
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

		public void Shutdown()
		{
			//if (DebugToFile)
			//	logFile.Close();
			logFile = null;
		}

		public void Log(object sender, string message)
		{
			if (Core.DEBUG) OnLogMessageDelegate(sender, new OnDebugEventArgs { Message = message, Error = null });
			//Console.WriteLine("Log: " + message);
			if (DebugToFile)
				_Log(message);
		}

		public void Log(string message, params object[] param)
		{
			Log(string.Format(message, param));
		}

		private void _Log(string message)
		{
			//Console.Write(message); //UnityEngine.Time.frameCount + ": " +
			//if (logFile != null)
			//	logFile.WriteLine("[LOG] " + message + "\n");
				LogMessageToFile("[LOG] " + message + "\n");
		}

		public void LogWarning(object sender, string message)
		{
			OnLogMessageDelegate(sender, new OnDebugEventArgs { Message = message, Error = false });
			if (DebugToFile)
				_LogWarning(message);
		}

		private void _LogWarning(string message)
		{
			//Console.Write("[WARN] " + message); //UnityEngine.Time.frameCount +
			//if (logFile != null)
			//	logFile.WriteLine("[WARN] " + message + "\n");
				LogMessageToFile("[WARN] " + message + "\n");
		}

		public void LogError(object sender, string message)
		{
			OnLogMessageDelegate(sender, new OnDebugEventArgs { Message = message, Error = true });
			//Console.WriteLine("Log Error: " + message);
			if (DebugToFile)
				_LogError(message);
		}

		private void _LogError(string message)
		{
			//Console.Write("[ERR] " + message); //UnityEngine.Time.frameCount +
			//if (logFile != null)
			//	logFile.WriteLine("[ERR] " + message + "\n");
				LogMessageToFile("[ERR] " + message + "\n");
		}

		private void LogMessageToFile(string msg)
		{
			using (System.IO.StreamWriter sw = System.IO.File.AppendText(logFile))
				try
				{
					msg = System.String.Format("{0:G}: {1}", System.DateTime.Now, msg);
					sw.WriteLine(msg);
				}
				catch (Exception ex)
				{
					//ToDo: May be recursive loop, if error is thrown while trying to log error
					LogError(this, ex.Message);
				}
			//finally
			//{
			//	sw.Close();
			//}
		}

		public void OnLogMessageDelegate(object sender, OnDebugEventArgs args) {
			if (OnLog != null) OnLog.Invoke(sender, args);
		}

		void IDebugger.LogWarning(string message)
		{
			LogWarning(null, message);
		}

		void IDebugger.LogError(string message)
		{
			LogError(null, message);
		}
	}
}