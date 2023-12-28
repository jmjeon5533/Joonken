using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class Skill
{
    public float damage;
    public Vector2 dir;
    public float throwPower;
    public float guardStiffTime;
    public float hitStiffTime;
    public bool isThrow;
}

public abstract class Player : MonoBehaviour
{
    //member
    public bool stiffness; //경직 유무
    public bool isSit; //앉음 유무
    public bool isGround; //착지 유무
    public bool isGuard; //방어 유무
    public bool isHit; //피격 유무
    public bool isRage; //레이지 유무
    public bool flipX = false;
    public float HP;
    public float maxHp;
    public float tlqkfGauge;
    public float maxtlqkfGauge;
    public float MoveSpeed;
    public float inputDelay; //선입력 가능 시간
    public float inputX; //X축 horizontal Input
    [SerializeField] float RayDistance;
    [SerializeField] SpriteRenderer sp;
    [SerializeField] protected GameObject RageObj;
    public Animator anim;
    public Player enemy;
    protected Rigidbody2D rigid;
    public List<SkillList> skillLists = new List<SkillList>();
    public Queue<KeyCode> buttonInput = new Queue<KeyCode>();

    protected Coroutine skillCoroutine;
    protected Coroutine stiffCoroutine;

    private KeyCode preInput;
    public float comboWaitTime;

    KeyCode[] SkillInput = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Z, KeyCode.X, KeyCode.C };

    public void AttackAnimationEnd()
    {
        anim.SetFloat("KeyPressRemainTime", 0.25f);
        stiffness = false;
        comboWaitTime = 0.25f;
    }
    public void AttackAnimationStart()
    {
        stiffness = true;
    }
    protected virtual void Start()
    {
        RageObj.SetActive(false);
        rigid = GetComponent<Rigidbody2D>();
        maxHp = HP;
        tlqkfGauge = 0;
        maxtlqkfGauge = 175;
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
            skill.hitStiffTime = 0.8f;
            skill.guardStiffTime = 0.3f;
            Damage(skill);
        }
    }
    void UseSkill()
    {
        if (buttonInput.Count >= 1 && !stiffness)
        {
            if (skillCoroutine != null) StopCoroutine(skillCoroutine);

            var input = buttonInput.Dequeue();
            var comboIndex = anim.GetInteger($"{input.ToString()[^1]}AttackIndex");
            var comboMaxIndex = anim.GetInteger($"{input.ToString()[^1]}AttackMaxIndex");
            Debug.Log(preInput);

            if(comboWaitTime > 0 && preInput == input && comboIndex != comboMaxIndex)
                anim.SetInteger($"{input.ToString()[^1]}AttackIndex", comboIndex + 1);
            else 
                anim.SetInteger($"{preInput.ToString()[^1]}AttackIndex", 0);

            anim.SetTrigger($"{input.ToString()[^1]}");
            preInput = input;
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
            }
        }
        inputX = GetHorizontalInput();
        if (!stiffness)
        {
            anim.SetInteger("Walk", (int)inputX * (flipX ? -1 : 1));
            rigid.velocity = new Vector2(inputX * 100 * Time.deltaTime * MoveSpeed, rigid.velocity.y);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGround && !stiffness)
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
        sp.flipX = flipX;
        anim.SetBool("IsGround", isGround);
        anim.SetBool("IsRage", isRage);
        anim.SetBool("Stiffness", stiffness);
        if (inputDelay >= 0.5f)
        {
            InputReset();
        }
        else
        {
            if (buttonInput.Count > 0)
                inputDelay += Time.deltaTime;
            else
                inputDelay = 0;
        }

        if(comboWaitTime > 0)
            comboWaitTime -= Time.deltaTime;
    }
    protected virtual void InputReset()
    {
        buttonInput.Clear();
        inputDelay = 0;
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
        tlqkfGauge += hitSkill.damage * 1.5f;
        tlqkfGauge = Mathf.Clamp(tlqkfGauge,0,maxtlqkfGauge);

        UIManager.instance.UIUpdate();
        print(tlqkfGauge);
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
    public abstract void Dash();
    public abstract void PowerSkill();
    public abstract void Ultimate();
    public abstract void NormalAttack();
    public abstract void SlowAttack();
    public abstract void FastAttack();
}
