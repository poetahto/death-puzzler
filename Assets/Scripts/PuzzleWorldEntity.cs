using UnityEngine;

namespace DefaultNamespace
{
    public class PuzzleWorldEntity : MonoBehaviour
    {
        public Vector3Int Position
        {
            get => Vector3Int.CeilToInt(transform.position);
            set => transform.position = value;
        }

        public PuzzleWorldGrid World { get; private set; }

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
        public static bool IsWalkable(this PuzzleWorldEntity entity)
        {
            return entity.GetComponent<Traversable>() && entity.IsGrounded(out _);
        }

        public static bool IsGrounded(this PuzzleWorldEntity entity, out Ground ground)
        {
            return entity.World.Get(entity.Position + Vector3Int.down).TryGetComponent(out ground);
        }

        public static void Slide(this PuzzleWorldEntity entity, Vector3Int offset)
        {
            PuzzleWorldGrid world = entity.World;
            Vector3Int newPosition = entity.Position + offset;

            if (world.Get(newPosition).IsWalkable())
            {
                world.Move(entity, newPosition);
            }
        }
    }
}
