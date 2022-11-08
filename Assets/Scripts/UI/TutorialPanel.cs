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
    public GameObject sharkPanel;
    public GameObject aimIntroPanel;
    public GameObject aimIntroExPanel;
    public GameObject completePanel;

    public ScriptableMask controlMask;
    public ScriptableMask sharkMask;
    public ScriptableMask aimIntroMask;
    public ScriptableMask aimIntroExMask;
    public ScriptableMask completeMask;
    

    public enum PanelType { CONTROL, SHARK, AIM_INTRO, AIM_INTRO_EX, COMPLETE}

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
                sharkPanel.SetActive(canVisit);
                break;
            case PanelType.AIM_INTRO:
                aimIntroPanel.SetActive(canVisit);
                break;
            case PanelType.AIM_INTRO_EX:
                aimIntroExPanel.SetActive(canVisit);
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
                if (isAdd)
                    sharkMask.AddCallBack(callBack);
                else
                    sharkMask.RemoveCallBack(callBack);
                break;
            }
            case PanelType.AIM_INTRO:
            {
                if (isAdd)
                    aimIntroMask.AddCallBack(ShowAimIntroEx);
                else
                    aimIntroMask.RemoveCallBack(ShowAimIntroEx);
                break;
            }
            case PanelType.AIM_INTRO_EX:
            {
                if (isAdd)
                    aimIntroMask.AddCallBack(callBack);
                else
                    aimIntroMask.RemoveCallBack(callBack);
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

    public void ShowAimIntroEx()
    {
        UpdatePanelVisibility(PanelType.AIM_INTRO, false);
        UpdatePanelVisibility(PanelType.AIM_INTRO_EX, true);
    }

}
