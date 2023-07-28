using DefaultNamespace;
using UnityEngine;

namespace UI
{
    public class GrabHint : MonoBehaviour
    {
        [SerializeField] private ControlledEntity entity;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite up;
        [SerializeField] private Sprite down;
        [SerializeField] private Sprite left;
        [SerializeField] private Sprite right;

        private void Update()
        {
            if (entity.IsGrabbing)
            {
                TryUpdateSprite(Vector3Int.up, up);
                TryUpdateSprite(Vector3Int.down, down);
                TryUpdateSprite(Vector3Int.left, left);
                TryUpdateSprite(Vector3Int.right, right);
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
