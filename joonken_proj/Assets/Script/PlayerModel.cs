using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] private Player player;

    public void Dash() => player.Dash();
    public void PowerSkill() => player.PowerSkill();
    public void Ultimate() => player.Ultimate();
    public void NormalAttack() => player.NormalAttack();
    public void SlowAttack() => player.SlowAttack();
    public void FastAttack() => player.FastAttack();

    public void AttackAnimationEnd() => player.AttackAnimationEnd();
    public void AttackAnimationStart() => player.AttackAnimationStart();
}
