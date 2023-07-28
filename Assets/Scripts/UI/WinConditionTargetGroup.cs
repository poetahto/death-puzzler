using Cinemachine;
using UnityEngine;

namespace UI
{
    public class WinConditionTargetGroup : MonoBehaviour
    {
        public CinemachineTargetGroup group;

        private void Start()
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Win Condition"))
                group.AddMember(obj.transform, 1, 1);
        }
    }
}
