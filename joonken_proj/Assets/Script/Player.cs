using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{

    Rigidbody2D rigid;

    public float HP;
    public float tlqkfGauge;
    public float MoveSpeed;

    public bool stiffness; //경직 유무
    public bool isSit; //앉음 유무
    public bool isGround; //착지 유무

    [SerializeField] float RayDistance;

    public Queue<KeyCode> buttonInput = new Queue<KeyCode>();

    KeyCode[] SkillInput = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Z, KeyCode.X, KeyCode.C };

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
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
            switch (buttonInput.Dequeue())
            {
                case KeyCode.A: Dash(); break;
                case KeyCode.S: PowerSkill(); break;
                case KeyCode.D: Ultimate(); break;
                case KeyCode.Z: NormalAttack(); break;
                case KeyCode.X: SlowAttack(); break;
                case KeyCode.C: FastAttack(); break;
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

    }
    private void OnDrawGizmos() {
        Debug.DrawRay(transform.position, Vector2.down * RayDistance, Color.red);   
    }
    protected abstract void Dash();
    protected abstract void PowerSkill();
    protected abstract void Ultimate();
    protected abstract void NormalAttack();
    protected abstract void SlowAttack();
    protected abstract void FastAttack();
}
