using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RecordMenu : MonoBehaviour
{
    // VARIABLES
    public float smooth = 5.0f;
    public Quaternion rotationTarget;
    
    public float ringRadius;
    public Vector3 ringCenter;

    public float ringRotationAngle;
    public float ringCurrentAngle;

    public Vector3 scaleChange; //Record change in size (negative)

    public List<GameObject> recordsList = new List<GameObject>();
    public int numberOfRecords;


    public int currentSelectionIndex;

    public TextMeshProUGUI currentSongTitle;
    public TextMeshProUGUI currentSongTracksRecorded;
    
    void Start()
    {
        // Set the currently selected record to the first record in the list
        currentSelectionIndex = 0;

        // Get the total number of records in the menu
        numberOfRecords = recordsList.Count;

        // Get the center position of the ring of records
        ringCenter = this.transform.position;

        //
        ringRotationAngle = (360 / numberOfRecords);

        // For each record in the menu, set its starting position
        for (int i = 0; i < numberOfRecords; i++)
        {
            // CALCULATE & SET RECORD STARTING POSITION 
            // Calculate the angle of the next record in radians
            float angleToRecord = ((2 * Mathf.PI / numberOfRecords) * i);

            float recordX = ringRadius * Mathf.Sin(angleToRecord);
            float recordZ = ringRadius * Mathf.Cos(angleToRecord);

            Vector3 recordPosition = new Vector3(recordsList[i].transform.position.x + recordX, recordsList[i].transform.position.y, recordsList[i].transform.position.z - recordZ);

            // Set record starting position
            recordsList[i].transform.position = recordPosition;

            // SET RECORD STARTING SIZE
            if (i > 0)
            {
                recordsList[i].transform.localScale += scaleChange;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotationTarget,  Time.deltaTime * smooth);
    }

    void RotateAllRecords(int movement)
    {
        rotationTarget = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z + (ringRotationAngle * movement));
    }

    void SelectNextRecord(string direction)
    {
        if (direction == "right")
        {
            // Rotate record selector
            RotateAllRecords(-1);

            // Make the current record smaller
            recordsList[currentSelectionIndex].transform.localScale += scaleChange;

            // Update active record index
            if (currentSelectionIndex < (numberOfRecords - 1))
            {
                currentSelectionIndex += 1;
            }
            else
            {
                currentSelectionIndex = 0;
            }

            for (int i = 0; i < numberOfRecords; i++)
            {
                recordsList[i].GetComponent<RecordElement>().RotateRecord(ringRotationAngle);
            }      
        }
        else if (direction == "left")
        {
            // Rotate record selector
            RotateAllRecords(1);

            // Make the current record smaller
            recordsList[currentSelectionIndex].transform.localScale += scaleChange;

            // Update active record index
            if (currentSelectionIndex > 0)
            {
                currentSelectionIndex -= 1;
            }
            else
            {
                currentSelectionIndex = numberOfRecords - 1;
            }

            for (int i = 0; i < numberOfRecords; i++)
            {
                recordsList[i].GetComponent<RecordElement>().RotateRecord(ringRotationAngle * -1);
            }
        }
        
        // Make active record bigger
        recordsList[currentSelectionIndex].transform.localScale -= scaleChange;   
    }
}
