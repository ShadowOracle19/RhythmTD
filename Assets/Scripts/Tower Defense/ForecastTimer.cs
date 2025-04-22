using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForecastTimer : MonoBehaviour
{
    //to be changed to using beats once it's been proven to work
    public float forecastEndsIn = 4;

    //don't touch this bit, this is fine
    public bool forecastingActive = false;

    // Start is called before the first frame update
    void Start()
    {
        forecastingActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (forecastingActive)
        { 
            if (forecastEndsIn > 0)
            {
                //to be changed to using beats once it's been proven to work
                forecastEndsIn -= Time.deltaTime;
            }

            else
            {
                Debug.Log("Forecasting ended, over");
                forecastEndsIn = 0;
                forecastingActive = false;
            }
        }
    }
}
