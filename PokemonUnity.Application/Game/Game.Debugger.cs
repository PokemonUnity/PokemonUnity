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
		private static Debugger debugger = Debugger.Instance;

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

		public static void LogDebug(string message)
		{
			debugger.LogDebug(sender: null, message);
		}

		public static void LogDebug(string message, params object[] param)
		{
			LogDebug(string.Format(message, param));
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
	/// ToDo: Introduce Serilog.Dll Library to project
	public class Debugger : IDebugger
	{
		public event EventHandler<OnDebugEventArgs> OnLog;
		public static Debugger Instance { get { return instance; } }
		private static readonly Debugger instance = new Debugger();

		//private Serilog.ILogger serilogLogger;
		private bool useSerilog = false;
		private string logFilePath;

		private Debugger() { Core.Logger = this; }

		public void Init(string logFilePath, string logBaseName, bool useSerilog = false)
		{
			this.logFilePath = System.IO.Path.Combine(logFilePath, logBaseName + ".log");
			this.useSerilog = useSerilog;
			if (useSerilog)
			{
				//serilogLogger = new LoggerConfiguration()
				//	.WriteTo.File(this.logFilePath)
				//	.CreateLogger();
			}
			Log(this, "Debugger initialized. Logging to " + this.logFilePath);
		}

		public void Shutdown()
		{
			if (useSerilog)
			{
				//serilogLogger = null;
			}
			logFilePath = null;
		}

		public void Log(object sender, string message)
		{
			if (OnLog != null)
				OnLog(sender, new OnDebugEventArgs { Message = message, Error = null });
			//if (useSerilog && serilogLogger != null)
			//{
			//	serilogLogger.Information(message);
			//}
			if (logFilePath != null)//else
			{
				LogMessageToFile("[LOG] " + message);
			}
		}

		public void LogDebug(object sender, string message)
		{
			//if (useSerilog && serilogLogger != null)
			//{
			//	serilogLogger.Information(message);
			//}
			if (logFilePath != null)//else
			{
				LogMessageToFile("[DBG] " + message);
			}
		}

		public void LogWarning(object sender, string message)
		{
			if (OnLog != null)
				OnLog(sender, new OnDebugEventArgs { Message = message, Error = false });
			//if (useSerilog && serilogLogger != null)
			//{
			//	serilogLogger.Warning(message);
			//}
			if (logFilePath != null)//else
			{
				LogMessageToFile("[WRN] " + message);
			}
		}

		public void LogError(object sender, string message)
		{
			if (OnLog != null)
				OnLog(sender, new OnDebugEventArgs { Message = message, Error = true });
			//if (useSerilog && serilogLogger != null)
			//{
			//	serilogLogger.Error(message);
			//}
			if (logFilePath != null)//else
			{
				LogMessageToFile("[ERR] " + message);
			}
		}

		private void LogMessageToFile(string message)
		{
			using (System.IO.StreamWriter sw = System.IO.File.AppendText(logFilePath))
			{
				try
				{
					string timestampedMessage = $"{DateTime.Now:G}: {message}";
					sw.WriteLine(timestampedMessage);
				}
				catch (Exception ex)
				{
					LogError(this, "Failed to log to file: " + ex.Message);
				}
			}
		}

		public void OnLogMessageDelegate(object sender, OnDebugEventArgs args) {
			if (OnLog != null) OnLog.Invoke(sender, args);
		}

		void IDebugger.Init(string logfilePath, string logBaseName)
		{
			Init(logfilePath, logBaseName);
		}

		void IDebugger.Log(string message, params object[] param)
		{
			Log(null, string.Format(message, param));
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