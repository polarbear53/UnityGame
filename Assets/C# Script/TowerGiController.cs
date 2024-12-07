using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class TowerGiController : MonoBehaviour
{
    public Button TowerAttackSpeed, TowerDamage, TowerGiSpeed, TowerHpRecovery; // 레벨업 선택지 버튼(타워)

    public float speed = 10f; // Gi 속도
    private float lifetime = 3f;
    private float spawnTime;
    private GameObject target;

    void Start()
    {
        TimeSetGi(); // 생성, 호출 되었을 때 현재 시간을 저장
    }

    void Update()
    {
        if (target == null)  //타켓이 없으면 반환
        {
            PoolManager.instance.ReturnPreFab(gameObject);
            return;
        }
        else {
            // 타겟 방향으로 이동
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }

        // 유지 시간이 지나면 소멸
        if (Time.time - spawnTime > lifetime)
        {
            PoolManager.instance.ReturnPreFab(gameObject);
        }
    }

    private void TimeSetGi()
    {
        spawnTime = Time.time; // 생성, 활성화 되었을 때 현재 시간을 저장
    }

    private void OnEnable()
    {
        TimeSetGi(); // 활성화될 때 기 시간 초기화
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
            PoolManager.instance.ReturnPreFab(gameObject); // 기 반환
        }
    }
}