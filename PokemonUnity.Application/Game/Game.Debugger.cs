using System;
using System.Diagnostics;
using System.Text;

namespace PokemonUnity.Application
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
	///    *IF* we are in editor, they are also sent to Debug. So they show up in editor Console window
	///    </para><para>
	/// Console.Write =>
	///    Only used for things that should not be logged. Typically responses to user commands. Only shown on Console.
	/// </para>
	/// </summary>
	public class Debugger : IDebugger
	{
		public event EventHandler<OnDebugEventArgs> OnLog;
		public static Debugger Instance { get { return instance; } }
		private static readonly Debugger instance = new Debugger();

		//private Serilog.ILogger serilogLogger;
		//protected bool useSerilog = false;
		protected string logFilePath;
		protected StringBuilder logBuilder = new StringBuilder();

		protected Debugger() { Core.Logger = this; }

		public virtual void Init(string logFilePath, string logBaseName)//, bool useSerilog = false
		{
			this.logFilePath = System.IO.Path.Combine(logFilePath, logBaseName + ".log");
			if (!System.IO.File.Exists(this.logFilePath))
				System.IO.File.Create(this.logFilePath);
			//this.useSerilog = useSerilog;
			//if (useSerilog)
			//{
			//	serilogLogger = new LoggerConfiguration()
			//		.WriteTo.File(this.logFilePath)
			//		.CreateLogger();
			//}
			Log("Debugger initialized. Logging to " + this.logFilePath);
		}

		public virtual void Shutdown()
		{
			//if (useSerilog)
			//{
			//	serilogLogger = null;
			//}
			logFilePath = null;
		}

		public virtual void Log(string message, params object[] param)
		{
			//if (OnLog != null)
			//	OnLog(sender, new OnDebugEventArgs { Message = message, Error = null });
			//if (useSerilog && serilogLogger != null)
			//{
			//	serilogLogger.Information(message);
			//} else
			if (logFilePath != null)
			{
				logBuilder = new StringBuilder(); //logBuilder.Clear();
				logBuilder.AppendFormat("[LOG] " + message, param);
				LogMessageToFile();
			}
		}

		public virtual void LogDebug(string message, params object[] param)
		{
			//if (useSerilog && serilogLogger != null)
			//{
			//	serilogLogger.Information(message);
			//} else
			if (logFilePath != null)
			{
				logBuilder = new StringBuilder(); //logBuilder.Clear();
				logBuilder.AppendFormat("[DBG] " + message, param);
				if (Core.DEBUG)
					logBuilder.AppendLine("\nStack Trace:\n" + new StackTrace().ToString());
				LogMessageToFile();
			}
		}

		public virtual void LogWarning(string message, params object[] param)
		{
			//if (OnLog != null)
			//	OnLog(sender, new OnDebugEventArgs { Message = message, Error = false });
			//if (useSerilog && serilogLogger != null)
			//{
			//	serilogLogger.Warning(message);
			//} else
			if (logFilePath != null)
			{
				logBuilder = new StringBuilder(); //logBuilder.Clear();
				logBuilder.AppendFormat("[WRN] " + message, param);
				LogMessageToFile();
			}
		}

		public virtual void LogError(string message, params object[] param)
		{
			//if (OnLog != null)
			//	OnLog(sender, new OnDebugEventArgs { Message = message, Error = true });
			//if (useSerilog && serilogLogger != null)
			//{
			//	serilogLogger.Error(message);
			//} else
			if (logFilePath != null)
			{
				logBuilder = new StringBuilder(); //logBuilder.Clear();
				logBuilder.AppendFormat("[ERR] " + message, param);
				LogMessageToFile();
			}
		}

		protected virtual void LogMessageToFile()
		{
			using (System.IO.StreamWriter sw = System.IO.File.AppendText(logFilePath))
			{
				try
				{
					//string timestampedMessage = $"{DateTime.Now:G}: {logBuilder.ToString()}";
					//sw.WriteLine(timestampedMessage);
					sw.WriteLine($"{DateTime.Now:G}: {logBuilder.ToString()}"); //no boxing for garbage collection?
				}
				catch (Exception ex)
				{
					//LogError("Failed to log to file: " + ex.Message);
					OnDebugEventArgs errArg = new OnDebugEventArgs { Message = "Failed to log to file: " + ex.Message, Error = true };
					OnLogMessageDelegate(this, errArg); // Curious to see what would happen...
				}
			}
		}

		protected virtual void OnLogMessageDelegate(object sender, OnDebugEventArgs args) {
			//if (OnLog != null) OnLog.Invoke(sender, args);
			if (args != null || args != System.EventArgs.Empty)
			{
				logBuilder = new StringBuilder(); //logBuilder.Clear();
				if (sender != null)
				{
					logBuilder.AppendFormat("Run: {0} => {1}", sender.GetType().Name, args.Method?.Name ?? string.Empty); // + System.Reflection.MethodBase.GetCurrentMethod().Name
					if (Core.DEBUG)
						logBuilder.AppendFormat("Entity: {0}", sender.ToString());
				}
				if (args.Error == true)
					logBuilder.Append("[ERR] ");
				else if (args.Error == false)
					logBuilder.Append("[WRN] ");
				else
					logBuilder.Append("[LOG] ");
				if (args.MessageParameters != null)
					logBuilder.AppendFormat(args.Message, args.MessageParameters);
				else
					logBuilder.Append(args.Message);
				if (args.Debug)
				{
					logBuilder.AppendLine("[DBG] ");
					logBuilder.Append("Stack Trace:\n" + new StackTrace().ToString());
				}
				LogMessageToFile();
			}
		}

		void IDebugger.Init(string logfilePath, string logBaseName)
		{
			Init(logfilePath, logBaseName);
		}
	}
}