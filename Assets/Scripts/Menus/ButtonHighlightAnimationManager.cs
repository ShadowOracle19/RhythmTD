using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlightAnimationManager : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject bandSilhouette;

    Animator silhouetteAnimator;
    Image silhouetteSprite;

    public List<Sprite> bandSprites;
    public int spriteIndex;

    //public bool animationFlipped;

    void Start()
    {
        silhouetteAnimator = bandSilhouette.GetComponent<Animator>();
        silhouetteSprite = bandSilhouette.GetComponent<Image>();

        //Sprite[] sprites = Resources.LoadAll<Sprite>("Characters/Fayruz");

        //bandSprites.AddRange(sprites);

        spriteIndex = Random.Range(0 , bandSprites.Count);

        //animationFlipped = true;
    }

    public void OnSelect(BaseEventData eventData)
    {
        // Update sprite
        silhouetteSprite.sprite = bandSprites[spriteIndex];

        // Randomize next sprite
        spriteIndex = Random.Range(0 , bandSprites.Count); 

        // Flip next animation
        if (silhouetteAnimator.GetBool("Silhouette Flipped") == true)
        {
            silhouetteAnimator.SetBool("Silhouette Flipped", false);
            //animationFlipped = true;
        }
        else if (silhouetteAnimator.GetBool("Silhouette Flipped") == false)
        {
            silhouetteAnimator.SetBool("Silhouette Flipped", true);
            //animationFlipped = false;
        }

        // Update parameter for animation side (left or right)
        //silhouetteAnimator.SetBool("Silhouette Flipped", animationFlipped);

        // Play animation
        silhouetteAnimator.SetTrigger("Button Highlighted");
        
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //silhouetteAnimator.Play("TitleScreenSilhouetteIdle");
    }
}
