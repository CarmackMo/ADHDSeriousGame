using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : Singleton<TutorialController>
{
    public float controlThold = 0.4f;



    public enum State { INIT, CONTROL, SHARK, COMPLETE}

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
        tutorialState = State.INIT;
        GameController.Instance.StartGame();
        StartCoroutine(ControlCoroutine());
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


        yield return new WaitForSecondsRealtime(3f);
        CompleteTutorial();
    }


}
