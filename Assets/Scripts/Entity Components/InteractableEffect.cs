using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Interactable))]
    public abstract class InteractableEffect : MonoBehaviour
    {
        public Interactable Interactable { get; private set; }

        private void OnEnable()
        {
            Interactable = GetComponent<Interactable>();
            Interactable.onInteract.AddListener(OnInteract);
        }

        private void OnDisable()
        {
            Interactable.onInteract.RemoveListener(OnInteract);
        }

        protected abstract void OnInteract(Interactable.InteractEvent eventData);
    }
}