using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            if (!FindAnyObjectByType<ControlledEntity>() &&FindAnyObjectByType<Egg>())
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
            InputUtil.PushActionMap("None");
        }

        public override void OnExit()
        {
            victoryUI.SetActive(false);
            InputUtil.PopActionMap();
        }
    }

    [Serializable]
    public class DefeatState : State
    {
        [SerializeField] private GameObject defeatUI;

        public override void OnEnter()
        {
            defeatUI.SetActive(true);
            InputUtil.PushActionMap("None");
        }

        public override void OnExit()
        {
            defeatUI.SetActive(false);
            InputUtil.PopActionMap();
        }
    }

    [Serializable]
    public class HatchingState : State
    {
        [SerializeField] private string hatchingActionMap;
        [SerializeField] private GameObject hatchingUI;
        [SerializeField] private HatchingController hatchingController;

        public override void OnEnter()
        {
            hatchingUI.SetActive(true);
            InputUtil.PushActionMap(hatchingActionMap);
            hatchingController.SelectedEgg.Select();
        }

        public override void OnExit()
        {
            hatchingUI.SetActive(false);
            InputUtil.PopActionMap();
        }
    }

    [Serializable]
    public class PlayingState : State
    {
        [SerializeField] private string playingActionMap;

        public override void OnEnter()
        {
            InputUtil.PushActionMap(playingActionMap);
        }

        public override void OnExit()
        {
            InputUtil.PopActionMap();
        }
    }
}
