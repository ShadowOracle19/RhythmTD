using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerBeatDetection : MonoBehaviour
{
    // TO DO LIST //
    /*
    - Create indicators based on inputs
    - Inputs should be in measure positions at first and then converted to timings
    - Next input needs to be updated when the previous one expires (based on threshold timing) or when it gets hit
    - When the input is hit it needs to update the corresponding input index and call the freeze method
    - Index should wraparound based on length
    - Should call the judgement method from the conductor or other script
    - input times need to be updated or calculated on input
    */
    
    [System.Serializable]
    public class Note
    {
        [Range(0.0f, 1.0f)]
        public float notePosition;
        public float noteTime;
        public float holdTime;
    }

    // VARIABLES //
    public List<Note> inputs = new List<Note>();
    [Range(0.0f, 1.0f)]
    public List<float> inputPositions = new List<float>();
    public List<float> inputTimes = new List<float>();

    public GameObject indicatorPrefab;
    public List<GameObject> indicators = new List<GameObject>();

    public int inputIndex; // the index of the closest input timing
    public int nextInputIndex; // the index of the next closest input timing
    public float timeAtInput; // song progress at the time of player input 
    
    public float songProgress = 0.0f; // progress of current song expressed in time

    public float measureLength = 0.0f; // length of 1 measure expressed in time
    public float inputTargetTime = 0.0f; // input timing in the song

    public float threshold = 0.0f; // PLACEHOLDER VARIABLE UNTIL I FIGURE OUT HOW I WANT TO HANDLE THRESHOLDS FOR TOWER ATTACK NOTES

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //
        measureLength = ConductorV2.instance.crotchet * 4;
        
        CalculateInputTimes();
        
        InstantiateIndicators();

        inputIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        songProgress = ConductorV2.instance.songPosition;

        inputTargetTime = ((measureLength * ConductorV2.instance.measureTrack) + inputTimes[inputIndex]); 

        // Update input tracking index when song progress exceeds threshold or when next note is closer //|| (inputTargetTime + inputTimes[] - songProgress) < (songProgress - inputTargetTime)
        if (songProgress > (inputTargetTime + threshold))
        {
            UpdateInputIndex();
        }
    }

    // Calculates input times as time from measure start
    public void CalculateInputTimes()
    {
        foreach (float inputPosition in inputTimes)
        {
            inputTimes.Add(inputPosition * measureLength);
        }
    }
    
    public void InstantiateIndicators()
    {
        foreach (float inputPosition in inputPositions)
        {
            GameObject newIndicator = Instantiate(indicatorPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
            newIndicator.GetComponent<InputIndicator>().notePosition = inputPosition;

            //newIndicator.GetComponent<InputIndicator>().SetIndicatorData();

            indicators.Add(newIndicator);
        }
    }

    // check player's time of input against the next expected input time
    public void CheckInputTiming(float inputTime, float inputTargetTime)
    {
        
    }

    public void UpdateInputIndex()
    {
        if (inputIndex == (inputTimes.Count - 1))
        {
            inputIndex = 0;
        }
        else
        {
            inputIndex += 1;
        }
    }
    
    /*
    indicators[inputIndex].GetComponent<InputIndicator>().StartCoroutine(FreezeIndicator());
    need to update index AFTER the indicator is updated
    */
}
