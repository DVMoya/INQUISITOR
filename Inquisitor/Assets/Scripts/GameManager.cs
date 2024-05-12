using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int score = 0;

    [SerializeField] private GameObject LevelController;
    [SerializeField] private WaterMeshGenerator water;
    [SerializeField] private GameObject player;
    [SerializeField] private Image fadeImg;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float timerTime;
    private float timeElapsed;
    private int minutes, seconds, cents;
    private int redZone = 10;
    private bool inFade = false;
    private float fadeTime = 0.75f;

    void Start()
    {
        StartCoroutine(FadeIn());
        LevelController.SendMessage("CreateLevel");
        timeElapsed = timerTime;
    }

    private void Update()
    {
        timeElapsed -= Time.deltaTime;

        if (timeElapsed < 0) timeElapsed = 0;
        else if (timeElapsed <= redZone)
        {
            float pingpong = Mathf.PingPong(Time.time, 1);
            timerText.color = Color.Lerp(Color.white, Color.red, pingpong);
            timerText.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.1f, pingpong);
        }

        minutes = (int)(timeElapsed / 60f);
        seconds = (int)(timeElapsed - minutes * 60f);
        cents   = (int)((timeElapsed - (int)(timeElapsed)) * 100f);

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);

        if (timeElapsed <= 0) {
            LevelController.SendMessage("DestroyPreviousLevel", false);
            Debug.Log("GAME OVER");
        }

        if (water.planeHeight >= player.transform.position.y && !inFade)
        {
            inFade = true;
            StartCoroutine(FadeOut());
        }
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

        // Go to GAME OVER screen
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

    private void StageComplete()
    {
        score++;
        scoreText.text = "SCORE: " + score.ToString();

        LevelController.SendMessage("CreateLevel");
        timeElapsed = timerTime;
        timerText.color = Color.white;
        timerText.transform.localScale = Vector3.one;
    }
}
