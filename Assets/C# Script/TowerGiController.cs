using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGiController : MonoBehaviour
{
    public float speed = 10f; // Gi 속도
    private float lifetime = 3f;
    private float spawnTime;
    private GameObject target;

    void Start()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        if (target == null) // 타겟이 없으면 소멸
        {
            Destroy(gameObject);
            return; //타켓이 없으면 실행되지 않도록
        }
        else {
            // 타겟 방향으로 이동
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }

        // 유지 시간이 지나면 소멸
        if (Time.time - spawnTime > lifetime)
        {
            Destroy(gameObject);
        }
    }

    // 타겟 설정
    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            // 타겟에 도달했을 때 데미지 입히기
            var targetController = collision.GetComponent<MonsterController>();
            if (targetController != null)
            {
                targetController.TakeDamage(1.0f); // 데미지 1 주기
            }
            Destroy(gameObject); // 투사체 소멸
        }
    }
}