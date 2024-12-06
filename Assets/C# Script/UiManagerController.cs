using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UiManagerController : MonoBehaviour
{
    public static UiManagerController instance; //싱글패턴 

    public GameObject levelUpPanel; // 레벨업 선택지를 담은 패널
    public Button HpUp, Recovery, DamageUp, SpeedUp; // 레벨업 선택지 버튼(플레이어)
    public Button TowerAttackSpeed, TowerDamage, TowerGiSpeed, TowerHpRecovery; // 레벨업 선택지 버튼(타워)

    public PlayerController playerController; // 플레이어에 접근
    public TowerController towerController; // 타워에 접근

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

        HideLevelUpPanel(); // 초기에는 레벨업 패널 숨기기
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


        PositionButtonsRandomly(randomIndexes); // 버튼 위치를 랜덤하게 배치

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
            case 0: // 플레이어 최대 체력 up
                playerController.maxHp += 5;
                playerController.hpfront.localScale = new Vector3(playerController.currHp / playerController.maxHp, 1.0f, 1.0f);
                Debug.Log("플레이어 최대 체력 증가!");
                Debug.Log("현재 플레이어의 최대 체력: " + playerController.maxHp);
                break;
            case 1: // 플레이어 HP 회복
                playerController.currHp = playerController.maxHp;
                playerController.hpfront.localScale = new Vector3(playerController.currHp / playerController.maxHp, 1.0f, 1.0f);
                Debug.Log("플레이어 HP 회복!");
                break;
            case 2: // 플레이어 공격력 up
                playerController.giDamage += 1.0f;
                Debug.Log("플레이어 공격력 증가!");
                Debug.Log("현재 플레이어의 공격력: " + playerController.giDamage);
                break;
            case 3: // 플레이어 이동 속도 up
                playerController.speed = Mathf.Min(playerController.speed + 2.0f, 20.0f); //이동 속도 증가, 최대값 제한
                Debug.Log("플레이어 이동 속도 증가!");
                Debug.Log("현재 플레이어 이동 속도: " + playerController.speed);
                break;
            case 4: // 타워 투사체 발사 속도 up
                towerController.fireRate += 0.5f;
                Debug.Log("타워 투사체 발사 속도 증가!");
                break;
            case 5: // 타워 공격력 up
                towerController.towerdamage += 1;
                Debug.Log("타워 공격력 증가!");
                Debug.Log("현재 타워의 공격력: " + towerController.towerdamage);
                break;
            case 6: // 타워 투사체 속도 up
                towerController.projectileSpeed += 2.0f;
                Debug.Log("타워 투사체 속도 증가!");
                Debug.Log("현재 타워의 투사체 속도: " + towerController.projectileSpeed);
                break;
            case 7: // 타워 회복
                towerController.currHp = towerController.maxHp;
                playerController.hpfront.localScale = new Vector3(towerController.currHp / towerController.maxHp, 1.0f, 1.0f);
                Debug.Log("타워 회복!");
                break;
        }

        // 레벨업 패널 숨기고 게임 재개
        HideLevelUpPanel();
        Time.timeScale = 1; // 게임 재개
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
        // 카메라의 중심 위치를 가져옵니다.
        Vector3 cameraCenter = Camera.main.transform.position;

        // 버튼이 배치될 위치들
        Vector3[] positions = new Vector3[]
        {
            new Vector3(cameraCenter.x - 15, cameraCenter.y, 5),
            new Vector3(cameraCenter.x, cameraCenter.y, 5),
            new Vector3(cameraCenter.x + 15, cameraCenter.y, 5)
        };

        // 버튼 배열
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

        // 버튼 배열 크기에 맞춰서 랜덤으로 3개만 선택
        System.Random rand = new System.Random();
        List<int> selectedIndexes = new List<int>();
        while (selectedIndexes.Count < 3)
        {
            int randomIndex = rand.Next(randomIndexes.Length);
            if (!selectedIndexes.Contains(randomIndex)) // 중복 방지
            {
                selectedIndexes.Add(randomIndex);
            }
        }

        // 선택된 버튼 텍스트 업데이트
        string[] options = new string[]
        {
        "타워 투사체 속도 up", "플레이어 이동 속도 up", "플레이어 최대 체력 up",
        "타워 투사체 발사 속도 up", "플레이어 HP 회복", "HP 낮은 타워 회복",
        "플레이어 공격력 up", "타워 공격력 up"
        };

        // 각 버튼에 해당하는 텍스트 설정
        for (int i = 0; i < selectedIndexes.Count; i++)
        {
            buttons[selectedIndexes[i]].GetComponentInChildren<TextMeshProUGUI>().text = options[i];
        }

        // 위치 배열을 랜덤하게 섞기 위해 List로 변환
        List<Vector3> positionList = new List<Vector3>(positions);
        positionList = positionList.OrderBy(x => rand.Next()).ToList(); // 랜덤 섞기

        // 선택된 버튼들에 랜덤 위치 할당
        for (int i = 0; i < selectedIndexes.Count; i++)
        {
            buttons[selectedIndexes[i]].transform.position = positionList[i];
        }
    }
}
