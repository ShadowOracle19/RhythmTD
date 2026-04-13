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
    public List<AnimatedElement> combatSceneAnimators = new List<AnimatedElement>();
    
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
        float animCrotchet = 60.0f / animBpm;
        Debug.Log("Animation Crotchet:" + animCrotchet);

        float speedMultiplier = animCrotchet/ConductorV2.instance.crotchet;
        Debug.Log("Crotchet:" + ConductorV2.instance.crotchet);

        float targetAnimSpeed = 1 * speedMultiplier;
        Debug.Log("Target Speed:" + targetAnimSpeed);

        return targetAnimSpeed;
    }

    public void SetAnimSpeed(Animator animator, float animBpm) //requires programmers atm to know the animation BPMs and hardcode them when calling this function which could be improved later
    {
        animator.speed = CalculateAnimSpeed(animator, animBpm);
    }

    public void SetCombatAnimSpeed()
    {
        foreach (Animator animator in towerLoadoutAnimators)
        {
            if (animator == null) {
                continue;
            }
            SetAnimSpeed(animator, 40);
        }
        
        foreach (AnimatedElement animElement in combatSceneAnimators)
        {
            if (animElement.animator == null) {
                continue;
            }
            
            Debug.Log(animElement.animator);
            SetAnimSpeed(animElement.animator, animElement.animationBpm);
        }
    }
}

[System.Serializable]
public class AnimatedElement
{
    public Animator animator;
    public int animationBpm;
}