using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Entity))]
    public abstract class EntityBehaviour : MonoBehaviour
    {
        public Entity Entity { get; private set; }
        public PuzzleWorldGrid World => Entity.World;

        private void Awake()
        {
            Entity = GetComponent<Entity>();
        }
    }
}
