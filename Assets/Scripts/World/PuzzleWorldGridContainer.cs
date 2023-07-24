using UnityEngine;

namespace DefaultNamespace
{
    public class PuzzleWorldGridContainer : MonoBehaviour
    {
        [SerializeField] private Vector3Int size;
        [SerializeField] private Entity defaultEntityPrefab;

        public Vector3Int Size => size;
        public PuzzleWorldGrid PuzzleWorld { get; private set; }

        private void Awake()
        {
            PuzzleWorld = new PuzzleWorldGrid(size, defaultEntityPrefab, transform);
        }

        private void Start()
        {
            PuzzleWorld.Initialize();
        }
    }
}
