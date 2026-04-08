using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    //Animation Manager Instance
    public static AnimationManager instance;
    void Awake()
    {
        instance = this;
    }
    
    //VARIABLES
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float CalculateAnimSpeed(Animator animator, float animBpm)
    {
        float animCrotchet = 60.0f / animBpm;

        float speedMultiplier = animCrotchet/ConductorV2.instance.crotchet;

        float targetAnimSpeed = animator.speed * speedMultiplier;

        return targetAnimSpeed;
    }

    public void SetAnimSpeed(Animator animator, float animBpm)
    {
        animator.speed = CalculateAnimSpeed(animator, animBpm);
    }

    /*
    public void SetCombatAnimationSpeed()
    {

    }
    */
}
