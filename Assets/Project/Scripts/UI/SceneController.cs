using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using Misc;

public class SceneController : Singleton<SceneController>
{
    private bool _isLoading;

    /// <summary>
    /// Открывает сцену по имени.
    /// </summary>
    public void OpenScene(string sceneName)
    {
        Time.timeScale = 1f;

        if (SceneExists(sceneName))
        {
            _ = LoadSceneAsync(sceneName);
        }
        else
        {
            Debug.LogError($"Сцена '{sceneName}' не найдена в Build Settings!");
        }
    }

    /// <summary>
    /// Перезапускает текущую сцену.
    /// </summary>
    public void RestartScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        OpenScene(currentScene);
    }

    /// <summary>
    /// Выход из игры.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    /// <summary>
    /// Проверяет, существует ли сцена в Build Settings.
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
    /// Асинхронная загрузка сцены.
    /// </summary>
    private async UniTaskVoid LoadSceneAsync(string sceneName)
    {
        if (_isLoading)
            return;

        _isLoading = true;

        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
        await load.ToUniTask();

        _isLoading = false;
    }
}
