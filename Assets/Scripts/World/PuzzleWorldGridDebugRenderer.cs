using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(PuzzleWorldGridContainer))]
    public class PuzzleWorldGridDebugRenderer : MonoBehaviour
    {
        [SerializeField] private Color color = Color.white;

        private void OnDrawGizmos()
        {
            var container = GetComponent<PuzzleWorldGridContainer>();
            Gizmos.color = color;
            Gizmos.DrawWireCube(container.transform.position + (Vector3) container.Size / 2.0f - Vector3.one * 0.5f, container.Size);
        }
    }
}
