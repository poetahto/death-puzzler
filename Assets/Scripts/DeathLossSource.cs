using UnityEngine;

namespace DefaultNamespace
{
    public class DeathLossSource : MonoBehaviour
    {
        public void HandleDeath()
        {
            FindAnyObjectByType<DeathLossCondition>().HandleEntityDeath();
        }
    }
}