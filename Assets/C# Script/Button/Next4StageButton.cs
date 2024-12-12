using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Next4StageButton : MonoBehaviour
{
    public void NextStage()
    {
        SceneManager.LoadScene("GameScene4");
    }

}
