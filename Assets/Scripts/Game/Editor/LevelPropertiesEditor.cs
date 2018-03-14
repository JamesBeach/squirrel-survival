using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(LevelProperties))]
[CanEditMultipleObjects]
public class LevelPropertiesEditor : Editor {
    private CampaignParamsSerial campaign;
    private SurvivalParamsSerial survival;
    private SerializedProperty mode;

    private bool campaignPanelOn = false;
    private bool survivalPanelOn = false;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(mode);

        if (campaignPanelOn = EditorGUILayout.Foldout(campaignPanelOn, "Campaign Mode Settings"))
        {
            EditorGUILayout.PropertyField(campaign.timeLimit);
            EditorGUILayout.PropertyField(campaign.minimumNuts);
        }

        if (survivalPanelOn = EditorGUILayout.Foldout(survivalPanelOn, "Survival Mode Settings"))
        {
            EditorGUILayout.PropertyField(survival.timeOnTimer);
            EditorGUILayout.PropertyField(survival.xMin);
            EditorGUILayout.PropertyField(survival.xMax);
            EditorGUILayout.PropertyField(survival.yMin);
            EditorGUILayout.PropertyField(survival.yMax);
            EditorGUILayout.PropertyField(survival.acornPrefab);
        }

        serializedObject.ApplyModifiedProperties();
    }

    // Use this for initialization
    private void OnEnable()
    {
        SerializedProperty campaignParams = serializedObject.FindProperty("_campaignParams");
        campaign.minimumNuts = campaignParams.FindPropertyRelative("minimumNuts");
        campaign.timeLimit = campaignParams.FindPropertyRelative("timeLimit");

        SerializedProperty survivalParams = serializedObject.FindProperty("_survivalParams");
        survival.timeOnTimer = survivalParams.FindPropertyRelative("timeOnTimer");
        survival.xMin = survivalParams.FindPropertyRelative("xMin");
        survival.xMax = survivalParams.FindPropertyRelative("xMax");
        survival.yMin = survivalParams.FindPropertyRelative("yMin");
        survival.yMax = survivalParams.FindPropertyRelative("yMax");
        survival.acornPrefab = survivalParams.FindPropertyRelative("acornPrefab");

        mode = serializedObject.FindProperty("testGameMode");
    }

    private struct CampaignParamsSerial
    {
        public SerializedProperty minimumNuts;
        public SerializedProperty timeLimit;
    }

    private struct SurvivalParamsSerial
    {
        public SerializedProperty timeOnTimer;
        public SerializedProperty xMin, xMax, yMin, yMax;
        public SerializedProperty acornPrefab;
    }
}
