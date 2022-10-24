using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePanel : Singleton<DamagePanel>
{
    void Start()
    {
        Hide();       
    }

     

    public void ShowHideEffect()
    {
        Show();
        Invoke(nameof(Hide), 0.25f);
    }


    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
