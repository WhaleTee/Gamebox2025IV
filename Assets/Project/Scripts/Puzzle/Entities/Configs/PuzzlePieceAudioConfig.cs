using UnityEngine;
using UnityEngine.Audio;
using Audio;

namespace Puzzle
{
    [CreateAssetMenu(menuName = "Scriptables/Puzzle/Entities/Effects/Audio")]
    public class PuzzlePieceAudioConfig : AudioConfigBase
    {
        [field: SerializeField] public int PolyphonyLimit { get; private set; } = 3;
        [field: SerializeField] public AudioResource Deactivated { get; private set; }
        [field: SerializeField] public AudioResource Charged { get; private set; }
        [field: SerializeField] public AudioResource Discharged { get; private set; }
        [field: SerializeField] public AudioResource Broken { get; private set; }
        [field: SerializeField] public AudioResource Damaged { get; private set; }
        [field: SerializeField] public AudioResource DamagedStrong { get; private set; }

        private void OnEnable()
        {
            mixerGroup = Sound.SoundType.Environment;
        }

        protected override void InitPrefab() => InitPrefab("PuzzlePieceAudio");
    }
}