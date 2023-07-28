using TMPro;
using UnityEngine;

namespace UI
{
    public class UICounter : MonoBehaviour
    {
        public TMP_Text text;
        public int max;
        public float scale;
        public float restoreSpeed = 1;

        private int _count;
        private float _scale = 1;
        private Vector3 _baseScale;

        private void Start()
        {
            _baseScale = transform.localScale;
        }

        private void Update()
        {
            float t = 15 * Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, _baseScale * _scale, t);
            _scale = Mathf.MoveTowards(_scale, 1, restoreSpeed * Time.deltaTime);
        }

        public void Increment()
        {
            _count++;
            _scale = scale;
            text.text = $"{_count}/{max}";
        }
    }
}
