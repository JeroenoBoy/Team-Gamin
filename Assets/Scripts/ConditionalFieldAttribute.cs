using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Test))]
public class MyScriptEditor : Editor
{
    override public void OnInspectorGUI()
    {
        var myScript = target as Test;

        myScript.WanderAround = EditorGUILayout.Toggle("Hide Fields", myScript.WanderAround);

        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.WanderAround)))
        {
            if (group.visible == false)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PrefixLabel("Color");
                myScript.WanderDistance = EditorGUILayout.FloatField(myScript.WanderDistance);
                EditorGUI.indentLevel--;
            }
        }

        myScript.WanderAround = GUILayout.Toggle(myScript.WanderAround, "Disable Fields");

        /*myScript.disableBool = GUILayout.Toggle(myScript.disableBool, "Disable Fields");

        using (new EditorGUI.DisabledScope(myScript.disableBool))
        {
            myScript.someColor = EditorGUILayout.ColorField("Color", myScript.someColor);
            myScript.someString = EditorGUILayout.TextField("Text", myScript.someString);
            myScript.someNumber = EditorGUILayout.IntField("Number", myScript.someNumber);
        }*/
    }
}