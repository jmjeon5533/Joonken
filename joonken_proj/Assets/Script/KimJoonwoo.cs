using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KimJoonwoo : Player
{
    protected override void Dash()
    {
        print("A");
    }
    protected override void PowerSkill()
    {
        print("S");
    }
    protected override void Ultimate()
    {
        print("D");
    }
    protected override void NormalAttack()
    {
        print("Z");
    }
    protected override void SlowAttack()
    {
        print("X");
    }
    protected override void FastAttack()
    {
        print("C");
    }
}
