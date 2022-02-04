using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Test : MonoBehaviour
{
    private bool show;
    string name;
    int WanderAround;
    float WanderDistance = 5;

    #region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(Test))]
    public class TestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Test test = (Test)target;

            test.show = GUILayout.Toggle(test.show, "Show");

            if (test.show)
                test.name = EditorGUILayout.TextField("Name", test.name);
        }
    }

#endif
    #endregion 
}
