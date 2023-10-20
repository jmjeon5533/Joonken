using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Dialogue
{
    public class StartSentence : BaseSentence
    {
#if UNITY_EDITOR
        public override void Draw(Vector2 windowPos)
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

            normalStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node6.png") as Texture2D;
            normalStyle.border = new RectOffset(12, 12, 12, 12);

            selectedStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node6 on.png") as Texture2D;
            selectedStyle.border = new RectOffset(12, 12, 12, 12);

            var snapedPos = new Vector2(Mathf.RoundToInt(rect.position.x / snapSpace) * snapSpace, Mathf.RoundToInt(rect.position.y / snapSpace) * snapSpace);
            displayRect = new Rect(snapedPos + windowPos, rect.size);
            GUI.Box(displayRect, "Start", isSelected ? selectedStyle : normalStyle);
        }
#endif
    }
}