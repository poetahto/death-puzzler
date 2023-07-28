using System.Collections;
using UnityEngine;

namespace UI
{
    public class LevelTitle : MonoBehaviour
    {
        [SerializeField] private float duration;
        [SerializeField] private CanvasGroup canvasGroup;

        public Coroutine Show() => StartCoroutine(SetAlphaCoroutine(1));
        public Coroutine Hide() => StartCoroutine(SetAlphaCoroutine(0));

        private IEnumerator SetAlphaCoroutine(float alpha)
        {
            float elapsed = 0;
            float initial = canvasGroup.alpha;

            while (elapsed < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(initial, alpha, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = alpha;
        }
    }
}
