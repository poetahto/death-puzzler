using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(PuzzleWorldEntity))]
    public abstract class EntityBehaviour : MonoBehaviour
    {
        public PuzzleWorldEntity Entity { get; private set; }

        private void Awake()
        {
            Entity = GetComponent<PuzzleWorldEntity>();
        }
    }
}