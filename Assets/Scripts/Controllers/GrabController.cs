using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class GrabController : MonoBehaviour
    {
        private bool _isGrabbing;
        private double _time;

        public void HandleGrab(InputAction.CallbackContext context)
        {
            if (context.performed && !_isGrabbing && _time < context.time)
            {
                _isGrabbing = true;
                _time = context.time;

                foreach (var entity in FindObjectsByType<ControlledEntity>(FindObjectsSortMode.None))
                    entity.HandleGrab();
            }
        }

        public void HandleDrop(InputAction.CallbackContext context)
        {
            if (context.performed && _isGrabbing && _time < context.time)
            {
                _isGrabbing = false;
                _time = context.time;

                foreach (var entity in FindObjectsByType<ControlledEntity>(FindObjectsSortMode.None))
                    entity.HandleDrop();
            }
        }
    }
}
