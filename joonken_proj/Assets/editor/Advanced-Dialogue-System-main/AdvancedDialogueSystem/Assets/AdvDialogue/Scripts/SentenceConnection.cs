using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class SentenceConnection : ScriptableObject
    {
        [SerializeField] private BaseSentence from;
        [SerializeField] private BaseSentence to;

#if UNITY_EDITOR
        
#endif
    }
}