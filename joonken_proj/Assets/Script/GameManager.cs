using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}

    public Player player, Enemy;

    [SerializeField] Camera cam;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        var p = player.transform.position;
        var e = Enemy.transform.position;
        var camPosX = Mathf.Lerp(p.x,e.x,0.5f);
        cam.transform.position = new Vector3(camPosX,-1,-10);
        cam.orthographicSize = 5 + Mathf.Clamp(Vector2.Distance(p,e) - 14f,0,20) * 0.35f;
    }
    public bool isStart = false;

    public void GameStart()
    {
        
    }
}
