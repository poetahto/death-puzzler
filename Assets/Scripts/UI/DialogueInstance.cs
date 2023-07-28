using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class DialogueInstance : MonoBehaviour
    {
        [SerializeField] private float characterPause;
        [SerializeField] private float linePause;
        [SerializeField] private float finishPause;
        [SerializeField] private CanvasGroup dialogueUI;
        [SerializeField] private float fadeInTime;
        [SerializeField] private TMP_Text textDisplay;
        public string[] lines;

        public Coroutine Play(Action onComplete = null)
        {
            return StartCoroutine(PlayCoroutine(onComplete));
        }

        private IEnumerator PlayCoroutine(Action onComplete = null)
        {
            InputUtil.PushActionMap("None");
            dialogueUI.gameObject.SetActive(true);
            string displayedText = string.Empty;

            foreach (string line in lines)
            {
                foreach (char character in line)
                {
                    displayedText += character;
                    textDisplay.SetText(displayedText);
                    yield return new WaitForSeconds(characterPause);
                }

                yield return new WaitForSeconds(linePause);
                displayedText += '\n';
                textDisplay.SetText(displayedText);
            }

            yield return new WaitForSeconds(finishPause);

            float elapsed = 0;
            float start = dialogueUI.alpha;

            while (elapsed < fadeInTime)
            {
                dialogueUI.alpha = Mathf.Lerp(start, 0, elapsed / fadeInTime);
                elapsed += Time.deltaTime;
                yield return null;
            }

            dialogueUI.gameObject.SetActive(false);
            InputUtil.PopActionMap();
            onComplete?.Invoke();
        }
    }
}
