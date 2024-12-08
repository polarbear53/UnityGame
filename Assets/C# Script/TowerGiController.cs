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

    public float speed = 10f; // Gi �ӵ�
    private float lifetime = 3f;
    private float spawnTime;
    private GameObject target;

    void Start()
    {
        TimeSetGi(); // ����, ȣ�� �Ǿ��� �� ���� �ð��� ����
    }

    void Update()
    {
        if (target == null)  //Ÿ���� ������ ��ȯ
        {
            PoolManager.instance.ReturnPreFab(gameObject);
            return;
        }
        else {
            // Ÿ�� �������� �̵�
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
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

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            // Ÿ�ٿ� �������� �� ������ ������
            var targetController = collision.GetComponent<MonsterController>();
            if (targetController != null)
            {
                targetController.TakeDamage(1.0f); // ������ 1 �ֱ�
            }
            PoolManager.instance.ReturnPreFab(gameObject); // �� ��ȯ
        }
    }
}