using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KimJoonwoo : Player
{
    int normalInput = 0;
    [SerializeField] GameObject limbus;
    [SerializeField] ParticleSystem SpiritObj;
    public override void Dash()
    {
        print("A");

    }
    public override void PowerSkill()
    {
        print("S");
        Instantiate(SpiritObj, transform.position, Quaternion.identity);
        Collider2D[] hitall = Physics2D.OverlapBoxAll(transform.position, new Vector2(15, 15), 0, LayerMask.GetMask("Entity"));
        foreach (var hit in hitall)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<Player>().Damage(skillLists[2].skill);
                print(hit.name);
                var Cost = Mathf.RoundToInt(maxtlqkfGauge * 0.25f);
                print($"{Cost} : {tlqkfGauge}");
                if(tlqkfGauge >= Cost)
                {
                    tlqkfGauge -= Cost;
                    HP += Cost / 6;
                    UIManager.instance.UIUpdate();
                }
            }
        }
    }
    public override void Ultimate()
    {
        var spawnPos = (flipX ? transform.position + Vector3.right * 6 : transform.position + Vector3.left * 6) + Vector3.up * 1.5f;
        var bus = Instantiate(limbus, spawnPos, Quaternion.identity).GetComponent<Limbus>();
        bus.player = this;
        isRage = false;
        Instantiate(SpiritObj, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        GameManager.instance.UltimateFade(transform);
        StartCoroutine(UltimateProduce());
    }
    IEnumerator UltimateProduce()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
    }
    public override void NormalAttack()
    {
        Collider2D[] hitall = Physics2D.OverlapBoxAll(transform.position + new Vector3(flipX ? -2 : 2, 0.5f), new Vector2(4, 2), 0, LayerMask.GetMask("Entity"));
        foreach (var hit in hitall)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<Player>().Damage(skillLists[0].skill);
                print(hit.name);
            }
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
            HP -= hitSkill.damage * (1 + Mathf.InverseLerp(0, maxtlqkfGauge, tlqkfGauge) * 0.7f);
            if (!isRage && HP <= maxHp * 0.3f && !isGetRage)
            {
                isRage = true;
                isGetRage = true;
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
        tlqkfGauge += hitSkill.damage * 3f;
        tlqkfGauge = Mathf.Clamp(tlqkfGauge, 0, maxtlqkfGauge);

        UIManager.instance.UIUpdate();

        print(tlqkfGauge);

    }
    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + new Vector3(flipX ? -2 : 2, 0.5f), new Vector2(4, 2));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(15, 15));
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
