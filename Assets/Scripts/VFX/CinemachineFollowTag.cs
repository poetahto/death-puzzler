using Cinemachine;
using UnityEngine;

namespace VFX
{
    public class CinemachineFollowTag : MonoBehaviour
    {
        public string targetTag;

        private void Start()
        {
            if (TryGetComponent(out CinemachineVirtualCamera cam))
                cam.Follow = GameObject.FindWithTag(targetTag).transform;
        }
    }
}
