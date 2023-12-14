using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set;}
    [SerializeField] Image hp1, hp2;
    [Space(10)]
    [SerializeField] Image smoothHp1;
    [SerializeField] Image smoothHp2;
    [Space(10)]
    [SerializeField] Image tlqkfGauge1;
    [SerializeField] Image tlqkfGauge2;
    [SerializeField] Text TimeText;
    float time = 60;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        hp1.fillAmount = 1;
        hp2.fillAmount = 1;

        smoothHp1.fillAmount = 1;
        smoothHp2.fillAmount = 1;

        tlqkfGauge1.fillAmount = 0;
        tlqkfGauge2.fillAmount = 0;
    }
    private void Update()
    {
        var g =  GameManager.instance;

        if(!g.player.stiffness) smoothHp1.fillAmount
         = Mathf.MoveTowards(smoothHp1.fillAmount,hp1.fillAmount,Time.deltaTime);
         if(!g.Enemy.stiffness) smoothHp2.fillAmount
         = Mathf.MoveTowards(smoothHp2.fillAmount,hp2.fillAmount,Time.deltaTime);
        time -= Time.deltaTime;
        TimeText.text = $"{(int)time}";
    }
    public void UIUpdate()
    {
        var g =  GameManager.instance;
        hp1.fillAmount = g.player.HP / g.player.maxHp;
        hp2.fillAmount = g.Enemy.HP / g.Enemy.maxHp;

        tlqkfGauge1.fillAmount = g.player.tlqkfGauge / g.player.maxtlqkfGauge;
        tlqkfGauge2.fillAmount = g.Enemy.tlqkfGauge / g.Enemy.maxtlqkfGauge;
    }
}
