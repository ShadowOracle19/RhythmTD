using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncedAnimatorToTwoBeats : MonoBehaviour
{
    public Animator animator;

    public AnimatorStateInfo stateInfo;

    public int currentState;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        currentState = stateInfo.fullPathHash;
    }

    // Update is called once per frame
    void Update()
    {
        animator.Play(currentState, -1, ((ConductorV2.instance.songPositionInBeats - ConductorV2.instance.measureTrack * ConductorV2.instance.beatsPerLoop + 1) / 2));
        animator.speed = 0;
    }
}
