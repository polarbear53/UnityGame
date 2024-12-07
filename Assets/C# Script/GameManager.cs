using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void ResumeGame()
    {
        // 게임을 계속 진행할 수 있게 설정
        Time.timeScale = 1;  // 게임 속도 재개
    }

    public void PauseGame()
    {
        // 레벨업 선택 시 게임 일시 정지
        Time.timeScale = 0;  // 게임 일시 정지
    }
}
