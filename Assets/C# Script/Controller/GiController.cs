using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiController : MonoBehaviour
{
    private float lifetime = 3f; //�Ⱑ �����ִ� �ð�
    private float spawnTime; //�����ð��� �����ϱ� ���� ����

    void Start()
    {
        TimeSetGi();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - spawnTime > lifetime)
        { //���� �ð��� ������ �ڵ� �Ҹ� 
            PoolManager.instance.ReturnPreFab(gameObject);
            Debug.Log("�Ⱑ ��Ȱ��ȭ �Ǿ����ϴ�");
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
}
