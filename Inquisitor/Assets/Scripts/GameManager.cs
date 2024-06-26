using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int score = 0;

    [SerializeField] private GameObject levelController;
    [SerializeField] private WaterMeshGenerator water;
    [SerializeField] private GameObject player;
    [SerializeField] private Image fadeImg;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float timerTime;
    [SerializeField] private AudioClip gameOverSFX;
    private float timeElapsed;
    private int minutes, seconds, cents;
    private int redZone = 10;
    private bool inFade = false;
    private float fadeTime = 1.5f;

    void Start()
    {
        StartCoroutine(FadeIn());
        levelController.SendMessage("CreateLevel");
        timeElapsed = timerTime;
        PlayerPrefs.SetInt("ScoreCurr", score);
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
            levelController.SendMessage("DestroyPreviousLevel", false);
        }

        if ((water.planeHeight >= player.transform.position.y ||
            player.GetComponent<Character>()._healthC <= 0) && !inFade)
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

        GetComponent<AudioSource>().enabled = false;

        AudioController.Instance.PlaySound(gameOverSFX);

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

        // Store score in currentg computer
        PlayerPrefs.SetInt("ScoreCurr", score);
        if(score > PlayerPrefs.GetInt("ScoreBest", 0))
        {
            PlayerPrefs.SetInt("ScoreBest", score);
        }

        levelController.SendMessage("CreateLevel");
        timeElapsed = timerTime;
        timerText.color = Color.white;
        timerText.transform.localScale = Vector3.one;
    }
}
