using UnityEngine;

namespace DefaultNamespace
{
    public class PuzzleWorldEntity : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("A transform with smoothing applied, and will be moved every frame.")]
        private Transform view;

        [SerializeField]
        [Tooltip("A transform with no smoothing applied, and is always at a snapped position.")]
        private Transform logic;

        [SerializeField]
        [Tooltip("How quickly the view should move to follow the logic.")]
        private float speed = 15f;

        public Vector3Int Position
        {
            get => Vector3Int.CeilToInt(logic.position);
            set => logic.position = value;
        }

        public PuzzleWorldGrid World { get; private set; }
        public Vector3 TargetViewPosition { get; set; }
        public Quaternion TargetViewRotation { get; set; }

        // === Logic ===

        private void Awake()
        {
            TargetViewPosition = logic.position;
            TargetViewRotation = logic.rotation;
        }

        protected virtual void Update()
        {
            float t = speed * Time.deltaTime;
            view.position = Vector3.Lerp(view.position, TargetViewPosition, t);
            view.rotation = Quaternion.Lerp(view.rotation, TargetViewRotation, t);
        }

        public void PuzzleCreate(PuzzleWorldGrid world)
        {
            World = world;
        }

        public void PuzzleDestroy()
        {
            World = null;
        }

        // === Actions ===

        public void Move(Vector3Int position) => World.Move(this, position);

        public void Slide(Vector3Int offset)
        {
            PuzzleWorldEntity targetEntity = GetNeighbor(offset);

            // Stepping UP onto stairs.
            if (targetEntity.TryGetComponent(out Stairs stairs))
            {
                if (stairs.CanEntityEnter(this) && targetEntity.GetAbove().IsTraversable())
                {
                    Move(targetEntity.Position + Vector3Int.up);
                }
            }
            // Stepping DOWN onto stairs.
            else if (targetEntity.GetBelow().TryGetComponent(out stairs))
            {
                if (stairs.CanEntityEnter(this) && targetEntity.IsTraversable())
                {
                    Move(targetEntity.Position);
                }
            }
            else if (targetEntity.IsTraversable())
            {
                if (GetBelow().TryGetComponent(out stairs))
                {
                    // Stepping DOWN off of stairs.
                    if (stairs.CanEntityEnter(targetEntity.GetBelow()))
                    {
                        Move(targetEntity.Position + Vector3Int.down);
                    }
                    // Stepping UP off of stairs.
                    else if (stairs.CanEntityEnter(targetEntity))
                    {
                        Move(targetEntity.Position);
                    }
                }
                else
                {
                    // Normal movement.
                    Move(targetEntity.Position);
                }
            }

            // No matter how we slide, always update our view transform to look correct.
            if (GetBelow().TryGetComponent(out CustomSlideTransform slideTransform))
            {
                TargetViewPosition = slideTransform.Position;
                TargetViewRotation = slideTransform.Rotation;
            }
            else
            {
                TargetViewPosition = Position;
                TargetViewRotation = Quaternion.identity;
            }
        }

        // === Queries ===

        public PuzzleWorldEntity GetNeighbor(Vector3Int offset) => World.Get(Position + offset);
        public PuzzleWorldEntity GetFront() => GetNeighbor(Vector3Int.forward);
        public PuzzleWorldEntity GetBack() => GetNeighbor(Vector3Int.back);
        public PuzzleWorldEntity GetAbove() => GetNeighbor(Vector3Int.up);
        public PuzzleWorldEntity GetBelow() => GetNeighbor(Vector3Int.down);
        public PuzzleWorldEntity GetLeft() => GetNeighbor(Vector3Int.left);
        public PuzzleWorldEntity GetRight() => GetNeighbor(Vector3Int.right);

        public bool IsGrounded()
        {
            var below = GetBelow();
            return below.TryGetComponent(out Ground _) || below.TryGetComponent(out Stairs _);
        }

        public bool IsTraversable()
        {
            return TryGetComponent(out Traversable _);
        }
    }
}
