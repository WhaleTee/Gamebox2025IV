using Combat;
using Environment;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Puzzle.Temporal
{
    public class PuzzleSphere : MonoBehaviour, IDamageable
    {
        [SerializeField] private SpriteRenderer m_myRenderer;
        [SerializeField] private Sprite m_notActive;
        [SerializeField] private Sprite m_charged;
        [SerializeField] private AudioSource m_audio;
        [SerializeField] private AudioClip m_clipCharged;
        [SerializeField] private AudioClip m_clipsDiscahrged;
        [SerializeField] private Light2D m_light;
        [SerializeField] private MovingPlatform[] m_platforms;
        private bool active;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Projectile"))
                return;

            HandleCollision();
        }

        private void HandleCollision()
        {
            active = !active;

            Activate(active);
            ChangeSprite(active);
            ActivateLight(active);
            PlaySound(active);
        }

        private void PlaySound(bool value)
        {
            if (value)
                m_audio.clip = m_clipCharged;
            else
                m_audio.clip = m_clipsDiscahrged;
            m_audio.Play();
        }

        private void Activate(bool value)
        {
            foreach (var platform in m_platforms)
                platform.SetActive(value);
        }

        private void ChangeSprite(bool value)
        {
            if (value)
                m_myRenderer.sprite = m_charged;
            else
                m_myRenderer.sprite = m_notActive;
        }

        private void ActivateLight(bool value)
        {
            m_light.enabled = value;
        }

        public void SetDamageSource(IDamageSource damageSource) {}
        public void InflictDamage(DamageBundle damage) {}
    }
}
