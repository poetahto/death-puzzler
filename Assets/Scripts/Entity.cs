using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public sealed class Entity : MonoBehaviour
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

        // === Events ===

        public UnityEvent<SlideEvent> onSlide;
        public UnityEvent<MoveEvent> onMove;

        // === State ===

        public Vector3Int Position
        {
            get => Vector3Int.CeilToInt(logic.position);
            set => logic.position = value;
        }

        public PuzzleWorldGrid World { get; private set; }
        public Vector3 TargetViewPosition { get; set; }
        public Quaternion TargetViewRotation { get; set; }
        private Vector3 _angles;

        // === Logic ===

        private void Awake()
        {
            TargetViewPosition = logic.position;
            TargetViewRotation = view.localRotation;
        }

        private void Start()
        {
            var world = FindAnyObjectByType<PuzzleWorldGridContainer>().PuzzleWorld;
            world?.Set(Position, this);
        }

        private void Update()
        {
            float t = speed * Time.deltaTime;
            view.position = Vector3.Lerp(view.position, TargetViewPosition, t);
            view.localRotation = Quaternion.Lerp(view.localRotation, TargetViewRotation, t);
        }

        public void PuzzleCreate(PuzzleWorldGrid world)
        {
            World = world;
        }

        public void PuzzleDestroy()
        {
            // World = null;
            enabled = false;
        }

        // === Actions ===

        public void Move(Vector3Int position)
        {
            World.Move(this, position);
        }

        // todo: this method is out-of-hand.
        // needs a cleaner way to define movement state
        // i wish you could make more complex movement per-entity, like boxes only sliding down ramps
        public bool Slide(Vector3Int offset)
        {
            if (!World.InBounds(Position + offset) || offset == Vector3Int.zero)
                return false;

            Entity targetEntity = GetNeighbor(offset);
            Entity oldAbove = GetAbove();
            Vector3Int previousPosition = Position;

            // Stepping UP onto stairs.
            if (targetEntity.TryGetComponent(out Stairs stairs) && stairs.CanEntityEnter(this))
            {
                var above = targetEntity.GetAbove();

                if (above.IsTraversable())
                    Move(targetEntity.Position + Vector3Int.up);

                else if (above.IsPushable() && above.Slide(offset))
                    Move(targetEntity.Position + Vector3Int.up);
            }
            // Stepping DOWN onto stairs.
            else if (targetEntity.GetBelow().TryGetComponent(out stairs))
            {
                if (stairs.CanEntityEnter(this))
                {
                    if (targetEntity.IsTraversable())
                        Move(Position + offset);

                    else if (targetEntity.IsPushable() && targetEntity.Slide(offset))
                        Move(Position + offset);
                }
            }
            else if (targetEntity.IsTraversable())
            {
                if (GetBelow().TryGetComponent(out stairs))
                {
                    // Stepping DOWN off of stairs.
                    if (stairs.CanEntityEnter(targetEntity.GetBelow()))
                    {
                        var below = targetEntity.GetBelow();

                        if (below.IsTraversable())
                            Move(targetEntity.Position + Vector3Int.down);

                        else if (below.IsPushable() && below.Slide(offset))
                            Move(targetEntity.Position + Vector3Int.down);
                    }
                    // Stepping UP off of stairs.
                    else if (stairs.CanEntityEnter(targetEntity))
                        Move(targetEntity.Position);
                }
                else Move(Position + offset);
            }

            else if (targetEntity.IsPushable() && targetEntity.Slide(offset))
                Move(Position + offset);

            else if (targetEntity.IsInteractable())
                targetEntity.TryInteract(this);

            // No matter how we slide, always update our view transform to look correct.
            UpdateView();

            if (Position != previousPosition)
            {
                onSlide.Invoke(new SlideEvent{From = previousPosition, To = Position, Entity = this});

                if (oldAbove.TryGetComponent(out Pushable _))
                    oldAbove.Slide(offset);

                return true;
            }

            return false;
        }

        public void UpdateView()
        {
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

        public Entity GetNeighbor(Vector3Int offset) => World.Get(Position + offset);
        public Entity GetFront() => GetNeighbor(Vector3Int.forward);
        public Entity GetBack() => GetNeighbor(Vector3Int.back);
        public Entity GetAbove() => GetNeighbor(Vector3Int.up);
        public Entity GetBelow() => GetNeighbor(Vector3Int.down);
        public Entity GetLeft() => GetNeighbor(Vector3Int.left);
        public Entity GetRight() => GetNeighbor(Vector3Int.right);

        public bool IsAdjacent(Entity other)
        {
            return (Position - other.Position).sqrMagnitude == 1;
        }

        public bool IsGrounded()
        {
            return GetBelow().TryGetComponent(out Ground _);
        }

        public bool IsTraversable()
        {
            return TryGetComponent(out Traversable _);
        }

        // === Structures ===
        public struct SlideEvent
        {
            public Entity Entity;
            public Vector3Int From;
            public Vector3Int To;
        }

        public struct MoveEvent
        {
            public Entity Entity;
            public Vector3Int From;
            public Vector3Int To;
        }

        // === Util ===
        [ContextMenu("Generate Transforms")]
        private void GenerateTransforms()
        {
            if (view == null)
            {
                view = new GameObject("View").transform;
                view.SetParent(transform, false);
            }

            if (logic == null)
            {
                logic = new GameObject("Logic").transform;
                logic.SetParent(transform, false);
            }
        }
    }
}
