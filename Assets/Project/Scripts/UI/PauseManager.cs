using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("UI Паузы")]
    [SerializeField] private GameObject pauseMenu;

    private bool isPaused = false;

    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }
    public void ResumeGame()
    {
        if (!isPaused) return;

        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
}
