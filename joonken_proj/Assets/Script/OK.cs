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

    }
    public override void Damage(Skill hitSkill)
    {
        if (isGuard)
        {
            Stiff(hitSkill.guardStiffTime);
        }
        else
        {
            HP -= hitSkill.damage;
            if(!isRage && HP <= maxHp * 0.3f)
            {
                isRage = true;
                RageObj.SetActive(true);
            }
            if (hitSkill.isThrow)
            {
                transform.Translate(Vector2.up * 0.5f);
                stiffness = true;
                isHit = true;
                rigid.velocity = hitSkill.dir * hitSkill.damage * (flipX ? -1 : 1);
            }
            else
            {
                Stiff(hitSkill.hitStiffTime);
            }
        }
        anim.SetTrigger("Damage");
        tlqkfGauge += hitSkill.damage * 1.5f;
        tlqkfGauge = Mathf.Clamp(tlqkfGauge,0,maxtlqkfGauge);

        UIManager.instance.UIUpdate();
        print(tlqkfGauge);
    }
}
