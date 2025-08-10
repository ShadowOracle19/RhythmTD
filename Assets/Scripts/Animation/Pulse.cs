using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public GameObject thisObject;
    public Vector3 defaultSize;
    public Vector3 pulseSize;

    // Update is called once per frame
    void Update()
    {
        thisObject.transform.localScale = Vector3.Lerp(thisObject.transform.localScale, defaultSize, Time.deltaTime * 5);
    }

    public void PulseAnim()
    {
        thisObject.transform.localScale = pulseSize;
    }
}
