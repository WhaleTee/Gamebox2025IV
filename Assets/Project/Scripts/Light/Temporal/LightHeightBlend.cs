using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Cysharp.Threading.Tasks;

namespace Light.Temporal
{
    public class LightHeightBlend : MonoBehaviour
    {
        [SerializeField] private float rooftopPosition;
        [SerializeField] private float undergroundPosition;
        [SerializeField] private Light2D globalLight;
        [SerializeField] private AnimationCurve intensity;
        [SerializeField] private Gradient color;
        private Transform target;
        private float timePosition;

        private void Start() => BeginSeek();

        private void Blend()
        {
            globalLight.color = color.Evaluate(timePosition);
            globalLight.intensity = intensity.Evaluate(timePosition);
        }

        private void Update()
        {
            if (target == null)
                return;
            timePosition = Mathf.InverseLerp(undergroundPosition, rooftopPosition, target.position.y);
            Blend();
        }

        #region SeekForTarget
        private void BeginSeek()
        {
            CancellationTokenSource cts = new();
            UniTask.Action(cts.Token, SeekForTargetOnScene).Invoke();
        }

        private async UniTaskVoid SeekForTargetOnScene(CancellationToken cts)
        {
            while (target == null && !cts.IsCancellationRequested)
            {
                await UniTask.Delay(400);
                target = FindFirstObjectByType<Characters.Hero>(FindObjectsInactive.Exclude).transform;
            }
        }
        #endregion
    }
}