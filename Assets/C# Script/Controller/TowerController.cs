using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class TowerController : MonoBehaviour
{
    Rigidbody2D rigid2D; // 물리 이동을 위한 변수

    public float currHp; // 타워의 현재 hp
    public float maxHp = 20f; // 타워의 최대 체력

    public GameObject hpbar; // 체력바를 보이거나 보이지 않게 하기 위해
    public RectTransform hpfront; // 체력바의 스케일을 조정해 닳게하기 위해
    public GameObject towergiPrefab; // 발사할 기(투사체)
    public float attackRange; // 공격 범위
    public float fireRate; // 발사 속도 (1초에 10번 발사)
    public float towerdamage; // 공격력
    public float projectileSpeed; // 기(투사체)의 속도

    public Button TowerAttackSpeed, TowerDamage, TowerGiSpeed, TowerHpRecovery; // 레벨업 선택지 버튼(타워)

    private float fireCooldown; // 발사 쿨다운 시간

    AudioSource shootsound;
    AudioSource hurtsound;
    void Start()
    {
        attackRange = 30f;
        fireRate = 2f;
        towerdamage = 1f;
        projectileSpeed = 10f;

        rigid2D = GetComponent<Rigidbody2D>(); // rigidbody 컴포넌트 가져오기
        currHp = maxHp; // 최대 체력만큼 현재 체력 설정
        fireCooldown = 0f;

        shootsound = GameObject.Find("ShootSound").GetComponent<AudioSource>();
        hurtsound = GameObject.Find("HurtSound").GetComponent<AudioSource>();
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            GameObject target = FindMonsterInRange();
            if (target != null)
            {
                FireProjectile(target);
                fireCooldown = fireRate; // 쿨다운 초기화
            }/*
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
            }*/
        }
    }

    // "eat" 몬스터 추적
    GameObject FindMonsterInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        GameObject closestGohome = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("monster"))
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
    /*
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
    GameObject FindMonsterInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        GameObject closestGohome = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("monster"))
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
    */
    void FireProjectile(GameObject target)
    {
        shootsound.PlayOneShot(shootsound.clip);
        GameObject projectile = PoolManager.instance.GetPreFab(towergiPrefab);
        projectile.transform.position = transform.position;
        TowerGiController projectileScript = projectile.GetComponent<TowerGiController>();
        if (projectileScript != null)
        {
            projectileScript.SetTarget(target);
        }
        // 타겟 방향으로 이동
        Vector3 direction = (target.transform.position - transform.position).normalized;
        //transform.position += direction * speed * Time.deltaTime;
        projectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("monster"))
        {
            TakeDamage(collision.gameObject.GetComponent<MonsterController>().dmg, collision.gameObject);
        }/*
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
        }*/
    }

    void TakeDamage(float damage, GameObject monster)
    {
        
        hurtsound.PlayOneShot(hurtsound.clip);
        if (currHp > 0)
        {
            currHp -= damage;
            hpfront.localScale = new Vector3(currHp / maxHp, 1.0f, 1.0f);
            PoolManager.instance.ReturnPreFab(monster);
        }
        if (currHp <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        PoolManager.instance.ClearAll();
        SceneManager.LoadScene("GameOver");
    }
}
