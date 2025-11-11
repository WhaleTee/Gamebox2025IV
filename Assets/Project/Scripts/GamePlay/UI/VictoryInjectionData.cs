using Misc;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay
{
    [Serializable]
    public class VictoryInjectionData
    {
        [field: SerializeField] public ObservableTrigger Trigger { get; private set; }
        [field: SerializeField] public RectTransform Dialog { get; private set; }
        [field: SerializeField] public Button ButtonNext { get; private set; }
    }
}
