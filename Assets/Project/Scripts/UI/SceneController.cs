using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Cysharp.Threading.Tasks;
using Misc;

namespace UI
{
    public class SceneController : Singleton<SceneController>
    {
        private bool _isLoading;

        /// <summary>
        /// ��������� ����� �� �����.
        /// </summary>
        public void OpenScene(string sceneName)
        {
            Time.timeScale = 1f;

            if (SceneExists(sceneName))
            {
                UniTask.Void(() => LoadSceneAsync(sceneName));
            }
            else
            {
                Debug.LogError($"����� '{sceneName}' �� ������� � Build Settings!");
            }
        }

        /// <summary>
        /// ������������� ������� �����.
        /// </summary>
        public void RestartScene()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            OpenScene(currentScene);
        }

        /// <summary>
        /// ����� �� ����.
        /// </summary>
        public void ExitGame()
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        /// <summary>
        /// ���������, ���������� �� ����� � Build Settings.
        /// </summary>
        private bool SceneExists(string sceneName)
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;

            for (int i = 0; i < sceneCount; i++)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                string name = System.IO.Path.GetFileNameWithoutExtension(path);

                if (name == sceneName)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// ����������� �������� �����.
        /// </summary>
        private async UniTaskVoid LoadSceneAsync(string sceneName)
        {
            AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
            await UniTask.WaitUntil(() => load.isDone);
        }
    }
}