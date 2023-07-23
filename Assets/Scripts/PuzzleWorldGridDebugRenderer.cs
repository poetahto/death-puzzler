using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(PuzzleWorldGridContainer))]
    public class PuzzleWorldGridDebugRenderer : MonoBehaviour
    {
        // [SerializeField] private float size = 0.5f;
        [SerializeField] private Color color = Color.white;
        [SerializeField] private Vector3Int offset;

        private void OnDrawGizmos()
        {
            var container = GetComponent<PuzzleWorldGridContainer>();
            Gizmos.color = color;
            Gizmos.DrawWireCube(container.transform.position + (Vector3) container.Size / 2.0f - Vector3.one * 0.5f, container.Size);

            // for (int x = 0; x < container.Size.x; x++)
            // {
            //     for (int y = 0; y < container.Size.y; y++)
            //     {
            //         for (int z = 0; z < container.Size.z; z++)
            //         {
            //             Gizmos.color = color;
            //             Gizmos.DrawWireCube(new Vector3(x, y, z), Vector3.one * size);
            //         }
            //     }
            // }
        }
    }
}
