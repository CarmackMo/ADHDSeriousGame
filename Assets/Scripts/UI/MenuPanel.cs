using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MenuPanel : Singleton<MenuPanel>
{
    public Button startBtn;
    public Button infoBtn;
    public Button exitBtn;
    public Button restartBtn;
    public Button resumeBtn;


    protected override void Awake()
    {
        base.Awake();

        startBtn.onClick.AddListener(OnClickStartBtn);
        infoBtn.onClick.AddListener(OnClickInfoBtn);
        exitBtn.onClick.AddListener(OnClickExitBtn);
        restartBtn.onClick.AddListener(OnClickRestartBtn);
        resumeBtn.onClick.AddListener(OnClickResumeBtn);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnClickStartBtn()
    {
        GameController.Instance.StartInitCountDownRoutine();
        startBtn.gameObject.SetActive(false);
        restartBtn.gameObject.SetActive(true);
        resumeBtn.gameObject.SetActive(true);
        Hide();
    }

    public void OnClickInfoBtn()
    {

    }

    public void OnClickExitBtn()
    {
        GameController.Instance.ExitGame();
    }

    public void OnClickRestartBtn()
    {
        GameController.Instance.RestartGame();
    }

    public void OnClickResumeBtn()
    {
        Hide();
        GameController.Instance.StartGame();
    }

}
