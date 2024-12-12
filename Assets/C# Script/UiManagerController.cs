using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

public class UiManagerController : MonoBehaviour
{
    public static UiManagerController instance; //�̱����� 

    public GameObject levelUpPanel; // ������ �������� ���� �г�
    public Button HpUp, Recovery, DamageUp, SpeedUp; // ������ ������ ��ư(�÷��̾�)
    public Button TowerAttackSpeed, TowerDamage, TowerGiSpeed, TowerHpRecovery; // ������ ������ ��ư(Ÿ��)

    public PlayerController playerController; // �÷��̾ ����
    public TowerController tower; // Ÿ��1�� ����
    public TowerController tower2; // Ÿ��2�� ����
    public TowerController tower3; // Ÿ��3�� ����
    public TowerController tower4; // Ÿ��4�� ����

    void Update()
    {
        if (levelUpPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            // Ŭ���� ��ġ�� UI�� �ƴ϶�� ����
            if (!IsPointerOverUIObject())
            {
                Debug.Log("UI �ܺ� Ŭ�� ����");
                return;
            }
        }
    }

    // Pointer�� UI ���� �ִ��� Ȯ���ϴ� �޼���
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

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

    public void ShowLevelUpPanel(int[] randomIndexes)
    {
        HpUp.onClick.AddListener(() => ApplyLevelUpEffect(0));
        Recovery.onClick.AddListener(() => ApplyLevelUpEffect(1));
        DamageUp.onClick.AddListener(() => ApplyLevelUpEffect(2));
        SpeedUp.onClick.AddListener(() => ApplyLevelUpEffect(3));
        TowerAttackSpeed.onClick.AddListener(() => ApplyLevelUpEffect(4));
        TowerDamage.onClick.AddListener(() => ApplyLevelUpEffect(5));
        TowerGiSpeed.onClick.AddListener(() => ApplyLevelUpEffect(6));
        TowerHpRecovery.onClick.AddListener(() => ApplyLevelUpEffect(7));
        

        PositionButtonsRandomly(randomIndexes); // ��ư ��ġ�� �����ϰ� ��ġ

        // ó������ ��ư�� ��Ȱ��ȭ
        HideButtons();

        levelUpPanel.SetActive(true);

        // 1�� �Ŀ� ��ư�� Ȱ��ȭ
        StartCoroutine(ActivateButtonsAfterDelay(1.5f));

    }

    private void HideButtons()
    {
        HpUp.gameObject.SetActive(false);
        Recovery.gameObject.SetActive(false);
        DamageUp.gameObject.SetActive(false);
        SpeedUp.gameObject.SetActive(false);
        TowerAttackSpeed.gameObject.SetActive(false);
        TowerDamage.gameObject.SetActive(false);
        TowerGiSpeed.gameObject.SetActive(false);
        TowerHpRecovery.gameObject.SetActive(false);
    }

    private IEnumerator ActivateButtonsAfterDelay(float delay)
    {
        // 1�� ���� ��� (�Ͻ����� ���¿���)
        yield return new WaitForSecondsRealtime(delay);

        // ��ư�� Ȱ��ȭ
        foreach (Button button in new Button[] { HpUp, Recovery, DamageUp, SpeedUp, TowerAttackSpeed, TowerDamage, TowerGiSpeed, TowerHpRecovery })
        {
            button.gameObject.SetActive(true);
        }

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
                // Ÿ��, Ÿ��2, Ÿ��3, Ÿ��4�� ���� ���
                if (tower.fireRate <= 0.1f) break;
                if (tower2 != null && tower3 != null && tower4 != null)
                {
                    tower.fireRate -= 0.1f;
                    tower2.fireRate -= 0.1f;
                    tower3.fireRate -= 0.1f;
                    tower4.fireRate -= 0.1f;
                }
                // Ÿ��, Ÿ��2�� ���� ���
                else if (tower2 != null)
                {
                    tower.fireRate -= 0.1f;
                    tower2.fireRate -= 0.1f;
                }
                // Ÿ���� ���� ���
                else
                {
                    tower.fireRate -= 0.1f;
                }
                Debug.Log("Ÿ�� ����ü �߻� �ӵ� ����!");
                break;
            case 5: // Ÿ�� ���ݷ� up
                // Ÿ��, Ÿ��2, Ÿ��3, Ÿ��4�� ���� ���
                if (tower2 != null && tower3 != null && tower4 != null)
                {
                    tower.towerdamage += 1;
                    tower2.towerdamage += 1;
                    tower3.towerdamage += 1;
                    tower4.towerdamage += 1;

                    Debug.Log("���� Ÿ��1�� ���ݷ�: " + tower.towerdamage);
                    Debug.Log("���� Ÿ��2�� ���ݷ�: " + tower2.towerdamage);
                    Debug.Log("���� Ÿ��3�� ���ݷ�: " + tower3.towerdamage);
                    Debug.Log("���� Ÿ��4�� ���ݷ�: " + tower4.towerdamage);
                }
                // Ÿ��, Ÿ��2�� ���� ���
                else if (tower2 != null)
                {
                    tower.towerdamage += 1;
                    tower2.towerdamage += 1;

                    Debug.Log("���� Ÿ��1�� ���ݷ�: " + tower.towerdamage);
                    Debug.Log("���� Ÿ��2�� ���ݷ�: " + tower2.towerdamage);
                }
                // Ÿ���� ���� ���
                else
                {
                    tower.towerdamage += 1;
                    Debug.Log("���� Ÿ���� ���ݷ�: " + tower.towerdamage);
                }

                Debug.Log("Ÿ�� ���ݷ� ����!");
                break;
            case 6: // Ÿ�� ����ü �ӵ� up
                // Ÿ��, Ÿ��2, Ÿ��3, Ÿ��4�� ���� ���
                if (tower2 != null && tower3 != null && tower4 != null)
                {
                    tower.projectileSpeed += 4.0f;
                    tower2.projectileSpeed += 4.0f;
                    tower3.projectileSpeed += 4.0f;
                    tower4.projectileSpeed += 4.0f;

                    Debug.Log("���� Ÿ���� ����ü �ӵ�: " + tower.projectileSpeed);
                    Debug.Log("���� Ÿ��2�� ����ü �ӵ�: " + tower2.projectileSpeed);
                    Debug.Log("���� Ÿ��3�� ����ü �ӵ�: " + tower3.projectileSpeed);
                    Debug.Log("���� Ÿ��4�� ����ü �ӵ�: " + tower4.projectileSpeed);
                }
                // Ÿ��, Ÿ��2�� ���� ���
                else if (tower2 != null)
                {
                    tower.projectileSpeed += 4.0f;
                    tower2.projectileSpeed += 4.0f;
                    Debug.Log("���� Ÿ���� ����ü �ӵ�: " + tower.projectileSpeed);
                    Debug.Log("���� Ÿ��2�� ����ü �ӵ�: " + tower2.projectileSpeed);
                }
                // Ÿ���� ���� ���
                else
                {
                    tower.projectileSpeed += 4.0f;

                    Debug.Log("���� Ÿ���� ����ü �ӵ�: " + tower.projectileSpeed);
                }

                Debug.Log("Ÿ�� ����ü �ӵ� ����!");
                break;
            case 7: // Ÿ�� ȸ��
                // Ÿ��, Ÿ��2, Ÿ��3, Ÿ��4�� ���� ���
                if (tower2 != null && tower3 != null && tower4 != null)
                {
                    tower.currHp = tower.maxHp;
                    tower2.currHp = tower3.maxHp;
                    tower3.currHp = tower3.maxHp;
                    tower4.currHp = tower4.maxHp;

                    tower.hpfront.localScale = new Vector3(tower.currHp / tower.maxHp, 1.0f, 1.0f);
                    tower2.hpfront.localScale = new Vector3(tower2.currHp / tower2.maxHp, 1.0f, 1.0f);
                    tower3.hpfront.localScale = new Vector3(tower3.currHp / tower3.maxHp, 1.0f, 1.0f);
                    tower4.hpfront.localScale = new Vector3(tower4.currHp / tower4.maxHp, 1.0f, 1.0f);
                }
                // Ÿ��, Ÿ��2�� ���� ���
                else if (tower2 != null)
                {
                    tower.currHp = tower.maxHp;
                    tower2.currHp = tower2.maxHp;

                    tower.hpfront.localScale = new Vector3(tower.currHp / tower.maxHp, 1.0f, 1.0f);
                    tower2.hpfront.localScale = new Vector3(tower2.currHp / tower2.maxHp, 1.0f, 1.0f);
                }
                // Ÿ���� ���� ���
                else
                {
                    tower.currHp = tower.maxHp;

                    tower.hpfront.localScale = new Vector3(tower.currHp / tower.maxHp, 1.0f, 1.0f);
                }

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

        // ��ư �迭 ũ�⿡ ���� �������� 3���� ����
        if (randomIndexes.Length < 3)
        {
            Debug.LogError("randomIndexes �迭 ũ�Ⱑ 3���� �۽��ϴ�!");
            return;
        }

        List<int> selectedIndexes = randomIndexes.Take(3).ToList();

        // ��ġ �迭�� �����ϰ� ����
        System.Random rand = new System.Random();
        List<Vector3> positionList = positions.OrderBy(x => rand.Next()).ToList();

        // ���õ� ��ư�� Ȱ��ȭ�ϰ� ��ġ�� �ؽ�Ʈ�� ����
        for (int i = 0; i < selectedIndexes.Count; i++)
        {
            int buttonIndex = selectedIndexes[i];
            Button button = buttons[buttonIndex];

            // �ؽ�Ʈ ����
            button.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;

            // ��ġ ����
            button.transform.position = positionList[i];

            // ��ư Ȱ��ȭ
            button.gameObject.SetActive(true);
        }

        // ������ ��ư ��Ȱ��ȭ
        for (int i = 0; i < buttons.Length; i++)
        {
            if (!selectedIndexes.Contains(i))
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }
}
