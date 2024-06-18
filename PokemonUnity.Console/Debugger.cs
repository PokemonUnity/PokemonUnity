using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using PokemonUnity;
using PokemonUnity.Localization;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Utility;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using Serilog;

namespace PokemonUnity.ConsoleApp
{
	/// <summary>
	/// Logging of messages
	/// <para>
	/// There are three different types of messages:
	/// </para><para>
	/// Debug.Log/Warn/Error coming from unity (or code, e.g. packages, not using GameDebug) =>
	///    These get caught here and sent onto the console and into our log file
	///    </para><para>
	/// Core.Logger?.Log/Warn/Error coming from game =>
	///    These gets sent onto the console and into our log file.
	///    *IF* we are in editor, they are also sent to Debug. So they show up in editor Console window
	///    </para><para>
	/// Console.Write =>
	///    Only used for things that should not be logged. Typically responses to user commands. Only shown on Console.
	/// </para>
	/// </summary>
	public class Debugger : PokemonUnity.Application.Debugger, IDebugger
	{
		//public event EventHandler<OnDebugEventArgs> OnLog;
		//public static Debugger Instance { get { return instance; } }
		//private static readonly Debugger instance = new Debugger();

		private Serilog.ILogger serilogLogger;
		protected bool useSerilog = false;
		//protected string logFilePath;
		//protected StringBuilder logBuilder = new StringBuilder();


		//static Debugger() { Core.Logger = new Debugger(); }

		public Debugger() : base() { LogManager.Logger.OnLog += OnLogMessageDelegate; } //Game.OnLoad += OnLoadMessageDelegate;

		public virtual void Init(string logFilePath, string logBaseName, bool useSerilog = true)
		{
			this.logFilePath = System.IO.Path.Combine(logFilePath, logBaseName + ".log");
			if (System.IO.File.Exists(this.logFilePath))
				//System.IO.File.Create(this.logFilePath);
				System.IO.File.Delete(this.logFilePath); //Clear log file
			this.useSerilog = useSerilog;
			if (useSerilog)
			{
				serilogLogger = new LoggerConfiguration()
					.MinimumLevel.Verbose()
					.WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
						outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u4}] {Message:lj}{NewLine}{Exception}")
					.WriteTo.File(this.logFilePath,
						restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug,
						outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
						//rollingInterval: RollingInterval.Hour,
						retainedFileTimeLimit: TimeSpan.FromHours(0))
					.CreateLogger();
			}
			Log("Debugger initialized. Logging to `{0}`", System.IO.Path.GetFullPath(this.logFilePath));
		}

		public override void Shutdown()
		{
			if (useSerilog)
			{
				serilogLogger = null;
			}
			logFilePath = null;
		}

		public override void Log(string message, params object[] param)
		{
			if (useSerilog && serilogLogger != null)
			{
				serilogLogger.Information(message,param);
			} else
			if (logFilePath != null)
			{
				logBuilder = new StringBuilder(); //logBuilder.Clear();
				logBuilder.AppendFormat("[LOG] " + message, param);
				System.Console.WriteLine(logBuilder.ToString());
				LogMessageToFile();
			}
		}

		public override void LogDebug(string message, params object[] param)
		{
			if (useSerilog && serilogLogger != null)
			{
				serilogLogger.Debug(message,param);
				if (Core.DEBUG)
					serilogLogger.Verbose("Stack Trace:\n" + new StackTrace().ToString());
			} else
			if (logFilePath != null)
			{
				logBuilder = new StringBuilder(); //logBuilder.Clear();
				logBuilder.AppendFormat("[DBG] " + message, param);
				if (Core.DEBUG)
					logBuilder.AppendLine("\nStack Trace:\n" + new StackTrace().ToString());
				LogMessageToFile();
			}
		}

		public override void LogWarning(string message, params object[] param)
		{
			if (useSerilog && serilogLogger != null)
			{
				serilogLogger.Warning(message,param);
			} else
			if (logFilePath != null)
			{
				logBuilder = new StringBuilder(); //logBuilder.Clear();
				logBuilder.AppendFormat("[WRN] " + message, param);
				System.Console.WriteLine(logBuilder.ToString());
				LogMessageToFile();
			}
		}

		public override void LogError(string message, params object[] param)
		{
			if (useSerilog && serilogLogger != null)
			{
				serilogLogger.Error(message,param);
			} else
			if (logFilePath != null)
			{
				logBuilder = new StringBuilder(); //logBuilder.Clear();
				logBuilder.AppendFormat("[ERR] " + message, param);
				System.Console.WriteLine(logBuilder.ToString());
				LogMessageToFile();
			}
		}

		//protected override void LogMessageToFile()
		//{
		//	using (System.IO.StreamWriter sw = System.IO.File.AppendText(logFilePath))
		//	{
		//		try
		//		{
		//			//string timestampedMessage = $"{DateTime.Now:G}: {logBuilder.ToString()}";
		//			//sw.WriteLine(timestampedMessage);
		//			sw.WriteLine($"{DateTime.Now:G}: {logBuilder.ToString()}"); //no boxing for garbage collection?
		//		}
		//		catch (Exception ex)
		//		{
		//			//LogError("Failed to log to file: " + ex.Message);
		//			OnDebugEventArgs errArg = new OnDebugEventArgs { Message = "Failed to log to file: " + ex.Message, Error = true };
		//			OnLogMessageDelegate(this, errArg); // Curious to see what would happen...
		//		}
		//	}
		//}

		protected override void OnLogMessageDelegate(object sender, OnDebugEventArgs args) {
			if (args != null || args != System.EventArgs.Empty)
			{
				if (useSerilog && serilogLogger != null)
				{
					if (sender != null)
					{
						serilogLogger.Debug("Run: {sender} => {method}", sender.GetType().Name, args.Method?.Name);
						if (Core.DEBUG || args.Debug)
							serilogLogger.Verbose("Entity: {inspect}", sender.ToString());
					}
					if (args.Error == true)
						serilogLogger.Error(args.Message, args.MessageParameters);
					else if (args.Error == false)
						serilogLogger.Warning(args.Message, args.MessageParameters);
					else if (args.Debug)
					{
						serilogLogger.Debug(args.Message, args.MessageParameters);
						if (Core.DEBUG)
							serilogLogger.Verbose("Stack Trace: => {method}\n" + new StackTrace().ToString(), args.Method?.Name);
					}
					else
						serilogLogger.Information(args.Message, args.MessageParameters);
				}
				else
				{
					logBuilder = new StringBuilder(); //logBuilder.Clear();
					if (args.Error == true)
						logBuilder.Append("[ERR] ");
					else if (args.Error == false)
						logBuilder.Append("[WRN] ");
					else
						logBuilder.Append("[LOG] ");
					if (sender != null)
						logBuilder.Append(sender.GetType().Name + ": ");
					if (Core.DEBUG || args.Debug)
						logBuilder.AppendLine(sender.ToString());
					logBuilder.Append(args.Message);
					if (Core.DEBUG)
						logBuilder.AppendLine("[DMP] Stack Trace:\n" + new StackTrace().ToString());
					LogMessageToFile();
				}
			}
		}

		protected virtual void OnLoadMessageDelegate(object sender, OnLoadEventArgs args)
		{
			if (args != null || args != System.EventArgs.Empty)
			{
				OnDebugEventArgs debug = new OnDebugEventArgs()
				{
					Debug = true,
					//Message = "Total: {total}; Total Pieces: {totalPieces}; Check {check}; Piece {piece}",
					Message = "Total: {0}; Total Pieces: {1}; Check {2}; Piece {3}",
					MessageParameters = new object[] { args.Total, args.TotalPieces, args.Check, args.Piece }
				};
				OnLogMessageDelegate(sender, debug);
			}
		}

		void IDebugger.Init(string logfilePath, string logBaseName)
		{
			Init(logfilePath, logBaseName);
		}
	}
}