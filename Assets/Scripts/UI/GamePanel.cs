using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GamePanel : Singleton<GamePanel>
{
    public Button fishingBtn;

    public TextMeshProUGUI fishNumText;

    protected override void Awake()
    {
        base.Awake();

        fishingBtn.onClick.AddListener(OnClickFishingBtn);
    }


    public void OnClickFishingBtn()
    {
        FishGenerator.Instance.CatchFish();
    }

    public void UpdateFishNumText()
    {
        fishNumText.text = $"{Player.Instance.fishCatchNum}";
    }
}
