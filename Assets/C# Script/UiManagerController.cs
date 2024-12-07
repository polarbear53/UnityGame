using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UiManagerController : MonoBehaviour
{
    public static UiManagerController instance; //�̱����� 

    public GameObject levelUpPanel; // ������ �������� ���� �г�
    public Button HpUp, Recovery, DamageUp, SpeedUp; // ������ ������ ��ư(�÷��̾�)
    public Button TowerAttackSpeed, TowerDamage, TowerGiSpeed, TowerHpRecovery; // ������ ������ ��ư(Ÿ��)

    public PlayerController playerController; // �÷��̾ ����
    public TowerController towerController; // Ÿ���� ����

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        towerController = FindObjectOfType<TowerController>();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        HideLevelUpPanel(); // �ʱ⿡�� ������ �г� �����
    }

    public void ShowLevelUpPanel(string[] options, int[] randomIndexes)
    {
        HpUp.GetComponentInChildren<TextMeshProUGUI>().text = options[randomIndexes[0]];
        Recovery.GetComponentInChildren<TextMeshProUGUI>().text = options[randomIndexes[1]];
        DamageUp.GetComponentInChildren<TextMeshProUGUI>().text = options[randomIndexes[2]];
        SpeedUp.GetComponentInChildren<TextMeshProUGUI>().text = options[randomIndexes[3]];
        TowerAttackSpeed.GetComponentInChildren<TextMeshProUGUI>().text = options[randomIndexes[4]];
        TowerDamage.GetComponentInChildren<TextMeshProUGUI>().text = options[randomIndexes[5]];
        TowerGiSpeed.GetComponentInChildren<TextMeshProUGUI>().text = options[randomIndexes[6]];
        TowerHpRecovery.GetComponentInChildren<TextMeshProUGUI>().text = options[randomIndexes[7]];

        HpUp.onClick.AddListener(() => ApplyLevelUpEffect(0));
        Recovery.onClick.AddListener(() => ApplyLevelUpEffect(1));
        DamageUp.onClick.AddListener(() => ApplyLevelUpEffect(2));
        SpeedUp.onClick.AddListener(() => ApplyLevelUpEffect(3));
        TowerAttackSpeed.onClick.AddListener(() => ApplyLevelUpEffect(4));
        TowerDamage.onClick.AddListener(() => ApplyLevelUpEffect(5));
        TowerGiSpeed.onClick.AddListener(() => ApplyLevelUpEffect(6));
        TowerHpRecovery.onClick.AddListener(() => ApplyLevelUpEffect(7));


        PositionButtonsRandomly(randomIndexes); // ��ư ��ġ�� �����ϰ� ��ġ

        levelUpPanel.SetActive(true);
    }

    public void HideLevelUpPanel()
    {
        HpUp.onClick.RemoveAllListeners();
        Recovery.onClick.RemoveAllListeners();
        DamageUp.onClick.RemoveAllListeners();
        SpeedUp.onClick.RemoveAllListeners();
        TowerAttackSpeed.onClick.RemoveAllListeners();
        TowerDamage.onClick.RemoveAllListeners();
        TowerGiSpeed.onClick.RemoveAllListeners();
        TowerHpRecovery.onClick.RemoveAllListeners();

        levelUpPanel.SetActive(false);
    }

    private void ApplyLevelUpEffect(int optionIndex)
    {
        switch (optionIndex)
        {
            case 0: // �÷��̾� �ִ� ü�� up
                playerController.maxHp += 5;
                playerController.hpfront.localScale = new Vector3(playerController.currHp / playerController.maxHp, 1.0f, 1.0f);
                Debug.Log("�÷��̾� �ִ� ü�� ����!");
                Debug.Log("���� �÷��̾��� �ִ� ü��: " + playerController.maxHp);
                break;
            case 1: // �÷��̾� HP ȸ��
                playerController.currHp = playerController.maxHp;
                playerController.hpfront.localScale = new Vector3(playerController.currHp / playerController.maxHp, 1.0f, 1.0f);
                Debug.Log("�÷��̾� HP ȸ��!");
                break;
            case 2: // �÷��̾� ���ݷ� up
                playerController.giDamage += 1.0f;
                Debug.Log("�÷��̾� ���ݷ� ����!");
                Debug.Log("���� �÷��̾��� ���ݷ�: " + playerController.giDamage);
                break;
            case 3: // �÷��̾� �̵� �ӵ� up
                playerController.speed = Mathf.Min(playerController.speed + 2.0f, 20.0f); //�̵� �ӵ� ����, �ִ밪 ����
                Debug.Log("�÷��̾� �̵� �ӵ� ����!");
                Debug.Log("���� �÷��̾� �̵� �ӵ�: " + playerController.speed);
                break;
            case 4: // Ÿ�� ����ü �߻� �ӵ� up
                towerController.fireRate += 0.5f;
                Debug.Log("Ÿ�� ����ü �߻� �ӵ� ����!");
                break;
            case 5: // Ÿ�� ���ݷ� up
                towerController.towerdamage += 1;
                Debug.Log("Ÿ�� ���ݷ� ����!");
                Debug.Log("���� Ÿ���� ���ݷ�: " + towerController.towerdamage);
                break;
            case 6: // Ÿ�� ����ü �ӵ� up
                towerController.projectileSpeed += 2.0f;
                Debug.Log("Ÿ�� ����ü �ӵ� ����!");
                Debug.Log("���� Ÿ���� ����ü �ӵ�: " + towerController.projectileSpeed);
                break;
            case 7: // Ÿ�� ȸ��
                towerController.currHp = towerController.maxHp;
                playerController.hpfront.localScale = new Vector3(towerController.currHp / towerController.maxHp, 1.0f, 1.0f);
                Debug.Log("Ÿ�� ȸ��!");
                break;
        }

        // ������ �г� ����� ���� �簳
        HideLevelUpPanel();
        Time.timeScale = 1; // ���� �簳
    }

    public int[] GetRandomIndexes(int range, int count)
    {
        System.Random rand = new System.Random();
        HashSet<int> indexes = new HashSet<int>();
        while (indexes.Count < count)
        {
            indexes.Add(rand.Next(range));
        }
        return new List<int>(indexes).ToArray();
    }

    private void PositionButtonsRandomly(int[] randomIndexes)
    {
        // ī�޶��� �߽� ��ġ�� �����ɴϴ�.
        Vector3 cameraCenter = Camera.main.transform.position;

        // ��ư�� ��ġ�� ��ġ��
        Vector3[] positions = new Vector3[]
        {
            new Vector3(cameraCenter.x - 15, cameraCenter.y, 5),
            new Vector3(cameraCenter.x, cameraCenter.y, 5),
            new Vector3(cameraCenter.x + 15, cameraCenter.y, 5)
        };

        // ��ư �迭
        Button[] buttons = new Button[]
        {
            HpUp,
            Recovery,
            DamageUp,
            SpeedUp,
            TowerAttackSpeed,
            TowerDamage,
            TowerGiSpeed,
            TowerHpRecovery
        };

        // ��ư �迭 ũ�⿡ ���缭 �������� 3���� ����
        System.Random rand = new System.Random();
        List<int> selectedIndexes = new List<int>();
        while (selectedIndexes.Count < 3)
        {
            int randomIndex = rand.Next(randomIndexes.Length);
            if (!selectedIndexes.Contains(randomIndex)) // �ߺ� ����
            {
                selectedIndexes.Add(randomIndex);
            }
        }

        // ���õ� ��ư �ؽ�Ʈ ������Ʈ
        string[] options = new string[]
        {
        "Ÿ�� ����ü �ӵ� up", "�÷��̾� �̵� �ӵ� up", "�÷��̾� �ִ� ü�� up",
        "Ÿ�� ����ü �߻� �ӵ� up", "�÷��̾� HP ȸ��", "HP ���� Ÿ�� ȸ��",
        "�÷��̾� ���ݷ� up", "Ÿ�� ���ݷ� up"
        };

        // �� ��ư�� �ش��ϴ� �ؽ�Ʈ ����
        for (int i = 0; i < selectedIndexes.Count; i++)
        {
            buttons[selectedIndexes[i]].GetComponentInChildren<TextMeshProUGUI>().text = options[i];
        }

        // ��ġ �迭�� �����ϰ� ���� ���� List�� ��ȯ
        List<Vector3> positionList = new List<Vector3>(positions);
        positionList = positionList.OrderBy(x => rand.Next()).ToList(); // ���� ����

        // ���õ� ��ư�鿡 ���� ��ġ �Ҵ�
        for (int i = 0; i < selectedIndexes.Count; i++)
        {
            buttons[selectedIndexes[i]].transform.position = positionList[i];
        }
    }
}
