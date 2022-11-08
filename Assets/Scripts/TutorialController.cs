using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : Singleton<TutorialController>
{
    public float controlThold = 0.4f;



    public enum State { IDLE, CONTROL_INTRO, CONTROL_ONGO, SHARK_INTRO, SHARK_ONGO, CATCH_INTRO, CATCH_ONGO, COMPLETE}

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
        TutorialPanel.Instance.Hide();
        GameController.Instance.PauseGame();
        GameController.Instance.StartInitCountDownRoutine();
    }

    public void UpdateTutorialState(State state = State.IDLE)
    {
        tutorialState = state;
    }

    IEnumerator ControlIntroCoroutine()
    {
        GameController.Instance.PauseGame();
        TutorialPanel.Instance.UpdatePanelVisibility(TutorialPanel.PanelType.CONTROL, true);
        TutorialPanel.Instance.UpdateMaskCallBack(TutorialPanel.PanelType.CONTROL, true, () => UpdateTutorialState(State.CONTROL_ONGO));
        while (tutorialState == State.CONTROL_INTRO)
        {
            yield return new WaitForEndOfFrame();
        }
        TutorialPanel.Instance.UpdatePanelVisibility(TutorialPanel.PanelType.CONTROL, false);
        TutorialPanel.Instance.UpdateMaskCallBack(TutorialPanel.PanelType.CONTROL, false, () => UpdateTutorialState(State.CONTROL_ONGO));
        GameController.Instance.StartGame();
        StartCoroutine(ControlCoroutine());
        yield break;
    }

    IEnumerator ControlCoroutine()
    {
        bool isMoveLeft = false;
        bool isMoveRight = false;

        while (isMoveLeft == false || isMoveRight == false)
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


        //yield return new WaitForSecondsRealtime(2f);

        UpdateTutorialState(State.SHARK_INTRO);
        StartCoroutine(SharkIntroCoroutine());
        yield break;
    }

    IEnumerator SharkIntroCoroutine()
    {
        Vector3 spawnPos = SharkGenerator.Instance.transform.position;
        Shark shark = SharkGenerator.Instance.SpawnSharkAtPos(spawnPos);

        while (shark.transform.position.z >= 10)
        {
            yield return new WaitForEndOfFrame();
        }

        GameController.Instance.PauseGame();
        TutorialPanel.Instance.UpdatePanelVisibility(TutorialPanel.PanelType.SHARK, true);
        TutorialPanel.Instance.UpdateMaskCallBack(TutorialPanel.PanelType.SHARK, true, () => UpdateTutorialState(State.SHARK_ONGO));
        while (tutorialState == State.SHARK_INTRO)
        {
            yield return new WaitForEndOfFrame();
        }
        TutorialPanel.Instance.UpdatePanelVisibility(TutorialPanel.PanelType.SHARK, false);
        TutorialPanel.Instance.UpdateMaskCallBack(TutorialPanel.PanelType.SHARK, false, () => UpdateTutorialState(State.SHARK_ONGO));
        GameController.Instance.StartGame();

        StartCoroutine(SharkOngoCoroutine());
        yield break;
    }

    IEnumerator SharkOngoCoroutine()
    {
        SharkGenerator.Instance.StartSharkSpawnCoroutine();
        yield return new WaitForSecondsRealtime(5f);
        SharkGenerator.Instance.StopSharkSpawnCoroutine();
        yield return new WaitForSecondsRealtime(5f);

        UpdateTutorialState(State.CATCH_INTRO);
        StartCoroutine(CatchIntroCoroutine());
        yield break;
    }

    IEnumerator CatchIntroCoroutine()
    {
        Vector3 spawnPos = SharkGenerator.Instance.transform.position;
        Fish fish = FishGenerator.Instance.SpawnFishAtPos(spawnPos);

        while (fish.transform.position.z >= 10)
        {
            yield return new WaitForEndOfFrame();
        }

        GameController.Instance.PauseGame();
        TutorialPanel.Instance.UpdatePanelVisibility(TutorialPanel.PanelType.CATCH_INTRO, true);
        TutorialPanel.Instance.UpdateMaskCallBack(TutorialPanel.PanelType.CATCH_INTRO, true, () => UpdateTutorialState(State.CATCH_ONGO));
        while (tutorialState == State.CATCH_INTRO)
        {
            yield return new WaitForEndOfFrame();
        }
        TutorialPanel.Instance.UpdatePanelVisibility(TutorialPanel.PanelType.CATCH_INTRO, false);
        TutorialPanel.Instance.UpdateMaskCallBack(TutorialPanel.PanelType.CATCH_INTRO, false, () => UpdateTutorialState(State.CATCH_ONGO));
        GameController.Instance.StartGame();


        UpdateTutorialState(State.COMPLETE);
        StartCoroutine(CompleteCoroutine());
        yield break;
    }

    IEnumerator CompleteCoroutine()
    {
        GameController.Instance.PauseGame();
        TutorialPanel.Instance.UpdatePanelVisibility(TutorialPanel.PanelType.COMPLETE, true);
        TutorialPanel.Instance.UpdateMaskCallBack(TutorialPanel.PanelType.COMPLETE, true, () => UpdateTutorialState(State.IDLE));
        while (tutorialState == State.COMPLETE)
        {
            yield return new WaitForEndOfFrame();
        }
        TutorialPanel.Instance.UpdatePanelVisibility(TutorialPanel.PanelType.COMPLETE, false);
        TutorialPanel.Instance.UpdateMaskCallBack(TutorialPanel.PanelType.COMPLETE, false, () => UpdateTutorialState(State.IDLE));
        CompleteTutorial();
        yield break;
    }
}
