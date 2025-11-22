using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Characters
{
    [Serializable]
    public class CameraInjectionData
    {
        [field: SerializeField] public CinemachineCamera CameraVirtual { get; private set; }
        [field: SerializeField] public UnityEngine.Camera Camera { get; private set; }
    }
}
