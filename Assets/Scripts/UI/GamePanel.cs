using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;


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

    public ScreenTransformGesture transformGesture;
    public LongPressGesture longPressGesture;
    public ReleaseGesture releaseGesture;


    private bool isPressFishBtn = false;     

    private Vector2 originScreenPos = Vector2.zero;


    

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

        Init();
    }

    protected override void Update()
    {
        base.Update();

        OnPressFishingBtn();
    }

    protected void OnEnable()
    {
        transformGesture.Transformed += TransformGestureHandler;
        transformGesture.TransformStarted += TransformStartHandler;
        longPressGesture.StateChanged += LongPressedHandler;
        releaseGesture.Released += ReleaseHandler;
    }

    protected void OnDisable()
    {
        transformGesture.Transformed -= TransformGestureHandler;
        longPressGesture.StateChanged -= LongPressedHandler;
        releaseGesture.Released -= ReleaseHandler;
    }

    public void Init()
    {
        initCountDown.SetActive(true);
        gameCountDown.SetActive(false);
    }

    public void OnClickFishingBtn()
    {
        if (Player.Instance.PlayerState != Player.State.CATCHING)
            Player.Instance.HookFish();
    }

    public void OnPressFishingBtn()
    {
        if (isPressFishBtn && Player.Instance.PlayerState == Player.State.CATCHING)
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

    private void LongPressedHandler(object sender, GestureStateChangeEventArgs e)
    {
        Debug.Log("Detect long press!!!!!!!!!!");
        if (e.State == Gesture.GestureState.Recognized)
        {
            Player.Instance.UpdateCanvasVisiability(true);
            Player.Instance.UpdatePlayerState(Player.State.AIMING);
        }
        else if (e.State == Gesture.GestureState.Failed)
        {
            Player.Instance.UpdateCanvasVisiability(false);
        }
        else if (e.State == Gesture.GestureState.Ended)
        {
            Player.Instance.UpdateCanvasVisiability(false);
        }
    }

    private void ReleaseHandler(object sender, EventArgs e)
    {
        Player.Instance.UpdateCanvasVisiability(false);
    }

    private void TransformGestureHandler(object sender, EventArgs e)
    {        
        Vector2 currPos = transformGesture.ScreenPosition;
        Vector2 delta = currPos - originScreenPos;


        float distance = Vector2.Distance(currPos, originScreenPos);
        float cos = delta.y / distance;
        float angle = Mathf.Acos(cos);

        Debug.Log($"origin: {originScreenPos}, currPos: {currPos}, delta: {delta}");
        Debug.Log($"distance: {distance}, cos: {cos}, angle: {Mathf.Rad2Deg * angle}");
        Debug.Log("===============================================================");
    }

    private void TransformStartHandler(object sender, EventArgs e)
    {
        Debug.Log("Detect transform@@@@@@@@@@@");
        originScreenPos = transformGesture.ScreenPosition;
        Player.Instance.UpdateCanvasVisiability(true);
        Player.Instance.UpdatePlayerState(Player.State.AIMING);
    }


}
