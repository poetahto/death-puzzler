using DefaultNamespace;
using UnityEngine;

namespace VFX
{
    public class DeathEyeEffect : MonoBehaviour
    {
        public LivingEntity livingEntity;
        public GameObject deadEyes;
        public GameObject livingEyes;

        private void Start()
        {
            livingEntity.onDeath.AddListener(HandleDeath);
        }

        private void HandleDeath(LivingEntity.DeathEvent eventData)
        {
            livingEyes.SetActive(false);
            deadEyes.SetActive(true);
        }
    }
}
