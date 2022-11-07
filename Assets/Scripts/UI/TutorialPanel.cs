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
    public GameObject completePanel;

    public ScriptableMask controlMask;
    public ScriptableMask completeMask;
    

    public enum PanelType { CONTROL, SHARK, COMPLETE}

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


    public void UpdatePanelVisibility(PanelType type, bool canVisit)
    {
        switch (type)
        {
            case PanelType.CONTROL:
                controlPanel.SetActive(canVisit);
                break;
            case PanelType.SHARK:
                break;
            case PanelType.COMPLETE:
                completePanel.SetActive(canVisit);
                break;
            default:
                controlPanel.SetActive(canVisit);
                break;
        }
    }

    public void UpdateMaskCallBack(PanelType type, bool isAdd, Action callBack)
    {
        switch (type)
        {
            case PanelType.CONTROL:
            {
                if (isAdd)
                    controlMask.AddCallBack(callBack);
                else
                    controlMask.RemoveCallBack(callBack);
                break;
            }
            case PanelType.SHARK:
            {
                break;
            }
            case PanelType.COMPLETE:
            {
                if (isAdd)
                    completeMask.AddCallBack(callBack);
                else
                    completeMask.RemoveCallBack(callBack);
                break;
            }
            default:
            {
                if (isAdd)
                    controlMask.AddCallBack(callBack);
                else
                    controlMask.RemoveCallBack(callBack);
                break;
            }
        }
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
