using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    [Serializable]
    public class BehavioursContainer
    {
        [SerializeField] public BehaviourType Behaviours;
        [SerializeField] public List<BehaviourBaseConfig> Configs;
    }
}