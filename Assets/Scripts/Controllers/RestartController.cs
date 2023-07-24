using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class RestartController : MonoBehaviour
    {
        public void HandleRestart(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                FindAnyObjectByType<GameplayStateMachine>().Restart();
            }
        }
    }
}
