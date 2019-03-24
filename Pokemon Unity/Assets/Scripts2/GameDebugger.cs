using System;

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
#if !DEBUG
				UnityEngine.Debug.Log(value);
#endif
		}
	}
	private static string DebugWarning
	{
		set
		{
#if !DEBUG
				UnityEngine.Debug.LogWarning(value);
#endif
		}
	}
	private static string DebugError
	{
		set
		{
#if !DEBUG
				UnityEngine.Debug.LogError(value);
#endif
		}
	}
	static System.IO.StreamWriter logFile = null;
	/// <summary>
	/// Determines whether or not to store Debug Log to a file, or display only
	/// </summary>
	static bool forwardToDebug { get; set; }
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
		if (logFile != null)
			logFile.Close();
		logFile = null;
	}

	static void LogCallback(string message, string stack, UnityEngine.LogType logtype)
	{
		switch (logtype)
		{
			default:
			case UnityEngine.LogType.Log:
				GameDebug._Log(message);
				break;
			case UnityEngine.LogType.Warning:
				GameDebug._LogWarning(message);
				break;
			case UnityEngine.LogType.Error:
				GameDebug._LogError(message);
				break;
		}
	}

	public static void Log(string message)
	{
		if (forwardToDebug)
			//Debug.Log(message);
			Debug = message;
		else
			_Log(message);
	}

	static void _Log(string message)
	{
		Console.Write(UnityEngine.Time.frameCount + ": " + message);
		if (logFile != null)
			logFile.WriteLine(UnityEngine.Time.frameCount + ": " + message + "\n");
	}

	public static void LogError(string message)
	{
		if (forwardToDebug)
			//Debug.LogError(message);
			DebugError = message;
		else
			_LogError(message);
	}

	static void _LogError(string message)
	{
		Console.Write(UnityEngine.Time.frameCount + ": [ERR] " + message);
		if (logFile != null)
			logFile.WriteLine("[ERR] " + message + "\n");
	}

	public static void LogWarning(string message)
	{
		if (forwardToDebug)
			//Debug.LogWarning(message);
			DebugWarning = message;
		else
			_LogWarning(message);
	}

	static void _LogWarning(string message)
	{
		Console.Write(UnityEngine.Time.frameCount + ": [WARN] " + message);
		if (logFile != null)
			logFile.WriteLine("[WARN] " + message + "\n");
	}
}