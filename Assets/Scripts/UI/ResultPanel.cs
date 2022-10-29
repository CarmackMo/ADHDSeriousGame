using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : Singleton<ResultPanel>
{

    public Button restartBtn;

    protected override void Awake()
    {
        base.Awake();

        restartBtn.onClick.AddListener(OnClickRestartBtn);
    }

    protected override void Start()
    {
        base.Start();

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnClickRestartBtn()
    {
        GameController.Instance.RestartGame();
    }

}
