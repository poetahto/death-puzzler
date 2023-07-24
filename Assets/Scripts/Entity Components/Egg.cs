using UnityEngine;

namespace DefaultNamespace
{
    public class Egg : EntityBehaviour
    {
        [SerializeField] private Entity entityPrefab;

        public Entity Hatch()
        {
            var instance = Instantiate(entityPrefab, Entity.Position, Quaternion.identity);
            Destroy(gameObject); // todo: better animation?
            return instance;
        }
    }
}
