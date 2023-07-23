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
                if (entity.Position - Vector3Int.RoundToInt(transform.position) == entrance)
                    return true;
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            foreach (var entrance in entrancePositions)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawCube(Vector3Int.RoundToInt(transform.position) + entrance, Vector3.one);
            }
        }
    }
}
