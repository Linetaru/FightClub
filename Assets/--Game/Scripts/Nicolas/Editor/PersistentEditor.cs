using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class PersistentEditor
{
    static PersistentEditor()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        //Debug.Log("New State = " + obj.ToString());
        if (obj == PlayModeStateChange.EnteredPlayMode)
        {
            EditorApplication.isPlaying = false;
            Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }
    }
}
