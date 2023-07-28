using UI;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class InteractAllWinCondition : MonoBehaviour
    {
        public UnityEvent<Interactable.InteractEvent> onInteract;
        public UICounter counter;

        private int _remaining;

        private void Start()
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Win Condition"))
            {
                if (obj.TryGetComponent(out Interactable interactable))
                {
                    interactable.onInteract.AddListener(HandleInteract);
                    _remaining++;
                }
            }

            if (_remaining == 1)
                counter.gameObject.SetActive(false);

            counter.Max = _remaining;
        }

        private void HandleInteract(Interactable.InteractEvent eventData)
        {
            _remaining--;
            onInteract.Invoke(eventData);
            counter.Count++;

            if (_remaining <= 0)
                FindAnyObjectByType<GameplayStateMachine>().TransitionToVictory();
        }
    }
}
