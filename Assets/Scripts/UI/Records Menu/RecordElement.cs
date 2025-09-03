using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordElement : MonoBehaviour
{
    public float smooth = 5.0f;
    public Quaternion rotationTarget;
    public float flipTargetRotation;

    public bool flipped;

    public string songTitle;
    public int songTracksRecorded;
    public int songRemixTracksRecorded;
    
    // Start is called before the first frame update
    void Start()
    {
        //rotationTarget = transform.localRotation;
        rotationTarget = Quaternion.Euler(transform.localRotation.x + 90 + 12.5f, transform.localRotation.y + flipTargetRotation + 12.5f, transform.localRotation.z);
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
        rotationTarget = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + rotationAngle + flipTargetRotation, transform.localRotation.z);
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
