using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(PuzzleWorldEntity))]
    public class GravityApplier : MonoBehaviour
    {
        [SerializeField] private float dropDuration;

        private PuzzleWorldEntity _entity;

        private void Start()
        {
            _entity = GetComponent<PuzzleWorldEntity>();
        }

        private void Update()
        {
        }
    }
}