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

    #region Variables
    public int x1, x2;

    [Space(20)][Header("<b><size=15>Indicators<b><size=15>")]
    [Line(255,255,255)]
    public List<GameObject> leftIndicators = new List<GameObject>();
    public List<GameObject> rightIndicators = new List<GameObject>();

    [Space(20)][Header("<b><size=15>Time<b><size=15>")]
    [Line(255,255,255)]
    public float time;
    public float lerpTime;
    [Space(10)]
    int beat = 1;
    #endregion

    private void Start()
    {
        beat = 0;
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
