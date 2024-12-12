using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class TowerGiController : MonoBehaviour
{
    public Button TowerAttackSpeed, TowerDamage, TowerGiSpeed, TowerHpRecovery; // ������ ������ ��ư(Ÿ��)

    TowerController tower;
    public float speed; // Gi �ӵ�
    public float dmg;
    private float lifetime = 3f;
    private float spawnTime;
    private GameObject target;

    void Start()
    {
        tower = GameObject.Find("tower").GetComponent<TowerController>();
        TimeSetGi(); // ����, ȣ�� �Ǿ��� �� ���� �ð��� ����
        speed = tower.projectileSpeed;
    }

    void Update()
    {
        dmg = tower.towerdamage;
        if (target == null)  //Ÿ���� ������ ��ȯ
        {
            PoolManager.instance.ReturnPreFab(gameObject);
            return;
        }
        // ���� �ð��� ������ �Ҹ�
        if (Time.time - spawnTime > lifetime)
        {
            PoolManager.instance.ReturnPreFab(gameObject);
        }
    }

    private void TimeSetGi()
    {
        spawnTime = Time.time; // ����, Ȱ��ȭ �Ǿ��� �� ���� �ð��� ����
    }

    private void OnEnable()
    {
        TimeSetGi(); // Ȱ��ȭ�� �� �� �ð� �ʱ�ȭ
    }

    // Ÿ�� ����
    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            PoolManager.instance.ReturnPreFab(gameObject); // �� ��ȯ
        }
    }
}