using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputIndicator : MonoBehaviour
{
    // TO DO LIST //
    /*
    - Adjust indicator scaling time frame to extend beyond the target input time 
    - When indicator's corresponding input window is hit, indicator should freeze in place temporarily and change colour
    -
    */
    
    // VARIABLES
    [Header ("Parameters")]
    [Range(0.0f, 1.0f)]
    public float notePosition = 0.0f; // input position in the measure expressed as a percentage
    public Color defaultColor;
    public Color hitColor;    
    public float hitFreezeTime = 0.1f; // the duration the indicator remains frozen & visible after being hit
    
    [Header ("Measurement Variables (DO NOT TOUCH)")]
    public float measureTargetTime = 0.0f; // input timing in the measure
    public float inputTargetTime = 1.0f; // input timing in the song
    public float spawnTime = 0.0f; // spawn timing in the song
    public float measureLength = 0.0f; // length of 1 measure expressed in time
    public float songProgress = 0.0f; // progress of current song expressed in time
    public SpriteRenderer spriteRenderer;
    public Transform towerTransform;

    [Header ("State (DO NOT TOUCH)")]
    public bool isActive = true;
    public bool isHeld = false;
    public bool isHit = false;
    public bool isScaling = false;
    
    [Header ("Lerp Controls")]
    public float scrollSpeed = 1.0f;
    public float scrollTime = 1.0f;
    public float startingScale = 1.0f;
    public float targetScale = 0.75f;

    [Header ("Lerp Variables (DO NOT TOUCH)")]
    public float currentScale = 1.0f;
    public float scalingProgress = 0.0f;

    [Header("Testing Resources")]
    public AudioSource testSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get reference to the indicator sprite renderer so sprite visibility can be updated
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = defaultColor;

        //
        measureLength = ConductorV2.instance.crotchet * 4;
        measureTargetTime = notePosition * measureLength;

        //
        inputTargetTime = (ConductorV2.instance.crotchet * (ConductorV2.instance.measureTrack * 4)) + measureTargetTime;
        spawnTime = inputTargetTime - (scrollTime * scrollSpeed);

        // align rotation with parent tower
        this.gameObject.transform.rotation = towerTransform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        songProgress = ConductorV2.instance.songPosition;
        //Debug.Log(songProgress);
        
        if (songProgress >= (inputTargetTime + hitFreezeTime))
        {
            UpdateTargetTime();
        }
        
        if (!isActive)
        {
            spriteRenderer.enabled = false;
            return;
        }
        
        //at or past spawn time, before or at target time, not hit, and not scaling down
        if (songProgress >= spawnTime && songProgress <= inputTargetTime && !isHit && !isScaling) 
        {
            spriteRenderer.enabled = true;
            StartCoroutine(ScaleIndicator());
        }
        else if (songProgress < spawnTime || songProgress > inputTargetTime && !isHit)
        {
            spriteRenderer.enabled = false;
        }
    }

    public IEnumerator ScaleIndicator() // turn into coroutine
    {
        isScaling = true;

        this.transform.localScale = new Vector3(startingScale, startingScale, 1.0f); // reset scale
        spriteRenderer.color = defaultColor; // reset color

        scalingProgress = 0.0f; // reset scaling progress

        while (scalingProgress <= 1.0)
        {
            //scalingProgress = (songProgress - (scrollTime * scrollSpeed)) / (inputTargetTime - (scrollTime * scrollSpeed));

            scalingProgress = (songProgress - spawnTime) / (inputTargetTime - spawnTime);

            currentScale = Mathf.Lerp(startingScale, targetScale, scalingProgress); // right now it only scales to the target time
            this.transform.localScale = new Vector3(currentScale, currentScale, 1.0f);

            yield return null;
        }
        
        // TEST SOUND //
        testSound.Play();

        UpdateTargetTime();

        isScaling = false;

        StopCoroutine(ScaleIndicator());
    }

    public IEnumerator FreezeIndicator()
    {
        isHit = true;

        // stop scaling
        StopCoroutine(ScaleIndicator());

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
        inputTargetTime = (ConductorV2.instance.crotchet * ((ConductorV2.instance.measureTrack + 1) * 4)) + measureTargetTime; 
        spawnTime = inputTargetTime - (scrollTime * scrollSpeed);
    }

}
