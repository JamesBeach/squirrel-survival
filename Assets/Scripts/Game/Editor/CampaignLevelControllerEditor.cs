using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CampaignLevelController))]
[CanEditMultipleObjects]
public class CampaignLevelControllerEditor : Editor
{
    SerializedProperty time;
    SerializedProperty nutsNeeded;
    SerializedProperty goldenAcornValue;

    private void OnEnable()
    {
        time = serializedObject.FindProperty("time");
        nutsNeeded = serializedObject.FindProperty("nutsNeeded");
        goldenAcornValue = serializedObject.FindProperty("goldenAcornValue");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(time);
        EditorGUILayout.PropertyField(nutsNeeded);

        serializedObject.ApplyModifiedProperties();
    }
}
