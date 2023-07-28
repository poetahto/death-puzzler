namespace DefaultNamespace
{
    public class VictoryInteractable : InteractableEffect
    {
        protected override void OnInteract(Interactable.InteractEvent eventData)
        {
            FindAnyObjectByType<GameplayStateMachine>().TransitionToVictory();
        }
    }
}
