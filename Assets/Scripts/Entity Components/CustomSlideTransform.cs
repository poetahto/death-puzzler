using UnityEngine;

namespace DefaultNamespace
{
    public class CustomSlideTransform : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = Vector3.up * 0.5f;
        [SerializeField] private float angle = 45;

        public Vector3 Position => transform.localToWorldMatrix.MultiplyPoint3x4(offset);
        public Quaternion Rotation => Quaternion.AngleAxis(angle, transform.forward);
        public Vector3 Normal => Rotation * Vector3.up;

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(Position, Position + Normal);
        }
    }
}
