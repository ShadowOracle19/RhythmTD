using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordElement : MonoBehaviour
{
    public float smooth = 5.0f;
    public Quaternion rotationTarget;
    public float rotationTargetX = 0.0f;
    public float rotationTargetY = 180.0f;
    public float rotationTargetZ = 0.0f;
    public float flipTargetRotation;

    public bool flipped;

    public string songTitle;
    public int songTracksRecorded;
    public int songRemixTracksRecorded;
    
    // Start is called before the first frame update
    void Start()
    {
        //rotationTarget = transform.localRotation;
        rotationTarget = Quaternion.Euler(rotationTargetX, rotationTargetY, rotationTargetZ);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotationTarget,  Time.deltaTime * smooth);
    }

    // Used to make the record bigger/smaller when selected/deselected
    /*
    public void UpdateScale(Vector3 scaleDifference)
    {
        transform.localScale += scaleDifference;
    }
    */

    // Used to make the rotate the records to stay facing forward when rotating the record selection object
    public void RotateRecord(float rotationAngle)
    {
        rotationTargetX += rotationAngle;
        
        rotationTarget = Quaternion.Euler(rotationTargetX + flipTargetRotation, rotationTargetY, rotationTargetZ);
    }

    // Used to flip the record between its front and remix sides
    public void FlipRecord()
    {
        if (!flipped)
        {
            flipTargetRotation -= 180;
            flipped = true;
        }
        else
        {
            flipTargetRotation += 180;
            flipped = false;
        }
    }
}
