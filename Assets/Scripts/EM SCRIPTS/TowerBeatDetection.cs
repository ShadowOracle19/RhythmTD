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
    
    public List<float> inputs = new List<float>();
    private List<IGameObject> indicators = new List<GameObject>();
    public GameObject indicatorPrefab;

    public float tempSpawnTime = 0.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateIndicators()
    {
        foreach (float inputTime in inputs)
        {
            GameObject indicator = Instantiate(indicatorPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
            
            /*
            tempSpawnTime = inputTime - (scrollTime); //either need a way for circles to retroactively appear and at the right scale or towers need a delay after spawning
            GameObject newIndicatorObject = Instantiate(indicatorObject, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
            var newIndicator = new Indicator(newIndicatorObject, tempSpawnTime);
            indicators.Add(newIndicator);
            */
        }
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
