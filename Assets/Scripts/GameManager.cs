using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    private Camera mainCamera;
    private bool isPlaying = true;
    private float maxHeightReached = 0f; // Tracks the maximum height reached by the player during the current session
    private int score = 0; // Tracks the score during the current session
    private int highScore; // Tracks the highest score across all game sessions
    private const string HighScoreKey = "HighScore"; // Key for PlayerPrefs
    private bool isNewHighScore = false; // Tracks whether a new high score was achieved
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private TextMeshProUGUI highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        // Get the main camera reference
        mainCamera = Camera.main;

        // Load the stored high score
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0); // Default value is 0 if no high score exists
        Debug.Log("Loaded High Score: " + highScore);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying && player != null)
        {
            CheckHeightScore();

            // Check if player falls below the camera's view
            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(player.transform.position);
            if (viewportPosition.y < 0)
            {
                GameOver();
            }
        }
    }

    public bool IsPlaying
    {
        get { return isPlaying; }
    }

    // Function to check and update height-based score
    private void CheckHeightScore()
    {
        float currentPlayerHeight = player.transform.position.y;

        // If player reaches a new maximum height, update the score
        if (currentPlayerHeight > maxHeightReached)
        {
            maxHeightReached = currentPlayerHeight;
            score = (int)maxHeightReached; // Set the current session's score to the new height
        }
    }

    // Function to update high score and save using PlayerPrefs
    private void UpdateHighScore()
    {
        // If the current score is higher than the saved high score, update the high score
        if (score > highScore)
        {
            highScore = score;

            // Save the new high score
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save(); // Ensure the data is saved to disk

            // Mark that a new high score was achieved
            isNewHighScore = true;
        }
    }

    private void GameOver()
    {
        // Update the high score at the end of the game if needed
        UpdateHighScore();
        gameOverScreen.SetActive(true);

        if (isNewHighScore)
        {
            highScoreText.text = "New High Score: " + highScore;
            Debug.Log("Game Over: New High Score! " + highScore);
        }
        else
        {
            highScoreText.text = "High Score: " + highScore + "\nYour Score: " + score;
            Debug.Log("Game Over: Your Score is " + score + ", High Score: " + highScore);
        }

        isPlaying = false;
    }
}
