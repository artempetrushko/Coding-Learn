using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Scripts
{
    [CustomEditor(typeof(TriggerData))]
    public class TriggerComponentEditor : Editor
    {
        private TriggerData component;
        private SerializedProperty purpose;
        private SerializedProperty actionButtonText;
        private SerializedProperty scriptMoment_TriggerNumber;
        private SerializedProperty task_TaskNumber;
        private SerializedProperty changeLevel_NextLevelIndex;
        private SerializedProperty enterToMiniScene_DestinationTrigger;
        private SerializedProperty enterToMiniScene_IsMinimapShouldActive;

        private void OnEnable()
        {
            component = target as TriggerData;
            purpose = serializedObject.FindProperty("TriggerPurpose");
            actionButtonText = serializedObject.FindProperty("ActionButtonText");
            scriptMoment_TriggerNumber = serializedObject.FindProperty("ScriptMoment_TriggerNumber");
            task_TaskNumber = serializedObject.FindProperty("Task_TaskNumber");
            changeLevel_NextLevelIndex = serializedObject.FindProperty("ChangeLevel_NextLevelIndex");
            enterToMiniScene_DestinationTrigger = serializedObject.FindProperty("EnterToMiniScene_DestinationTrigger");
            enterToMiniScene_IsMinimapShouldActive = serializedObject.FindProperty("EnterToMiniScene_IsMinimapShouldActive");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(purpose);
            EditorGUILayout.PropertyField(actionButtonText);
            switch (component.TriggerPurpose)
            {
                case TriggerData.Purpose.Task:
                    EditorGUILayout.PropertyField(task_TaskNumber, new GUIContent("Task Number"));
                    break;
                case TriggerData.Purpose.ChangeLevel:
                    EditorGUILayout.PropertyField(changeLevel_NextLevelIndex, new GUIContent("Next Level Index"));
                    break;
                case TriggerData.Purpose.ScriptMoment:
                    EditorGUILayout.PropertyField(scriptMoment_TriggerNumber, new GUIContent("Trigger Number"));
                    break;
                case TriggerData.Purpose.EnterToMiniScene:
                    EditorGUILayout.PropertyField(enterToMiniScene_DestinationTrigger, new GUIContent("Destination Trigger"));
                    EditorGUILayout.PropertyField(enterToMiniScene_IsMinimapShouldActive, new GUIContent("Is Minimap Will Be Active"));
                    break;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
