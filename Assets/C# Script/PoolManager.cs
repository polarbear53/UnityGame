using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private Dictionary<GameObject, List<GameObject>> pools; //�����հ� Ǯ�� �����ϴ� ��ųʸ�
    public static PoolManager instance; //�̱��� ���� �������� �� ��ũ��Ʈ�� �ٸ� ��ũ��Ʈ������ ���� �����ϰ� ��

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

    public GameObject GetPreFab(GameObject prefab) //���� ������Ʈ�� ��ȯ�ϴ� �Լ�
    {
        if (!pools.ContainsKey(prefab)) //Ǯ �ȿ� ������Ʈ�� �ش��ϴ� ����Ʈ�� ���ٸ�
        {
            pools[prefab] = new List<GameObject>(); //���ο� ����Ʈ ����
        }

        GameObject select = null; //���� ������Ʈ�� ��ȯ�ϱ� ���� ��������

        foreach (GameObject obj in pools[prefab]) //Ǯ���� ������Ʈ�� �ش��ϴ� ����Ʈ�� Ȯ�� 
        {
            if (!obj.activeSelf) //Ǯ�� ������Ʈ�� ��Ȱ��ȭ �Ǿ��ִ°� �ִٸ� 
            {
                select = obj;// ��Ȱ��ȭ �Ǿ��ִ� ������Ʈ �Ҵ�
                select.SetActive(true); //Ȱ��ȭ ��Ű��
                break;
            }
        }
        if (select == null) // ��Ȱ��ȭ �� ������Ʈ�� ã�� ���Ѵٸ� 
        {
            select = Instantiate(prefab, transform); //���Ӱ� ������Ʈ�� �����ϰ� �Ҵ�
            pools[prefab].Add(select); //������ ������Ʈ�� Ǯ ����Ʈ�� �߰�
        }
        return select; //���� ������Ʈ ��ȯ
    }

    public void ReturnPreFab(GameObject obj) // ������Ʈ�� ��ȯ�ϴ� �Լ�
    {
        obj.SetActive(false);
    }

    public void ClearAll ()
    {
        foreach (var pool in pools.Values) //��ųʸ��� �ִ� ��� Ǯ������
        {
            while (pool.Count > 0) //Ǯ�� ������Ʈ�� �ִٸ�
            {
                GameObject obj = pool[0]; //������Ʈ�� ������
                pool.RemoveAt(0); //Ǯ���� ����
                Destroy(obj); //������Ʈ ����
            }
        }
        pools.Clear();
    }
}
