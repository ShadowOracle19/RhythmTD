using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlightManager : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    // VARIABLES
    public GameObject menuCursor;
    public GameObject buttonHighlightAudio;
    AudioSource buttonHighlightAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        //buttonHighlightAudioSource = buttonHighlightAudio.GetComponent<AudioSource>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameManager.Instance.buttonHighlightSFX.Play();

        //menuCursor.GetComponent<MenuCursorMovement>().SetActiveElement(this.gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //menuCursor.GetComponent<MenuCursorMovement>().SetPreviousElement(this.gameObject);
    }
}