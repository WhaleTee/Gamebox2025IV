using UnityEngine;
using SceneManagement;

namespace UI
{
    /// <summary>
    /// Универсальный контроллер UI, позволяющий вызывать методы SceneController из кнопок.
    /// </summary>
    public class UIController : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneController.Instance.OpenScene(sceneName);
        }

        public void RestartCurrentScene()
        {
            SceneController.Instance.RestartScene();
        }

        public void ExitToMainMenu()
        {
            SceneController.Instance.OpenScene("MainMenu");
        }

        public void QuitGame()
        {
            SceneController.Instance.ExitGame();
        }
    }
}
