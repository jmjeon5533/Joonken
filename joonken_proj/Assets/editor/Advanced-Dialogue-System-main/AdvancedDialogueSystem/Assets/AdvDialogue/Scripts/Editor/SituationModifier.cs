using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Dialogue
{
    public static class SituationModifier
    {
        [InitializeOnLoadMethod]
        private static void OnLoad()
        {
            foreach (var instance in GetAllInstances<Situation>())
            {
                for (int i = instance.Sentences.Count - 1; i >= 0; i--)
                    if (instance.Sentences[i] == null) instance.Sentences.RemoveAt(i);

                EditorUtility.SetDirty(instance);
                AssetDatabase.SaveAssetIfDirty(instance);

                var assetPath = AssetDatabase.GetAssetPath(instance).Replace("Assets", Application.dataPath);
                var assetContent = File.ReadAllText(assetPath);
                var split = assetContent.Split("---");

                StringBuilder sb = new();

                for (int i = 0; i < split.Length; i++)
                {
                    if (i == 0)
                    {
                        sb.Append(split[i]);
                        continue;
                    }

                    var s = split[i].LastIndexOf("m_Script: ");
                    var e = s;
                    while (split[i][++e] != '\n') ;

                    var m_ScriptText = split[i][s..e];
                    var guidIndex = m_ScriptText.LastIndexOf("guid: ");
                    var guid = m_ScriptText[(guidIndex + 6)..(guidIndex + 38)];
                    var scriptPath = AssetDatabase.GUIDToAssetPath(guid);
                    if(AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath) != null)
                    {
                        sb.Append("---").Append(split[i]);
                    }
                }
                File.WriteAllText(assetPath, sb.ToString());
            }
        }

        public static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            var guids = AssetDatabase.FindAssets("t:" + typeof(T).Name); //FindAssets uses tags check documentation for more info
            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++) //probably could get optimized
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;
        }
    }
}
#endif