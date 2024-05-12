using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private float fadeTime = 0.75f;
    [SerializeField] private Image fadeImg;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        Color c;
        float alpha;

        for (float i = fadeTime; i >= 0; i -= Time.deltaTime)
        {
            alpha = (i - 0) / (fadeTime - 0); //Normalize value between 0 and 1

            c = fadeImg.color;
            c.a = alpha;
            fadeImg.color = c;

            yield return null;
        }

        fadeImg.enabled = false;
    }

    IEnumerator FadeOut()
    {
        fadeImg.enabled = true;

        Color c;
        float alpha;

        for (float i = 0; i <= fadeTime; i += Time.deltaTime)
        {
            alpha = (i - 0) / (fadeTime - 0); //Normalize value between 0 and 1

            c = fadeImg.color;
            c.a = alpha;
            fadeImg.color = c;

            yield return null;
        }

        // Go to main menu
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void MainMenu()
    {
        StartCoroutine(FadeOut());
    }
}
