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
    public GameObject aimStartPanel;
    public GameObject aimEndPanel;
    public GameObject catchIntroPanel;
    public GameObject catchRhythmPanel;
    public GameObject catchProgressPanel;
    public GameObject catchRetryPanel;
    public GameObject completePanel;

    public ScriptableMask controlMask;
    public ScriptableMask sharkMask;
    public ScriptableMask aimIntroMask;
    public ScriptableMask aimStartMask;
    public ScriptableMask aimEndMask;
    public ScriptableMask catchIntroMask;
    public ScriptableMask catchRhythmMash;
    public ScriptableMask catchProgressMask;
    public ScriptableMask catchRetryMask;
    public ScriptableMask completeMask;
    

    public enum PanelType { 
        CONTROL, 
        SHARK, 
        AIM_INTRO, AIM_START, AIM_END, 
        CATCH_INTRO, CATCH_RHYTHM, CATCH_PROGRESS, CATCH_RETRY,
        COMPLETE}

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
            case PanelType.AIM_START:
                aimStartPanel.SetActive(canVisit);
                break;
            case PanelType.AIM_END:
                aimEndPanel.SetActive(canVisit);
                break;
            case PanelType.CATCH_INTRO:
                catchIntroPanel.SetActive(canVisit);
                break;
            case PanelType.CATCH_RHYTHM:
                catchRhythmPanel.SetActive(canVisit);
                break;
            case PanelType.CATCH_PROGRESS:
                catchProgressPanel.SetActive(canVisit);
                break;
            case PanelType.CATCH_RETRY:
                catchRetryPanel.SetActive(canVisit);
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
                    aimIntroMask.AddCallBack(ShowAimStart);
                else
                    aimIntroMask.RemoveCallBack(ShowAimStart);
                break;
            }
            case PanelType.AIM_START:
            {
                if (isAdd)
                    aimStartMask.AddCallBack(callBack);
                else
                    aimStartMask.RemoveCallBack(callBack);
                break;
            }
            case PanelType.AIM_END:
            {
                if (isAdd)
                    aimEndMask.AddCallBack(callBack);
                else
                    aimEndMask.RemoveCallBack(callBack);
                break;
            }
            case PanelType.CATCH_INTRO:
            {
                if (isAdd)
                    catchIntroMask.AddCallBack(ShowCatchRhythem);
                else
                    catchIntroMask.RemoveCallBack(ShowCatchRhythem);
                break;
            }
            case PanelType.CATCH_RHYTHM:
            {
                if (isAdd)
                    catchRhythmMash.AddCallBack(callBack);
                else
                    catchRhythmMash.RemoveCallBack(callBack);
                break;
            }
            case PanelType.CATCH_PROGRESS:
            {
                if (isAdd)
                    catchProgressMask.AddCallBack(callBack);
                else
                    catchProgressMask.RemoveCallBack(callBack);
                break;
            }
            case PanelType.CATCH_RETRY:
            {
                if (isAdd)
                    catchRetryMask.AddCallBack(callBack);
                else
                    catchRetryMask.RemoveCallBack(callBack);
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

    public void ShowAimStart()
    {
        UpdatePanelVisibility(PanelType.AIM_INTRO, false);
        UpdatePanelVisibility(PanelType.AIM_START, true);
        UpdateMaskCallBack(PanelType.AIM_START, true, ExitAimStart);
    }

    public void ExitAimStart()
    {
        UpdateMaskCallBack (PanelType.AIM_INTRO, false, ShowAimStart);
        UpdatePanelVisibility(PanelType.AIM_START, false);
        TutorialController.Instance.UpdateTutorialState(TutorialController.State.AIM_ONGO);
    }

    public void ShowCatchRhythem()
    {
        UpdatePanelVisibility(PanelType.CATCH_INTRO, false);
        UpdatePanelVisibility(PanelType.CATCH_RHYTHM, true);
        UpdateMaskCallBack(PanelType.CATCH_RHYTHM, true, ShowCatchProgress);
    }

    public void ShowCatchProgress()
    {
        UpdatePanelVisibility(PanelType.CATCH_RHYTHM, false);
        UpdatePanelVisibility(PanelType.CATCH_PROGRESS, true);
        UpdateMaskCallBack(PanelType.CATCH_PROGRESS, true, 
            () => TutorialController.Instance.UpdateTutorialState(TutorialController.State.CATCH_ONGO));

    }
}
