using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OK : Player
{
    float time;
    Vector2 pos;

    protected override IEnumerator Dash()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator FastAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator NormalAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator PowerSkill()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator SlowAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator Ultimate()
    {
        throw new System.NotImplementedException();
    }

    new void Start()
    {
        pos = transform.position;        
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
