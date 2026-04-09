using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    //Animation Manager Instance
    public static AnimationManager instance;
    void Awake()
    {
        instance = this;
    }
    
    //VARIABLES
    public List<Animator> towerLoadoutAnimators = new List<Animator>();
    
    /*
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */

    public float CalculateAnimSpeed(Animator animator, float animBpm)
    {
        Debug.Log(ConductorV2.instance.crotchet);

        float animCrotchet = 60.0f / animBpm;

        float speedMultiplier = animCrotchet/ConductorV2.instance.crotchet;

        float targetAnimSpeed = animator.speed * speedMultiplier;

        return targetAnimSpeed;
    }

    public void SetAnimSpeed(Animator animator, float animBpm) //requires programmers atm to know the animation BPMs and hardcode them when calling this function which could be improved later
    {
        animator.speed = CalculateAnimSpeed(animator, animBpm);
    }

    public void SetCombatAnimSpeed() //rn this isn't flexible enough to account for differing animation speeds across UI elements
    {
        foreach (Animator animator in towerLoadoutAnimators)
        {
            if (animator == null) {
                continue;
            }
            SetAnimSpeed(animator, 40);
        }
    }
}
