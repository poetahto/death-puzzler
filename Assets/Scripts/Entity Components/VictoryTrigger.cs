using UnityEngine;

namespace DefaultNamespace
{
    public class VictoryTrigger : PuzzleTrigger
    {
        protected override void OnPuzzleTriggerEnter(Entity entity, Vector3Int from)
        {
            FindAnyObjectByType<GameplayController>().TransitionToVictory();
        }
    }
}
