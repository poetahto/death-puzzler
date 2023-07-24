using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Entity))]
    public abstract class EntityBehaviour : MonoBehaviour
    {
        public Entity Entity { get; private set; }

        private void Awake()
        {
            Entity = GetComponent<Entity>();
        }
    }
}