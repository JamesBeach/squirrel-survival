using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace InspectorUtilities
{
    public static class ListUtils
    {
        public static void ShowList(SerializedProperty list)
        {
            if (list.isArray)
            {
                list.Next(true); // skip generic field
                list.Next(true); // skip to array size field

                int length = list.intValue;

                list.Next(true); // skip to first array value


            }
            
            /*
            EditorGUILayout.PropertyField(list);
            for (int i = 0; i < list.arraySize; i++)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
            */
        }
    }
}
