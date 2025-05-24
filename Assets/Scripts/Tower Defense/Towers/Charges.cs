using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charges : MonoBehaviour
{
    public int resourceGain;

    public void initalizeCharge(int _resourceGain)
    {
        resourceGain = _resourceGain;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CombatManager.Instance.resourceNum += resourceGain;
        }
    }
}
