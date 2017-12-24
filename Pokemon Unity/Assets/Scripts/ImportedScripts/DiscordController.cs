using UnityEngine;

[System.Serializable]
public class DiscordJoinEvent : UnityEngine.Events.UnityEvent<string> { }

[System.Serializable]
public class DiscordSpectateEvent : UnityEngine.Events.UnityEvent<string> { }

[System.Serializable]
public class DiscordJoinRequestEvent : UnityEngine.Events.UnityEvent<DiscordRpc.JoinRequest> { }

public class DiscordController : MonoBehaviour
{
    public DiscordRpc.RichPresence presence;
    public string applicationId;
    public string optionalSteamId;
    public int callbackCalls;
    public int clickCounter;
    public string details;
    public UnityEngine.Events.UnityEvent onConnect;
    public UnityEngine.Events.UnityEvent onDisconnect;
    public DiscordJoinEvent onJoin;
    public DiscordJoinEvent onSpectate;
    public DiscordJoinRequestEvent onJoinRequest;

    DiscordRpc.EventHandlers handlers;

    public void ChangePresence(string pdetails)
    {
        Debug.Log("Discord: on click!");
        clickCounter++;

        presence.details = pdetails;
        details = pdetails;

        DiscordRpc.UpdatePresence(ref presence);
    }

    public void ReadyCallback()
    {
        ++callbackCalls;
        Debug.Log("Discord: ready");
        onConnect.Invoke();
    }

    public void DisconnectedCallback(int errorCode, string message)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: disconnect {0}: {1}", errorCode, message));
        onDisconnect.Invoke();
    }

    public void ErrorCallback(int errorCode, string message)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: error {0}: {1}", errorCode, message));
    }

    public void JoinCallback(string secret)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: join ({0})", secret));
        onJoin.Invoke(secret);
    }

    public void SpectateCallback(string secret)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: spectate ({0})", secret));
        onSpectate.Invoke(secret);
    }

    public void RequestCallback(DiscordRpc.JoinRequest request)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: join request {0}: {1}", request.username, request.userId));
        onJoinRequest.Invoke(request);
    }

    void Start()
    {
    }

    void Update()
    {
        DiscordRpc.RunCallbacks();
    }

    void OnEnable()
    {
        Debug.Log("Discord: init");
        callbackCalls = 0;

        handlers = new DiscordRpc.EventHandlers();
        handlers.readyCallback = ReadyCallback;
        handlers.disconnectedCallback += DisconnectedCallback;
        handlers.errorCallback += ErrorCallback;
        handlers.joinCallback += JoinCallback;
        handlers.spectateCallback += SpectateCallback;
        handlers.requestCallback += RequestCallback;
        DiscordRpc.Initialize(applicationId, ref handlers, true, optionalSteamId);
    }

    void OnDisable()
    {
        Debug.Log("Discord: shutdown");
        DiscordRpc.Shutdown();
    }

    void OnDestroy()
    {

    }
}
