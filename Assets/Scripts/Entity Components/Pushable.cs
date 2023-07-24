using UnityEngine;

namespace DefaultNamespace
{
    public class Pushable : EntityBehaviour
    {
        public bool CanPush(Vector3Int direction)
        {
            Vector3Int current = Entity.Position + direction;

            while (World.InBounds(current))
            {
                Entity currentEntity = World.Get(current);

                if (currentEntity.IsTraversable())
                    return true;

                current += direction;
            }

            return false;
        }

        public bool TryPush(Vector3Int direction)
        {
            if (CanPush(direction))
            {
                Entity currentEntity = Entity;

                while (currentEntity.TryGetComponent(out Pushable _))
                {
                    Entity next = currentEntity.GetNeighbor(direction);
                    currentEntity.Move(currentEntity.Position + direction);

                    if (currentEntity.GetBelow().TryGetComponent(out CustomSlideTransform slideTransform))
                    {
                        currentEntity.TargetViewPosition = slideTransform.Position;
                        currentEntity.TargetViewRotation = slideTransform.Rotation;
                    }
                    else
                    {
                        currentEntity.TargetViewPosition = currentEntity.Position;
                        currentEntity.TargetViewRotation = Quaternion.identity;
                    }

                    currentEntity = next;
                }

                return true;
            }

            return false;
        }
    }
}
