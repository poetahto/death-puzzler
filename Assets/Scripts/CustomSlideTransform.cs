using UnityEngine;

namespace DefaultNamespace
{
    public class CustomSlideTransform : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = Vector3.up * 0.5f;
        [SerializeField] private Vector3 normal = Vector3.up;

        public Vector3 Position => transform.localToWorldMatrix.MultiplyPoint3x4(offset);
        public Quaternion Rotation => Quaternion.LookRotation(Vector3Int.forward, transform.localToWorldMatrix.MultiplyVector(normal));

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(Position, Position + transform.localToWorldMatrix.MultiplyVector(normal));
        }
    }
}
