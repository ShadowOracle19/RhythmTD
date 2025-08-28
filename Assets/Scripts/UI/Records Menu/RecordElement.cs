using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordElement : MonoBehaviour
{
    public float smooth = 5.0f;
    public Quaternion rotationTarget;
    public float sideTargetRotation;
    public bool flipped;
    
    // Start is called before the first frame update
    void Start()
    {
        rotationTarget = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotationTarget,  Time.deltaTime * smooth);
    }

    // Used to make the record bigger/smaller when selected/deselected
    void UpdateScale(Vector3 scaleDifference)
    {
        transform.localScale += scaleDifference;
    }

    // Used to make the rotate the records to stay facing forward when rotating the record selection object
    void RotateRecord(float rotationAngle)
    {
        rotationTarget = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + rotationAngle + sideTargetRotation, transform.localRotation.z);
    }

    // Used to flip the record between its front and remix sides
    void FlipRecord()
    {
        if (!flipped)
        {
            sideTargetRotation -= 180;
            flipped = true;
        }
        else
        {
            sideTargetRotation += 180;
            flipped = false;
        }
    }
}
