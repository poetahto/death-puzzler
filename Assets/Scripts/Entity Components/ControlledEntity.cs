using UnityEngine;

namespace DefaultNamespace
{
    public class ControlledEntity : EntityBehaviour
    {
        public void HandleMove(Vector3Int offset)
        {
            if (Entity.IsGrounded() && enabled)
            {
                Entity.Slide(offset);
            }
        }
    }
}
