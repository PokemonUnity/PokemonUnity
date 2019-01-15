using System;
using PokemonUnity.Saving.Location;

namespace PokemonUnity.Saving
{
    [System.Serializable]
    public class SaveEvent
    {
        public DateTime EventTime;
        public SaveEventType EventType;
        public int SceneIndex;
        public string ObjectName;
        public SerializableVector3 ObjectPosition;

        public SaveEvent(SaveEventType eventType, UnityEngine.GameObject eventObject, int sceneIndex)
        {
            EventTime = DateTime.Now;
            EventType = eventType;
            SceneIndex = sceneIndex;

            ObjectName = eventObject.name;
            ObjectPosition = eventObject.transform.position;
        }

        public SaveEvent(SaveEventType eventType, string objectName, SerializableVector3 objectPosition, int sceneIndex)
        {
            EventTime = DateTime.Now;
            EventType = eventType;
            SceneIndex = sceneIndex;

            ObjectName = objectName;
            ObjectPosition = objectPosition;
        }

        public override string ToString()
        {
            return ("EventType : " + EventType.ToString() + ", Time : " + EventTime + ", Map: " + SceneIndex);
        }
    }
}
