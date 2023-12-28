using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OK : Player
{
    float time;
    Vector2 pos;

    public override void Dash()
    {
        throw new System.NotImplementedException();
    }

    public override void FastAttack()
    {
        throw new System.NotImplementedException();
    }

    public override void NormalAttack()
    {
        throw new System.NotImplementedException();
    }

    public override void PowerSkill()
    {
        throw new System.NotImplementedException();
    }

    public override void SlowAttack()
    {
        throw new System.NotImplementedException();
    }

    public override void Ultimate()
    {
        throw new System.NotImplementedException();
    }

    new void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        RageObj.SetActive(false);
        pos = transform.position;       
        maxHp = HP;
        tlqkfGauge = 0;
        maxtlqkfGauge = maxHp;
    }
    new void Update()
    {
        time += Time.deltaTime;
        transform.position = new Vector2(0,Mathf.Sin(time)) + pos;
    }
    public override void Damage(Skill hitSkill)
    {
        base.Damage(hitSkill);
        StartCoroutine(moveMoonY());
    }
    IEnumerator moveMoonY()
    {
        transform.position = transform.position + new Vector3(-0.5f,0);
        yield return new WaitForSeconds(0.1f);
        transform.position = transform.position + new Vector3(1f,0);
        yield return new WaitForSeconds(0.1f);
        transform.position = transform.position + new Vector3(0.5f,0);
    }
}
