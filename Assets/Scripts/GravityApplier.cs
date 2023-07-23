using UnityEngine;

namespace DefaultNamespace
{
    public class GravityApplier : EntityBehaviour
    {
        [SerializeField] private float dropDuration = 0.5f;

        private float _cooldown;
        private bool _wasGrounded;

        private void Update()
        {
            bool isGrounded = Entity.IsGrounded();

            if (!isGrounded && _wasGrounded)
            {
                // just left ground
                _cooldown = dropDuration;
            }

            if (!isGrounded && _cooldown <= 0)
            {
                Entity.Move(Entity.Position + Vector3Int.down);
                Entity.TargetViewPosition = Entity.Position;
                Entity.TargetViewRotation = Quaternion.identity;
                _cooldown = dropDuration;
            }
            else
            {
                _cooldown = Mathf.Max(0, _cooldown - Time.deltaTime);
            }

            _wasGrounded = isGrounded;
        }
    }
}
