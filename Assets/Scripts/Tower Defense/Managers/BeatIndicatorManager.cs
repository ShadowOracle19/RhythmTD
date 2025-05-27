using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatIndicatorManager : MonoBehaviour
{
    #region dont touch this
    private static BeatIndicatorManager _instance;
    public static BeatIndicatorManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("BeatIndicatorManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public int x1, x2;

    public List<GameObject> leftIndicators = new List<GameObject>();
    public List<GameObject> rightIndicators = new List<GameObject>();

    public float time;
    public float lerpTime;

    int beat = 1;

    private void Start()
    {
        beat = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Beat()
    {
        if(beat == 4)
        {
            beat = 0;
        }
        float duration = leftIndicators.Count * (ConductorV2.instance.crotchet);
        beat += 1;

        leftIndicators[beat - 1].GetComponent<BeatDongle>().StartDongle(duration, x1);


        rightIndicators[beat - 1].GetComponent<BeatDongle>().StartDongle(duration, x2);
    }

    public void ResetBeatIndicator()
    {
        beat = 0;

        foreach (GameObject left in leftIndicators)
        {
            left.GetComponent<BeatDongle>().ResetPosition(x1);
        }
        foreach (GameObject right in rightIndicators)
        {
            right.GetComponent<BeatDongle>().ResetPosition(x2);
        }
    }
}
