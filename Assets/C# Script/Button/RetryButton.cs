using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{

    public void GameRetry()
    {
        SceneManager.LoadScene("GameScene1");
    }

}
