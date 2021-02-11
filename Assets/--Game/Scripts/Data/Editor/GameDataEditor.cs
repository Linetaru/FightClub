using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Sirenix.OdinInspector.Editor;

[CanEditMultipleObjects]
[CustomEditor(typeof(GameData))]
public class GameDataEditor : OdinEditor
{
    SerializedProperty numberOfLifeEditor;
    SerializedProperty timeOfRoundEditor;
    SerializedProperty victoryConditionEditor;
    SerializedProperty gameModeEditor;

    private new void OnEnable()
    {
        numberOfLifeEditor = serializedObject.FindProperty("numberOfLifes");
        timeOfRoundEditor = serializedObject.FindProperty("timeOfRoundInSecond");
        victoryConditionEditor = serializedObject.FindProperty("victoryCondition");
        gameModeEditor = serializedObject.FindProperty("gameMode");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ligneBetweenVar();

        EditorGUILayout.PropertyField(victoryConditionEditor);

        if (victoryConditionEditor.enumValueIndex == 0)
        {
            EditorGUILayout.PropertyField(numberOfLifeEditor, new GUIContent("Round Player Life : "));
        }
        else
        {
            var ts = TimeSpan.FromSeconds(timeOfRoundEditor.floatValue);
            EditorGUILayout.PropertyField(timeOfRoundEditor, new GUIContent("Round Timer : "));
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds));
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        ligneBetweenVar();

        EditorGUILayout.PropertyField(gameModeEditor);

        serializedObject.ApplyModifiedProperties();
    }

    private new void OnDisable()
    {
        numberOfLifeEditor = null;
        timeOfRoundEditor = null;
        victoryConditionEditor = null;
        gameModeEditor = null;
    }

    void ligneBetweenVar()
    {
        EditorGUILayout.Space(5);
        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label("------------------------------------");
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.Space(5);
    }
}