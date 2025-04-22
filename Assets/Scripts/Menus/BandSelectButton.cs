using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BandSelectButton : MonoBehaviour, ISelectHandler
{
    public GameObject bandScreen;
    public GameObject mapScreen;
    public AudioSource buttonSFX;
    public Animator bandAnimator;
    public Animator mapAnimator;
    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Band Select");
        bandScreen.SetActive(true);
        mapScreen.SetActive(false);
        buttonSFX.Play();
        bandAnimator.SetTrigger("Band Open");
        bandAnimator.ResetTrigger("Band Close");
        mapAnimator.SetTrigger("Map Close");
        mapAnimator.ResetTrigger("Map Open");
    }

}
