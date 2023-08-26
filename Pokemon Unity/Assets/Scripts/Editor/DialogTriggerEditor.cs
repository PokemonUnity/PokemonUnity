using MarkupAttributes.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogTriggerBehaviour))]
public class DialogTriggerEditor : MarkedUpEditor {

    protected override void OnInitialize() {
        AddCallback(serializedObject.FindProperty("DialogTriggers"), CallbackEvent.BeforeProperty, SyncEpisodes);
    }

    public void SyncEpisodes(SerializedProperty dialogTriggersProperty) {
        if (!GUILayout.Button("Sync Dialog Series")) return;

        DialogTriggerBehaviour dialogTriggerBehaviour = (DialogTriggerBehaviour)serializedObject.targetObject;
        SerializedProperty dialogSeriesProperty = serializedObject.FindProperty("dialogSeries");
        if (dialogSeriesProperty == null)
            return;

        DialogSeries dialogSeries = (DialogSeries)dialogSeriesProperty.objectReferenceValue;
        List<DialogTrigger> dialogTriggers = dialogTriggerBehaviour.DialogTriggers;
        if (dialogTriggers.Count > 0) {
            Debug.LogError("Dialog Triggers is already populated. Delete all entries to sync with a Dialog Series", serializedObject.targetObject);
            return;
        }

        dialogTriggers.Clear();
        foreach (DialogEpisode episode in dialogSeries.Dialogue) {
            dialogTriggersProperty.InsertArrayElementAtIndex(dialogTriggersProperty.arraySize);
            DialogTrigger trigger = new DialogTrigger();
            trigger.Name = episode.Name;
            dialogTriggers.Add(trigger);
        }
        serializedObject.Update();
    }
}
