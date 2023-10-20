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
        Collider2D hit = Physics2D.OverlapBox(transform.position + new Vector3(FlipX ? -2 : 2, 0.5f),new Vector2(4,2),0,LayerMask.GetMask("Entity"));
        if(hit == null) return;
        if(hit.CompareTag("Entity"))
        {
            hit.GetComponent<Player>().Damage(skillLists[0].skill);
            print(hit.name);
        }
    }
    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + new Vector3(FlipX ? -2 : 2, 0.5f),new Vector2(4,2));
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
