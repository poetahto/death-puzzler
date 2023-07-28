using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class Interactable : EntityBehaviour
    {
        [SerializeField]
        private Vector3Int[] interactPositions =
        {
            Vector3Int.forward,
            Vector3Int.back,
            Vector3Int.left,
            Vector3Int.right,
        };

        [SerializeField]
        private Color gizmoColor = Color.red;

        [SerializeField]
        private bool oneShot;

        public UnityEvent<InteractEvent> onInteract = new UnityEvent<InteractEvent>();

        private bool _isUsed;

        public bool Interact(Entity user)
        {
            if (oneShot && _isUsed)
                return false;

            if (CanEntityInteract(user))
            {
                var eventData = new InteractEvent
                {
                    Interactable = Entity,
                    User = user,
                };

                _isUsed = true;
                onInteract.Invoke(eventData);
                return true;
            }

            return false;
        }

        // todo: this code overlaps a lot w/ stairs
        public bool CanEntityInteract(Entity user)
        {
            foreach (Vector3Int entrance in interactPositions)
            {
                var worldPos = transform.localToWorldMatrix.MultiplyPoint3x4(entrance);

                if (user.Position == worldPos)
                    return true;
            }

            return false;
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var entrance in interactPositions)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawCube(transform.localToWorldMatrix.MultiplyPoint3x4(entrance), Vector3.one);
            }
        }

        public struct InteractEvent
        {
            public Entity Interactable;
            public Entity User;
        }
    }

    public static class InteractableExtensions
    {
        public static bool IsInteractable(this Entity entity)
        {
            return entity.TryGetComponent(out Interactable _);
        }

        public static bool TryInteract(this Entity entity, Entity user)
        {
            if (entity.TryGetComponent(out Interactable interactable))
                return interactable.Interact(user);

            return false;
        }
    }
}
