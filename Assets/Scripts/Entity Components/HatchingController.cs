using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class HatchingController : MonoBehaviour
    {
        [SerializeField] private List<Egg> spawners;

        public int SpawnIndex { get; private set; }

        private int Repeat(int value, int max)
        {
            max = Mathf.Max(1, max);

            while (value >= max)
            {
                value -= max;
            }

            while (value < 0)
            {
                value += max;
            }

            return value;
        }

        public void HandleHatchBackward(InputAction.CallbackContext context)
        {
            if (context.started)
                SpawnIndex = Repeat(SpawnIndex - 1, spawners.Count);
        }

        public void HandleHatchForward(InputAction.CallbackContext context)
        {
            if (context.started)
                SpawnIndex = Repeat(SpawnIndex + 1, spawners.Count);
        }

        public void HandleHatchSelect(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Egg egg = spawners[SpawnIndex];
                var instance = egg.Hatch();
                spawners.Remove(egg);
                SpawnIndex = 0;

                if (instance.TryGetComponent(out LivingEntity livingEntity))
                {
                    livingEntity.onDeath.AddListener(HandleEntityDeath);
                }

                FindAnyObjectByType<GameplayController>().TransitionToPlaying();
            }
        }

        private void HandleEntityDeath(LivingEntity.DeathEvent eventData)
        {
            eventData.entity.onDeath.RemoveListener(HandleEntityDeath);

            if (spawners.Count > 0)
                FindAnyObjectByType<GameplayController>().TransitionToHatching();
        }
    }
}
