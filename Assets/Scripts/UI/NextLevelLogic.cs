using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.UI
{
    public class NextLevelLogic : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                var levelData = FindAnyObjectByType<LevelData>();
                SceneManager.LoadScene(levelData.nextScene);
            }
        }
    }
}
