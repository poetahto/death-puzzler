using UnityEngine;

namespace DefaultNamespace
{
    public class GravityApplier : EntityBehaviour
    {
        [SerializeField] private float dropDuration = 0.5f;
        [SerializeField] private int fallDamageLength = 2;

        private float _cooldown;
        private bool _wasGrounded;
        private int _fallLength;

        public bool IsFalling => _fallLength > 0;

        private void Update()
        {
            bool isGrounded = Entity.IsGrounded();

            if (!isGrounded && _wasGrounded) // Just left ground
            {
                _cooldown = dropDuration;
                _fallLength = 0;
            }

            if (!isGrounded && _cooldown <= 0) // Falling in air, apply gravity.
            {
                var above = Entity.GetAbove();
                Drop();

                if (above.TryGetComponent(out GravityApplier gravityApplier))
                    gravityApplier.Drop();
            }
            else // Grounded or delayed.
            {
                _cooldown = Mathf.Max(0, _cooldown - Time.deltaTime);
            }

            if (isGrounded && !_wasGrounded) // Just hit ground
            {
                if (_fallLength >= fallDamageLength)
                {
                    Entity.TryKill();
                }

                _fallLength = 0;
            }

            _wasGrounded = isGrounded;
        }

        private void Drop()
        {
            Entity.Move(Entity.Position + Vector3Int.down);
            Entity.UpdateView();
            _cooldown = dropDuration;
            _fallLength++;
        }
    }

    public static class GravityExtensions
    {
        public static bool IsFalling(this Entity entity)
        {
            return entity.TryGetComponent(out GravityApplier gravityApplier) && gravityApplier.IsFalling;

        }
    }
}
