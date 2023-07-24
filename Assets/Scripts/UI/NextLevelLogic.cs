using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.UI
{
    public class NextLevelLogic : MonoBehaviour
    {
        public void Run()
        {
            // todo: transitions
            var levelData = FindAnyObjectByType<LevelData>();
            SceneManager.LoadScene(levelData.nextScene);
        }
    }
}
