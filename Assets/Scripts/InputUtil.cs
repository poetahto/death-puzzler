using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public static class InputUtil
    {
        private static Stack<string> _actionMaps;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _actionMaps = new Stack<string>();
        }

        public static void PushActionMap(string mapName)
        {
            _actionMaps.Push(mapName);
            SetActionMap(mapName);
        }

        public static void PopActionMap()
        {
            _actionMaps.Pop();

            if (_actionMaps.TryPeek(out string newMap))
                SetActionMap(newMap);
        }

        private static void SetActionMap(string actionMap)
        {
            var input = Object.FindAnyObjectByType<PlayerInput>();
            input.SwitchCurrentActionMap(actionMap);
        }
    }
}
