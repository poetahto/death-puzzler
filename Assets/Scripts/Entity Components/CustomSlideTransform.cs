using UnityEngine;

namespace DefaultNamespace
{
    public class CustomSlideTransform : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = Vector3.up * 0.5f;
        [SerializeField] private Vector3 angles = Vector3.zero;

        public Vector3 Position => transform.localToWorldMatrix.MultiplyPoint3x4(offset);
        public Vector3 EulerAngles => angles + transform.rotation.eulerAngles;
        public Quaternion Rotation => Quaternion.Euler(EulerAngles); // todo: fix this shit rotation
        public Vector3 Normal => Rotation * Vector3.up;

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(Position, Position + Normal);
        }
    }
}
