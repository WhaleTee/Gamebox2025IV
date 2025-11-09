using UnityEngine;

/// <summary>
/// Универсальный контроллер UI, позволяющий вызывать методы SceneLoader из кнопок.
/// </summary>
public class UIController : MonoBehaviour
{
    /// <summary>
    /// Загружает сцену по имени.
    /// </summary>
    /// <param name="sceneName">Имя сцены, указанное в Build Settings.</param>
    public void LoadScene(string sceneName)
    {
        SceneController.Instance.OpenScene(sceneName);
    }

    /// <summary>
    /// Перезапускает текущую сцену.
    /// </summary>
    public void RestartCurrentScene()
    {
        SceneController.Instance.RestartScene();
    }

    /// <summary>
    /// Выходит в главное меню.
    /// </summary>
    public void ExitToMainMenu()
    {
        SceneController.Instance.OpenScene("MainMenu");
    }

    /// <summary>
    /// Полностью завершает игру (работает и в билде, и в редакторе).
    /// </summary>
    public void QuitGame()
    {
        SceneController.Instance.ExitGame();
    }
}
