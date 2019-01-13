using UnityEngine;
using System;

[System.Serializable]
public class CustomSaveEvent
{
    public DateTime EventTime;
    public SaveEventType EventType;
    public int SceneIndex;
    public string ObjectName;
    public SerializableVector3 ObjectPosition;

    public CustomSaveEvent(DateTime eventTime, SaveEventType eventType, GameObject eventObject, int sceneIndex)
    {
        EventTime = eventTime;
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
