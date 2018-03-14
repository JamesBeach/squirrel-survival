using UnityEngine;
using UnityEditor;

/// <summary>
/// This provides a custom inspector for the MainMenuController class. New properties of the class intended to be
/// editable from the inspector need to be explicitly exposed here. New levels are added to the game and exposed through
/// the main menu by adding them through the MainMenuController inspector, defined here.
/// </summary>
[CustomEditor(typeof(MainMenuController))]
[CanEditMultipleObjects]
public class MainMenuEditor : Editor {

    private SerializedProperty levels;
    private bool showLevels = false;

    private SerializedProperty levelListEntry;

	private void OnEnable() {
        levels = serializedObject.FindProperty("levels");
        levelListEntry = serializedObject.FindProperty("levelListEntry");
	}

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(levelListEntry);

        if (showLevels = EditorGUILayout.Foldout(showLevels, "Levels"))
        {
            ShowLevelList(levels);
            DropAreaGUI();
        }

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Display the levels, with reordering buttons, in the inspector panel.
    /// </summary>
    /// <param name="list"></param>
    public static void ShowLevelList(SerializedProperty list)
    {
        var iterator = list.Copy();

        if (iterator.isArray)
        {
            int length = iterator.arraySize;
            for (int i = 0; i < length; ++i)
            {
                SerializedProperty p = iterator.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();
                {// BEGIN HORIZONTAL LAYOUT BLOCK
                    EditorGUILayout.LabelField(p.stringValue);

                    // MOVE AN ITEM UP ONE POSITION IN THE LIST
                    if (GUILayout.Button("\u25b2") && i != 0)
                    {
                        SerializedProperty above = iterator.GetArrayElementAtIndex(i - 1);
                        string s = above.stringValue;
                        above.stringValue = p.stringValue;
                        p.stringValue = s;
                    }

                    // MOVE AN ITEM DOWN ONE POSITION IN THE LIST
                    if (GUILayout.Button("\u25bc") && i != length - 1)
                    {
                        iterator.InsertArrayElementAtIndex(i + 2);
                        iterator.GetArrayElementAtIndex(i + 2).stringValue = p.stringValue;
                        iterator.DeleteArrayElementAtIndex(i);
                    }

                    // REMOVE AN ITEM FROM THE LIST
                    if (GUILayout.Button("-"))
                    {
                        iterator.DeleteArrayElementAtIndex(i);
                    }
                }// END HORIZONTAL LAYOUT BLOCK
                EditorGUILayout.EndHorizontal();
            }
        }
    }

    public void DropAreaGUI()
    {
        Event evt = Event.current;
        Rect drop_area = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(drop_area, "Drag Scenes Here");

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!drop_area.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (Object dragged_object in DragAndDrop.objectReferences)
                    {
                        if (dragged_object.GetType().Equals(typeof(SceneAsset)))
                        {
                            SerializedProperty it = levels.Copy();

                            SceneAsset sc = dragged_object as SceneAsset;
                            ++it.arraySize;

                            SerializedProperty lvl = it.GetArrayElementAtIndex(it.arraySize - 1);
                            lvl.stringValue = sc.name;
                        }
                    }
                }
                break;
        }
    }
}
