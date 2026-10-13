using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputIndicator : MonoBehaviour
{
    // TO DO LIST //
    /* 
    - PFX for more impact?
    - Fix bug where indicators don't start scaling until next measure cycle after spawn
    - Make disappearance of indicators when moving off of towers more immediate
    */
    
    // VARIABLES
    [Header ("Parameters")]
    [Range(0.0f, 1.0f)]
    public float notePosition = 0.0f; // input position in the measure expressed as a percentage
    public Color defaultColor;
    public Color approachColor;
    public List<Color> hitColors = new List<Color>();   
    public float hitFreezeTime = 0.125f; // the duration the indicator remains frozen & visible after being hit
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
    
    [Header("")]
    public SpriteRenderer spriteRenderer;
    public GameObject parentTower;

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
        parentTower = this.transform.parent.gameObject;
        
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
        
        //NOTE: Towers without enemy sightlines weren't displaying indicators properly. If we only want some towers to display indicators in the presence of enemies we'll need additional variables.
        if (parentTower.GetComponent<Tower>().towerHover) //(parentTower.GetComponent<Tower>().towerHover && parentTower.GetComponent<Tower>().enemyInRange)
        {
            isActive = true;
            //Debug.Log("Tower is highlighted and enemy in range");
        }
        else
        {
            isActive = false;
            //Debug.Log("Tower not highlighted or enemy out of range :(");
        }

        if (!isActive)
        {
            spriteRenderer.enabled = false;
            parentTower.GetComponent<Tower>().inputPrompt.SetActive(false);
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
            if (!isActive)
            {
                StopCoroutine(ScaleIndicator(inputTime));
            }
            
            if (songProgress > inputTime && !isHit)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;
            }
            
            scalingProgress = (songProgress - spawnTime) / ((inputTime + scrollTime) - spawnTime); //NOTE: prompt size should match indicator at 50% scale progress
            colorProgress = ((songProgress - spawnTime) + colorOffset) / (inputTime - spawnTime);

            if (!isHit)
            {
                currentScale = Mathf.Lerp(startingScale, targetScale, scalingProgress); 
                this.transform.localScale = new Vector3(currentScale, currentScale, 1.0f);

                currentColor.a = Mathf.Lerp(defaultColor.a, approachColor.a, colorProgress);
                currentColor.r = Mathf.Lerp(defaultColor.r, approachColor.r, colorProgress);
                currentColor.g = Mathf.Lerp(defaultColor.g, approachColor.g, colorProgress);
                currentColor.b = Mathf.Lerp(defaultColor.b, approachColor.b, colorProgress);
            }
            
            spriteRenderer.color = currentColor;
            parentTower.GetComponent<Tower>().inputPrompt.SetActive(true);

            yield return null;
        }
        
        isHit = false;

        UpdateTargetTime();

        spriteRenderer.enabled = false;

        isScaling = false;
        StopCoroutine(ScaleIndicator(inputTime));
    }

    public void UpdateTargetTime()
    {
        measureCycleCount += 1;
        inputTargetTime = (measureLength * (measureCycleCount)) + measureTargetTime; 
        spawnTime = inputTargetTime - scrollTime;
    }
}
