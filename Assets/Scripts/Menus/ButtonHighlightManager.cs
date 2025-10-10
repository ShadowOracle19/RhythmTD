using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlightManager : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    // VARIABLES
    //public GameObject menuCursor;

    public GameObject buttonHighlightAudio;
    AudioSource buttonHighlightAudioSource;

    public Vector3 highlightScale = new Vector3(0.25f, 0.25f, 0.0f); //change in scale

    // Start is called before the first frame update
    void Start()
    {
        //buttonHighlightAudioSource = buttonHighlightAudio.GetComponent<AudioSource>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameManager.Instance.buttonHighlightSFX.Play();

        // Make button larger
        this.gameObject.transform.localScale += highlightScale;

        //menuCursor.GetComponent<MenuCursorMovement>().SetActiveElement(this.gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // Make button smaller
        this.gameObject.transform.localScale -= highlightScale;
        
        //menuCursor.GetComponent<MenuCursorMovement>().SetPreviousElement(this.gameObject);
    }
}