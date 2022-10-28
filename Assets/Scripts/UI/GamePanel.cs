using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GamePanel : Singleton<GamePanel>
{
    public Button fishingBtn;
    public Button pauseBtn;

    public GameObject countDown;

    public TextMeshProUGUI fishNumText;
    public TextMeshProUGUI timerText;

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
        countDown.SetActive(true);
    }

    public void OnClickFishingBtn()
    {
        Player.Instance.CatchFish();
    }

    public void OnClickPauseBtn()
    {
        GameController.Instance.PauseGame();
    }

    public void UpdateFishNumText()
    {
        fishNumText.text = $"{Player.Instance.fishCatchNum}";
    }

    public void StartCountDown()
    {
        StartCoroutine(GameStartCountDownCoroutine());
    }

    public IEnumerator GameStartCountDownCoroutine()
    {
        float timer = 4f;

        while (timer > 0)
        {
            timer -= 1;
            timerText.text = $"{(int)timer}";
            
            yield return new WaitForSecondsRealtime(1f);
        }

        countDown.SetActive(false);
        GameController.Instance.StartGame();
        yield break;
    }
}
