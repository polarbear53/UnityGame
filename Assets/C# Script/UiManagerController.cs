using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

public class UiManagerController : MonoBehaviour
{
    public static UiManagerController instance; //싱글패턴 

    public GameObject levelUpPanel; // 레벨업 선택지를 담은 패널
    public Button HpUp, Recovery, DamageUp, SpeedUp; // 레벨업 선택지 버튼(플레이어)
    public Button TowerAttackSpeed, TowerDamage, TowerGiSpeed, TowerHpRecovery; // 레벨업 선택지 버튼(타워)

    public PlayerController playerController; // 플레이어에 접근
    public TowerController tower; // 타워1에 접근
    public TowerController tower2; // 타워2에 접근
    public TowerController tower3; // 타워3에 접근
    public TowerController tower4; // 타워4에 접근

    void Update()
    {
        if (levelUpPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            // 클릭한 위치가 UI가 아니라면 무시
            if (!IsPointerOverUIObject())
            {
                Debug.Log("UI 외부 클릭 무시");
                return;
            }
        }
    }

    // Pointer가 UI 위에 있는지 확인하는 메서드
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

        HideLevelUpPanel(); // 초기에는 레벨업 패널 숨기기
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
        

        PositionButtonsRandomly(randomIndexes); // 버튼 위치를 랜덤하게 배치

        // 처음에는 버튼을 비활성화
        HideButtons();

        levelUpPanel.SetActive(true);

        // 1초 후에 버튼을 활성화
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
        // 1초 동안 대기 (일시정지 상태에서)
        yield return new WaitForSecondsRealtime(delay);

        // 버튼을 활성화
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
                // 타워, 타워2, 타워3, 타워4가 있을 경우
                if (tower.fireRate <= 0.1f) break;
                if (tower2 != null && tower3 != null && tower4 != null)
                {
                    tower.fireRate -= 0.1f;
                    tower2.fireRate -= 0.1f;
                    tower3.fireRate -= 0.1f;
                    tower4.fireRate -= 0.1f;
                }
                // 타워, 타워2가 있을 경우
                else if (tower2 != null)
                {
                    tower.fireRate -= 0.1f;
                    tower2.fireRate -= 0.1f;
                }
                // 타워만 있을 경우
                else
                {
                    tower.fireRate -= 0.1f;
                }
                Debug.Log("타워 투사체 발사 속도 증가!");
                break;
            case 5: // 타워 공격력 up
                // 타워, 타워2, 타워3, 타워4가 있을 경우
                if (tower2 != null && tower3 != null && tower4 != null)
                {
                    tower.towerdamage += 1;
                    tower2.towerdamage += 1;
                    tower3.towerdamage += 1;
                    tower4.towerdamage += 1;

                    Debug.Log("현재 타워1의 공격력: " + tower.towerdamage);
                    Debug.Log("현재 타워2의 공격력: " + tower2.towerdamage);
                    Debug.Log("현재 타워3의 공격력: " + tower3.towerdamage);
                    Debug.Log("현재 타워4의 공격력: " + tower4.towerdamage);
                }
                // 타워, 타워2가 있을 경우
                else if (tower2 != null)
                {
                    tower.towerdamage += 1;
                    tower2.towerdamage += 1;

                    Debug.Log("현재 타워1의 공격력: " + tower.towerdamage);
                    Debug.Log("현재 타워2의 공격력: " + tower2.towerdamage);
                }
                // 타워만 있을 경우
                else
                {
                    tower.towerdamage += 1;
                    Debug.Log("현재 타워의 공격력: " + tower.towerdamage);
                }

                Debug.Log("타워 공격력 증가!");
                break;
            case 6: // 타워 투사체 속도 up
                // 타워, 타워2, 타워3, 타워4가 있을 경우
                if (tower2 != null && tower3 != null && tower4 != null)
                {
                    tower.projectileSpeed += 4.0f;
                    tower2.projectileSpeed += 4.0f;
                    tower3.projectileSpeed += 4.0f;
                    tower4.projectileSpeed += 4.0f;

                    Debug.Log("현재 타워의 투사체 속도: " + tower.projectileSpeed);
                    Debug.Log("현재 타워2의 투사체 속도: " + tower2.projectileSpeed);
                    Debug.Log("현재 타워3의 투사체 속도: " + tower3.projectileSpeed);
                    Debug.Log("현재 타워4의 투사체 속도: " + tower4.projectileSpeed);
                }
                // 타워, 타워2가 있을 경우
                else if (tower2 != null)
                {
                    tower.projectileSpeed += 4.0f;
                    tower2.projectileSpeed += 4.0f;
                    Debug.Log("현재 타워의 투사체 속도: " + tower.projectileSpeed);
                    Debug.Log("현재 타워2의 투사체 속도: " + tower2.projectileSpeed);
                }
                // 타워만 있을 경우
                else
                {
                    tower.projectileSpeed += 4.0f;

                    Debug.Log("현재 타워의 투사체 속도: " + tower.projectileSpeed);
                }

                Debug.Log("타워 투사체 속도 증가!");
                break;
            case 7: // 타워 회복
                // 타워, 타워2, 타워3, 타워4가 있을 경우
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
                // 타워, 타워2가 있을 경우
                else if (tower2 != null)
                {
                    tower.currHp = tower.maxHp;
                    tower2.currHp = tower2.maxHp;

                    tower.hpfront.localScale = new Vector3(tower.currHp / tower.maxHp, 1.0f, 1.0f);
                    tower2.hpfront.localScale = new Vector3(tower2.currHp / tower2.maxHp, 1.0f, 1.0f);
                }
                // 타워만 있을 경우
                else
                {
                    tower.currHp = tower.maxHp;

                    tower.hpfront.localScale = new Vector3(tower.currHp / tower.maxHp, 1.0f, 1.0f);
                }

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

        // 버튼 배열 크기에 맞춰 랜덤으로 3개만 선택
        if (randomIndexes.Length < 3)
        {
            Debug.LogError("randomIndexes 배열 크기가 3보다 작습니다!");
            return;
        }

        List<int> selectedIndexes = randomIndexes.Take(3).ToList();

        // 위치 배열을 랜덤하게 섞기
        System.Random rand = new System.Random();
        List<Vector3> positionList = positions.OrderBy(x => rand.Next()).ToList();

        // 선택된 버튼을 활성화하고 위치와 텍스트를 설정
        for (int i = 0; i < selectedIndexes.Count; i++)
        {
            int buttonIndex = selectedIndexes[i];
            Button button = buttons[buttonIndex];

            // 텍스트 제거
            button.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;

            // 위치 설정
            button.transform.position = positionList[i];

            // 버튼 활성화
            button.gameObject.SetActive(true);
        }

        // 나머지 버튼 비활성화
        for (int i = 0; i < buttons.Length; i++)
        {
            if (!selectedIndexes.Contains(i))
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }
}
