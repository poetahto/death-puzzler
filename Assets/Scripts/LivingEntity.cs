using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class LivingEntity : MonoBehaviour
    {
        public UnityEvent onDeath;

        public bool IsAlive { get; private set; } = true;

        public void Kill()
        {
            if (IsAlive)
            {
                IsAlive = false;
                onDeath.Invoke();
            }
        }
    }

    public static class LivingEntityExtensions
    {
        public static bool IsAlive(this PuzzleEntity entity)
        {
            return entity.TryGetComponent(out LivingEntity livingEntity) && livingEntity.IsAlive;
        }

        public static bool TryKill(this PuzzleEntity entity)
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
