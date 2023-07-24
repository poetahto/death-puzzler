using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.UI
{
    public class RestartLevelLogic : MonoBehaviour
    {
        public void Run()
        {
            // todo: transitions
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
