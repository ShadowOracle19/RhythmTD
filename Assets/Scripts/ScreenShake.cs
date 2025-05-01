using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public Animator camAnim;
    public AudioSource screenShakeAudio;

    public void CamShake()
    {
        //camAnim.SetTrigger("Screen Shake");
        screenShakeAudio.Play();
    }
}
