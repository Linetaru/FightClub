using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Transform))]
[CanEditMultipleObjects]
public class InspectorTransformEditor : Editor
{
    //Unity's built-in editor
    Editor _defaultEditor;
    Transform _transform;

    void OnEnable()
    {
        _defaultEditor = Editor.CreateEditor(targets, Type.GetType("UnityEditor.TransformInspector, UnityEditor"));
        _transform = target as Transform;
    }

    void OnDisable()
    {
        MethodInfo disableMethod = _defaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (disableMethod != null)
            disableMethod.Invoke(_defaultEditor, null);
        DestroyImmediate(_defaultEditor);
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Local Space", EditorStyles.boldLabel);
        _defaultEditor.OnInspectorGUI();

        //Show World Space Transform
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("World Space", EditorStyles.boldLabel);

        GUI.enabled = false;
        Vector3 localPosition = _transform.localPosition;
        _transform.localPosition = _transform.position;

        Quaternion localRotation = _transform.localRotation;
        _transform.localRotation = _transform.rotation;

        Vector3 localScale = _transform.localScale;
        _transform.localScale = _transform.lossyScale;

        _defaultEditor.OnInspectorGUI();
        _transform.localPosition = localPosition;
        _transform.localRotation = localRotation;
        _transform.localScale = localScale;
        GUI.enabled = true;
    }


}
