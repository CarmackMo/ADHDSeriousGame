using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
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
    }

    protected override void Update()
    {
        base.Update();

        RotateSky();
    }

    public void PauseGame()
    {
        MenuPanel.Instance.Show();
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
}
