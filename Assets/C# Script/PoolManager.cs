using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//작동이 안되요 ㅠㅠㅠ

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs; //prefabs를 보관한 배열
    private List<GameObject>[] pools; //풀링을 담당하는 리스트 변수
    public static PoolManager instance; //싱글톤 패턴 구현으로 이 스크립트를 다른 스크립트에서도 접근 가능하게 함

    void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        pools = new List<GameObject>[prefabs.Length]; //prefab의 갯수 만큼 리스트 배열 생성

        for (int i = 0; i < prefabs.Length; i++) //리스트 초기화
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject GetMonster(int Index) //게임 오브젝트를 반환하는 함수
    {
        GameObject select = null; //게임 오브젝트를 반환하기 위한 지역변수

        foreach (GameObject item in pools[Index]) //pool의 인덱스 리스트의 데이터에 접근
        {
            if (!item.activeSelf)
            { //내용물 오브젝트가 비활성화 되어있는게 있다면 
                select = item;// 비활성화 되어있는 오브젝트 할당
                select.SetActive(true); //활성화 시키기
                break;
            }
        }
        if (!select) 
        {
            select = Instantiate(prefabs[Index], transform); //새롭게 오브젝트를 생성하고 할당
            pools[Index].Add(select); //생성된 오브젝트는 풀 리스트에 추가
        }
        return select; //게임 오브젝트 반환
    }

    public void ReturnMonster(GameObject monster) //몬스터 오브젝트를 반환하는 함수
    {
        monster.SetActive(false);
    }
}
