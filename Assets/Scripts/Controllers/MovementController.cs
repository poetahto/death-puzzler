using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class MovementController : MonoBehaviour
    {
        private Queue<Vector3Int> _movementQueue;
        private int _maxQueue;

        private void Awake()
        {
            _maxQueue = 10;
            _movementQueue = new Queue<Vector3Int>(_maxQueue);
        }

        private void OnEnable()
        {
            _movementQueue.Clear();
        }

        protected void Update()
        {
            if (_movementQueue.TryDequeue(out Vector3Int offset))
            {
                foreach (var entity in FindObjectsByType<ControlledEntity>(FindObjectsSortMode.None))
                    entity.HandleMove(offset);
            }
        }

        // === Input ===

        public void HandleUp(InputAction.CallbackContext context)
        {
            if (context.started && _movementQueue.Count <= _maxQueue)
                _movementQueue.Enqueue(Vector3Int.forward);
        }

        public void HandleDown(InputAction.CallbackContext context)
        {
            if (context.started && _movementQueue.Count <= _maxQueue)
                _movementQueue.Enqueue(Vector3Int.back);
        }

        public void HandleLeft(InputAction.CallbackContext context)
        {
            if (context.started && _movementQueue.Count <= _maxQueue)
                _movementQueue.Enqueue(Vector3Int.left);
        }

        public void HandleRight(InputAction.CallbackContext context)
        {
            if (context.started && _movementQueue.Count <= _maxQueue)
                _movementQueue.Enqueue(Vector3Int.right);
        }
    }
}
