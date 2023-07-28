using UnityEngine;

namespace VFX
{
    public class EyeballAnimator : MonoBehaviour
    {
        [SerializeField] private Transform[] eyeballs;
        [SerializeField] private float duration;
        [SerializeField] private float moveSpeed;

        private float _elapsed;
        private Quaternion _rotation;

        private void Update()
        {
            if (_elapsed > duration)
            {
                _elapsed = 0;
                _rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
            }
            else _elapsed += Time.deltaTime;

            foreach (Transform eyeball in eyeballs)
                eyeball.localRotation = Quaternion.Lerp(eyeball.localRotation, _rotation, moveSpeed * Time.deltaTime);
        }
    }
}
