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
        yield return new WaitForSeconds(0.58f);
        stiffness = false;
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
