using System;

[System.Serializable]
public class CustomSaveEvent
{
    public DateTime EventTime;
    public SaveEventType EventType;
    public int SceneIndex;
    public string ObjectName;
    public SerializableVector3 ObjectPosition;

    public CustomSaveEvent(SaveEventType eventType, UnityEngine.GameObject eventObject, int sceneIndex)
    {
        EventTime = DateTime.Now;
        EventType = eventType;
        SceneIndex = sceneIndex;

        ObjectName = eventObject.name;
        ObjectPosition = eventObject.transform.position;
    }

    public override string ToString()
    {
        return ("EventType : " + EventType.ToString() + ", Time : " + EventTime + ", Map: " + SceneIndex);
    }
}
