using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class LivingEntity : MonoBehaviour
    {
        public UnityEvent<DeathEvent> onDeath;

        public bool IsAlive { get; private set; } = true;

        public void Kill()
        {
            if (IsAlive)
            {
                IsAlive = false;
                onDeath.Invoke(new DeathEvent{entity = this});
            }
        }

        public struct DeathEvent
        {
            public LivingEntity entity;
        }
    }

    public static class LivingEntityExtensions
    {
        public static bool IsAlive(this Entity entity)
        {
            return entity.TryGetComponent(out LivingEntity livingEntity) && livingEntity.IsAlive;
        }

        public static bool TryKill(this Entity entity)
        {
            if (entity.TryGetComponent(out LivingEntity livingEntity))
            {
                livingEntity.Kill();
                return true;
            }

            return false;
        }
    }
}
