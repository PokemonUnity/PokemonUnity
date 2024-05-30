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

	public class OnDebugEventArgs : EventArgs, PokemonEssentials.Interface.EventArg.IEventArgs
	{
		public static readonly int EventId = typeof(OnDebugEventArgs).GetHashCode();

		public int Id { get { return EventId; } }
		/// <summary>
		/// If true (error), pause/stop game to display error message on screen.
		/// If false (warning), display message on screen without pausing game.
		/// If null (debug), silently log message in background.
		/// </summary>
		public bool? Error { get; set; }
		public string Message { get; set; }
	}
}