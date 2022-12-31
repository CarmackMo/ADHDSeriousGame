using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePanel : Singleton<DamagePanel>
{
    protected override void Start()
    {
        base.Start();

        Hide();       
    }

     

    public void ShowHitEffect()
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
