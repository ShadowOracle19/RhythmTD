using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendController : MonoBehaviour
{
    // VARIABLES
    public Animator animator;
    public float spinSpeedWeight;

    void Start()
    {

    }

    void Update() 
    {
        spinSpeedWeight = Mathf.Clamp(spinSpeedWeight, 0.0f, 1.0f);
        animator.SetFloat("Spin", spinSpeedWeight);
    }
}
