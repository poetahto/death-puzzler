namespace DefaultNamespace
{
    public class Pushable : EntityBehaviour
    {
    }

    public static class PushableExtensions
    {
        public static bool IsPushable(this Entity entity)
        {
            return entity.TryGetComponent(out Pushable _) && entity.IsGrounded();
        }
    }
}
