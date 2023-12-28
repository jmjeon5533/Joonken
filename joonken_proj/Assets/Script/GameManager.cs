using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}

    public Player player, Enemy;

    [SerializeField] Camera cam;

    [SerializeField] SpriteRenderer bg;

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
    public void UltimateFade()
    {
        StartCoroutine(FadeCor());
    }
    IEnumerator FadeCor()
    {
        bg.color = new Color(0.5f,0.5f,0.5f,1);
        yield return new WaitForSecondsRealtime(1.5f);
        bg.color = Color.white;
    }
}
