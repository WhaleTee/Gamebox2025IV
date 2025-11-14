using Cysharp.Threading.Tasks;
using Input;
using Misc;
using Reflex.Attributes;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class SceneController : Singleton<SceneController>
    {
        [Header("UI Загрузки")]
        [SerializeField] private LoadingScreen loadingScreenPrefab;

        private LoadingScreen _loadingScreenInstance;
        private bool _isLoading;

        [Inject] private UserInput userInput;

        public void OpenScene(string sceneName)
        {
            if (userInput != null)
                userInput.Enabled = true;

            Time.timeScale = 1f;

            if (!SceneExists(sceneName))
            {
                Debug.LogError($"Сцена '{sceneName}' не найдена в Build Settings!");
                return;
            }

            _ = LoadSceneWithScreenAsync(sceneName);
        }

        public void RestartScene()
        {
            OpenScene(SceneManager.GetActiveScene().name);
        }

        public void ExitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private bool SceneExists(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(
                    SceneUtility.GetScenePathByBuildIndex(i));

                if (name == sceneName)
                    return true;
            }

            return false;
        }

        private async UniTask LoadSceneWithScreenAsync(string sceneName)
        {
            if (_isLoading)
                return;

            _isLoading = true;

            if (_loadingScreenInstance == null)
            {
                _loadingScreenInstance = Instantiate(loadingScreenPrefab);
                DontDestroyOnLoad(_loadingScreenInstance.gameObject);
            }

            await _loadingScreenInstance.FadeInAsync();

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
            loadOperation.allowSceneActivation = false;

            while (loadOperation.progress < 0.9f)
            {
                _loadingScreenInstance.SetProgress(loadOperation.progress / 0.9f);
                await UniTask.Yield();
            }

            _loadingScreenInstance.SetProgress(1f);
            await UniTask.Delay(300);

            loadOperation.allowSceneActivation = true;
            await UniTask.WaitUntil(() => loadOperation.isDone);

            await _loadingScreenInstance.FadeOutAsync();
            _isLoading = false;
        }
    }
}
