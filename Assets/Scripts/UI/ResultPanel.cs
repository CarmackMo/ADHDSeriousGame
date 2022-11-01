using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultPanel : Singleton<ResultPanel>
{

    public Button restartBtn;

    public TextMeshProUGUI fishCatchText;
    public TextMeshProUGUI catchTimeText;
    public TextMeshProUGUI sharkHitText;

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

    public void UpdateGameResult(int fishCatchNum, float fishCatchTime, float sharkHitNum)
    {
        fishCatchText.text = $"{fishCatchNum}";
        sharkHitText.text = $"{sharkHitNum}";

        string text = "";
        float averageTime = fishCatchTime / fishCatchNum;
        float sec = averageTime % 60;
        float min = averageTime / 60;

        if (min < 10)
            text += $"0{(int)min}";
        else
            text += $"{(int)min}";

        if (sec < 10)
            text += $":0{(int)sec}";
        else
            text += $":{(int)sec}";

        catchTimeText.text = text;
    }

}
