using System;
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

        public UnityEvent<SlideEventData> onSlide;

        // === State ===

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

        private void Start()
        {
            var world = FindAnyObjectByType<PuzzleWorldGridContainer>().PuzzleWorld;
            world?.Set(Position, this);
        }

        private void Update()
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
            // World = null;
            enabled = false;
        }

        // === Actions ===

        public void Move(Vector3Int position) => World.Move(this, position);

        public void Slide(Vector3Int offset)
        {
            Entity targetEntity = GetNeighbor(offset);
            Vector3Int previousPosition = Position;

            // Stepping UP onto stairs.
            if (targetEntity.TryGetComponent(out Stairs stairs) && stairs.CanEntityEnter(this) && targetEntity.GetAbove().IsTraversable())
            {
                Move(targetEntity.Position + Vector3Int.up);
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
            else if (targetEntity.TryGetComponent(out Pushable pushable) && pushable.TryPush(offset))
            {
                // Normal movement.
                Move(Position + offset);
            }

            if (Position != previousPosition)
            {
                onSlide.Invoke(new SlideEventData{From = previousPosition, To = Position});
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

        public Entity GetNeighbor(Vector3Int offset) => World.Get(Position + offset);
        public Entity GetFront() => GetNeighbor(Vector3Int.forward);
        public Entity GetBack() => GetNeighbor(Vector3Int.back);
        public Entity GetAbove() => GetNeighbor(Vector3Int.up);
        public Entity GetBelow() => GetNeighbor(Vector3Int.down);
        public Entity GetLeft() => GetNeighbor(Vector3Int.left);
        public Entity GetRight() => GetNeighbor(Vector3Int.right);

        public bool IsGrounded()
        {
            return GetBelow().TryGetComponent(out Ground _);
        }

        public bool IsTraversable()
        {
            return TryGetComponent(out Traversable _);
        }

        // === Structures ===
        public struct SlideEventData
        {
            public Vector3Int From;
            public Vector3Int To;
        }
    }
}
