using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiController : MonoBehaviour
{
    private float lifetime = 3f; //기가 남아있는 시간
    private float spawnTime; //생성시간을 저장하기 위한 변수

    void Start()
    {
        spawnTime = Time.time; // 생성되었을 때 현재 시간을 저장
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - spawnTime > lifetime)
        { //유지 시간이 지나면 자동 소멸 
            Destroy(this.gameObject);
        }
    }
}
