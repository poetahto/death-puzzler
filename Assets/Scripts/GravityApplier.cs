using TMPro.EditorUtilities;
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
                Entity.Move(Entity.Position + Vector3Int.down);
                Entity.TargetViewPosition = Entity.Position;
                Entity.TargetViewRotation = Quaternion.identity;
                _cooldown = dropDuration;
                _fallLength++;
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
            }

            _wasGrounded = isGrounded;
        }
    }
}
