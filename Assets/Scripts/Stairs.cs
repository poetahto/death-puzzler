using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Stairs : MonoBehaviour
    {
        [SerializeField] private Vector3Int[] entrancePositions = Array.Empty<Vector3Int>();
        [SerializeField] private Color gizmoColor = Color.red;

        public bool CanEntityEnter(PuzzleWorldEntity entity)
        {
            foreach (var entrance in entrancePositions)
            {

                if (entity.Position == transform.localToWorldMatrix.MultiplyPoint3x4(entrance))
                    return true;
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            foreach (var entrance in entrancePositions)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawCube(transform.localToWorldMatrix.MultiplyPoint3x4(entrance), Vector3.one);
            }
        }
    }
}
