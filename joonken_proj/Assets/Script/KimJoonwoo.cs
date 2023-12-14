using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KimJoonwoo : Player
{
    int normalInput = 0;
    public override void Dash()
    {
        print("A");
        
    }
    public override void PowerSkill()
    {
        print("S");
        
    }
    public override void Ultimate()
    {
        print("D");
        
    }
    public override void NormalAttack()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position + new Vector3(flipX ? -2 : 2, 0.5f),new Vector2(4,2),0,LayerMask.GetMask("Entity"));
        if(hit == null) return;
        if(hit.CompareTag("Entity"))
        {
            hit.GetComponent<Player>().Damage(skillLists[0].skill);
            print(hit.name);
        }
    }
    public override void Damage(Skill hitSkill)
    {
        if (isGuard)
        {
            Stiff(hitSkill.guardStiffTime);
        }
        else
        {
            HP -= hitSkill.damage * (1 + Mathf.InverseLerp(0,maxtlqkfGauge, tlqkfGauge) * 0.7f);
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
        tlqkfGauge += hitSkill.damage * 3f;
        tlqkfGauge = Mathf.Clamp(tlqkfGauge,0,maxtlqkfGauge);

        UIManager.instance.UIUpdate();

        print(tlqkfGauge);

    }
    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + new Vector3(flipX ? -2 : 2, 0.5f),new Vector2(4,2));
        base.OnDrawGizmos();
    }
    public override void SlowAttack()
    {
        print("X");
        
    }
    public override void FastAttack()
    {
        print("C");
        
    }
    protected override void InputReset()
    {
        base.InputReset();
        normalInput = 0;
    }
}
