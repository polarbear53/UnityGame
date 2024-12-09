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

    TowerController tower;
    public float speed; // Gi 속도
    public float dmg;
    private float lifetime = 3f;
    private float spawnTime;
    private GameObject target;

    void Start()
    {
        tower = GameObject.Find("tower").GetComponent<TowerController>();
        TimeSetGi(); // 생성, 호출 되었을 때 현재 시간을 저장
        speed = tower.projectileSpeed;
    }

    void Update()
    {
        dmg = tower.towerdamage;
        if (target == null)  //타켓이 없으면 반환
        {
            PoolManager.instance.ReturnPreFab(gameObject);
            return;
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
            PoolManager.instance.ReturnPreFab(gameObject); // 기 반환
        }
    }
}