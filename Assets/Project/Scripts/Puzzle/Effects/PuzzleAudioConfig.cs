using Audio;
using UnityEngine;

namespace Puzzle
{
    [CreateAssetMenu(menuName = "Scriptables/Puzzle/Effects/Audio")]
    public class PuzzleAudioConfig : AudioConfigBase
    {
        [field: SerializeField] public AudioClip Solved { get; private set; }
        [field: SerializeField] public AudioClip Failed { get; private set; }
        [field: SerializeField] public AudioClip Reset { get; private set; }

        private void OnEnable()
        {
            mixerGroup = MixerGroup.Environment;
        }

        protected override void InitPrefab() => InitPrefab("PuzzleAudio");
    }
}
