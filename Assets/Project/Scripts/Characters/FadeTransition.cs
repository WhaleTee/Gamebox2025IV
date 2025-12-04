using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Renderer))]
    public class FadeTransition : MonoBehaviour
    {
        [SerializeField] private float m_duration = 1.2f;
        [SerializeField] private string m_shaderProperty = "_Fade";

        private Renderer _renderer;
        private MaterialPropertyBlock mbp;
        private int fadePropertyID;

        private float current;
        private float velocity;
        private bool transitionToVisible;

        public void Hide() => transitionToVisible = false;
        public void Appear() => transitionToVisible = true;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            mbp = new();
            fadePropertyID = Shader.PropertyToID(m_shaderProperty);

            _renderer.GetPropertyBlock(mbp);
            current = mbp.GetFloat(fadePropertyID);
        }

        private void Update()
        {
            float target = transitionToVisible ? 1f : 0;

            if (Mathf.Approximately(current, target))
                return;

            current = Mathf.SmoothDamp(current, target, ref velocity, m_duration);

            Apply(current);
        }

        private void Apply(float value)
        {
            _renderer.GetPropertyBlock(mbp);
            mbp.SetFloat(fadePropertyID, value);
            _renderer.SetPropertyBlock(mbp);
        }
    }
}
