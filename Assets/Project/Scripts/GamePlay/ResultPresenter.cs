using UnityEngine;
using UnityEngine.UI;
using Reflex.Attributes;
using Misc;

namespace GamePlay
{
    public class ResultPresenter
    {
        [SerializeField] private string m_playerTag = "Player";
        private ITrigger trigger;
        private RectTransform dialog;
        private Button closeButton;
        [Inject] private Misc.Dummy.SceneService scm;

        [Inject]
        public void Construct(ITrigger trigger, RectTransform dialog, Button closeButton)
        {
            this.trigger = trigger;
            this.dialog = dialog;
            this.closeButton = closeButton;
            OnEnable(); 
        }

        private void HandleEntrance(GameObject triggerEntryTarget)
        {
            if (triggerEntryTarget.tag != m_playerTag)
                return;

            ShowDialog();
            scm.StopGame();
        }

        private void ShowDialog()
        {
            dialog.gameObject.SetActive(true);
            closeButton.onClick.AddListener(scm.LoadNextSceneOrQuitGame);
            closeButton.onClick.AddListener(() => closeButton.onClick.RemoveAllListeners());
        }

        private void SetSubscriptionState(bool flag)
        {
            if (flag)
                trigger.OnEnter += HandleEntrance;
            else
                trigger.OnEnter -= HandleEntrance;
        }

        private void OnEnable() => SetSubscriptionState(true);
        private void OnDisable() => SetSubscriptionState(false);
    }
}
