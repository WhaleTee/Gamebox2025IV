using UnityEngine;
using UnityEngine.Video;
using Core;
using SceneManagement;
using Misc.Trigger.Collisions;

namespace Core
{
    [RequireComponent(typeof(Trigger))]
    public class VideoTrigger : MonoBehaviour
    {
        [Header("Video Settings")]
        [SerializeField] private VideoPlayer videoPlayer;

        [Header("UI")]
        [SerializeField] private Canvas targetCanvas;

        [Header("Scene Settings")]
        [SerializeField] private string menuSceneName = "MainMenu";

        private bool hasPlayed = false;
        private Trigger trigger;

        private void Awake()
        {
            trigger = GetComponent<Trigger>();
            if (trigger == null)
            {
                Debug.LogError("Trigger component is missing on VideoTrigger!");
                enabled = false;
                return;
            }

            trigger.OnEnter += HandleTriggerEnter;

            if (videoPlayer != null)
                videoPlayer.Stop();
        }

        private void OnDestroy()
        {
            if (trigger != null)
                trigger.OnEnter -= HandleTriggerEnter;
        }

        private void HandleTriggerEnter(Collider2D collider)
        {
            if (hasPlayed) return;
            if (!collider.CompareTag("Player")) return;

            hasPlayed = true;

            PauseManager.RequestPause();

            if (targetCanvas != null)
                targetCanvas.gameObject.SetActive(false);

            if (videoPlayer != null)
            {
                videoPlayer.loopPointReached += OnVideoFinished;
                videoPlayer.Play();
            }
        }

        private void OnVideoFinished(VideoPlayer vp)
        {
            SceneController.Instance.OpenScene(menuSceneName);
        }
    }
}
