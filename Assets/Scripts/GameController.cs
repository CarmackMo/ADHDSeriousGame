using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
    public float gameTime = 0f;
    public float initTime = 0f;

    protected override void Awake()
    {
        base.Awake();

        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    protected override void Start()
    {
        base.Start();

        PauseGame();
        MenuPanel.Instance.Show();
    }

    protected override void Update()
    {
        base.Update();

        RotateSky();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void RotateSky()
    {
        float num = Camera.main.GetComponent<Skybox>().material.GetFloat("_Rotation");
        Camera.main.GetComponent<Skybox>().material.SetFloat("_Rotation", num + 0.006f);
    }

    public void StartInitCountDownRoutine()
    {
        StartCoroutine(InitCountDownCoroutine());
    }

    IEnumerator InitCountDownCoroutine()
    {
        float timer = initTime;

        while (timer > 0)
        {
            timer -= 1;
            GamePanel.Instance.UpdateInitTimerText(timer);
            yield return new WaitForSecondsRealtime(1f);
        }

        GamePanel.Instance.initCountDown.SetActive(false);
        GamePanel.Instance.gameCountDown.SetActive(true);
        StartGame();
        StartCoroutine(GameCountdDownCoroutine());
        yield break;
    }

    IEnumerator GameCountdDownCoroutine()
    {
        float timer = gameTime;

        while(timer > 0)
        {
            GamePanel.Instance.UpdateGameTimer(timer);
            timer -= Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        PauseGame();
        ResultPanel.Instance.UpdateGameResult(
            Player.Instance.FishCatchNum, Player.Instance.FishCatchTime, Player.Instance.SharkHitNum);
        ResultPanel.Instance.Show();
        yield break;
    }
}
