using System;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace
{
    public class PuzzleWorldEntity : MonoBehaviour
    {
        [SerializeField] private Transform view;
        [SerializeField] private Transform logic;
        [SerializeField] private float speed = 15f;

        public Vector3Int Position
        {
            get => Vector3Int.CeilToInt(logic.position);
            set => logic.position = value;
        }

        public PuzzleWorldGrid World { get; private set; }

        public Vector3 TargetViewPosition { get; set; }
        public Quaternion TargetViewRotation { get; set; }

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

        public virtual void PuzzleCreate(PuzzleWorldGrid world)
        {
            World = world;
        }

        public virtual void PuzzleDestroy()
        {
            World = null;
        }
    }

    public static class PuzzleExtensions
    {
        public static PuzzleWorldEntity GetOffset(this PuzzleWorldEntity entity, Vector3Int offset)
            => entity.World.Get(entity.Position + offset);

        public static PuzzleWorldEntity GetFront(this PuzzleWorldEntity entity) => entity.GetOffset(Vector3Int.forward);
        public static PuzzleWorldEntity GetBack(this PuzzleWorldEntity entity) => entity.GetOffset(Vector3Int.back);
        public static PuzzleWorldEntity GetAbove(this PuzzleWorldEntity entity) => entity.GetOffset(Vector3Int.up);
        public static PuzzleWorldEntity GetBelow(this PuzzleWorldEntity entity) => entity.GetOffset(Vector3Int.down);
        public static PuzzleWorldEntity GetLeft(this PuzzleWorldEntity entity) => entity.GetOffset(Vector3Int.left);
        public static PuzzleWorldEntity GetRight(this PuzzleWorldEntity entity) => entity.GetOffset(Vector3Int.right);

        public static void Move(this PuzzleWorldEntity entity, Vector3Int position) => entity.World.Move(entity, position);

        public static bool IsGrounded(this PuzzleWorldEntity entity)
        {
            var below = entity.GetBelow();
            return below.TryGetComponent(out Ground _) || below.TryGetComponent(out Stairs _);
        }

        public static bool IsTraversable(this PuzzleWorldEntity entity)
        {
            return entity.TryGetComponent(out Traversable _);
        }

        public static void Slide(this PuzzleWorldEntity entity, Vector3Int offset)
        {
            PuzzleWorldEntity targetEntity = entity.GetOffset(offset);

            // Stepping up onto stairs.
            if (targetEntity.TryGetComponent(out Stairs stairs))
            {
                if (stairs.CanEntityEnter(entity) && targetEntity.GetAbove().IsTraversable())
                {
                    entity.Move(targetEntity.Position + Vector3Int.up);
                }
            }
            else if (targetEntity.GetBelow().TryGetComponent(out stairs))
            {
                if (stairs.CanEntityEnter(entity) && targetEntity.IsTraversable())
                {
                    entity.Move(targetEntity.Position);
                }
            }
            else if (targetEntity.IsTraversable())
            {
                // We are standing on stairs, walking off onto an open space.
                if (entity.GetBelow().TryGetComponent(out stairs))
                {
                    if (stairs.CanEntityEnter(targetEntity.GetBelow()))
                    {
                        entity.Move(targetEntity.Position + Vector3Int.down);
                    }
                    else if (stairs.CanEntityEnter(targetEntity))
                    {
                        entity.Move(targetEntity.Position);
                    }
                }
                else
                {
                    entity.Move(targetEntity.Position);
                }
            }

            // No matter how we slide, always update our view transform to look correct.
            if (entity.GetBelow().TryGetComponent(out CustomSlideTransform slideTransform))
            {
                entity.TargetViewPosition = slideTransform.Position;
                entity.TargetViewRotation = slideTransform.Rotation;
            }
            else
            {
                entity.TargetViewPosition = entity.Position;
                entity.TargetViewRotation = Quaternion.identity;
            }
        }
    }
}
