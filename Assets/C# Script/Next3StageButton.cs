using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Next3StageButton : MonoBehaviour
{
    public void Next3Stage()
    {
        SceneManager.LoadScene("GameScene3");
    }

}
