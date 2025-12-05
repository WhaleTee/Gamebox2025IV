using UnityEngine;

namespace Visual
{
    public class CharacterParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] prefabs;
        private ParticleSystem[][] particles;
        private int[] counts;
        private int[] currents;

        private void Awake()
        {
            int enumsCount = System.Enum.GetValues(typeof(CharacterParticleType)).Length;

            counts = new int[] { 5, 2, 2 };
            currents = new int[enumsCount];
            particles = new ParticleSystem[enumsCount][];

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new ParticleSystem[counts[i]];
                for (int k = 0; k < counts[i]; k++)
                    particles[i][k] = Instantiate(prefabs[i], transform, false);
            }
        }

        public void Play(CharacterParticleType type)
        {
            int index = (int)type;
            if (currents[index] > counts[index])
                currents[index] = 0;
            particles[index][currents[index]].Play();
            currents[index]++;
        }
    }

    public enum CharacterParticleType
    {
        Footstep,
        Jump,
        Landing
    }
}