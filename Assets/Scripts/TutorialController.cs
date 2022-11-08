using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : Singleton<TutorialController>
{
    public float controlThold = 0.4f;

    public enum State { 
        IDLE, 
        CONTROL_INTRO, CONTROL_ONGO, 
        SHARK_INTRO, SHARK_ONGO, 
        AIM_INTRO, AIM_ONGO, AIM_END,
        CATCH_INTRO,CATCH_ONGO, CATCH_END, CATCH_RETRY,
        COMPLETE}


    private State tutorialState;
    private Fish sampleFish;


    public State TutorialState { get { return tutorialState; } set { tutorialState = value; } }
    public Fish SampleFish { get { return sampleFish; } set { sampleFish = value; } }



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
        //StartCoroutine(ControlIntroCoroutine());


        /* Test */
        UpdateTutorialState(State.AIM_INTRO);
        StartCoroutine(AimIntroCoroutine());
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

        UpdateTutorialState(State.AIM_INTRO);
        StartCoroutine(AimIntroCoroutine());
        yield break;
    }

    IEnumerator AimIntroCoroutine()
    {
        Vector3 spawnPos = SharkGenerator.Instance.transform.position;
        sampleFish = FishGenerator.Instance.SpawnFishAtPos(spawnPos);
        sampleFish.Init();

        while (sampleFish.transform.position.z >= 10)
        {
            yield return new WaitForEndOfFrame();
        }

        sampleFish.isHooked = true;
        GameController.Instance.PauseGame();
        TutorialPanel.Instance.UpdatePanelVisibility(TutorialPanel.PanelType.AIM_INTRO, true);
        TutorialPanel.Instance.UpdateMaskCallBack(TutorialPanel.PanelType.AIM_INTRO, true, null);
        while (tutorialState == State.AIM_INTRO)
        {
            yield return new WaitForEndOfFrame();
        }
        GameController.Instance.StartGame();






        while (tutorialState == State.AIM_ONGO)
        {
            yield return new WaitForEndOfFrame();
        }
        GameController.Instance.PauseGame();
        TutorialPanel.Instance.UpdatePanelVisibility(TutorialPanel.PanelType.AIM_END, true);
        while (tutorialState == State.AIM_END)
        {
            yield return new WaitForEndOfFrame();
        }

        TutorialPanel.Instance.UpdatePanelVisibility(TutorialPanel.PanelType.AIM_END, false);
        UpdateTutorialState(State.CATCH_INTRO);
        StartCoroutine(CatchIntroCoroutine());
        yield break;
    }


    IEnumerator CatchIntroCoroutine()
    {
        GameController.Instance.PauseGame();
        TutorialPanel.Instance.UpdatePanelVisibility(TutorialPanel.PanelType.CATCH_INTRO, true);
        TutorialPanel.Instance.UpdateMaskCallBack(TutorialPanel.PanelType.CATCH_INTRO, true, null);
        while (tutorialState == State.CATCH_INTRO)
        {
            yield return new WaitForEndOfFrame();
        }

        TutorialPanel.Instance.UpdatePanelVisibility(TutorialPanel.PanelType.CATCH_PROGRESS, false);
        StartCoroutine(CatchOngoCoroutine());
        yield break;
    }


    IEnumerator CatchOngoCoroutine()
    {
        GameController.Instance.StartGame();
        while (tutorialState == State.CATCH_ONGO)
        {
            yield return new WaitForEndOfFrame();
        }


        GameController.Instance.PauseGame();
        TutorialPanel.Instance.UpdatePanelVisibility(TutorialPanel.PanelType.CATCH_RETRY, true);
        TutorialPanel.Instance.UpdateMaskCallBack(TutorialPanel.PanelType.CATCH_RETRY, true, () => UpdateTutorialState(State.CATCH_RETRY));
        while (tutorialState == State.CATCH_END)
        {
            yield return new WaitForEndOfFrame();
        }

        GameController.Instance.StartGame();
        TutorialPanel.Instance.UpdatePanelVisibility(TutorialPanel.PanelType.CATCH_RETRY, false);
        TutorialPanel.Instance.UpdateMaskCallBack(TutorialPanel.PanelType.CATCH_RETRY, false, () => UpdateTutorialState(State.CATCH_RETRY));

        StartCoroutine(CatchRetryCoroutine());
        yield break;
    }

    IEnumerator CatchRetryCoroutine()
    {
        Vector3 spawnPos = SharkGenerator.Instance.transform.position;
        sampleFish = FishGenerator.Instance.SpawnFishAtPos(spawnPos);
        sampleFish.Init();

        while (sampleFish.transform.position.z >= 10)
        {
            yield return new WaitForEndOfFrame();
        }

        sampleFish.isHooked = true;

        while (tutorialState == State.CATCH_RETRY)
        {
            yield return new WaitForEndOfFrame();
        }

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
