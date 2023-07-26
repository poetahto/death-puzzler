using System.Collections;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class DialogueInstance : MonoBehaviour
    {
        [SerializeField] private string[] lines;
        [SerializeField] private float characterPause;
        [SerializeField] private float linePause;
        [SerializeField] private GameObject dialogueUI;
        [SerializeField] private TMP_Text textDisplay;

        public void Play()
        {
            StartCoroutine(PlayCoroutine());
        }

        private IEnumerator PlayCoroutine()
        {
            InputUtil.PushActionMap("None");
            dialogueUI.SetActive(true);
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
            }

            dialogueUI.SetActive(false);
            InputUtil.PopActionMap();
        }
    }
}
