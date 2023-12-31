﻿using UnityEngine;

namespace DefaultNamespace
{
    public class DeathLossCondition : MonoBehaviour
    {
        private int _remaining;

        private void Start()
        {
            _remaining += FindObjectsByType<Egg>(FindObjectsSortMode.None).Length;
            _remaining += FindObjectsByType<ControlledEntity>(FindObjectsSortMode.None).Length;
        }

        public void HandleEntityDeath()
        {
            _remaining--;

            if (_remaining <= 0)
            {
                FindAnyObjectByType<GameplayStateMachine>().TransitionToDefeat();
            }
        }
    }
}
