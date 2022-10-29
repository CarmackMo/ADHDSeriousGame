using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GamePanel : Singleton<GamePanel>
{
    public Button fishingBtn;
    public Button pauseBtn;

    public GameObject initCountDown;
    public GameObject gameCountDown;

    public TextMeshProUGUI fishNumText;
    public TextMeshProUGUI initTimerText;
    public TextMeshProUGUI gameTimerText;

    protected override void Awake()
    {
        base.Awake();

        fishingBtn.onClick.AddListener(OnClickFishingBtn);
        pauseBtn.onClick.AddListener(OnClickPauseBtn);
    }

    protected override void Start()
    {
        base.Start();

        Inite();
    }

    public void Inite()
    {
        initCountDown.SetActive(true);
        gameCountDown.SetActive(false);
    }

    public void OnClickFishingBtn()
    {
        Player.Instance.CatchFish();
    }

    public void OnClickPauseBtn()
    {
        GameController.Instance.PauseGame();
        MenuPanel.Instance.Show();
    }

    public void UpdateFishNumText()
    {
        fishNumText.text = $"{Player.Instance.fishCatchNum}";
    }

    public void UpdateInitTimerText(float second)
    {
        initTimerText.text = $"{(int)second}";
    }

    public void UpdateGameTimer(float second)
    {
        string text = "";
        float sec = second % 60;
        float min = second / 60;

        if (min < 10)
            text += $"0{(int)min}";
        else
            text += $"{(int)min}";

        if (sec < 10)
            text += $":0{(int)sec}";
        else
            text += $":{(int)sec}";

        gameTimerText.text = text;
    }
}
