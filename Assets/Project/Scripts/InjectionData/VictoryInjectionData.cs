using System;
using UnityEngine;
using UnityEngine.UI;
using Misc.Trigger.Collisions;

namespace GamePlay
{
    [Serializable]
    public class VictoryInjectionData
    {
        [field: SerializeField] public Trigger Trigger { get; private set; }
        [field: SerializeField] public RectTransform Dialog { get; private set; }
        [field: SerializeField] public Button ButtonNext { get; private set; }
    }
}
