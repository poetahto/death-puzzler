using UnityEngine;

namespace DefaultNamespace
{
    public class PuzzleWorldGridContainer : MonoBehaviour
    {
        [SerializeField] private Vector3Int size;
        [SerializeField] private PuzzleWorldEntity defaultEntityPrefab;

        public Vector3Int Size => size;
        public PuzzleWorldGrid PuzzleWorld { get; private set; }

        private void Start()
        {
            PuzzleWorldEntity[] prePlacedEntities = FindObjectsByType<PuzzleWorldEntity>(FindObjectsSortMode.None);
            PuzzleWorld = new PuzzleWorldGrid(size, defaultEntityPrefab, transform);
            var clampedPos = Vector3Int.RoundToInt(transform.position);
            transform.position = clampedPos;

            foreach (var puzzleWorldEntity in prePlacedEntities)
            {
                PuzzleWorld.Set(puzzleWorldEntity.Position - clampedPos, puzzleWorldEntity);
            }
        }
    }
}
