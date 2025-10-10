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
    public float rotationTargetX = 0.0f;
    public float rotationTargetY = 0.0f;
    public float rotationTargetZ = 0.0f;

    public GameObject recordOutline;
    public GameObject prevRecordOutline;
    
    public float ringRadius;
    public Vector3 ringCenter;

    public float ringRotationAngle;
    public float ringCurrentAngle;

    public Vector3 scaleChange; //Record change in size (negative)

    public List<GameObject> recordsList = new List<GameObject>();
    public int numberOfRecords;

    public int currentSelectionIndex;

    public TextMeshProUGUI currentSongTitle;
    public TextMeshProUGUI currentSongShadow;
    public TextMeshProUGUI currentSongTracksRecorded;
    public TextMeshProUGUI currentSongTracksShadow;
    
    void Start()
    {
        // Set the currently selected record to the first record in the list
        currentSelectionIndex = 0;

        // Get the total number of records in the menu
        numberOfRecords = recordsList.Count;

        // Get the center position of the ring of records
        ringCenter = this.transform.position;

        //
        ringRotationAngle = 360 / numberOfRecords;

        //
        rotationTarget = Quaternion.Euler(rotationTargetX, rotationTargetY, rotationTargetZ);

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

    public void RotateAllRecords(int movement)
    {
        rotationTargetZ += (ringRotationAngle * movement);
        
        rotationTarget = Quaternion.Euler(rotationTargetX, rotationTargetY, rotationTargetZ);
        
        //transform.Rotate(0.0f, 0.0f, this.transform.rotation.z + (ringRotationAngle * movement), Space.Self);
    }

    public void SelectNextRecord(string direction)
    {
        recordOutline = recordsList[currentSelectionIndex].transform.GetChild(0).gameObject;
        recordOutline.SetActive(false);
        
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

        recordOutline = recordsList[currentSelectionIndex].transform.GetChild(0).gameObject;
        recordOutline.SetActive(true);

        currentSongTitle.text = recordsList[currentSelectionIndex].GetComponent<RecordElement>().songTitle;
        currentSongShadow.text = recordsList[currentSelectionIndex].GetComponent<RecordElement>().songTitle;
        currentSongTracksRecorded.text = $"{recordsList[currentSelectionIndex].GetComponent<RecordElement>().songTracksRecorded}/12";
        currentSongTracksShadow.text = $"{recordsList[currentSelectionIndex].GetComponent<RecordElement>().songTracksRecorded}/12";
    }

    public void FlipCurrentRecord()
    {
        recordsList[currentSelectionIndex].GetComponent<RecordElement>().FlipRecord();
    }
}
