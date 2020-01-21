using System;
using PokemonUnity;

namespace PokemonUnity.Saving
{
    [System.Serializable]
    public class SaveEvent
    {
        public DateTime EventTime;
        public SaveEventType EventType;
        public int SceneIndex;
		/// <summary>
		/// </summary>
		/// ToDo: This should be replaced with an object type...
		/// There are hundreds of enum values throughout code you can rely on,
		/// Make a method, use a switch(object){case: object = whatever... }
		/// OR! Make a new method for each individual object value that can be used with code...
		/// Using is extremely unreliable, and will cause more problems than solutions
		/// You save it as a string, then load the string value, then what? 
		/// Still needs to be converted to enum before it can be used. Store it as an int/enum value
        public string ObjectName;
        public SeriV3 ObjectPosition;

        public SaveEvent(SaveEventType eventType, UnityEngine.GameObject eventObject, int sceneIndex)
        {
            EventTime = DateTime.Now;
            EventType = eventType;
            SceneIndex = sceneIndex;

            ObjectName = eventObject.name;
            ObjectPosition = (SeriV3)eventObject.transform.position;
        }

		/// <summary>
		/// </summary>
		/// <param name="eventType"></param>
		/// <param name="objectName"></param>
		/// <param name="objectPosition"></param>
		/// <param name="sceneIndex"></param>
		/// ToDo: Remove string value...
        public SaveEvent(SaveEventType eventType, string objectName, SeriV3 objectPosition, int sceneIndex)
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
