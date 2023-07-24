using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class HatchingController : MonoBehaviour
    {
        private List<Egg> _spawners;
        public int SpawnIndex { get; private set; }

        private void Start()
        {
            _spawners = FindObjectsByType<Egg>(FindObjectsSortMode.None).ToList();
        }

        public void HandleHatchBackward(InputAction.CallbackContext context)
        {
            if (context.started)
                SpawnIndex = Repeat(SpawnIndex - 1, _spawners.Count);
        }

        public void HandleHatchForward(InputAction.CallbackContext context)
        {
            if (context.started)
                SpawnIndex = Repeat(SpawnIndex + 1, _spawners.Count);
        }

        public void HandleHatchSelect(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Egg egg = _spawners[SpawnIndex];
                var instance = egg.Hatch();
                _spawners.Remove(egg);
                SpawnIndex = 0;

                if (instance.TryGetComponent(out LivingEntity livingEntity))
                {
                    livingEntity.onDeath.AddListener(HandleEntityDeath);
                }

                FindAnyObjectByType<GameplayStateMachine>().TransitionToPlaying();
            }
        }

        private void HandleEntityDeath(LivingEntity.DeathEvent eventData)
        {
            eventData.entity.onDeath.RemoveListener(HandleEntityDeath);

            if (_spawners.Count > 0)
                FindAnyObjectByType<GameplayStateMachine>().TransitionToHatching();
        }

        private static int Repeat(int value, int max)
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
    }
}
