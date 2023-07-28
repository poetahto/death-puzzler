using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class ShrinkOnInteract : InteractableEffect
    {
        [SerializeField] private float duration = 1;

        protected override void OnInteract(Interactable.InteractEvent eventData)
        {
            StartCoroutine(ShrinkCoroutine());
        }

        private IEnumerator ShrinkCoroutine()
        {
            float elapsed = 0;
            Vector3 originalScale = transform.localScale;

            while (elapsed < duration)
            {
                transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localScale = Vector3.zero;
        }
    }
}
