using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set;}
    private void Awake()
    {
        instance = this;
    }
    
    void Update()
    {
        
    }
}
