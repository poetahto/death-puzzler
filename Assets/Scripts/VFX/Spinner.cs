using UnityEngine;

namespace Effects
{
    public class Spinner : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Vector3 axis = Vector3.right;

        private void Update()
        {
            transform.Rotate(axis, speed * Time.deltaTime);
        }
    }
}
