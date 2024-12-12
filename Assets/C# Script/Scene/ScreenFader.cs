using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;

public class ScreenFader : MonoBehaviour
{

    public Image fadeImage;
    public float fadeDuration = 5.0f;

    public string nextScene;
    float time;
    bool FadeOutStart;
    bool click;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;   
        FadeOutStart = true;
        click = false;
        fadeImage.enabled = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (SceneManager.GetActiveScene().name != "MainScene" &&
            SceneManager.GetActiveScene().name != "GameScene5")
        {
            if (click || (time > 5f && FadeOutStart))
            {
                StartFadeOut();
                FadeOutStart = false;
            }
            if (Input.GetMouseButtonDown(0)) click = true;
        }
    }
    public void StartFadeOut()
    {
        fadeImage.enabled = true;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Color color = fadeImage.color;
        float time = 0;
        while (time < fadeDuration && !click)
        {
            color.a = Mathf.Clamp01(time / fadeDuration); // Alpha 값 증가
            fadeImage.color = color;

            time += Time.deltaTime;

            Debug.Log($"Fade Alpha: {color.a}, Time: {time}");

            yield return null;
        }

        color.a = 1;
        fadeImage.color = color;

        LoadNextscene();
    }
    protected void LoadNextscene()
    {
        Debug.Log("다음 씬 로드");
        SceneManager.LoadScene(nextScene);
    }
}
