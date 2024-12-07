using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D; // 물리 이동을 위한 변수
    Vector2 moveVelocity; //정규화된 벡터값을 담기 위한 변수
    Vector3 dir; //마우스 방향벡터를 저장할 변수

    public float speed = 10.0f; // 플레이어의 스피드를 조절하는 변수
    private float currHp; //플레이어의 현재 hp
    public float maxHp = 10f; //플레이어의 최대 체력
    public float giSpeed = 30.0f; //기의 속도
    public float ShootRate = 0.5f; //다음 기를 쏘기까지 걸리는 딜레이 시간
    private float nextShootTime = 0f; //시간 계산

    public GameObject giPrefab; //쏠 기
    public GameObject hpbar; // 체력바를 보이거나 보이지 않게 하기 위해
    public RectTransform hpfront; // 체력바의 스케일을 조정해 닳게하기 위해

    private float currExp; //플레이어의 현재 Exp
    public float maxExp = 200f; // Exp 최대치
    public float minExp = 1f; // Exp 최소치
    public GameObject expbar; // 경험치바를 보이거나 보이지 않게 하기 위해
    public RectTransform expfront; // 경험치바의 스케일을 조정해 닳게하기 위해

    public float stageTimeLimit = 180f; // 스테이지 제한 시간 (초 단위)
    private float elapsedTime = 0f;    // 경과 시간

    public Button HpUp, Recovery, DamageUp, SpeedUp; // 레벨업 선택지 버튼(플레이어)
    public Button TowerAttackSpeed, TowerDamage, TowerGiSpeed, TowerHpRecovery; // 레벨업 선택지 버튼(타워)
    public GameObject levelUpPanel; // 레벨업 선택지를 담은 패널

    Vector2 minBounds = new Vector2(-57, -32); //맵의 크기
    Vector2 maxBounds = new Vector2(57, 32);
    Vector2 startPos = new Vector2(0, -2); //시작 위치 조정

    private bool isLevelingUp = false;

    void Start()
    {

        rigid2D = GetComponent<Rigidbody2D>(); //rigidbody 컴포넌트 가져오기
        transform.position = startPos; //시작지점에서 시작
        currHp = maxHp; // 최대 체력만큼 현재 체력 설정
        currExp = minExp; // 최소치 Exp로 설정

        UpdateExpBar(); // 경험치바 초기화

        HideLevelUpPanel(); // 초기에는 레벨업 패널 숨기기
    }

    void Update()
    {
        if (isLevelingUp) return; // 레벨업 중에는 조작 금지

        if (Time.timeScale > 0) // 게임이 일시정지 상태가 아닐 때만 타이머 작동
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= stageTimeLimit)
            {
                StageClear();
            }
        }

        Vector2 pos = transform.position; //플레이어의 위치
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x); //플레이어의 위치를 맵 크기에 맞춰 제한
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
        transform.position = pos; //제한된 위치로 변환

        float x = Input.GetAxisRaw("Horizontal"); //ad를 사용해서 이동(input 매니저 조정 필요)
        float y = Input.GetAxisRaw("Vertical"); // ws를 이용해서 이동

        Vector2 move = new Vector2(x, y);
        moveVelocity = move.normalized * speed; // 속도에 맞춰 벡터값 정규화

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //마우스가 클릭된 위치 구하기
        dir = (mousePosition - pos).normalized; ////마우스가 클릭되었을 때 위치와 플레이어의 위치 간의 방향 구하기

        if (Input.GetMouseButtonDown(0) && Time.time > nextShootTime) // 마우스를 좌클릭 했을때, 딜레이 시간이 지나면
        {
            nextShootTime = Time.time + ShootRate; //딜레이 시간 재조정
            Shoot(); //쏘기
        }
    }

    private void StageClear()
    {
        string currentSceneName = SceneManager.GetActiveScene().name; // 현재 씬 이름 가져오기
        string nextSceneName = GetNextSceneName(currentSceneName);    // 다음 씬 이름 계산


        Debug.Log($"Stage Clear! Moving to next scene: {nextSceneName}");
        SceneManager.LoadScene(nextSceneName);
        
    }

    private string GetNextSceneName(string currentSceneName)
    {
        // 정규식을 사용해 숫자를 추출
        System.Text.RegularExpressions.Match match =
            System.Text.RegularExpressions.Regex.Match(currentSceneName, @"(\d+)$");

        if (match.Success)
        {
            int currentSceneNumber = int.Parse(match.Value); // 현재 씬 번호 추출
            string nextSceneName;

            // ClearNextScene → GameScene 전환
            if (currentSceneName.StartsWith("ClearNextScene"))
            {
                nextSceneName = $"GameScene{currentSceneNumber}"; // GameSceneX로 이동
            }
            // GameScene → ClearNextScene 전환
            else if (currentSceneName.StartsWith("GameScene"))
            {
                nextSceneName = $"ClearNextScene{currentSceneNumber + 1}"; // ClearNextSceneX로 이동
            }
            else
            {
                nextSceneName = null; // 예상치 못한 씬 이름
            }

            return nextSceneName;
        }

        return null; // 숫자가 포함되지 않은 경우
    }

    private void UpdateExpBar()
    {
        if (expfront != null)
        {
            float expRatio = currExp / maxExp; // 현재 경험치 비율 계산
            expfront.localScale = new Vector3(expRatio, 1.0f, 1.0f); // 경험치바 크기 조정
        }
    }

    public void GainExperience(float exp)
    {

        currExp += exp; // 경험치 증가
        UpdateExpBar(); // 경험치바 업데이트

        // 경험치가 최대치에 도달하면 레벨업 처리
        if (currExp >= maxExp)
        {
            currExp -= maxExp; // 초과된 경험치 유지
            LevelUp();         // 레벨업 메서드 호출
        }

        // 경험치 UI 업데이트 (필요하다면 구현)
        Debug.Log($"Current Experience: {currExp}");
    }

    private void LevelUp()
    {
        // 레벨업 로직
        Debug.Log("Level Up!");

        // 게임 일시정지
        Time.timeScale = 0;

        ShowLevelUpPanel();
    }

    private void ShowLevelUpPanel()
    {
        // 8개의 옵션 배열
        string[] options = new string[]
        {
        "타워 투사체 속도 up", "플레이어 이동 속도 up", "플레이어 최대 체력 up",
        "타워 투사체 발사 속도 up", "플레이어 HP 회복", "HP 낮은 타워 회복",
        "플레이어 공격력 up", "타워 공격력 up", "타워 기속도 up", "타워 HP up"
        };

        int[] randomIndexes = GetRandomIndexes(options.Length, 8); // 버튼 8개 랜덤 선택

        HpUp.GetComponentInChildren<TextMeshProUGUI>().text = options[randomIndexes[0]];
        Recovery.GetComponentInChildren<TextMeshProUGUI>().text = options[randomIndexes[1]];
        DamageUp.GetComponentInChildren<TextMeshProUGUI>().text = options[randomIndexes[2]];
        SpeedUp.GetComponentInChildren<TextMeshProUGUI>().text = options[randomIndexes[3]];
        TowerAttackSpeed.GetComponentInChildren<TextMeshProUGUI>().text = options[randomIndexes[4]];
        TowerDamage.GetComponentInChildren<TextMeshProUGUI>().text = options[randomIndexes[5]];
        TowerGiSpeed.GetComponentInChildren<TextMeshProUGUI>().text = options[randomIndexes[6]];
        TowerHpRecovery.GetComponentInChildren<TextMeshProUGUI>().text = options[randomIndexes[7]];

        
        HpUp.onClick.AddListener(() => ApplyLevelUpEffect(randomIndexes[0]));
        Recovery.onClick.AddListener(() => ApplyLevelUpEffect(randomIndexes[1]));
        DamageUp.onClick.AddListener(() => ApplyLevelUpEffect(randomIndexes[2]));
        SpeedUp.onClick.AddListener(() => ApplyLevelUpEffect(randomIndexes[3]));
        TowerAttackSpeed.onClick.AddListener(() => ApplyLevelUpEffect(randomIndexes[4]));
        TowerDamage.onClick.AddListener(() => ApplyLevelUpEffect(randomIndexes[5]));
        TowerGiSpeed.onClick.AddListener(() => ApplyLevelUpEffect(randomIndexes[6]));
        TowerHpRecovery.onClick.AddListener(() => ApplyLevelUpEffect(randomIndexes[7]));
        

        PositionButtonsRandomly(); // 버튼 위치 랜덤으로 배치
        levelUpPanel.SetActive(true);
    }

    private void HideLevelUpPanel()
    {
        // 레벨업 패널에서 버튼 이벤트 리스너 제거
        HpUp.onClick.RemoveAllListeners();
        Recovery.onClick.RemoveAllListeners();
        DamageUp.onClick.RemoveAllListeners();

        // 레벨업 패널 숨기기
        levelUpPanel.SetActive(false);

        // 게임 재개
        Time.timeScale = 1;
        isLevelingUp = false; // 레벨업 중이 아님
    }

    private void ApplyLevelUpEffect(int optionIndex)
    {
        switch (optionIndex)
        {
            case 0: // 타워 투사체 속도 up
                Debug.Log("타워 투사체 속도 증가!");
                break;
            case 1: // 플레이어 이동 속도 up
                //speed += 2.0f;
                Debug.Log("플레이어 이동 속도 증가!");
                break;
            case 2: // 플레이어 최대 체력 up
                //maxHp += 5.0f;
                Debug.Log("플레이어 최대 체력 증가!");
                break;
            case 3: // 타워 투사체 발사 속도 up
                ShootRate -= 0.1f;
                Debug.Log("타워 투사체 발사 속도 증가!");
                break;
            case 4: // 플레이어 HP 회복
                //currHp = maxHp;
                Debug.Log("플레이어 HP 회복!");
                break;
            case 5: // HP 낮은 타워 회복
                Debug.Log("HP 낮은 타워 회복!");
                break;
            case 6: // 플레이어 공격력 up
                Debug.Log("플레이어 공격력 증가!");
                break;
            case 7: // 타워 공격력 up
                Debug.Log("타워 공격력 증가!");
                break;
        }

        // 레벨업 패널 숨기고 게임 재개
        HideLevelUpPanel();
    }
    
    private int[] GetRandomIndexes(int range, int count)
    {
        System.Random rand = new System.Random();
        HashSet<int> indexes = new HashSet<int>();
        while (indexes.Count < count)
        {
            indexes.Add(rand.Next(range));
        }
        return new List<int>(indexes).ToArray();
    }

    private void PositionButtonsRandomly()
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
            int randomIndex = rand.Next(buttons.Length);
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

    void FixedUpdate() //Update함수는 프레임이 일정하지 않기 때문에 rigidbody를 다루는 코드를 설정하는 함수
    {
        if (!isLevelingUp)
        {
            rigid2D.MovePosition(rigid2D.position + moveVelocity * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("eat")) //eat 태그를 가진 오브젝트와 부딪히면
        {
            if (currHp > 0)
            { //현재 체력이 남아있다면
                currHp -= 1.0f; //체력 1갂기
                hpfront.localScale = new Vector3(currHp / maxHp, 1.0f, 1.0f); // 현재 체력을 최대 체력으로 나누어서 hp조절

                PoolManager.instance.ReturnPreFab(collision.gameObject); //충돌한 몬스터 비활성화(풀링)

            }

            else
            {
                GameOver();
            }
        }
        if (collision.gameObject.CompareTag("gohome")) //gohome 태그를 가진 오브젝트와 부딪히면
        {
            if (currHp > 0)
            { //현재 체력이 남아있다면
                currHp -= 1.0f; //체력 1갂기
                hpfront.localScale = new Vector3(currHp / maxHp, 1.0f, 1.0f); // 현재 체력을 최대 체력으로 나누어서 hp조절

                PoolManager.instance.ReturnPreFab(collision.gameObject);//충돌한 몬스터 비활성화(풀링)

            }

            else
            {
                GameOver();
            }
        }
        if (collision.gameObject.CompareTag("what")) //what 태그를 가진 오브젝트와 부딪히면
        {
            if (currHp > 0)
            { //현재 체력이 남아있다면
                currHp -= 2.0f; //체력 2갂기
                hpfront.localScale = new Vector3(currHp / maxHp, 1.0f, 1.0f); // 현재 체력을 최대 체력으로 나누어서 hp조절

                PoolManager.instance.ReturnPreFab(collision.gameObject); //충돌한 몬스터 비활성화(풀링)
            }

            else
            {
                GameOver();
            }
        }
        if (collision.gameObject.CompareTag("no")) //no 태그를 가진 오브젝트와 부딪히면
        {
            if (currHp > 0)
            { //현재 체력이 남아있다면
                currHp -= 3.0f; //현재 체력 갂기
                hpfront.localScale = new Vector3(currHp / maxHp, 1.0f, 1.0f); // 현재 체력을 최대 체력으로 나누어서 hp조절

                PoolManager.instance.ReturnPreFab(collision.gameObject); //충돌한 몬스터 비활성화(풀링)

            }

            else
            {
                GameOver();
            }
        }
        if (collision.gameObject.CompareTag("pressure")) //pressure 태그를 가진 오브젝트와 부딪히면
        {
            if (currHp > 0)
            { //현재 체력이 남아있다면
                currHp -= 5.0f; //현재 체력 갂기
                hpfront.localScale = new Vector3(currHp / maxHp, 1.0f, 1.0f); // 현재 체력을 최대 체력으로 나누어서 hp조절

                PoolManager.instance.ReturnPreFab(collision.gameObject); //충돌한 몬스터 비활성화(풀링)

            }

            else
            {
                GameOver();
            }
        }
        if (collision.gameObject.CompareTag("exp")) {
            GainExperience(10); // 경험치 획득
            PoolManager.instance.ReturnPreFab(collision.gameObject); //경험치 반환
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    void Shoot() { // 기를 쏘는 함수
        GameObject gi = PoolManager.instance.GetPreFab(giPrefab); //풀에서 기를 가져오기
        gi.transform.position = transform.position; //기의 위치를 플레이어의 위치로 이동
        gi.GetComponent<Rigidbody2D>().velocity = dir * giSpeed; //설정한 기의 속도만큼 마우스 위치로 발사
    }

}


