using UnityEngine;
using UnityEngine.UI;
using Reflex.Attributes;
using Misc;

namespace GamePlay
{
    public class ResultPresenter : MonoBehaviour
    {
        [SerializeField] private string m_playerTag = "Player";
        private ITrigger<Collider2D> trigger;
        private RectTransform dialog;
        private Button buttonNext;

        [Inject]
        public void Inject(VictoryInjectionData data)
        {
            this.trigger = data.Trigger;
            this.dialog = data.Dialog;
            this.buttonNext = data.ButtonNext;
        }

        private void HandleEntrance(Collider2D triggerEntryTarget)
        {
            if (!triggerEntryTarget.CompareTag(m_playerTag))
                return;

            ShowModal();
        }

        private void SetModal(bool state) => dialog.gameObject.SetActive(state);
        private void ShowModal() => SetModal(true);
        private void HideModal() => SetModal(false);

        private void SetSubscriptionState(bool flag)
        {
            if (flag)
            {
                trigger.OnEnter += HandleEntrance;
                buttonNext.onClick.AddListener(HideModal);
            }
            else
            {
                trigger.OnEnter -= HandleEntrance;
                buttonNext.onClick.RemoveListener(HideModal);
            }
        }

        private void OnEnable() => SetSubscriptionState(true);
        private void OnDisable() => SetSubscriptionState(false);
    }
}
