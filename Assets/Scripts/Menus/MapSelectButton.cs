using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapSelectButton : MonoBehaviour, ISelectHandler
{
    public GameObject bandScreen;
    public GameObject mapScreen;
    public AudioSource buttonSFX;
    public Animator bandAnimator;
    public Animator mapAnimator;
    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Map Select");
        bandScreen.SetActive(false);
        mapScreen.SetActive(true);
        buttonSFX.Play();
        bandAnimator.SetTrigger("Band Close");
        bandAnimator.ResetTrigger("Band Open");
        mapAnimator.SetTrigger("Map Open");
        mapAnimator.ResetTrigger("Map Close");
    }
}
