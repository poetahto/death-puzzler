using UnityEngine;

namespace DefaultNamespace
{
    public class DeathLossCondition : MonoBehaviour
    {
        [SerializeField] private int remaining;

        public void HandleEntityDeath()
        {
            remaining--;

            if (remaining <= 0)
            {
                FindAnyObjectByType<GameplayController>().TransitionToDefeat();
            }
        }
    }
}
