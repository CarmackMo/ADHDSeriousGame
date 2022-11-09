using UnityEngine;
using System.Collections;

public class SmallSplash : MonoBehaviour 
{
    public GameObject splash;
    public ParticleSystem jetEffect;
    public ParticleSystem rippleEffect;
    public ParticleSystem sprayEffect;

    public AudioSource effectAudio;

    private float splashFlag = 0;


    public void PlaySplaceEffect()
    {
        jetEffect.Play();
        rippleEffect.Play();
        sprayEffect.Play();
        effectAudio.PlayOneShot(effectAudio.clip);
    }

}