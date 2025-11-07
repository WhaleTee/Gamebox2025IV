using DG.Tweening;
using Project.Scripts.Input;
using Reflex.Attributes;
using Reflex.Extensions;
using UnityEngine;
using USM = UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.SceneManager;
using Reflex.Core;

namespace Misc.Dummy
{
    public class SceneService
    {
        [Inject] private UserInput userInput;
        private float slowTimeDuration = 1.1f;
        private Tween SlowTime
        {
            get
            {
                slowTime ??= DOVirtual.Float(Time.timeScale, 0, slowTimeDuration, t => Time.timeScale = t)
                    .SetUpdate(true).SetAutoKill(false).SetId(tweenID);
                return slowTime;
            }
        }
        private Tween slowTime;
        private string tweenID = "Dummy_SceneService_";

        /// <summary>
        /// Or get some specific service and pause/shutdown current game state
        /// </summary>
        public void StopGame()
        {
            SetInputEnabled(false);
            SlowTime.Play();
        }

        public void LoadNextSceneOrQuitGame()
        {
            if (SceneExists(NextSceneIndex))
            {
                SetInputEnabled(true);
                LoadScene(NextSceneIndex);
            }
            else
                QuitGame();
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }

        private void SetInputEnabled(bool value)
        {
            userInput ??= SceneContainer.Resolve<UserInput>();
            userInput.enabled = value;
        }

        public Container SceneContainer
        {
            get { sceneContainer ??= CurrentScene.GetSceneContainer(); return sceneContainer; }
            set => sceneContainer = value;
        }
        [Inject] private Container sceneContainer;

        public void LoadScene(int index) => USM.SceneManager.LoadScene(index);

        public bool SceneExists(int index) => index < sceneCountInBuildSettings;
        public USM.Scene CurrentScene;

        public int SceneIndex = GetActiveScene().buildIndex;
        public int NextSceneIndex => SceneIndex + 1;
    }
}
