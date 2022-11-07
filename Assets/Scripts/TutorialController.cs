using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : Singleton<TutorialController>
{
    public float controlThold = 0.4f;



    public enum State { INIT, CONTROL_INTRO, CONTROL, SHARK, COMPLETE}

    private State tutorialState;


    public State TutorialState { get { return tutorialState; } set { tutorialState = value; } }



    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

    }


    public void StartTutorial()
    {
        UpdateTutorialState(State.CONTROL_INTRO);
        GameController.Instance.StartGame();
        StartCoroutine(ControlIntroCoroutine());
    }

    public void CompleteTutorial()
    {
        tutorialState = State.COMPLETE;
        TutorialPanel.Instance.Hide();
        GameController.Instance.PauseGame();
        GameController.Instance.StartInitCountDownRoutine();
    }

    public void UpdateTutorialState(State state = State.INIT)
    {
        tutorialState = state;
    }

    IEnumerator ControlIntroCoroutine()
    {
        GameController.Instance.PauseGame();
        TutorialPanel.Instance.UpdateControlPanelVisibility(true);
        TutorialPanel.Instance.UpdateControlMaskCallBack(() => UpdateTutorialState(State.CONTROL), true);
        while (tutorialState == State.CONTROL_INTRO)
        {
            yield return new WaitForEndOfFrame();
        }
        TutorialPanel.Instance.UpdateControlPanelVisibility(false);
        GameController.Instance.StartGame();
        StartCoroutine(ControlCoroutine());
    }

    IEnumerator ControlCoroutine()
    {
        bool isMoveLeft = false;
        bool isMoveRight = false;

        while (isMoveLeft == false && isMoveRight == false)
        {
            float dirtX = 0f;

        #if UNITY_ANDROID
            dirtX = Input.acceleration.x;
        #else 
            dirtX = Input.GetAxis("Horizontal");
        #endif

            if (dirtX > controlThold)
                isMoveRight = true;
            else if (dirtX < -controlThold)
                isMoveLeft = true;

            yield return new WaitForEndOfFrame();
        }


        yield return new WaitForSecondsRealtime(2f);
        CompleteTutorial();
    }


}
