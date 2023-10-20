using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dialogue
{
    public class SituationEditorWindow : EditorWindow
    {
        public static void ShowWindow(Situation target)
        {
            var window = GetWindow<SituationEditorWindow>();
            window.target = target;
            window.sentences = target.Sentences.ToList();
            window.titleContent = new GUIContent("Advance Dialogue");
            window.Show();
        }

        private Situation target;
        private List<BaseSentence> sentences;
        private readonly List<BaseSentence> sentenceSelections = new();
        private bool isSelectionsMove;
        private bool isDragSelect;
        private bool isLeftCtrlPressed;

        private void OnEnable()
        {
            sentences = target?.Sentences.ToList();
        }

        private void OnGUI()
        {
            if (target == null)
            {
                Close();
                return;
            }

            DrawGrid(25, 200);

            EventHandle(Event.current);
            DrawSentences();

            Repaint();
        }

        private void DrawGrid(float gridSpace, float accentSpace)
        {
            Handles.BeginGUI();
            Handles.color = Color.black;

            var gridOffset = new Vector2(target.windowPos.x % gridSpace, target.windowPos.y % gridSpace);
            for (float w = 0; w < position.width + gridSpace; w += gridSpace)
            {
                Handles.DrawLine(new Vector2(w + gridOffset.x, 0), new Vector2(w + gridOffset.x, position.height));
            }

            for (float h = 0; h < position.height + gridSpace; h += gridSpace)
            {
                Handles.DrawLine(new Vector2(0, h + gridOffset.y), new Vector2(position.width, h + gridOffset.y));
            }

            var accentOffset = new Vector2(target.windowPos.x % accentSpace, target.windowPos.y % accentSpace);
            for (float aw = 0; aw < position.width + accentSpace; aw += accentSpace)
            {
                Handles.DrawLine(new Vector2(aw + accentOffset.x, 0), new Vector2(aw + accentOffset.x, position.height), 2);
            }

            for (float ah = 0; ah < position.height + accentSpace; ah += accentSpace)
            {
                Handles.DrawLine(new Vector2(0, ah + accentOffset.y), new Vector2(position.width, ah + accentOffset.y), 2);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void DrawSentences()
        {
            foreach(var sentence in sentences) sentence.isSelected = sentenceSelections.Contains(sentence);
            for(int i = 0; i < sentences.Count; i++) sentences[^(i + 1)].Draw(target.windowPos);
        }

        private void DrawSentenceConnections()
        {
            
        }

        private void EventHandle(Event e)
        {
            for (int i = 0; i < sentences.Count; i++)
            {
                if (e.type == EventType.Used) continue;
                switch (e.type)
                {
                    case EventType.MouseDown:
                        if(isLeftCtrlPressed)
                        {
                            if (sentences[i].RectContains(e.mousePosition))
                            {
                                if (e.button == 0)
                                {
                                    if (sentences[i].isSelected)
                                    {
                                        sentenceSelections.Remove(sentences[i]);
                                    }
                                    else
                                    {
                                        sentenceSelections.Add(sentences[i]);
                                        var temp = sentences[i];
                                        sentences.Remove(sentences[i]);
                                        sentences.Insert(0, temp);
                                    }
                                    Selection.objects = sentenceSelections.ToArray();
                                    e.Use();
                                }
                            }
                        }
                        else
                        {
                            if (sentences[i].RectContains(e.mousePosition))
                            {
                                if (e.button == 0)
                                {
                                    if(!sentences[i].isSelected)
                                    {
                                        sentenceSelections.Clear();
                                        sentenceSelections.Add(sentences[i]);
                                        var temp = sentences[i];
                                        sentences.Remove(sentences[i]);
                                        sentences.Insert(0, temp);
                                        Selection.objects = sentenceSelections.ToArray();
                                        e.Use();
                                    }
                                    isSelectionsMove = true;
                                    e.Use();
                                }

                                if(e.button == 1)
                                {
                                    if(sentenceSelections.Count == 1)
                                    {
                                        sentenceSelections[0].ShowContextMenu();
                                    }
                                    else
                                    {
                                        var menu = new GenericMenu();
                                        menu.AddItem(new GUIContent("Remove Sentences"), false, null);
                                        menu.ShowAsContext();
                                    }
                                    e.Use();
                                }
                            }
                        }
                        break;

                    case EventType.MouseDrag:
                        if (isSelectionsMove)
                        {
                            foreach (var sentence in sentenceSelections) sentence.Drag(e);
                            e.Use();
                        }
                        break;

                    case EventType.MouseUp:
                        isSelectionsMove = false;
                        break;
                }
            }

            if (e.type == EventType.Used) return;
            switch (e.type)
            {
                case EventType.MouseDown:
                    sentenceSelections.Clear();
                    Selection.objects = null;
                    if(e.button == 0)
                    {
                        isDragSelect = true;
                    }

                    if (e.button == 1) ShowContextMenu();
                    break;
                case EventType.MouseDrag:
                    if (e.button == 2)
                    {
                        target.windowPos += e.delta;
                    }
                    break;
                case EventType.KeyDown:
                    if(e.keyCode == KeyCode.LeftControl) isLeftCtrlPressed = true;
                    break;
                case EventType.KeyUp:
                    if(e.keyCode == KeyCode.LeftControl) isLeftCtrlPressed = false;
                    break;
            }
        }

        private void ShowContextMenu()
        {
            var menu = new GenericMenu();

            foreach (var t in TypeCache.GetTypesDerivedFrom<BaseSentence>())
            {
                if(t == typeof(StartSentence)) continue;

                var type = t;
                menu.AddItem(new GUIContent($"Custom/{type.Name}"), false, () =>
                {
                    var newSentence = CreateInstance(type) as BaseSentence;
                    target.Sentences.Add(newSentence);
                    sentences.Add(newSentence);

                    AssetDatabase.AddObjectToAsset(newSentence, target);
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(target));

                    EditorUtility.SetDirty(target);
                });
            }

            menu.ShowAsContext();
        }
    }
}