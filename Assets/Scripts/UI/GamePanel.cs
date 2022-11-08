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
    public GameObject transGestureArea;
    public GameObject pressGestureArea;

    public Image rhythmImage;
    public Image progressImage;

    public RectTransform midBound;

    public TextMeshProUGUI fishNumText;
    public TextMeshProUGUI initTimerText;
    public TextMeshProUGUI gameTimerText;

    public ScreenTransformGesture transformGesture;
    public ReleaseGesture releaseGesture;


    private bool isPressing = false;     
    private Vector2 originScreenPos = Vector2.zero;

    public bool IsPressing { get { return isPressing; } set { isPressing = value;} }




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

        //OnPressFishingBtn();  
        OnPressing();
    }

    protected void OnEnable()
    {
        transformGesture.Transformed += TransformGestureHandler;
        transformGesture.TransformStarted += TransformStartHandler;
        releaseGesture.Released += ReleaseHandler;
    }

    protected void OnDisable()
    {
        transformGesture.Transformed -= TransformGestureHandler;
        transformGesture.TransformStarted -= TransformStartHandler;
        releaseGesture.Released -= ReleaseHandler;
    }

    public void Init()
    {
        //initCountDown.SetActive(false);
        gameCountDown.SetActive(false);
    }

    public void OnClickFishingBtn()
    {
        if (Player.Instance.PlayerState != Player.State.CATCHING)
            Player.Instance.HookFish();
    }

    public void OnPressFishingBtn()
    {
        if (IsPressing && Player.Instance.PlayerState == Player.State.CATCHING)
            Player.Instance.AddRhythm();
    }


    private void OnPressing()
    {
        if (IsPressing && Player.Instance.PlayerState == Player.State.CATCHING)
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

    public void UpdateCatchingPanelVisibility(bool visible)
    {
        catchingPanel.gameObject.SetActive(visible);
    }

    public void UpdateTouchAreaVisibility(bool canPress)
    {
        isPressing = false;
        pressGestureArea.SetActive(canPress);
        transGestureArea.SetActive(!canPress);
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

        Vector2 sizeDelta = midBound.sizeDelta;
        sizeDelta.x = targetAmount / 100 * rhythmImage.GetComponent<RectTransform>().sizeDelta.x;
        midBound.sizeDelta = sizeDelta;

        //layoutGroup.spacing = targetAmount / 100 * rhythmImage.GetComponent<RectTransform>().sizeDelta.x;
    }

    private void ReleaseHandler(object sender, EventArgs e)
    {
        Player.Instance.UpdateAimingVisiability(false);
        Player.Instance.UpdateAimingDirection();
        Player.Instance.TryHookFish();
    }

    private void TransformGestureHandler(object sender, EventArgs e)
    {        
        Vector2 currPos = transformGesture.ScreenPosition;
        Vector2 delta = currPos - originScreenPos;
        float distance = Vector2.Distance(currPos, originScreenPos);
        float cos = delta.y / distance;
        float radAngle = Mathf.Acos(cos);
        float degAngle = radAngle * Mathf.Rad2Deg;
        degAngle = degAngle < 90 ? degAngle : 90; 
        degAngle = degAngle * (delta.x < 0 ? 1 : -1);

        Player.Instance.AimingVec = delta.normalized;
        Player.Instance.UpdateAimingDirection(degAngle);
        


        //Debug.Log($"origin: {originScreenPos}, currPos: {currPos}, delta: {delta}");
        //Debug.Log($"distance: {distance}, cos: {cos}, angle: {degAngle}");
        //Debug.Log("===============================================================");
    }

    private void TransformStartHandler(object sender, EventArgs e)
    {
        originScreenPos = transformGesture.ScreenPosition;
        Player.Instance.UpdateAimingVisiability(true);
        Player.Instance.UpdatePlayerState(Player.State.AIMING);
    }


}
