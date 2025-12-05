using System;
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
        [SerializeField] private float farViewOffsetY;
        [SerializeField] private float farViewDuration;

        private CinemachineCamera _camera;
        private CinemachineFollow _cameraFollow;

        private float _initZoom;
        private float _initOffsetY;

        private CancellationTokenSource _ctsZoom;
        private CancellationTokenSource _ctsOffset;

        [Inject]
        public void SetTarget(CameraInjectionData data)
        {
            _camera = data.CameraVirtual;
            _cameraFollow = _camera.GetComponent<CinemachineFollow>();

            _initZoom = _camera.Lens.OrthographicSize;
            _initOffsetY = _cameraFollow.FollowOffset.y;

            _camera.Target.TrackingTarget = transform;
        }

        /// <summary>
        /// Публичные методы зон
        /// </summary>
        public void EnterGodZone() => AnimateCameraValue(() => _camera.Lens.OrthographicSize, val => _camera.Lens.OrthographicSize = val, zoomNearGods, zoomDuration, ref _ctsZoom);
        public void ExitGodZone() => AnimateCameraValue(() => _camera.Lens.OrthographicSize, val => _camera.Lens.OrthographicSize = val, _initZoom, zoomDuration, ref _ctsZoom);

        public void EnterFarView() => AnimateCameraValue(() => _cameraFollow.FollowOffset.y, SetOffsetY, _initOffsetY + farViewOffsetY, farViewDuration, ref _ctsOffset);
        public void ExitFarView() => AnimateCameraValue(() => _cameraFollow.FollowOffset.y, SetOffsetY, _initOffsetY, farViewDuration, ref _ctsOffset);

        /// <summary>
        /// Универсальный метод анимации
        /// </summary>
        private void AnimateCameraValue(Func<float> getter, Action<float> setter, float target, float duration, ref CancellationTokenSource cts)
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();

            UniTask.Void(async token =>
            {
                float start = getter();
                float time = 0f;

                while (time < duration)
                {
                    setter(Mathf.Lerp(start, target, time / duration));
                    time += Time.deltaTime;
                    await UniTask.Yield(token);
                }

                setter(target);
            }, cts.Token);
        }

        private void SetOffsetY(float offsetY)
        {
            var offset = _cameraFollow.FollowOffset;
            offset.y = offsetY;
            _cameraFollow.FollowOffset = offset;
        }
    }
}
