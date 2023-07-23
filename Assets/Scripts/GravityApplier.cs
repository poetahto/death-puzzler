using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(PuzzleWorldEntity))]
    public class GravityApplier : MonoBehaviour
    {
        [SerializeField] private float dropDuration = 0.5f;

        private PuzzleWorldEntity _entity;
        private float _cooldown;
        private bool _wasGrounded;

        private void Start()
        {
            _entity = GetComponent<PuzzleWorldEntity>();
        }

        private void Update()
        {
            bool isGrounded = _entity.IsGrounded();

            if (!isGrounded && _wasGrounded)
            {
                // just left ground
                _cooldown = dropDuration;
            }

            if (!isGrounded && _cooldown <= 0)
            {
                _entity.Move(_entity.Position + Vector3Int.down);
                _entity.TargetViewPosition = _entity.Position;
                _entity.TargetViewRotation = Quaternion.identity;
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
