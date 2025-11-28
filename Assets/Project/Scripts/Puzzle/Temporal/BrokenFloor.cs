using UnityEngine;

namespace Puzzle.Temporal
{
    public class BrokenFloor : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] m_renderers;
        [SerializeField] private Collider2D m_collider;
        [SerializeField] private Collider2D m_trigger;
        [SerializeField] private AudioSource[] m_audios;
        [SerializeField] private ParticleSystem m_hugeShards;
        [SerializeField] private ParticleSystem m_smallShards;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
                return;

            DisableColliders();
            PlaySound();
            PlayParticles();
            DisableVisuals();
        }

        private void PlaySound()
        {
            foreach (var a in m_audios)
                a.Play();
        }

        private void PlayParticles()
        {
            m_hugeShards.Play();
            m_smallShards.Play();
        }

        private void DisableColliders()
        {
            m_trigger.enabled = false;
            m_collider.enabled = false;
        }

        private void DisableVisuals()
        {
            foreach (var r in m_renderers)
                r.enabled = false;
        }
    }
}