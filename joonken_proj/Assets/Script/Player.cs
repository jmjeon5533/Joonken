using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer sp;
    public Animator anim;

    public float HP;
    public float maxHp;
    public float tlqkfGauge;
    public float MoveSpeed;

    public bool stiffness; //경직 유무
    public bool isSit; //앉음 유무
    public bool isGround; //착지 유무

    public Player enemy;

    public float inputDelay; //선입력 가능 시간
    public float inputX; //X축 horizontal Input
    public bool FlipX = false;

    protected Coroutine skillCoroutine;

    [SerializeField] float RayDistance;

    public Queue<KeyCode> buttonInput = new Queue<KeyCode>();

    KeyCode[] SkillInput = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Z, KeyCode.X, KeyCode.C };

    protected virtual void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        maxHp = HP;
    }

    protected virtual void Update()
    {
        input();
        UseSkill();
    }
    void UseSkill()
    {
        if (buttonInput.Count >= 1 && !stiffness)
        {
            stiffness = true;
            print(buttonInput.Count);

            if(skillCoroutine != null) StopCoroutine(skillCoroutine);
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
        if(!stiffness) rigid.velocity = new Vector2(inputX * 100 * Time.deltaTime * MoveSpeed, rigid.velocity.y);
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGround)
        {
            transform.Translate(Vector2.up * 0.2f);
            anim.SetTrigger("Jump");
            rigid.AddForce(Vector2.up * 700);
        }
        isSit = Input.GetKey(KeyCode.DownArrow);
        isGround = Physics2D.Raycast(transform.position, Vector2.down, RayDistance, LayerMask.GetMask("Ground"));
        anim.SetInteger("Walk",(int)inputX * (FlipX ? -1 : 1));
        sp.flipX = FlipX;
        anim.SetBool("IsGround",isGround);
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
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector2.down * RayDistance, Color.red);
    }
    protected abstract IEnumerator Dash();
    protected abstract IEnumerator PowerSkill();
    protected abstract IEnumerator Ultimate();
    protected abstract IEnumerator NormalAttack();
    protected abstract IEnumerator SlowAttack();
    protected abstract IEnumerator FastAttack();
}
