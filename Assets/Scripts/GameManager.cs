using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private float startTime;
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private TextMeshProUGUI gameOverText;
    [SerializeField]
    private TextMeshProUGUI highScoreText;
    [SerializeField]
    private Slider progressBar;
    [SerializeField]
    private float endHeight = 500f;
    [SerializeField]
    private AudioManager audioManager;
    private Camera mainCamera;
    private bool isPlaying = true;
    private float maxHeightReached = 0f;
    private int score = 0;
    private int highScore;
    private const string HighScoreKey = "HighScore";
    private bool isNewHighScore = false;

    void Start()
    {
        startTime = Time.time;
        mainCamera = Camera.main;
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        if (progressBar != null)
        {
            progressBar.minValue = 0f;
            progressBar.maxValue = endHeight;
            progressBar.value = 0f;
        }
    }

    void Update()
    {
        if (isPlaying && player != null)
        {
            CheckHeightScore();

            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(player.transform.position);
            if (viewportPosition.y < 0)
            {
                GameOver();
            }
            if (player.transform.position.y >= endHeight)
            {
                Win();
            }

            float elapsedTime = Time.time - startTime;
            UpdateTimer(elapsedTime);
            UpdateProgressBar();
        }
    }

    public bool IsPlaying => isPlaying;

    private void UpdateTimer(float elapsedTime)
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void CheckHeightScore()
    {
        float currentPlayerHeight = player.transform.position.y;

        if (currentPlayerHeight > maxHeightReached)
        {
            maxHeightReached = currentPlayerHeight;
            score = (int)maxHeightReached;
        }
    }

    private void UpdateHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
            isNewHighScore = true;
        }
    }

    private void GameOver()
    {
        UpdateHighScore();
        float elapsedTime = Time.time - startTime;
        gameOverScreen.SetActive(true);
        gameOverText.text = "Game Over!";

        if (isNewHighScore)
        {
            highScoreText.text = "Time: " + FormatTime(elapsedTime) + "\nNew High Score: " + highScore;
        }
        else
        {
            highScoreText.text = "\nTime: " + FormatTime(elapsedTime) + "\nHigh Score: " + highScore + "\nYour Score: " + score;
        }

        isPlaying = false;
        audioManager.PlayLoseSound();
    }

    private void Win()
    {
        UpdateHighScore();
        float elapsedTime = Time.time - startTime;
        gameOverScreen.SetActive(true);
        gameOverText.text = "You Win!";
        highScoreText.text = "Time: " + FormatTime(elapsedTime) + "\nHigh Score: " + highScore;
        isPlaying = false;
        audioManager.PlayWinSound();
    }

    private void UpdateProgressBar()
    {
        if (progressBar != null && player != null)
        {
            float playerHeight = Mathf.Clamp(player.transform.position.y, 0f, endHeight);
            progressBar.value = playerHeight;
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
