using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KimJoonwoo : Player
{
    protected override IEnumerator Dash()
    {
        print("A");
        yield return new WaitForSeconds(1);
    }
    protected override IEnumerator PowerSkill()
    {
        print("S");
        yield return new WaitForSeconds(1);
    }
    protected override IEnumerator Ultimate()
    {
        print("D");
        yield return new WaitForSeconds(1);
    }
    protected override IEnumerator NormalAttack()
    {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.3f);
        NAttack();
        print("1");
        yield return new WaitForSeconds(0.28f);
        stiffness = false;
    }
    public void NAttack()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position + new Vector3(FlipX ? -2 : 2, 0.5f),new Vector2(4,2),0,LayerMask.GetMask("Entity"));
        if(hit == null) return;
        if(hit.CompareTag("Entity"))
        {
            hit.GetComponent<Player>().Damage(skills.skill[0]);
            print(hit.name);
        }
    }
    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + new Vector3(FlipX ? -2 : 2, 0.5f),new Vector2(4,2));
        base.OnDrawGizmos();
    }
    protected override IEnumerator SlowAttack()
    {
        print("X");
        yield return new WaitForSeconds(1);
    }
    protected override IEnumerator FastAttack()
    {
        print("C");
        yield return new WaitForSeconds(1);
    }
}
