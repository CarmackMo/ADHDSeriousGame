using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;


public class GamePanel : Singleton<GamePanel>
{
    public Button fishingBtn;
    public Button pauseBtn;

    public GameObject initCountDown;
    public GameObject gameCountDown;
    public GameObject catchingPanel;
    public GameObject targetArea;

    public Image rhythmImage;
    public Image progressImage;

    public TextMeshProUGUI fishNumText;
    public TextMeshProUGUI initTimerText;
    public TextMeshProUGUI gameTimerText;

    private bool isPressFishBtn = false;

    public bool IsPressFishBtn { get { return isPressFishBtn; } set { isPressFishBtn = value;} }

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

    protected override void Update()
    {
        base.Update();

        OnPressFishingBtn();
    }

    public void Inite()
    {
        initCountDown.SetActive(true);
        gameCountDown.SetActive(false);
    }

    public void OnClickFishingBtn()
    {
        if (!Player.Instance.IsCatching)
            Player.Instance.HookFish();
    }

    public void OnPressFishingBtn()
    {
        if (isPressFishBtn && Player.Instance.IsCatching)
            Player.Instance.AddRhythm();
    }

    public void OnClickPauseBtn()
    {
        GameController.Instance.PauseGame();
        MenuPanel.Instance.Show();
    }


    public void UpdateFishNumText(int fishCatchNum)
    {
        fishNumText.text = $"{fishCatchNum}";
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

    public void UpdateCatchingPanel(bool visibility)
    {
        catchingPanel.gameObject.SetActive(visibility);
    }

    public void UpdateCatchingRhythm(float rhythmAmount, float threshold)
    {
        rhythmImage.fillAmount = rhythmAmount / threshold;
    }

    public void UpdateCatchingProgress(float progressAmount, float threshold)
    {
        progressImage.fillAmount = progressAmount / threshold;
    }

    public void UpdateTargetArea(float targetStartPos, float targetAmount)
    {
        HorizontalLayoutGroup layoutGroup = targetArea.GetComponent<HorizontalLayoutGroup>();
        RectTransform rect = targetArea.GetComponent<RectTransform>();
        Vector3 pos = rect.position;
        pos.x += targetStartPos / 100 * rhythmImage.GetComponent<RectTransform>().sizeDelta.x;
        rect.position = pos;
        layoutGroup.spacing = targetAmount / 100 * rhythmImage.GetComponent<RectTransform>().sizeDelta.x;
    }

}
