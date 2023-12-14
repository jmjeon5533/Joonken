using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}

    public Player player, Enemy;

    private void Awake()
    {
        instance = this;
    }
    public bool isStart = false;

    public void GameStart()
    {
        
    }
}
