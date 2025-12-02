using System.Threading;
using Characters;
using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using Unity.Cinemachine;
using UnityEngine;

namespace Camera
{
    public class CharacterCameraController : MonoBehaviour
    {
        [Header("God Zone Settings")]
        [SerializeField] private float zoomNearGods;
        [SerializeField] private float zoomDuration;

        [Header("FarView Zone Settings")]
        [SerializeField] private float farViewOffsetY;   // Смещение Y при FarView
        [SerializeField] private float farViewDuration;  // Плавность смещения Y

        private CinemachineCamera _camera;
        private CinemachineFollow cameraFollow;

        private float initZoom;
        private float initOffsetY;

        private CancellationTokenSource ctsZoom;   // Для зума
        private CancellationTokenSource ctsOffset; // Для смещения Y

        [Inject]
        public void SetTarget(CameraInjectionData data)
        {
            _camera = data.CameraVirtual;
            cameraFollow = _camera.GetComponent<CinemachineFollow>();

            initZoom = _camera.Lens.OrthographicSize;
            initOffsetY = cameraFollow.FollowOffset.y;

            _camera.Target.TrackingTarget = transform;
        }

        // ============================================
        // GOD ZONE METHODS (только зум)
        // ============================================
        public void EnterGodZone()
        {
            ctsZoom?.Cancel();
            ctsZoom = new CancellationTokenSource();

            float targetOffsetY = cameraFollow.FollowOffset.y;
            // Y остаётся текущим, зум только изменяем

            UniTask.Void(token =>
                ChangeZoom(zoomNearGods, targetOffsetY, zoomDuration, token),
                ctsZoom.Token
            );
        }

        public void ExitGodZone()
        {
            ctsZoom?.Cancel();
            ctsZoom = new CancellationTokenSource();

            UniTask.Void(token =>
                ChangeZoom(initZoom, cameraFollow.FollowOffset.y, zoomDuration, token),
                ctsZoom.Token
            );
        }

        // ============================================
        // FAR VIEW ZONE METHODS (только смещение Y)
        // ============================================
        public void EnterFarView()
        {
            ctsOffset?.Cancel();
            ctsOffset = new CancellationTokenSource();

            float targetOffsetY = initOffsetY + farViewOffsetY;

            UniTask.Void(token =>
                ChangeOffsetY(targetOffsetY, farViewDuration, token),
                ctsOffset.Token
            );
        }

        public void ExitFarView()
        {
            ctsOffset?.Cancel();
            ctsOffset = new CancellationTokenSource();

            UniTask.Void(token =>
                ChangeOffsetY(initOffsetY, farViewDuration, token),
                ctsOffset.Token
            );
        }

        // ============================================
        // PRIVATE METHODS
        // ============================================
        /// <summary>
        /// Плавное изменение зума, Y остаётся текущим
        /// </summary>
        private async UniTaskVoid ChangeZoom(float targetZoom, float offsetY, float duration, CancellationToken token)
        {
            float time = 0f;
            float startZoom = _camera.Lens.OrthographicSize;

            while (time < duration)
            {
                float t = time / duration;
                _camera.Lens.OrthographicSize = Mathf.Lerp(startZoom, targetZoom, t);
                SetOffsetY(offsetY); // Y фиксированный
                time += Time.deltaTime;
                await UniTask.Yield(token);
            }

            _camera.Lens.OrthographicSize = targetZoom;
            SetOffsetY(offsetY);
        }

        /// <summary>
        /// Плавное изменение только смещения Y
        /// </summary>
        private async UniTaskVoid ChangeOffsetY(float targetOffsetY, float duration, CancellationToken token)
        {
            float time = 0f;
            float startOffsetY = cameraFollow.FollowOffset.y;

            while (time < duration)
            {
                float t = time / duration;
                SetOffsetY(Mathf.Lerp(startOffsetY, targetOffsetY, t));

                time += Time.deltaTime;
                await UniTask.Yield(token);
            }

            SetOffsetY(targetOffsetY);
        }

        private void SetOffsetY(float offsetY)
        {
            var offset = cameraFollow.FollowOffset;
            offset.y = offsetY;
            cameraFollow.FollowOffset = offset;
        }
    }
}
