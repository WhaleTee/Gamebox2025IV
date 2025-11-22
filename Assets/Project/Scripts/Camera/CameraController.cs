using UnityEngine;
using Reflex.Attributes;
using Characters;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [Inject] 
        public void SetTarget(CameraInjectionData data)
        {
            data.CameraVirtual.Target.TrackingTarget = transform;
        }
    }
}
