using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlightManager : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    // VARIABLES
    //public GameObject menuCursor;

    [Header("SFX")]
    public AudioClip buttonHighlightSfx;

    private Vector3 highlightScale = new Vector3(1.25f, 1.25f, 1.0f);

    // Start is called before the first frame update
    void Start()
    {
        //
    }

    public void OnSelect(BaseEventData eventData)
    {
        // Make button larger
        this.gameObject.transform.localScale = highlightScale;

        //play highlight sound
        SoundEffectsManager.instance.PlaySound(buttonHighlightSfx, this.gameObject.transform, 1.0f);

        //menuCursor.GetComponent<MenuCursorMovement>().SetActiveElement(this.gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // Make button smaller
        this.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        
        //menuCursor.GetComponent<MenuCursorMovement>().SetPreviousElement(this.gameObject);
    }
}