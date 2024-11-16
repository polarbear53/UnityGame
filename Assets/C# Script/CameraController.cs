using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player; // 플레이어의 위치를 가져오기 위해
    float cameraMoveSpeed = 1f; // 카메라 이동 속도 변수
    Vector3 cameraPosition = new Vector3(0, 0, -5); // 카메라의 z좌표를 이동시켜 부드럽게 카메라를 움직이기 위해
    Vector2 mapSize_half = new Vector2(57, 32); // 맵 크기 크기 절반
    Vector2 cameraSize = new Vector2(21.5f, 9.5f); // 카메라 크기와 플레이어를 고려한 크기

    void Start()
    {
        this.player = GameObject.Find("player");
    }

    
    void Update()
    {
        //유니티에서 자체적으로 제공하는 Lerp함수를 이용해 카메라의 부드러운 움직임을 표현
        transform.position = Vector3.Lerp(transform.position, this.player.transform.position + cameraPosition, Time.deltaTime * cameraMoveSpeed);

        //카메라가 일정 화면 밖으로 나가지 않도록 하는 코드
        float clampX = Mathf.Clamp(transform.position.x, -mapSize_half.x + cameraSize.x, mapSize_half.x - cameraSize.x);
        float clampY = Mathf.Clamp(transform.position.y, -mapSize_half.y + cameraSize.y, mapSize_half.y - cameraSize.y);

        transform.position = new Vector3(clampX, clampY, -10f);

    }
}
