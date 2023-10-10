using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{

    Rigidbody2D rigid;

    public float HP;
    public float maxHp;
    public float tlqkfGauge;
    public float MoveSpeed;

    public bool stiffness; //경직 유무
    public bool isSit; //앉음 유무
    public bool isGround; //착지 유무

    public Player enemy;

    public float inputDelay; 

    [SerializeField] float RayDistance;

    public Queue<KeyCode> buttonInput = new Queue<KeyCode>();

    KeyCode[] SkillInput = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Z, KeyCode.X, KeyCode.C };

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        maxHp = HP;
    }

    private void Update()
    {
        input();
        UseSkill();
    }
    void UseSkill()
    {
        if (buttonInput.Count >= 1 && !stiffness)
        {
            stiffness = true;
            switch (buttonInput.Dequeue())
            {
                case KeyCode.A: StartCoroutine(Dash()); break;
                case KeyCode.S: StartCoroutine(PowerSkill()); break;
                case KeyCode.D: StartCoroutine(Ultimate()); break;
                case KeyCode.Z: StartCoroutine(NormalAttack()); break;
                case KeyCode.X: StartCoroutine(SlowAttack()); break;
                case KeyCode.C: StartCoroutine(FastAttack()); break;
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
        if(Input.GetKey(KeyCode.LeftArrow) && isGround)
        {
            rigid.velocity = new Vector2(-100 * Time.deltaTime * MoveSpeed,rigid.velocity.y);
        }
        if(Input.GetKey(KeyCode.RightArrow) && isGround)
        {
            rigid.velocity = new Vector2(100 * Time.deltaTime * MoveSpeed,rigid.velocity.y);
        }
        if(Input.GetKeyDown(KeyCode.UpArrow) && isGround)
        {
            rigid.AddForce(Vector2.up * 700);
        }
        isSit = Input.GetKey(KeyCode.DownArrow);
        isGround = Physics2D.Raycast(transform.position, Vector2.down,RayDistance,LayerMask.GetMask("Ground"));

        if(inputDelay >= 1) buttonInput.Clear();
        else inputDelay += Time.deltaTime;
    }
    private void OnDrawGizmos() {
        Debug.DrawRay(transform.position, Vector2.down * RayDistance, Color.red);   
    }
    protected abstract IEnumerator Dash();
    protected abstract IEnumerator PowerSkill();
    protected abstract IEnumerator Ultimate();
    protected abstract IEnumerator NormalAttack();
    protected abstract IEnumerator SlowAttack();
    protected abstract IEnumerator FastAttack();
}
