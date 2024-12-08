using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class TowerController : MonoBehaviour
{
    Rigidbody2D rigid2D; // ���� �̵��� ���� ����

    public float currHp; // Ÿ���� ���� hp
    public float maxHp = 20f; // Ÿ���� �ִ� ü��

    public GameObject hpbar; // ü�¹ٸ� ���̰ų� ������ �ʰ� �ϱ� ����
    public RectTransform hpfront; // ü�¹��� �������� ������ ����ϱ� ����
    public GameObject towergiPrefab; // �߻��� ��(����ü)
    public float attackRange = 10f; // ���� ����
    public float fireRate = 3f; // �߻� �ӵ� (1�ʿ� 10�� �߻�)
    public float towerdamage = 1f; // ���ݷ�
    public float projectileSpeed = 5f; // ��(����ü)�� �ӵ�

    public Button TowerAttackSpeed, TowerDamage, TowerGiSpeed, TowerHpRecovery; // ������ ������ ��ư(Ÿ��)

    private float fireCooldown; // �߻� ��ٿ� �ð�

    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>(); // rigidbody ������Ʈ ��������
        currHp = maxHp; // �ִ� ü�¸�ŭ ���� ü�� ����
        fireCooldown = 0f;
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            GameObject target = FindgohomeInRange();
            if (target != null)
            {
                FireProjectile(target);
                fireCooldown = fireRate; // ��ٿ� �ʱ�ȭ
            }
            GameObject target2 = FindeatInRange();
            if (target2 != null)
            {
                FireProjectile(target2);
                fireCooldown = fireRate; // ��ٿ� �ʱ�ȭ
            }
            GameObject target3 = FindwhatInRange();
            if (target3 != null)
            {
                FireProjectile(target3);
                fireCooldown = fireRate; // ��ٿ� �ʱ�ȭ
            }
            GameObject target4 = FindnoInRange();
            if (target4 != null)
            {
                FireProjectile(target4);
                fireCooldown = fireRate; // ��ٿ� �ʱ�ȭ
            }
            GameObject target5 = FindpressureInRange();
            if (target5 != null)
            {
                FireProjectile(target5);
                fireCooldown = fireRate; // ��ٿ� �ʱ�ȭ
            }
        }
    }

    // "eat" ���� ����
    GameObject FindeatInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        GameObject closestGohome = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("eat"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestGohome = hit.gameObject;
                }
            }
        }

        return closestGohome;
    }

    // "pressure" ���� ����
    GameObject FindpressureInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        GameObject closestGohome = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("pressure"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestGohome = hit.gameObject;
                }
            }
        }

        return closestGohome;
    }

    // "no" ���� ����
    GameObject FindnoInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        GameObject closestGohome = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("no"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestGohome = hit.gameObject;
                }
            }
        }

        return closestGohome;
    }

    // "what" ���� ����
    GameObject FindwhatInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        GameObject closestGohome = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("what"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestGohome = hit.gameObject;
                }
            }
        }

        return closestGohome;
    }

    // "gohome" ���� ����
    GameObject FindgohomeInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        GameObject closestGohome = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("gohome"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestGohome = hit.gameObject;
                }
            }
        }

        return closestGohome;
    }

    void FireProjectile(GameObject target)
    {
        GameObject projectile = PoolManager.instance.GetPreFab(towergiPrefab);
        projectile.transform.position = transform.position;
        TowerGiController projectileScript = projectile.GetComponent<TowerGiController>();
        if (projectileScript != null)
        {
            projectileScript.SetTarget(target);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("eat"))
        {
            TakeDamage(1.0f, collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("gohome"))
        {
            TakeDamage(1.0f, collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("what"))
        {
            TakeDamage(2.0f, collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("no"))
        {
            TakeDamage(3.0f, collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("pressure"))
        {
            TakeDamage(5.0f, collision.gameObject);
        }
    }

    void TakeDamage(float damage, GameObject monster)
    {
        if (currHp > 0)
        {
            currHp -= damage;
            hpfront.localScale = new Vector3(currHp / maxHp, 1.0f, 1.0f);
            PoolManager.instance.ReturnPreFab(monster);
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
