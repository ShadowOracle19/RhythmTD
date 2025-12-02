using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraTargetTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTargetTransform = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = cameraTargetTransform.position + new Vector3 (0.0f, 12.5f,-7.5f);
    }

    public void SetCameraTarget(Transform targetTransform) 
    {
        cameraTargetTransform = targetTransform;
    }
}
