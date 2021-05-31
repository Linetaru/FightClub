using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public static class ToolBoxUtils
{
    public enum GUIAddingName{
        Space,

    }
    public static void GUIAdding(GUIAddingName name, int space = 0)
    {
        switch (name)
        {
            case GUIAddingName.Space:
                GUILayout.Space(space);
                break;
        }
    }

    ///<summary> Will translate int in boolean.</summary>
    ///<returns>True == 0, False == 1</returns>
    public static bool IntToBoolean(int value)
    {
        if(value == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    ///<summary> SearchBar will reference given string and draw a toolbar Search Field with automatic cancel button.</summary>
    public static void SearchArea(ref string stringSearch)
    {
        GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
        stringSearch = GUILayout.TextField(stringSearch, GUI.skin.FindStyle("ToolbarSeachTextField"));
        if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
        {
            // Remove focus if cleared
            stringSearch = "";
            GUI.FocusControl(null);
        }
        GUILayout.EndHorizontal();
    }

    public static readonly GUIStyle DEFAULT_STYLE = new GUIStyle() { normal = new GUIStyleState() { textColor = Color.white } };

    ///<summary> Will draw element label field with a style.</summary>
    public static void DrawElement(Rect rect, string LabelName, GUIStyle style)
    {
        EditorGUI.LabelField(rect, LabelName, style);
    }

    ///<summary> Will draw element label field (Default Style).</summary>
    public static void DrawElement(Rect rect, string LabelName) => ToolBoxUtils.DrawElement(rect, LabelName, DEFAULT_STYLE);

    //-----------------------------------------------------------------------------------------------------------------------------//

    public static readonly Color DEFAULT_COLOR = new Color(0f, 0f, 0f, 0.3f);
    public static readonly Vector2 DEFAULT_LINE_MARGIN = new Vector2(2f, 2f);

    public const float DEFAULT_LINE_HEIGHT = 1f;


    ///<summary> Create a horizontal line to separate from the GUI .</summary>
    ///<param name="color"> Color to colorate the line. (Default = Black) </param>
    ///<param name="height"> Height of the line. (Default = 1f) </param>
    ///<param name="margin"> Vector 2 margin for spacing above and under the line. (Default = Vector2(2f, 2f)) </param>
    public static void HorizontalLine(Color color, float height, Vector2 margin)
    {
        GUILayout.Space(margin.x);

        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, height), color);

        GUILayout.Space(margin.y);
    }

    ///<summary> Create a horizontal line to separate from the GUI.</summary>
    ///<param name="color"> Color to colorate the line. (Default = Black) </param>
    ///<param name="height"> Height of the line. (Default = 1f) </param>
    public static void HorizontalLine(Color color, float height) => ToolBoxUtils.HorizontalLine(color, height, DEFAULT_LINE_MARGIN);

    ///<summary> Create a horizontal line to separate from the GUI.</summary>
    ///<param name="color"> Color to colorate the line. (Default = Black) </param>
    ///<param name="margin"> Vector 2 margin for spacing above and under the line. (Default = Vector2(2f, 2f)) </param>
    public static void HorizontalLine(Color color, Vector2 margin) => ToolBoxUtils.HorizontalLine(color, DEFAULT_LINE_HEIGHT, margin);

    ///<summary> Create a horizontal line to separate from the GUI.</summary>
    ///<param name="height"> Height of the line. (Default = 1f) </param>
    ///<param name="margin"> Vector 2 margin for spacing above and under the line. (Default = Vector2(2f, 2f)) </param>
    public static void HorizontalLine(Vector2 margin, float height) => ToolBoxUtils.HorizontalLine(DEFAULT_COLOR, height, margin);

    ///<summary> Create a horizontal line to separate from the GUI.</summary>
    ///<param name="color"> Color to colorate the line. (Default = Black) </param>
    public static void HorizontalLine(Color color) => ToolBoxUtils.HorizontalLine(color, DEFAULT_LINE_HEIGHT, DEFAULT_LINE_MARGIN);

    ///<summary> Create a horizontal line to separate from the GUI.</summary>
    ///<param name="height"> Height of the line. (Default = 1f) </param>
    public static void HorizontalLine(float height) => ToolBoxUtils.HorizontalLine(DEFAULT_COLOR, height, DEFAULT_LINE_MARGIN);

    ///<summary> Create a horizontal line to separate from the GUI.</summary>
    ///<param name="margin"> Vector 2 margin for spacing above and under the line. (Default = Vector2(2f, 2f)) </param>
    public static void HorizontalLine(Vector2 margin) => ToolBoxUtils.HorizontalLine(DEFAULT_COLOR, DEFAULT_LINE_HEIGHT, margin);

    ///<summary> Create a horizontal line to separate from the GUI with default parameter.</summary>
    public static void HorizontalLine() => ToolBoxUtils.HorizontalLine(DEFAULT_COLOR, DEFAULT_LINE_HEIGHT, DEFAULT_LINE_MARGIN);
}