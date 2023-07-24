using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField]
        private GameObject pauseUI;

        public void HandlePause(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                // Toggle the pause menu when pressed.
                pauseUI.SetActive(!pauseUI.activeSelf);
            }
        }
    }
}
