using UnityEngine;

namespace DefaultNamespace
{
    public class ControlledEntity : EntityBehaviour
    {
        [SerializeField]
        private Transform facingTransform;

        [SerializeField]
        private Animator movementAnimator;

        [SerializeField] private float boostAmount = 0.1f;
        [SerializeField] private float boostSpeed = 5;
        [SerializeField] private float boostDecay = 1;

        private Entity _grabbedEntity;
        private Vector3 _baseScale;
        private float _boost;

        public Vector3Int Direction { get; private set; } = Vector3Int.forward;
        public bool IsGrabbing { get; private set; }

        private void Start()
        {
            Entity.onMove.AddListener(CheckGrounded);
            _baseScale = facingTransform.localScale;
        }

        private void CheckGrounded(Entity.MoveEvent eventData)
        {
            bool isGrounded = Entity.IsGrounded();
            movementAnimator.SetBool("isGrounded", isGrounded);

            if (IsGrabbing && !isGrounded)
                HandleDrop();
        }

        public bool test;

        private void Update()
        {
            var t = 15 * Time.deltaTime;
            Vector3Int dir;
            if (test)
            {
                dir = IsGrabbing ? -Direction : Vector3Int.forward;
            }
            else dir = -Direction;
            facingTransform.localRotation = Quaternion.Lerp(facingTransform.localRotation, Quaternion.LookRotation(dir), t);
            facingTransform.localScale = Vector3.Lerp(facingTransform.localScale, _baseScale * _boost, boostSpeed * Time.deltaTime);
            _boost  = Mathf.Max(_boost - boostDecay * Time.deltaTime, 1);
        }

        public void HandleMove(Vector3Int offset)
        {
            if (Entity.IsGrounded() && enabled)
            {
                if (IsGrabbing)
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
                    Direction = offset;
                    Entity.Slide(offset);
                }

                _boost = boostAmount;
            }
        }

        public void HandleGrab()
        {
            if (IsGrabbing)
                return;

            for (int i = 0; i < 4; i++)
            {
                Direction = Vector3Int.RoundToInt(Quaternion.Euler(0, 90, 0) * Direction);

                if (TryGrabNeighbor(Direction, out _grabbedEntity))
                {
                    IsGrabbing = true;
                    break;
                }
            }

            movementAnimator.SetBool("grabbing", IsGrabbing);
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
            IsGrabbing = false;
        }
    }
}
