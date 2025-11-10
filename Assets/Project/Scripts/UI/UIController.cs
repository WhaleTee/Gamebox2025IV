using UnityEngine;

namespace UI
{
    /// <summary>
    /// ������������� ���������� UI, ����������� �������� ������ SceneLoader �� ������.
    /// </summary>
    public class UIController : MonoBehaviour
    {
        /// <summary>
        /// ��������� ����� �� �����.
        /// </summary>
        /// <param name="sceneName">��� �����, ��������� � Build Settings.</param>
        public void LoadScene(string sceneName)
        {
            SceneController.Instance.OpenScene(sceneName);
        }

        /// <summary>
        /// ������������� ������� �����.
        /// </summary>
        public void RestartCurrentScene()
        {
            SceneController.Instance.RestartScene();
        }

        /// <summary>
        /// ������� � ������� ����.
        /// </summary>
        public void ExitToMainMenu()
        {
            SceneController.Instance.OpenScene("MainMenu");
        }

        /// <summary>
        /// ��������� ��������� ���� (�������� � � �����, � � ���������).
        /// </summary>
        public void QuitGame()
        {
            SceneController.Instance.ExitGame();
        }
    }
}