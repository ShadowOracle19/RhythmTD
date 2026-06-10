using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputIndicator : MonoBehaviour
{
    // TO DO LIST //
    /*
    - When indicator's corresponding input window is hit, indicator should freeze in place temporarily and change colour 
    - Enable / disable sprite renderer when indicators are done scaling or when tower isn't selected
    - Have colour fade out past input target timing as well
    - Different colours for different judgements?
    - Fix bug where indicators don't start scaling until next measure cycle after spawn
    */
    
    // VARIABLES
    [Header ("Parameters")]
    [Range(0.0f, 1.0f)]
    public float notePosition = 0.0f; // input position in the measure expressed as a percentage
    public Color defaultColor;
    public Color approachColor;
    public Color hitColor;    
    public float hitFreezeTime = 0.25f; // the duration the indicator remains frozen & visible after being hit
    public float defaultScrollTime = 1.0f;
    public float scrollSpeed = 1.0f;
    public float startingScale = 1.0f;
    public float targetScale = 0.0f;
    
    [Header ("Measurement Variables (DO NOT TOUCH)")]
    public int measureCycleCount;
    public float measureTargetTime = 0.0f; // input timing in the measure
    public float inputTargetTime = 1.0f; // input timing in the song
    public float spawnTime = 0.0f; // spawn timing in the song
    public float measureLength = 0.0f; // length of 1 measure expressed in time
    public float songProgress = 0.0f; // progress of current song expressed in time
    public SpriteRenderer spriteRenderer;
    //public Transform towerTransform;

    [Header ("State (DO NOT TOUCH)")]
    public bool isActive = true;
    public bool isHeld = false;
    public bool isHit = false;
    public bool isScaling = false;

    [Header ("Lerp Variables (DO NOT TOUCH)")]
    public float scrollTime = 1.0f;
    public float currentScale = 1.0f;
    public float scalingProgress = 0.0f;
    public Color currentColor;
    public float colorProgress = 0.0f;
    public float colorOffset = 0.0f;

    [Header("Testing Resources")]
    public AudioSource testSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get reference to the indicator sprite renderer so sprite visibility can be updated
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = defaultColor;
        currentColor = spriteRenderer.color;

        //
        measureLength = ConductorV2.instance.crotchet * 4;
        measureTargetTime = notePosition * measureLength;

        //
        measureCycleCount = ConductorV2.instance.measureTrack;
        inputTargetTime = (measureLength * measureCycleCount) + measureTargetTime;
        scrollTime = defaultScrollTime / scrollSpeed;
        spawnTime = inputTargetTime - scrollTime;

        //
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        songProgress = ConductorV2.instance.songPosition;
        
        if (songProgress > (inputTargetTime + scrollTime) && !isScaling)
        {
            UpdateTargetTime();
        }

        if (!isActive)
        {
            spriteRenderer.enabled = false;
            Debug.Log("Tower Not Highlighted");
            return;
        }
        
        //at or past spawn time, before or at target time, not hit, and not scaling down
        if ((songProgress >= spawnTime) && (songProgress <= inputTargetTime) && !isHit && !isScaling) 
        {
            spriteRenderer.enabled = true;
            StartCoroutine(ScaleIndicator(inputTargetTime));
        }
    }

    public IEnumerator ScaleIndicator(float inputTime) // turn into coroutine
    {
        isScaling = true;

        spriteRenderer.enabled = true;

        this.transform.localScale = new Vector3(startingScale, startingScale, 1.0f); // reset scale
        spriteRenderer.color = defaultColor; // reset color

        scalingProgress = 0.0f; // reset scaling progress

        while (scalingProgress <= 1.0)
        {
            if (songProgress > inputTime)
            {
                spriteRenderer.enabled = false;
            }
            
            scalingProgress = (songProgress - spawnTime) / ((inputTime + scrollTime) - spawnTime); //NOTE: prompt size should match indicator at 50% scale progress
            colorProgress = ((songProgress - spawnTime) + colorOffset) / (inputTime - spawnTime);

            currentScale = Mathf.Lerp(startingScale, targetScale, scalingProgress); 
            this.transform.localScale = new Vector3(currentScale, currentScale, 1.0f);

            currentColor.a = Mathf.Lerp(defaultColor.a, approachColor.a, colorProgress);
            currentColor.r = Mathf.Lerp(defaultColor.r, approachColor.r, colorProgress);
            currentColor.g = Mathf.Lerp(defaultColor.g, approachColor.g, colorProgress);
            currentColor.b = Mathf.Lerp(defaultColor.b, approachColor.b, colorProgress);
            
            spriteRenderer.color = currentColor;

            yield return null;
        }
        
        // TEST SOUND //
        //testSound.Play();

        UpdateTargetTime();

        spriteRenderer.enabled = false;

        isScaling = false;
        StopCoroutine(ScaleIndicator(inputTime));
    }

    public IEnumerator FreezeIndicator()
    {
        isHit = true;
        spriteRenderer.enabled = true;

        // stop scaling
        StopCoroutine(ScaleIndicator(inputTargetTime));

        UpdateTargetTime();

        spriteRenderer.color = hitColor;
        
        while (isHit)
        {
            yield return new WaitForSecondsRealtime(hitFreezeTime);
            isHit = false;
        }

        StopCoroutine(FreezeIndicator());
    }

    public void UpdateTargetTime()
    {
        measureCycleCount += 1;
        inputTargetTime = (measureLength * (measureCycleCount)) + measureTargetTime; 
        spawnTime = inputTargetTime - scrollTime;
    }

    /*
    public void SetIndicatorData()
    {

    }
    */

}
