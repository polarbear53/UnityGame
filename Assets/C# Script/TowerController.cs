using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerController : MonoBehaviour
{
    Rigidbody2D rigid2D; // 물리 이동을 위한 변수

    private float currHp; // 타워의 현재 hp
    public float maxHp = 20f; // 타워의 최대 체력

    public GameObject hpbar; // 체력바를 보이거나 보이지 않게 하기 위해
    public RectTransform hpfront; // 체력바의 스케일을 조정해 닳게하기 위해
    public GameObject towergiPrefab; // 발사할 기(투사체)
    public float attackRange = 10f; // 공격 범위
    public float fireRate = 3f; // 발사 속도 (1초에 10번 발사)

    private float fireCooldown; // 발사 쿨다운 시간

    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>(); // rigidbody 컴포넌트 가져오기
        currHp = maxHp; // 최대 체력만큼 현재 체력 설정
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
                fireCooldown = fireRate; // 쿨다운 초기화
            }
            GameObject target2 = FindeatInRange();
            if (target2 != null)
            {
                FireProjectile(target2);
                fireCooldown = fireRate; // 쿨다운 초기화
            }
            GameObject target3 = FindwhatInRange();
            if (target3 != null)
            {
                FireProjectile(target3);
                fireCooldown = fireRate; // 쿨다운 초기화
            }
            GameObject target4 = FindnoInRange();
            if (target4 != null)
            {
                FireProjectile(target4);
                fireCooldown = fireRate; // 쿨다운 초기화
            }
            GameObject target5 = FindpressureInRange();
            if (target5 != null)
            {
                FireProjectile(target5);
                fireCooldown = fireRate; // 쿨다운 초기화
            }
        }
    }

    // "eat" 몬스터 추적
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

    // "pressure" 몬스터 추적
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

    // "no" 몬스터 추적
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

    // "what" 몬스터 추적
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

    // "gohome" 몬스터 추적
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
        GameObject projectile = Instantiate(towergiPrefab, transform.position, Quaternion.identity);
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
            Destroy(monster);
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
