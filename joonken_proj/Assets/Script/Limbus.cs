using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limbus : MonoBehaviour
{
    [SerializeField] float MoveSpeed;
    public Player player;
    void Start()
    {
        var angle = player.flipX ? 180 : 0;
        transform.eulerAngles = new Vector3(0,angle,0);
        StartCoroutine(Move());
        Destroy(gameObject,5);
    }
    IEnumerator Move()
    {
        var Speed = MoveSpeed;
        MoveSpeed = 0;
        yield return new WaitForSeconds(0.2f);
        MoveSpeed = Speed;
    }
    void Update()
    {
        transform.Translate(Vector2.right * MoveSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<Player>().Damage(player.skillLists[1].skill);
        }
    }
}
