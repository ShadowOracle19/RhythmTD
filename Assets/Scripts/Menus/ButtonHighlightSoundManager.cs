using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlightSoundManager : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject buttonHighlightAudio;

    AudioSource buttonHighlightAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        buttonHighlightAudioSource = buttonHighlightAudio.GetComponent<AudioSource>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        buttonHighlightAudioSource.Play();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //
    }
}
