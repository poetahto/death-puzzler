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
        public Egg SelectedEgg { get; private set; }

        private void Awake()
        {
            _spawners = FindObjectsByType<Egg>(FindObjectsSortMode.None).ToList();
            ResetSelection();
        }

        public void HandleHatchBackward(InputAction.CallbackContext context)
        {
            if (context.started)
                UpdateSelectedEntity(-1);
        }

        public void HandleHatchForward(InputAction.CallbackContext context)
        {
            if (context.started)
                UpdateSelectedEntity(1);
        }

        public void HandleHatchSelect(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                var instance = SelectedEgg.Hatch();
                _spawners.Remove(SelectedEgg);
                ResetSelection();

                if (instance.TryGetComponent(out LivingEntity livingEntity))
                    livingEntity.onDeath.AddListener(HandleEntityDeath);

                FindAnyObjectByType<GameplayStateMachine>().TransitionToPlaying();
            }
        }

        private void ResetSelection()
        {
            SpawnIndex = 0;

            if (_spawners.Count > 0)
                SelectedEgg = _spawners[0];
        }

        private void UpdateSelectedEntity(int offset)
        {
            if (SelectedEgg != null)
                SelectedEgg.Deselect();

            SpawnIndex = Repeat(SpawnIndex + offset, _spawners.Count);
            SelectedEgg = _spawners[SpawnIndex];
            SelectedEgg.Select();
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
