using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; //스폰 포인트의 위치를 저장
    public GameObject[] prefabs;
    public GameObject tower2;  // 타워2 오브젝트
    public GameObject tower3;  // 타워3 오브젝트
    public GameObject tower4;  // 타워4 오브젝트

    private float spawnTime = 0f;        // 스폰 타이머
    private float elapsedTime = 0f;     // 게임 총 경과 시간
    private float spawnDelay = 1f;      // 초기 스폰 딜레이 (1초)
    private const float spawnDelayReduction = 0.2f; // 1분 경과 시 딜레이 감소 값
    private const float minSpawnDelay = 0.3f;       // 최소 스폰 딜레이 값

    void Awake() 
    {
        spawnPoint = GetComponentsInChildren<Transform>(); //스포너가 자식으로 갖고있는 포인트들의 위치를 가져옴
    }
   
    void Update()
    {
        spawnTime += Time.deltaTime;   // 스폰 타이머 증가
        elapsedTime += Time.deltaTime; // 총 경과 시간 증가

        // 1분마다 스폰 딜레이 감소
        if (elapsedTime >= 60f)
        {
            IncreaseSpawnSpeed();
            elapsedTime = 0f; // 경과 시간 초기화
        }

        // 스폰 딜레이 초과 시 몬스터 스폰
        if (spawnTime > spawnDelay)
        {
            spawnTime = 0f; // 타이머 초기화
            Spawn();        // 몬스터 스폰
        }
    }

    void IncreaseSpawnSpeed()
    {
        if (spawnDelay > minSpawnDelay) // 최소 스폰 딜레이보다 크면 감소
        {
            spawnDelay -= spawnDelayReduction;
            Debug.Log($"Spawn speed increased! New spawn delay: {spawnDelay:F2}s");
        }
        else
        {
            Debug.Log("Spawn delay is at its minimum!");
        }
    }

    void Spawn()
    {
        // 타워, 타워2, 타워3, 타워4가 있을 경우
        if (tower2 != null && tower3 != null && tower4 != null)
        {
            GameObject monster2 = Instantiate(prefabs[Random.Range(0, 5)], transform);
            monster2.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        }

        // 타워, 타워2가 있을 경우
        else if (tower2 != null)
        {
            GameObject monster2 = Instantiate(prefabs[Random.Range(0, 3)], transform);
            monster2.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        }
        else
        {
            GameObject monster = Instantiate(prefabs[Random.Range(0, 2)], transform); //몬스터 생성하기(프리팹에 있는 몬스터중 랜덤하게 0번째 인덱스의 몬스터부터 1번째 몬스터)
            monster.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;// 스폰 위치는 spawnPoint의 1번부터 마지막까지(0번은 스포너 위치(0,0))
        }
    }
}
