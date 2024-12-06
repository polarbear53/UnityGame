using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private Dictionary<GameObject, List<GameObject>> pools; //프리팹과 풀을 매핑하는 딕셔너리
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
            return;
        }

        DontDestroyOnLoad(gameObject);

        pools = new Dictionary<GameObject, List<GameObject>>();

    }

    public GameObject GetPreFab(GameObject prefab) //게임 오브젝트를 반환하는 함수
    {
        if (!pools.ContainsKey(prefab)) //풀 안에 오브젝트에 해당하는 리스트가 없다면
        {
            pools[prefab] = new List<GameObject>(); //새로운 리스트 생성
        }

        GameObject select = null; //게임 오브젝트를 반환하기 위한 지역변수

        foreach (GameObject obj in pools[prefab]) //풀에서 오브젝트에 해당하는 리스트에 확인 
        {
            if (!obj.activeSelf) //풀에 오브젝트가 비활성화 되어있는게 있다면 
            {
                select = obj;// 비활성화 되어있는 오브젝트 할당
                select.SetActive(true); //활성화 시키기
                break;
            }
        }
        if (select == null) // 비활성화 된 오브젝트를 찾지 못한다면 
        {
            select = Instantiate(prefab, transform); //새롭게 오브젝트를 생성하고 할당
            pools[prefab].Add(select); //생성된 오브젝트는 풀 리스트에 추가
        }
        return select; //게임 오브젝트 반환
    }

    public void ReturnPreFab(GameObject obj) // 오브젝트를 반환하는 함수
    {
        obj.SetActive(false);
    }
}
