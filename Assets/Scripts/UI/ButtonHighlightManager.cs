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
    public AudioClip buttonHighlightSfx; //sound that plays when the button is highlighted

    [Header("Animation")]
    public Animator buttonAnimator;

    [Header("Visual Changes")]
    public bool showCharacter;
    public GameObject characterObject;

    [Header("Selection Scale")]
    public Vector3 highlightScale = new Vector3(1.25f, 1.25f, 1.0f); //scale of the button when highlighted

    // Start is called before the first frame update
    void Start()
    {
        //
    }

    public void OnSelect(BaseEventData eventData)
    {
        //play select sound
        SoundEffectsManager.instance.PlaySound(buttonHighlightSfx, this.gameObject.transform, 1.0f);
        
        //play select animation
        buttonAnimator.SetBool("Highlighted", true);

        //change shown character (title screen only)
        if (showCharacter) 
        {
            characterObject.SetActive(true);
        }
        
        //set button scale to large
        //this.gameObject.transform.localScale = highlightScale;

        //set button to the object actively highlighted by the cursor
        //menuCursor.GetComponent<MenuCursorMovement>().SetActiveElement(this.gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //play deselect animation
        buttonAnimator.SetBool("Highlighted", false);

        //change shown character (title screen only)
        if (showCharacter) 
        {
            characterObject.SetActive(false);
        }
        
        //set button scale to default
        //this.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        
        //set button to the object last highlighted by the cursor
        //menuCursor.GetComponent<MenuCursorMovement>().SetPreviousElement(this.gameObject);
    }
}