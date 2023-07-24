﻿using UnityEngine;

namespace DefaultNamespace
{
    public abstract class PuzzleTrigger : MonoBehaviour
    {
        [SerializeField] private bool oneShot;

        private Vector3Int _position;

        private void Awake()
        {
            _position = Vector3Int.RoundToInt(transform.position);
        }

        private void OnEnable()
        {
            var container = FindAnyObjectByType<PuzzleWorldGridContainer>();

            if (container != null)
            {
                container.PuzzleWorld.OnMove += HandleOnMove;
            }
        }

        private void OnDisable()
        {
            var container = FindAnyObjectByType<PuzzleWorldGridContainer>();

            if (container != null)
            {
                container.PuzzleWorld.OnMove -= HandleOnMove;
            }
        }

        private void HandleOnMove(PuzzleWorldGrid.MoveEvent moveEvent)
        {
            if (moveEvent.to == _position)
            {
                OnPuzzleTriggerEnter(moveEvent.entity, moveEvent.from);

                if (oneShot)
                    enabled = false;
            }
        }

        protected abstract void OnPuzzleTriggerEnter(Entity entity, Vector3Int from);
    }
}