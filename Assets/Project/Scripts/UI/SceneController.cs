using Cysharp.Threading.Tasks;
using Input;
using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    [Header("UI Загрузки")]
    [SerializeField] private LoadingScreen loadingScreenPrefab;

    private LoadingScreen _loadingScreenInstance;
    private bool _isLoading;

    public void OpenScene(string sceneName)
    {
        if (UserInput.Instance != null)
            UserInput.Instance.enabled = true;

        Time.timeScale = 1f;

        if (!SceneExists(sceneName))
        {
            Debug.LogError($"Сцена '{sceneName}' не найдена в Build Settings!");
            return;
        }

        _ = LoadSceneWithScreenAsync(sceneName);
    }

    public void RestartScene() => OpenScene(SceneManager.GetActiveScene().name);

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
            if (System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)) == sceneName)
                return true;
        }
        return false;
    }

    private async UniTask LoadSceneWithScreenAsync(string sceneName)
    {
        if (_isLoading) return;
        _isLoading = true;

        if (_loadingScreenInstance == null)
        {
            _loadingScreenInstance = Instantiate(loadingScreenPrefab);
            DontDestroyOnLoad(_loadingScreenInstance.gameObject);
        }

        await _loadingScreenInstance.FadeInAsync();

        var loadOperation = SceneManager.LoadSceneAsync(sceneName);
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

    /// <summary>
    /// Метод для имитации загрузки сцены с задержкой.
    /// </summary>

    //private async UniTask LoadSceneWithScreenAsync(string sceneName)
    //{
    //    if (_isLoading)
    //        return;

    //    _isLoading = true;

    //    // Создаем или находим экран загрузки
    //    if (_loadingScreenInstance == null)
    //    {
    //        _loadingScreenInstance = Instantiate(loadingScreenPrefab);
    //        DontDestroyOnLoad(_loadingScreenInstance.gameObject);
    //    }

    //    await _loadingScreenInstance.FadeInAsync();

    //    AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
    //    loadOperation.allowSceneActivation = false;

    //    float fakeProgress = 0f;              // искусственный прогресс
    //    float duration = 5f;                  // длительность загрузки в секундах
    //    float timer = 0f;

    //    // Цикл искусственной загрузки
    //    while (fakeProgress < 1f)
    //    {
    //        timer += Time.deltaTime;
    //        fakeProgress = Mathf.Clamp01(timer / duration);

    //        _loadingScreenInstance.SetProgress(fakeProgress);


    //        await UniTask.Yield();
    //    }

    //    // Завершаем загрузку
    //    await UniTask.Delay(300);
    //    loadOperation.allowSceneActivation = true;
    //    await UniTask.WaitUntil(() => loadOperation.isDone);

    //    await _loadingScreenInstance.FadeOutAsync();

    //    _isLoading = false;
    //}

}
