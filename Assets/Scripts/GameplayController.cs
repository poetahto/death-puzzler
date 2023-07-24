using System;
using UnityEngine;

namespace DefaultNamespace
{
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
        }
    }

    [Serializable]
    public class DefeatState : State
    {
        [SerializeField] private GameObject defeatUI;

        public override void OnEnter()
        {
            defeatUI.SetActive(true);
        }
    }

    [Serializable]
    public class PlayingState : State
    {
        [SerializeField] private InputController controller;

        public override void OnEnter()
        {
            controller.enabled = true;
        }

        public override void OnExit()
        {
            controller.enabled = false;
        }
    }


    public class GameplayController : MonoBehaviour
    {
        public VictoryState victoryState;
        public DefeatState defeatState;
        public PlayingState playingState;

        private State _currentState;

        private void Start()
        {
            TransitionTo(playingState);
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
    }
}
