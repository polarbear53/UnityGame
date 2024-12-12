using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Next5StageButton : MonoBehaviour
{
    public void NextStage()
    {
        SceneManager.LoadScene("GameScene5");
    }

}
