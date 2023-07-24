using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(PuzzleEntity))]
    public abstract class EntityBehaviour : MonoBehaviour
    {
        public PuzzleEntity Entity { get; private set; }

        private void Awake()
        {
            Entity = GetComponent<PuzzleEntity>();
        }
    }
}