using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scripts
{
    [CustomEditor(typeof(ScriptTrigger))]
    public class ScriptTriggerComponentEditor : Editor
    {
        private SerializedProperty isFirstCutscene;
        private SerializedProperty taskNumber;
        private SerializedProperty startCutscene;
        private SerializedProperty taskEndingCutscene;
        private SerializedProperty transitionToNextTaskCutscene;

        private void OnEnable()
        {
            isFirstCutscene = serializedObject.FindProperty("isFirstCutscene");
            taskNumber = serializedObject.FindProperty("taskNumber");
            startCutscene = serializedObject.FindProperty("startCutscene");
            taskEndingCutscene = serializedObject.FindProperty("taskEndingCutscene");
            transitionToNextTaskCutscene = serializedObject.FindProperty("transitionToNextTaskCutscene");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(isFirstCutscene);
            if (isFirstCutscene.boolValue)
                EditorGUILayout.PropertyField(startCutscene);
            else
            {
                EditorGUILayout.PropertyField(taskNumber);
                EditorGUILayout.PropertyField(taskEndingCutscene);
                EditorGUILayout.PropertyField(transitionToNextTaskCutscene);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
