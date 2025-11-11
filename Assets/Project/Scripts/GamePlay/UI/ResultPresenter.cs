using UnityEngine;
using UnityEngine.UI;
using Misc;
using Reflex.Attributes;

namespace GamePlay
{
    public class ResultPresenter : MonoBehaviour
    {
        [SerializeField] private string m_playerTag = "Player";
        private ITrigger trigger;
        private RectTransform dialog;
        private Button buttonNext;
        private Misc.Dummy.SceneService sceneService;

        [Inject]
        public void Inject(VictoryInjectionData data, Misc.Dummy.SceneService scm)
        {
            this.trigger = data.Trigger;
            this.dialog = data.Dialog;
            this.buttonNext = data.ButtonNext;
            this.sceneService = scm;
        }

        private void HandleEntrance(GameObject triggerEntryTarget)
        {
            if (!triggerEntryTarget.CompareTag(m_playerTag))
                return;

            ShowDialog();
            sceneService.StopGame();
        }

        private void ShowDialog()
        {
            dialog.gameObject.SetActive(true);
        }

        private void SetSubscriptionState(bool flag)
        {
            if (flag)
            {
                trigger.OnEnter += HandleEntrance;
                buttonNext.onClick.AddListener(() => sceneService.LoadNextSceneOrQuitGame());
            }
            else
            {
                trigger.OnEnter -= HandleEntrance;
                buttonNext.onClick.RemoveListener(() => sceneService.LoadNextSceneOrQuitGame());
            }
        }

        private void OnEnable() => SetSubscriptionState(true);
        private void OnDisable() => SetSubscriptionState(false);
    }
}
