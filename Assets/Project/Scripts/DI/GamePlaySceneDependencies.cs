using System;
using UnityEngine;
using GamePlay;
using Characters;
using Sound;
using Assets.Project.Scripts.InjectionData;
using VisualEffects;

namespace DI
{
    [Serializable]
    public class GamePlaySceneDependencies : MonoBehaviour
    {
        [field: SerializeField] public PlayerInjectionData PlayerData { get; private set; }
        [field: SerializeField] public VictoryInjectionData VictoryData { get; private set; }
        [field: SerializeField] public AudioInjectionData AudioData { get; private set; }
        [field: SerializeField] public SoundSettingsInjectionData SoundSettingsData { get; private set; }
        [field: SerializeField] public ParticlesInjectionData ParticlesData { get; private set; }
        [field: SerializeField] public CameraInjectionData CameraInjectionData { get; private set; }
        [field: SerializeField] public UpdatesInjectionData UpdatesInjectionData { get; private set; }
    }
}
