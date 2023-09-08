using UnityEngine;

namespace DefaultNamespace.UI
{
    public class RestartLevelLogic : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
                FindAnyObjectByType<GameplayStateMachine>().Restart();
        }
    }
}
