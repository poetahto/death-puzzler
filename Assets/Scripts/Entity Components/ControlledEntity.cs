using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ControlledEntity : EntityBehaviour
    {
        [SerializeField]
        private Transform facingTransform;

        [SerializeField]
        private Animator movementAnimator;

        private Vector3Int _grabDirection = Vector3Int.forward;
        private bool _isGrabbing;
        private Entity _grabbedEntity;

        private void Start()
        {
            Entity.onMove.AddListener(CheckGrounded);
        }

        private void CheckGrounded(Entity.MoveEvent eventData)
        {
            bool isGrounded = Entity.IsGrounded();
            movementAnimator.SetBool("isGrounded", isGrounded);

            if (_isGrabbing && !isGrounded)
                HandleDrop();
        }

        private void Update()
        {
            var t = 15 * Time.deltaTime;
            facingTransform.localRotation = Quaternion.Lerp(facingTransform.localRotation, Quaternion.LookRotation(-_grabDirection), t);
        }

        public void HandleMove(Vector3Int offset)
        {
            if (Entity.IsGrounded() && enabled)
            {
                if (_isGrabbing)
                {
                    Vector3 gePos = _grabbedEntity.Position;
                    Vector3 ePos = Entity.Position;
                    Vector3 legalDirection = ePos - gePos;
                    legalDirection.y = 0;
                    legalDirection.Normalize();
                    Vector3Int finalOffset = Vector3Int.CeilToInt(legalDirection * Mathf.Clamp01(Vector3.Dot(legalDirection, offset)));

                    _grabbedEntity.Slide(finalOffset);
                }
                else
                {
                    _grabDirection = offset;
                    Entity.Slide(offset);
                }
            }
        }

        public void HandleGrab()
        {
            if (_isGrabbing)
                return;

            for (int i = 0; i < 4; i++)
            {
                _grabDirection = Vector3Int.RoundToInt(Quaternion.Euler(0, 90, 0) * _grabDirection);

                if (TryGrabNeighbor(_grabDirection, out _grabbedEntity))
                {
                    _isGrabbing = true;
                    break;
                }
            }

            movementAnimator.SetBool("grabbing", _isGrabbing);
        }

        private bool TryGrabNeighbor(Vector3Int direction, out Entity entity)
        {
            entity = Entity.GetNeighbor(direction);

            if (entity.IsPushable())
                return true;

            if (Entity.GetNeighbor(direction).TryGetComponent(out Stairs _))
            {
                entity = Entity.GetNeighbor(direction + Vector3Int.up);

                if (entity.IsPushable())
                    return true;
            }

            return false;
        }

        public void HandleDrop()
        {
            movementAnimator.SetBool("grabbing", false);
            _isGrabbing = false;
        }
    }
}
