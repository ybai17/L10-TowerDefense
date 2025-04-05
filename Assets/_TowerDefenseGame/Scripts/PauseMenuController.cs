using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    bool isGamePaused = false;

    public GameObject pauseMenuPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                // resume the game
                ResumeGame();
            }
            else
            {
                // pause the game
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;

        pauseMenuPanel.SetActive(false);
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;

        pauseMenuPanel.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Debug.Log("Loading main menu scene");
        //SceneManager.LoadScene(0); //assuming main menu is stored at scene index 0
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game");
        Application.Quit();
    }
}
