using Characters;
using GamePlay;
using System;
using UnityEngine;

namespace DI
{
    [Serializable]
    public class GamePlaySceneDependencies : MonoBehaviour
    {
        [field: SerializeField] public PlayerInjectionData PlayerData { get; private set; }
        [field: SerializeField] public VictoryInjectionData VictoryData { get; private set; }
    }
}
