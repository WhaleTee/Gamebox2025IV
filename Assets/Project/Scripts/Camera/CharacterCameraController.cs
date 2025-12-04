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
        [SerializeField] private float zoomNearGods;
        [SerializeField] private float zoomDuration;
        
        private CinemachineCamera _camera;
        private CinemachineFollow cameraFollow;
        private float initZoom;
        private float initFollowOffsetY;
        private CancellationTokenSource cts;
        
        [Inject] 
        public void SetTarget(CameraInjectionData data)
        {
            _camera = data.CameraVirtual;
            cameraFollow = _camera.GetComponent<CinemachineFollow>();
            initZoom = _camera.Lens.OrthographicSize;
            initFollowOffsetY = cameraFollow.FollowOffset.y;
            _camera.Target.TrackingTarget = transform;
        }

        public void SetNearGodZoom()
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();
            UniTask.Void(token => ChangeZoom(zoomNearGods, initFollowOffsetY + zoomNearGods - initZoom, zoomDuration, token), cts.Token);
        }
        
        public void SetDefaultZoom()
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();
            UniTask.Void(token => ChangeZoom(initZoom, initFollowOffsetY, zoomDuration, token), cts.Token);
        }

        private async UniTaskVoid ChangeZoom(float targetZoom, float targetOffsetY, float duration, CancellationToken token)
        {
            var time = 0f;
            var init = _camera.Lens.OrthographicSize;
            var initOffsetY = cameraFollow.FollowOffset.y;
            
            while (time < duration)
            {
                _camera.Lens.OrthographicSize = Mathf.Lerp(init, targetZoom, time / duration);
                SetOffsetY(Mathf.Lerp(initOffsetY, targetOffsetY, time / duration));
                time += Time.deltaTime;
                await UniTask.Yield(token);
            }
            
            _camera.Lens.OrthographicSize = targetZoom;
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