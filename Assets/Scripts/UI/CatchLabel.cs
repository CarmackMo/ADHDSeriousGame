using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchLabel : MonoBehaviour
{
    public Image progressBar;
    public Image catchIcon;
    public GameObject progress;

    public void Init()
    {
        catchIcon.gameObject.SetActive(true);
        progress.gameObject.SetActive(false);
    }

    public void UpdateCatchingProgress(float progressAmount, float threshold)
    {
        progressBar.fillAmount = progressAmount / threshold;
    }

    public void UpdateCatchIconVisiability(bool visibility)
    {
        catchIcon.gameObject.SetActive(visibility);
    }

    public void UpdateProgressVisiability(bool visibility)
    {
        progress.gameObject.SetActive(visibility);
    }
}
