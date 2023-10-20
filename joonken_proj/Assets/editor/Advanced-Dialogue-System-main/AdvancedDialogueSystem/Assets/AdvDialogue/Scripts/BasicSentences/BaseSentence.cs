using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Dialogue
{
    public abstract class BaseSentence : ScriptableObject
    {
        [SerializeField] private string speaker;
        [TextArea(3, 4)]
        [SerializeField] private string text = " ";

        public string Speaker => speaker;
        public string Text => text;

#if UNITY_EDITOR
        protected const float snapSpace = 25;

        public Rect rect;
        public Rect displayRect;
        public bool isSelected;

        public virtual void Draw(Vector2 windowPos)
        {
            rect.width = 200;
            rect.height = 50;

            var normalStyle = new GUIStyle();
            normalStyle.alignment = TextAnchor.MiddleCenter;
            normalStyle.normal.textColor = Color.white;
            normalStyle.fontStyle = FontStyle.Bold;
            normalStyle.wordWrap = true;

            var selectedStyle = new GUIStyle();
            selectedStyle.alignment = TextAnchor.MiddleCenter;
            selectedStyle.normal.textColor = Color.white;
            selectedStyle.fontStyle = FontStyle.Bold;
            selectedStyle.wordWrap = true;

            normalStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0.png") as Texture2D;
            normalStyle.border = new RectOffset(12, 12, 12, 12);

            selectedStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0 on.png") as Texture2D;
            selectedStyle.border = new RectOffset(12, 12, 12, 12);

            var snapedPos = new Vector2(Mathf.RoundToInt(rect.position.x / snapSpace) * snapSpace, Mathf.RoundToInt(rect.position.y / snapSpace) * snapSpace);
            displayRect = new Rect(snapedPos + windowPos, rect.size);
            GUI.Box(displayRect, Text[..Mathf.Min(Text.Length, 25)].Replace('\n', '\0'), isSelected ? selectedStyle : normalStyle);
        }

        public virtual void ShowContextMenu()
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add Connection"), false, null);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Remove Sentence"), false, null);
            menu.ShowAsContext();
        }

        public void Drag(Event e)
        {
            rect.position += e.delta;
        }

        public bool RectContains(Vector2 mousePos)
        {
            return displayRect.Contains(mousePos);
        }
#endif
    }
}