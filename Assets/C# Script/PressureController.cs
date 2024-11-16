using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureController : MonoBehaviour
{
    float speed = 1f; // 몬스터의 속도
    public float maxHp = 30f; //몬스터의 최대 체력
    private float currHp; //몬스터의 현재 hp

    GameObject player; //플레이어 오브젝트
    GameObject tower; //타워 오브젝트
    GameObject tower2; //타워2 오브젝트
    GameObject tower3; //타워3 오브젝트
    GameObject tower4; //타워4 오브젝트
    public GameObject hpbar; // 체력바를 보이거나 보이지 않게 하기 위해
    public RectTransform hpfront; // 체력바의 스케일을 조정해 닳게하기 위해

    void Start()
    {
        this.player = GameObject.Find("player"); //플레이어의 찾기
        this.tower = GameObject.Find("tower"); //타워 찾기
        this.tower2 = GameObject.Find("tower2"); //타워2 찾기 (없을수도 있음)
        this.tower3 = GameObject.Find("tower3"); //타워3 찾기 (없을수도 있음)
        this.tower4 = GameObject.Find("tower4"); //타워4 찾기 (없을수도 있음)

        currHp = maxHp; // 게임 시작시 최대체력에 따라 현재 체력 설정
        hpbar.SetActive(false); // 체력바 숨기기
    }
    void Update()
    {
        if (currHp <= 0)
        { 
            Destroy(gameObject);
            PlayerController playerController = player.GetComponent<PlayerController>(); // 플레이어의 스크립트 참조
            playerController.GainExperience(10f); // 경험치 10 증가 (적절한 값으로 조정)
            //체력이 0이되면 몬스터 비활성화(풀링 사용 때문)
            //PoolManager.instance.ReturnMonster(gameObject);
        }
    }
    void FixedUpdate()
    {
        Vector3 monsterPosition = transform.position; // 몬스터의 위치
        Vector3 playerPosition = player.GetComponent<Transform>().position; // 플레이어의 위치
        Vector3 towerPosition = tower.GetComponent<Transform>().position; // 타워의 위치

        float playerToMonster = Vector3.Distance(monsterPosition, playerPosition); // 몬스터와 플레이어간의 거리
        float towerToMonster = Vector3.Distance(monsterPosition, towerPosition); // 몬스터와 타워간의 거리

        // 타워, 타워2, 타워3, 타워4가 있을 경우
        if (tower2 != null && tower3 != null && tower4 != null)
        {
            Vector3 tower2Position = tower2.GetComponent<Transform>().position; // 타워2의 위치
            Vector3 tower3Position = tower3.GetComponent<Transform>().position; // 타워3의 위치
            Vector3 tower4Position = tower4.GetComponent<Transform>().position; // 타워4의 위치

            float tower2ToMonster = Vector3.Distance(monsterPosition, tower2Position); // 몬스터와 타워2간의 거리
            float tower3ToMonster = Vector3.Distance(monsterPosition, tower3Position); // 몬스터와 타워3간의 거리
            float tower4ToMonster = Vector3.Distance(monsterPosition, tower4Position); // 몬스터와 타워4간의 거리

            // 타워들과 플레이어의 거리 비교
            if (towerToMonster > playerToMonster && tower2ToMonster > playerToMonster && tower3ToMonster > playerToMonster && tower4ToMonster > playerToMonster)
            {
                transform.position = Vector3.MoveTowards(monsterPosition, playerPosition, speed * Time.deltaTime); // 플레이어를 따라가기
            }
            else if (tower2ToMonster < towerToMonster && tower2ToMonster < tower3ToMonster && tower2ToMonster < tower4ToMonster)
            {
                transform.position = Vector3.MoveTowards(monsterPosition, tower2Position, speed * Time.deltaTime); // 타워2로 가기
            }
            else if (tower3ToMonster < towerToMonster && tower3ToMonster < tower2ToMonster && tower3ToMonster < tower4ToMonster)
            {
                transform.position = Vector3.MoveTowards(monsterPosition, tower3Position, speed * Time.deltaTime); // 타워3로 가기
            }
            else if (tower4ToMonster < towerToMonster && tower4ToMonster < tower2ToMonster && tower4ToMonster < tower3ToMonster)
            {
                transform.position = Vector3.MoveTowards(monsterPosition, tower4Position, speed * Time.deltaTime); // 타워4로 가기
            }
            else
            {
                transform.position = Vector3.MoveTowards(monsterPosition, towerPosition, speed * Time.deltaTime); // 기본 타워로 가기
            }
        }
        // 타워, 타워2가 있을 경우
        else if (tower2 != null)
        {
            Vector3 tower2Position = tower2.GetComponent<Transform>().position; // 타워2의 위치
            float tower2ToMonster = Vector3.Distance(monsterPosition, tower2Position); // 몬스터와 타워2간의 거리

            if (towerToMonster > playerToMonster)
            {
                transform.position = Vector3.MoveTowards(monsterPosition, playerPosition, speed * Time.deltaTime); // 플레이어를 따라가기
            }
            else
            {
                transform.position = Vector3.MoveTowards(monsterPosition, tower2Position, speed * Time.deltaTime); // 타워2로 가기
            }
        }

        // 타워만 있을 경우
        else
        {
            if (towerToMonster > playerToMonster)
            {
                transform.position = Vector3.MoveTowards(monsterPosition, playerPosition, speed * Time.deltaTime); // 플레이어를 따라가기
            }
            else
            {
                transform.position = Vector3.MoveTowards(monsterPosition, towerPosition, speed * Time.deltaTime); // 타워로 가기
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("gi")) //gi 태그를 가진 오브젝트와 부딪히면
        {
            hpbar.SetActive(true); //체력바 보이기
            if (currHp > 0)
            { //현재 체력이 남아있다면
                currHp -= 1.0f; //현재 체력 갂기
                hpfront.localScale = new Vector3(currHp / maxHp, 1.0f, 1.0f); // 현재 체력을 최대 체력으로 나누어서 hp조절

                Destroy(collision.gameObject); //충돌한 기는 파괴

            }
            else
            {
                PlayerController playerController = player.GetComponent<PlayerController>(); // 플레이어의 스크립트 참조
                playerController.GainExperience(10f); // 경험치 10 증가 (적절한 값으로 조정)
            }
        }
    }
}
