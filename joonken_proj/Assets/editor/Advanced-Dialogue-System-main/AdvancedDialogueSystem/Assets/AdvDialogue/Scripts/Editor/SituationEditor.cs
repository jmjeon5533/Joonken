using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Dialogue
{
    [CustomEditor(typeof(Situation))]
    public class SituationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Open Editor"))
            {
                SituationEditorWindow.ShowWindow(target as Situation);
            }
        }
    }
}
#endif