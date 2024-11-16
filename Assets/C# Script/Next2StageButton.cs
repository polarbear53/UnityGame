using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Next2StageButton : MonoBehaviour
{
    public void NextStage()
    {
        SceneManager.LoadScene("GameScene2");
    }

}
