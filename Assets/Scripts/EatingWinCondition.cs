using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class InteractAllWinCondition : MonoBehaviour
    {
        [SerializeField] private Interactable[] interactables;

        public UnityEvent<Interactable.InteractEvent> onInteract;

        private int _remaining;

        private void Start()
        {
            _remaining = interactables.Length;

            foreach (Interactable interactable in interactables)
                interactable.onInteract.AddListener(HandleInteract);
        }

        private void HandleInteract(Interactable.InteractEvent eventData)
        {
            _remaining--;
            onInteract.Invoke(eventData);

            if (_remaining <= 0)
                FindAnyObjectByType<GameplayStateMachine>().TransitionToVictory();
        }
    }
}
