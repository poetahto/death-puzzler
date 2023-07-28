using System;
using System.Collections;
using Cinemachine;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GameplayStateMachine : MonoBehaviour
    {
        public IntroState introState;
        public PlayingState playingState;
        public HatchingState hatchingState;
        public VictoryState victoryState;
        public DefeatState defeatState;

        private State _currentState;

        private void Start()
        {
            introState.Fsm = this;
            playingState.Fsm = this;
            hatchingState.Fsm = this;
            victoryState.Fsm = this;
            defeatState.Fsm = this;

            TransitionTo(introState);
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
        public GameplayStateMachine Fsm { get; set; }

        public virtual void OnEnter() {}
        public virtual void OnExit() {}
    }

    [Serializable]
    public class IntroState : State
    {
        [SerializeField] private DialogueInstance introDialogue;
        [SerializeField] private CinemachineVirtualCamera introCamera;
        [SerializeField] private bool startHatching;
        [SerializeField] private LevelTitle title;

        public override void OnEnter()
        {
            Fsm.StartCoroutine(IntroCoroutine());
        }

        private IEnumerator IntroCoroutine()
        {
            introCamera.Priority = 100;
            yield return introDialogue.Play();
            yield return new WaitForSeconds(0.5f);
            yield return title.Show();
            yield return new WaitForSeconds(4);
            yield return title.Hide();
            introCamera.Priority = -1;
            yield return new WaitForSeconds(2);

            if (startHatching)
                Fsm.TransitionToHatching();

            else Fsm.TransitionToPlaying();
        }
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
