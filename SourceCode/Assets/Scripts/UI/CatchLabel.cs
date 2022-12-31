using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchLabel : MonoBehaviour
{
    public Image progressBar;
    public Image targetIcon;
    public Image selectIcon;
    public GameObject progress;

    public void Init()
    {
        targetIcon.gameObject.SetActive(true);
        selectIcon.gameObject.SetActive(false);
        progress.gameObject.SetActive(false);
        transform.SetAsFirstSibling();
    }

    public void UpdateCatchingProgress(float progressAmount, float threshold)
    {
        progressBar.fillAmount = progressAmount / threshold;
    }

    public void UpdateTargetIconVisiability(bool visibility)
    {
        targetIcon.gameObject.SetActive(visibility);
    }

    public void UpdateSelectIconVisiability(bool visibility)
    {
        selectIcon.gameObject.SetActive(visibility);
    }

    public void UpdateProgressVisiability(bool visibility)
    {
        progress.gameObject.SetActive(visibility);
    }
}
