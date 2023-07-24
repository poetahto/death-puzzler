using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public class GameplayStateMachine : MonoBehaviour
    {
        public VictoryState victoryState;
        public DefeatState defeatState;
        public PlayingState playingState;
        public HatchingState hatchingState;

        private State _currentState;

        private void Start()
        {
            if (FindObjectsByType<Egg>(FindObjectsSortMode.None).Length > 0)
                TransitionToHatching();

            else TransitionToPlaying();
        }

        public void TransitionTo(State state)
        {
            _currentState?.OnExit();
            _currentState = state;
            _currentState?.OnEnter();
        }

        public void TransitionToDefeat() => TransitionTo(defeatState);
        public void TransitionToVictory() => TransitionTo(victoryState);
        public void TransitionToPlaying() => TransitionTo(playingState);
        public void TransitionToHatching() => TransitionTo(hatchingState);

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public abstract class State
    {
        public virtual void OnEnter() {}
        public virtual void OnExit() {}
    }

    [Serializable]
    public class VictoryState : State
    {
        [SerializeField] private GameObject victoryUI;

        public override void OnEnter()
        {
            victoryUI.SetActive(true);
            Object.FindAnyObjectByType<PlayerInput>().SwitchCurrentActionMap("None"); // disable controls
        }

        public override void OnExit()
        {
            victoryUI.SetActive(false);
        }
    }

    [Serializable]
    public class DefeatState : State
    {
        [SerializeField] private GameObject defeatUI;

        public override void OnEnter()
        {
            defeatUI.SetActive(true);
            Object.FindAnyObjectByType<PlayerInput>().SwitchCurrentActionMap("None"); // disable controls
        }

        public override void OnExit()
        {
            defeatUI.SetActive(false);
        }
    }

    [Serializable]
    public class HatchingState : State
    {
        [SerializeField] private string hatchingActionMap;
        [SerializeField] private GameObject hatchingUI;

        public override void OnEnter()
        {
            hatchingUI.SetActive(true);
            Object.FindAnyObjectByType<PlayerInput>().SwitchCurrentActionMap(hatchingActionMap);
        }

        public override void OnExit()
        {
            hatchingUI.SetActive(false);
        }
    }

    [Serializable]
    public class PlayingState : State
    {
        [SerializeField] private string playingActionMap;

        public override void OnEnter()
        {
            Object.FindAnyObjectByType<PlayerInput>().SwitchCurrentActionMap(playingActionMap);
        }
    }
}
