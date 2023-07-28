using DefaultNamespace;
using UnityEngine;

namespace UI
{
    public class GrabHint : MonoBehaviour
    {
        [SerializeField] private ControlledEntity entity;
        [SerializeField] private Pushable pushable;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject target;
        [SerializeField] private Sprite up;
        [SerializeField] private Sprite down;
        [SerializeField] private Sprite left;
        [SerializeField] private Sprite right;

        private void Update()
        {
            target.SetActive(pushable.Entity.IsAdjacent(entity.Entity));

            if (entity.IsGrabbing)
            {
                TryUpdateSprite(Vector3Int.forward, down);
                TryUpdateSprite(Vector3Int.back, up);
                TryUpdateSprite(Vector3Int.left, right);
                TryUpdateSprite(Vector3Int.right, left);
            }

            else spriteRenderer.sprite = null;
        }

        private void TryUpdateSprite(Vector3Int direction, Sprite sprite)
        {
            if (entity.Direction == direction)
                spriteRenderer.sprite = sprite;
        }
    }
}
