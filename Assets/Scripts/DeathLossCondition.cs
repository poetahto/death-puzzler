using UnityEngine;

namespace DefaultNamespace
{
    public class DeathLossCondition : MonoBehaviour
    {
        [SerializeField] private LivingEntity[] entities;

        private int _remaining;

        private void Start()
        {
            _remaining = entities.Length;

            foreach (var livingEntity in entities)
            {
                livingEntity.onDeath.AddListener(HandleEntityDeath);
            }
        }

        private void HandleEntityDeath()
        {
            _remaining--;

            if (_remaining <= 0)
            {
                FindAnyObjectByType<GameplayController>().TransitionToDefeat();
            }
        }
    }
}
