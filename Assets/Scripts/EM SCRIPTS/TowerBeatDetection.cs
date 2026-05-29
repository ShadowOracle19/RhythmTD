using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerBeatDetection : MonoBehaviour
{
    // VARIABLES //

    public int nextInputIndex;
    public float timeAtInput;
    
    private List<float> inputs = new List<float>();
    private List<Indicator> indicators = new List<Indicator>();

    public GameObject indicatorObject;
    public float tempSpawnTime = 0.0f;

    public float scrollSpeed = 1.0f;
    public float scrollTime = 1.0f;

    //Lerp
    public float indicatorStartingScale = 1.0f;
    public float indicatorTargetScale = 0.0f;
    public float scaleProgress = 0.0f;
    
    public class Indicator : TowerBeatDetection
    {
        public GameObject indicator { get; set; }
        public float indicatorSpawnTime { get; set; }

        public Indicator(GameObject indicator, float indicatorSpawnTime)
        {
            this.indicator = indicator;
            this.indicatorSpawnTime = indicatorSpawnTime;
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scrollTime = scrollTime * scrollSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateIndicators()
    {
        foreach (float inputTime in inputs)
        {
            tempSpawnTime = inputTime - (scrollTime); //either need a way for circles to retroactively appear and at the right scale or towers need a delay after spawning

            GameObject newIndicatorObject = Instantiate(indicatorObject, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);

            var newIndicator = new Indicator(newIndicatorObject, tempSpawnTime);

            indicators.Add(newIndicator);
        }
    }

    void ScaleIndicator()
    {
        float indicatorScale = Mathf.Lerp(indicatorStartingScale, indicatorTargetScale, scaleProgress);

        //INDICATORVARIABLE.transform.scale = new Vector3(indicatorScale, indicatorScale, 1.0f);
    }

    // check player's time of input against the next expected input time
    public void CheckInputTiming(float inputTime, float inputTargetTime)
    {
        /*
        if (inputTime inputTargetTime)
        {

        }
        */
    }
    
}
