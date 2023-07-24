using UnityEngine;

namespace DefaultNamespace.UI
{
    public class RestartLevelLogic : MonoBehaviour
    {
        public void Run()
        {
            FindAnyObjectByType<GameplayStateMachine>().Restart();
        }
    }
}
