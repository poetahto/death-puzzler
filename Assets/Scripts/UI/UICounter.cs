using TMPro;
using UnityEngine;

namespace UI
{
    public class UICounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private int max;
        [SerializeField] private float scale;
        [SerializeField] private float restoreSpeed = 1;

        private int _count;
        private float _scale = 1;
        private Vector3 _baseScale;

        public int Max
        {
            get => max;
            set
            {
                max = value;
                UpdateText(false);
            }
        }

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                UpdateText(true);
            }
        }

        private void UpdateText(bool playAnimation)
        {
            if (playAnimation)
                _scale = scale;

            text.text = $"{_count}/{max}";
        }

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
    }
}
