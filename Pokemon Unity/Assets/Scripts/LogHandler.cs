using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System;

[InitializeOnLoad]

public class LogHandler : ILogHandler
{
    /*
     * Baseline for code taken from Unity Docs for ILogHandler:
     * 
     * https://docs.unity3d.com/ScriptReference/ILogHandler.html
     */

    //private FileStream m_FileStream;
    //private StreamWriter m_StreamWriter;
    public static ILogHandler DefaultLogHandler { get; private set; }//= Debug.unityLogger.logHandler;
    private static LogHandler handler;

    static LogHandler()
    {
        // Replace the default debug log handler
        DefaultLogHandler = Debug.unityLogger.logHandler;
        handler = new LogHandler();
        Debug.unityLogger.logHandler = handler;
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        // Re-format view
        format = ColorMe(logType);
        MyWindow.LogBuffer = format;
        DefaultLogHandler.LogFormat(logType, context, format, args);        
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        DefaultLogHandler.LogException(exception, context);
    }

    private string ColorMe(LogType logType)
    {
        if (logType == LogType.Log)
            return "【DEBUG】  : <color=lime>{0}</color>";
        else if (logType == LogType.Warning)
            return "【WARNING】: <color=yellow>{0}</color>";
        else if (logType == LogType.Error)
            return "【ERROR】  : <color=red>{0}</color>";
        else
            return "【NoLogType】  : <color=blue>{0}</color>";
    }
}


public class MyGameClass : MonoBehaviour
{
    private static ILogger logger = Debug.unityLogger;
    private static string kTAG = "MyGameClass";
    private LogHandler LogHandler = new LogHandler();

    void Start()
    {
        logger.Log(kTAG, "MyGameClass Start.");
    }
}

public class MyWindow : EditorWindow
{
    [MenuItem("Window/CustomConsole")]
    static void Init()
    {
        SaveEditorSkin();
        // Get existing open window or if none, make a new one:
        MyWindow window = (MyWindow)EditorWindow.GetWindow(typeof(MyWindow));
        window.Show();
    }

    [MenuItem("Assets/Editor Skin")]
    static public void SaveEditorSkin()
    {
        GUISkin skin = Instantiate(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector));
        AssetDatabase.CreateAsset(skin, "Assets/EditorSkin.guiskin");
    }

    private bool collapse;
    private bool clearOnPlay;
    private bool errorPause;

    private GUIStyle box;

    private GUIContent clearContent = new GUIContent("Clear");
    private GUIContent collapseContent = new GUIContent("Collapse");
    private GUIContent clearOnPlayContent = new GUIContent("Clear on Play");
    private GUIContent errorPauseContent = new GUIContent("ErrorPause");

    private Rect rect;
    private Rect resizeRect;// = new Rect(0, 0, 0, 20);
    
    private bool resizing = false;
    private float ratio = 0.7f;
    private const int MinBoxSize = 28; // I admit to eyeballing this from he original console

    private static string logBuffer = "null";

    private void ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (resizeRect.Contains(e.mousePosition))
                {
                    resizing = true;
                }
                break;

            case EventType.MouseUp:
                resizing = false;
                break;
        }
    }

    private void Resize(Event e)
    {

    }

    private void OnGUI()
    {
        if (box == null)
        {
            box = GUI.skin.GetStyle("CN Box");
        }

        if (resizeRect.height == 0)
        {
            resizeRect.height = 20;
        }

        var style = EditorStyles.toolbarButton;
        
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Height(EditorStyles.toolbar.fixedHeight), GUILayout.ExpandWidth(true));
        {
            GUILayout.Button(clearContent, style, GUILayout.Width(style.CalcSize(clearContent).x));
            GUILayout.Space(7);
            collapse = GUILayout.Toggle(collapse, collapseContent, style, GUILayout.Width(style.CalcSize(collapseContent).x));
            clearOnPlay = GUILayout.Toggle(clearOnPlay, clearOnPlayContent, style, GUILayout.Width(style.CalcSize(clearOnPlayContent).x));
            errorPause = GUILayout.Toggle(errorPause, errorPauseContent, style, GUILayout.Width(style.CalcSize(errorPauseContent).x));
        }
        EditorGUILayout.EndHorizontal();

        /*here lies the toolbar drawing code we did last time */
        ProcessEvents(Event.current);
        if (resizing && (Event.current.type == EventType.MouseDrag))
        {
            resizeRect.y = Event.current.mousePosition.y - rect.y;
            ratio = resizeRect.y / rect.height;
            Repaint();
        }

        Rect tmp = EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
        {
            if ((Event.current.type != EventType.Layout) && (Event.current.type != EventType.Used))
            {
                rect = tmp;
                resizeRect.width = rect.width;
                resizeRect.y = Mathf.Clamp((rect.height * ratio), MinBoxSize, rect.height - MinBoxSize);
                ratio = (resizeRect.y) / rect.height;
            }

            Rect TopBox = EditorGUILayout.BeginVertical(GUILayout.Height(rect.height * ratio));
            {
                
                GUILayout.Box("", box, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                EditorGUILayout.BeginHorizontal(new GUIStyle(GUI.skin.GetStyle("CN EntryBackEven")),GUILayout.ExpandWidth(true));
                {
                    //EditorGUILayout.BeginHorizontal();
                    //{
                    EditorGUILayout.LabelField("", new GUIStyle(GUI.skin.GetStyle("CN EntryInfoIconSmall")));
                    //EditorGUILayout.LabelField(LogBuffer.ToString());
                    //}
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Box("CountBadge", new GUIStyle(GUI.skin.GetStyle("CN CountBadge")));
                GUILayout.Box("EntryBackEven", new GUIStyle(GUI.skin.GetStyle("CN EntryBackEven")));
                GUILayout.Box("EntryBackOdd", new GUIStyle(GUI.skin.GetStyle("CN EntryBackOdd")));
                GUILayout.Box("EntryError", new GUIStyle(GUI.skin.GetStyle("CN EntryError")));
                GUILayout.Box("EntryErrorIcon", new GUIStyle(GUI.skin.GetStyle("CN EntryErrorIcon")));
                GUILayout.Box("EntryErrorIconSmall", new GUIStyle(GUI.skin.GetStyle("CN EntryErrorIconSmall")));
                GUILayout.Box("EntryErrorSmall", new GUIStyle(GUI.skin.GetStyle("CN EntryErrorSmall")));
                //EditorGUILayout.TextField("Test 2");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Box("EntryInfo", new GUIStyle(GUI.skin.GetStyle("CN EntryInfo")));
                GUILayout.Box("EntryInfoIcon", new GUIStyle(GUI.skin.GetStyle("CN EntryInfoIcon")));
                GUILayout.Box("EntryInfoIconSmall", new GUIStyle(GUI.skin.GetStyle("CN EntryInfoIconSmall")));
                GUILayout.Box("EntryInfoSmall", new GUIStyle(GUI.skin.GetStyle("CN EntryInfoSmall")));
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Box("EntryWarn", new GUIStyle(GUI.skin.GetStyle("CN EntryWarn")));
                GUILayout.Box("EntryWarnIcon", new GUIStyle(GUI.skin.GetStyle("CN EntryWarnIcon")));
                GUILayout.Box("EntryWarnIconSmall", new GUIStyle(GUI.skin.GetStyle("CN EntryWarnIconSmall")));
                GUILayout.Box("EntryWarnSmall", new GUIStyle(GUI.skin.GetStyle("CN EntryWarnSmall")));
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Box("Message", new GUIStyle(GUI.skin.GetStyle("CN Message")));
                GUILayout.Box("StatusError", new GUIStyle(GUI.skin.GetStyle("CN StatusError")));
                GUILayout.Box("StatusInfo", new GUIStyle(GUI.skin.GetStyle("CN StatusInfo")));
                GUILayout.Box("StatusWarn", new GUIStyle(GUI.skin.GetStyle("CN StatusWarn")));
            }
            EditorGUILayout.EndHorizontal();

            Rect BottomBox = EditorGUILayout.BeginVertical();
            {
                GUILayout.Box("", box, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

            }
            EditorGUILayout.EndVertical();

        }
        EditorGUILayout.EndVertical();
        EditorGUIUtility.AddCursorRect(resizeRect, MouseCursor.ResizeVertical);

        /*here lies the rest of the GUIWindow */

    }

    public static string LogBuffer
    {
        // Call this when we wish to log something
        get
        {
            return logBuffer;
        }
        set
        {
            logBuffer = value;
        }
    }

}


