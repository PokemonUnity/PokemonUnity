using System;

namespace PokemonUnity
{
	public interface IDebugger
	{
		//event EventHandler<OnDebugEventArgs> OnDebug;
		event EventHandler<OnDebugEventArgs> OnLog;

		/// <summary>
		/// Create and open data stream to file used for storing log entries.
		/// </summary>
		/// <param name="logfilePath">File Directory</param>
		/// <param name="logBaseName">Name of the File</param>
		void Init(string logfilePath, string logBaseName);
		/// <summary>
		/// Silently writes into log file
		/// </summary>
		/// <param name="message"></param>
		void Log(string message, params object[] param);
		/// <summary>
		/// Silently writes verbose context into log file, along with stack trace.
		/// </summary>
		/// <remarks>
		/// Should use only if debug mode is enabled
		/// </remarks>
		/// <param name="message"></param>
		void LogDebug(string message, params object[] param);
		/// <summary>
		/// Displays to user, but doesnt pause or interrupt game.
		/// Typically flashes on screen and goes away.
		/// </summary>
		/// <param name="message"></param>
		void LogWarning(string message, params object[] param);
		/// <summary>
		/// Pauses and interrupts game to be displayed to user.
		/// Typically responses to user commands.
		/// </summary>
		/// <param name="message"></param>
		void LogError(string message, params object[] param);
		/// <summary>
		/// Save and close data stream to file.
		/// </summary>
		void Shutdown();
	}

	/// <summary>
	/// </summary>
	/// <remarks>
	/// LogManager now also implements IDebugger which allows it to act as an IDebugger itself,
	/// facilitating direct logging or acting as a dispatch center, forwarding logs to other IDebugger instances.
	/// </remarks>
	public class OnDebugEventArgs : EventArgs, PokemonEssentials.Interface.EventArg.IEventArgs
	{
		public static readonly int EventId = typeof(OnDebugEventArgs).GetHashCode();

		public int Id { get { return EventId; } }
		/// <summary>
		/// If debug flag is set to true,
		/// will add stack trace to output logs.
		/// </summary>
		public bool Debug { get; set; }
		/// <summary>
		/// If true (error), pause/stop game to display error message on screen.
		/// If false (warning), display message on screen without pausing game.
		/// If null (debug), silently log message in background.
		/// </summary>
		public bool? Error { get; set; }
		/// <summary>
		/// Message to log to console, file, and/or user
		/// </summary>
		public string Message { get; set; }
		/// <summary>
		/// If message contains additional variables that get passed as input parameters for formatting.
		/// </summary>
		public object[] MessageParameters { get; set; }
		/// <summary>
		/// Calling method that initiated the log response entry.
		/// </summary>
		public System.Reflection.MethodBase Method { get; set; }
	}

	public class LogManager : IDebugger
	{
		private static readonly LogManager instance = new LogManager();
		public static LogManager Logger { get { return instance; } }

		//private event Action<string, object[]> OnLog;
		public event EventHandler<OnDebugEventArgs> OnLog;

		private LogManager() { Core.Logger = this; } // Ensures Singleton

		//public void Subscribe(IDebugger subscriber)
		//{
		//	OnLog += subscriber.Log;
		//	OnLog += subscriber.LogDebug;
		//	OnLog += subscriber.LogWarning;
		//	OnLog += subscriber.LogError;
		//}
		//
		//public void Unsubscribe(IDebugger subscriber)
		//{
		//	OnLog -= subscriber.Log;
		//	OnLog -= subscriber.LogDebug;
		//	OnLog -= subscriber.LogWarning;
		//	OnLog -= subscriber.LogError;
		//}

		public void Log(object sender, string message, params object[] obj)
		{
			OnDebugEventArgs eventArgs = new OnDebugEventArgs();
			eventArgs.Error = null;
			eventArgs.Message = message;
			eventArgs.MessageParameters = obj;
			Log(sender, eventArgs);
		}

		public void LogDebug(object sender, string message, params object[] obj)
		{
			OnDebugEventArgs eventArgs = new OnDebugEventArgs();
			eventArgs.Debug = true;
			eventArgs.Error = null;
			eventArgs.Message = message;
			eventArgs.MessageParameters = obj;
			Log(sender, eventArgs);
		}

		public void LogWarning(object sender, string message, params object[] obj)
		{
			OnDebugEventArgs eventArgs = new OnDebugEventArgs();
			eventArgs.Error = false;
			eventArgs.Message = message;
			eventArgs.MessageParameters = obj;
			Log(sender, eventArgs);
		}

		public void LogError(object sender, string message, params object[] obj)
		{
			OnDebugEventArgs eventArgs = new OnDebugEventArgs();
			eventArgs.Error = true;
			eventArgs.Message = message;
			eventArgs.MessageParameters = obj;
			Log(sender, eventArgs);
		}

		public void Log(object sender, OnDebugEventArgs eventArgs)
		{
			OnLog?.Invoke(sender, eventArgs);
		}

		void IDebugger.Init(string logfilePath, string logBaseName)
		{
			// Implementation for initializing log file not needed, leave up to subscriber.
			Log(null, new OnDebugEventArgs { Message = "Log initialized called with parameters: `{0}` and `{1}`", MessageParameters = new[] { logfilePath, logBaseName } });
		}

		void IDebugger.Log(string message, params object[] param)
		{
			Log(null, new OnDebugEventArgs { Message = message, MessageParameters = param });
		}

		void IDebugger.LogDebug(string message, params object[] param)
		{
			Log(null, new OnDebugEventArgs { Debug = true, Message = message, MessageParameters = param });
		}

		void IDebugger.LogWarning(string message, params object[] param)
		{
			Log(null, new OnDebugEventArgs { Error = false, Message = message, MessageParameters = param });
		}

		void IDebugger.LogError(string message, params object[] param)
		{
			Log(null, new OnDebugEventArgs { Error = true, Message = message, MessageParameters = param });
		}

		void IDebugger.Shutdown()
		{
			// Implementation for shutting down log file not really needed here,
			// But can build an `IList` collection of subscribed `IDebugger` and call `Shutdown()` with loop.
		}
	}
}