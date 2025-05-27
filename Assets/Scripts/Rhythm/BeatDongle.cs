using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatDongle : MonoBehaviour
{
    float duration;
    int xReset;
    public float time;
    public float lerpTime;
    public bool startOnce = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!startOnce)
            return;

        if (time < duration)
        {
            time += Time.deltaTime;
            lerpTime = time / duration;

            gameObject.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(xReset, 0, lerpTime), 0);
        }
        else
        {
            time = 0;
            gameObject.GetComponent<RectTransform>().localPosition = new Vector3(xReset, 0, 0);
            startOnce = false;
        }
    }

    public void StartDongle(float _duration, int x)
    {
        duration = _duration;
        xReset = x;
        time = 0;
        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(xReset, 0, 0);
        startOnce = true;
    }

    public void ResetPosition(int x)
    {
        xReset = x;
        time = 0;
        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(xReset, 0, 0);
        startOnce = false;
    }
}
