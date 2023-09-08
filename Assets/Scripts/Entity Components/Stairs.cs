using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Stairs : EntityBehaviour
    {
        [SerializeField] private Vector3Int[] entrancePositions = Array.Empty<Vector3Int>();
        [SerializeField] private Color gizmoColor = Color.red;

        public bool CanEntityEnter(Entity entity)
        {
            foreach (var entrance in entrancePositions)
            {
                if (entity.Position == Entity.Logic.localToWorldMatrix.MultiplyPoint3x4(entrance))
                    return true;
            }

            return false;
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var entrance in entrancePositions)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawCube(transform.localToWorldMatrix.MultiplyPoint3x4(entrance), Vector3.one);
            }
        }
    }
}
