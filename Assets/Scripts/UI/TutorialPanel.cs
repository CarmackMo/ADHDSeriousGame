using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialPanel : Singleton<TutorialPanel>
{
    public TextMeshProUGUI testText;

    public GameObject controlPanel;

    public ScriptableMask controlMask;
    

    protected override void Start()
    {
        base.Start();
        
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdateTestText(float directX)
    {
        testText.text = $"{directX}";
    }


    public void UpdateControlPanelVisibility(bool canVisit)
    {
        controlPanel.SetActive(canVisit);
    }

    public void UpdateControlMaskCallBack(Action callBack, bool isAdd)
    {
        if (isAdd)
            controlMask.AddCallBack(callBack);
        else
            controlMask.RemoveCallBack(callBack);
    }
}
