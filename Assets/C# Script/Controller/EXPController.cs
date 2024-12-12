using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPController : MonoBehaviour
{
    GameObject player;
    Vector2 dirVec;
    Vector2 nextVec;
    Rigidbody2D rigid;
    float speed;
    float range;
    public float exppoint;
    // Update is called once per frame
    private void Awake()
    {
        range = 7f;
        rigid = GetComponent<Rigidbody2D>();
        player = GameObject.Find("player"); //플레이어 오브젝트 찾기
        exppoint = 10;
    }
    void FixedUpdate()
    {
        if (Vector2.Distance(gameObject.transform.position, player.transform.position) < range)
        {
            speed += Time.deltaTime * 100;
            dirVec = player.transform.position - gameObject.transform.position;
            nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
        }
        else 
        {
            speed = 0;
        }
    }
}
