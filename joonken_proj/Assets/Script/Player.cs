using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class Skill
{
    public int damage;
    public Vector2 dir;
    public float throwPower;
    public float guardStiffTime;
    public float hitStiffTime;
    public bool isThrow;
}

public abstract class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    [SerializeField] SpriteRenderer sp;
    public Animator anim;
    public SkillList skills;

    public float HP;
    public float maxHp;
    public float tlqkfGauge;
    public float MoveSpeed;

    public bool stiffness; //경직 유무
    public bool isSit; //앉음 유무
    public bool isGround; //착지 유무
    public bool isGuard; //방어 유무

    public bool isHit; //피격 유무

    public Player enemy;

    public float inputDelay; //선입력 가능 시간
    public float inputX; //X축 horizontal Input
    public bool FlipX = false;

    protected Coroutine skillCoroutine;
    protected Coroutine stiffCoroutine;

    [SerializeField] float RayDistance;


    public Queue<KeyCode> buttonInput = new Queue<KeyCode>();

    KeyCode[] SkillInput = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Z, KeyCode.X, KeyCode.C };

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        maxHp = HP;
    }

    protected virtual void Update()
    {
        input();
        UseSkill();
        HitRotation();
        if (Input.GetKeyDown(KeyCode.J))
        {
            Skill skill = new Skill();
            skill.damage = 15;
            skill.dir = new Vector2(-1, 1);
            skill.throwPower = 10;
            skill.isThrow = true;
            Damage(skill);
        }
    }
    void UseSkill()
    {
        if (buttonInput.Count >= 1 && !stiffness)
        {
            stiffness = true;

            if (skillCoroutine != null) StopCoroutine(skillCoroutine);
            switch (buttonInput.Dequeue())
            {
                case KeyCode.A: skillCoroutine = StartCoroutine(Dash()); break;
                case KeyCode.S: skillCoroutine = StartCoroutine(PowerSkill()); break;
                case KeyCode.D: skillCoroutine = StartCoroutine(Ultimate()); break;
                case KeyCode.Z: skillCoroutine = StartCoroutine(NormalAttack()); break;
                case KeyCode.X: skillCoroutine = StartCoroutine(SlowAttack()); break;
                case KeyCode.C: skillCoroutine = StartCoroutine(FastAttack()); break;
            }
        }
    }
    void HitRotation()
    {
        if (isHit)
        {
            sp.transform.Rotate(new Vector3(0, 0, 750) * Time.deltaTime);
        }
    }
    void input()
    {
        foreach (var key in SkillInput)
        {
            if (Input.GetKeyDown(key))
            {
                buttonInput.Enqueue(key);
                inputDelay = 0;
            }
        }
        inputX = GetHorizontalInput();
        if (!stiffness)
        {
            anim.SetInteger("Walk", (int)inputX * (FlipX ? -1 : 1));
            rigid.velocity = new Vector2(inputX * 100 * Time.deltaTime * MoveSpeed, rigid.velocity.y);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGround)
        {
            transform.Translate(Vector2.up * 0.2f);
            anim.SetTrigger("Jump");
            rigid.AddForce(Vector2.up * 700);
        }
        isSit = Input.GetKey(KeyCode.DownArrow);
        var ground = Physics2D.Raycast(transform.position, Vector2.down, RayDistance, LayerMask.GetMask("Ground"));
        isGround = ground;
        if (isGround)
        {
            if (isHit)
            {
                Stiff(1);
                isHit = false;
                sp.transform.localPosition = new Vector3(0, -1.3f, 0);
                sp.transform.localEulerAngles = new Vector3(0, 0, 90);
                sp.transform.DOLocalMove(new Vector2(0, 0), 1).SetEase(Ease.InQuint);
                sp.transform.DOLocalRotate(Vector3.zero, 1).SetEase(Ease.InQuint);
            }
        }
        sp.flipX = FlipX;
        anim.SetBool("IsGround", isGround);
        if (inputDelay >= 0.5f) buttonInput.Clear();
        else inputDelay += Time.deltaTime;
    }
    float GetHorizontalInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            return 0;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            return -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            return 1f;
        }
        return 0;
    }
    public void ResetAnim()
    {
        stiffness = false;
    }
    protected virtual void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector2.down * RayDistance, Color.red);
    }
    public virtual void Damage(Skill hitSkill)
    {
        if (isGuard)
        {
            Stiff(hitSkill.guardStiffTime);
        }
        else
        {
            HP -= hitSkill.damage;
            if (hitSkill.isThrow)
            {
                transform.Translate(Vector2.up * 0.5f);
                stiffness = true;
                isHit = true;
                rigid.velocity = hitSkill.dir * hitSkill.damage * (FlipX ? -1 : 1);
            }
            else
            {
                Stiff(hitSkill.hitStiffTime);
            }

        }
    }
    public void Stiff(float stiffTime)
    {
        if (stiffCoroutine != null) StopCoroutine(stiffCoroutine);
        stiffCoroutine = StartCoroutine(StiffCoroutine(stiffTime));
    }
    public IEnumerator StiffCoroutine(float stiffTime)
    {
        stiffness = true;
        yield return new WaitForSeconds(stiffTime);
        stiffness = false;
    }
    protected abstract IEnumerator Dash();
    protected abstract IEnumerator PowerSkill();
    protected abstract IEnumerator Ultimate();
    protected abstract IEnumerator NormalAttack();
    protected abstract IEnumerator SlowAttack();
    protected abstract IEnumerator FastAttack();
}
