using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MenuCursorMovement : MonoBehaviour
{
    // VARIABLES

    // Cursor Corners
    public GameObject upperLeftCorner;
    public GameObject upperRightCorner;
    public GameObject lowerLeftCorner;
    public GameObject lowerRightCorner;

    // UI Elements
    public GameObject previousElement; // Currently Selected UI Element
    public GameObject targetElement; // Previously Selected UI Element
    public RectTransform targetTransform;

    // Target Element Dimensions
    public float elementWidth = 0.0f;
    public float elementHeight = 0.0f;

    // Previous Cursor Corner Positions
    public Vector3 upperLeftPrev;
    public Vector3 upperRightPrev;
    public Vector3 lowerLeftPrev;
    public Vector3 lowerRightPrev;

    // Target Cursor Corner Positions
    public Vector3 upperLeftTarget;
    public Vector3 upperRightTarget;
    public Vector3 lowerLeftTarget;
    public Vector3 lowerRightTarget;

    // Lerp Animation
    public float lerpSpeed = 2000.0f;
    public float lerpStart;
    public float lerpLength;
    
    // Start is called before the first frame update
    void Start()
    {
        // Sets the currently active element
        //targetElement = EventSystem.current.currentSelectedGameObject;

        // Set the "previous" element to the current target element 
        //previousElement = EventSystem.current.currentSelectedGameObject;
        
        // Sets cursor corner starting positions
        targetTransform = (RectTransform)targetElement.transform;

        elementWidth = targetTransform.sizeDelta.x;
        elementHeight = targetTransform.sizeDelta.y;
        
        upperLeftCorner.transform.position = new Vector3(targetElement.transform.position.x - (elementWidth/3), targetElement.transform.position.y + (elementHeight/3), 0);
        upperRightCorner.transform.position = new Vector3(targetElement.transform.position.x + (elementWidth/3), targetElement.transform.position.y + (elementHeight/3),0);
        lowerLeftCorner.transform.position = new Vector3(targetElement.transform.position.x - (elementWidth/3), targetElement.transform.position.y - (elementHeight/3), 0);
        lowerRightCorner.transform.position = new Vector3(targetElement.transform.position.x + (elementWidth/3), targetElement.transform.position.y - (elementHeight/3), 0);

        upperLeftPrev = upperLeftCorner.transform.position;
        upperRightPrev = upperRightCorner.transform.position;
        lowerLeftPrev = lowerLeftCorner.transform.position;
        lowerRightPrev = lowerRightCorner.transform.position;

        // Lerp
        lerpStart = Time.time;
        lerpLength = Vector3.Distance(previousElement.transform.position, targetElement.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // Lerp Animation
        float distanceLerped = (Time.time - lerpStart) * lerpSpeed;
        float lerpFraction = distanceLerped / lerpLength;
        
        UpdateCursorPos(lerpFraction);
    }

    void UpdateCursorPos(float lerpSegment)
    {
        upperLeftCorner.transform.position = Vector3.Lerp(upperLeftPrev, upperLeftTarget, lerpSegment);
        upperRightCorner.transform.position = Vector3.Lerp(upperRightPrev, upperRightTarget, lerpSegment);
        lowerLeftCorner.transform.position = Vector3.Lerp(lowerLeftPrev, lowerLeftTarget, lerpSegment);
        lowerRightCorner.transform.position = Vector3.Lerp(lowerRightPrev, lowerRightTarget, lerpSegment);
    }

    void SetTargetPos(GameObject targetObject)
    {
        targetTransform = (RectTransform)targetObject.transform;

        elementWidth = targetTransform.sizeDelta.x;
        elementHeight = targetTransform.sizeDelta.y;
        
        upperLeftTarget = new Vector3(targetObject.transform.position.x - (elementWidth/3), targetObject.transform.position.y + (elementHeight/3), 0);
        upperRightTarget = new Vector3(targetObject.transform.position.x + (elementWidth/3), targetObject.transform.position.y + (elementHeight/3),0);
        lowerLeftTarget = new Vector3(targetObject.transform.position.x - (elementWidth/3), targetObject.transform.position.y - (elementHeight/3), 0);
        lowerRightTarget = new Vector3(targetObject.transform.position.x + (elementWidth/3), targetObject.transform.position.y - (elementHeight/3), 0);
    }

    public void SetPreviousElement(GameObject lastSelectedElement) 
    {
        previousElement = lastSelectedElement;

        upperLeftPrev = upperLeftCorner.transform.position;
        upperRightPrev = upperRightCorner.transform.position;
        lowerLeftPrev = lowerLeftCorner.transform.position;
        lowerRightPrev = lowerRightCorner.transform.position;

        // Set new lerp start time
        lerpStart = Time.time;
    }

    public void SetActiveElement(GameObject currentlySelectedElement) 
    {
        targetElement = currentlySelectedElement;

        SetTargetPos(targetElement);

        lerpLength = Vector3.Distance(previousElement.transform.position, targetElement.transform.position);
    }
}
