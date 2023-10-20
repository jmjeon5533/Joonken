using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Situation", menuName = "Advanced Dialogue/Situation", order = 0)]
    public class Situation : ScriptableObject
    {
        [SerializeField] private List<BaseSentence> sentences = new();

        public List<BaseSentence> Sentences => sentences;

#if UNITY_EDITOR
        public Vector2 windowPos;

        private void OnEnable()
        {
            EditorApplication.update += Init;
        }

        private void Init()
        {
            if (sentences.Count == 0)
            {
                if (EditorUtility.IsPersistent(this))
                {
                    var start = CreateInstance<StartSentence>();
                    //start.hideFlags = HideFlags.HideInHierarchy;
                    start.name = "start";
                    //start.rect.position = Vector2.one * 300;
                    sentences.Add(start);

                    AssetDatabase.AddObjectToAsset(start, this);
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(this));
                    EditorUtility.SetDirty(this);
                    AssetDatabase.SaveAssetIfDirty(this);
                }
            }
            else
            {
                EditorApplication.update -= Init;
            }
        }
#endif
    }
}