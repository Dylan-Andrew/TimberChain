using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject escapeMenuScreen;

    private bool isPaused = false;

    void Start()
    {
        escapeMenuScreen.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
            escapeMenuScreen.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            escapeMenuScreen.SetActive(false);
        }
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Resume()
    {
        TogglePause();
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
