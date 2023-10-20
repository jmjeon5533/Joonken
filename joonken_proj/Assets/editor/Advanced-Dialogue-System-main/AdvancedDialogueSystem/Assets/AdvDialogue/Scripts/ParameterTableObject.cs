using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Dialogue
{
    [System.Serializable]
    public class ParameterValue
    {
        public int intValue;
        public float floatValue;
        public bool boolValue;
    }

    public class ParameterTableObject : ScriptableObject, ISerializationCallbackReceiver
    {
        private static ParameterTableObject instance;

        [SerializeField] private List<string> keys = new();
        [SerializeField] private List<ParameterValue> values = new();
        private Dictionary<string, ParameterValue> parameters = new();

        public void OnAfterDeserialize()
        {
            parameters = new();

            for(int i = 0; i < Mathf.Min(keys.Count, values.Count); i++)
                parameters.Add(keys[i], values[i]);
        }

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach(var kvp in parameters)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        public static void CheckInstanceIsExist()
        {
            if (instance == null)
            {
                var objs = Resources.FindObjectsOfTypeAll<ParameterTableObject>();
                if (objs.Length == 0)
                {
                    instance = CreateInstance<ParameterTableObject>();
                    Directory.CreateDirectory(Application.dataPath + "/Resources");
                    AssetDatabase.CreateAsset(instance, "Assets/Resources/[ParameterTableObject].asset");
                }
                else
                {
                    instance = objs[0];
                }
            }

            EditorApplication.update -= CheckInstanceIsExistUpdate;
            EditorApplication.update += CheckInstanceIsExistUpdate;
        }

        private static void CheckInstanceIsExistUpdate()
        {
            if(instance == null)
            {
                instance = CreateInstance<ParameterTableObject>();
                Directory.CreateDirectory(Application.dataPath + "/Resources");
                AssetDatabase.CreateAsset(instance, "Assets/Resources/[ParameterTableObject].asset");
            }
            else
            {
                if(instance.name != "[ParameterTableObject]")
                {
                    instance.name = "[ParameterTableObject]";
                    AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(instance), "[ParameterTableObject]");

                    EditorUtility.SetDirty(instance);
                    AssetDatabase.SaveAssetIfDirty(instance);
                }
            }
        }
#endif
    }
}